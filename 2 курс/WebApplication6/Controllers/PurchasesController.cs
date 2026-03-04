using Microsoft.AspNetCore.Mvc;

namespace practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseService _service;

        public PurchasesController(IPurchaseService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Purchase>> Create(PurchaseCreateDto dto)
        {
            var item = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PurchaseUpdateDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpGet("report/top-customers")]
        public async Task<ActionResult<IEnumerable<TopCustomerDto>>> GetTopCustomers([FromQuery] int limit = 5)
            => Ok(await _service.GetTopCustomersAsync(limit));
    }
}

