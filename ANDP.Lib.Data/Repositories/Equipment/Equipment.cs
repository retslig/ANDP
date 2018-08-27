

// This file was automatically generated.
// Do not make changes directly to this file - edit the template instead.
// 
// The following connection settings were used to generate this file
// 
//     Configuration file:     "ANDP.Lib.Data\App.config"
//     Connection String Name: "ANDP_Entities"
//     Connection String:      "Data Source=sl5ivsvwoq.database.windows.net,1433;initial catalog=QssAndp;persist security info=False;user id=andpuser@sl5ivsvwoq;password=**zapped**;"

// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier

using System;
using Common.Lib.EntityProvider;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
//using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.DatabaseGeneratedOption;

namespace ANDP.Lib.Data.Repositories.Equipment
{
    // ************************************************************************
    // Unit of work
    public interface IANDP_Equipment_Entities : IDisposable
    {
        IDbSet<Account> Accounts { get; set; } // Account
        IDbSet<Company> Companies { get; set; } // Companies
        IDbSet<DataDictionary> DataDictionaries { get; set; } // DataDictionary
        IDbSet<Equipment> Equipments { get; set; } // Equipment
        IDbSet<EquipmentConnectionLoginSequence> EquipmentConnectionLoginSequences { get; set; } // EquipmentConnectionLoginSequences
        IDbSet<EquipmentConnectionLogoutSequence> EquipmentConnectionLogoutSequences { get; set; } // EquipmentConnectionLogoutSequences
        IDbSet<EquipmentConnectionSetting> EquipmentConnectionSettings { get; set; } // EquipmentConnectionSettings
        IDbSet<EquipmentConnectionSettingsX> EquipmentConnectionSettingsXes { get; set; } // EquipmentConnectionSettingsX
        IDbSet<EquipmentLoginSettingsVw> EquipmentLoginSettingsVws { get; set; } // EquipmentLoginSettings_VW
        IDbSet<EquipmentLogoutSettingsVw> EquipmentLogoutSettingsVws { get; set; } // EquipmentLogoutSettings_VW
        IDbSet<EquipmentSetting> EquipmentSettings { get; set; } // EquipmentSettings
        IDbSet<EquipmentSettingsVw> EquipmentSettingsVws { get; set; } // EquipmentSettings_VW
        IDbSet<EquipmentSettingsX> EquipmentSettingsXes { get; set; } // EquipmentSettingsX
        IDbSet<EquipmentSetup> EquipmentSetups { get; set; } // EquipmentSetup
        IDbSet<EquipmentSetupX> EquipmentSetupXes { get; set; } // EquipmentSetupX
        IDbSet<EquipmentX> EquipmentXes { get; set; } // EquipmentX
        IDbSet<UsocToCommandTranslation> UsocToCommandTranslations { get; set; } // UsocToCommandTranslation

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
		/// Refreshes the entities.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <param name="entities">The entities.</param>
		void RefreshEntities<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
		
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
    public class ANDP_Equipment_Entities : CommonDbContext, IANDP_Equipment_Entities, IDbModelCacheKeyProvider
    {
        public IDbSet<Account> Accounts { get; set; } // Account
        public IDbSet<Company> Companies { get; set; } // Companies
        public IDbSet<DataDictionary> DataDictionaries { get; set; } // DataDictionary
        public IDbSet<Equipment> Equipments { get; set; } // Equipment
        public IDbSet<EquipmentConnectionLoginSequence> EquipmentConnectionLoginSequences { get; set; } // EquipmentConnectionLoginSequences
        public IDbSet<EquipmentConnectionLogoutSequence> EquipmentConnectionLogoutSequences { get; set; } // EquipmentConnectionLogoutSequences
        public IDbSet<EquipmentConnectionSetting> EquipmentConnectionSettings { get; set; } // EquipmentConnectionSettings
        public IDbSet<EquipmentConnectionSettingsX> EquipmentConnectionSettingsXes { get; set; } // EquipmentConnectionSettingsX
        public IDbSet<EquipmentLoginSettingsVw> EquipmentLoginSettingsVws { get; set; } // EquipmentLoginSettings_VW
        public IDbSet<EquipmentLogoutSettingsVw> EquipmentLogoutSettingsVws { get; set; } // EquipmentLogoutSettings_VW
        public IDbSet<EquipmentSetting> EquipmentSettings { get; set; } // EquipmentSettings
        public IDbSet<EquipmentSettingsVw> EquipmentSettingsVws { get; set; } // EquipmentSettings_VW
        public IDbSet<EquipmentSettingsX> EquipmentSettingsXes { get; set; } // EquipmentSettingsX
        public IDbSet<EquipmentSetup> EquipmentSetups { get; set; } // EquipmentSetup
        public IDbSet<EquipmentSetupX> EquipmentSetupXes { get; set; } // EquipmentSetupX
        public IDbSet<EquipmentX> EquipmentXes { get; set; } // EquipmentX
        public IDbSet<UsocToCommandTranslation> UsocToCommandTranslations { get; set; } // UsocToCommandTranslation

        static ANDP_Equipment_Entities()
        {
            Database.SetInitializer<ANDP_Equipment_Entities>(null);
        }

        public ANDP_Equipment_Entities()
            : base("Name=ANDP_Entities")
        {
        }

        public ANDP_Equipment_Entities(string connectionString) : base(connectionString)
        {
        }

		public ANDP_Equipment_Entities(string connectionString, string schema) : base(connectionString)
        {
            Database.SetInitializer<ANDP_Equipment_Entities>(null);
            this.SchemaName = schema;
        }

