using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HoldManagementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HoldController : ControllerBase
    {
        private static readonly List<Hold> _holds = new List<Hold>();

        [HttpGet("{studentId}")]
        public IActionResult GetHolds(string studentId)
        {
            var studentHolds = _holds.Where(h => h.StudentId == studentId).ToList();
            return Ok(studentHolds);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddHold([FromBody] Hold hold)
        {
            if (string.IsNullOrEmpty(hold.StudentId) || string.IsNullOrEmpty(hold.Service))
            {
                return BadRequest("StudentId and Service are required.");
            }
            hold.Id = System.Guid.NewGuid().ToString();
            _holds.Add(hold);
            return Ok(hold);
        }

        [HttpDelete("{holdId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveHold(string holdId)
        {
            var hold = _holds.FirstOrDefault(h => h.Id == holdId);
            if (hold == null)
            {
                return NotFound();
            }
            _holds.Remove(hold);
            return NoContent();
        }
    }

    public class Hold
    {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string Service { get; set; }
        public string Reason { get; set; }
    }
}
