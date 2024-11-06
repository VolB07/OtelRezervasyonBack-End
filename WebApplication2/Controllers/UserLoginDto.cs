public class UserService
{
    private readonly YourDbContext _context;

    public UserService(YourDbContext context)
    {
        _context = context;
    }

    public User GetUserByEmailAndPassword(string email, string password)
    {
        // Şifreyi genellikle veritabanında hashlenmiş olarak saklarsınız,
        // bu örnekte basit bir kontrol yapılıyor:
        return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
    }
}
