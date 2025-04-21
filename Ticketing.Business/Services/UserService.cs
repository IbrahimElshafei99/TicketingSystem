using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Business.Interfaces;
using Ticketing.Core.DTO;
using Ticketing.Core.Interfaces;

namespace Ticketing.Business.Services;
public class UserService : IUserService
{
    private readonly IUserRepo _userRepo;
    private readonly ITicketRepo _ticketRepo;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepo userRepo, ILogger<UserService> logger, ITicketRepo ticketRepo)
    {
        _userRepo = userRepo;
        _logger = logger;
        _ticketRepo = ticketRepo;
    }

    public async Task<List<AssignOperatorDTO>> GetAssignedOperators(string city, DateTime date)
    {
        try
        {
            if (city.IsNullOrEmpty() || date == null)
            {
                throw new NullReferenceException("City or date is null");
            }
            var operators = await _userRepo.GetOperatorsByCity(city);
            var assignOperators = new List<AssignOperatorDTO>();
            foreach (var opt in operators)
            {
                var ticketsCount = await _ticketRepo.GetTicketsCountByOperatorId(opt.Id, date);
                assignOperators.Add(new AssignOperatorDTO
                {
                    OperatorId = opt.Id,
                    OperatorName = opt.UserName,
                    TicketsCount = ticketsCount
                });
            }
            return assignOperators;
        }
        catch(Exception ex)
        {
            _logger.LogError("Error in GetAssignedOperators: {message}", ex.Message);
            return null;
        }
            
    }
}
