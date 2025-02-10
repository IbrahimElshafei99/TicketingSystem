using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Core.DTO;
public class TicketForOperatorDTO
{
    public int Id { get; set; }
    public decimal? OperatorLattitude { get; set; }
    public decimal? OperatorLongitude { get; set; }
    public string? Comment { get; set; }
    public bool? CheckIn { get; set; }
    public string? StatusTxt { get; set; }
}
