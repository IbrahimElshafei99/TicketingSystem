using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Core.DTO;

namespace Ticketing.Business.Interfaces;
public interface IUserService
{
    Task<List<AssignOperatorDTO>> GetAssignedOperators(string city, DateTime date);
}
