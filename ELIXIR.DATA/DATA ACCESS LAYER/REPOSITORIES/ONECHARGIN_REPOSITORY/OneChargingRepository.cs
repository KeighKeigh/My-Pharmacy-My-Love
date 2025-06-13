﻿using DocumentFormat.OpenXml.EMMA;
using ELIXIR.DATA.CORE.INTERFACES.ONECHARGING_INTERFACE;
using ELIXIR.DATA.DATA_ACCESS_LAYER.HELPERS;
using ELIXIR.DATA.DATA_ACCESS_LAYER.MODELS.ONECHARGING_MODEL;
using ELIXIR.DATA.DATA_ACCESS_LAYER.STORE_CONTEXT;
using ELIXIR.DATA.DTOs.ONECHARGING_DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELIXIR.DATA.DATA_ACCESS_LAYER.REPOSITORIES.ONECHARGIN_REPOSITORY
{
    public class OneChargingRepository :IOneChargingRepository
    {
        public readonly StoreContext _context;

        public OneChargingRepository(StoreContext context)
        {
            _context = context;
        }


        public async Task<bool> AddDataOneCharging(List<OneChargingDto> data)
        {
            var allCommands = data;

            var incomingSyncIds = allCommands
                .Where(x => x.sync_id != null)
                .Select(x => x.sync_id)
                .ToList();
            var existingSyncIds = await _context.OneChargings
                .Where(x => incomingSyncIds.Contains(x.sync_id))
                .Select(x => x.sync_id).ToListAsync();

            var updateSync = allCommands.Where(x => existingSyncIds.Contains(x.sync_id)).ToList();
            var newSync = allCommands.Where(x => !existingSyncIds.Contains(x.sync_id)).ToList();


            var dataSync = newSync.Select(x => new OneCharging
            {
                code = x.code,
                name = x.name,
                sync_id = x.sync_id,
                company_code = x.company_code,
                company_name = x.company_name,
                company_id = x.company_id,
                business_unit_code = x.business_unit_code,
                business_unit_name = x.business_unit_name,
                business_unit_id = x.business_unit_id,
                department_code = x.department_code,
                department_name = x.department_name,
                department_id = x.department_id,
                department_unit_code = x.department_unit_code,
                department_unit_name = x.department_unit_name,
                department_unit_id = x.department_unit_id,
                sub_unit_code = x.sub_unit_code,
                sub_unit_name = x.sub_unit_name,
                sub_unit_id = x.sub_unit_id,
                location_code = x.location_code,
                location_name = x.location_name,
                location_id = x.location_id,
                deleted_at = x.deleted_at,
                IsActive = x.deleted_at  != null ? false : true,

            }).ToList();


            await _context.OneChargings.AddRangeAsync(dataSync);

            foreach (OneChargingDto datas in updateSync)
            {
                var updatedata = _context.OneChargings.FirstOrDefault(o => o.sync_id == datas.sync_id);
                if (updatedata != null)
                {
                    updatedata.code = datas.code;
                    updatedata.name = datas.name;
                    updatedata.company_code = datas.company_code;
                    updatedata.company_name = datas.company_name;
                    updatedata.company_id = datas.company_id;
                    updatedata.business_unit_code = datas.business_unit_code;
                    updatedata.business_unit_name = datas.business_unit_name;
                    updatedata.business_unit_id = datas.business_unit_id;
                    updatedata.department_code = datas.department_code;
                    updatedata.department_name = datas.department_name;
                    updatedata.department_id = datas.department_id;
                    updatedata.department_unit_code = datas.department_unit_code;
                    updatedata.department_unit_name = datas.department_unit_name;
                    updatedata.department_unit_id = datas.department_unit_id;
                    updatedata.sub_unit_code = datas.sub_unit_code;
                    updatedata.sub_unit_name = datas.sub_unit_name;
                    updatedata.sub_unit_id = datas.sub_unit_id;
                    updatedata.location_code = datas.location_code;
                    updatedata.location_name = datas.location_name;
                    updatedata.location_id = datas.location_id;
                    updatedata.deleted_at = datas.deleted_at;
                    updatedata.IsActive = datas.deleted_at != null ? true : false;
                }

            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PagedList<OneChargingDto>> GetOneChargingPagination(UserParams userParams, bool? status, string search)
        {
            var result = _context.OneChargings.Select(x => new OneChargingDto
            {
                code = x.code,
                name = x.name,
                company_code = x.company_code,
                company_name = x.company_name,
                company_id = x.company_id,
                business_unit_code = x.business_unit_code,
                business_unit_name = x.business_unit_name,
                business_unit_id = x.business_unit_id,
                department_code = x.department_code,
                department_name = x.department_name,
                department_id = x.department_id,
                department_unit_code = x.department_unit_code,
                department_unit_name = x.department_unit_name,
                department_unit_id = x.department_unit_id,
                sub_unit_code = x.sub_unit_code,
                sub_unit_name = x.sub_unit_name,
                sub_unit_id = x.sub_unit_id,
                location_code = x.location_code,
                location_name = x.location_name,
                location_id = x.location_id,
                IsActive = x.IsActive,
                

            });

            if(status != null)
            {
                result = result.Where(x => x.IsActive == status);
            }

            if(!string.IsNullOrEmpty(search))
            {
                result = result.Where(x => Convert.ToString(x.code).ToLower().Contains(search.Trim().ToLower())
                                        || Convert.ToString(x.name).ToLower().Contains(search.Trim().ToLower()));
            }
            return await PagedList<OneChargingDto>.CreateAsync(result, userParams.PageNumber, userParams.PageSize);
        }


        public async Task<bool> AddAccountTitle(List<OneAccountTitleDto> data)
        {
            var allCommands = data;

            var incomingSyncIds = allCommands
                .Where(x => x.syncId != null)
                .Select(x => x.syncId)
                .ToList();
            var existingSyncIds = await _context.OneAccountTitles
                .Where(x => incomingSyncIds.Contains(x.SyncId))
                .Select(x => x.SyncId).ToListAsync();

            var updateSync = allCommands.Where(x => existingSyncIds.Contains(x.syncId)).ToList();
            var newSync = allCommands.Where(x => !existingSyncIds.Contains(x.syncId)).ToList();


            var dataSync = newSync.Select(x => new OneAccountTitle
            {
                code = x.code,
                AccountTitleName = x.accountTitleName,
                AccountTitleCode = x.accountTitleCode,
                SyncId = x.syncId,
                Delete = x.delete,
                IsActive = string.IsNullOrEmpty(x.delete) ? true : false,

            }).ToList();


            await _context.OneAccountTitles.AddRangeAsync(dataSync);

            foreach (OneAccountTitleDto datas in updateSync)
            {
                var updatedata = _context.OneAccountTitles.FirstOrDefault(o => o.SyncId == datas.syncId);
                if (updatedata != null)
                {
                    updatedata.code = datas.code;
                    updatedata.AccountTitleName = datas.accountTitleName;
                    updatedata.AccountTitleCode = datas.accountTitleCode;
                    updatedata.Delete = datas.delete;
                    updatedata.IsActive = datas.delete != null ? false : true;
                }

            }

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<PagedList<OneAccountTitleDto>> GetAccountTitle(UserParams userParams, bool? status, string search)
        {
            var result = _context.OneAccountTitles.Select(x => new OneAccountTitleDto
            {
                code = x.code,
                accountTitleCode = x.AccountTitleCode,
                accountTitleName = x.AccountTitleName,
                isActive = x.IsActive,

            });

            if (status != null)
            {
                result = result.Where(x => x.isActive == status);
            }

            if (!string.IsNullOrEmpty(search))
            {
                result = result.Where(x => Convert.ToString(x.code).ToLower().Contains(search.Trim().ToLower())
                                        || Convert.ToString(x.accountTitleName).ToLower().Contains(search.Trim().ToLower())
                                        || Convert.ToString(x.accountTitleCode).ToLower().Contains(search.Trim().ToLower()));
            }
            return await PagedList<OneAccountTitleDto>.CreateAsync(result, userParams.PageNumber, userParams.PageSize);
        }
    }
}
