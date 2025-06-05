using ELIXIR.API.Authentication;
using ELIXIR.API.Controllers;
using ELIXIR.DATA.DATA_ACCESS_LAYER.STORE_CONTEXT;
using ELIXIR.DATA.DTOs.REPORT_DTOs;
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


namespace ELIXIR.API.Controllers.PGL_CONTROLLER
{
    [Route("api/pharma-gl"), ApiController]
    [AllowAnonymous]
    public class PHARMAGL : ControllerBase
    {
        private readonly IMediator _mediator;
        public PHARMAGL(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ApiKeyAuth]
        public async Task<IActionResult> Get([FromQuery] PGLQuery query)
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

        public class PGLQuery : IRequest<Result<List<PGLResult>>>
        {
            public string adjustment_month { get; set; }
        }

        public class PGLResult
        {
            public string SyncId { get; set; }
            public string Mark1 { get; set; }
            public string Mark2 { get; set; }
            public string AssetCIP { get; set; }
            public string AccountingTag { get; set; }
            public DateTime? TransactionDate { get; set; }
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
            public string Remarks { get; set; }
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
            public string Adjustment { get; set; }
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

        public class Handler : IRequestHandler<PGLQuery, Result<List<PGLResult>>>
        {
            private readonly StoreContext _context;
            public Handler(StoreContext context)
            {
                _context = context;
            }
            // Type desc


            public async Task<Result<List<PGLResult>>> Handle(PGLQuery request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrWhiteSpace(request.adjustment_month))
                {
                    return Result.Success(new List<PGLResult>());
                }

                if (!DateTime.TryParseExact(request.adjustment_month, "yyyy-MM",
                                            CultureInfo.InvariantCulture, DateTimeStyles.None,
                                            out DateTime adjustmentMonth))
                {
                    throw new ArgumentException("Adjustment_month must be in the format yyyy-MM");
                }

                var startDate = new DateTime(adjustmentMonth.Year, adjustmentMonth.Month, 1);
                var endDate = startDate.AddMonths(1);

                var moveOrderTask = await MoveOrderTransactions(startDate, endDate);
                var receiptTask = await ReceiptTransactions(startDate, endDate);
                var issueTask = await IssueTransactions(startDate, endDate);
                var transformTask = await TransformTransactions(startDate, endDate);
                var tranformFormula = await TransformationFormula(startDate, endDate);
                var transformRecipe = await TransformationRecipe(startDate, endDate);


                var consolidateList = moveOrderTask.Concat(receiptTask).Concat(issueTask)
                    .Concat(transformTask);

