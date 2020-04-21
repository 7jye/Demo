using IRepository;
using Microsoft.EntityFrameworkCore;
using Student.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
//using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Repository
{
    public class RepositoryBase<T> : IBaseRepository<T> where T : class
    {
        protected readonly string ConnectionString;

        private readonly IDbSet<T> _objectSet;

        public virtual IQueryable<T> Table => Entities;

        protected DemoDbContext Context;

        private DbSet<T> _entities;

        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = Context.Set<T>();

                return _entities;
            }
        }

        public RepositoryBase()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DataSource"].ConnectionString;
            //总是返回当前的datacontext
            Context = DemoDbContext.Current;

            _objectSet = Context.Set<T>();
        }

        public IQueryable<T> AsQueryable()
        {
            return _objectSet.AsQueryable();
        }

        public IEnumerable<T> GetAll()
        {
            return AsQueryable().AsEnumerable();
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _objectSet.Where(predicate);
        }

        public IEnumerable<T> GetList(Expression<Func<T, bool>> filter)
        {
            return Entities.Where(filter);
        }

        public T Single(Func<T, bool> predicate)
        {
            return _objectSet.Single(predicate);
        }

        public T SingleOrDefault(Func<T, bool> predicate)
        {
            return _objectSet.SingleOrDefault(predicate);
        }

        public T First(Func<T, bool> predicate)
        {
            return _objectSet.First(predicate);
        }

        public T FirstOrDefault(Func<T, bool> predicate)
        {
            return _objectSet.FirstOrDefault(predicate);
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _objectSet.Remove(entity);
            Context.SaveChanges();
        }

        public void Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _objectSet.Add(entity);
            Context.SaveChanges();
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public void SaveList(IEnumerable<T> entityList)
        {
            if (entityList == null)
                throw new ArgumentNullException(nameof(entityList));

            try
            {
                Entities.AddRange(entityList);
                Context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        protected string GetFullErrorTextAndRollbackEntityChanges(DbUpdateException exception)
        {
            //rollback entity changes
            if (Context is DbContext dbContext)
            {
                var entries = dbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

                entries.ForEach(entry =>
                {
                    try
                    {
                        entry.State = EntityState.Unchanged;
                    }
                    catch (InvalidOperationException)
                    {
                        // ignored
                    }
                });
            }

            Context.SaveChanges();
            return exception.ToString();
        }


        public void Attach(T entity)
        {
            _objectSet.Attach(entity);
        }

        public IEnumerable<T> Take(int count)
        {
            return _objectSet.Take(count);
        }

        public IEnumerable<T> GetAll(string query, object param = null)
        {
            using (var conn = ConfigUtil.GetSqlConnection())
            {
                return param != null ? conn.Query<T>(query, param)
                    : conn.Query<T>(query);
            }
        }

        public IEnumerable<T> GetAll(string tableName, string whereStr, string primaryKey, int pageSize, int pageIndex, bool isWms = false, ViewModelBase viewModel = null)
        {
            if (viewModel != null) whereStr += viewModel.GetFilterWhere();
            using (var conn = isWms ? ConfigUtil.GetWmsConnection() : ConfigUtil.GetSqlConnection())
            {
                var p = new DynamicParameters();
                p.Add("@tblName", tableName);
                p.Add("@PageSize", pageSize);
                p.Add("@PageIndex", pageIndex);
                p.Add("@strGetFields", "*");
                p.Add("@Sort", "ID DESC");
                p.Add("@fldName", primaryKey);
                p.Add("@strWhere", whereStr);
                p.Add("@Group", "");
                p.Add("@doCount", 0);
                return conn.Query<T>("SP_Pagination", p, null, true, null, CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PRODUCT_DETALL_FRESH> GetAllForFresh(string tableName, string whereStr, string primaryKey, int pageSize, int pageIndex, bool isWms = false, ViewModelBase viewModel = null)
        {
            if (viewModel != null) whereStr += viewModel.GetFilterWhere();
            using (var conn = isWms ? ConfigUtil.GetWmsConnection() : ConfigUtil.GetSqlConnection())
            {
                var p = new DynamicParameters();
                p.Add("@tblName", tableName);
                p.Add("@PageSize", pageSize);
                p.Add("@PageIndex", pageIndex);
                p.Add("@strGetFields", "*");
                p.Add("@Sort", primaryKey);
                p.Add("@fldName", primaryKey);
                p.Add("@strWhere", whereStr);
                p.Add("@Group", "");
                p.Add("@doCount", 0);
                return conn.Query<PRODUCT_DETALL_FRESH>("SP_Pagination", p, null, true, null, CommandType.StoredProcedure);
            }
        }

        public IEnumerable<T> GetAll(string tableName, string fieldKey, int pageCurrent, int pageSize, string fieldOrder, string strWhere, string pageCount)
        {
            using (var conn = ConfigUtil.GetSqlConnection())
            {
                var p = new DynamicParameters();
                p.Add("@tbname", tableName);
                p.Add("@FieldKey", fieldKey);
                p.Add("@PageCurrent", pageCurrent);
                p.Add("@PageSize", pageSize);
                p.Add("@FieldShow", "*");
                p.Add("@FieldOrder", fieldOrder);
                p.Add("@Where", strWhere);
                p.Add("@PageCount", pageCount);
                return conn.Query<T>("sp_PageView", p, null, true, null, CommandType.StoredProcedure);
            }
        }

        public int GetAllCounts(string tableName, string whereStr, string primaryKey, int pageSize, int pageIndex, bool isWms = false, ViewModels.ViewModelBase viewModel = null)
        {
            if (viewModel != null) whereStr += viewModel.GetFilterWhere();

            using (var conn = isWms ? ConfigUtil.GetWmsConnection() : ConfigUtil.GetSqlConnection())
            {
                var p = new DynamicParameters();
                p.Add("@tblName", tableName);
                p.Add("@PageSize", pageSize);
                p.Add("@PageIndex", pageIndex);
                p.Add("@strGetFields", "*");
                p.Add("@Sort", primaryKey);
                p.Add("@fldName", primaryKey);
                p.Add("@strWhere", whereStr);
                p.Add("@Group", "");
                p.Add("@doCount", 1);
                return conn.ExecuteScalar<int>("SP_Pagination", p, null, null, CommandType.StoredProcedure);
            }
        }

        public DataTable GetDataTable(string query, object param = null, bool isOms = true)
        {
            using (var conn = isOms ? ConfigUtil.GetSqlConnection() : ConfigUtil.GetWmsConnection())
            {
                var reader = conn.ExecuteReader(query, param, null, 90000, CommandType.Text);
                var dt = new DataTable();
                dt.Load(reader);
                return dt;
            }
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            //Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;

            Context.SaveChanges();
        }

        public IEnumerable<T> GetActivityGroupList(string tableName, string whereStr, string primaryKey, int pageSize, int pageIndex, bool isWms = false, ViewModelBase viewModel = null)
        {
            if (viewModel != null) whereStr += viewModel.GetFilterWhere();
            using (var conn = isWms ? ConfigUtil.GetWmsConnection() : ConfigUtil.GetSqlConnection())
            {
                var p = new DynamicParameters();
                p.Add("@tblName", tableName);
                p.Add("@PageSize", pageSize);
                p.Add("@PageIndex", pageIndex);
                p.Add("@strGetFields", "*");
                p.Add("@Sort", primaryKey);
                p.Add("@fldName", primaryKey);
                p.Add("@strWhere", whereStr);
                p.Add("@Group", "");
                p.Add("@doCount", 0);
                return conn.Query<T>("SP_Pagination", p, null, true, null, CommandType.StoredProcedure);
            }
        }

        public int GettActivityGroupCounts(string tableName, string whereStr, string primaryKey, int pageSize, int pageIndex, bool isWms = false, ViewModels.ViewModelBase viewModel = null)
        {
            if (viewModel != null) whereStr += viewModel.GetFilterWhere();

            using (var conn = isWms ? ConfigUtil.GetWmsConnection() : ConfigUtil.GetSqlConnection())
            {
                var p = new DynamicParameters();
                p.Add("@tblName", tableName);
                p.Add("@PageSize", pageSize);
                p.Add("@PageIndex", pageIndex);
                p.Add("@strGetFields", "*");
                p.Add("@Sort", primaryKey);
                p.Add("@fldName", primaryKey);
                p.Add("@strWhere", whereStr);
                p.Add("@Group", "");
                p.Add("@doCount", 1);
                return conn.ExecuteScalar<int>("SP_Pagination", p, null, null, CommandType.StoredProcedure);
            }
        }
    }
}
