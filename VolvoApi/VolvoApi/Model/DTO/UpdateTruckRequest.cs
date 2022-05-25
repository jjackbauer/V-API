namespace VolvoApi.Model.DTO
{
    public class UpdateTruckRequest
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public int ManufacturingYear { get; set; }
        public int ModelYear { get; set; }
        public UpdateTruckRequest()
        {

        }
    }
}
