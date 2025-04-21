using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<UserRepo> _logger;


    public UserRepo(AppDbContext context, ILogger<UserRepo> logger)
    {
        _context = context;
        _logger = logger;
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



    public async Task<string> GetRoleById(int roleId)
    {
        try
        {
            if (roleId == 0)
                return null;

            var role = await _context.Roles.FirstOrDefaultAsync(x => x.RoleId == roleId);
            if (role == null)
                return null;

            return role.RoleName;

        }
        catch (Exception ex)
        {
            _logger.LogError("Error in GetRoleById: {message}", ex.Message);
            return null;
        }
        finally
        {
            _logger.LogInformation("GetRoleById executed");

        }
    }

    public async Task<int> GetRoleIdByRoleName(string roleName)
    {
        try
        {
            if (string.IsNullOrEmpty(roleName))
                return 0;

            var role = await _context.Roles.FirstOrDefaultAsync(x => x.RoleName == roleName);
            if (role == null)
                return 0;

            return role.RoleId;
        }
        catch (Exception ex)
        {
            _logger.LogError( "Error in GetRoleIdByRoleName: {message}", ex.Message);
            return 0;
        }
        
    }   
    public async Task<int>GetCityIdByCityName(string cityName)
    {
        try
        {
            if (string.IsNullOrEmpty(cityName))
                return 0;

            var city = await _context.City.FirstOrDefaultAsync(x => x.CityName == cityName);
            if (city == null)
                return 0;

            return city.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in GetCityIdByCityName: {message}", ex.Message);
            return 0;
        }
            
    }

    public async Task<List<User>> GetOperatorsByCity(string city)
    {
        try
        {
            if (string.IsNullOrEmpty(city))
                return null;

            int opratorId = await GetRoleIdByRoleName("Operator");
            int cityId = await GetCityIdByCityName(city);
            if (opratorId == 0 || cityId == 0)
                return null;

            var users = await _context.User_City
                .Include(x => x.User)
                .Where(x => x.CityId == cityId && x.User.RoleId == opratorId)
                .Select(x => x.User).ToListAsync();
            if (users == null || users.Count == 0)
                return null;

            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in GetOperatorsByCity: {message}", ex.Message);
            return null;
        }
        
        

    }

}
