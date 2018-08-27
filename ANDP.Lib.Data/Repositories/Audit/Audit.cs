

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

namespace ANDP.Lib.Data.Repositories.Audit
{
    // ************************************************************************
    // Unit of work
    public interface IANDP_Audit_Entities : IDisposable
    {
        IDbSet<AuditRecord> AuditRecords { get; set; } // AuditRecords

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
    public class ANDP_Audit_Entities : CommonDbContext, IANDP_Audit_Entities, IDbModelCacheKeyProvider
    {
        public IDbSet<AuditRecord> AuditRecords { get; set; } // AuditRecords

        static ANDP_Audit_Entities()
        {
            Database.SetInitializer<ANDP_Audit_Entities>(null);
        }

        public ANDP_Audit_Entities()
            : base("Name=ANDP_Entities")
        {
        }

        public ANDP_Audit_Entities(string connectionString) : base(connectionString)
        {
        }

		public ANDP_Audit_Entities(string connectionString, string schema) : base(connectionString)
        {
            Database.SetInitializer<ANDP_Audit_Entities>(null);
            this.SchemaName = schema;
        }

        public ANDP_Audit_Entities(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model) : base(connectionString, model)
        {
        }

		public string SchemaName { get; private set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(string.IsNullOrEmpty(this.SchemaName)
                ? new AuditRecordConfiguration()
                : new AuditRecordConfiguration(this.SchemaName));

        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new AuditRecordConfiguration(schema));
            return modelBuilder;
        }

		public string CacheKey { get { return this.SchemaName; } }

    }

    // ************************************************************************
    // POCO classes

    // AuditRecords
    public class AuditRecord
    {
        public int Id { get; set; } // Id (Primary key)
        public Guid RunNumber { get; set; } // RunNumber
        public DateTime RunDate { get; set; } // RunDate
        public int CompanyId { get; set; } // CompanyId
        public int EquipmentSetupId { get; set; } // EquipmentSetupId
        public bool BillingOrEquipmentIndicator { get; set; } // BillingOrEquipmentIndicator
        public string ExternalAccountId { get; set; } // ExternalAccountId
        public string ExternalServiceId { get; set; } // ExternalServiceId
        public string ExternalItemId { get; set; } // ExternalItemId
        public int ItemTypeId { get; set; } // ItemTypeId
        public string RecordKey { get; set; } // RecordKey
        public string RecordType { get; set; } // RecordType
        public string RecordValue { get; set; } // RecordValue
        public bool? Ignore { get; set; } // Ignore
        public Guid? MatchId { get; set; } // MatchId
        public bool? AddToEquiment { get; set; } // AddToEquiment
        public bool? AddToBilling { get; set; } // AddToBilling
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public string CreatedByUser { get; set; } // CreatedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version
    }

 

    // ************************************************************************
    // Enums
	
		
	


    // ************************************************************************
    // POCO Configuration

    // AuditRecords
    internal class AuditRecordConfiguration : EntityTypeConfiguration<AuditRecord>
    {
        public AuditRecordConfiguration(string schema = "test")
        {
            ToTable(schema + ".AuditRecords");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.RunNumber).HasColumnName("RunNumber").IsRequired();
            Property(x => x.RunDate).HasColumnName("RunDate").IsRequired();
            Property(x => x.CompanyId).HasColumnName("CompanyId").IsRequired();
            Property(x => x.EquipmentSetupId).HasColumnName("EquipmentSetupId").IsRequired();
            Property(x => x.BillingOrEquipmentIndicator).HasColumnName("BillingOrEquipmentIndicator").IsRequired();
            Property(x => x.ExternalAccountId).HasColumnName("ExternalAccountId").IsOptional().HasMaxLength(36);
            Property(x => x.ExternalServiceId).HasColumnName("ExternalServiceId").IsOptional().HasMaxLength(36);
            Property(x => x.ExternalItemId).HasColumnName("ExternalItemId").IsOptional().HasMaxLength(36);
            Property(x => x.ItemTypeId).HasColumnName("ItemTypeId").IsOptional();
            Property(x => x.RecordKey).HasColumnName("RecordKey").IsRequired().HasMaxLength(50);
            Property(x => x.RecordType).HasColumnName("RecordType").IsRequired().HasMaxLength(50);
            Property(x => x.RecordValue).HasColumnName("RecordValue").IsRequired().HasMaxLength(200);
            Property(x => x.Ignore).HasColumnName("Ignore").IsOptional();
            Property(x => x.MatchId).HasColumnName("MatchId").IsOptional();
            Property(x => x.AddToEquiment).HasColumnName("AddToEquiment").IsOptional();
            Property(x => x.AddToBilling).HasColumnName("AddToBilling").IsOptional();
            Property(x => x.ModifiedByUser).HasColumnName("ModifiedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.CreatedByUser).HasColumnName("CreatedByUser").IsRequired().HasMaxLength(20);
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();
        }
    }

}