        public ANDP_Equipment_Entities(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model) : base(connectionString, model)
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
                ? new CompanyConfiguration()
                : new CompanyConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new DataDictionaryConfiguration()
                : new DataDictionaryConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new EquipmentConfiguration()
                : new EquipmentConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new EquipmentConnectionLoginSequenceConfiguration()
                : new EquipmentConnectionLoginSequenceConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new EquipmentConnectionLogoutSequenceConfiguration()
                : new EquipmentConnectionLogoutSequenceConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new EquipmentConnectionSettingConfiguration()
                : new EquipmentConnectionSettingConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new EquipmentConnectionSettingsXConfiguration()
                : new EquipmentConnectionSettingsXConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new EquipmentLoginSettingsVwConfiguration()
                : new EquipmentLoginSettingsVwConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new EquipmentLogoutSettingsVwConfiguration()
                : new EquipmentLogoutSettingsVwConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new EquipmentSettingConfiguration()
                : new EquipmentSettingConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new EquipmentSettingsVwConfiguration()
                : new EquipmentSettingsVwConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new EquipmentSettingsXConfiguration()
                : new EquipmentSettingsXConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new EquipmentSetupConfiguration()
                : new EquipmentSetupConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new EquipmentSetupXConfiguration()
                : new EquipmentSetupXConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new EquipmentXConfiguration()
                : new EquipmentXConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new UsocToCommandTranslationConfiguration()
                : new UsocToCommandTranslationConfiguration(this.SchemaName));

        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new AccountConfiguration(schema));
            modelBuilder.Configurations.Add(new CompanyConfiguration(schema));
            modelBuilder.Configurations.Add(new DataDictionaryConfiguration(schema));
            modelBuilder.Configurations.Add(new EquipmentConfiguration(schema));
            modelBuilder.Configurations.Add(new EquipmentConnectionLoginSequenceConfiguration(schema));
            modelBuilder.Configurations.Add(new EquipmentConnectionLogoutSequenceConfiguration(schema));
            modelBuilder.Configurations.Add(new EquipmentConnectionSettingConfiguration(schema));
            modelBuilder.Configurations.Add(new EquipmentConnectionSettingsXConfiguration(schema));
            modelBuilder.Configurations.Add(new EquipmentLoginSettingsVwConfiguration(schema));
            modelBuilder.Configurations.Add(new EquipmentLogoutSettingsVwConfiguration(schema));
            modelBuilder.Configurations.Add(new EquipmentSettingConfiguration(schema));
            modelBuilder.Configurations.Add(new EquipmentSettingsVwConfiguration(schema));
            modelBuilder.Configurations.Add(new EquipmentSettingsXConfiguration(schema));
            modelBuilder.Configurations.Add(new EquipmentSetupConfiguration(schema));
            modelBuilder.Configurations.Add(new EquipmentSetupXConfiguration(schema));
            modelBuilder.Configurations.Add(new EquipmentXConfiguration(schema));
            modelBuilder.Configurations.Add(new UsocToCommandTranslationConfiguration(schema));
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
        public virtual ICollection<DataDictionary> DataDictionaries { get; set; } // DataDictionary.FK_test_DataDictionary_Companies
        public virtual ICollection<EquipmentSetup> EquipmentSetups { get; set; } // EquipmentSetup.FK_test_EquipmentSetup_Companies
        public virtual ICollection<UsocToCommandTranslation> UsocToCommandTranslations { get; set; } // UsocToCommandTranslation.FK_test_UsocToCommandTranslation_Companies

