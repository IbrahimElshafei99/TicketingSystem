﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Core.DTO;
using Ticketing.Data.Models;

namespace Ticketing.Core.Interfaces
{
    public interface IUserRepo
    {
        Task<bool> AddUser(UserRegistrationDTO userRegistration);
        Task<User> FindUserBy(string PN);
        Task<bool> IsRegistered(UserRegistrationDTO R_user);
        Task<string> GetRoleById(int roleId);

        Task<int> GetRoleIdByRoleName(string roleName);
        Task<int> GetCityIdByCityName(string cityName);
        Task<List<User>> GetOperatorsByCity(string city);
    }
}
