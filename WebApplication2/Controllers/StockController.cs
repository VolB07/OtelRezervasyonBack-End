namespace WebApplication2.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Stock/Consume
        [HttpPost("Consume")]
        public async Task<IActionResult> ConsumeStock([FromBody] StockConsumption stockConsumption)
        {
            var material = await _context.Materials.FindAsync(stockConsumption.MaterialId);
            if (material == null || material.StockQuantity < stockConsumption.QuantityConsumed)
            {
                return BadRequest("Yeterli stok yok.");
            }

            material.StockQuantity -= stockConsumption.QuantityConsumed;
            stockConsumption.RemainingStock = material.StockQuantity;
            _context.StockConsumptions.Add(stockConsumption);

            await _context.SaveChangesAsync();

            return Ok(stockConsumption);
        }
    }

}
