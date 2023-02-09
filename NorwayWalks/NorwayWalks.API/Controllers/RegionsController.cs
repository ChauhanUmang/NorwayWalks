using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorwayWalks.API.Models.Domain;
using NorwayWalks.API.Models.DTO;
using NorwayWalks.API.Repositories;

namespace NorwayWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync() 
        {
            var regions = await _regionRepository.GetAllAsync();

            //return DTO regions
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population
            //    };

            //    regionsDTO.Add(regionDTO);
            //});

            var regionsDTO = _mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await _regionRepository.GetAsync(id);

            if(region is null)
            {
                return NotFound();
            }

            var regionDTO = _mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(AddRegionRequest addRegionRequest)
        {
            //validate the request
            //if (!ValidateAddRegionAsync(addRegionRequest))
            //{
            //    return BadRequest(ModelState);
            //}

            //Request(DTO) to domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population
            };

            //pass details to repository
            var response = await _regionRepository.AddAsync(region);

            //Convert back to DTO
            var responseDTO =  _mapper.Map<Models.DTO.Region>(response);
            return CreatedAtAction(nameof(GetRegionAsync), new {id = responseDTO.Id}, responseDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get the region from the database
            var region = await _regionRepository.DeleteAsync(id);

            //if null, send notfound
            if (region is null)
            {
                return NotFound();
            }

            //if region not null, delete it and convert response to DTO model
            var regionDTO = _mapper.Map<Models.DTO.Region>(region);
            //return Ok response
            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id, [FromBody]UpdateRegionRequest updateRegionRequest)
        {
            //Convert DTO to domain model
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population
            };

            //Update region using repository
            var updatedregion = await _regionRepository.UpdateAsync(id,region);

            //if null return not found
            if(updatedregion is null)
            {
                return NotFound();
            }
            //Convert domain back to DTO
            var updateRegionDTO = _mapper.Map<Models.DTO.Region>(updatedregion);

            //Return ok response
            return Ok(updateRegionDTO);
        }

        #region Private Methods

        //private bool ValidateAddRegionAsync(AddRegionRequest addRegionRequest)
        //{
        //    if(addRegionRequest is null)
        //    {
        //        ModelState.AddModelError(nameof(addRegionRequest),
        //            $"Add Region data is required.");
        //        return false;
        //    }

        //    if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
        //    {
        //        ModelState.AddModelError(nameof(addRegionRequest.Code), 
        //            $"{nameof(addRegionRequest.Code)} cannot be null or empty or white space.");
        //    }

        //    if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
        //    {
        //        ModelState.AddModelError(nameof(addRegionRequest.Name),
        //            $"{nameof(addRegionRequest.Name)} cannot be null or empty or white space.");
        //    }

        //    if(addRegionRequest.Area <= 0)
        //    {
        //        ModelState.AddModelError(nameof(addRegionRequest.Area),
        //            $"{nameof(addRegionRequest.Area)} cannot be less than or equal to zero.");
        //    }

        //    if (addRegionRequest.Population < 0)
        //    {
        //        ModelState.AddModelError(nameof(addRegionRequest.Population),
        //            $"{nameof(addRegionRequest.Population)} cannot be less than zero.");
        //    }

        //    if(ModelState.ErrorCount > 0)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        #endregion
    }
}
