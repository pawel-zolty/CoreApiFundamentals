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

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository _campRepository;
        private readonly IMapper _mapper;

        public CampsController(ICampRepository campRepository, IMapper mapper)
        {
            this._campRepository = campRepository;
            this._mapper = mapper;
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

                if(!results.Any())
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

    }
}