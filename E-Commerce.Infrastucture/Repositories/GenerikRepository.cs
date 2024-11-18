using E_Commerce.Domain.Interfaces;
using E_Commerce.Infrastucture.ExternalServices.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastucture.Repositories
{
    
        public class GenericRepository<T> : IGenericRepository<T> where T : class
        {
            private readonly ApplicationDbContext _context;

            public GenericRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<T>> GetAllAsync()
            {
                return await _context.Set<T>().ToListAsync();
            }

            public async Task<T?> GetByIdAsync(int id)
            {
                return await _context.Set<T>().FindAsync(id);
            }

            public async Task AddAsync(T entity)
            {
                await _context.Set<T>().AddAsync(entity);
            }

            public void Update(T entity)
            {
                _context.Set<T>().Update(entity);
            }

            public void Delete(T entity)
            {
                _context.Set<T>().Remove(entity);
            }

            public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            {
                return await _context.Set<T>().Where(predicate).ToListAsync();
            }

            public async Task SaveChangesAsync()
            {
                await _context.SaveChangesAsync();
            }
        }
    
}
