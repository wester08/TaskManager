
using System.Linq.Expressions;
using TaskManager.Domain.Base;

namespace TaskManager.Application.Interfaces.Respositories
{
    public interface IBaseRepository <TEntity> where TEntity : class
    {
        Task<OperationResult> GetAllAsync(Expression<Func<TEntity, bool>> filter);
        Task<OperationResult> GetByIdAsync(int id);
        Task<OperationResult> GetByNameAsync(string name);

        Task<OperationResult> GetByEmailAsync(string email);
        Task<OperationResult> GetByAsync<Tparameter>(Expression<Func<TEntity, Tparameter>> parameterSelector, Tparameter value);
        Task<OperationResult> AddAsync(TEntity entity);
        Task<OperationResult> UpdateAsync(TEntity entity);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);
        Task<OperationResult> DeleteAsync(int id);

    }
}


