using System;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using VolvoApi;
using System.Threading.Tasks;
using Xunit.Abstractions;
using System.Net.Http;
using VolvoApi.Model;
using VolvoApi.Model.DTO;
using Newtonsoft.Json;
using System.Text;
using Xunit.Priority;
using System.Collections.Generic;
using System.Net;

namespace VolvoApiTest
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class VolvoControllerTest : IClassFixture<WebApplicationFactory<Startup>>, IAsyncLifetime
    {
        protected readonly WebApplicationFactory<Startup> _factory;
        protected readonly ITestOutputHelper _output;
        protected readonly HttpClient _httpClient;
        protected List<Truck> trucks;
        public VolvoControllerTest(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            trucks = new();
            _factory = factory;
            _output = output;
            _httpClient = _factory.CreateClient();
        }

        public async Task DisposeAsync()
        {
            await deleteAllTrucks();
            _httpClient.Dispose();
        }

        public async Task InitializeAsync()
        {
            for(int c = 0; c < 9; c++)
                trucks.Add(await RegisterNewTruck());
        }

        [Fact, Priority(10)]
        public async Task RegisterTruck_ProvidingValidTruck_ShouldReturnCreated()
        {
            var request = new CreateTruckRequest
            {
                Name = "Model",
                Model = "FH",
                Description = "This is a Model Truck, FH Type!",
                ManufacturingYear = DateTime.Now.Year,
                ModelYear = DateTime.Now.Year + 1
            };


            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var httpClientRequest = await _httpClient.PostAsync("api/Truck", content);

            CreateTruckResponse response = JsonConvert.DeserializeObject<CreateTruckResponse>(await httpClientRequest.Content.ReadAsStringAsync());

            Assert.Empty(response.Errors);
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.Equal(HttpStatusCode.Created, httpClientRequest.StatusCode);
            Assert.Equal(request.Name, response.Name);
            Assert.Equal(request.ModelYear, response.ModelYear);
            Assert.Equal(request.Description, response.Description);
            Assert.Equal(request.ManufacturingYear, response.ManufacturingYear);
            Assert.Equal(request.Model, response.Model);
            _output.WriteLine($"{nameof(VolvoControllerTest)}_{nameof(RegisterTruck_ProvidingValidTruck_ShouldReturnCreated)} => {await httpClientRequest.Content.ReadAsStringAsync()}");

            trucks.Add(new Truck
            {
                Id = response.Id,
                Name = response.Name,
                Model = response.Model,
                ManufacturingYear = response.ManufacturingYear,
                Description = response.Description,
                CreatedAt = response.CreatedAt,
                ModifiedAt = new DateTime(),
                ModelYear = response.ModelYear
            });

        }
        [Fact]
        public async Task RegisterTruck_ProvidingInvalidTruck_ShouldReturnErrors()
        {
            var request = new CreateTruckRequest
            {
                Name = "ModelA",
                Model = "FL",//Invalid Model
                Description = "This is a ModelA Truck, FH Type!",
                ManufacturingYear = DateTime.Now.Year - 2, //Invalid ManufacturingYear
                ModelYear = DateTime.Now.Year + 2//Invalid ModelYear
            };


            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var httpClientRequest = await _httpClient.PostAsync("api/Truck", content);

            CreateTruckResponse response = JsonConvert.DeserializeObject<CreateTruckResponse>(await httpClientRequest.Content.ReadAsStringAsync());

            Assert.NotEmpty(response.Errors);
            Assert.Equal(HttpStatusCode.BadRequest, httpClientRequest.StatusCode);
            Assert.Contains("The Model of the truck must be either FH or FM", response.Errors);
            Assert.Contains($"The ManufacturingYear of the truck must be {DateTime.Now.Year}", response.Errors);
            Assert.Contains($"The ModelYear of the truck must be {DateTime.Now.Year} or {DateTime.Now.Year + 1}", response.Errors);

            _output.WriteLine($"{nameof(VolvoControllerTest)}_{nameof(RegisterTruck_ProvidingInvalidTruck_ShouldReturnErrors)} => {await httpClientRequest.Content.ReadAsStringAsync()}");

        }
        [Fact]
        public async Task DeleteTruck_WithValidTruckId_ShouldReturnOK()
        {
            var httpClientRequest = await _httpClient.DeleteAsync($"api/Truck/{trucks[0].Id}");

            DeleteTruckResponse response = JsonConvert.DeserializeObject<DeleteTruckResponse>(await httpClientRequest.Content.ReadAsStringAsync());

            Assert.Empty(response.Errors);
            Assert.Equal(HttpStatusCode.OK, httpClientRequest.StatusCode);

            trucks.Remove(trucks[0]);

            _output.WriteLine($"{nameof(VolvoControllerTest)}_{nameof(DeleteTruck_WithValidTruckId_ShouldReturnOK)} => {await httpClientRequest.Content.ReadAsStringAsync()}");

        }
        [Fact]
        public async Task DeleteTruck_WithInvalidTruckId_ShouldReturnErrors()
        {
            var httpClientRequest = await _httpClient.DeleteAsync($"api/Truck/{Guid.NewGuid()}");

            DeleteTruckResponse response = JsonConvert.DeserializeObject<DeleteTruckResponse>(await httpClientRequest.Content.ReadAsStringAsync());

            Assert.NotEmpty(response.Errors);
            Assert.Equal(HttpStatusCode.BadRequest, httpClientRequest.StatusCode);

            _output.WriteLine($"{nameof(VolvoControllerTest)}_{nameof(DeleteTruck_WithInvalidTruckId_ShouldReturnErrors)} => {await httpClientRequest.Content.ReadAsStringAsync()}");

        }
        [Fact]
        public async Task GetAllTrucks_Allways_MustReturnAllTrucks()
        {
            var httpClientRequest = await _httpClient.GetAsync("api/Truck");
            List<GetTruckResponse> response = JsonConvert.DeserializeObject<List<GetTruckResponse>>(await httpClientRequest.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, httpClientRequest.StatusCode);
            Assert.Empty(response[0].Errors);
            Assert.NotEmpty(response);
            Assert.Equal(trucks.Count, response.Count);
            _output.WriteLine($"{nameof(VolvoControllerTest)}_{nameof(GetAllTrucks_Allways_MustReturnAllTrucks)} => {await httpClientRequest.Content.ReadAsStringAsync()}");
        }
        [Fact]
        public async Task GetTruckById_WithValidTruckId_MustReturnTruck()
        {
            var httpClientRequest = await _httpClient.GetAsync($"api/Truck/{trucks[0].Id}");
           GetTruckResponse response = JsonConvert.DeserializeObject<GetTruckResponse>(await httpClientRequest.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, httpClientRequest.StatusCode);
            Assert.Empty(response.Errors);
            Assert.Equal(trucks[0].Name,response.Name);
            Assert.Equal(trucks[0].Model, response.Model);
            Assert.Equal(trucks[0].Description, response.Description);
            Assert.Equal(trucks[0].ModelYear, response.ModelYear);
            Assert.Equal(trucks[0].ManufacturingYear, response.ManufacturingYear);

            _output.WriteLine($"{nameof(VolvoControllerTest)}_{nameof(GetTruckById_WithValidTruckId_MustReturnTruck)} => {await httpClientRequest.Content.ReadAsStringAsync()}");


        }
        [Fact]
        public async Task UpdateTruck_WithValidTruckIdAndProperties_MustReturnCreated()
        {
            var request = new UpdateTruckRequest
            {
                Name = "Model",
                Model = "FH",
                Description = "This is a Model Truck, FH Type Turbo!",
                ManufacturingYear = DateTime.Now.Year,
                ModelYear = DateTime.Now.Year + 1
            };

            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var httpClientRequest = await _httpClient.PutAsync($"api/Truck/{trucks[0].Id}", content);

            UpdatedTruckResponse response = JsonConvert.DeserializeObject<UpdatedTruckResponse>(await httpClientRequest.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.Created, httpClientRequest.StatusCode);
            Assert.Equal(request.Name, response.Name);
            Assert.Equal(request.Model, response.Model);
            Assert.Equal(request.Description, response.Description);
            Assert.Equal(request.ManufacturingYear, response.ManufacturingYear);
            Assert.Equal(request.ModelYear, response.ModelYear);
            Assert.True(response.ModifiedAt > response.CreatedAt);

            _output.WriteLine($"{nameof(VolvoControllerTest)}_{nameof(UpdateTruck_WithValidTruckIdAndProperties_MustReturnCreated)} => {await httpClientRequest.Content.ReadAsStringAsync()}");


        }
        [Fact]
        public async Task UpdateTruck_WithInvalidTruckIdAndProperties_MustReturnBadRequest()
        {
            var request = new UpdateTruckRequest
            {
                Name = "Model",
                Model = "FL",// Invalid Model
                Description = "This is a Model Truck, FH Type Turbo!",
                ManufacturingYear = DateTime.Now.Year-1, // Invalid ManufacturingYear
                ModelYear = DateTime.Now.Year + 3// Invalid ModelYear
            };

            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var httpClientRequest = await _httpClient.PutAsync($"api/Truck/{trucks[0].Id}", content);

            UpdatedTruckResponse response = JsonConvert.DeserializeObject<UpdatedTruckResponse>(await httpClientRequest.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.BadRequest, httpClientRequest.StatusCode);
            Assert.NotEmpty(response.Errors);
            Assert.Contains("The Model of the truck must be either FH or FM", response.Errors);
            Assert.Contains($"The ManufacturingYear of the truck must be {DateTime.Now.Year}", response.Errors);
            Assert.Contains($"The ModelYear of the truck must be {DateTime.Now.Year} or {DateTime.Now.Year + 1}", response.Errors);

            _output.WriteLine($"{nameof(VolvoControllerTest)}_{nameof(UpdateTruck_WithInvalidTruckIdAndProperties_MustReturnBadRequest)} => {await httpClientRequest.Content.ReadAsStringAsync()}");
        }
        private async Task<Truck> RegisterNewTruck()
        {
            var request = new CreateTruckRequest
            {
                Name = "Model",
                Model = "FH",
                Description = "This is a Model Truck, FH Type!",
                ManufacturingYear = DateTime.Now.Year,
                ModelYear = DateTime.Now.Year + 1
            };


            StringContent content = new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var httpClientRequest = await _httpClient.PostAsync("api/Truck", content);

            CreateTruckResponse response = JsonConvert.DeserializeObject<CreateTruckResponse>(await httpClientRequest.Content.ReadAsStringAsync());


            return new Truck
            {
                Id = response.Id,
                Name = response.Name,
                Model = response.Model,
                ManufacturingYear = response.ManufacturingYear,
                Description = response.Description,
                CreatedAt = response.CreatedAt,
                ModifiedAt = new DateTime(),
                ModelYear = response.ModelYear
            };

        }
        private async Task DeleteTruck()
        {
            var httpClientRequest = await _httpClient.DeleteAsync($"api/Truck/{trucks[0].Id}");

            DeleteTruckResponse response = JsonConvert.DeserializeObject<DeleteTruckResponse>(await httpClientRequest.Content.ReadAsStringAsync());

            trucks.Remove(trucks[0]);

        }
        private async Task deleteAllTrucks()
        {
            while (trucks.Count > 0)
                await DeleteTruck();
        }
    }
}

