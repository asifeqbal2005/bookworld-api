using bookworld.core.DTO.UseCaseRequests;
using bookworld.core.DTO.UseCaseResponses;
using System;
using System.Collections.Generic;
using System.Text;

namespace bookworld.core.Interfaces.UseCases
{
    public interface ILoginUseCase : IUseCaseRequestHandler<LoginRequest, LoginResponse>
    {
    }
}
