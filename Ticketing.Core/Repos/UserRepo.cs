using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Core.Context;
using Ticketing.Core.DTO;
using Ticketing.Core.Interfaces;
using Ticketing.Data.Models;

namespace Ticketing.Core.Repos;

public class UserRepo : IUserRepo
{
    private readonly AppDbContext _context;

    public UserRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsRegistered(UserRegistrationDTO R_user)
    {
        var user = await _context.User.FirstOrDefaultAsync(x => x.PhoneNumber == R_user.PhoneNumber
                         || x.Social_Id == R_user.NationalId || x.Email == R_user.Email);
        return user != null;
    }

    public async Task<User> FindUserBy(string PN)
    {
        return await _context.User.FirstOrDefaultAsync(x=> x.PhoneNumber == PN);
    }

    public async Task<bool> AddUser(UserRegistrationDTO userRegistration)
    {
        if(await IsRegistered(userRegistration))
        {
            return false;
        }
        User newUser = new User()
        {
            PhoneNumber = userRegistration.PhoneNumber,
            UserName = userRegistration.FullName,
            Social_Id = userRegistration.NationalId,
            Email = userRegistration.Email,
            Password = userRegistration.Password,
            Address = userRegistration.Address,
            StatusId = 6
        };
        await _context.User.AddAsync(newUser); 
        await _context.SaveChangesAsync();
        return true;
    }
}