                var formula = tranformFormula.SelectMany(x => new List<PGLResult>
                {

                    //debit
                     new PGLResult
                    {
                        SyncId = "PH-" + (x.SyncId.ToString() ?? string.Empty) + "-D",
                        Mark1 = x.Mark1 ?? string.Empty,
                        Mark2 = x.Mark2 ?? string.Empty,
                        AssetCIP = x.AssetCIP ?? string.Empty,
                        AccountingTag = x.AccountingTag ?? string.Empty,
                        TransactionDate = x.TransactionDate,
                        ClientSupplier = x.ClientSupplier ?? string.Empty,
                        AccountTitleCode = x.AccountTitleCode ?? string.Empty,
                        AccountTitle = x.AccountTitle,
                        CompanyCode = "0001",
                        Company = "RDFFLFI",
                        DivisionCode = x.DivisionCode ?? string.Empty,
                        Division = x.Division ?? string.Empty,
                        DepartmentCode = x.DepartmentCode ?? string.Empty,
                        Department = x.Department ?? string.Empty,
                        UnitCode = x.UnitCode ?? string.Empty,
                        Unit = x.Unit ?? string.Empty,
                        SubUnitCode = x.SubUnitCode ?? string.Empty,
                        SubUnit = x.SubUnit ?? string.Empty,
                        LocationCode = x.LocationCode ?? string.Empty,
                        Location = x.Location ?? string.Empty,
                        PONumber = x.PONumber ?? string.Empty,
                        RRNumber = x.RRNumber ?? string.Empty,
                        ReferenceNo = x.ReferenceNo ?? string.Empty,
                        ItemCode = x.ItemCode ?? string.Empty,
                        ItemDescription = x.ItemDescription ?? string.Empty,
                        Quantity = x?.Quantity ?? 0,
                        UOM = x.UOM ?? string.Empty,
                        UnitPrice = x?.UnitPrice ?? 0,
                        LineAmount = x?.LineAmount ?? 0,
                        VoucherJournal = string.Empty,
                        AccountType = "Inventoriables",
                        DRCR = "Debit",
                        AssetCode = x.AssetCode ?? string.Empty,
                        Asset= x.Asset ?? string.Empty,
                        ServiceProviderCode = x.ServiceProviderCode ?? string.Empty,
                        ServiceProvider = x.ServiceProvider ?? string.Empty,
                        BOA = "Inventoriables",
                        Allocation = x.Allocation ?? string.Empty,
                        AccountGroup = "Current Assets",
                        AccountSubGroup = "Inventories",
                        FinancialStatement = "Balance Sheet",
                        UnitResponsible = "MAU",
                        Batch = x.Batch ?? string.Empty,
                        Remarks = x.Remarks ?? string.Empty,
                        PayrollPeriod = x.PayrollPeriod ?? string.Empty,
                        Position = x.Position ?? string.Empty,
                        PayrollType = x.PayrollType ?? string.Empty,
                        PayrollType2 = x.PayrollType2 ?? string.Empty,
                        DepreciationDescription = x.DepreciationDescription ?? string.Empty,
                        RemainingDepreciationValue = x.RemainingDepreciationValue ?? string.Empty,
                        UsefulLife = x.UsefulLife ?? string.Empty,
                        Month = x.TransactionDate.Value.ToString("MMM") ?? string.Empty,
                        Year = x.TransactionDate.Value.ToString("yyyy") ?? string.Empty,
                        Particulars = x.Particulars ?? string.Empty,
                        Month2 = x.TransactionDate.Value.ToString("yyyyMM") ?? string.Empty,
                        FarmType = x.FarmType ?? string.Empty,
                        Adjustment = x.Adjustment ?? string.Empty,
                        From = x.From ?? string.Empty,
                        ChangeTo = x.ChangeTo ?? string.Empty,
                        Reason = x.Reason ?? string.Empty,
                        CheckingRemarks = x.CheckingRemarks ?? string.Empty,
                        BankName = x.BankName ?? string.Empty,
                        ChequeNumber = x.ChequeNumber ?? string.Empty,
                        ChequeVoucherNumber = x.ChequeVoucherNumber ?? string.Empty,
                        ReleasedDate = x.ReleasedDate ?? string.Empty,
                        ChequeDate = x.ChequeDate ?? string.Empty,
                        BOA2 = "Inventoriables",
                        System = "Elixir - Pharmacy",
                        Books = "Journal Book",
                    } });

