

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

namespace ANDP.Lib.Data.Repositories.Engine
{
    // ************************************************************************
    // Unit of work
    public interface IANDP_Engine_Entities : IDisposable
    {
        IDbSet<Company> Companies { get; set; } // Companies
        IDbSet<ProvisioningEngineItemActionTypesSetting> ProvisioningEngineItemActionTypesSettings { get; set; } // ProvisioningEngineItemActionTypesSettings
        IDbSet<ProvisioningEngineOrderOrServiceActionTypesSetting> ProvisioningEngineOrderOrServiceActionTypesSettings { get; set; } // ProvisioningEngineOrderOrServiceActionTypesSettings
        IDbSet<ProvisioningEngineSchedule> ProvisioningEngineSchedules { get; set; } // ProvisioningEngineSchedules
        IDbSet<ProvisioningEngineSetting> ProvisioningEngineSettings { get; set; } // ProvisioningEngineSettings

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
    public class ANDP_Engine_Entities : CommonDbContext, IANDP_Engine_Entities, IDbModelCacheKeyProvider
    {
        public IDbSet<Company> Companies { get; set; } // Companies
        public IDbSet<ProvisioningEngineItemActionTypesSetting> ProvisioningEngineItemActionTypesSettings { get; set; } // ProvisioningEngineItemActionTypesSettings
        public IDbSet<ProvisioningEngineOrderOrServiceActionTypesSetting> ProvisioningEngineOrderOrServiceActionTypesSettings { get; set; } // ProvisioningEngineOrderOrServiceActionTypesSettings
        public IDbSet<ProvisioningEngineSchedule> ProvisioningEngineSchedules { get; set; } // ProvisioningEngineSchedules
        public IDbSet<ProvisioningEngineSetting> ProvisioningEngineSettings { get; set; } // ProvisioningEngineSettings

        static ANDP_Engine_Entities()
        {
            Database.SetInitializer<ANDP_Engine_Entities>(null);
        }

        public ANDP_Engine_Entities()
            : base("Name=ANDP_Entities")
        {
        }

        public ANDP_Engine_Entities(string connectionString) : base(connectionString)
        {
        }

		public ANDP_Engine_Entities(string connectionString, string schema) : base(connectionString)
        {
            Database.SetInitializer<ANDP_Engine_Entities>(null);
            this.SchemaName = schema;
        }

        public ANDP_Engine_Entities(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model) : base(connectionString, model)
        {
        }

