

// This file was automatically generated.
// Do not make changes directly to this file - edit the template instead.
// 
// The following connection settings were used to generate this file
// 
//     Configuration file:     "ANDP.Domain.Test\App.config"
//     Connection String Name: "ANDP_Entities"
//     Connection String:      "Data Source=sl5ivsvwoq.database.windows.net,1433;initial catalog=QssAndp;persist security info=False;user id=andpuser@sl5ivsvwoq;password=**zapped**;"

// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier

using System;
using System.Data.Entity.Core.Objects;
using Common.Lib.EntityProvider;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
//using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.DatabaseGeneratedOption;

namespace ANDP.Lib.Data.Repositories.Order
{
    // ************************************************************************
    // Unit of work
    public interface IANDP_Order_Entities : IDisposable
    {
        IDbSet<Account> Accounts { get; set; } // Account
        IDbSet<AccountX> AccountXes { get; set; } // AccountX
        IDbSet<CompaniesX> CompaniesXes { get; set; } // CompaniesX
        IDbSet<Company> Companies { get; set; } // Companies
        IDbSet<Equipment> Equipments { get; set; } // Equipment
        IDbSet<Item> Items { get; set; } // Item
        IDbSet<ItemProvisioningResultsVw> ItemProvisioningResultsVws { get; set; } // ItemProvisioningResults_VW
        IDbSet<ItemX> ItemXes { get; set; } // ItemX
        IDbSet<Order> Orders { get; set; } // Order
        IDbSet<OrderX> OrderXes { get; set; } // OrderX
        IDbSet<Service> Services { get; set; } // Service
        IDbSet<ServiceX> ServiceXes { get; set; } // ServiceX

		string ConnectionString { get; }

		/// <summary>
		/// Saves the changes and removes specified entity from context if set to true.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="removeSpecifiedEntities">if set to <c>true</c> [remove specified entities].</param>
		/// <returns></returns>
		int SaveChanges<TEntity>(bool removeSpecifiedEntities);


		/// <summary>
		/// Saves the changes.
		/// </summary>
		/// <param name="removeAllEntities">if set to <c>true</c> [remove all entities].</param>
		/// <returns></returns>
		int SaveChanges(bool removeAllEntities);

		/// <summary>
		/// Persists all updates to the store.
		/// </summary>
		/// <returns>The number of saved objects</returns>
		int SaveChanges();

		/// <summary>
		/// Refreshes the entire context using the store.
		/// </summary>
		void RefreshAll();

		/// <summary>
		/// Refreshes the entity.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="entity">The entity.</param>
		void RefreshEntity<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Refreshes the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="refreshMode">The refresh mode.</param>
        void RefreshEntity<TEntity>(TEntity entity, RefreshMode refreshMode) where TEntity : class;

		/// <summary>
		/// Refreshes the entities.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="entities">The entities.</param>
		void RefreshEntities<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
		
		/// <summary>
        /// Refreshes the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        /// <param name="refreshMode">The refresh mode.</param>
        void RefreshEntities<TEntity>(IEnumerable<TEntity> entities, RefreshMode refreshMode) where TEntity : class;

		/// <summary>
		/// Clears the context entries.
		/// </summary>
		void ClearContextEntries<TEntity>();

		/// <summary>
		/// Clears the context entries.
		/// </summary>
		void ClearContextEntries();

		/// <summary>
		/// Attaches the entity.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="context">The context.</param>
		/// <param name="entity">The entity.</param>
		/// <param name="keyNames">Name of the key.</param>
		/// <param name="userId">The user unique identifier.</param>
		/// <exception cref="System.Exception">The given key( + keyName + ) for entity( + entities.FirstOrDefault().GetType().Name + ) is not found.</exception>
		void AttachEntity<TEntity>(DbContext context, TEntity entity, IEnumerable<string> keyNames, string userId) where TEntity : class;

		/// <summary>
		/// Attaches the entities.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="context">The context.</param>
		/// <param name="entities">The entities.</param>
		/// <param name="keyNames">Name of the key.</param>
		/// <param name="userId">The user unique identifier.</param>
		/// <exception cref="System.Exception">The given key( + keyName + ) for entity( + entity.GetType().Name + ) is not found.</exception>
		void AttachEntities<TEntity>(DbContext context, IEnumerable<TEntity> entities, IEnumerable<string> keyNames, string userId) where TEntity : class;

