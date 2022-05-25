using System;

namespace VolvoApi.Model
{
    public class Truck
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public int ManufacturingYear { get; set; }
        public int ModelYear { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public Truck()
        {

        }
        public Truck(string name, string model, string description, int manufacturingYear, int modelYear, DateTime createdAt, DateTime modifiedAt = default)
        {
            Id = Guid.NewGuid();
            Name = name;
            Model = model;
            Description = description;
            ManufacturingYear = manufacturingYear;
            ModelYear = modelYear;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
        }
    }
}
