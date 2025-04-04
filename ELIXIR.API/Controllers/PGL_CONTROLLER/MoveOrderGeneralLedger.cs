﻿using ELIXIR.API.Authentication;
using ELIXIR.API.Controllers;
using ELIXIR.DATA.DATA_ACCESS_LAYER.STORE_CONTEXT;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ELIXIRETD.API.Controllers.ETDGL_CONTROLLER
{
    [Route("api/pharma-gl"), ApiController]
    [AllowAnonymous]
    public class ETDGL : ControllerBase
    {
        private readonly IMediator _mediator;
        public ETDGL(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ApiKeyAuth]
        public async Task<IActionResult> Get([FromQuery] ETDGLQuery query)
        {
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result);
            }

        }

        public class ETDGLQuery : IRequest<Result<List<ETDGLResult>>>
        {
            public string adjustment_month { get; set; }
        }

        public class ETDGLResult
        {
            public string SyncId { get; set; }
            public string Mark1 { get; set; }
            public string Mark2 { get; set; }
            public string AssetCIP { get; set; }
            public string AccountingTag { get; set; }
            public string TransactionDate { get; set; }
            public string ClientSupplier { get; set; }
            public string AccountTitleCode { get; set; }
            public string AccountTitle { get; set; }
            public string CompanyCode { get; set; }
            public string Company { get; set; }
            public string DivisionCode { get; set; }
            public string Division { get; set; }
            public string DepartmentCode { get; set; }
            public string Department { get; set; }
            public string UnitCode { get; set; }
            public string Unit { get; set; }
            public string SubUnitCode { get; set; }
            public string SubUnit { get; set; }
            public string LocationCode { get; set; }
            public string Location { get; set; }
            public string PONumber { get; set; }
            public string RRNumber { get; set; }
            public string ReferenceNo { get; set; }
            public string ItemCode { get; set; }
            public string ItemDescription { get; set; }
            public decimal? Quantity { get; set; }
            public string UOM { get; set; }
            public decimal? UnitPrice { get; set; }
            public decimal? LineAmount { get; set; }
            public string VoucherJournal { get; set; }
            public string AccountType { get; set; }
            public string DRCR { get; set; }
            public string AssetCode { get; set; }
            public string Asset { get; set; }
            public string ServiceProviderCode { get; set; }
            public string ServiceProvider { get; set; }
            public string BOA { get; set; }
            public string Allocation { get; set; }
            public string AccountGroup { get; set; }
            public string AccountSubGroup { get; set; }
            public string FinancialStatement { get; set; }
            public string UnitResponsible { get; set; }
            public string Batch { get; set; }
            public string LineDescription { get; set; }
            public string PayrollPeriod { get; set; }
            public string Position { get; set; }
            public string PayrollType { get; set; }
            public string PayrollType2 { get; set; }
            public string DepreciationDescription { get; set; }
            public string RemainingDepreciationValue { get; set; }
            public string UsefulLife { get; set; }
            public string Month { get; set; }
            public string Year { get; set; }
            public string Particulars { get; set; }
            public string Month2 { get; set; }
            public string FarmType { get; set; }
            public string JeanRemarks { get; set; }
            public string From { get; set; }
            public string ChangeTo { get; set; }
            public string Reason { get; set; }
            public string CheckingRemarks { get; set; }
            public string BankName { get; set; }
            public string ChequeNumber { get; set; }
            public string ChequeVoucherNumber { get; set; }
            public string ChequeDate { get; set; }
            public string ReleasedDate { get; set; }
            public string BOA2 { get; set; }
            public string System { get; set; }
            public string Books { get; set; }
        }

        public class Handler : IRequestHandler<ETDGLQuery, Result<List<ETDGLResult>>>
        {
            private readonly StoreContext _context;
            public Handler(StoreContext context)
            {
                _context = context;
            }
            // Type desc


            public async Task<Result<List<ETDGLResult>>> Handle(ETDGLQuery request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrWhiteSpace(request.adjustment_month))
                {
                    return Result.Success(new List<ETDGLResult>());
                }

                if (!DateTime.TryParseExact(request.adjustment_month, "yyyy-MM",
                                            CultureInfo.InvariantCulture, DateTimeStyles.None,
                                            out DateTime adjustmentMonth))
                {
                    throw new ArgumentException("Adjustment_month must be in the format yyyy-MM");
                }

                var startDate = new DateTime(adjustmentMonth.Year, adjustmentMonth.Month, 1);
                var endDate = startDate.AddMonths(1);




                var transactions = from m in _context.MoveOrders
                                   join t in _context.TransactMoveOrder
                                   on m.OrderNo equals t.OrderNo
                                   join w in _context.WarehouseReceived
                                   on m.WarehouseId equals w.Id

                                   where t.PreparedDate >= startDate && t.PreparedDate <= endDate && m.IsTransact == true
                                   //where t.IsActive == true && t.IsTransact == true
                                   //where t.PreparedDate >= startDate && t.PreparedDate <= endDate
                                   //where w.IsActive == true
                                   //where m.PreparedDate >= startDate && m.PreparedDate <= endDate
                                   //where m.IsActive == true && m.IsTransact == true



                                   select new
                                   {
                                       SyncId = m.Id,
                                       TransactionDate = t.PreparedDate,
                                       //ClientSupplier = m.CustomerName,
                                       PONumber = w.PO_Number,
                                       //RRNumber = w.RRNo,
                                       ItemCode = w.ItemCode,
                                       Description = w.ItemDescription,
                                       Quantity = m.QuantityOrdered,
                                       //UnitPrice = w.UnitPrice,
                                       UOM = w.Uom,
                                       DivisionCode = m.CompanyCode,
                                       Division = m.CompanyName,
                                       LocationCode = m.LocationCode,
                                       Location = m.LocationName,
                                       AccountTitle = m.AccountTitle,
                                       AccountCode = m.AccountCode,
                                       DepartmentCode = m.DepartmentCode,
                                       Department = m.DepartmentName,
                                   };



                var transactonList = await transactions.ToListAsync();



                var result = transactonList.SelectMany(x => new List<ETDGLResult>
                {

                    //debit
                    new ETDGLResult
                    {
                        SyncId = "PH-" + (x.SyncId.ToString() ?? string.Empty) + "-D",
                        Mark1 = string.Empty,
                        Mark2 = string.Empty,
                        AssetCIP = string.Empty,
                        AccountingTag = string.Empty,
                        TransactionDate = x.TransactionDate.HasValue ? x.TransactionDate.Value.ToString("yyyy-MM-dd") : string.Empty,
                        ClientSupplier = string.Empty,
                        AccountTitleCode = x.AccountCode ?? string.Empty,
                        AccountTitle = x.AccountTitle ?? string.Empty,
                        CompanyCode = "0001",
                        Company = "RDFFLFI",
                        DivisionCode = x.DivisionCode ?? string.Empty,
                        Division = x.Division ?? string.Empty,
                        DepartmentCode = x.DepartmentCode ?? string.Empty,
                        Department = x.Department ?? string.Empty,
                        UnitCode = string.Empty,
                        Unit = string.Empty,
                        SubUnitCode = string.Empty,
                        SubUnit = string.Empty,
                        LocationCode = x.LocationCode ?? string.Empty,
                        Location = x.Location ?? string.Empty,
                        PONumber = x.PONumber.ToString(),
                        RRNumber = string.Empty,
                        ReferenceNo = string.Empty,
                        ItemCode = x.ItemCode ?? string.Empty,
                        ItemDescription = x.Description ?? string.Empty,
                        Quantity = x.Quantity,
                        UOM = x.UOM ?? string.Empty,
                        UnitPrice = 0,
                        LineAmount =  0,
                        VoucherJournal = string.Empty,
                        AccountType = string.Empty,
                        DRCR = "Debit",
                        AssetCode = string.Empty,
                        Asset= string.Empty,
                        ServiceProviderCode = string.Empty,
                        ServiceProvider = string.Empty,
                        BOA = "Inventoriables",
                        Allocation = string.Empty,
                        AccountGroup = string.Empty,
                        AccountSubGroup = string.Empty,
                        FinancialStatement = string.Empty,
                        UnitResponsible = string.Empty,
                        Batch = string.Empty,
                        LineDescription = string.Empty,
                        PayrollPeriod = string.Empty,
                        Position = string.Empty,
                        PayrollType = string.Empty,
                        PayrollType2 = string.Empty,
                        DepreciationDescription = string.Empty,
                        RemainingDepreciationValue = string.Empty,
                        UsefulLife = string.Empty,
                        Month = x?.TransactionDate?.ToString("MMM") ?? string.Empty,
                        Year = x?.TransactionDate?.ToString("yyyy") ?? string.Empty,
                        Particulars = string.Empty,
                        Month2 = string.Empty,
                        FarmType = string.Empty,
                        JeanRemarks = string.Empty,
                        From = string.Empty,
                        ChangeTo = string.Empty,
                        Reason = string.Empty,
                        CheckingRemarks = string.Empty,
                        BankName = string.Empty,
                        ChequeNumber = string.Empty,
                        ChequeVoucherNumber = string.Empty,
                        ReleasedDate = string.Empty,
                        ChequeDate = string.Empty,
                        BOA2 = "Inventoriables",
                        System = "Elixir - Pharmacy",
                        Books = "Journal Book",
                    },


                    //credit
                    new ETDGLResult
                    {
                        SyncId = "PH-" + (x.SyncId.ToString() ?? string.Empty) + "-C",
                        Mark1 = string.Empty,
                        Mark2 = string.Empty,
                        AssetCIP = string.Empty,
                        AccountingTag = string.Empty,
                        TransactionDate = x.TransactionDate.HasValue ? x.TransactionDate.Value.ToString("yyyy-MM-dd") : string.Empty,
                        ClientSupplier  = string.Empty,
                        AccountTitleCode = "115998",
                        AccountTitle = "Materials & Supplies Inventory",
                        CompanyCode = "0001",
                        Company = "RDFFLFI",
                        DivisionCode = x.DivisionCode ?? string.Empty,
                        Division = x.Division ?? string.Empty,
                        DepartmentCode = x.DepartmentCode ?? string.Empty,
                        Department = x.Department ?? string.Empty,
                        UnitCode = string.Empty,
                        Unit = string.Empty,
                        SubUnitCode = string.Empty,
                        SubUnit = string.Empty,
                        LocationCode = x.LocationCode ?? string.Empty,
                        Location = x.Location ?? string.Empty,
                        PONumber = x.PONumber.ToString(),
                        RRNumber = string.Empty,
                        ReferenceNo = string.Empty,
                        ItemCode = x.ItemCode ?? string.Empty,
                        ItemDescription = x.Description ?? string.Empty,
                        Quantity = -(x?.Quantity) ?? 0,
                        UOM = x.UOM ?? string.Empty,
                        UnitPrice =  0,
                        LineAmount =  0,
                        VoucherJournal = string.Empty,
                        AccountType = string.Empty,
                        DRCR = "Credit",
                        AssetCode = string.Empty,
                        Asset= string.Empty,
                        ServiceProviderCode = string.Empty,
                        ServiceProvider = string.Empty,
                        BOA = "Inventoriables",
                        Allocation = string.Empty,
                        AccountGroup = string.Empty,
                        AccountSubGroup = string.Empty,
                        FinancialStatement = string.Empty,
                        UnitResponsible = string.Empty,
                        Batch = string.Empty,
                        LineDescription = string.Empty,
                        PayrollPeriod = string.Empty,
                        Position = string.Empty,
                        PayrollType = string.Empty,
                        PayrollType2 = string.Empty,
                        DepreciationDescription = string.Empty,
                        RemainingDepreciationValue = string.Empty,
                        UsefulLife = string.Empty,
                        Month = x?.TransactionDate?.ToString("MMM") ?? string.Empty,
                        Year = x?.TransactionDate?.ToString("yyyy") ?? string.Empty,
                        Particulars = string.Empty,
                        Month2 = string.Empty,
                        FarmType = string.Empty,
                        JeanRemarks = string.Empty,
                        From = string.Empty,
                        ChangeTo = string.Empty,
                        Reason = string.Empty,
                        CheckingRemarks = string.Empty,
                        BankName = string.Empty,
                        ChequeNumber = string.Empty,
                        ChequeVoucherNumber = string.Empty,
                        ReleasedDate = string.Empty,
                        ChequeDate = string.Empty,
                        BOA2 = "Inventoriables",
                        System = "Elixir - Pharmacy",
                        Books = "Journal Book",
                    }
                }).ToList();

                return Result.Success(result);
            }
        }
    }
}
