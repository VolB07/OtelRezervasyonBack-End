using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IUserService _userService;

    // Constructor injection with ApplicationDbContext and IUserService
    public UsersController(ApplicationDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }




    // GET api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    // POST api/users
    [HttpPost]
    public async Task<ActionResult<Users>> PostUser(Users user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUsers), new { id = user.id }, user);
    }

    // POST api/users/register
    [HttpPost("register")]
    public IActionResult Register([FromBody] Users user)
    {
        try
        {
            // Kullanıcının aynı e-posta ile zaten kayıtlı olup olmadığını kontrol et
            var existingUser = _userService.GetUserByEmail(user.email);
            if (existingUser != null)
            {
                // Eğer kullanıcı zaten varsa, 400 BadRequest ile hata mesajı döndür
                return BadRequest(new { message = "Bu e-posta zaten kullanılıyor." });
            }

            // Yeni kullanıcıyı ekle
            _userService.AddUser(user);
            return Ok(new { message = "Kayıt başarılı" });
        }
        catch (Exception ex)
        {
            // Hata durumunda log'la ve 500 InternalServerError dön
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { message = "Sunucu hatası, lütfen tekrar deneyin" });
        }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel loginModel)
    {
        try
        {
            var isValidUser = _userService.ValidateUser(loginModel.Email, loginModel.Password);
            if (!isValidUser)
            {
                return Unauthorized(new { message = "Geçersiz e-posta veya şifre." });
            }

            // Kullanıcı bilgilerini al (örneğin, kullanıcı ID'si ve rolü)
            var user = _userService.GetUserByEmail(loginModel.Email);

            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            // Kullanıcının rolü
            var role = user.role;

            // JWT token oluştur
            var token = _userService.GenerateJwtToken(loginModel.Email);

            return Ok(new
            {
                message = "Giriş başarılı.",
                token = token,
                userId = user.id,
                role = role // Rol bilgisi yanıt içerisinde gönderiliyor
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { message = "Sunucu hatası, lütfen tekrar deneyin." });
        }
    }






    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // GET: api/Users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        return Ok(user);
    }

    // GET: api/users/role/{id}
    [HttpGet("role/{id}")]
    public async Task<IActionResult> GetUserRole(int id)
    {
        try
        {
            // Kullanıcıyı ID'ye göre veritabanında bul
            var user = await _context.Users.FindAsync(id);

            // Kullanıcı bulunamazsa hata mesajı döndür
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Kullanıcı rolünü döndür
            return Ok(new { role = user.role });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, new { message = "Server error, please try again later." });
        }
    }

    // PUT: api/Users/role/{id}
    [HttpPut("role/{id}")]
    public IActionResult AssignRoleToUser(int id, [FromBody] string role)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }

        user.role = role; // Rolü string olarak atama
        _context.SaveChanges();
        return Ok();
    }




}



