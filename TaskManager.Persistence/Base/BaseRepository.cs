using TaskManager.Application.Interfaces.Respositories;
using TaskManager.Domain.Base;
using TaskManager.Persistence.Context;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Persistence.Base
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly TaskManagerContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(TaskManagerContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();

        }

        public virtual async Task<OperationResult> GetAllAsync(Expression<Func<TEntity, bool>> filter)
        {
            OperationResult Opresult = new OperationResult();

            try 
            {
                var entities = await _dbSet.Where(filter).ToListAsync();
                Opresult = OperationResult.Success($"Entity {typeof(TEntity)} retrieved successfully", entities);


            }
            catch (Exception ex)
            {
                Opresult = OperationResult.Failure($"Error retrieving entities{typeof(TEntity)} : {ex.Message}");
            }
            return Opresult;

        }
        public virtual async Task<OperationResult> GetByIdAsync(int id)
        {
            OperationResult Opresult = new OperationResult();

            try
            {

                var entity = await _dbSet.FindAsync(id);

                if (entity == null)
                {
                    Opresult = OperationResult.Failure($"Entity {typeof(TEntity)} with ID {id} not found.");
                    return Opresult;
                }

                Opresult = OperationResult.Success($"Entity {typeof(TEntity)} retrieved successfully", entity);


            }
            catch (Exception ex)
            {
                Opresult = OperationResult.Failure($"Error retrieving entities{typeof(TEntity)} : {ex.Message}");
            }
            return Opresult;
        }
        public virtual async Task<OperationResult> AddAsync(TEntity entity)
        {
            OperationResult Opresult = new OperationResult();

            try
            {
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();
                Opresult = OperationResult.Success($"Entity {typeof(TEntity)} added successfully", entity);
            }
            catch (Exception ex)
            {
                Opresult = OperationResult.Failure($"Error adding entity {typeof(TEntity)}: {ex.Message}");
            }
            return Opresult;

        }
        public virtual async Task<OperationResult> UpdateAsync(TEntity entity)
        {
            OperationResult Opresult = new OperationResult();

            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                Opresult = OperationResult.Success($"Entity {typeof(TEntity)} updated successfully", entity);
            }
            catch (Exception ex)
            {
                Opresult = OperationResult.Failure($"Error updating entity {typeof(TEntity)}: {ex.Message}");
            }
            return Opresult;
        }
        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            OperationResult Opresult = new OperationResult();

            try
            {
                var exists = await _dbSet.AnyAsync(filter);
                if (exists)
                {
                    Opresult = OperationResult.Success($"Entity {typeof(TEntity)} exists.");
                }
                else
                {
                    Opresult = OperationResult.Failure($"Entity {typeof(TEntity)} does not exist.");
                }
            }
            catch (Exception ex)
            {
                Opresult = OperationResult.Failure($"Error checking existence of entity {typeof(TEntity)}: {ex.Message}");

            }
          
            return await _dbSet.AnyAsync(filter);
        }

        public virtual async Task<OperationResult> DeleteAsync(int id)
        {
            OperationResult Opresult = new OperationResult();

            //Action Task Delete
            Action<int> notifyDelete = task =>
                Console.WriteLine($"Task Deleted with id {id}");

            try
            {

                var entity = await _dbSet.FindAsync(id);

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                Opresult = OperationResult.Success($"Entity {typeof(TEntity)} removed successfully", entity);


            }
            catch (Exception ex)
            {
                Opresult = OperationResult.Failure($"Error removed entity{typeof(TEntity)} : {ex.Message}");
            }

            if (Opresult.IsSuccess)
            {
                notifyDelete(id);
            }
            return Opresult;
        }


        //Filtros.
        public virtual async Task<OperationResult> GetByAsync<Tparameter>(Expression<Func<TEntity, Tparameter>> parameterSelector, Tparameter value)
        {
            OperationResult operationResult = new OperationResult();
            try
            {
                var parameter = Expression.Parameter(typeof(TEntity), "e");

                var body = Expression.Equal(
                    Expression.Invoke(parameterSelector, parameter),
                    Expression.Constant(value, typeof(Tparameter))

                    );

                var lambda = Expression.Lambda<Func<TEntity, bool>>(body, parameter);

                var entities = await _dbSet.Where(lambda).ToListAsync();

                if (entities == null || entities.Any())
                {
                    operationResult = OperationResult.Success($"{entities.Count} {typeof(TEntity).Name}(s) retrieved successfuly ", entities);

                }
            }
            catch (Exception ex)
            {
                operationResult = OperationResult.Failure($"Error retrieving {typeof(TEntity).Name} by property {ex.Message}");

            }
            return operationResult;
        }
    }
}



