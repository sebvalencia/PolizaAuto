using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PolizaAuto.Models;

namespace PolizaAuto.Controllers
{
    [ApiController]
    [Route("api/policies")]
    public class PolicyController : ControllerBase
    {
        private readonly ILogger<PolicyController> _logger;
        private readonly InsuranceContext _context;

        public PolicyController(ILogger<PolicyController> logger, PolizaAutoContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public IActionResult CreatePolicy([FromBody] Policy policy)
        {
            try
            {
                // Validar la vigencia de la póliza
                if (!IsPolicyValid(policy.PolicyStartDate, policy.PolicyEndDate))
                {
                    return BadRequest("La póliza no está vigente");
                }

                _context.Policies.Add(policy);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetPolicyByPolicyNumber), new { policyNumber = policy.PolicyNumber }, policy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la póliza");
                return StatusCode(500, "Ocurrió un error al crear la póliza");
            }
        }

        [HttpGet("{policyNumber}")]
        public IActionResult GetPolicyByPolicyNumber(string policyNumber)
        {
            try
            {
                var policy = _context.Policies.FirstOrDefault(p => p.PolicyNumber == policyNumber);
                if (policy == null)
                {
                    return NotFound();
                }

                return Ok(policy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la póliza");
                return StatusCode(500, "Ocurrió un error al obtener la póliza");
            }
        }

        [HttpGet("vehicle/{licensePlate}")]
        public IActionResult GetPoliciesByVehicleLicensePlate(string licensePlate)
        {
            try
            {
                var policies = _context.Policies.Where(p => p.VehicleLicensePlate == licensePlate).ToList();
                if (policies == null || policies.Count == 0)
                {
                    return NotFound();
                }

                return Ok(policies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las pólizas");
                return StatusCode(500, "Ocurrió un error al obtener las pólizas");
            }
        }

        private bool IsPolicyValid(DateTime startDate, DateTime endDate)
        {
            var currentDate = DateTime.UtcNow;
            return startDate <= currentDate && endDate >= currentDate;
        }
    }
}
