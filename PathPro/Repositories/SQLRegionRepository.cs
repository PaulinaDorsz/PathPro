using Microsoft.EntityFrameworkCore;
using PathPro.Data;
using PathPro.Models.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathPro.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly PathProDbContext dbContext;

        public SQLRegionRepository(PathProDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        //Delete
        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegion == null)
            {
                return null;
            }

            dbContext.Regions.Remove(existingRegion);
            await dbContext.SaveChangesAsync();
            return existingRegion;
        }

        //Get All
        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        //Get by Id
        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        //Update
        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegion == null)
            {
                return null;
            }

            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            await dbContext.SaveChangesAsync();
            return existingRegion;
        }
    }
}