        public Company()
        {
            Accounts = new List<Account>();
            DataDictionaries = new List<DataDictionary>();
            EquipmentSetups = new List<EquipmentSetup>();
            UsocToCommandTranslations = new List<UsocToCommandTranslation>();
        }
    }

    // DataDictionary
    public class DataDictionary
    {
        public int Id { get; set; } // Id (Primary key)
        public int CompanyId { get; set; } // CompanyId
        public int EquipmentId { get; set; } // EquipmentId
        public string Key1 { get; set; } // Key1
        public string Key2 { get; set; } // Key2
        public string Key3 { get; set; } // Key3
        public string Key4 { get; set; } // Key4
        public string Key5 { get; set; } // Key5
        public string Key6 { get; set; } // Key6
        public string Key7 { get; set; } // Key7
        public string Key8 { get; set; } // Key8
        public string Key9 { get; set; } // Key9
        public string Value1 { get; set; } // Value1
        public string Value2 { get; set; } // Value2
        public string Value3 { get; set; } // Value3
        public string Value4 { get; set; } // Value4
        public string Value5 { get; set; } // Value5
        public string Value6 { get; set; } // Value6
        public string Value7 { get; set; } // Value7
        public string Value8 { get; set; } // Value8
        public string Value9 { get; set; } // Value9
        public bool Active { get; set; } // Active
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version

        // Foreign keys
        public virtual Company Company { get; set; } // FK_test_DataDictionary_Companies
        public virtual EquipmentSetup EquipmentSetup { get; set; } // FK_test_DataDictionary_Equipment
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
        public virtual EquipmentSetup EquipmentSetup { get; set; } // FK_test_Equipment_EquipmentSetup
    }

    // EquipmentConnectionLoginSequences
    public class EquipmentConnectionLoginSequence
    {
        public int Id { get; set; } // Id (Primary key)
        public int EquipmentConnectionSettingsId { get; set; } // EquipmentConnectionSettingsId
        public int SequenceNumber { get; set; } // SequenceNumber
        public string Command { get; set; } // Command
        public string ExpectedResponse { get; set; } // ExpectedResponse
        public int Timeout { get; set; } // Timeout
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version

        // Foreign keys
        public virtual EquipmentConnectionSetting EquipmentConnectionSetting { get; set; } // FK_test_EquipmentConnectionLoginSequences_EquipmentConnectionSettings
    }

    // EquipmentConnectionLogoutSequences
    public class EquipmentConnectionLogoutSequence
    {
        public int Id { get; set; } // Id (Primary key)
        public int EquipmentConnectionSettingsId { get; set; } // EquipmentConnectionSettingsId
        public int SequenceNumber { get; set; } // SequenceNumber
        public string Command { get; set; } // Command
        public string ExpectedResponse { get; set; } // ExpectedResponse
        public int Timeout { get; set; } // Timeout
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version

        // Foreign keys
        public virtual EquipmentConnectionSetting EquipmentConnectionSetting { get; set; } // FK_test_EquipmentConnectionLogoutSequences_EquipmentConnectionSettings
    }

    // EquipmentConnectionSettings
    public class EquipmentConnectionSetting
    {
        public int Id { get; set; } // Id (Primary key)
        public int EquipmentConnectionTypeId { get; set; } // EquipmentConnectionTypeId
        public int EquipmentEncodingTypeId { get; set; } // EquipmentEncodingTypeId
        public string Url { get; set; } // Url
        public string Ip { get; set; } // Ip
        public int EquipmentIpVersionTypeId { get; set; } // EquipmentIpVersionTypeId
        public int? Port { get; set; } // Port
        public int EquipmentAuthenticationTypeId { get; set; } // EquipmentAuthenticationTypeId
        public string Username { get; set; } // Username
        public string Password { get; set; } // Password
        public bool? ShowTelnetCodes { get; set; } // ShowTelnetCodes
        public bool? RemoveNonPrintableChars { get; set; } // RemoveNonPrintableChars
        public bool? ReplaceNonPrintableChars { get; set; } // ReplaceNonPrintableChars
        public bool? CustomBool1 { get; set; } // CustomBool1
        public string CustomString1 { get; set; } // CustomString1
        public int? CustomInt1 { get; set; } // CustomInt1
        public bool? CustomBool2 { get; set; } // CustomBool2
        public string CustomString2 { get; set; } // CustomString2
        public int? CustomInt2 { get; set; } // CustomInt2
        public bool? CustomBool3 { get; set; } // CustomBool3
        public string CustomString3 { get; set; } // CustomString3
        public int? CustomInt3 { get; set; } // CustomInt3
        public int MaxConcurrentConnections { get; set; } // MaxConcurrentConnections
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version

        // Reverse navigation
        public virtual ICollection<EquipmentConnectionLoginSequence> EquipmentConnectionLoginSequences { get; set; } // EquipmentConnectionLoginSequences.FK_test_EquipmentConnectionLoginSequences_EquipmentConnectionSettings
        public virtual ICollection<EquipmentConnectionLogoutSequence> EquipmentConnectionLogoutSequences { get; set; } // EquipmentConnectionLogoutSequences.FK_test_EquipmentConnectionLogoutSequences_EquipmentConnectionSettings
        public virtual ICollection<EquipmentSetup> EquipmentSetups { get; set; } // EquipmentSetup.FK_test_EquipmentSetup_EquipmentConnectionSettings

        // Foreign keys

        public EquipmentConnectionSetting()
        {
            EquipmentConnectionLoginSequences = new List<EquipmentConnectionLoginSequence>();
            EquipmentConnectionLogoutSequences = new List<EquipmentConnectionLogoutSequence>();
            EquipmentSetups = new List<EquipmentSetup>();
        }
    }

    // EquipmentConnectionSettingsX
    public class EquipmentConnectionSettingsX
    {
        public int Id { get; set; } // Id (Primary key)
        public int EquipmentConnectionSettingsId { get; set; } // EquipmentConnectionSettingsId
        public int EquipmentConnectionTypeId { get; set; } // EquipmentConnectionTypeId
        public int EquipmentEncodingTypeId { get; set; } // EquipmentEncodingTypeId
        public string Url { get; set; } // Url
        public string Ip { get; set; } // Ip
        public int EquipmentIpVersionTypeId { get; set; } // EquipmentIpVersionTypeId
        public int? Port { get; set; } // Port
        public int EquipmentAuthenticationTypeId { get; set; } // EquipmentAuthenticationTypeId
        public string Username { get; set; } // Username
        public string Password { get; set; } // Password
        public bool? ShowTelnetCodes { get; set; } // ShowTelnetCodes
        public bool? RemoveNonPrintableChars { get; set; } // RemoveNonPrintableChars
        public bool? ReplaceNonPrintableChars { get; set; } // ReplaceNonPrintableChars
        public bool? CustomBool1 { get; set; } // CustomBool1
        public string CustomString1 { get; set; } // CustomString1
        public int? CustomInt1 { get; set; } // CustomInt1
        public bool? CustomBool2 { get; set; } // CustomBool2
        public string CustomString2 { get; set; } // CustomString2
        public int? CustomInt2 { get; set; } // CustomInt2
        public bool? CustomBool3 { get; set; } // CustomBool3
        public string CustomString3 { get; set; } // CustomString3
        public int? CustomInt3 { get; set; } // CustomInt3
        public int MaxConcurrentConnections { get; set; } // MaxConcurrentConnections
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version
        public DateTime RecordModified { get; set; } // RecordModified
        public string Action { get; set; } // Action
    }

    // EquipmentLoginSettings_VW
    public class EquipmentLoginSettingsVw
    {
        public string EquipmentName { get; set; } // EquipmentName
        public int EquipmentId { get; set; } // EquipmentId
        public string CompanyName { get; set; } // CompanyName
        public string EquipmentType { get; set; } // EquipmentType
        public string EquipmentConnectionType { get; set; } // EquipmentConnectionType
        public string EquipmentEncodingType { get; set; } // EquipmentEncodingType
        public string Url { get; set; } // Url
        public string Ip { get; set; } // Ip
        public string EquipmentIpVersionType { get; set; } // EquipmentIpVersionType
        public int? Port { get; set; } // Port
        public string EquipmentAuthenticationType { get; set; } // EquipmentAuthenticationType
        public string Username { get; set; } // Username
        public string Password { get; set; } // Password
        public bool? ShowTelnetCodes { get; set; } // ShowTelnetCodes
        public bool? RemoveNonPrintableChars { get; set; } // RemoveNonPrintableChars
        public bool? ReplaceNonPrintableChars { get; set; } // ReplaceNonPrintableChars
        public bool? CustomBool1 { get; set; } // CustomBool1
        public string CustomString1 { get; set; } // CustomString1
        public int? CustomInt1 { get; set; } // CustomInt1
        public bool? CustomBool2 { get; set; } // CustomBool2
        public string CustomString2 { get; set; } // CustomString2
        public int? CustomInt2 { get; set; } // CustomInt2
        public bool? CustomBool3 { get; set; } // CustomBool3
        public string CustomString3 { get; set; } // CustomString3
        public int? CustomInt3 { get; set; } // CustomInt3
        public int? SequenceNumber { get; set; } // SequenceNumber
        public string Command { get; set; } // Command
        public string ExpectedResponse { get; set; } // ExpectedResponse
        public int? Timeout { get; set; } // Timeout
    }

    // EquipmentLogoutSettings_VW
    public class EquipmentLogoutSettingsVw
    {
        public string EquipmentName { get; set; } // EquipmentName
        public int EquipmentId { get; set; } // EquipmentId
        public string CompanyName { get; set; } // CompanyName
        public string EquipmentType { get; set; } // EquipmentType
        public string EquipmentConnectionType { get; set; } // EquipmentConnectionType
        public string EquipmentEncodingType { get; set; } // EquipmentEncodingType
        public string Url { get; set; } // Url
        public string Ip { get; set; } // Ip
        public string EquipmentIpVersionType { get; set; } // EquipmentIpVersionType
        public int? Port { get; set; } // Port
        public string EquipmentAuthenticationType { get; set; } // EquipmentAuthenticationType
        public string Username { get; set; } // Username
        public string Password { get; set; } // Password
        public bool? ShowTelnetCodes { get; set; } // ShowTelnetCodes
        public bool? RemoveNonPrintableChars { get; set; } // RemoveNonPrintableChars
        public bool? ReplaceNonPrintableChars { get; set; } // ReplaceNonPrintableChars
        public bool? CustomBool1 { get; set; } // CustomBool1
        public string CustomString1 { get; set; } // CustomString1
        public int? CustomInt1 { get; set; } // CustomInt1
        public bool? CustomBool2 { get; set; } // CustomBool2
        public string CustomString2 { get; set; } // CustomString2
        public int? CustomInt2 { get; set; } // CustomInt2
        public bool? CustomBool3 { get; set; } // CustomBool3
        public string CustomString3 { get; set; } // CustomString3
        public int? CustomInt3 { get; set; } // CustomInt3
        public int? SequenceNumber { get; set; } // SequenceNumber
        public string Command { get; set; } // Command
        public string ExpectedResponse { get; set; } // ExpectedResponse
        public int? Timeout { get; set; } // Timeout
    }

    // EquipmentSettings
    public class EquipmentSetting
    {
        public int Id { get; set; } // Id (Primary key)
        public int ItemType { get; set; } // ItemType
        public bool MaxThreads { get; set; } // MaxThreads
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version
    }

    // EquipmentSettings_VW
    public class EquipmentSettingsVw
    {
        public string EquipmentName { get; set; } // EquipmentName
        public int EquipmentId { get; set; } // EquipmentId
        public string CompanyName { get; set; } // CompanyName
        public string EquipmentType { get; set; } // EquipmentType
        public string EquipmentConnectionType { get; set; } // EquipmentConnectionType
        public string EquipmentEncodingType { get; set; } // EquipmentEncodingType
        public string Url { get; set; } // Url
        public string Ip { get; set; } // Ip
        public string EquipmentIpVersionType { get; set; } // EquipmentIpVersionType
        public int? Port { get; set; } // Port
        public string EquipmentAuthenticationType { get; set; } // EquipmentAuthenticationType
        public string Username { get; set; } // Username
        public string Password { get; set; } // Password
        public bool? ShowTelnetCodes { get; set; } // ShowTelnetCodes
        public bool? RemoveNonPrintableChars { get; set; } // RemoveNonPrintableChars
        public bool? ReplaceNonPrintableChars { get; set; } // ReplaceNonPrintableChars
        public bool? CustomBool1 { get; set; } // CustomBool1
        public string CustomString1 { get; set; } // CustomString1
        public int? CustomInt1 { get; set; } // CustomInt1
        public bool? CustomBool2 { get; set; } // CustomBool2
        public string CustomString2 { get; set; } // CustomString2
        public int? CustomInt2 { get; set; } // CustomInt2
        public bool? CustomBool3 { get; set; } // CustomBool3
        public string CustomString3 { get; set; } // CustomString3
        public int? CustomInt3 { get; set; } // CustomInt3
    }

    // EquipmentSettingsX
    public class EquipmentSettingsX
    {
        public int Id { get; set; } // Id (Primary key)
        public int EquipmentSettingsId { get; set; } // EquipmentSettingsId
        public int ItemType { get; set; } // ItemType
        public bool MaxThreads { get; set; } // MaxThreads
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version
        public DateTime RecordModified { get; set; } // RecordModified
        public string Action { get; set; } // Action
    }

    // EquipmentSetup
    public class EquipmentSetup
    {
        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public int EquipmentTypeId { get; set; } // EquipmentTypeId
        public int CompanyId { get; set; } // CompanyId
        public int EquipmentConnectionSettingsId { get; set; } // EquipmentConnectionSettingsId
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version
        public int? EquipmentVersion { get; set; } // EquipmentVersion

        // Reverse navigation
        public virtual ICollection<DataDictionary> DataDictionaries { get; set; } // DataDictionary.FK_test_DataDictionary_Equipment
        public virtual ICollection<Equipment> Equipments { get; set; } // Equipment.FK_test_Equipment_EquipmentSetup
        public virtual ICollection<UsocToCommandTranslation> UsocToCommandTranslations { get; set; } // UsocToCommandTranslation.FK_test_UsocToCommandTranslation_Equipment

        // Foreign keys
        public virtual Company Company { get; set; } // FK_test_EquipmentSetup_Companies
        public virtual EquipmentConnectionSetting EquipmentConnectionSetting { get; set; } // FK_test_EquipmentSetup_EquipmentConnectionSettings

        public EquipmentSetup()
        {
            DataDictionaries = new List<DataDictionary>();
            Equipments = new List<Equipment>();
            UsocToCommandTranslations = new List<UsocToCommandTranslation>();
        }
    }

    // EquipmentSetupX
    public class EquipmentSetupX
    {
        public int Id { get; set; } // Id (Primary key)
        public int EquipmentSetupId { get; set; } // EquipmentSetupId
        public string Name { get; set; } // Name
        public int EquipmentTypeId { get; set; } // EquipmentTypeId
        public int CompanyId { get; set; } // CompanyId
        public int EquipmentConnectionSettingsId { get; set; } // EquipmentConnectionSettingsId
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version
        public int? EquipmentVersion { get; set; } // EquipmentVersion
        public DateTime RecordModified { get; set; } // RecordModified
        public string Action { get; set; } // Action
    }

    // EquipmentX
    public class EquipmentX
    {
        public int Id { get; set; } // Id (Primary key)
        public int EquipmentId { get; set; } // EquipmentId
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
        public DateTime RecordModified { get; set; } // RecordModified
        public string Action { get; set; } // Action
    }

    // UsocToCommandTranslation
    public class UsocToCommandTranslation
    {
        public int Id { get; set; } // Id (Primary key)
        public int CompanyId { get; set; } // CompanyId
        public int EquipmentId { get; set; } // EquipmentId
        public string UsocName { get; set; } // UsocName
        public string AddCommand { get; set; } // AddCommand
        public string DeleteCommand { get; set; } // DeleteCommand
        public bool Active { get; set; } // Active
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version

        // Foreign keys
        public virtual Company Company { get; set; } // FK_test_UsocToCommandTranslation_Companies
        public virtual EquipmentSetup EquipmentSetup { get; set; } // FK_test_UsocToCommandTranslation_Equipment
    }

 

    // ************************************************************************
    // Enums
	
		
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

    // DataDictionary
    internal class DataDictionaryConfiguration : EntityTypeConfiguration<DataDictionary>
    {
        public DataDictionaryConfiguration(string schema = "test")
        {
            ToTable(schema + ".DataDictionary");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.EquipmentId).HasColumnName("EquipmentId").IsRequired();
            Property(x => x.Key1).HasColumnName("Key1").IsRequired().HasMaxLength(100);
            Property(x => x.Key2).HasColumnName("Key2").IsRequired().HasMaxLength(100);
            Property(x => x.Key3).HasColumnName("Key3").IsRequired().HasMaxLength(100);
            Property(x => x.Key4).HasColumnName("Key4").IsRequired().HasMaxLength(100);
            Property(x => x.Key5).HasColumnName("Key5").IsRequired().HasMaxLength(100);
            Property(x => x.Key6).HasColumnName("Key6").IsRequired().HasMaxLength(100);
            Property(x => x.Key7).HasColumnName("Key7").IsRequired().HasMaxLength(100);
            Property(x => x.Key8).HasColumnName("Key8").IsRequired().HasMaxLength(100);
            Property(x => x.Key9).HasColumnName("Key9").IsRequired().HasMaxLength(100);
            Property(x => x.Value1).HasColumnName("Value1").IsRequired().HasMaxLength(100);
            Property(x => x.Value2).HasColumnName("Value2").IsRequired().HasMaxLength(100);
            Property(x => x.Value3).HasColumnName("Value3").IsRequired().HasMaxLength(100);
            Property(x => x.Value4).HasColumnName("Value4").IsRequired().HasMaxLength(100);
            Property(x => x.Value5).HasColumnName("Value5").IsRequired().HasMaxLength(100);
            Property(x => x.Value6).HasColumnName("Value6").IsRequired().HasMaxLength(100);
            Property(x => x.Value7).HasColumnName("Value7").IsRequired().HasMaxLength(100);
            Property(x => x.Value8).HasColumnName("Value8").IsRequired().HasMaxLength(100);
            Property(x => x.Value9).HasColumnName("Value9").IsRequired().HasMaxLength(100);
            Property(x => x.Active).HasColumnName("Active").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();

            // Foreign keys
            HasRequired(a => a.Company).WithMany(b => b.DataDictionaries).HasForeignKey(c => c.CompanyId); // FK_test_DataDictionary_Companies
            HasRequired(a => a.EquipmentSetup).WithMany(b => b.DataDictionaries).HasForeignKey(c => c.EquipmentId); // FK_test_DataDictionary_Equipment
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
            HasRequired(a => a.EquipmentSetup).WithMany(b => b.Equipments).HasForeignKey(c => c.EquipmentSetupId); // FK_test_Equipment_EquipmentSetup
        }
    }

    // EquipmentConnectionLoginSequences
    internal class EquipmentConnectionLoginSequenceConfiguration : EntityTypeConfiguration<EquipmentConnectionLoginSequence>
    {
        public EquipmentConnectionLoginSequenceConfiguration(string schema = "test")
        {
            ToTable(schema + ".EquipmentConnectionLoginSequences");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.EquipmentConnectionSettingsId).HasColumnName("EquipmentConnectionSettingsId").IsRequired();
            Property(x => x.SequenceNumber).HasColumnName("SequenceNumber").IsRequired();
            Property(x => x.Command).HasColumnName("Command").IsOptional().HasMaxLength(100);
            Property(x => x.ExpectedResponse).HasColumnName("ExpectedResponse").IsOptional().HasMaxLength(100);
            Property(x => x.Timeout).HasColumnName("Timeout").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();

            // Foreign keys
            HasRequired(a => a.EquipmentConnectionSetting).WithMany(b => b.EquipmentConnectionLoginSequences).HasForeignKey(c => c.EquipmentConnectionSettingsId); // FK_test_EquipmentConnectionLoginSequences_EquipmentConnectionSettings
        }
    }

    // EquipmentConnectionLogoutSequences
    internal class EquipmentConnectionLogoutSequenceConfiguration : EntityTypeConfiguration<EquipmentConnectionLogoutSequence>
    {
        public EquipmentConnectionLogoutSequenceConfiguration(string schema = "test")
        {
            ToTable(schema + ".EquipmentConnectionLogoutSequences");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.EquipmentConnectionSettingsId).HasColumnName("EquipmentConnectionSettingsId").IsRequired();
            Property(x => x.SequenceNumber).HasColumnName("SequenceNumber").IsRequired();
            Property(x => x.Command).HasColumnName("Command").IsOptional().HasMaxLength(100);
            Property(x => x.ExpectedResponse).HasColumnName("ExpectedResponse").IsOptional().HasMaxLength(100);
            Property(x => x.Timeout).HasColumnName("Timeout").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();

            // Foreign keys
            HasRequired(a => a.EquipmentConnectionSetting).WithMany(b => b.EquipmentConnectionLogoutSequences).HasForeignKey(c => c.EquipmentConnectionSettingsId); // FK_test_EquipmentConnectionLogoutSequences_EquipmentConnectionSettings
        }
    }

    // EquipmentConnectionSettings
    internal class EquipmentConnectionSettingConfiguration : EntityTypeConfiguration<EquipmentConnectionSetting>
    {
        public EquipmentConnectionSettingConfiguration(string schema = "test")
        {
            ToTable(schema + ".EquipmentConnectionSettings");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.EquipmentConnectionTypeId).HasColumnName("EquipmentConnectionTypeId").IsRequired();
            Property(x => x.EquipmentEncodingTypeId).HasColumnName("EquipmentEncodingTypeId").IsRequired();
            Property(x => x.Url).HasColumnName("Url").IsOptional().HasMaxLength(100);
            Property(x => x.Ip).HasColumnName("Ip").IsOptional().HasMaxLength(20);
            Property(x => x.EquipmentIpVersionTypeId).HasColumnName("EquipmentIpVersionTypeId").IsRequired();
            Property(x => x.Port).HasColumnName("Port").IsOptional();
            Property(x => x.EquipmentAuthenticationTypeId).HasColumnName("EquipmentAuthenticationTypeId").IsRequired();
            Property(x => x.Username).HasColumnName("Username").IsOptional().HasMaxLength(20);
            Property(x => x.Password).HasColumnName("Password").IsOptional().HasMaxLength(20);
            Property(x => x.ShowTelnetCodes).HasColumnName("ShowTelnetCodes").IsOptional();
            Property(x => x.RemoveNonPrintableChars).HasColumnName("RemoveNonPrintableChars").IsOptional();
            Property(x => x.ReplaceNonPrintableChars).HasColumnName("ReplaceNonPrintableChars").IsOptional();
            Property(x => x.CustomBool1).HasColumnName("CustomBool1").IsOptional();
            Property(x => x.CustomString1).HasColumnName("CustomString1").IsOptional().HasMaxLength(200);
            Property(x => x.CustomInt1).HasColumnName("CustomInt1").IsOptional();
            Property(x => x.CustomBool2).HasColumnName("CustomBool2").IsOptional();
            Property(x => x.CustomString2).HasColumnName("CustomString2").IsOptional().HasMaxLength(200);
            Property(x => x.CustomInt2).HasColumnName("CustomInt2").IsOptional();
            Property(x => x.CustomBool3).HasColumnName("CustomBool3").IsOptional();
            Property(x => x.CustomString3).HasColumnName("CustomString3").IsOptional().HasMaxLength(200);
            Property(x => x.CustomInt3).HasColumnName("CustomInt3").IsOptional();
            Property(x => x.MaxConcurrentConnections).HasColumnName("MaxConcurrentConnections").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();

            // Foreign keys
        }
    }

    // EquipmentConnectionSettingsX
    internal class EquipmentConnectionSettingsXConfiguration : EntityTypeConfiguration<EquipmentConnectionSettingsX>
    {
        public EquipmentConnectionSettingsXConfiguration(string schema = "test")
        {
            ToTable(schema + ".EquipmentConnectionSettingsX");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.EquipmentConnectionSettingsId).HasColumnName("EquipmentConnectionSettingsId").IsRequired();
            Property(x => x.EquipmentConnectionTypeId).HasColumnName("EquipmentConnectionTypeId").IsRequired();
            Property(x => x.EquipmentEncodingTypeId).HasColumnName("EquipmentEncodingTypeId").IsRequired();
            Property(x => x.Url).HasColumnName("Url").IsOptional().HasMaxLength(100);
            Property(x => x.Ip).HasColumnName("Ip").IsOptional().HasMaxLength(20);
            Property(x => x.EquipmentIpVersionTypeId).HasColumnName("EquipmentIpVersionTypeId").IsRequired();
            Property(x => x.Port).HasColumnName("Port").IsOptional();
            Property(x => x.EquipmentAuthenticationTypeId).HasColumnName("EquipmentAuthenticationTypeId").IsRequired();
            Property(x => x.Username).HasColumnName("Username").IsOptional().HasMaxLength(20);
            Property(x => x.Password).HasColumnName("Password").IsOptional().HasMaxLength(20);
            Property(x => x.ShowTelnetCodes).HasColumnName("ShowTelnetCodes").IsOptional();
            Property(x => x.RemoveNonPrintableChars).HasColumnName("RemoveNonPrintableChars").IsOptional();
            Property(x => x.ReplaceNonPrintableChars).HasColumnName("ReplaceNonPrintableChars").IsOptional();
            Property(x => x.CustomBool1).HasColumnName("CustomBool1").IsOptional();
            Property(x => x.CustomString1).HasColumnName("CustomString1").IsOptional().HasMaxLength(200);
            Property(x => x.CustomInt1).HasColumnName("CustomInt1").IsOptional();
            Property(x => x.CustomBool2).HasColumnName("CustomBool2").IsOptional();
            Property(x => x.CustomString2).HasColumnName("CustomString2").IsOptional().HasMaxLength(200);
            Property(x => x.CustomInt2).HasColumnName("CustomInt2").IsOptional();
            Property(x => x.CustomBool3).HasColumnName("CustomBool3").IsOptional();
            Property(x => x.CustomString3).HasColumnName("CustomString3").IsOptional().HasMaxLength(200);
            Property(x => x.CustomInt3).HasColumnName("CustomInt3").IsOptional();
            Property(x => x.MaxConcurrentConnections).HasColumnName("MaxConcurrentConnections").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();
            Property(x => x.RecordModified).HasColumnName("RecordModified").IsRequired();
            Property(x => x.Action).HasColumnName("Action").IsRequired().HasMaxLength(1);
        }
    }

    // EquipmentLoginSettings_VW
    internal class EquipmentLoginSettingsVwConfiguration : EntityTypeConfiguration<EquipmentLoginSettingsVw>
    {
        public EquipmentLoginSettingsVwConfiguration(string schema = "test")
        {
            ToTable(schema + ".EquipmentLoginSettings_VW");
            HasKey(x => new { x.CompanyName, x.EquipmentAuthenticationType, x.EquipmentConnectionType, x.EquipmentEncodingType, x.EquipmentId, x.EquipmentIpVersionType, x.EquipmentName, x.EquipmentType });

            Property(x => x.EquipmentName).HasColumnName("EquipmentName").IsRequired().HasMaxLength(50);
            Property(x => x.EquipmentId).HasColumnName("EquipmentId").IsRequired();
            Property(x => x.CompanyName).HasColumnName("CompanyName").IsRequired().HasMaxLength(50);
            Property(x => x.EquipmentType).HasColumnName("EquipmentType").IsRequired().HasMaxLength(50);
            Property(x => x.EquipmentConnectionType).HasColumnName("EquipmentConnectionType").IsRequired().HasMaxLength(50);
            Property(x => x.EquipmentEncodingType).HasColumnName("EquipmentEncodingType").IsRequired().HasMaxLength(50);
            Property(x => x.Url).HasColumnName("Url").IsOptional().HasMaxLength(100);
            Property(x => x.Ip).HasColumnName("Ip").IsOptional().HasMaxLength(20);
            Property(x => x.EquipmentIpVersionType).HasColumnName("EquipmentIpVersionType").IsRequired().HasMaxLength(50);
            Property(x => x.Port).HasColumnName("Port").IsOptional();
            Property(x => x.EquipmentAuthenticationType).HasColumnName("EquipmentAuthenticationType").IsRequired().HasMaxLength(50);
            Property(x => x.Username).HasColumnName("Username").IsOptional().HasMaxLength(20);
            Property(x => x.Password).HasColumnName("Password").IsOptional().HasMaxLength(20);
            Property(x => x.ShowTelnetCodes).HasColumnName("ShowTelnetCodes").IsOptional();
            Property(x => x.RemoveNonPrintableChars).HasColumnName("RemoveNonPrintableChars").IsOptional();
            Property(x => x.ReplaceNonPrintableChars).HasColumnName("ReplaceNonPrintableChars").IsOptional();
            Property(x => x.CustomBool1).HasColumnName("CustomBool1").IsOptional();
            Property(x => x.CustomString1).HasColumnName("CustomString1").IsOptional().HasMaxLength(200);
            Property(x => x.CustomInt1).HasColumnName("CustomInt1").IsOptional();
            Property(x => x.CustomBool2).HasColumnName("CustomBool2").IsOptional();
            Property(x => x.CustomString2).HasColumnName("CustomString2").IsOptional().HasMaxLength(200);
            Property(x => x.CustomInt2).HasColumnName("CustomInt2").IsOptional();
            Property(x => x.CustomBool3).HasColumnName("CustomBool3").IsOptional();
            Property(x => x.CustomString3).HasColumnName("CustomString3").IsOptional().HasMaxLength(200);
            Property(x => x.CustomInt3).HasColumnName("CustomInt3").IsOptional();
            Property(x => x.SequenceNumber).HasColumnName("SequenceNumber").IsOptional();
            Property(x => x.Command).HasColumnName("Command").IsOptional().HasMaxLength(100);
            Property(x => x.ExpectedResponse).HasColumnName("ExpectedResponse").IsOptional().HasMaxLength(100);
            Property(x => x.Timeout).HasColumnName("Timeout").IsOptional();
        }
    }

    // EquipmentLogoutSettings_VW
    internal class EquipmentLogoutSettingsVwConfiguration : EntityTypeConfiguration<EquipmentLogoutSettingsVw>
    {
        public EquipmentLogoutSettingsVwConfiguration(string schema = "test")
        {
            ToTable(schema + ".EquipmentLogoutSettings_VW");
            HasKey(x => new { x.CompanyName, x.EquipmentAuthenticationType, x.EquipmentConnectionType, x.EquipmentEncodingType, x.EquipmentId, x.EquipmentIpVersionType, x.EquipmentName, x.EquipmentType });

            Property(x => x.EquipmentName).HasColumnName("EquipmentName").IsRequired().HasMaxLength(50);
            Property(x => x.EquipmentId).HasColumnName("EquipmentId").IsRequired();
            Property(x => x.CompanyName).HasColumnName("CompanyName").IsRequired().HasMaxLength(50);
            Property(x => x.EquipmentType).HasColumnName("EquipmentType").IsRequired().HasMaxLength(50);
            Property(x => x.EquipmentConnectionType).HasColumnName("EquipmentConnectionType").IsRequired().HasMaxLength(50);
            Property(x => x.EquipmentEncodingType).HasColumnName("EquipmentEncodingType").IsRequired().HasMaxLength(50);
            Property(x => x.Url).HasColumnName("Url").IsOptional().HasMaxLength(100);
            Property(x => x.Ip).HasColumnName("Ip").IsOptional().HasMaxLength(20);
            Property(x => x.EquipmentIpVersionType).HasColumnName("EquipmentIpVersionType").IsRequired().HasMaxLength(50);
            Property(x => x.Port).HasColumnName("Port").IsOptional();
            Property(x => x.EquipmentAuthenticationType).HasColumnName("EquipmentAuthenticationType").IsRequired().HasMaxLength(50);
            Property(x => x.Username).HasColumnName("Username").IsOptional().HasMaxLength(20);
            Property(x => x.Password).HasColumnName("Password").IsOptional().HasMaxLength(20);
            Property(x => x.ShowTelnetCodes).HasColumnName("ShowTelnetCodes").IsOptional();
            Property(x => x.RemoveNonPrintableChars).HasColumnName("RemoveNonPrintableChars").IsOptional();
            Property(x => x.ReplaceNonPrintableChars).HasColumnName("ReplaceNonPrintableChars").IsOptional();
            Property(x => x.CustomBool1).HasColumnName("CustomBool1").IsOptional();
            Property(x => x.CustomString1).HasColumnName("CustomString1").IsOptional().HasMaxLength(200);
            Property(x => x.CustomInt1).HasColumnName("CustomInt1").IsOptional();
            Property(x => x.CustomBool2).HasColumnName("CustomBool2").IsOptional();
            Property(x => x.CustomString2).HasColumnName("CustomString2").IsOptional().HasMaxLength(200);
            Property(x => x.CustomInt2).HasColumnName("CustomInt2").IsOptional();
            Property(x => x.CustomBool3).HasColumnName("CustomBool3").IsOptional();
            Property(x => x.CustomString3).HasColumnName("CustomString3").IsOptional().HasMaxLength(200);
            Property(x => x.CustomInt3).HasColumnName("CustomInt3").IsOptional();
            Property(x => x.SequenceNumber).HasColumnName("SequenceNumber").IsOptional();
            Property(x => x.Command).HasColumnName("Command").IsOptional().HasMaxLength(100);
            Property(x => x.ExpectedResponse).HasColumnName("ExpectedResponse").IsOptional().HasMaxLength(100);
            Property(x => x.Timeout).HasColumnName("Timeout").IsOptional();
        }
    }

    // EquipmentSettings
    internal class EquipmentSettingConfiguration : EntityTypeConfiguration<EquipmentSetting>
    {
        public EquipmentSettingConfiguration(string schema = "test")
        {
            ToTable(schema + ".EquipmentSettings");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.ItemType).HasColumnName("ItemType").IsRequired();
            Property(x => x.MaxThreads).HasColumnName("MaxThreads").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();
        }
    }

    // EquipmentSettings_VW
    internal class EquipmentSettingsVwConfiguration : EntityTypeConfiguration<EquipmentSettingsVw>
    {
        public EquipmentSettingsVwConfiguration(string schema = "test")
        {
            ToTable(schema + ".EquipmentSettings_VW");
            HasKey(x => new { x.CompanyName, x.EquipmentAuthenticationType, x.EquipmentConnectionType, x.EquipmentEncodingType, x.EquipmentId, x.EquipmentIpVersionType, x.EquipmentName, x.EquipmentType });

            Property(x => x.EquipmentName).HasColumnName("EquipmentName").IsRequired().HasMaxLength(50);
            Property(x => x.EquipmentId).HasColumnName("EquipmentId").IsRequired();
            Property(x => x.CompanyName).HasColumnName("CompanyName").IsRequired().HasMaxLength(50);
            Property(x => x.EquipmentType).HasColumnName("EquipmentType").IsRequired().HasMaxLength(50);
            Property(x => x.EquipmentConnectionType).HasColumnName("EquipmentConnectionType").IsRequired().HasMaxLength(50);
            Property(x => x.EquipmentEncodingType).HasColumnName("EquipmentEncodingType").IsRequired().HasMaxLength(50);
            Property(x => x.Url).HasColumnName("Url").IsOptional().HasMaxLength(100);
            Property(x => x.Ip).HasColumnName("Ip").IsOptional().HasMaxLength(20);
            Property(x => x.EquipmentIpVersionType).HasColumnName("EquipmentIpVersionType").IsRequired().HasMaxLength(50);
            Property(x => x.Port).HasColumnName("Port").IsOptional();
            Property(x => x.EquipmentAuthenticationType).HasColumnName("EquipmentAuthenticationType").IsRequired().HasMaxLength(50);
            Property(x => x.Username).HasColumnName("Username").IsOptional().HasMaxLength(20);
            Property(x => x.Password).HasColumnName("Password").IsOptional().HasMaxLength(20);
            Property(x => x.ShowTelnetCodes).HasColumnName("ShowTelnetCodes").IsOptional();
            Property(x => x.RemoveNonPrintableChars).HasColumnName("RemoveNonPrintableChars").IsOptional();
            Property(x => x.ReplaceNonPrintableChars).HasColumnName("ReplaceNonPrintableChars").IsOptional();
            Property(x => x.CustomBool1).HasColumnName("CustomBool1").IsOptional();
            Property(x => x.CustomString1).HasColumnName("CustomString1").IsOptional().HasMaxLength(200);
            Property(x => x.CustomInt1).HasColumnName("CustomInt1").IsOptional();
            Property(x => x.CustomBool2).HasColumnName("CustomBool2").IsOptional();
            Property(x => x.CustomString2).HasColumnName("CustomString2").IsOptional().HasMaxLength(200);
            Property(x => x.CustomInt2).HasColumnName("CustomInt2").IsOptional();
            Property(x => x.CustomBool3).HasColumnName("CustomBool3").IsOptional();
            Property(x => x.CustomString3).HasColumnName("CustomString3").IsOptional().HasMaxLength(200);
            Property(x => x.CustomInt3).HasColumnName("CustomInt3").IsOptional();
        }
    }

    // EquipmentSettingsX
    internal class EquipmentSettingsXConfiguration : EntityTypeConfiguration<EquipmentSettingsX>
    {
        public EquipmentSettingsXConfiguration(string schema = "test")
        {
            ToTable(schema + ".EquipmentSettingsX");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.EquipmentSettingsId).HasColumnName("EquipmentSettingsId").IsRequired();
            Property(x => x.ItemType).HasColumnName("ItemType").IsRequired();
            Property(x => x.MaxThreads).HasColumnName("MaxThreads").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();
            Property(x => x.RecordModified).HasColumnName("RecordModified").IsRequired();
            Property(x => x.Action).HasColumnName("Action").IsRequired().HasMaxLength(1);
        }
    }

    // EquipmentSetup
    internal class EquipmentSetupConfiguration : EntityTypeConfiguration<EquipmentSetup>
    {
        public EquipmentSetupConfiguration(string schema = "test")
        {
            ToTable(schema + ".EquipmentSetup");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(x => x.EquipmentTypeId).HasColumnName("EquipmentTypeId").IsRequired();
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.EquipmentConnectionSettingsId).HasColumnName("EquipmentConnectionSettingsId").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();
            Property(x => x.EquipmentVersion).HasColumnName("EquipmentVersion").IsOptional();

            // Foreign keys
            HasRequired(a => a.Company).WithMany(b => b.EquipmentSetups).HasForeignKey(c => c.CompanyId); // FK_test_EquipmentSetup_Companies
            HasRequired(a => a.EquipmentConnectionSetting).WithMany(b => b.EquipmentSetups).HasForeignKey(c => c.EquipmentConnectionSettingsId); // FK_test_EquipmentSetup_EquipmentConnectionSettings
        }
    }

    // EquipmentSetupX
    internal class EquipmentSetupXConfiguration : EntityTypeConfiguration<EquipmentSetupX>
    {
        public EquipmentSetupXConfiguration(string schema = "test")
        {
            ToTable(schema + ".EquipmentSetupX");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.EquipmentSetupId).HasColumnName("EquipmentSetupId").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            Property(x => x.EquipmentTypeId).HasColumnName("EquipmentTypeId").IsRequired();
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.EquipmentConnectionSettingsId).HasColumnName("EquipmentConnectionSettingsId").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();
            Property(x => x.EquipmentVersion).HasColumnName("EquipmentVersion").IsOptional();
            Property(x => x.RecordModified).HasColumnName("RecordModified").IsRequired();
            Property(x => x.Action).HasColumnName("Action").IsRequired().HasMaxLength(1);
        }
    }

    // EquipmentX
    internal class EquipmentXConfiguration : EntityTypeConfiguration<EquipmentX>
    {
        public EquipmentXConfiguration(string schema = "test")
        {
            ToTable(schema + ".EquipmentX");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.EquipmentId).HasColumnName("EquipmentId").IsRequired();
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
            Property(x => x.RecordModified).HasColumnName("RecordModified").IsRequired();
            Property(x => x.Action).HasColumnName("Action").IsRequired().HasMaxLength(1);
        }
    }

    // UsocToCommandTranslation
    internal class UsocToCommandTranslationConfiguration : EntityTypeConfiguration<UsocToCommandTranslation>
    {
        public UsocToCommandTranslationConfiguration(string schema = "test")
        {
            ToTable(schema + ".UsocToCommandTranslation");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.EquipmentId).HasColumnName("EquipmentId").IsRequired();
            Property(x => x.UsocName).HasColumnName("UsocName").IsRequired().HasMaxLength(100);
            Property(x => x.AddCommand).HasColumnName("AddCommand").IsRequired().HasMaxLength(500);
            Property(x => x.DeleteCommand).HasColumnName("DeleteCommand").IsRequired().HasMaxLength(500);
            Property(x => x.Active).HasColumnName("Active").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();

            // Foreign keys
            HasRequired(a => a.Company).WithMany(b => b.UsocToCommandTranslations).HasForeignKey(c => c.CompanyId); // FK_test_UsocToCommandTranslation_Companies
            HasRequired(a => a.EquipmentSetup).WithMany(b => b.UsocToCommandTranslations).HasForeignKey(c => c.EquipmentId); // FK_test_UsocToCommandTranslation_Equipment
        }
    }

}

