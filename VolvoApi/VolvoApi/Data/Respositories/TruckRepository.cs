using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolvoApi.Data.Context;
using VolvoApi.Model;

namespace VolvoApi.Data.Respositories
{
    public class TruckRepository : ITruckRepository
    {
        private readonly TruckDbContext _context;
        public TruckRepository(TruckDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Add(Truck input)
        {
            await _context.Trucks.AddAsync(input);

            return await UoW();
        }

        public async Task<bool> Delete(Guid id)
        {
            Truck t = await Get(id);
            _context.Trucks.Remove(t);

            return await UoW();
        }

        public async Task<Truck> Get(Guid id)
        {
           return await _context.Trucks.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Truck>> GetAll()
        {
            return await _context.Trucks.AsNoTracking().ToListAsync();
        }

        public async Task<bool> Update(Truck input)
        {
            _context.Trucks.Update(input);

            return await UoW();
        }
        private async Task<bool> UoW()
        {
            bool sucess = await _context.SaveChangesAsync() > 0;

            return sucess;
        }
    }
}