                var recipe = transformRecipe.SelectMany(x => new List<PGLResult>
                {

                    //debit
                     new PGLResult
                    {
                        SyncId = "PH-" + (x.SyncId.ToString() ?? string.Empty) + "-D",
                        Mark1 = x.Mark1 ?? string.Empty,
                        Mark2 = x.Mark2 ?? string.Empty,
                        AssetCIP = x.AssetCIP ?? string.Empty,
                        AccountingTag = x.AccountingTag ?? string.Empty,
                        TransactionDate = x.TransactionDate,
                        ClientSupplier = x.ClientSupplier ?? string.Empty,
                        AccountTitleCode = x.AccountTitleCode ?? string.Empty,
                        AccountTitle = x.AccountTitle,
                        CompanyCode = "0001",
                        Company = "RDFFLFI",
                        DivisionCode = x.DivisionCode ?? string.Empty,
                        Division = x.Division ?? string.Empty,
                        DepartmentCode = x.DepartmentCode ?? string.Empty,
                        Department = x.Department ?? string.Empty,
                        UnitCode = x.UnitCode ?? string.Empty,
                        Unit = x.Unit ?? string.Empty,
                        SubUnitCode = x.SubUnitCode ?? string.Empty,
                        SubUnit = x.SubUnit ?? string.Empty,
                        LocationCode = x.LocationCode ?? string.Empty,
                        Location = x.Location ?? string.Empty,
                        PONumber = x.PONumber ?? string.Empty,
                        RRNumber = x.RRNumber ?? string.Empty,
                        ReferenceNo = x.ReferenceNo ?? string.Empty,
                        ItemCode = x.ItemCode ?? string.Empty,
                        ItemDescription = x.ItemDescription ?? string.Empty,
                        Quantity = x?.Quantity ?? 0,
                        UOM = x.UOM ?? string.Empty,
                        UnitPrice = x?.UnitPrice ?? 0,
                        LineAmount = x?.LineAmount ?? 0,
                        VoucherJournal = string.Empty,
                        AccountType = "Inventoriables",
                        DRCR = "Debit",
                        AssetCode = x.AssetCode ?? string.Empty,
                        Asset= x.Asset ?? string.Empty,
                        ServiceProviderCode = x.ServiceProviderCode ?? string.Empty,
                        ServiceProvider = x.ServiceProvider ?? string.Empty,
                        BOA = "Inventoriables",
                        Allocation = x.Allocation ?? string.Empty,
                        AccountGroup = "Current Assets",
                        AccountSubGroup = "Inventories",
                        FinancialStatement = "Balance Sheet",
                        UnitResponsible = "MAU",
                        Batch = x.Batch ?? string.Empty,
                        Remarks = x.Remarks ?? string.Empty,
                        PayrollPeriod = x.PayrollPeriod ?? string.Empty,
                        Position = x.Position ?? string.Empty,
                        PayrollType = x.PayrollType ?? string.Empty,
                        PayrollType2 = x.PayrollType2 ?? string.Empty,
                        DepreciationDescription = x.DepreciationDescription ?? string.Empty,
                        RemainingDepreciationValue = x.RemainingDepreciationValue ?? string.Empty,
                        UsefulLife = x.UsefulLife ?? string.Empty,
                        Month = x.TransactionDate.Value.ToString("MMM") ?? string.Empty,
                        Year = x.TransactionDate.Value.ToString("yyyy") ?? string.Empty,
                        Particulars = x.Particulars ?? string.Empty,
                        Month2 = x.TransactionDate.Value.ToString("yyyyMM") ?? string.Empty,
                        FarmType = x.FarmType ?? string.Empty,
                        Adjustment = x.Adjustment ?? string.Empty,
                        From = x.From ?? string.Empty,
                        ChangeTo = x.ChangeTo ?? string.Empty,
                        Reason = x.Reason ?? string.Empty,
                        CheckingRemarks = x.CheckingRemarks ?? string.Empty,
                        BankName = x.BankName ?? string.Empty,
                        ChequeNumber = x.ChequeNumber ?? string.Empty,
                        ChequeVoucherNumber = x.ChequeVoucherNumber ?? string.Empty,
                        ReleasedDate = x.ReleasedDate ?? string.Empty,
                        ChequeDate = x.ChequeDate ?? string.Empty,
                        BOA2 = "Inventoriables",
                        System = "Elixir - Pharmacy",
                        Books = "Journal Book",
                    } }).Union(formula);

