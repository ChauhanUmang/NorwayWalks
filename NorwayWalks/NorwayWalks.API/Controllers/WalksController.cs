using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorwayWalks.API.Models.DTO;
using NorwayWalks.API.Repositories;

namespace NorwayWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalksRepository _walkRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;
        private readonly IMapper _mapper;

        public WalksController(IWalksRepository walkRepository,IRegionRepository regionRepository, 
            IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
            _regionRepository = regionRepository;
            _walkDifficultyRepository = walkDifficultyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walks = await _walkRepository.GetAllAsync();

            var walksDTO = _mapper.Map<List<Models.DTO.WalksDTO>>(walks);
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //Get walk domain object from database
            var walk = await _walkRepository.GetAsync(id);

            //if null, return not found
            if (walk is null)
            {
                return NotFound();
            }

            //change domain object to DTO object
            var walkDTO = _mapper.Map<Models.DTO.WalksDTO>(walk);

            //return Ok response with DTO object
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody]AddWalkRequest addWalkRequest)
        {
            //validate request
            if (! (await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }

            //Convert DTO to domain object
            var walk = new Models.Domain.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            //pass domain object to repository to persist this
            var response = await _walkRepository.AddAsync(walk);

            //convert the domain object back to DTO
            var responseDTO = _mapper.Map<Models.DTO.WalksDTO>(response);

            //send DTO response back to client
            return CreatedAtAction(nameof(GetWalkAsync), new { id = responseDTO.Id }, responseDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody]UpdateWalksRequest updateWalksRequest)
        {
            //change DTO model to domain model
            var walk = new Models.Domain.Walk()
            {
                Name = updateWalksRequest.Name,
                Length = updateWalksRequest.Length,
                RegionId = updateWalksRequest.RegionId,
                WalkDifficultyId = updateWalksRequest.WalkDifficultyId
            };

            //update walk using repository
            var updatedWalks = await _walkRepository.UpdateAsync(id,walk);
            //if null return not found
            if(updatedWalks is null)
            {
                return NotFound();
            }

            //convert domain to DTO
            var walkDTO = _mapper.Map<Models.DTO.WalksDTO>(updatedWalks);

            //return Ok response
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //Get the region from the database
            var walk = await _walkRepository.DeleteAsync(id);

            //if null, send notfound
            if(walk is null)
            {
                return NotFound();
            }

            //Change to DTO model
            var walkDTO = _mapper.Map<Models.DTO.WalksDTO>(walk);
            return Ok(walkDTO);
        }

        #region validation

        private async Task<bool> ValidateAddWalkAsync(AddWalkRequest addWalkRequest)
        {
            //if(addWalkRequest == null)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest),
            //        $"{nameof(addWalkRequest)} cannot be empty.");
            //    return false;
            //}

            //if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Name),
            //        $"{nameof(addWalkRequest.Name)} cannot be null or empty or white space.");
            //}

            //if(addWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Length),
            //        $"{nameof(addWalkRequest.Length)} cannot be less than or equal to zero.");
            //}

            var region = await _regionRepository.GetAsync(addWalkRequest.RegionId);
            if(region is null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId),
                    $"{nameof(addWalkRequest.RegionId)} is invalid.");
            }

            var walkDifficulty = await _walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if(walkDifficulty is null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
                    $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid.");
            }

            if(ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion

    }
}
