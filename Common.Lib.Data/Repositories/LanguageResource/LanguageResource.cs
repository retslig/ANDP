

// This file was automatically generated.
// Do not make changes directly to this file - edit the template instead.
// 
// The following connection settings were used to generate this file
// 
//     Configuration file:     "Common.Lib.Data\App.config"
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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
//using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.DatabaseGeneratedOption;

namespace Common.Lib.Data.Repositories.LanguageResource
{
    // ************************************************************************
    // Unit of work
    public interface ICommon_LanguageResource_Entities : IDisposable
    {
        IDbSet<LanguageResource> LanguageResources { get; set; } // LanguageResource

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
    public class Common_LanguageResource_Entities : CommonDbContext, ICommon_LanguageResource_Entities
    {
        public IDbSet<LanguageResource> LanguageResources { get; set; } // LanguageResource

        static Common_LanguageResource_Entities()
        {
            Database.SetInitializer<Common_LanguageResource_Entities>(null);
        }

        public Common_LanguageResource_Entities()
            : base("Name=ANDP_Entities")
        {
        }

        public Common_LanguageResource_Entities(string connectionString) : base(connectionString)
        {
        }

        public Common_LanguageResource_Entities(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model) : base(connectionString, model)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new LanguageResourceConfiguration());
        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new LanguageResourceConfiguration(schema));
            return modelBuilder;
        }
    }

    // ************************************************************************
    // POCO classes

    // LanguageResource
    public class LanguageResource
    {
        public int Id { get; set; } // Id (Primary key)
        public string ResourceType { get; set; } // ResourceType
        public string CultureCode { get; set; } // CultureCode
        public string ResourceKey { get; set; } // ResourceKey
        public string ResourceValue { get; set; } // ResourceValue
        public int CreatedById { get; set; } // CreatedById
        public int ModifiedById { get; set; } // ModifiedById
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version
    }

 

    // ************************************************************************
    // Enums
	
		
	


    // ************************************************************************
    // POCO Configuration

    // LanguageResource
    internal class LanguageResourceConfiguration : EntityTypeConfiguration<LanguageResource>
    {
        public LanguageResourceConfiguration(string schema = "common")
        {
            ToTable(schema + ".LanguageResource");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired();
            Property(x => x.ResourceType).HasColumnName("ResourceType").IsRequired().HasMaxLength(50);
            Property(x => x.CultureCode).HasColumnName("CultureCode").IsRequired().HasMaxLength(10);
            Property(x => x.ResourceKey).HasColumnName("ResourceKey").IsRequired().HasMaxLength(20);
            Property(x => x.ResourceValue).HasColumnName("ResourceValue").IsRequired();
            Property(x => x.CreatedById).HasColumnName("CreatedById").IsRequired();
            Property(x => x.ModifiedById).HasColumnName("ModifiedById").IsRequired();
            Property(x => x.DateCreated).HasColumnName("DateCreated").IsRequired();
            Property(x => x.DateModified).HasColumnName("DateModified").IsRequired();
            Property(x => x.Version).HasColumnName("Version").IsRequired().IsConcurrencyToken();
        }
    }

}

