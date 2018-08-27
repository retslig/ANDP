
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace Common.Lib.EntityProvider
{
    public abstract class CommonDbContext : DbContext
    {
        
        public string ConnectionString
        {
            get { return _connectionString; }
        }

        private readonly string _connectionString;
        private int _counter = 0;
        private int _resetLimit = 100;
        public int ResetLimit
        {
            get { return _resetLimit; }
            set { _resetLimit = value; }
        }

        protected CommonDbContext()
        {
        }

        protected CommonDbContext(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        protected CommonDbContext(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model)
        {
        }

        protected CommonDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        protected CommonDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }

        /// <summary>
        /// Saves the changes and removes specified entity from context if set to true.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="removeSpecifiedEntities">if set to <c>true</c> [remove specified entities].</param>
        /// <returns></returns>
        public int SaveChanges<TEntity>(bool removeSpecifiedEntities)
        {
            _counter++;
            int results = base.SaveChanges();

            if (removeSpecifiedEntities && _counter >= _resetLimit)
                ClearContextEntries<TEntity>();

            return results;
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="removeAllEntities">if set to <c>true</c> [remove all entities].</param>
        /// <returns></returns>
        public int SaveChanges(bool removeAllEntities)
        {
            _counter++;
            int results = base.SaveChanges();

            if (removeAllEntities && _counter >= _resetLimit)
                ClearContextEntries();

            return results;
        }

        /// <summary>
        /// Clears the context entries.
        /// </summary>
        public void ClearContextEntries<TEntity>()
        {
            var context = ((IObjectContextAdapter)this).ObjectContext;
            var objectStateEntries = (from entry in context.ObjectStateManager.GetObjectStateEntries(
                                        EntityState.Added
                                        | EntityState.Deleted
                                        | EntityState.Modified
                                        | EntityState.Unchanged)
                                      where entry.EntityKey != null && entry.Entity.GetType() == typeof(TEntity)
                                      select entry.Entity);

            foreach (var objectStateEntry in objectStateEntries)
            {
                context.Detach(objectStateEntry);
            }

            _counter = 0;
        }

        /// <summary>
        /// Clears the context entries.
        /// </summary>
        public void ClearContextEntries()
        {
            var context = ((IObjectContextAdapter)this).ObjectContext;
            var objectStateEntries = (from entry in context.ObjectStateManager.GetObjectStateEntries(
                                        EntityState.Added
                                        | EntityState.Deleted
                                        | EntityState.Modified
                                        | EntityState.Unchanged)
                                        where entry.EntityKey != null
                                        select entry.Entity);

            foreach (var objectStateEntry in objectStateEntries)
            {
                context.Detach(objectStateEntry);
            }

            _counter = 0;
        }

        public override int SaveChanges()
        {
            //foreach (var dbEntityEntry in ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified))
            //{
            //    object entity = dbEntityEntry.Entity;
            //    PropertyInfo pi = entity.GetType().GetProperty("Version");
            //    if (pi != null)
            //    {
            //        int version = Convert.ToInt32(pi.GetValue(entity, null));

            //        switch (dbEntityEntry.State)
            //        {
            //            case EntityState.Added:
            //                pi.SetValue(dbEntityEntry.Entity, 1, null);
            //                break;
            //            case EntityState.Modified:
            //                pi.SetValue(dbEntityEntry.Entity, version + 1, null);
            //                break;
            //        }
            //    }
            //}

            return base.SaveChanges();
        }

        /// <summary>
        /// Refreshes the entire context using the store.
        /// </summary>
        public void RefreshAll()
        {
            var context = ((IObjectContextAdapter)this).ObjectContext;
            // Get all objects in state manager with entityKey 
            // (context.Refresh will throw an exception otherwise) 
            var refreshableObjects = (from entry in context.ObjectStateManager.GetObjectStateEntries(
                                        EntityState.Added
                                        | EntityState.Deleted
                                        | EntityState.Modified
                                        | EntityState.Unchanged)
                                      where entry.EntityKey != null
                                      select entry.Entity);
                        
            context.Refresh(RefreshMode.StoreWins, refreshableObjects);
        }

        /// <summary>
        /// Refreshes the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        public void RefreshEntities<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities.Where(entity => base.Entry(entity).State == EntityState.Detached))
            {
                base.Set<TEntity>().Attach(entity);
            }

            var context = ((IObjectContextAdapter)this).ObjectContext;
            context.Refresh(RefreshMode.StoreWins, entities);
        }

        /// <summary>
        /// Refreshes the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        /// <param name="refreshMode">The refresh mode.</param>
        public void RefreshEntities<TEntity>(IEnumerable<TEntity> entities, RefreshMode refreshMode) where TEntity : class
        {
            foreach (var entity in entities.Where(entity => base.Entry(entity).State == EntityState.Detached))
            {
                base.Set<TEntity>().Attach(entity);
            }

            var context = ((IObjectContextAdapter)this).ObjectContext;
            context.Refresh(refreshMode, entities);
        }

        /// <summary>
        /// Refreshes the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void RefreshEntity<TEntity>(TEntity entity) where TEntity : class
        {
            if ( base.Entry(entity).State == EntityState.Detached)
                base.Set<TEntity>().Attach(entity);            

            var context = ((IObjectContextAdapter)this).ObjectContext;
            context.Refresh(RefreshMode.StoreWins, entity);
        }

        /// <summary>
        /// Refreshes the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="refreshMode">The refresh mode.</param>
        public void RefreshEntity<TEntity>(TEntity entity, RefreshMode refreshMode) where TEntity : class
        {
            if (base.Entry(entity).State == EntityState.Detached)
                base.Set<TEntity>().Attach(entity);

            var context = ((IObjectContextAdapter)this).ObjectContext;
            context.Refresh(refreshMode, entity);
        }

        /// <summary>
        /// Attaches the entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="keyNames">The key names.</param>
        /// <param name="userId">The user unique identifier.</param>
        /// <exception cref="System.Exception">The given key( + keyName + ) for entity( + entity.GetType().Name + ) is not found.</exception>
        public void AttachEntities<TEntity>(DbContext context, IEnumerable<TEntity> entities, IEnumerable<string> keyNames, string userId) where TEntity : class
        {
            //If you provide a new context use this else use base dbcontext.
            //this is due to bulk inserts will give new context so it can get better performance from not tracking entities.
            foreach (var entity in entities)
            {
                AttachEntity(context, entity, keyNames, userId);
            }
        }
        
        //TODO: Clean up below.
        ///// <summary>
        ///// Attaches the entities automatic parent entity.
        ///// </summary>
        ///// <typeparam name="TEntity">The type of the entity.</typeparam>
        ///// <param name="context">The context.</param>
        ///// <param name="entities">The entities.</param>
        ///// <param name="keyName">Name of the key.</param>
        ///// <param name="userId">The user unique identifier.</param>
        //public void AttachEntitiesToParentEntity<TEntity>(DbContext context, IEnumerable<TEntity> entities, string keyName, string userId) where TEntity : class
        //{
        //    //If you provide a new context use this else use base dbcontext.
        //    //this is due to bulk inserts will give new context so it can get better performance from not tracking entities.
        //    foreach (var entity in entities)
        //    {
        //        AttachEntityToParentEntity(context, entity, keyName, userId);
        //    }
        //}

        ///// <summary>
        ///// Attaches the entity automatic parent entity.
        ///// </summary>
        ///// <typeparam name="TParentEntity">The type of the parent entity.</typeparam>
        ///// <typeparam name="TEntity">The type of the entity.</typeparam>
        ///// <param name="context">The context.</param>
        ///// <param name="parentEntity">The parent entity.</param>
        ///// <param name="entity">The entity.</param>
        ///// <param name="keyName">Name of the key.</param>
        ///// <param name="userId">The user unique identifier.</param>
        //public void AttachEntityToParentEntity<TParentEntity, TEntity>(DbContext context, TParentEntity parentEntity, TEntity entity, string keyName, string userId) where TEntity : class
        //{
        //    //1- Get fresh data from database
        //    var existingStudent = context.Students.AsNoTracking().Include(s => s.Standard).Include(s => s.Standard.Teachers).Where(s => s.StudentName == "updated student").FirstOrDefault<Student>();

        //    var existingTeachers = existingStudent.Standard.Teachers.ToList<Teacher>();

        //    var updatedTeachers = teachers.ToList<Teacher>();

        //    //2- Find newly added teachers by updatedTeachers (teacher came from client sided) - existingTeacher = newly added teacher
        //    var addedTeachers = updatedTeachers.Except(existingTeachers, tchr => tchr.TeacherId);

        //    //3- Find deleted teachers by existing teachers - updatedTeachers = deleted teachers
        //    var deletedTeachers = existingTeachers.Except(updatedTeachers, tchr => tchr.TeacherId);

        //    //4- Find modified teachers by updatedTeachers - addedTeachers = modified teachers
        //    var modifiedTeacher = updatedTeachers.Except(addedTeachers, tchr => tchr.TeacherId);

        //    //5- Mark all added teachers entity state to Added
        //    addedTeachers.ToList<Teacher>().ForEach(tchr => context.Entry(tchr).State = System.Data.EntityState.Added);

        //    //6- Mark all deleted teacher entity state to Deleted
        //    deletedTeachers.ToList<Teacher>().ForEach(tchr => context.Entry(tchr).State = System.Data.EntityState.Deleted);


        //    //7- Apply modified teachers current property values to existing property values
        //    foreach (Teacher teacher in modifiedTeacher)
        //    {
        //        //8- Find existing teacher by id from fresh database teachers
        //        var existingTeacher = context.Teachers.Find(teacher.TeacherId);

        //        if (existingTeacher != null)
        //        {
        //            //9- Get DBEntityEntry object for each existing teacher entity
        //            var teacherEntry = context.Entry(existingTeacher);
        //            //10- overwrite all property current values from modified teachers' entity values, 
        //            //so that it will have all modified values and mark entity as modified
        //            teacherEntry.CurrentValues.SetValues(teacher);
        //        }

        //    }
        //}

        /// <summary>
        /// Attaches the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="keyNames">The key names. This must be in the correct composite order.</param>
        /// <param name="userId">The user unique identifier.</param>
        /// <exception cref="System.Exception">The given key( + keyName + ) for entity( + entities.FirstOrDefault().GetType().Name + ) is not found.</exception>
        public void AttachEntity<TEntity>(DbContext context, TEntity entity, IEnumerable<string> keyNames, string userId) where TEntity : class
        {
            bool forceAdd = false;
            var keyValues = new List<object>();
            foreach (var keyName in keyNames)
            {
                PropertyInfo pi = entity.GetType().GetProperty(keyName);
                if (pi == null)
                    throw new Exception("The given key(" + keyName + ") for entity(" + entity.GetType().Name + ") is not found.");

                object keyValue = pi.GetValue(entity, null);
                keyValues.Add(keyValue);

                //Many times we may be adding entities that we will not have a Id as this is auto generated in the database
                //so in these cases make sure we are adding not updating. 
                forceAdd = keyName.Equals("Id", StringComparison.InvariantCultureIgnoreCase) && int.Parse(keyValue.ToString()) < 1;
            }

            var entry = context != null ? context.Entry(entity) : base.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                DbPropertyEntry property;
                var set = context != null ? context.Set<TEntity>() : base.Set<TEntity>();
                var attachedEntity = set.Find(keyValues.ToArray());
                if (attachedEntity == null || forceAdd)
                {
                    entry.State = EntityState.Added;

                    //if (entry.CurrentValues.PropertyNames.Contains("Version"))
                    if (entry.Entity.GetType().GetProperty("Version") != null)
                    {
                        property = entry.Property("Version");
                        if (property != null)
                            property.IsModified = false;
                    }

                    if (entry.Entity.GetType().GetProperty("DateCreated") != null)
                    {
                        property = entry.Property("DateCreated");
                        if (property != null)
                            property.CurrentValue = DateTime.Now;
                    }

                    if (entry.Entity.GetType().GetProperty("DateModified") != null)
                    {
                        property = entry.Property("DateModified");
                        if (property != null)
                            property.CurrentValue = DateTime.Now;
                    }

                    if (entry.Entity.GetType().GetProperty("CreatedByUser") != null)
                    {
                        property = entry.Property("CreatedByUser");
                        if (property != null)
                            property.CurrentValue = userId;
                    }

                    if (entry.Entity.GetType().GetProperty("ModifiedByUser") != null)                        
                    {
                        property = entry.Property("ModifiedByUser");
                        if (property != null)
                            property.CurrentValue = userId;
                    }
                }
                else
                {
                    var localContext = context != null
                                           ? ((IObjectContextAdapter)context).ObjectContext
                                           : ((IObjectContextAdapter)this).ObjectContext;
                    localContext.Detach(attachedEntity);
                    //You will get this error if you do not detach first.
                    //An object with the same key already exists in the ObjectStateManager. The ObjectStateManager cannot track multiple objects with the same key.
                    entry.State = EntityState.Modified;

                    if (entry.Entity.GetType().GetProperty("Version") != null)
                    {
                        property = entry.Property("Version");
                        if (property != null)
                            property.IsModified = false;
                    }

                    if (entry.Entity.GetType().GetProperty("DateCreated") != null)
                    {
                        property = entry.Property("DateCreated");
                        if (property != null)
                            property.IsModified = false;
                    }

                    if (entry.Entity.GetType().GetProperty("DateModified") != null)
                    {
                        property = entry.Property("DateModified");
                        if (property != null)
                            property.CurrentValue = DateTime.Now;
                    }

                    if (entry.Entity.GetType().GetProperty("CreatedByUser") != null)
                    {
                        property = entry.Property("CreatedByUser");
                        if (property != null)
                            property.IsModified = false;
                    }

                    if (entry.Entity.GetType().GetProperty("ModifiedByUser") != null)
                    {
                        property = entry.Property("ModifiedByUser");
                        if (property != null)
                            property.CurrentValue = userId;
                    }
                }
            }
        }

        /// <summary>
        /// Bulks the attach entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="userId">The user unique identifier.</param>
        public void BulkAttachEntities<TEntity>(IEnumerable<TEntity> entities, string keyName, string userId) where TEntity : class
        {
            //default TransactionScope closes after 60 seconds, so need to explicitly define it as longer.
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, 10, 0)))
            {
                DbContext context = null;
                try
                {
                    //This connectionString needs to be come from EntityConnectionStringBuilder, so will likely come from your Bootstrapper!
                    context = new DbContext(_connectionString);
                    //turn off change tracking in the context
                    context.Configuration.AutoDetectChangesEnabled = false;

                    PropertyInfo pi = entities.FirstOrDefault().GetType().GetProperty(keyName);
                    if (pi == null)
                        throw new Exception("The given key(" + keyName + ") for entity(" + entities.FirstOrDefault().GetType().Name + ") is not found.");

                    var count = 0;
                    foreach (var entityToInsert in entities)
                    {
                        ++count;
                        object key = pi.GetValue(entityToInsert, null);
                        context = AddToContext2(context, _connectionString, entityToInsert, new []{keyName} , userId, count, 100, true);
                    }

                    context.SaveChanges();
                }
                finally
                {
                    if (context != null)
                    {
                        context.Dispose();
                    }
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// Bulks the insert.
        /// This will NOT work for updates!!!
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The entities.</param>
        public void BulkInsert<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            //default TransactionScope closes after 60 seconds, so need to explicitly define it as longer.
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 0, 10, 0)))
            {
                CommonDbContext context = null;
                try
                {
                    //This connectionString needs to be come from EntityConnectionStringBuilder, so will likely come from your Bootstrapper!
                    context = (CommonDbContext)Activator.CreateInstance(this.GetType(), new object[] { _connectionString });

                    //turn off change tracking in the context
                    context.Configuration.AutoDetectChangesEnabled = false;

                    var count = 0;
                    foreach (var entityToInsert in entities)
                    {
                        ++count;
                        context = AddToContext(context, _connectionString, entityToInsert, count, 100, true);
                    }

                    context.SaveChanges();
                }
                finally
                {
                    if (context != null)
                    {
                        context.Dispose();
                    }
                }

                scope.Complete();
            }
        }

        private CommonDbContext AddToContext<T>(CommonDbContext context, string connectionString, T entity, int count, int commitCount, bool recreateContext) where T : class
        {
            context.Set<T>().Add(entity);

            if (count % commitCount == 0)
            {
                context.SaveChanges();
                //tearing down and recreating the context can further improve performance when dealing with thousands of records.
                //most testing finds 100 to be the sweet spot for performance, which matches my previous experience with NHibernate.
                if (recreateContext)
                {
                    context.Dispose();
                    context = (CommonDbContext)Activator.CreateInstance(this.GetType(), new object[] { _connectionString });
                    context.Configuration.AutoDetectChangesEnabled = false;
                }
            }
            
            return context;
        }

        private DbContext AddToContext2<T>(DbContext context, string connectionString, T entity, IEnumerable<string> keyNames, string userId, int count, int commitCount, bool recreateContext) where T : class
        {
            AttachEntity(context, entity, keyNames, userId);

            if (count % commitCount == 0)
            {
                context.SaveChanges();
                //tearing down and recreating the context can further improve performance when dealing with thousands of records.
                //most testing finds 100 to be the sweet spot for performance, which matches my previous experience with NHibernate.
                if (recreateContext)
                {
                    //Another consideration to be made is simply detaching the entities instead of breaking down the connection.
                    context.Dispose();
                    context = new DbContext(connectionString);
                    context.Configuration.AutoDetectChangesEnabled = false;
                }
            }

            return context;
        }
    }

    public static class CommonDbContextHelper
    {
        public static IEnumerable<T> Except<T, TKey>(this IEnumerable<T> items, IEnumerable<T> other, Func<T, TKey> getKey)
        {
            return from item in items
                   join otherItem in other on getKey(item)
                       equals getKey(otherItem) into tempItems
                   from temp in tempItems.DefaultIfEmpty()
                   where ReferenceEquals(null, temp) || temp.Equals(default(T))
                   select item;
        }
    }
}
