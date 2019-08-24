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
                var results = await _campRepository.GetAllCampsAsync(includeTalks);

                CampModel[] campModels = _mapper.Map<CampModel[]>(results);

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

                CampModel campModels = _mapper.Map<CampModel>(result);

                return Ok(campModels);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }
    }
}