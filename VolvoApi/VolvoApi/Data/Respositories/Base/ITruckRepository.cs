using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VolvoApi.Model;

namespace VolvoApi.Data.Respositories
{
    public interface ITruckRepository
    {
        Task<bool> Add(Truck input);
        Task<bool> Update(Truck input);
        Task<Truck> Get(Guid id);
        Task<List<Truck>> GetAll();
        Task<bool> Delete(Guid id);

    }
}
