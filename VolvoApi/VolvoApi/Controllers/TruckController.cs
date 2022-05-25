using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VolvoApi.Model.DTO;
using VolvoApi.Service;

namespace VolvoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruckController : ControllerBase
    {
        private readonly ITruckService _service;
        public TruckController(ITruckService service)
        {
            _service = service;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetTruck([FromRoute]Guid id)
        {
            GetTruckResponse output = await _service.GetTruck(id);

            if (output.Errors.Count > 0)
                return BadRequest(output.Errors);

            return Ok(output);
        }
        [HttpGet("")]
        public async Task<ActionResult> GetAllTrucks()
        {
            IEnumerable<GetTruckResponse> output = await _service.GetAllTrucks();

            if (output != null && output.FirstOrDefault()?.Errors?.Count > 0)
                return Problem(JsonConvert.SerializeObject(output.First().Errors.Count));

            return Ok(output);
        }
        [HttpPost("")]
        public async Task<ActionResult> AddTruck([FromBody] CreateTruckRequest input)
        {
            if (!ModelState.IsValid)
                return CleanErrors();

            CreateTruckResponse output =  await _service.AddTruck(input);

            if (output.Errors.Count > 0)
                return BadRequest(output.Errors);

            return Created("truck", output);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTruck([FromRoute] Guid id, [FromBody] UpdateTruckRequest input)
        {
            if (!ModelState.IsValid)
                return CleanErrors();

            UpdatedTruckResponse output = await _service.UpdateTruck(id, input);

            if (output.Errors.Count > 0)
                return BadRequest(output);

            return Created("truck/{id}", output);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTruck([FromRoute] Guid id)
        {
            DeleteTruckResponse output = await _service.DeleteTruck(id);

            if(output.Errors.Count > 0)
                return BadRequest(output);

            return Ok(output);
        }

        private ActionResult CleanErrors()
        {
            List<string> eList = new List<string>();

            foreach (var e in ModelState.Values)
                eList.AddRange(e.Errors.Select(x => x.ErrorMessage).ToArray());
            

            return BadRequest(
                                new
                                    {
                                       Errors =  eList
                                    }
                             );
        }
    }
}