		/// <summary>
		/// Bulks the attach entities.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="entities">The entities.</param>
		/// <param name="keyName">Name of the key.</param>
		/// <param name="userId">The user unique identifier.</param>
		void BulkAttachEntities<TEntity>(IEnumerable<TEntity> entities, string keyName, string userId) where TEntity : class;

		/// <summary>
		/// Bulks the insert.
		/// This will NOT work for updates!!!
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="entities">The entities.</param>
		void BulkInsert<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
    }

    // ************************************************************************
    // Database context
    public class ANDP_Order_Entities : CommonDbContext, IANDP_Order_Entities, IDbModelCacheKeyProvider
    {
        public IDbSet<Account> Accounts { get; set; } // Account
        public IDbSet<AccountX> AccountXes { get; set; } // AccountX
        public IDbSet<CompaniesX> CompaniesXes { get; set; } // CompaniesX
        public IDbSet<Company> Companies { get; set; } // Companies
        public IDbSet<Equipment> Equipments { get; set; } // Equipment
        public IDbSet<Item> Items { get; set; } // Item
        public IDbSet<ItemProvisioningResultsVw> ItemProvisioningResultsVws { get; set; } // ItemProvisioningResults_VW
        public IDbSet<ItemX> ItemXes { get; set; } // ItemX
        public IDbSet<Order> Orders { get; set; } // Order
        public IDbSet<OrderX> OrderXes { get; set; } // OrderX
        public IDbSet<Service> Services { get; set; } // Service
        public IDbSet<ServiceX> ServiceXes { get; set; } // ServiceX

        static ANDP_Order_Entities()
        {
            Database.SetInitializer<ANDP_Order_Entities>(null);
        }

        public ANDP_Order_Entities()
            : base("Name=ANDP_Entities")
        {
        }

        public ANDP_Order_Entities(string connectionString) : base(connectionString)
        {
        }

		public ANDP_Order_Entities(string connectionString, string schema) : base(connectionString)
        {
            Database.SetInitializer<ANDP_Order_Entities>(null);
            this.SchemaName = schema;
        }

        public ANDP_Order_Entities(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model) : base(connectionString, model)
        {
        }

