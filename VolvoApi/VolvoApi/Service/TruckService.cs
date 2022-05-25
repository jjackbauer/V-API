using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolvoApi.Data.Respositories;
using VolvoApi.Model;
using VolvoApi.Model.DTO;

namespace VolvoApi.Service
{
    public class TruckService : ITruckService
    {
        private readonly IMapper _mapper;
        private readonly ITruckRepository _repository;
        public TruckService(ITruckRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CreateTruckResponse> AddTruck(CreateTruckRequest input)
        {
            CreateTruckResponse output = new CreateTruckResponse();

            try
            {
                Truck t = _mapper.Map<Truck>(input);

                bool sucess = await _repository.Add(t);

                if (!sucess)
                    output.AddError($"It wasn't possible to insert Truck {t}");
                
                output = _mapper.Map<CreateTruckResponse>(t);

            }
            catch (Exception ex)
            {
                output.AddError(ex.Message);
                
            }

            return output;
        }

        public async Task<DeleteTruckResponse> DeleteTruck(Guid id)
        {
            Truck t = await _repository.Get(id);
            DeleteTruckResponse output = new DeleteTruckResponse();

            if (t == null)
            {
                output.AddError($"The Truck with Id: {id} wasn't found in the database");
                return output;
            }

            try
            {
                bool sucess = await _repository.Delete(id);

                if (!sucess)
                    output.AddError($"It wasn't possible to delete the Truck {t}");

                output = _mapper.Map<DeleteTruckResponse>(t);
            }
            catch (Exception ex)
            {
                output.AddError(ex.Message);
            }

            return output;
        }

        public async Task<IEnumerable<GetTruckResponse>> GetAllTrucks()
        {
            List<GetTruckResponse> output = new List<GetTruckResponse>();

            try
            {
                var trucks = await _repository.GetAll();

                if (trucks.Count == 0)
                    return null;

                output = trucks.Select(x => _mapper.Map<GetTruckResponse>(x)).ToList();
            }
            catch (Exception ex)
            {
                var t = new GetTruckResponse();
                t.AddError(ex.Message);
                output.Add(t);
            }
            return output;
        }

        public async Task<GetTruckResponse> GetTruck(Guid id)
        {
            GetTruckResponse output = new GetTruckResponse();

            try
            {
                var t = await _repository.Get(id);

                if (t == null)
                {
                    output.AddError($"The Truck with Id: {id} wasn't found");
                    return output;
                }
                    

                output = _mapper.Map<GetTruckResponse>(t);
            }
            catch (Exception ex)
            {
                output.AddError(ex.Message);
            }

            return output;
        }

        public async Task<UpdatedTruckResponse> UpdateTruck(Guid id, UpdateTruckRequest input)
        {
            Truck t = await _repository.Get(id);
            
            UpdatedTruckResponse output = new UpdatedTruckResponse();

            if (t == null)
            {
                output.AddError($"The Truck with Id: {id} wasn't found in the database");
                return output;
            }

            t = _mapper.Map<UpdateTruckRequest, Truck>(input, t);
            
            try
            {
                bool sucess = await _repository.Update(t);

                if (!sucess)
                    output.AddError($"It wasn't possible to update the Truck with Id: {id}");

                output = _mapper.Map<UpdatedTruckResponse>(t);
            }
            catch (Exception ex)
            {
                output.AddError(ex.Message); ;
            }

            return output;
        }
    }
}
