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
        public async Task<ActionResult<CampModel[]>> GetCamps()
        {
            try
            {
                var results = await _campRepository.GetAllCampsAsync();

                CampModel[] campModels = _mapper.Map<CampModel[]>(results);

                return Ok(campModels);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpGet]
        [Route("{moniker}")]
        public async Task<ActionResult<CampModel>> GetCamp(string moniker)
        {
            try
            {
                var result = await _campRepository.GetCampAsync(moniker);

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