		public string SchemaName { get; private set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new CompanyConfiguration()
                : new CompanyConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new ProvisioningEngineItemActionTypesSettingConfiguration()
                : new ProvisioningEngineItemActionTypesSettingConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new ProvisioningEngineOrderOrServiceActionTypesSettingConfiguration()
                : new ProvisioningEngineOrderOrServiceActionTypesSettingConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new ProvisioningEngineScheduleConfiguration()
                : new ProvisioningEngineScheduleConfiguration(this.SchemaName));

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new ProvisioningEngineSettingConfiguration()
                : new ProvisioningEngineSettingConfiguration(this.SchemaName));

        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new CompanyConfiguration(schema));
            modelBuilder.Configurations.Add(new ProvisioningEngineItemActionTypesSettingConfiguration(schema));
            modelBuilder.Configurations.Add(new ProvisioningEngineOrderOrServiceActionTypesSettingConfiguration(schema));
            modelBuilder.Configurations.Add(new ProvisioningEngineScheduleConfiguration(schema));
            modelBuilder.Configurations.Add(new ProvisioningEngineSettingConfiguration(schema));
            return modelBuilder;
        }

		public string CacheKey { get { return this.SchemaName; } }

    }

    // ************************************************************************
    // POCO classes

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
        public virtual ICollection<ProvisioningEngineSetting> ProvisioningEngineSettings { get; set; } // ProvisioningEngineSettings.FK_test_ProvisioningEngineSettings_Companies

        public Company()
        {
            ProvisioningEngineSettings = new List<ProvisioningEngineSetting>();
        }
    }

    // ProvisioningEngineItemActionTypesSettings
    public class ProvisioningEngineItemActionTypesSetting
    {
        public int Id { get; set; } // Id (Primary key)
        public int ProvisioningEngineSettingsId { get; set; } // ProvisioningEngineSettingsId
        public int ActionTypeEnumId { get; set; } // ActionTypeEnumId
        public int ItemTypeEnumId { get; set; } // ItemTypeEnumId
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version

        // Foreign keys
        public virtual ProvisioningEngineSetting ProvisioningEngineSetting { get; set; } // FK_test_ProvisioningEngineItemActionTypesSettings_ProvisioningEngineSettings
    }

    // ProvisioningEngineOrderOrServiceActionTypesSettings
    public class ProvisioningEngineOrderOrServiceActionTypesSetting
    {
        public int Id { get; set; } // Id (Primary key)
        public int ProvisioningEngineSettingsId { get; set; } // ProvisioningEngineSettingsId
        public int ActionTypeEnumId { get; set; } // ActionTypeEnumId
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version

        // Foreign keys
        public virtual ProvisioningEngineSetting ProvisioningEngineSetting { get; set; } // FK_test_ProvisioningEngineOrderOrServiceActionTypesSettings_ProvisioningEngineSettings
    }

    // ProvisioningEngineSchedules
    public class ProvisioningEngineSchedule
    {
        public int Id { get; set; } // Id (Primary key)
        public int ProvisioningEngineSettingsId { get; set; } // ProvisioningEngineSettingsId
        public bool Active { get; set; } // Active
        public string Name { get; set; } // Name
        public bool Sunday { get; set; } // Sunday
        public TimeSpan SundayStartTime { get; set; } // SundayStartTime
        public TimeSpan SundayEndtime { get; set; } // SundayEndtime
        public bool Monday { get; set; } // Monday
        public TimeSpan MondayStartTime { get; set; } // MondayStartTime
        public TimeSpan MondayEndtime { get; set; } // MondayEndtime
        public bool Tuesday { get; set; } // Tuesday
        public TimeSpan TuesdayStartTime { get; set; } // TuesdayStartTime
        public TimeSpan TuesdayEndtime { get; set; } // TuesdayEndtime
        public bool Wednesday { get; set; } // Wednesday
        public TimeSpan WednesdayStartTime { get; set; } // WednesdayStartTime
        public TimeSpan WednesdayEndtime { get; set; } // WednesdayEndtime
        public bool Thursday { get; set; } // Thursday
        public TimeSpan ThursdayStartTime { get; set; } // ThursdayStartTime
        public TimeSpan ThursdayEndtime { get; set; } // ThursdayEndtime
        public bool Friday { get; set; } // Friday
        public TimeSpan FridayStartTime { get; set; } // FridayStartTime
        public TimeSpan FridayEndtime { get; set; } // FridayEndtime
        public bool Saturday { get; set; } // Saturday
        public TimeSpan SaturdayStartTime { get; set; } // SaturdayStartTime
        public TimeSpan SaturdayEndtime { get; set; } // SaturdayEndtime
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version

        // Foreign keys
        public virtual ProvisioningEngineSetting ProvisioningEngineSetting { get; set; } // FK_test_ProvisioningEngineSchedules_ProvisioningEngineSettings
    }

    // ProvisioningEngineSettings
    public class ProvisioningEngineSetting
    {
        public int Id { get; set; } // Id (Primary key)
        public int CompanyId { get; set; } // CompanyId
        public string ScriptName { get; set; } // ScriptName
        public bool LoadBalancingActive { get; set; } // LoadBalancingActive
        public bool FailOverActive { get; set; } // FailOverActive
        public int ProvisioningInterval { get; set; } // ProvisioningInterval
        public int MaxThreadsPerDispatcher { get; set; } // MaxThreadsPerDispatcher
        public int ProvisionByMethodTypeId { get; set; } // ProvisionByMethodTypeId
        public bool ProvisioningPaused { get; set; } // ProvisioningPaused
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version

        // Reverse navigation
        public virtual ICollection<ProvisioningEngineItemActionTypesSetting> ProvisioningEngineItemActionTypesSettings { get; set; } // ProvisioningEngineItemActionTypesSettings.FK_test_ProvisioningEngineItemActionTypesSettings_ProvisioningEngineSettings
        public virtual ICollection<ProvisioningEngineOrderOrServiceActionTypesSetting> ProvisioningEngineOrderOrServiceActionTypesSettings { get; set; } // ProvisioningEngineOrderOrServiceActionTypesSettings.FK_test_ProvisioningEngineOrderOrServiceActionTypesSettings_ProvisioningEngineSettings
        public virtual ICollection<ProvisioningEngineSchedule> ProvisioningEngineSchedules { get; set; } // ProvisioningEngineSchedules.FK_test_ProvisioningEngineSchedules_ProvisioningEngineSettings

        // Foreign keys
        public virtual Company Company { get; set; } // FK_test_ProvisioningEngineSettings_Companies

        public ProvisioningEngineSetting()
        {
            ProvisioningEngineItemActionTypesSettings = new List<ProvisioningEngineItemActionTypesSetting>();
            ProvisioningEngineOrderOrServiceActionTypesSettings = new List<ProvisioningEngineOrderOrServiceActionTypesSetting>();
            ProvisioningEngineSchedules = new List<ProvisioningEngineSchedule>();
        }
    }

 

    // ************************************************************************
    // Enums
	
		
	public enum ProvisionByMethodTypeEnum
	{
		Order = 1,
		Service = 2,
		Item = 3,
		Equipment = 4,
	}



    // ************************************************************************
    // POCO Configuration

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

    // ProvisioningEngineItemActionTypesSettings
    internal class ProvisioningEngineItemActionTypesSettingConfiguration : EntityTypeConfiguration<ProvisioningEngineItemActionTypesSetting>
    {
        public ProvisioningEngineItemActionTypesSettingConfiguration(string schema = "test")
        {
            ToTable(schema + ".ProvisioningEngineItemActionTypesSettings");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.ProvisioningEngineSettingsId).HasColumnName("ProvisioningEngineSettingsId").IsRequired();
            Property(x => x.ActionTypeEnumId).HasColumnName("ActionTypeEnumId").IsRequired();
            Property(x => x.ItemTypeEnumId).HasColumnName("ItemTypeEnumId").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();

            // Foreign keys
            HasRequired(a => a.ProvisioningEngineSetting).WithMany(b => b.ProvisioningEngineItemActionTypesSettings).HasForeignKey(c => c.ProvisioningEngineSettingsId); // FK_test_ProvisioningEngineItemActionTypesSettings_ProvisioningEngineSettings
        }
    }

    // ProvisioningEngineOrderOrServiceActionTypesSettings
    internal class ProvisioningEngineOrderOrServiceActionTypesSettingConfiguration : EntityTypeConfiguration<ProvisioningEngineOrderOrServiceActionTypesSetting>
    {
        public ProvisioningEngineOrderOrServiceActionTypesSettingConfiguration(string schema = "test")
        {
            ToTable(schema + ".ProvisioningEngineOrderOrServiceActionTypesSettings");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.ProvisioningEngineSettingsId).HasColumnName("ProvisioningEngineSettingsId").IsRequired();
            Property(x => x.ActionTypeEnumId).HasColumnName("ActionTypeEnumId").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();

            // Foreign keys
            HasRequired(a => a.ProvisioningEngineSetting).WithMany(b => b.ProvisioningEngineOrderOrServiceActionTypesSettings).HasForeignKey(c => c.ProvisioningEngineSettingsId); // FK_test_ProvisioningEngineOrderOrServiceActionTypesSettings_ProvisioningEngineSettings
        }
    }

    // ProvisioningEngineSchedules
    internal class ProvisioningEngineScheduleConfiguration : EntityTypeConfiguration<ProvisioningEngineSchedule>
    {
        public ProvisioningEngineScheduleConfiguration(string schema = "test")
        {
            ToTable(schema + ".ProvisioningEngineSchedules");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.ProvisioningEngineSettingsId).HasColumnName("ProvisioningEngineSettingsId").IsRequired();
            Property(x => x.Active).HasColumnName("Active").IsRequired();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(100);
            Property(x => x.Sunday).HasColumnName("Sunday").IsRequired();
            Property(x => x.SundayStartTime).HasColumnName("SundayStartTime").IsRequired();
            Property(x => x.SundayEndtime).HasColumnName("SundayEndtime").IsRequired();
            Property(x => x.Monday).HasColumnName("Monday").IsRequired();
            Property(x => x.MondayStartTime).HasColumnName("MondayStartTime").IsRequired();
            Property(x => x.MondayEndtime).HasColumnName("MondayEndtime").IsRequired();
            Property(x => x.Tuesday).HasColumnName("Tuesday").IsRequired();
            Property(x => x.TuesdayStartTime).HasColumnName("TuesdayStartTime").IsRequired();
            Property(x => x.TuesdayEndtime).HasColumnName("TuesdayEndtime").IsRequired();
            Property(x => x.Wednesday).HasColumnName("Wednesday").IsRequired();
            Property(x => x.WednesdayStartTime).HasColumnName("WednesdayStartTime").IsRequired();
            Property(x => x.WednesdayEndtime).HasColumnName("WednesdayEndtime").IsRequired();
            Property(x => x.Thursday).HasColumnName("Thursday").IsRequired();
            Property(x => x.ThursdayStartTime).HasColumnName("ThursdayStartTime").IsRequired();
            Property(x => x.ThursdayEndtime).HasColumnName("ThursdayEndtime").IsRequired();
            Property(x => x.Friday).HasColumnName("Friday").IsRequired();
            Property(x => x.FridayStartTime).HasColumnName("FridayStartTime").IsRequired();
            Property(x => x.FridayEndtime).HasColumnName("FridayEndtime").IsRequired();
            Property(x => x.Saturday).HasColumnName("Saturday").IsRequired();
            Property(x => x.SaturdayStartTime).HasColumnName("SaturdayStartTime").IsRequired();
            Property(x => x.SaturdayEndtime).HasColumnName("SaturdayEndtime").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();

            // Foreign keys
            HasRequired(a => a.ProvisioningEngineSetting).WithMany(b => b.ProvisioningEngineSchedules).HasForeignKey(c => c.ProvisioningEngineSettingsId); // FK_test_ProvisioningEngineSchedules_ProvisioningEngineSettings
        }
    }

    // ProvisioningEngineSettings
    internal class ProvisioningEngineSettingConfiguration : EntityTypeConfiguration<ProvisioningEngineSetting>
    {
        public ProvisioningEngineSettingConfiguration(string schema = "test")
        {
            ToTable(schema + ".ProvisioningEngineSettings");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.ScriptName).HasColumnName("ScriptName").IsRequired().HasMaxLength(100);
            Property(x => x.LoadBalancingActive).HasColumnName("LoadBalancingActive").IsRequired();
            Property(x => x.FailOverActive).HasColumnName("FailOverActive").IsRequired();
            Property(x => x.ProvisioningInterval).HasColumnName("ProvisioningInterval").IsRequired();
            Property(x => x.MaxThreadsPerDispatcher).HasColumnName("MaxThreadsPerDispatcher").IsRequired();
            Property(x => x.ProvisionByMethodTypeId).HasColumnName("ProvisionByMethodTypeId").IsRequired();
            Property(x => x.ProvisioningPaused).HasColumnName("ProvisioningPaused").IsRequired();
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();

            // Foreign keys
            HasRequired(a => a.Company).WithMany(b => b.ProvisioningEngineSettings).HasForeignKey(c => c.CompanyId); // FK_test_ProvisioningEngineSettings_Companies
        }
    }

}

