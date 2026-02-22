using System;
using ComponentManagement.Domain.Entities; 

namespace ComponentManagement.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    RefreshToken GenerateRefreshToken(string ipAddress);
    string HashToken(string token);

    Guid ValidateAndExtractUserId(string token);
}

