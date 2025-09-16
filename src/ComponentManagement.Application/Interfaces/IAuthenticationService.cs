using System;
using ComponentManagement.Domain.Entities;


namespace ComponentManagement.Application.Interfaces

{

    public interface IAuthenticationService

    {

        bool VerifyPassword(string password, string passwordHash);

        string GenerateJwtToken(ComponentManagement.Domain.Entities.User user);

    }
    public interface IUserService
{
    Task<User> GetUserByUsernameAsync(string username);
}

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }


}
