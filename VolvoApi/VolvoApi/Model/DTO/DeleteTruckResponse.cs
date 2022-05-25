using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VolvoApi.Model.DTO
{
    public class DeleteTruckResponse: Response
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public int ManufacturingYear { get; set; }
        public int ModelYear { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public  DeleteTruckResponse()
        {
                
        }
    }
}