		public string SchemaName { get; private set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new AccountConfiguration()
                : new AccountConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new AccountXConfiguration()
                : new AccountXConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new CompaniesXConfiguration()
                : new CompaniesXConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new CompanyConfiguration()
                : new CompanyConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new EquipmentConfiguration()
                : new EquipmentConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new ItemConfiguration()
                : new ItemConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new ItemProvisioningResultsVwConfiguration()
                : new ItemProvisioningResultsVwConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new ItemXConfiguration()
                : new ItemXConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new OrderConfiguration()
                : new OrderConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new OrderXConfiguration()
                : new OrderXConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new ServiceConfiguration()
                : new ServiceConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new ServiceXConfiguration()
                : new ServiceXConfiguration(this.SchemaName));

        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new AccountConfiguration(schema));
            modelBuilder.Configurations.Add(new AccountXConfiguration(schema));
            modelBuilder.Configurations.Add(new CompaniesXConfiguration(schema));
            modelBuilder.Configurations.Add(new CompanyConfiguration(schema));
            modelBuilder.Configurations.Add(new EquipmentConfiguration(schema));
            modelBuilder.Configurations.Add(new ItemConfiguration(schema));
            modelBuilder.Configurations.Add(new ItemProvisioningResultsVwConfiguration(schema));
            modelBuilder.Configurations.Add(new ItemXConfiguration(schema));
            modelBuilder.Configurations.Add(new OrderConfiguration(schema));
            modelBuilder.Configurations.Add(new OrderXConfiguration(schema));
            modelBuilder.Configurations.Add(new ServiceConfiguration(schema));
            modelBuilder.Configurations.Add(new ServiceXConfiguration(schema));
            return modelBuilder;
        }

		public string CacheKey { get { return this.SchemaName; } }

    }

    // ************************************************************************
    // POCO classes

    // Account
    public class Account
    {
        public int Id { get; set; } // Id (Primary key)
        public string ExternalAccountId { get; set; } // ExternalAccountId
        public string ExternalAccountGroupId { get; set; } // ExternalAccountGroupId
        public int CompanyId { get; set; } // CompanyId
        public int StatusTypeId { get; set; } // StatusTypeId
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version

        // Foreign keys
        public virtual Company Company { get; set; } // FK_test_Account_Companies
    }

    // AccountX
    public class AccountX
    {
        public int Id { get; set; } // Id (Primary key)
        public int AccountId { get; set; } // AccountId
        public string ExternalAccountId { get; set; } // ExternalAccountId
        public string ExternalAccountGroupId { get; set; } // ExternalAccountGroupId
        public int CompanyId { get; set; } // CompanyId
        public int StatusTypeId { get; set; } // StatusTypeId
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version
        public DateTime RecordModified { get; set; } // RecordModified
        public string Action { get; set; } // Action
    }

    // CompaniesX
    public class CompaniesX
    {
        public int Id { get; set; } // Id (Primary key)
        public int CompanyId { get; set; } // CompanyId
        public string Name { get; set; } // Name
        public string ExternalCompanyId { get; set; } // ExternalCompanyId
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version
        public DateTime RecordModified { get; set; } // RecordModified
        public string Action { get; set; } // Action
    }

    // Companies
    public class Company
    {
        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public string ExternalCompanyId { get; set; } // ExternalCompanyId
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version

        // Reverse navigation
        public virtual ICollection<Account> Accounts { get; set; } // Account.FK_test_Account_Companies

        public Company()
        {
            Accounts = new List<Account>();
        }
    }

    // Equipment
    public class Equipment
    {
        public int Id { get; set; } // Id (Primary key)
        public string ExternalEquipmentId { get; set; } // ExternalEquipmentId
        public int EquipmentSetupId { get; set; } // EquipmentSetupId
        public int EquipmentItemId { get; set; } // EquipmentItemId
        public string EquipmentItemDescription { get; set; } // EquipmentItemDescription
        public int ItemId { get; set; } // ItemId
        public int Priority { get; set; } // Priority
        public int ProvisionSequence { get; set; } // ProvisionSequence
        public string Xml { get; set; } // Xml
        public DateTime ProvisionDate { get; set; } // ProvisionDate
        public int StatusTypeId { get; set; } // StatusTypeId
        public int ActionTypeId { get; set; } // ActionTypeId
        public DateTime? StartDate { get; set; } // StartDate
        public DateTime? CompletionDate { get; set; } // CompletionDate
        public string ResultMessage { get; set; } // ResultMessage
        public string Log { get; set; } // Log
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version

        // Foreign keys
        public virtual Item Item { get; set; } // FK_test_Equipment_Item
    }

    // Item
    public class Item
    {
        public int Id { get; set; } // Id (Primary key)
        public string ExternalItemId { get; set; } // ExternalItemId
        public int ServiceId { get; set; } // ServiceId
        public int Priority { get; set; } // Priority
        public int ProvisionSequence { get; set; } // ProvisionSequence
        public string Xml { get; set; } // Xml
        public DateTime ProvisionDate { get; set; } // ProvisionDate
        public int StatusTypeId { get; set; } // StatusTypeId
        public int ActionTypeId { get; set; } // ActionTypeId
        public int ItemTypeId { get; set; } // ItemTypeId
        public string ResultMessage { get; set; } // ResultMessage
        public DateTime? CompletionDate { get; set; } // CompletionDate
        public DateTime? StartDate { get; set; } // StartDate
        public string Log { get; set; } // Log
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int? Version { get; set; } // Version

        // Reverse navigation
        public virtual ICollection<Equipment> Equipments { get; set; } // Equipment.FK_test_Equipment_Item

        // Foreign keys
        public virtual Service Service { get; set; } // FK_test_Item_Service

        public Item()
        {
            Equipments = new List<Equipment>();
        }
    }

    // ItemProvisioningResults_VW
    public class ItemProvisioningResultsVw
    {
        public string ItemName { get; set; } // ItemName
        public string OrderName { get; set; } // OrderName
        public string OrderNumber { get; set; } // OrderNumber
        public string ExternalAccountId { get; set; } // ExternalAccountId
        public string Result { get; set; } // Result
        public DateTime OrderReceivedDate { get; set; } // OrderReceivedDate
        public DateTime? CompletionDate { get; set; } // CompletionDate
        public DateTime? StartDate { get; set; } // StartDate
        public int? ProvisionTimeInSeconds { get; set; } // ProvisionTimeInSeconds
        public string ResultMessage { get; set; } // ResultMessage
    }

    // ItemX
    public class ItemX
    {
        public int Id { get; set; } // Id (Primary key)
        public int ItemId { get; set; } // ItemId
        public string ExternalItemId { get; set; } // ExternalItemId
        public int ServiceId { get; set; } // ServiceId
        public int Priority { get; set; } // Priority
        public int ProvisionSequence { get; set; } // ProvisionSequence
        public string Xml { get; set; } // Xml
        public DateTime ProvisionDate { get; set; } // ProvisionDate
        public int StatusTypeId { get; set; } // StatusTypeId
        public int ActionTypeId { get; set; } // ActionTypeId
        public string Type { get; set; } // Type
        public string ResultMessage { get; set; } // ResultMessage
        public DateTime? CompletionDate { get; set; } // CompletionDate
        public DateTime? StartDate { get; set; } // StartDate
        public string Log { get; set; } // Log
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int? Version { get; set; } // Version
        public DateTime RecordModified { get; set; } // RecordModified
        public string Action { get; set; } // Action
    }

    // Order
    public class Order
    {
        public int Id { get; set; } // Id (Primary key)
        public string ExternalOrderId { get; set; } // ExternalOrderId
        public string ExternalAccountId { get; set; } // ExternalAccountId
        public string ExternalCompanyId { get; set; } // ExternalCompanyId
        public int Priority { get; set; } // Priority
        public string Xml { get; set; } // Xml
        public DateTime ProvisionDate { get; set; } // ProvisionDate
        public int StatusTypeId { get; set; } // StatusTypeId
        public string OrginatingIp { get; set; } // OrginatingIp
        public int ActionTypeId { get; set; } // ActionTypeId
        public string ResultMessage { get; set; } // ResultMessage
        public DateTime? CompletionDate { get; set; } // CompletionDate
        public DateTime? StartDate { get; set; } // StartDate
        public string Log { get; set; } // Log
        public bool ResponseSent { get; set; } // ResponseSent
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version

        // Reverse navigation
        public virtual ICollection<Service> Services { get; set; } // Service.FK_test_Service_Order

        // Foreign keys

        public Order()
        {
            Services = new List<Service>();
        }
    }

    // OrderX
    public class OrderX
    {
        public int Id { get; set; } // Id (Primary key)
        public int OrderId { get; set; } // OrderId
        public string ExternalOrderId { get; set; } // ExternalOrderId
        public string ExternalAccountId { get; set; } // ExternalAccountId
        public string ExternalCompanyId { get; set; } // ExternalCompanyId
        public int Priority { get; set; } // Priority
        public string Xml { get; set; } // Xml
        public DateTime ProvisionDate { get; set; } // ProvisionDate
        public int StatusTypeId { get; set; } // StatusTypeId
        public string OrginatingIp { get; set; } // OrginatingIp
        public int ActionTypeId { get; set; } // ActionTypeId
        public string ResultMessage { get; set; } // ResultMessage
        public DateTime? CompletionDate { get; set; } // CompletionDate
        public DateTime? StartDate { get; set; } // StartDate
        public string Log { get; set; } // Log
        public bool ResponseSent { get; set; } // ResponseSent
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version
        public DateTime RecordModified { get; set; } // RecordModified
        public string Action { get; set; } // Action
    }

    // Service
    public class Service
    {
        public int Id { get; set; } // Id (Primary key)
        public string ExternalServiceId { get; set; } // ExternalServiceId
        public int OrderId { get; set; } // OrderId
        public int Priority { get; set; } // Priority
        public int ProvisionSequence { get; set; } // ProvisionSequence
        public string Xml { get; set; } // Xml
        public DateTime ProvisionDate { get; set; } // ProvisionDate
        public int StatusTypeId { get; set; } // StatusTypeId
        public int ActionTypeId { get; set; } // ActionTypeId
        public string ResultMessage { get; set; } // ResultMessage
        public DateTime? CompletionDate { get; set; } // CompletionDate
        public DateTime? StartDate { get; set; } // StartDate
        public string Log { get; set; } // Log
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int? Version { get; set; } // Version

        // Reverse navigation
        public virtual ICollection<Item> Items { get; set; } // Item.FK_test_Item_Service

        // Foreign keys
        public virtual Order Order { get; set; } // FK_test_Service_Order

        public Service()
        {
            Items = new List<Item>();
        }
    }

    // ServiceX
    public class ServiceX
    {
        public int Id { get; set; } // Id (Primary key)
        public int ServiceId { get; set; } // ServiceId
        public string ExternalServiceId { get; set; } // ExternalServiceId
        public int OrderId { get; set; } // OrderId
        public int Priority { get; set; } // Priority
        public int ProvisionSequence { get; set; } // ProvisionSequence
        public string Xml { get; set; } // Xml
        public DateTime ProvisionDate { get; set; } // ProvisionDate
        public int StatusTypeId { get; set; } // StatusTypeId
        public int ActionTypeId { get; set; } // ActionTypeId
        public string ResultMessage { get; set; } // ResultMessage
        public DateTime? CompletionDate { get; set; } // CompletionDate
        public DateTime? StartDate { get; set; } // StartDate
        public string Log { get; set; } // Log
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int? Version { get; set; } // Version
        public DateTime RecordModified { get; set; } // RecordModified
        public string Action { get; set; } // Action
    }

 

    // ************************************************************************
    // Enums
	
		
	public enum ActionTypeEnum
	{
		Add = 1,
		Move = 2,
		Change = 3,
		Reassign = 4,
		Delete = 5,
		Restore = 6,
		Suspend = 7,
		ReconSuspend = 8,
		SuspendNonPay = 9,
		ReconNonPay = 10,
		Unchanged = 11,
	}
