using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Student")]
    [ApiController]
    [Route("api/fees")]
    public class FeesController : ControllerBase
    {
        private readonly IFeeService _feeService;

        public FeesController(IFeeService feeService)
        {
            _feeService = feeService;
        }

        //Retrieves current fee information for a student for the current semester.
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentFees([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            var fees = await _feeService.GetCurrentFeesByStudentIdAsync(studentId);
            return Ok(fees);
        }

        //Retrieves payment records by semester for a student.
        [HttpGet("payment-records")]
        public async Task<IActionResult> GetPaymentRecords([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            var paymentRecords = await _feeService.GetPaymentRecordsByStudentIdAsync(studentId);
            return Ok(paymentRecords);
        }

        
        //Retrieves fee holds for a student.
        [HttpGet("holds")]
        public async Task<IActionResult> GetFeeHolds([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            var feeHolds = await _feeService.GetFeeHoldsByStudentIdAsync(studentId);
            return Ok(feeHolds);
        }

        /// Marks a fee as paid for a student.
        [HttpPost("mark-paid")]
        public async Task<IActionResult> MarkFeeAsPaid([FromQuery] string studentId, [FromQuery] string feeId)
        {
            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(feeId))
            {
                return BadRequest("Student ID and fee ID are required.");
            }

            try
            {
                await _feeService.MarkFeeAsPaidAsync(studentId, feeId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // e.g., "Fee not found" or "Fee already paid"
            }
        }
    }
}