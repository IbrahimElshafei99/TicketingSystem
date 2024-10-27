using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Core.DTO;

namespace Ticketing.Business.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegesterUser(UserRegistrationDTO user);
        Task AddUserRole();
        Task<string> Login(UserLoginDTO loginDTO);


    }
}
