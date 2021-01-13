using Backend.Models;
using System.Threading.Tasks;

namespace Backend.Services.Interfaces
{
    public interface IWeatherRepository : IRepository<Weather>
    {
        Task<Weather> GetWeatherById(int weatherId);
    }
}