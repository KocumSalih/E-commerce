﻿using ECommerceProjectWithWebAPI.Core.Utilities.Responses;
using ECommerceProjectWithWebAPI.Entities.Dtos.Auth;
using ECommerceProjectWithWebAPI.Entities.Dtos.User;
using System.Threading.Tasks;

namespace ECommerceProjectWithWebAPI.Business.Abstract
{
    public interface IAuthService
    {
        Task<ApiDataResponse<UserDto>> LoginAsync(LoginDto loginDto);
    }
}
