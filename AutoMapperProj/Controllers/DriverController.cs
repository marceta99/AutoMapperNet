using AutoMapper;
using AutoMapperProj.Models;
using AutoMapperProj.Models.DTOs.Incoming;
using AutoMapperProj.Models.DTOs.Outgoing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoMapperProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private static List<Driver> drivers = new List<Driver>();
        private readonly IMapper _mapper;

        public DriverController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetDrivers()
        {
            var activeDrivers= drivers.Where(d=> d.Status== 1).ToList();

            var driversDtos = _mapper.Map<IEnumerable<DriverDTO>>(activeDrivers);
            return Ok(driversDtos);
        }

        [HttpPost]
        public IActionResult CreateDriver(DriverForCreationDTO data)
        {
            if (ModelState.IsValid)
            {
                var driver = _mapper.Map<Driver>(data);

                drivers.Add(driver);

                var driverDto = _mapper.Map<DriverDTO>(driver);
                return CreatedAtAction("GetDriver", new { driverDto.Id }, driverDto);
            }
            return BadRequest("Bad requestttt");
        }
        [HttpGet("{id}")]
        public IActionResult GetDriver(Guid id)
        {
            var driver = drivers.FirstOrDefault(x=> x.Id == id);
            if (driver == null) return NotFound();

            return Ok(driver);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDriver(Guid id, Driver data)
        {
            var existingDriver = drivers.FirstOrDefault(x => x.Id== data.Id);

            if (existingDriver == null) {
                return BadRequest();
            }

            existingDriver.DriverNumber = data.DriverNumber;
            existingDriver.FirstName = data.FirstName;
            existingDriver.LastName = data.LastName;

            return NoContent();
        }

    }
}
