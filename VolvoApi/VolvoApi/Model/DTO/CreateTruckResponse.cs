using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VolvoApi.Model.DTO
{
    public class CreateTruckResponse : Response
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public int ManufacturingYear { get; set; }
        public int ModelYear { get; set; }
        public DateTime CreatedAt { get; set; }
        public CreateTruckResponse()
        {

        }
    }
}
