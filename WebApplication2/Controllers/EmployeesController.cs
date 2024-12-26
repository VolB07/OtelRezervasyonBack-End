using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Çalışanları listeleme
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees
                .Where(e => !e.deleted)
                .ToListAsync();

            return Ok(employees);
        }

        // 2. Belirli bir otelde çalışanları listeleme
        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetEmployeesByHotel(int hotelId)
        {
            var employees = await _context.Employees
                .Where(e => e.hotel_id == hotelId && !e.deleted)
                .ToListAsync();

            if (!employees.Any())
            {
                return NotFound("Bu otelde çalışan bulunamadı.");
            }

            return Ok(employees);
        }

        // 3. Çalışan ekleme
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Çalışan bilgileri eksik.");
            }

            if (string.IsNullOrEmpty(employee.email) || string.IsNullOrEmpty(employee.phone))
            {
                return BadRequest("E-posta ve telefon alanları gereklidir.");
            }

            if (employee.user_id == 0 || employee.hotel_id == 0)
            {
                return BadRequest("Kullanıcı ID ve Otel ID gereklidir.");
            }

            employee.created_at = DateTime.UtcNow;
            employee.updated_at = DateTime.UtcNow;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployees), new { id = employee.id }, employee);
        }

        // 4. Çalışan güncelleme
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee updatedEmployee)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null || employee.deleted)
            {
                return NotFound("Çalışan bulunamadı.");
            }

            employee.name = updatedEmployee.name;
            employee.role = updatedEmployee.role;
            employee.email = updatedEmployee.email;
            employee.phone = updatedEmployee.phone;
            employee.hotel_id = updatedEmployee.hotel_id;
            employee.updated_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        // 5. Çalışan silme (Soft delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null || employee.deleted)
            {
                return NotFound("Çalışan bulunamadı.");
            }

            employee.deleted = true;
            employee.updated_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
