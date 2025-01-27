using Core.DTOs;
using Core.Entities;
using DataAccess;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void RegisterUser(RegisterDto dto)
        {
            var existingUser = _userRepository.GetUserByEmail(dto.Email);
            if (existingUser != null)
            {
                throw new Exception("Bu email allaqachon mavjud.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password)
            };

            _userRepository.Register(user);
        }

        public bool LoginUser(LoginDto dto, out User? user)
        {
            user = _userRepository.GetUserByEmail(dto.Email);
            if (user == null)
                return false;

            return VerifyPassword(dto.Password, user.PasswordHash);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}
