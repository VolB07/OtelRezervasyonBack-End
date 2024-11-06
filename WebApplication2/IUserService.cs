using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

public interface IUserService
{
    void AddUser(Users user);
    Users GetUserByEmail(string email);
    bool ValidateUser(string email, string password);
}

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Şifreyi hash'lemek için bir metod
    private string HashPassword(string password)
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] salt = new byte[16];
            rng.GetBytes(salt);
            var hash = PBKDF2(password, salt);
            return Convert.ToBase64String(salt) + ":" + hash;
        }
    }

    // PBKDF2 algoritmasını kullanarak şifreyi hash'leyen bir metod
    private string PBKDF2(string password, byte[] salt)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
        {
            byte[] hash = pbkdf2.GetBytes(20);
            return Convert.ToBase64String(hash);
        }
    }

    // Girilen şifreyi doğrulayan bir metod
    private bool VerifyPassword(string enteredPassword, string storedPassword)
    {
        var parts = storedPassword.Split(':');
        var salt = Convert.FromBase64String(parts[0]);
        var hash = parts[1];

        // Girilen şifreyi hash'le
        var hashToVerify = PBKDF2(enteredPassword, salt);

        return hashToVerify == hash; // Doğrulama
    }

    public void AddUser(Users user)
    {
        // Önce mevcut e-posta var mı diye kontrol ediyoruz
        var existingUser = _context.Users.FirstOrDefault(u => u.email == user.email);

        if (existingUser != null)
        {
            // E-posta zaten varsa hata fırlatıyoruz
            throw new Exception("Bu e-posta zaten kullanılıyor.");
        }

        // Şifreyi hash'le
        user.password = HashPassword(user.password);

        // E-posta yoksa kullanıcıyı ekle
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public Users GetUserByEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentNullException(nameof(email), "E-posta boş olamaz.");
        }

        return _context.Users.FirstOrDefault(u => u.email == email); // Kullanıcıyı geri döndür
    }
    public bool ValidateUser(string email, string password)
    {
        var user = GetUserByEmail(email) as Users;
        if (user == null)
            return false;

        return VerifyPassword(password, user.password); // VerifyPassword metodu daha önce tanımlanmış olmalı
    }

    public bool ValidatePassword(string inputPassword, string storedBase64Password)
    {
        byte[] decodedBytes = Convert.FromBase64String(storedBase64Password);
        string storedPassword = System.Text.Encoding.UTF8.GetString(decodedBytes);
        return inputPassword == storedPassword;
    }
    public Users Login(string email, string password)
    {
        var user = GetUserByEmail(email); // E-posta ile kullanıcıyı bul

        if (user == null || !VerifyPassword(password, user.password))
        {
            throw new Exception("Geçersiz e-posta veya şifre.");
        }

        return user; // Giriş başarılı
    }
}
