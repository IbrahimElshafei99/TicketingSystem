using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Core.Context;
using Ticketing.Core.Interfaces;

namespace Ticketing.Core.Repos
{
    public class GenericsRepo<T> : IGenericsRepo<T> where T : class 
    {
        private readonly AppDbContext _context;

        public GenericsRepo(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<T> GetbyId(int id) 
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task AddRecord(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRecord(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
