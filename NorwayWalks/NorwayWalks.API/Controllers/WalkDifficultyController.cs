using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorwayWalks.API.Models.Domain;
using NorwayWalks.API.Models.DTO;
using NorwayWalks.API.Repositories;

namespace NorwayWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;
        private readonly IMapper _mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            _walkDifficultyRepository = walkDifficultyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {
            //Get list of all domain objects
            var walkDifficulties = await _walkDifficultyRepository.GetAllAsync();
            
            //Change domain objects to DTO
            var walkDifficultyDTOs = _mapper.Map<List<Models.DTO.WalkDifficultyDTO>>(walkDifficulties);
            return Ok(walkDifficultyDTOs);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficulty")]
        public async Task<IActionResult> GetWalkDifficulty(Guid id)
        {
            //get domain object
            var walkDifficulty = await _walkDifficultyRepository.GetAsync(id);

            //if null return not found
            if(walkDifficulty is null)
            {
                return NotFound();
            }

            //change domain to DTO
            var walkDifficultyDTO = _mapper.Map<Models.DTO.WalkDifficultyDTO>(walkDifficulty);

            //Return ok response
            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //change DTO object to domain object
            var walkDifficulty = new WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code
            };

            //add DTO object to database for it to persist
            var response = await _walkDifficultyRepository.AddAsync(walkDifficulty);

            //change domain object back to DTO
            var responseDTO = _mapper.Map<Models.DTO.WalkDifficultyDTO>(response);

            //Return response
            return CreatedAtAction(nameof(GetWalkDifficulty), new { id = responseDTO.Id }, responseDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //change DTO object to domain object
            var existingWalkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code
            };

            //update DTO object
            var resposne = await _walkDifficultyRepository.UpdateAsync(id, existingWalkDifficulty);

            //if null return not found
            if(resposne is null)
            {
                return NotFound();
            }

            //convert domain to DTO
            var walkDifficultyDTO = _mapper.Map<Models.DTO.WalkDifficultyDTO>(resposne);

            //return response
            return Ok(walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            //Get the walk difficulty from database
            var walkDifficulty =await _walkDifficultyRepository.DeleteAsync(id);
            //if null, send not found
            if(walkDifficulty is null)
            {
                return NotFound();
            }
            //change Domain to DTO object
            var walkDifficultyDTO = _mapper.Map<Models.DTO.WalkDifficultyDTO>(walkDifficulty);
            //return response
            return Ok(walkDifficultyDTO);
        }
    }
}
