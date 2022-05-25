using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VolvoApi.Model.DTO;

namespace VolvoApi.Service
{
    public interface ITruckService
    {
        Task<CreateTruckResponse> AddTruck(CreateTruckRequest input);
        Task<GetTruckResponse> GetTruck(Guid id);
        Task<IEnumerable<GetTruckResponse>> GetAllTrucks();
        Task<UpdatedTruckResponse> UpdateTruck(Guid id, UpdateTruckRequest input);
        Task<DeleteTruckResponse> DeleteTruck(Guid id);

    }
}