                var result = consolidateList.SelectMany(x => new List<PGLResult>
                {

                    //debit
                     new PGLResult
                    {
                        SyncId = "PH-" + (x.SyncId.ToString() ?? string.Empty) + "-D",
                        Mark1 = x.Mark1 ?? string.Empty,
                        Mark2 = x.Mark2 ?? string.Empty,
                        AssetCIP = x.AssetCIP ?? string.Empty,
                        AccountingTag = x.AccountingTag ?? string.Empty,
                        TransactionDate = x.TransactionDate,
                        ClientSupplier = x.ClientSupplier ?? string.Empty,
                        AccountTitleCode = x.AccountTitleCode ?? string.Empty,
                        AccountTitle = x.AccountTitle,
                        CompanyCode = "0001",
                        Company = "RDFFLFI",
                        DivisionCode = x.DivisionCode ?? string.Empty,
                        Division = x.Division ?? string.Empty,
                        DepartmentCode = x.DepartmentCode ?? string.Empty,
                        Department = x.Department ?? string.Empty,
                        UnitCode = x.UnitCode ?? string.Empty,
                        Unit = x.Unit ?? string.Empty,
                        SubUnitCode = x.SubUnitCode ?? string.Empty,
                        SubUnit = x.SubUnit ?? string.Empty,
                        LocationCode = x.LocationCode ?? string.Empty,
                        Location = x.Location ?? string.Empty,
                        PONumber = x.PONumber ?? string.Empty,
                        RRNumber = x.RRNumber ?? string.Empty,
                        ReferenceNo = x.ReferenceNo ?? string.Empty,
                        ItemCode = x.ItemCode ?? string.Empty,
                        ItemDescription = x.ItemDescription ?? string.Empty,
                        Quantity = x?.Quantity ?? 0,
                        UOM = x.UOM ?? string.Empty,
                        UnitPrice = x?.UnitPrice ?? 0,
                        LineAmount = x?.LineAmount ?? 0,
                        VoucherJournal = string.Empty,
                        AccountType = "Inventoriables",
                        DRCR = "Debit",
                        AssetCode = x.AssetCode ?? string.Empty,
                        Asset= x.Asset ?? string.Empty,
                        ServiceProviderCode = x.ServiceProviderCode ?? string.Empty,
                        ServiceProvider = x.ServiceProvider ?? string.Empty,
                        BOA = "Inventoriables",
                        Allocation = x.Allocation ?? string.Empty,
                        AccountGroup = "Current Assets",
                        AccountSubGroup = "Inventories",
                        FinancialStatement = "Balance Sheet",
                        UnitResponsible = "MAU",
                        Batch = x.Batch ?? string.Empty,
                        Remarks = x.Remarks ?? string.Empty,
                        PayrollPeriod = x.PayrollPeriod ?? string.Empty,
                        Position = x.Position ?? string.Empty,
                        PayrollType = x.PayrollType ?? string.Empty,
                        PayrollType2 = x.PayrollType2 ?? string.Empty,
                        DepreciationDescription = x.DepreciationDescription ?? string.Empty,
                        RemainingDepreciationValue = x.RemainingDepreciationValue ?? string.Empty,
                        UsefulLife = x.UsefulLife ?? string.Empty,
                        Month = x.TransactionDate.Value.ToString("MMM") ?? string.Empty,
                        Year = x.TransactionDate.Value.ToString("yyyy") ?? string.Empty,
                        Particulars = x.Particulars ?? string.Empty,
                        Month2 = x.TransactionDate.Value.ToString("yyyyMM") ?? string.Empty,
                        FarmType = x.FarmType ?? string.Empty,
                        Adjustment = x.Adjustment ?? string.Empty,
                        From = x.From ?? string.Empty,
                        ChangeTo = x.ChangeTo ?? string.Empty,
                        Reason = x.Reason ?? string.Empty,
                        CheckingRemarks = x.CheckingRemarks ?? string.Empty,
                        BankName = x.BankName ?? string.Empty,
                        ChequeNumber = x.ChequeNumber ?? string.Empty,
                        ChequeVoucherNumber = x.ChequeVoucherNumber ?? string.Empty,
                        ReleasedDate = x.ReleasedDate ?? string.Empty,
                        ChequeDate = x.ChequeDate ?? string.Empty,
                        BOA2 = "Inventoriables",
                        System = "Elixir - Pharmacy",
                        Books = "Journal Book",
                    },
                    //credit
                    new PGLResult
                    {
                        SyncId = "PH-" + (x.SyncId.ToString() ?? string.Empty) + "-C",
                        Mark1 = string.Empty,
                        Mark2 = string.Empty,
                        AssetCIP = x.AssetCIP ?? string.Empty,
                        AccountingTag = string.Empty,
                        TransactionDate = x.TransactionDate,
                        ClientSupplier = x.ClientSupplier ?? string.Empty,
                        AccountTitleCode = "115998",
                        AccountTitle = "Materials & Supplies Inventory",
                        CompanyCode = "0001",
                        Company = "RDFFLFI",
                        DivisionCode = x.DivisionCode ?? string.Empty,
                        Division = x.Division ?? string.Empty,
                        DepartmentCode = x.DepartmentCode ?? string.Empty,
                        Department = x.Department ?? string.Empty,
                        UnitCode = x.UnitCode ?? string.Empty,
                        Unit = x.Unit ?? string.Empty,
                        SubUnitCode = x.SubUnitCode ?? string.Empty,
                        SubUnit = x.SubUnit ?? string.Empty,
                        LocationCode = x.LocationCode ?? string.Empty,
                        Location = x.Location ?? string.Empty,
                        PONumber = x.PONumber ?? string.Empty,
                        RRNumber = x.RRNumber ?? string.Empty,
                        ReferenceNo = x.ReferenceNo ?? string.Empty,
                        ItemCode = x.ItemCode ?? string.Empty,
                        ItemDescription = x.ItemDescription ?? string.Empty,
                        Quantity = x?.Quantity ?? 0,
                        UOM = x.UOM ?? string.Empty,
                        UnitPrice = x?.UnitPrice ?? 0,
                        LineAmount = -(x?.LineAmount) ?? 0,
                        VoucherJournal = string.Empty,
                        AccountType = "Inventoriables",
                        DRCR = "Credit",
                        AssetCode = x.AssetCode ?? string.Empty,
                        Asset= x.Asset ?? string.Empty,
                        ServiceProviderCode = x.ServiceProviderCode ?? string.Empty,
                        ServiceProvider = x.ServiceProvider ?? string.Empty,
                        BOA = "Inventoriables",
                        Allocation = x.Allocation ?? string.Empty,
                        AccountGroup = "Current Assets",
                        AccountSubGroup = "Inventories",
                        FinancialStatement = "Balance Sheet",
                        UnitResponsible = "MAU",
                        Batch = x.Batch ?? string.Empty,
                        Remarks = x.Remarks ?? string.Empty,
                        PayrollPeriod = x.PayrollPeriod ?? string.Empty,
                        Position = x.Position ?? string.Empty,
                        PayrollType = x.PayrollType ?? string.Empty,
                        PayrollType2 = x.PayrollType2 ?? string.Empty,
                        DepreciationDescription = x.DepreciationDescription ?? string.Empty,
                        RemainingDepreciationValue = x.RemainingDepreciationValue ?? string.Empty,
                        UsefulLife = x.UsefulLife ?? string.Empty,
                        Month = x.TransactionDate.Value.ToString("MMM") ?? string.Empty,
                        Year = x.TransactionDate.Value.ToString("yyyy") ?? string.Empty,
                        Particulars = x.Particulars ?? string.Empty,
                        Month2 = x.TransactionDate.Value.ToString("yyyyMM") ?? string.Empty,
                        FarmType = x.FarmType ?? string.Empty,
                        Adjustment = x.Adjustment ?? string.Empty,
                        From = x.From ?? string.Empty,
                        ChangeTo = x.ChangeTo ?? string.Empty,
                        Reason = x.Reason ?? string.Empty,
                        CheckingRemarks = x.CheckingRemarks ?? string.Empty,
                        BankName = x.BankName ?? string.Empty,
                        ChequeNumber = x.ChequeNumber ?? string.Empty,
                        ChequeVoucherNumber = x.ChequeVoucherNumber ?? string.Empty,
                        ReleasedDate = x.ReleasedDate ?? string.Empty,
                        ChequeDate = x.ChequeDate ?? string.Empty,
                        BOA2 = "Inventoriables",
                        System = "Elixir - Pharmacy",
                        Books = "Journal Book",
                    }
                }).ToList();

