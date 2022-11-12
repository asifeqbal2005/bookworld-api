using bookworld.core.DTO.UseCaseResponses;
using bookworld.core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace bookworld.core.DTO.UseCaseRequests
{
    public class LoginRequest : IUseCaseRequest<LoginResponse>
    {
        public string UserName { get; }
        public string Password { get; }

        public LoginRequest(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
