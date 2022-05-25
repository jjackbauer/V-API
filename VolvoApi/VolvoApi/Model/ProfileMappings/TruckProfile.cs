using AutoMapper;
using System;
using VolvoApi.Model.DTO;

namespace VolvoApi.Model.ProfileMappings
{
    public class TruckProfile : Profile
    {
        public TruckProfile()
        {
            CreateMap<CreateTruckRequest, Truck>()
                .ForMember(x => x.CreatedAt, y => y.MapFrom( _ => DateTime.Now));
            CreateMap<Truck, CreateTruckResponse>();
            CreateMap<Truck, DeleteTruckResponse>();
            CreateMap<UpdateTruckRequest, Truck>()
                .ForMember(x => x.Id, y => y.UseDestinationValue())
                .ForMember(x => x.CreatedAt, y => y.UseDestinationValue())
                .ForMember(x => x.ModifiedAt, y => y.MapFrom(_ => DateTime.Now));
            CreateMap<Truck, GetTruckResponse>();
            CreateMap<Truck, UpdatedTruckResponse>();
        }
    }
}
