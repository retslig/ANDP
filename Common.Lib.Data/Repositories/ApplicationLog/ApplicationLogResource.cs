

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

namespace Common.Lib.Data.Repositories.ApplicationLog
{
    // ************************************************************************
    // Unit of work
    public interface ICommon_ApplicationLog_Entities : IDisposable
    {
        IDbSet<ApplicationLog> ApplicationLogs { get; set; } // ApplicationLogs

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
    public class Common_ApplicationLog_Entities : CommonDbContext, ICommon_ApplicationLog_Entities
    {
        public IDbSet<ApplicationLog> ApplicationLogs { get; set; } // ApplicationLogs

        static Common_ApplicationLog_Entities()
        {
            Database.SetInitializer<Common_ApplicationLog_Entities>(null);
        }

        public Common_ApplicationLog_Entities()
            : base("Name=ANDP_Entities")
        {
        }

        public Common_ApplicationLog_Entities(string connectionString) : base(connectionString)
        {
        }

        public Common_ApplicationLog_Entities(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model) : base(connectionString, model)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new ApplicationLogConfiguration());
        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new ApplicationLogConfiguration(schema));
            return modelBuilder;
        }
    }

    // ************************************************************************
    // POCO classes

    // ApplicationLogs
    public class ApplicationLog
    {
        public int LogId { get; set; } // LogID (Primary key)
        public string TenantId { get; set; } // TenantId
        public Guid? SearchKey { get; set; } // SearchKey
        public string SourceMachineName { get; set; } // SourceMachineName
        public string AppCode { get; set; } // AppCode
        public int Pid { get; set; } // PID
        public string UserCode { get; set; } // UserCode
        public DateTime LoggedDateTime { get; set; } // LoggedDateTime
        public string MessageType { get; set; } // MessageType
        public string MessageData { get; set; } // MessageData
        public string StackTrace { get; set; } // StackTrace
        public string ExceptionMessage { get; set; } // ExceptionMessage
        public string DestinationMachineName { get; set; } // DestinationMachineName
    }

 

    // ************************************************************************
    // Enums
	
		
	


    // ************************************************************************
    // POCO Configuration

    // ApplicationLogs
    internal class ApplicationLogConfiguration : EntityTypeConfiguration<ApplicationLog>
    {
        public ApplicationLogConfiguration(string schema = "common")
        {
            ToTable(schema + ".ApplicationLogs");
            HasKey(x => x.LogId);

            Property(x => x.LogId).HasColumnName("LogID").IsRequired();
            Property(x => x.TenantId).HasColumnName("TenantId").IsOptional().HasMaxLength(50);
            Property(x => x.SearchKey).HasColumnName("SearchKey").IsOptional();
            Property(x => x.SourceMachineName).HasColumnName("SourceMachineName").IsOptional().HasMaxLength(50);
            Property(x => x.AppCode).HasColumnName("AppCode").IsRequired().HasMaxLength(50);
            Property(x => x.Pid).HasColumnName("PID").IsRequired();
            Property(x => x.UserCode).HasColumnName("UserCode").IsRequired().HasMaxLength(20);
            Property(x => x.LoggedDateTime).HasColumnName("LoggedDateTime").IsRequired();
            Property(x => x.MessageType).HasColumnName("MessageType").IsRequired().HasMaxLength(20);
            Property(x => x.MessageData).HasColumnName("MessageData").IsOptional();
            Property(x => x.StackTrace).HasColumnName("StackTrace").IsOptional().HasMaxLength(2000);
            Property(x => x.ExceptionMessage).HasColumnName("ExceptionMessage").IsOptional().HasMaxLength(2000);
            Property(x => x.DestinationMachineName).HasColumnName("DestinationMachineName").IsOptional().HasMaxLength(50);
        }
    }

}

