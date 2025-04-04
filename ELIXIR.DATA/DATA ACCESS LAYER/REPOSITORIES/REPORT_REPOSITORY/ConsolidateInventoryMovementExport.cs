using ClosedXML.Excel;
using ELIXIR.DATA.CORE.ICONFIGURATION;
using ELIXIR.DATA.CORE.INTERFACES.REPORT_INTERFACE;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ELIXIR.DATA.DATA_ACCESS_LAYER.REPOSITORIES.REPORT_REPOSITORY
{
    public class ConsolidateInventoryMovementExport
    {
        public class CondolidateInventoryMovementRequest : IRequest<Unit>
        {
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public string PlusOne { get; set; }
        }

        public class Handler : IRequestHandler<CondolidateInventoryMovementRequest, Unit>
        {
            private readonly IUnitOfWork _report;
            public Handler(IUnitOfWork report)
            {
                _report = report;
            }

            public async Task<Unit> Handle(CondolidateInventoryMovementRequest request, CancellationToken cancellationToken)
            {
                var inventoryMovementReport = await _report.Report.ConsolidateInventoryMovementReport(request.DateFrom, request.DateTo, request.PlusOne);

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add($"Inventory Movement Report");

                    var headers = new List<string>
                {
                    "Item Code",
                    "Item Description",
                    "Item Category",
                    "Total Out",
                    "Total In",
                    "Ending",
                    "Current Stock",
                    "Purchase Order",
                    "Others",
                };

                    var range = worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(1, headers.Count));

                    range.Style.Fill.BackgroundColor = XLColor.Azure;
                    range.Style.Font.Bold = true;
                    range.Style.Font.FontColor = XLColor.Black;
                    range.Style.Border.TopBorder = XLBorderStyleValues.Thick;
                    range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    range.SetAutoFilter(true);

                    for (var index = 1; index <= headers.Count; index++)
                    {
                        worksheet.Cell(1, index).Value = headers[index - 1];
                    }

                    for (var index = 1; index <= inventoryMovementReport.Count; index++)
                    {
                        var row = worksheet.Row(index + 1);

                        row.Cell(1).Value = inventoryMovementReport[index - 1].ItemCode;
                        row.Cell(2).Value = inventoryMovementReport[index - 1].ItemDescription;
                        row.Cell(3).Value = inventoryMovementReport[index - 1].ItemCategory;
                        row.Cell(4).Value = inventoryMovementReport[index - 1].TotalOut;
                        row.Cell(5).Value = inventoryMovementReport[index - 1].TotalIn;
                        row.Cell(6).Value = inventoryMovementReport[index - 1].Ending;
                        row.Cell(7).Value = inventoryMovementReport[index - 1].CurrentStock;
                        row.Cell(8).Value = inventoryMovementReport[index - 1].PurchasedOrder;
                        row.Cell(9).Value = inventoryMovementReport[index - 1].OthersPlus;
                    }

                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs($"Inventory Movement Report {request.DateFrom}-{request.DateTo}.xlsx");
                }

                return Unit.Value;
            }
        }
    }
}