                return Result.Success(result);
            }

            private async Task<List<PGLResult>> MoveOrderTransactions(DateTime startDate, DateTime endDate)
            {
                var result = (from m in _context.MoveOrders.AsNoTracking()
                                   join t in _context.TransactMoveOrder
                                   on m.OrderNo equals t.OrderNo
                                   join w in _context.WarehouseReceived
                                   on m.WarehouseId equals w.Id
                                   join u in _context.Users on t.PreparedBy equals u.FullName
                                   join p in _context.POSummary on w.PO_Number equals p.PO_Number
                                   


                              where t.PreparedDate >= startDate.Date && t.PreparedDate < endDate && m.IsTransact == true



                                   select new PGLResult
                                   {
                                       SyncId = "MO-" + m.Id.ToString(),
                                       TransactionDate = t.PreparedDate,
                                       PONumber = w.PO_Number.ToString(),
                                       RRNumber = "0",
                                       ItemCode = w.ItemCode,
                                       ItemDescription = w.ItemDescription,
                                       Quantity = m.QuantityOrdered,
                                       UOM = w.Uom,
                                       UnitPrice = p.UnitPrice,
                                       LineAmount = m.QuantityOrdered * p.UnitPrice,
                                       DivisionCode = "21",
                                       Division = m.CompanyName,
                                       CheckingRemarks = "Move Order",
                                       CompanyCode = "10",
                                       Company = "RDF Corporate Services",
                                       DepartmentCode = "0010",
                                       Department = "Corporate Common",
                                       LocationCode = "0001",
                                       Location = "Head Office",
                                       AccountTitle = m.Category == "DISINFECTANT" 
                                       || m.Category == "SOLUBLE ANTIBIOTICS" 
                                       || m.Category == "SUPPLIMENTS" 
                                       || m.Category == "ORAL PREPARATION" 
                                       || m.Category == "VACCINE POULTRY" 
                                       || m.Category == "VACCINE SWINE" 
                                       || m.Category == "INJECTABLES" ? "Inventory Transfer" : "COS - Farm Supplies",
                                       AccountTitleCode = m.Category == "DISINFECTANT"
                                       || m.Category == "SOLUBLE ANTIBIOTICS"
                                       || m.Category == "SUPPLIMENTS"
                                       || m.Category == "ORAL PREPARATION"
                                       || m.Category == "VACCINE POULTRY"
                                       || m.Category == "VACCINE SWINE"
                                       || m.Category == "INJECTABLES" ? "115999" : "510007",
                                       Remarks = m.Remarks,
                                       ServiceProvider = t.PreparedBy,
                                       Batch = m.BatchNo,
                                       ServiceProviderCode = u.UserName,
                                       ReferenceNo = t.Id.ToString() ?? "",
                                       FarmType = m.FarmType,
                                       
                                   });

                return await result.ToListAsync();
            }
            private async Task<List<PGLResult>> ReceiptTransactions(DateTime startDate, DateTime endDate)
            {

                var result = _context.MiscellaneousReceipts
                .GroupJoin(_context.WarehouseReceived, receipt => receipt.Id, warehouse => warehouse.MiscellaneousReceiptId, (receipt, warehouse) => new { receipt, warehouse })
                .SelectMany(x => x.warehouse.DefaultIfEmpty(), (x, warehouse) => new { x.receipt, warehouse })
                .Where(x => x.receipt.TransactionDate >= startDate.AddDays(1) && x.receipt.TransactionDate < endDate)
                .Where(x => x.warehouse.IsActive == true && x.warehouse.TransactionType == "MiscellaneousReceipt")
                .Select(x => new PGLResult
                {
                    SyncId = "MR-" + x.warehouse.Id.ToString(),
                    TransactionDate = x.receipt.TransactionDate,
                    ItemCode = x.warehouse.ItemCode,
                    ItemDescription = x.warehouse.ItemDescription,
                    UOM = x.warehouse.Uom,
                    //Category = "",
                    Quantity = x.warehouse.ActualGood,
                    UnitPrice = 0,
                    LineAmount = 0,
                    
                    CheckingRemarks = "Miscellaneous Receipt",
                    //Status = "",
                    //Reason = x.receipt.Remarks,
                    Remarks = x.receipt.Details,
                    //SupplierName = x.receipt.Supplier,
                    //EncodedBy = x.receipt.PreparedBy,
                    CompanyCode = "10",
                    Company = "RDF Corporate Services",
                    DepartmentCode = "0010",
                    Department = "Corporate Common",
                    LocationCode = "0001",
                    Location = "Head Office",
                    AccountTitle = /*m.AccountTitle*/"Inventory Transfer",
                    AccountTitleCode = /*m.AccountCode*/"115999",
                    ServiceProvider = x.receipt.PreparedBy,
                    AssetCIP = "",
                    PONumber = "",
                    RRNumber = "0",
                    //Remarks = x.receipt.Remarks,
                    
                });

                return await result.ToListAsync() ;
            }
            private async Task<List<PGLResult>> IssueTransactions(DateTime startDate, DateTime endDate)
            {


                var result = _context.MiscellaneousIssues
                .AsNoTracking()
                   .GroupJoin(
                    _context.MiscellaneousIssueDetails,
                    miscDetail => miscDetail.Id,
                    issue => issue.IssuePKey,
                    (miscDetail, issues) => new { miscDetail, issues })
                .SelectMany(
                    x => x.issues.DefaultIfEmpty(),
                    (x, issue) => new { miscDetail = x.miscDetail, issue }
                )

                .GroupJoin(_context.WarehouseReceived, misc => misc.issue.WarehouseId, ware => ware.Id, (misc, wareh) => new { misc.miscDetail, misc.issue, wareh })
                .SelectMany(x => x.wareh.DefaultIfEmpty(), (x, ware) => new { x.miscDetail, x.issue, ware })

                .GroupJoin(_context.POSummary, warehouse => warehouse.ware.PO_Number, posummary => posummary.PO_Number, (warehouse, posummary) => new { warehouse, posummary })
                .SelectMany(x => x.posummary.DefaultIfEmpty(),
                (x, posummary) => new { x.warehouse.miscDetail, x.warehouse.issue, x.warehouse.ware, posummary })
                .GroupJoin(_context.RawMaterials, warehouse => warehouse.ware.ItemCode, rawmaterials => rawmaterials.ItemCode, (warehouse, rawmaterials) => new { warehouse, rawmaterials })
                .SelectMany(x => x.rawmaterials.DefaultIfEmpty(), (x, rawmaterials) => new { x.warehouse.miscDetail, x.warehouse.issue, x.warehouse.ware, x.warehouse.posummary, rawmaterials })
                .GroupJoin(_context.ItemCategories, rawmaterials => rawmaterials.rawmaterials.ItemCategoryId, itemcategory => itemcategory.Id, (rawmaterials, itemcateogry) => new { rawmaterials, itemcateogry })
                .SelectMany(x => x.itemcateogry.DefaultIfEmpty(), (x, itemcategory) => new { x.rawmaterials.miscDetail, x.rawmaterials.issue, x.rawmaterials.ware, x.rawmaterials.posummary, x.rawmaterials.rawmaterials, itemcategory })
                .Where(x => x.posummary != null)

                .Where(x => x.issue == null || x.issue.IsActive == true)
                .Where(x => x.miscDetail.TransactionDate >= startDate.AddDays(1) && x.miscDetail.TransactionDate < endDate)
                .Select(x => new PGLResult
                {
                    
                    SyncId = "MI-" + x.issue.Id.ToString(),
                    TransactionDate = x.miscDetail.TransactionDate,
                    ItemCode = x.issue.ItemCode,
                    ItemDescription = x.issue.ItemDescription,
                    UOM = x.issue.Uom,
                    PONumber = "",
                    Quantity = Math.Round(x.issue.Quantity, 2),
                    UnitPrice = 1,
                    LineAmount = 1 * (Math.Round(x.issue.Quantity, 2)),
                    //Source = x.miscDetail.Id.ToString(),
                    CheckingRemarks = "Miscellaneous Issue",
                    //Status = "",
                    //Reason = x.issue.Remarks,
                    Remarks = x.miscDetail.Details,
                    //SupplierName = "",
                    ServiceProvider = x.issue.PreparedBy,
                    CompanyCode = "10",
                    Company = "RDF Corporate Services",
                    DepartmentCode = "0010",
                    Department = "Corporate Common",
                    LocationCode = "0001",
                    Location = "Head Office",
                    AccountTitle = "Inventory Transfer",
                    AccountTitleCode = "115999",

                    AssetCIP = "",
                    RRNumber = "0",
                    
                });
                
                return await result.ToListAsync();
            }
            private async Task<List<PGLResult>> TransformTransactions(DateTime startDate, DateTime endDate)
            {



                var result = _context.Transformation_Preparation
                .AsNoTracking()
                .GroupJoin(_context.WarehouseReceived, m => m.WarehouseId, w => w.Id, (m, w) => new { m, w })
                .SelectMany(
                    x => x.w.DefaultIfEmpty(),
                    (x, w) => new { m = x.m, w })
                .GroupJoin(_context.POSummary, x => x.w.PO_Number, po => po.PO_Number, (p, po) => new { p, po })
                .SelectMany(x => x.po.DefaultIfEmpty(), (x, posummary) => new { x.p.m, x.p.w, posummary })
                .GroupJoin(_context.RawMaterials, warehouse => warehouse.w.ItemCode, rawmaterials => rawmaterials.ItemCode, (warehouse, rawmaterials) => new { warehouse, rawmaterials })
                .SelectMany(x => x.rawmaterials.DefaultIfEmpty(), (x, rawmaterials) => new { x.warehouse.m, x.warehouse.w, x.warehouse.posummary, rawmaterials })
                .GroupJoin(_context.ItemCategories, rawmaterials => rawmaterials.rawmaterials.ItemCategoryId, itemcategory => itemcategory.Id, (rawmaterials, itemcateogry) => new { rawmaterials, itemcateogry })
                .SelectMany(x => x.itemcateogry.DefaultIfEmpty(), (x, itemcategory) => new { x.rawmaterials.m, x.rawmaterials.w, x.rawmaterials.posummary, x.rawmaterials.rawmaterials, itemcategory })
                .Where(t => t.m.IsActive && t.m.PreparedDate >= startDate.AddDays(1) && t.m.PreparedDate < endDate)
                .Select(x => new PGLResult
                {
                    SyncId = "T-" + x.m.Id.ToString(),
                    TransactionDate = x.m.PreparedDate,
                    ItemCode = x.m.ItemCode,
                    ItemDescription = x.m.ItemDescription,
                    UOM = "KG",
                    PONumber = "",
                    Quantity = Math.Round(x.m.QuantityNeeded, 2),
                    UnitPrice = x.posummary.UnitPrice,
                    LineAmount = (Math.Round(x.m.QuantityNeeded, 2)) * x.posummary.UnitPrice,
                    //Source = x.Id.ToString(),
                    CheckingRemarks = "Transformation",
                    //Status = "",
                    //Reason = "",
                    Remarks = "",
                    //SupplierName = "",
                    
                    ServiceProvider = x.m.PreparedBy,
                    CompanyCode = "10",
                    Company = "RDF Corporate Services",
                    DepartmentCode = "0010",
                    Department = "Corporate Common",
                    LocationCode = "0001",
                    Location = "Head Office",
                    AccountTitleCode = "115998",
                    AccountTitle = "Materials & Supplies Inventory",


                    AssetCIP = "",
                    RRNumber = "0",

                });
                return await result.ToListAsync();
            }
            