public enum EquipmentAuthenticationTypeEnum
	{
		Password = 1,
		PrivateKey = 2,
		KeyboardInteractive = 3,
		Other = 4,
	}
public enum EquipmentConnectionTypeEnum
	{
		Ssh = 1,
		TcpIp = 2,
		Telnet = 3,
		WebService = 4,
	}
public enum EquipmentEncodingTypeEnum
	{
		Ascii = 1,
		Unicode = 2,
		Utf32 = 3,
		Utf8 = 4,
		Utf7 = 5,
		BigEndianUnicode = 6,
	}
public enum EquipmentIpVersionTypeEnum
	{
		IPv4 = 4,
		IPv6 = 6,
	}
public enum EquipmentTypeEnum
	{
		Other = 1,
		Phone = 2,
		Video = 3,
		Internet = 4,
		DSLAM = 5,
	}
public enum ItemTypeEnum
	{
		Video = 1,
		Internet = 2,
		Phone = 3,
	}
public enum ProvisionByMethodTypeEnum
	{
		Order = 1,
		Service = 2,
		Item = 3,
		Equipment = 4,
	}
public enum StatusTypeEnum
	{
		Processing = 1,
		Pending = 2,
		Error = 3,
		Success = 4,
		Deleted = 5,
	}



    // ************************************************************************
    // POCO Configuration

    // Account
    internal class AccountConfiguration : EntityTypeConfiguration<Account>
    {
        public AccountConfiguration(string schema = "test")
        {
            ToTable(schema + ".Account");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.ExternalAccountId).HasColumnName("ExternalAccountId").IsRequired().HasMaxLength(36);
            Property(x => x.ExternalAccountGroupId).HasColumnName("ExternalAccountGroupId").IsOptional().HasMaxLength(36);
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.StatusTypeId).HasColumnName("StatusTypeId").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();

            // Foreign keys
            HasRequired(a => a.Company).WithMany(b => b.Accounts).HasForeignKey(c => c.CompanyId); // FK_test_Account_Companies
        }
    }

    // AccountX
    internal class AccountXConfiguration : EntityTypeConfiguration<AccountX>
    {
        public AccountXConfiguration(string schema = "test")
        {
            ToTable(schema + ".AccountX");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.AccountId).HasColumnName("AccountId").IsRequired();
            Property(x => x.ExternalAccountId).HasColumnName("ExternalAccountId").IsRequired().HasMaxLength(36);
            Property(x => x.ExternalAccountGroupId).HasColumnName("ExternalAccountGroupId").IsOptional().HasMaxLength(36);
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.StatusTypeId).HasColumnName("StatusTypeId").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();
            Property(x => x.RecordModified).HasColumnName("RecordModified").IsRequired();
            Property(x => x.Action).HasColumnName("Action").IsRequired().HasMaxLength(1);
        }
    }

    // CompaniesX
    internal class CompaniesXConfiguration : EntityTypeConfiguration<CompaniesX>
    {
        public CompaniesXConfiguration(string schema = "test")
        {
            ToTable(schema + ".CompaniesX");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(x => x.ExternalCompanyId).HasColumnName("ExternalCompanyId").IsRequired().HasMaxLength(36);
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();
            Property(x => x.RecordModified).HasColumnName("RecordModified").IsRequired();
            Property(x => x.Action).HasColumnName("Action").IsRequired().HasMaxLength(1);
        }
    }

    // Companies
    internal class CompanyConfiguration : EntityTypeConfiguration<Company>
    {
        public CompanyConfiguration(string schema = "test")
        {
            ToTable(schema + ".Companies");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(x => x.ExternalCompanyId).HasColumnName("ExternalCompanyId").IsRequired().HasMaxLength(36);
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();
        }
    }

    // Equipment
    internal class EquipmentConfiguration : EntityTypeConfiguration<Equipment>
    {
        public EquipmentConfiguration(string schema = "test")
        {
            ToTable(schema + ".Equipment");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.ExternalEquipmentId).HasColumnName("ExternalEquipmentId").IsRequired().HasMaxLength(36);
            Property(x => x.EquipmentSetupId).HasColumnName("EquipmentSetupId").IsRequired();
            Property(x => x.EquipmentItemId).HasColumnName("EquipmentItemId").IsRequired();
            Property(x => x.EquipmentItemDescription).HasColumnName("EquipmentItemDescription").IsOptional().HasMaxLength(100);
            Property(x => x.ItemId).HasColumnName("ItemId").IsRequired();
            Property(x => x.Priority).HasColumnName("Priority").IsRequired();
            Property(x => x.ProvisionSequence).HasColumnName("ProvisionSequence").IsRequired();
            Property(x => x.Xml).HasColumnName("Xml").IsRequired();
            Property(x => x.ProvisionDate).HasColumnName("ProvisionDate").IsRequired();
            Property(x => x.StatusTypeId).HasColumnName("StatusTypeId").IsRequired();
            Property(x => x.ActionTypeId).HasColumnName("ActionTypeId").IsRequired();
            Property(x => x.StartDate).HasColumnName("StartDate").IsOptional();
            Property(x => x.CompletionDate).HasColumnName("CompletionDate").IsOptional();
            Property(x => x.ResultMessage).HasColumnName("ResultMessage").IsOptional();
            Property(x => x.Log).HasColumnName("Log").IsOptional();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();

            // Foreign keys
            HasRequired(a => a.Item).WithMany(b => b.Equipments).HasForeignKey(c => c.ItemId); // FK_test_Equipment_Item
        }
    }

    // Item
    internal class ItemConfiguration : EntityTypeConfiguration<Item>
    {
        public ItemConfiguration(string schema = "test")
        {
            ToTable(schema + ".Item");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.ExternalItemId).HasColumnName("ExternalItemId").IsRequired().HasMaxLength(36);
            Property(x => x.ServiceId).HasColumnName("ServiceId").IsRequired();
            Property(x => x.Priority).HasColumnName("Priority").IsRequired();
            Property(x => x.ProvisionSequence).HasColumnName("ProvisionSequence").IsRequired();
            Property(x => x.Xml).HasColumnName("Xml").IsRequired();
            Property(x => x.ProvisionDate).HasColumnName("ProvisionDate").IsRequired();
            Property(x => x.StatusTypeId).HasColumnName("StatusTypeId").IsRequired();
            Property(x => x.ActionTypeId).HasColumnName("ActionTypeId").IsRequired();
            Property(x => x.ItemTypeId).HasColumnName("ItemTypeId").IsRequired();
            Property(x => x.ResultMessage).HasColumnName("ResultMessage").IsOptional();
            Property(x => x.CompletionDate).HasColumnName("CompletionDate").IsOptional();
            Property(x => x.StartDate).HasColumnName("StartDate").IsOptional();
            Property(x => x.Log).HasColumnName("Log").IsOptional();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsOptional().IsConcurrencyToken();

            // Foreign keys
            HasRequired(a => a.Service).WithMany(b => b.Items).HasForeignKey(c => c.ServiceId); // FK_test_Item_Service
        }
    }

    // ItemProvisioningResults_VW
    internal class ItemProvisioningResultsVwConfiguration : EntityTypeConfiguration<ItemProvisioningResultsVw>
    {
        public ItemProvisioningResultsVwConfiguration(string schema = "test")
        {
            ToTable(schema + ".ItemProvisioningResults_VW");
            HasKey(x => new { x.ItemName, x.OrderName, x.OrderNumber, x.ExternalAccountId, x.Result, x.OrderReceivedDate });

            Property(x => x.ItemName).HasColumnName("ItemName").IsRequired().HasMaxLength(50);
            Property(x => x.OrderName).HasColumnName("OrderName").IsRequired().HasMaxLength(50);
            Property(x => x.OrderNumber).HasColumnName("OrderNumber").IsRequired().HasMaxLength(36);
            Property(x => x.ExternalAccountId).HasColumnName("ExternalAccountId").IsRequired().HasMaxLength(36);
            Property(x => x.Result).HasColumnName("Result").IsRequired().HasMaxLength(50);
            Property(x => x.OrderReceivedDate).HasColumnName("OrderReceivedDate").IsRequired();
            Property(x => x.CompletionDate).HasColumnName("CompletionDate").IsOptional();
            Property(x => x.StartDate).HasColumnName("StartDate").IsOptional();
            Property(x => x.ProvisionTimeInSeconds).HasColumnName("ProvisionTimeInSeconds").IsOptional();
            Property(x => x.ResultMessage).HasColumnName("ResultMessage").IsOptional();
        }
    }

    // ItemX
    internal class ItemXConfiguration : EntityTypeConfiguration<ItemX>
    {
        public ItemXConfiguration(string schema = "test")
        {
            ToTable(schema + ".ItemX");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.ItemId).HasColumnName("ItemId").IsRequired();
            Property(x => x.ExternalItemId).HasColumnName("ExternalItemId").IsRequired().HasMaxLength(36);
            Property(x => x.ServiceId).HasColumnName("ServiceId").IsRequired();
            Property(x => x.Priority).HasColumnName("Priority").IsRequired();
            Property(x => x.ProvisionSequence).HasColumnName("ProvisionSequence").IsRequired();
            Property(x => x.Xml).HasColumnName("Xml").IsRequired();
            Property(x => x.ProvisionDate).HasColumnName("ProvisionDate").IsRequired();
            Property(x => x.StatusTypeId).HasColumnName("StatusTypeId").IsRequired();
            Property(x => x.ActionTypeId).HasColumnName("ActionTypeId").IsRequired();
            Property(x => x.Type).HasColumnName("Type").IsOptional().HasMaxLength(20);
            Property(x => x.ResultMessage).HasColumnName("ResultMessage").IsOptional();
            Property(x => x.CompletionDate).HasColumnName("CompletionDate").IsOptional();
            Property(x => x.StartDate).HasColumnName("StartDate").IsOptional();
            Property(x => x.Log).HasColumnName("Log").IsOptional();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsOptional().IsConcurrencyToken();
            Property(x => x.RecordModified).HasColumnName("RecordModified").IsRequired();
            Property(x => x.Action).HasColumnName("Action").IsRequired().HasMaxLength(1);
        }
    }

    // Order
    internal class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration(string schema = "test")
        {
            ToTable(schema + ".Order");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.ExternalOrderId).HasColumnName("ExternalOrderId").IsRequired().HasMaxLength(36);
            Property(x => x.ExternalAccountId).HasColumnName("ExternalAccountId").IsRequired().HasMaxLength(36);
            Property(x => x.ExternalCompanyId).HasColumnName("ExternalCompanyId").IsRequired().HasMaxLength(36);
            Property(x => x.Priority).HasColumnName("Priority").IsRequired();
            Property(x => x.Xml).HasColumnName("Xml").IsRequired();
            Property(x => x.ProvisionDate).HasColumnName("ProvisionDate").IsRequired();
            Property(x => x.StatusTypeId).HasColumnName("StatusTypeId").IsRequired();
            Property(x => x.OrginatingIp).HasColumnName("OrginatingIp").IsOptional().HasMaxLength(15);
            Property(x => x.ActionTypeId).HasColumnName("ActionTypeId").IsRequired();
            Property(x => x.ResultMessage).HasColumnName("ResultMessage").IsOptional();
            Property(x => x.CompletionDate).HasColumnName("CompletionDate").IsOptional();
            Property(x => x.StartDate).HasColumnName("StartDate").IsOptional();
            Property(x => x.Log).HasColumnName("Log").IsOptional();
            Property(x => x.ResponseSent).HasColumnName("ResponseSent").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();

            // Foreign keys
        }
    }

    // OrderX
    internal class OrderXConfiguration : EntityTypeConfiguration<OrderX>
    {
        public OrderXConfiguration(string schema = "test")
        {
            ToTable(schema + ".OrderX");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.OrderId).HasColumnName("OrderId").IsRequired();
            Property(x => x.ExternalOrderId).HasColumnName("ExternalOrderId").IsRequired().HasMaxLength(36);
            Property(x => x.ExternalAccountId).HasColumnName("ExternalAccountId").IsRequired().HasMaxLength(36);
            Property(x => x.ExternalCompanyId).HasColumnName("ExternalCompanyId").IsRequired().HasMaxLength(36);
            Property(x => x.Priority).HasColumnName("Priority").IsRequired();
            Property(x => x.Xml).HasColumnName("Xml").IsRequired();
            Property(x => x.ProvisionDate).HasColumnName("ProvisionDate").IsRequired();
            Property(x => x.StatusTypeId).HasColumnName("StatusTypeId").IsRequired();
            Property(x => x.OrginatingIp).HasColumnName("OrginatingIp").IsOptional().HasMaxLength(15);
            Property(x => x.ActionTypeId).HasColumnName("ActionTypeId").IsRequired();
            Property(x => x.ResultMessage).HasColumnName("ResultMessage").IsOptional();
            Property(x => x.CompletionDate).HasColumnName("CompletionDate").IsOptional();
            Property(x => x.StartDate).HasColumnName("StartDate").IsOptional();
            Property(x => x.Log).HasColumnName("Log").IsOptional();
            Property(x => x.ResponseSent).HasColumnName("ResponseSent").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();
            Property(x => x.RecordModified).HasColumnName("RecordModified").IsRequired();
            Property(x => x.Action).HasColumnName("Action").IsRequired().HasMaxLength(1);
        }
    }

    // Service
    internal class ServiceConfiguration : EntityTypeConfiguration<Service>
    {
        public ServiceConfiguration(string schema = "test")
        {
            ToTable(schema + ".Service");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.ExternalServiceId).HasColumnName("ExternalServiceId").IsRequired().HasMaxLength(36);
            Property(x => x.OrderId).HasColumnName("OrderId").IsRequired();
            Property(x => x.Priority).HasColumnName("Priority").IsRequired();
            Property(x => x.ProvisionSequence).HasColumnName("ProvisionSequence").IsRequired();
            Property(x => x.Xml).HasColumnName("Xml").IsRequired();
            Property(x => x.ProvisionDate).HasColumnName("ProvisionDate").IsRequired();
            Property(x => x.StatusTypeId).HasColumnName("StatusTypeId").IsRequired();
            Property(x => x.ActionTypeId).HasColumnName("ActionTypeId").IsRequired();
            Property(x => x.ResultMessage).HasColumnName("ResultMessage").IsOptional();
            Property(x => x.CompletionDate).HasColumnName("CompletionDate").IsOptional();
            Property(x => x.StartDate).HasColumnName("StartDate").IsOptional();
            Property(x => x.Log).HasColumnName("Log").IsOptional();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsOptional().IsConcurrencyToken();

            // Foreign keys
            HasRequired(a => a.Order).WithMany(b => b.Services).HasForeignKey(c => c.OrderId); // FK_test_Service_Order
        }
    }

    // ServiceX
    internal class ServiceXConfiguration : EntityTypeConfiguration<ServiceX>
    {
        public ServiceXConfiguration(string schema = "test")
        {
            ToTable(schema + ".ServiceX");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.ServiceId).HasColumnName("ServiceId").IsRequired();
            Property(x => x.ExternalServiceId).HasColumnName("ExternalServiceId").IsRequired().HasMaxLength(36);
            Property(x => x.OrderId).HasColumnName("OrderId").IsRequired();
            Property(x => x.Priority).HasColumnName("Priority").IsRequired();
            Property(x => x.ProvisionSequence).HasColumnName("ProvisionSequence").IsRequired();
            Property(x => x.Xml).HasColumnName("Xml").IsRequired();
            Property(x => x.ProvisionDate).HasColumnName("ProvisionDate").IsRequired();
            Property(x => x.StatusTypeId).HasColumnName("StatusTypeId").IsRequired();
            Property(x => x.ActionTypeId).HasColumnName("ActionTypeId").IsRequired();
            Property(x => x.ResultMessage).HasColumnName("ResultMessage").IsOptional();
            Property(x => x.CompletionDate).HasColumnName("CompletionDate").IsOptional();
            Property(x => x.StartDate).HasColumnName("StartDate").IsOptional();
            Property(x => x.Log).HasColumnName("Log").IsOptional();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsOptional().IsConcurrencyToken();
            Property(x => x.RecordModified).HasColumnName("RecordModified").IsRequired();
            Property(x => x.Action).HasColumnName("Action").IsRequired().HasMaxLength(1);
        }
    }

}

