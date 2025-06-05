

using ELIXIR.API.Authentication;
using ELIXIR.DATA.CORE.ICONFIGURATION;
using ELIXIR.DATA.DATA_ACCESS_LAYER.EXTENSIONS;
using ELIXIR.DATA.DATA_ACCESS_LAYER.HELPERS;
using ELIXIR.DATA.DATA_ACCESS_LAYER.MODELS;
using ELIXIR.DATA.DTOs.ONECHARGING_DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELIXIR.API.Controllers.ONECHARGING_CONTROLLER
{

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class OneCharginController : ControllerBase
    {

        public readonly IUnitOfWork _unitofWork;

        public OneCharginController(IUnitOfWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        [HttpPost]
        [ApiKeyAuth]
        [Route("AddOneCharging")]
        public async Task<IActionResult> AddDataOneCharging([FromBody] List<OneChargingDto> data)
        {


            var addFuel = await _unitofWork.One.AddDataOneCharging(data);

            if (addFuel == false)
            {
                return BadRequest("error");
            }

            await _unitofWork.CompleteAsync();

            return Ok("Successfully created");
        }

        [HttpGet]
        [Route("GetOneCharging")]
        public async Task<ActionResult<IEnumerable<OneChargingDto>>> OneChargingPagination([FromQuery] UserParams userParams, bool? status, string search)
        {
            var oneChargingList = await _unitofWork.One.GetOneChargingPagination(userParams, status, search);
            Response.AddPaginationHeader(oneChargingList.CurrentPage, oneChargingList.PageSize, oneChargingList.TotalCount, oneChargingList.TotalPages, oneChargingList.HasNextPage, oneChargingList.HasPreviousPage);

            var oneChargingResult = new
            {
                oneChargingList,
                oneChargingList.CurrentPage,
                oneChargingList.PageSize,
                oneChargingList.TotalCount,
                oneChargingList.TotalPages,
                oneChargingList.HasNextPage,
                oneChargingList.HasPreviousPage

            };

            return Ok(oneChargingResult);
        }
    }
}
