﻿using ECommerceProjectWithWebAPI.Business.Abstract;
using ECommerceProjectWithWebAPI.Business.Constants;
using ECommerceProjectWithWebAPI.Core.Helpers.JWT;
using ECommerceProjectWithWebAPI.Core.Utilities.Responses;
using ECommerceProjectWithWebAPI.DAL.Abstract;
using ECommerceProjectWithWebAPI.Entities.Concrete;
using ECommerceProjectWithWebAPI.Entities.Dtos.UserDtos;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceProjectWithWebAPI.Business.Concrete
{

    public class UserService : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly AppSettings _appSettings;
        public UserService(IUserDal userDal, IOptions<AppSettings> appSettings)
        {
            _userDal = userDal;
            _appSettings = appSettings.Value;
        }

        public async Task<ApiDataResponse<IEnumerable<UserDetailDto>>> GetListAsync()
        {
            List<UserDetailDto> usersDetail = new List<UserDetailDto>();
            var response = await _userDal.GetListAsync();
            foreach (var item in response)
            {
                usersDetail.Add(new UserDetailDto()
                {
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Gender = item.Gender == true ? "Erkek" : "Kadın",
                    DateOfBirth = item.DateOfBirth,
                    UserName = item.UserName,
                    Address = item.Address,
                    Email = item.Email,
                    UserId = item.Id
                });
            }

            return new SuccessApiDataResponse<IEnumerable<UserDetailDto>>(usersDetail,Messages.Listed);
        }

        public async Task<ApiDataResponse<UserDto>> GetByIdAsync(int id)
        {
            var user = await _userDal.GetAsync(x => x.Id == id);
            if (user != null)
            {
                UserDto userDto = new UserDto()
                {
                    Address = user.Address,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Gender = user.Gender,
                    UserId = user.Id
                };
                return new SuccessApiDataResponse<UserDto>(userDto,Messages.Listed);
            }
            return new ErrorApiDataResponse<UserDto>(null,Messages.NotListed);
        }

        public async Task<ApiDataResponse<UserDto>> AddAsync(UserAddDto entity)
        {
            User user = new User()
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                //Todo:CreatedDate ve CreatedUSerId düzenlenecek..
                Gender = entity.Gender,
                DateOfBirth = entity.DateOfBirth,
                UserName = entity.UserName,
                Address = entity.Address,
                CreatedDate = DateTime.Now,
                CreatedUserId = 1,
                Email = entity.Email,
                Password = entity.Password
            };

            var userAdd = await _userDal.AddAsync(user);

            UserDto userDto = new UserDto()
            {
                FirstName = userAdd.FirstName,
                LastName = userAdd.LastName,
                Gender = userAdd.Gender,
                DateOfBirth = userAdd.DateOfBirth,
                UserName = userAdd.UserName,
                Address = userAdd.Address,
                Email = userAdd.Email,
                UserId = userAdd.Id
            };

            return new SuccessApiDataResponse<UserDto>(userDto,Messages.Added);
        }

        public async Task<ApiDataResponse<UserUpdateDto>> UpdateAsync(UserUpdateDto entity)
        {
            var getUser = await _userDal.GetAsync(x => x.Id == entity.UserId);
            User user = new User()
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Gender = entity.Gender,
                DateOfBirth = entity.DateOfBirth,
                UserName = entity.UserName,
                Address = entity.Address,
                Email = entity.Email,
                Id = entity.UserId,
                CreatedDate = getUser.CreatedDate,
                CreatedUserId = getUser.CreatedUserId,
                Password = getUser.Password,
                UpdatedDate = DateTime.Now,
                UpdatedUserId = 1
            };
            var userUpdated = await _userDal.UpdateAsync(user);

            UserUpdateDto userUpdateDto = new UserUpdateDto()
            {
                FirstName = userUpdated.FirstName,
                LastName = userUpdated.LastName,
                Gender = userUpdated.Gender,
                DateOfBirth = userUpdated.DateOfBirth,
                UserName = userUpdated.UserName,
                Address = userUpdated.Address,
                Email = userUpdated.Email,
                Password = userUpdated.Password,
                UserId = userUpdated.Id
            };
            return new SuccessApiDataResponse<UserUpdateDto>(userUpdateDto,Messages.Updated);
        }

        public async Task<ApiDataResponse<bool>> DeleteAsync(int id)
        {
            var isDelete= await _userDal.DeleteAsync(id);
            return new SuccessApiDataResponse<bool>(isDelete, Messages.Deleted);
        }

        //public async Task<ApiDataResponse<AccessToken>> Authenticate(UserForLoginDto userForLoginDto)
        //{
        //    var user = await _userDal.GetAsync(x => x.UserName == userForLoginDto.UserName && x.Password == userForLoginDto.Password);
        //    if (user == null)
        //        return null;

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_appSettings.SecurityKey);
        //    var tokenDescriptor = new SecurityTokenDescriptor()
        //    {
        //        Subject=new ClaimsIdentity(new[]
        //        {
        //            new Claim(ClaimTypes.Name,user.Id.ToString())
        //        }),
        //        Expires=DateTime.UtcNow.AddDays(7),
        //        SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    AccessToken accessToken = new AccessToken()
        //    {
        //        Token = tokenHandler.WriteToken(token),
        //        Expiration = (DateTime)tokenDescriptor.Expires,
        //        UserName =user.UserName,                
        //        UserId=user.Id
        //    };
        //    return await Task.Run(() => accessToken);
        //}
    }
}
