using AutoMapper;
using CoreCodeCamp.Models;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository _campRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public CampsController(ICampRepository campRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            this._campRepository = campRepository;
            this._mapper = mapper;
            this._linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<CampModel[]>> GetCamps(bool includeTalks = false)
        {
            try
            {
                Camp[] results = await _campRepository.GetAllCampsAsync(includeTalks);

                var campModels = _mapper.Map<CampModel[]>(results);

                return Ok(campModels);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpGet]
        [Route("{moniker}")]
        public async Task<ActionResult<CampModel>> GetCamp(string moniker, bool includeTalks = false)
        {
            try
            {
                Camp result = await _campRepository.GetCampAsync(moniker, includeTalks);

                if (result == null)
                {
                    return NotFound();
                }

                var campModels = _mapper.Map<CampModel>(result);

                return Ok(campModels);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<CampModel>> GetCampsByDate(DateTime theDate, bool includeTalks = false)
        {
            try
            {
                Camp[] results = await _campRepository.GetAllCampsByEventDate(theDate, includeTalks);

                if (!results.Any())
                {
                    return NotFound();
                }

                var campModels = _mapper.Map<CampModel[]>(results);

                return Ok(campModels);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CampModel>> CreateCamp(CampModel campModel)
        {
            try
            {
                var existingCamp = await _campRepository.GetCampAsync(campModel.Moniker);

                if (existingCamp != null)
                {
                    return BadRequest("Moniker already used");
                }

                var location = _linkGenerator.GetPathByAction("GetCamp", "Camps",
                    new { moniker = campModel.Moniker });

                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use current moniker");
                }

                var camp = _mapper.Map<Camp>(campModel);
                _campRepository.Add(camp);

                if (await _campRepository.SaveChangesAsync())
                {
                    return Created(location, _mapper.Map<CampModel>(camp));
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("{moniker}")]
        public async Task<ActionResult<CampModel>> UpdateCamp(string moniker, CampModel campModel)
        {
            try
            {
                var oldCamp = await _campRepository.GetCampAsync(moniker);

                if (oldCamp == null)
                {
                    return NotFound($"Camp with moniker: {moniker} does not exist");
                }

                _mapper.Map(campModel, oldCamp);

                if (await _campRepository.SaveChangesAsync())
                {
                    return _mapper.Map<CampModel>(oldCamp);
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("{moniker}")]
        public async Task<IActionResult> DeleteCamp(string moniker)
        {
            try
            {
                var oldCamp = await _campRepository.GetCampAsync(moniker, true);

                if (oldCamp == null)
                {
                    return NotFound($"Camp with moniker: {moniker} does not exist");
                }

                _campRepository.Delete(oldCamp);

                if(await _campRepository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }

            return BadRequest();
        }
    }
}