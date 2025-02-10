using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using Ticketing.Business.Interfaces;
using Ticketing.Core.DTO;
using Ticketing.Core.Interfaces;
using Ticketing.Data.Models;
using Formatting = Newtonsoft.Json.Formatting;

namespace Ticketing.Business.Services
{
    public class TicketService : ITicketService
    {
        const string apiKey = "AIzaSyBL6a6XwUsRkygz7hHPI0Lc9RLLRU74Dpw";
        private readonly ITicketRepo _ticketRepo;
        private readonly IGenericsRepo<Ticket> _genericsRepo;
        private readonly ILogger<TicketService> _logger;
        private readonly HttpClient _httpClient;

        public TicketService(ITicketRepo ticketRepo, IGenericsRepo<Ticket> genericsRepo, ILogger<TicketService> logger, HttpClient httpClient)
        {
            _ticketRepo = ticketRepo;
            _genericsRepo = genericsRepo;
            _logger = logger;
            _httpClient = httpClient;
        }

        // Convert the returned DataTable to json 
        public string ConvertDataTableToJson(DataTable dataTable)
        {
            string jsonString = string.Empty;

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                jsonString = JsonConvert.SerializeObject(dataTable, Formatting.Indented);
            }

            return jsonString;
        }


        public async Task<DataTable> GetAllTickets()
        {
            return await _ticketRepo.ExecuteTicketSP();
        }
        public async Task<Ticket> GetById(int id)
        {
            return await _ticketRepo.GetById(id);
        }

        public async Task OpenTicket(TicketForSTeamDTO ticketDto)
        {
            TicketType ticketType= new TicketType();
            try
            {
                // Get TicketType Id
                ticketType = await _ticketRepo.GetTicketType(ticketDto.TicketTypeText);
                if (ticketType == null)
                    throw new NullReferenceException("Invalid ticket type");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Ticket Type: {message}",ex.Message);
            }
            await _ticketRepo.ExecuteTicketSP(longitude: ticketDto.Longitude, latitude: ticketDto.Latitude, 
                                        address:ticketDto.Address, activeDate:ticketDto.ActiveDate, comment:ticketDto.Comment, ticketTypeId: ticketType.Id);
        }

        
        // Update ticket by operator
        // Operator will add his long & lat.
        // If the distance between ticket and operator long & lat less than 3 km,
        // then checkin will be true and he could see the rest of details (ticketType, close or refuse).
        public async Task<DataTable> ModifyTicketByOperator(TicketForOperatorDTO ticket)
        {
            try
            {
                // Get the ticket by Id for comparison
                var assignedTicket = await _ticketRepo.GetById(ticket.Id);
                if (assignedTicket != null)
                {
                    string ticketDestination = assignedTicket.Latitude.ToString() + "," + assignedTicket.Longitude.ToString();
                    string operatorOrigin = ticket.OperatorLattitude.ToString() +","+ticket.OperatorLongitude.ToString();
                    var distance = await GetDistance(operatorOrigin, ticketDestination);
                    if (distance == -1)
                        throw new Exception("Error in Distance Matrix API response");

                    // if the distance less than 3 km the ticket should be updated (checkin = true)
                    if (distance < 3)
                    {
                        _logger.LogInformation("Distance less than 3 km");
                        // pass all ticket info to not replace the ticket repo with null values
                        var modifiedTicket = await _ticketRepo.ExecuteTicketSP(checkIn: true, id: assignedTicket.Id, longitude: assignedTicket.Longitude, latitude: assignedTicket.Latitude,
                                        address: assignedTicket.Address, activeDate: assignedTicket.ActiveDate, comment: assignedTicket.Comment, ticketTypeId: assignedTicket.Id);

                        // Should return to the operator a selected data (ActiveDate, Address, Comment, CheckIn, TicketTypeId) 
                        return modifiedTicket;
                    }
                    else
                        return null;
                }
                else
                {
                    throw new NullReferenceException("The ticket not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting ticket: {message}", ex.Message);
                return null;
            }

            
        }

        private async Task<decimal> GetDistance(string origin,  string destination)
        {
            string url = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={origin}&destinations={destination}&key={apiKey}";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        JObject data = JObject.Parse(jsonResponse);
                        if (data == null)
                        {
                            throw new NullReferenceException("Distance Matrix API response is Null");
                        }
                        // Extract distance in meters and convert to KM
                        int distanceMeters = (int)data["rows"][0]["elements"][0]["distance"]["value"];
                        decimal distanceKm = distanceMeters / 1000.0m; // Convert to KM
                        _logger.LogInformation("The distance between operator and ticket destination: {d}", distanceKm);
                        return distanceKm;
                    }
                    else
                    {
                        throw new Exception("Error in Distance Matrix API response status code");
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Error in Distance Matrix API response: {ex}", ex.Message);
                return -1;
            }
            
        }

        /*public async Task OpenTicket(TicketForSTeamDTO ticketDto)
        {
            var ticketType = await _ticketRepo.GetTicketType(ticketDto.TicketTypeText);
            int typeId;
            if (ticketType == null)
                typeId = 0;
            else
                typeId = ticketType.Id;
            Ticket ticket = new Ticket
            {
                ActiveDate = ticketDto.ActiveDate,
                Address = ticketDto.Address,
                Comment = ticketDto.Comment,
                Latitude = ticketDto.Latitude,
                Longitude = ticketDto.Longitude,
                TicketTypeId = typeId,
            };
            await _genericsRepo.AddRecord(ticket);

        }*/
    }
}
