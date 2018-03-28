using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsRentEF.EF;

namespace CarsRentEF.Repos
{
    public abstract class BaseRepo<T> : IDisposable where T : class, new()
    {
        public CarsRentEntities Context { get; } = new CarsRentEntities();
        protected DbSet<T> Table;

        internal int SaveChanges() {
            try {
                return Context.SaveChanges();
            } catch (DbUpdateConcurrencyException ex) {
                Console.WriteLine($"Ошбика параллелизма при обновлении Базы данных:\n{ex.Message}");
                throw;
            } catch (DbUpdateException ex) {
                Console.WriteLine($"Отказ при обновлении Базы данных:\n{ex.Message}");
                throw;
            } catch (CommitFailedException ex) {
                Console.WriteLine($"Ошибка транзакции:\n{ex.Message}");
                throw;
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        internal async Task<int> SaveChangesAsync() {
            try {
                return await Context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException ex) {
                Console.WriteLine($"Ошбика параллелизма при обновлении Базы данных:\n{ex.Message}");
                throw;
            } catch (DbUpdateException ex) {
                Console.WriteLine($"Отказ при обновлении Базы данных:\n{ex.Message}");
                throw;
            } catch (CommitFailedException ex) {
                Console.WriteLine($"Ошибка транзакции:\n{ex.Message}");
                throw;
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public T GetOne(int? id) => Table.Find(id);
        public Task<T> GetOneAsync(int? id) => Table.FindAsync(id);
        public List<T> GetAll() => Table.ToList();
        public Task<List<T>> GetAllAsync() => Table.ToListAsync();

        public List<T> ExecuteQuery(string sql) => Table.SqlQuery(sql).ToList();
        public Task<List<T>> ExecuteQueryAsync(string sql) => Table.SqlQuery(sql).ToListAsync();

        public List<T> ExecuteQuery(string sql, object[] sqlParametersObjects)
            => Table.SqlQuery(sql, sqlParametersObjects).ToList();

        public Task<List<T>> ExecuteQueryAsync(string sql, object[] sqlParametersObjects)
            => Table.SqlQuery(sql, sqlParametersObjects).ToListAsync();

        public int Add(T entity) {
            Table.Add(entity);
            return SaveChanges();
        }

        public Task<int> AddAsync(T entity) {
            Table.Add(entity);
            return SaveChangesAsync();
        }

        public int AddRange(IList<T> entities) {
            Table.AddRange(entities);
            return SaveChanges();
        }

        public Task<int> AddRangeAsync(IList<T> entities) {
            Table.AddRange(entities);
            return SaveChangesAsync();
        }

        public int Save(T entity) {
            Context.Entry(entity).State = EntityState.Modified;
            return SaveChanges();
        }

        public Task<int> SaveAsync(T entity) {
            Context.Entry(entity).State = EntityState.Modified;
            return SaveChangesAsync();
        }

        public int Delete(T entity) {
            Context.Entry(entity).State = EntityState.Deleted;
            return SaveChanges();
        }

        public Task<int> DeleteAsync(T entity) {
            Context.Entry(entity).State = EntityState.Deleted;
            return SaveChangesAsync();
        }
        //====================================================================
        private bool _disposed = false;
        public void Dispose() {
            CleanUp(true);
            // подавить финализацию, кот., реализ. в базовом DbContext
            GC.SuppressFinalize(this);
        }

        protected virtual void CleanUp(bool diposing) {
            if (_disposed) return;
            if (diposing) {
                Context.Dispose(); // вызов Dispose() кот., реализ. в базовом DbContext (осв.неупр.ресурс)
                // освободить здесь любые управляемые объекты
            }
            // освободить здесь любые управляемые объекты
            _disposed = true;
        }
    }
}
