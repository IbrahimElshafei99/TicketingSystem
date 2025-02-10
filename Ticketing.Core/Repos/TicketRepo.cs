using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Core.Context;
using Ticketing.Core.DTO;
using Ticketing.Core.Interfaces;
using Ticketing.Data.Models;

namespace Ticketing.Core.Repos
{
    public class TicketRepo : ITicketRepo
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TicketRepo> _logger;

        public TicketRepo(AppDbContext appDbContext, ILogger<TicketRepo> logger)
        {
            _context = appDbContext;
            _logger = logger;
        }

        public async Task<DataTable> ExecuteTicketSP(int id = default, DateTime? activeDate = null, string address = null, decimal? latitude = null,
                                        decimal? longitude = null, string comment = null, bool? checkIn = null,int? statusId = null,
                                        int? ticketTypeId = null, string? filterString = null, string? columnList = null)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (var connection = _context.Database.GetDbConnection())
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SP_Ticket";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@Id", id == 0 ? -1 : id));
                        command.Parameters.Add(new SqlParameter("@ActiveDate", activeDate ?? (object)DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@Address", address ?? (object)DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@Latitude", latitude ?? (object)DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@Longitude", longitude ?? (object)DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@Comment", comment ?? (object)DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@CheckIn", checkIn ?? (object)DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@StatusId", statusId ?? (object)DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@TicketTypeId", ticketTypeId ?? (object)DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@filterString", filterString ?? (object)DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@columnList", columnList ?? (object)DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@groupByList", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@pageno", -1));
                        command.Parameters.Add(new SqlParameter("@pageSize", -1));

                        // Construct SQL log
                        var sqlLog = new StringBuilder();
                        sqlLog.Append("EXEC " + command.CommandText + " ");

                        foreach (SqlParameter param in command.Parameters)
                        {
                            sqlLog.AppendFormat("{0} = '{1}', ", param.ParameterName, param.Value);
                        }

                        // Trim last comma
                        if (command.Parameters.Count > 0)
                        {
                            sqlLog.Length -= 2;
                        }

                        // Log the exact SQL execute statement
                        _logger.LogInformation("Executing SQL: {SqlQuery}", sqlLog.ToString());

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            await Task.Run(() => dataTable.Load(reader));
                            _logger.LogInformation(string.Concat("resulting rows count ", dataTable.Rows.Count));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in SP: {message} ", ex.Message);
                Console.WriteLine(ex.Message);
            }

            return dataTable;
        }


        public async Task<Ticket> GetById(int id)
        {
            return await _context.Ticket.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TicketType> GetTicketType(string T_type)
        {
            return await _context.TicketType.FirstOrDefaultAsync(x => x.Type_Text.ToLower() == T_type.ToLower());

        }
    }

}
