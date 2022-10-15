using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;

namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly XCountryDbContext _dbContext;
        internal DbSet<T> genericDbSet;

        public Repository(XCountryDbContext dbContext)
        {
            _dbContext = dbContext;
            genericDbSet = _dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            _dbContext.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? expressionFilter = null, string? filterProperties = null)
        {
            IQueryable<T> dbQuery = genericDbSet;

            if (expressionFilter != null)
            {
                dbQuery = dbQuery.Where(expressionFilter);
            }
            
            if (filterProperties != null)
            {
                foreach (var filterProperty in filterProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    dbQuery = dbQuery.Include(filterProperty);
                }
            }
            return dbQuery.ToList();
        }

        //get all, including orderby clause, and includes
        //usage: var s = repository.GetAll(i => i.Name, false, i => i.NavigationProperty);
        public IEnumerable<T> GetAll<TOrderKey>(Expression<Func<T, TOrderKey>> orderByExp,
                                                Boolean descending,
                                                params Expression<Func<T, Object>>[] includeExps)
        {
            var query = genericDbSet.AsQueryable();
            query = !descending ? query.OrderBy(orderByExp) : query.OrderByDescending(orderByExp);
            if (includeExps != null)
                query = includeExps.Aggregate(query, (current, exp) => current.Include(exp));

            return query.ToList();
        }


        //get all, including select clause, orderby clause, and includes
        //usage: var s = repository.GetAll(i => new { Name = i.Name }, i => i.Name, false, i => i.NavigationProperty
        public IEnumerable<TReturn> GetAll<TReturn, TOrderKey>(Expression<Func<T, TReturn>> selectExp,
                                                               Expression<Func<T, TOrderKey>> orderByExp,
                                                               Boolean descending,
                                                               params Expression<Func<T, Object>>[] includeExps)
        {
            var query = genericDbSet.AsQueryable();
            query = !descending ? query.OrderBy(orderByExp) : query.OrderByDescending(orderByExp);
            if (includeExps != null)
                query = includeExps.Aggregate(query, (current, exp) => current.Include(exp));

            return query.Select(selectExp).ToList();
        }


        //get all, including select clause, where clause, order by clause, and includes
        //usage: var s = repository.GetAll(i => new { i.Name }, i => i.Name.Contains('John'), i => i.Name, false, i => i.NavigationProperty
        public IEnumerable<TReturn> GetAll<TReturn, TOrderKey>(Expression<Func<T, TReturn>> selectExp,
                                                               Expression<Func<T, Boolean>> whereExp,
                                                               Expression<Func<T, TOrderKey>> orderbyExp,
                                                               Boolean descending,
                                                               params Expression<Func<T, object>>[] includeExps)
        {
            var query = genericDbSet.Where(whereExp);
            query = !descending ? query.OrderBy(orderbyExp) : query.OrderByDescending(orderbyExp);
            if (includeExps != null)
                query = includeExps.Aggregate(query, (current, exp) => current.Include(exp));

            return query.Select(selectExp).ToList();
        }

        public T GetById(T id)
        {
            return genericDbSet.Find(id);
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> expressionFilter, string? filterProperties = null)
        {
            IQueryable<T> dbQuery = genericDbSet;
            dbQuery = dbQuery.Where(expressionFilter);

            return dbQuery.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            genericDbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            genericDbSet.RemoveRange(entities);
        }
    }
}
