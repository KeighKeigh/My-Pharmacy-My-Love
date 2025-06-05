using ELIXIR.DATA.DATA_ACCESS_LAYER.HELPERS;
using ELIXIR.DATA.DTOs.ONECHARGING_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELIXIR.DATA.CORE.INTERFACES.ONECHARGING_INTERFACE
{
    public interface IOneChargingRepository
    {
        Task<bool> AddDataOneCharging(List<OneChargingDto> data);
        Task<PagedList<OneChargingDto>> GetOneChargingPagination(UserParams userParams, bool? status, string search);

        Task<bool> AddAccountTitle(List<OneAccountTitleDto> data);
        Task<PagedList<OneAccountTitleDto>> GetAccountTitle(UserParams userParams, bool? status, string search);
    }
}
