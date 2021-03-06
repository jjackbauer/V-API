using System;

namespace VolvoApi.Model.DTO
{
    public class UpdatedTruckResponse: Response
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public int ManufacturingYear { get; set; }
        public int ModelYear { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public UpdatedTruckResponse()
        {

        }
    }
}
