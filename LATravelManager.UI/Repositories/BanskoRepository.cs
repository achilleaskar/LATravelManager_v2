﻿using LATravelManager.DataAccess;
using LATravelManager.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LATravelManager.UI.Repositories
{
    public class BanskoRepository : GenericRepository<MainDatabase>,
                                    IBanskoRepository
    {
        protected BanskoRepository(MainDatabase context) : base(context)
        {
        }

        public async Task<IEnumerable<Hotel>> GetAllHotelsInBanskoAsync()
        {
            return await Context.Hotels.Where(x => x.City.Id == 2).ToListAsync();
        }
    }
}