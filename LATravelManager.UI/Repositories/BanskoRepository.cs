using LATravelManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LATravelManager.UI.Repositories
{
    public class BanskoRepository : GenericRepository,
                                    IBanskoRepository
    {
        public BanskoRepository() : base()
        {
        }

        public async Task<IEnumerable<Hotel>> GetAllHotelsInBanskoAsync()
        {
            return await Context.Hotels.Where(x => x.City.Id == 2).AsNoTracking().ToListAsync();
        }

        public async Task<List<Hotel>> GetAllHotelsWithRoomsInBanskoAsync(DateTime minDay, DateTime maxDay)
        {
            return await Context.Hotels.Where(x => x.City.Id == 2)
                .Include(x => x.City)
                .Include(x => x.HotelCategory)
                .Include(x => x.Rooms.Select(r => r.DailyBookingInfo))
                .Where(x => x.Rooms.Any(z => z.StartDate <= maxDay && z.EndDate > minDay))
                .ToListAsync();
        }

       
    }
}