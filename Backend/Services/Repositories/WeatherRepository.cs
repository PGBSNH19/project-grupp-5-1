using Backend.Data;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Repositories
{
    public class WeatherRepository : Repository<Weather>, IWeatherRepository
    {
        private readonly StoreDbContext _context;
        private readonly ILogger<WeatherRepository> _logger;

        public WeatherRepository(StoreDbContext context, ILogger<WeatherRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Weather> GetWeatherById(int weatherId)
        {
            return await _context.Set<Weather>().FirstOrDefaultAsync(w => w.Id == weatherId);
        }
    }
}
