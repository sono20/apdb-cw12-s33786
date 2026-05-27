using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Services;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public PatientsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatients([FromQuery] string? search)
        {
            try
            {
                var patients = await _dbService.GetPatients(search);
                return Ok(patients);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("{pesel}/bedassignments")]
        public async Task<IActionResult> CreateBedAssignment(string pesel, [FromBody] CreateBedAssignmentDto dto)
        {
            try
            {
                await _dbService.CreateBedAssignment(pesel, dto);
                return StatusCode(201);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
