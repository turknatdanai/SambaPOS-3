﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using Samba.Domain.Models.Accounts;
using Samba.Domain.Models.Automation;
using Samba.Domain.Models.Inventory;
using Samba.Domain.Models.Menus;
using Samba.Domain.Models.Resources;
using Samba.Domain.Models.Settings;
using Samba.Domain.Models.Tasks;
using Samba.Domain.Models.Tickets;
using Samba.Domain.Models.Users;
using Samba.Infrastructure.Data;
using Samba.Localization.Properties;
using Samba.Persistance.Data;

namespace Samba.Persistance.DaoClasses.Implementations
{
    [Export(typeof(ICacheDao))]
    class CacheDao : ICacheDao
    {
        [ImportingConstructor]
        public CacheDao()
        {
            ValidatorRegistry.RegisterDeleteValidator<Department>(
                x => Dao.Exists<UserRole>(y => y.DepartmentId == x.Id), Resources.Department, Resources.UserRole);
        }

        public void ResetCache()
        {
            Dao.ResetCache();
        }

        public IEnumerable<MenuItem> GetMenuItems()
        {
            return Dao.Query<MenuItem>(x => x.Portions.Select(y => y.Prices));
        }

        public IEnumerable<ProductTimer> GetProductTimers()
        {
            return Dao.Query<ProductTimer>(x => x.ProductTimerMaps);
        }

        public IEnumerable<OrderTagGroup> GetOrderTagGroups()
        {
            return Dao.Query<OrderTagGroup>(x => x.OrderTags, x => x.OrderTagMaps);
        }

        public IEnumerable<AccountTransactionType> GetAccountTransactionTypes()
        {
            return Dao.Query<AccountTransactionType>();
        }

        public IEnumerable<Resource> GetResources(int resourceTypeId)
        {
            return Dao.Query<Resource>(x => x.ResourceTypeId == resourceTypeId);
        }

        public IEnumerable<ResourceType> GetResourceTypes()
        {
            return Dao.Query<ResourceType>(x => x.ResoruceCustomFields).OrderBy(x => x.SortOrder);
        }

        public IEnumerable<AccountType> GetAccountTypes()
        {
            return Dao.Query<AccountType>().OrderBy(x => x.SortOrder);
        }

        public IEnumerable<AccountTransactionDocumentType> GetAccountTransactionDocumentTypes()
        {
            return Dao.Query<AccountTransactionDocumentType>(x => x.TransactionTypes, x => x.AccountTransactionDocumentTypeMaps, x => x.AccountTransactionDocumentAccountMaps);
        }

        public IEnumerable<State> GetStates()
        {
            return Dao.Query<State>();
        }

        public IEnumerable<PrintJob> GetPrintJobs()
        {
            return Dao.Query<PrintJob>(x => x.PrinterMaps);
        }

        public IEnumerable<PaymentType> GetPaymentTypes()
        {
            return Dao.Query<PaymentType>(x => x.PaymentTypeMaps, x => x.AccountTransactionType, x => x.Account);
        }

        public IEnumerable<ChangePaymentType> GetChangePaymentTypes()
        {
            return Dao.Query<ChangePaymentType>(x => x.ChangePaymentTypeMaps, x => x.AccountTransactionType, x => x.Account);
        }

        public IEnumerable<TicketTagGroup> GetTicketTagGroups()
        {
            return Dao.Query<TicketTagGroup>(x => x.TicketTags, x => x.TicketTagMaps);
        }

        public IEnumerable<AutomationCommand> GetAutomationCommands()
        {
            return Dao.Query<AutomationCommand>(x => x.AutomationCommandMaps);
        }

        public IEnumerable<CalculationSelector> GetCalculationSelectors()
        {
            return Dao.Query<CalculationSelector>(x => x.CalculationSelectorMaps, x => x.CalculationTypes.Select(y => y.AccountTransactionType));
        }

        public IEnumerable<AccountScreen> GetAccountScreens()
        {
            return Dao.Query<AccountScreen>(x => x.AccountScreenValues);
        }

        public IEnumerable<ScreenMenu> GetScreenMenus()
        {
            return Dao.Query<ScreenMenu>(
                    x => x.Categories,
                    x => x.Categories.Select(z => z.ScreenMenuItems.Select(w => w.OrderTagTemplate.OrderTagTemplateValues.Select(x1 => x1.OrderTag))),
                    x => x.Categories.Select(z => z.ScreenMenuItems.Select(w => w.OrderTagTemplate.OrderTagTemplateValues.Select(x1 => x1.OrderTagGroup))));
        }

        public IEnumerable<ResourceScreen> GetResourceScreens()
        {
            return Dao.Query<ResourceScreen>(x => x.ResourceScreenMaps, x => x.ScreenItems, x => x.Widgets);
        }

        public IEnumerable<TicketType> GetTicketTypes()
        {
            return Dao.Query<TicketType>(x => x.SaleTransactionType, x => x.OrderNumerator, x => x.TicketNumerator, x => x.ResourceTypeAssignments);
        }

        public IEnumerable<TaskType> GetTaskTypes()
        {
            return Dao.Query<TaskType>(x => x.ResourceTypes);
        }

        public IEnumerable<ForeignCurrency> GetForeignCurrencies()
        {
            return Dao.Query<ForeignCurrency>();
        }

        public IEnumerable<Department> GetDepartments()
        {
            return Dao.Query<Department>().OrderBy(x => x.SortOrder).ThenBy(x => x.Id);
        }

        public Resource GetResourceByName(int resourceTypeId, string resourceName)
        {
            return Dao.SingleWithCache<Resource>(x => x.Name == resourceName && x.ResourceTypeId == resourceTypeId);
        }

        public IEnumerable<TaxTemplate> GetTaxTemplates()
        {
            return Dao.Query<TaxTemplate>(x => x.AccountTransactionType, x => x.TaxTemplateMaps);
        }

        public IEnumerable<Warehouse> GetWarehouses()
        {
            return Dao.Query<Warehouse>().OrderBy(x => x.SortOrder).ThenBy(x => x.Id);
        }

        public IEnumerable<WarehouseType> GetWarehouseTypes()
        {
            return Dao.Query<WarehouseType>();
        }
    }
}