            private async Task<List<PGLResult>> TransformationFormula(DateTime startDate, DateTime endDate)
            {
                var result = from tp in _context.Transformation_Planning
                              where tp.Status == true
                              join warehouse in _context.WarehouseReceived on tp.Id equals warehouse.TransformId
                             join posummary in _context.POSummary on tp.ItemCode equals posummary.ItemCode into posJoin
                             from posummary in posJoin.DefaultIfEmpty()

                             select new PGLResult
                              {
                                  SyncId = "T-" + tp.Id.ToString(),
                                  TransactionDate = warehouse.ReceivingDate,
                                  ItemCode = tp.ItemCode,
                                  ItemDescription = tp.ItemDescription,
                                  UOM = "KG",
                                  PONumber = "",
                                  Quantity = tp.Batch * tp.Quantity,
                                  UnitPrice = posummary.UnitPrice,
                                  LineAmount = (Math.Round(tp.Batch * tp.Quantity, 2)) * posummary.UnitPrice,
                                  
                                  CheckingRemarks = "Transformation (Formula)",
                                  Remarks = "",
                                  
                                  ServiceProvider = tp.AddedBy,
                                  CompanyCode = "10",
                                  Company = "RDF Corporate Services",
                                  DepartmentCode = "0010",
                                  Department = "Corporate Common",
                                  LocationCode = "0001",
                                  Location = "Head Office",
                                  AccountTitleCode = "115998",
                                  AccountTitle = "Materials & Supplies Inventory",


                                  AssetCIP = "",
                                  RRNumber = "0",

                              };

                return await result.ToListAsync();
            }
            private async Task<List<PGLResult>> TransformationRecipe(DateTime startDate, DateTime endDate)
            {
                var result = from tp2 in _context.Transformation_Planning
                             join tp in _context.Transformation_Preparation on tp2.Id equals tp.TransformId
                             join po in _context.POSummary on tp2.ItemCode equals po.ItemCode into posJoin
                             from po in posJoin.DefaultIfEmpty()
                             where tp2.Status == true && tp.IsActive == true && tp.IsMixed == true
                             group tp by new { tp.TransformId, tp.ItemCode, tp.QuantityNeeded, tp.ItemDescription, tp2.Version, tp2.Batch, po.UnitPrice, tp2.AddedBy, tp2.ProdPlan, } into g
                             select new PGLResult
                             {
                                 SyncId = "T-" + g.Key.TransformId.ToString(),
                                 TransactionDate = g.Key.ProdPlan,
                                 ItemCode = g.Key.ItemCode,
                                 ItemDescription = g.Key.ItemDescription,
                                 UOM = "KG",
                                 PONumber = "",
                                 Quantity = g.Key.QuantityNeeded,
                                 UnitPrice = g.Key.UnitPrice,
                                 LineAmount = (Math.Round(g.Key.QuantityNeeded, 2)) * g.Key.UnitPrice,

                                 CheckingRemarks = "Transformation (Formula)",
                                 Remarks = "",

                                 ServiceProvider = g.Key.AddedBy,
                                 CompanyCode = "10",
                                 Company = "RDF Corporate Services",
                                 DepartmentCode = "0010",
                                 Department = "Corporate Common",
                                 LocationCode = "0001",
                                 Location = "Head Office",
                                 AccountTitleCode = "115998",
                                 AccountTitle = "Materials & Supplies Inventory",


                                 AssetCIP = "",
                                 RRNumber = "0",

                             };

                return await result.ToListAsync();
            }

        }
    }
}
