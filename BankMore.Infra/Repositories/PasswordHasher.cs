using System.Security.Cryptography;
using System.Text;
using BankMore.Core.Interfaces;

namespace BankMore.Infra.Repositories;

public class PasswordHasher : IPasswordHasher
{
    // Gera um salt aleatório.
    public string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }

    // Gera o hash da senha combinando com o salt.
    public string HashPassword(string senha, string salt)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha + salt));
            return Convert.ToBase64String(hashBytes);
        }
    }

    // Verifica se a senha fornecida corresponde ao hash armazenado.
    public bool VerifyPassword(string password, string hashedPassword, string salt)
    {
        var hash = HashPassword(password, salt);
        return hash == hashedPassword;
    }
}