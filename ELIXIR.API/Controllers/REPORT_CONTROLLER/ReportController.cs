using ELIXIR.DATA.CORE.ICONFIGURATION;
using ELIXIR.DATA.DATA_ACCESS_LAYER.MODELS;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Threading.Tasks;
using static ELIXIR.DATA.DATA_ACCESS_LAYER.REPOSITORIES.REPORT_REPOSITORY.GeneralLedgerExport;
using MediatR;
using static ELIXIR.DATA.DATA_ACCESS_LAYER.REPOSITORIES.REPORT_REPOSITORY.ConsolidateFinanceExport;
using static ELIXIR.DATA.DATA_ACCESS_LAYER.REPOSITORIES.REPORT_REPOSITORY.ConsolidateAuditExport;
using static ELIXIR.DATA.DATA_ACCESS_LAYER.REPOSITORIES.REPORT_REPOSITORY.ConsolidateInventoryMovementExport;
using DocumentFormat.OpenXml.Office2016.Excel;

namespace ELIXIR.API.Controllers.REPORT_CONTROLLER
{

    public class ReportController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public ReportController(IUnitOfWork unitofwork, IMediator mediator)
        {
            _unitOfWork = unitofwork;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("QcReceivingReport")]
        public async Task<IActionResult> QcReceivingReport([FromQuery] string DateFrom, [FromQuery] string DateTo)
        {

            var orders = await _unitOfWork.Report.QcRecevingReport(DateFrom, DateTo);

            return Ok(orders);

        }

        [HttpGet]
        [Route("WarehouseReceivingReport")]
        public async Task<IActionResult> WarehouseReceivingReport([FromQuery] string DateFrom, [FromQuery] string DateTo)
        {

            var orders = await _unitOfWork.Report.WarehouseRecivingReport(DateFrom, DateTo);

            return Ok(orders);

        }

        [HttpGet]
        [Route("TransformationHistoryReport")]
        public async Task<IActionResult> TransformationHistoryReport([FromQuery] string DateFrom, [FromQuery] string DateTo)
        {

            var orders = await _unitOfWork.Report.TransformationReport(DateFrom, DateTo);

            return Ok(orders);

        }

        [HttpGet]
        [Route("MoveOrderHistory")]
        public async Task<IActionResult> MoveOrderHistory([FromQuery] string DateFrom, [FromQuery] string DateTo)
        {

            var orders = await _unitOfWork.Report.MoveOrderReport(DateFrom, DateTo);

            return Ok(orders);

        }

        [HttpGet]
        [Route("MiscellaneousReceiptReport")]
        public async Task<IActionResult> MiscellaneousReceiptReport([FromQuery] string DateFrom, [FromQuery] string DateTo)
        {

            var receipt = await _unitOfWork.Report.MReceiptReport(DateFrom, DateTo);

            return Ok(receipt);

        }

        [HttpGet]
        [Route("MiscellaneousIssueReport")]
        public async Task<IActionResult> MiscellaneousIssueReport([FromQuery] string DateFrom, [FromQuery] string DateTo)
        {

            var issue = await _unitOfWork.Report.MIssueReport(DateFrom, DateTo);

            return Ok(issue);

        }

        [HttpGet]
        [Route("NearlyExpireItemsReport")]
        public async Task<IActionResult> NearlyExpireItemsReport([FromQuery] int expirydays)
        {

            var expiry = await _unitOfWork.Report.NearlyExpireItemsReport(expirydays);

            return Ok(expiry);

        }

        [HttpGet]
        [Route("TransactedMoveOrderReport")]
        public async Task<IActionResult> TransactedMoveOrderReport([FromQuery] string DateFrom, [FromQuery] string DateTo)
        {

            var transact = await _unitOfWork.Report.TransactedMoveOrderReport(DateFrom, DateTo);

            return Ok(transact);

        }

        [HttpGet]
        [Route("CancelledOrderReport")]
        public async Task<IActionResult> CancelledOrderReport([FromQuery] string DateFrom, [FromQuery] string DateTo)
        {

            var cancel = await _unitOfWork.Report.CancelledOrderedReports(DateFrom, DateTo);

            return Ok(cancel);

        }

        [HttpGet]
        [Route("InventoryMovementReport")]
        public async Task<IActionResult> InventoryMovementReport([FromQuery] string DateFrom, [FromQuery] string DateTo, [FromQuery] string PlusOne)
        {

            var cancel = await _unitOfWork.Report.InventoryMovementReports(DateFrom, DateTo, PlusOne);

            return Ok(cancel);

        }

        [HttpGet("GeneralLedgerExport")]
        public async Task<IActionResult> GeneralLedgerExport([FromQuery] GeneralLedgerExportCommand command)
        {
            var filePath = $"GeneralLedgerReports {DateTime.Today.Date.ToString("MM/dd/yyyy")}.xlsx";

            try
            {
                await _mediator.Send(command);
                var memory = new MemoryStream();
                await using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                var result = File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    filePath);
                System.IO.File.Delete(filePath);
                return result;

            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }

        }

        [HttpGet]
        [Route("GeneralLedgerReport")]
        public async Task<IActionResult> GeneralLedgerReport([FromQuery] string DateFrom, [FromQuery] string DateTo)
        {
            var reports = await _unitOfWork.Report.GeneralLedgerReport(DateFrom, DateTo);

            return Ok(reports);
        }

        [HttpGet]
        [Route("ConsolidationFinanceReports")]
        public async Task<IActionResult> ConsolidationFinanceReports([FromQuery] string DateFrom, [FromQuery] string DateTo, [FromQuery] string Search)
        {
            var reports = await _unitOfWork.Report.ConsolidateFinanceReport(DateFrom, DateTo, Search);

            return Ok(reports);
        }
        [HttpGet("ExportConsolidateFinance")]
        public async Task<IActionResult> ExportConsolidateFinance([FromQuery] ConsolidateFinanceExportCommand command)
        {
            var filePath = $"ConsolidatedReports {command.DateFrom} - {command.DateTo}.xlsx";

            try
            {
                await _mediator.Send(command);
                var memory = new MemoryStream();
                await using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                var result = File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    filePath);
                System.IO.File.Delete(filePath);
                return result;

            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpGet]
        [Route("ConsolidateAuditReport")]
        public async Task<IActionResult> ConsolidateAuditReport([FromQuery] string DateFrom, [FromQuery] string DateTo, [FromQuery] string Search)
        {
            var reports = await _unitOfWork.Report.ConsolidateAuditReport(DateFrom, DateTo, Search);

            return Ok(reports);
        }
        [HttpGet("ConsolidateAuditExport")]
        public async Task<IActionResult> ConsolidateAuditExport([FromQuery] ConsolidateAuditCommand command)
        {
            var filePath = $"ConsolidatedAuditReports {command.DateFrom} - {command.DateTo}.xlsx";

            try
            {
                await _mediator.Send(command);
                var memory = new MemoryStream();
                await using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                var result = File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    filePath);
                System.IO.File.Delete(filePath);
                return result;

            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }

        }

        [HttpGet("InventoryMovementExport")]
        public async Task<IActionResult> InventoryMovement([FromQuery] CondolidateInventoryMovementRequest request)
        {
            var filePath = $"Inventory Movement Report {request.DateFrom}-{request.DateTo}.xlsx";
            try
            {
                await _mediator.Send(request);
                var memory = new MemoryStream();
                await using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                var result = File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    filePath);
                System.IO.File.Delete(filePath);
                return result;

            }
            catch (Exception e)
            {
                return Conflict(e.Message);
            }
        }

    }
}
