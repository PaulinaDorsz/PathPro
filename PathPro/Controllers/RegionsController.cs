using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathPro.Data;
using PathPro.Models.Domain;
using PathPro.Models.DTO;
using PathPro.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PathPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly PathProDbContext dbContext;
        private readonly IRegionRepository regionRepository;

        public RegionsController(PathProDbContext dbContext, IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }
        // Get All
        [HttpGet]
        //[Authorize]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            //var regionsDomain = await dbContext.Regions.ToListAsync();
            var regionsDomain = await regionRepository.GetAllAsync();

            var regionsDto = regionsDomain.Select(regionDomain => new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            }).ToList();

            return Ok(regionsDto);
        }

        // Get by Id
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // var region = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };

            return Ok(regionDto);
        }

        //Create
        [HttpPost]
        //[Authorize]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            if (ModelState.IsValid)
            {
                // Mapowanie DTO do domeny
                var regionDomainModel = new Region
                {
                    Code = addRegionRequestDto.Code,
                    Name = addRegionRequestDto.Name,
                    RegionImageUrl = addRegionRequestDto.RegionImageUrl
                };

                // Dodanie do kontekstu i zapisanie zmian
                //await dbContext.Regions.AddAsync(regionDomainModel);
                //await dbContext.SaveChangesAsync();

                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                // Mapowanie domeny do DTO
                var regionDto = new RegionDto
                {
                    Id = regionDomainModel.Id,
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl
                };

                // Zwrócenie odpowiedzi 201 Created
                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            }

            else

            {
                return BadRequest(ModelState);
            }
        }
        // Update
        [HttpPut]
        [Route("{id:Guid}")]
        //[Authorize]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Mapowanie DTO to Domain Model
                    var regionDomainModel = new Region
                    {
                        Code = updateRegionRequestDto.Code,
                        Name = updateRegionRequestDto.Name,
                        RegionImageUrl = updateRegionRequestDto.RegionImageUrl
                    };

                    // Aktualizacja modelu domenowego
                    regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                    if (regionDomainModel == null)
                    {
                        return NotFound();
                    }

                    var regionDto = new RegionDto
                    {
                        Id = regionDomainModel.Id,
                        Code = regionDomainModel.Code,
                        Name = regionDomainModel.Name,
                        RegionImageUrl = regionDomainModel.RegionImageUrl
                    };

                    return Ok(regionDto);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Obsługa błędu aktualizacji współbieżności
                return StatusCode(StatusCodes.Status409Conflict, new { Message = ex.Message });
            }
        }

        //Delete 
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //dbContext.Regions.Remove(regionDomainModel); //pozostaje zsynchronizowany 
            //await dbContext.SaveChangesAsync();

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);

        }
    }
}


