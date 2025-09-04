namespace BankMore.Core.Interfaces;

public interface IPasswordHasher
{
    string HashPassword(string password, string salt);
    string GenerateSalt();
    bool VerifyPassword(string password, string hashedPassword, string salt);
}