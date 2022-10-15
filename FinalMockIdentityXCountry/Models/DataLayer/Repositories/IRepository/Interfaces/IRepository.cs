using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;

namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        T GetById(T id);
        T GetFirstOrDefault(Expression<Func<T, bool>> expressionFilter, string? searchProperties = null);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? expressionFilter = null, string? filterProperties = null);

        //get all, including orderby clause, and includes
        //usage: var s = repository.GetAll(i => i.Name, false, i => i.NavigationProperty);
        public IEnumerable<T> GetAll<TOrderKey>(Expression<Func<T, TOrderKey>> orderbyExp,
                                                Boolean descending,
                                                params Expression<Func<T, Object>>[] includeExps);


        //get all, including select clause, orderby clause, and includes
        //usage: var s = repository.GetAll(i => new { Name = i.Name }, i => i.Name, false, i => i.NavigationProperty
        public IEnumerable<TReturn> GetAll<TReturn, TOrderKey>(Expression<Func<T, TReturn>> selectExp,
                                                               Expression<Func<T, TOrderKey>> orderExp,
                                                               Boolean descending,
                                                               params Expression<Func<T, Object>>[] includeExps);

        //get all, including select clause, where clause, order by clause, and includes
        //usage: var s = repository.GetAll(i => new { i.Name }, i => i.Name.Contains('John'), i => i.Name, false, i => i.NavigationProperty
        public IEnumerable<TReturn> GetAll<TReturn, TOrderKey>(Expression<Func<T, TReturn>> selectExp,
                                                               Expression<Func<T, Boolean>> whereExp,
                                                               Expression<Func<T, TOrderKey>> orderbyExp,
                                                               Boolean descending,
                                                               params Expression<Func<T, object>>[] includeExps);
    }
}
