using DocumentFormat.OpenXml.Wordprocessing;
using ELIXIR.DATA.CORE.INTERFACES.REPORT_INTERFACE;
using ELIXIR.DATA.DATA_ACCESS_LAYER.HELPERS;
using ELIXIR.DATA.DATA_ACCESS_LAYER.MODELS.ORDERING_MODEL;
using ELIXIR.DATA.DATA_ACCESS_LAYER.STORE_CONTEXT;
using ELIXIR.DATA.DTOs.INVENTORY_DTOs;
using ELIXIR.DATA.DTOs.REPORT_DTOs;
using ELIXIR.DATA.DTOs.TRANSFORMATION_DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELIXIR.DATA.DATA_ACCESS_LAYER.REPOSITORIES.REPORT_REPOSITORY
{
    public class ReportRepository : IReportRepository
    {
        private readonly StoreContext _context;

        public ReportRepository(StoreContext context)
        {
            _context = context;

        }

        public async Task<IReadOnlyList<QCReport>> QcRecevingReport(string DateFrom, string DateTo)
        {
            var items = (from rawmaterials in _context.RawMaterials
                         where rawmaterials.IsActive == true
                         join category in _context.ItemCategories
                         on rawmaterials.ItemCategoryId equals category.Id into leftJ
                         from category in leftJ.DefaultIfEmpty()

                         select new
                         {
                             ItemCode = rawmaterials.ItemCode,
                             ItemCategory = category.ItemCategoryName
                         });

            var report = (from receiving in _context.QC_Receiving
                          where receiving.QC_ReceiveDate >= DateTime.Parse(DateFrom) && receiving.QC_ReceiveDate <= DateTime.Parse(DateTo) && receiving.IsActive == true
                          join posummary in _context.POSummary
                          on receiving.PO_Summary_Id equals posummary.Id into leftJ
                          from posummary in leftJ.DefaultIfEmpty()

                          join category in items
                          on posummary.ItemCode equals category.ItemCode
                          into leftJ2
                          from category in leftJ2.DefaultIfEmpty()


                          select new QCReport
                          {

                              Id = receiving.Id,
                              QcDate = receiving.QC_ReceiveDate.ToString(),
                              PONumber = posummary != null ? posummary.PO_Number : 0,
                              ItemCode = receiving.ItemCode,
                              ItemDescription = posummary != null ? posummary.ItemDescription : null,
                              Uom = posummary != null ? posummary.UOM : null,
                              Category = category != null ? category.ItemCategory : null,
                              Quantity = receiving.Actual_Delivered,
                              ManufacturingDate = receiving.Manufacturing_Date.ToString(),
                              ExpirationDate = receiving.Expiry_Date.ToString(),
                              TotalReject = receiving.TotalReject,
                              SupplierName = posummary != null ? posummary.VendorName : null,
                              Price = posummary != null ? posummary.UnitPrice : 0,
                              QcBy = receiving.QcBy,
                              TruckApproval1 = receiving.Truck_Approval1,
                              TruckApprovalRemarks1 = receiving.Truck_Approval1_Remarks,
                              TruckApproval2 = receiving.Truck_Approval2,
                              TruckApprovalRemarks2 = receiving.Truck_Approval2_Remarks,
                              TruckApproval3 = receiving.Truck_Approval3,
                              TruckApprovalRemarks3 = receiving.Truck_Approval3_Remarks,
                              TruckApproval4 = receiving.Truck_Approval4,
                              TruckApprovalRemarks4 = receiving.Truck_Approval4_Remarks,
                              UnloadingApproval1 = receiving.Unloading_Approval1,
                              UnloadingApprovalRemarks1 = receiving.Unloading_Approval1_Remarks,
                              UnloadingApproval2 = receiving.Unloading_Approval2,
                              UnloadingApprovalRemarks2 = receiving.Unloading_Approval2_Remarks,
                              UnloadingApproval3 = receiving.Unloading_Approval3,
                              UnloadingApprovalRemarks3 = receiving.Unloading_Approval3_Remarks,
                              UnloadingApproval4 = receiving.Unloading_Approval4,
                              UnloadingApprovalRemarks4 = receiving.Unloading_Approval4_Remarks,
                              CheckingApproval1 = receiving.Checking_Approval1,
                              CheckingApprovalRemarks1 = receiving.Checking_Approval1_Remarks,
                              CheckingApproval2 = receiving.Checking_Approval2, 
                              CheckingApprovalRemarks2 = receiving.Checking_Approval2_Remarks,
                              QAApproval = receiving.QA_Approval, 
                              QAApprovalRemarks = receiving.QA_Approval_Remarks

                          });

            return await report.ToListAsync();

        }
        public async Task<IReadOnlyList<WarehouseReport>> WarehouseRecivingReport(string DateFrom, string DateTo)
        {

            var warehouse = _context.WarehouseReceived.Where(x => x.ReceivingDate >= DateTime.Parse(DateFrom) && x.ReceivingDate <= DateTime.Parse(DateTo))
                                                      .Where(x => x.IsActive == true)
            .Select(x => new WarehouseReport
            { 
                WarehouseId = x.Id,
                PONumber = x.PO_Number,
                ReceiveDate = x.ReceivingDate.ToString(),
                ItemCode = x.ItemCode,
                ItemDescription = x.ItemDescription, 
                Uom = x.Uom, 
                Category = x.LotCategory, 
                Quantity = x.ActualGood,
                ManufacturingDate = x.ManufacturingDate.ToString(),
                ExpirationDate = x.Expiration.ToString(),
                TotalReject = x.TotalReject, 
                SupplierName = x.Supplier,
                ReceivedBy = x.ReceivedBy,
                TransactionType = x.TransactionType

            });

            return await warehouse.ToListAsync();

        }

        public async Task<IReadOnlyList<TransformationReportTesting>> TransformationReport(string DateFrom, string DateTo)
        {

            //var transform = (from planning in _context.Transformation_Planning
            //                 where planning.ProdPlan >= DateTime.Parse(DateFrom) && planning.ProdPlan <= DateTime.Parse(DateTo) && planning.Status == true
            //                 join preparation in _context.Transformation_Preparation
            //                 on planning.Id equals preparation.TransformId into leftJ
            //                 from preparation in leftJ.DefaultIfEmpty()

            //                 join warehouse in _context.WarehouseReceived
            //                 on planning.Id equals warehouse.TransformId into left2
            //                 from warehouse in left2.DefaultIfEmpty()

            //                 select new TransformationReport
            //                 {
            //                     TransformationId = planning.Id,
            //                     PlanningDate = planning.ProdPlan.ToString(),
            //                     ItemCode_Formula = planning.ItemCode,
            //                     ItemDescription_Formula = planning.ItemDescription,
            //                     Version = planning.Version,
            //                     Batch = planning.Batch,
            //                     Formula_Quantity = planning.Quantity,
            //                     ItemCode_Recipe = preparation.ItemCode != null ? preparation.ItemCode : null,
            //                     ItemDescription_Recipe = preparation.ItemDescription != null ? preparation.ItemDescription : null,
            //                     Recipe_Quantity = preparation.WeighingScale != null ? preparation.WeighingScale : 0,
            //                     DateTransformed = warehouse.ManufacturingDate.ToString()

            //                 }).GroupBy(x => new
            //                 {
            //                     x.TransformationId,
            //                     x.PlanningDate,
            //                     x.ItemCode_Formula,
            //                     x.ItemDescription_Formula,
            //                     x.Version,
            //                     x.Batch,
            //                     x.Formula_Quantity,
            //                     x.ItemCode_Recipe,
            //                     x.ItemDescription_Recipe,
            //                     x.Recipe_Quantity

            //                 }).Select(transform => new TransformationReport
            //                 {
            //                     TransformationId = transform.Key.TransformationId,
            //                     PlanningDate = transform.Key.PlanningDate.ToString(),
            //                     ItemCode_Formula = transform.Key.ItemCode_Formula,
            //                     Version = transform.Key.Version,
            //                     Batch = transform.Key.Batch,
            //                     Formula_Quantity = transform.Key.Formula_Quantity,
            //                     ItemCode_Recipe = transform.Key.ItemCode_Recipe,
            //                     ItemDescription_Recipe = transform.Key.ItemDescription_Recipe,
            //                     Recipe_Quantity = transform.Key.Recipe_Quantity

            //                 });

            //return await transform.ToListAsync();




            //var transform = (from planning in _context.Transformation_Planning
            //                 where planning.ProdPlan >= DateTime.Parse(DateFrom) && planning.ProdPlan <= DateTime.Parse(DateTo) && planning.Status == true
            //                 join preparation in _context.Transformation_Preparation
            //                 on planning.Id equals preparation.TransformId into leftJ
            //                 from preparation in leftJ.DefaultIfEmpty()

            //                 join warehouse in _context.WarehouseReceived
            //                 on planning.Id equals warehouse.TransformId into left2
            //                 from warehouse in left2.DefaultIfEmpty()

            //                 select new TransformationReport
            //                 {
            //                     TransformationId = planning.Id,
            //                     PlanningDate = planning.ProdPlan.ToString(),
            //                     ItemCode_Formula = planning.ItemCode,
            //                     ItemDescription_Formula = planning.ItemDescription,
            //                     Version = planning.Version,
            //                     Batch = planning.Batch,
            //                     Formula_Quantity = planning.Quantity,
            //                     ItemCode_Recipe = preparation.ItemCode != null ? preparation.ItemCode : null,
            //                     ItemDescription_Recipe = preparation.ItemDescription != null ? preparation.ItemDescription : null,
            //                     Recipe_Quantity = preparation.WeighingScale != null ? preparation.WeighingScale : 0,
            //                     DateTransformed = warehouse.ManufacturingDate.ToString()

            //                 });

            //var transformHistory = (from transform in _context.Transformation_Planning
            //                        where transform.ProdPlan >= DateTime.Parse(DateFrom) && transform.ProdPlan <= DateTime.Parse(DateTo) && transform.Status == true
            //                        select new
            //                        {
            //                            Id = transform.Id,
            //                            ItemCode = transform.ItemCode,
            //                            ItemDescription = transform.ItemDescription,
            //                            Total = transform.Batch * transform.Quantity,
            //                            Category = "Formula",
            //                            Batch = transform.Batch,
            //                            Version = transform.Version

            //                        });


            //var preparationHistory = from preparation in _context.Transformation_Preparation
            //                         select new
            //                         {
            //                             Id = preparation.TransformId,
            //                             ItemCode = preparation.ItemCode,
            //                             ItemDescription = preparation.ItemDescription,
            //                             Total = preparation.WeighingScale,
            //                             Category = "Recipe",
            //                             Batch = preparation.Batch,
            //                             Version = preparation.Batch
            //                         };


            //var unionResult = transformHistory.Union(preparationHistory).Select(x => new TransformationReportTesting
            //{
            //    TransformId = x.Id,
            //    ItemCode = x.ItemCode,
            //    ItemDescription = x.ItemDescription,
            //    TotalQuantity = x.Total,
            //    Category = x.Category,
            //    Batch = x.Batch,
            //    Version = x.Version

            //});


            //var getDatesTransformed = (from planning in _context.Transformation_Planning
            //                           where planning.ProdPlan >= DateTime.Parse(DateFrom) && planning.ProdPlan <= DateTime.Parse(DateTo) && planning.Status == true
            //                           join warehouse in _context.WarehouseReceived
            //                           on planning.Id equals warehouse.TransformId
            //                           select new
            //                           {
            //                               TranformId = planning.Id,
            //                               PlanningDate = planning.ProdPlan.ToString("MM/dd/yyyy"),
            //                               TransformDate = warehouse.ReceivingDate.ToString("MM/dd/yyyy")
            //                           });



            //var finalResult = (from union in unionResult
            //                   join dates in getDatesTransformed
            //                   on union.TransformId equals dates.TranformId
            //                   into leftJ
            //                   from dates in leftJ.DefaultIfEmpty()

            //                   select new TransformationReportTesting
            //                   {
            //                       TransformId = union.TransformId, 
            //                       ItemCode = union.ItemCode,
            //                       ItemDescription = union.ItemDescription, 
            //                       TotalQuantity = union.TotalQuantity, 
            //                       Category = union.Category, 
            //                       Batch = union.Batch, 
            //                       Version = union.Version, 
            //                       PlanningDate = dates.PlanningDate, 
            //                       DateTransformed = dates.TransformDate
            //                   });

            //    return await finalResult.ToListAsync();

            //   .OrderByDescending(x => x.PreparedDate);


            //return await unionResult.OrderBy(x => x.TransformId)
            //                        .ToListAsync();

            //        var transformationPlanning = _context.Transformation_Planning
            //.Select(x => new
            //{
            //    TransformId = x.Id,
            //    ItemCode = x.ItemCode,
            //    ItemDescription = x.ItemDescription,
            //    Total = x.Batch * x.Quantity,
            //    Category = "Formula"
            //});

            //        var transformationPreparation = _context.Transformation_Preparation
            //            .Select(x => new
            //            {
            //                TransformId = x.TransformId,
            //                ItemCode = x.ItemCode,
            //                ItemDescription = x.ItemDescription,
            //                Total = x.WeighingScale,
            //                Category = "Recipe"
            //            });

            //        var result = transformationPlanning.Union(transformationPreparation)
            //                       .Select(x => new TransformationReportTesting
            //                       {
            //                           TransformId = x.TransformId,
            //                           ItemCode = x.ItemCode,
            //                           ItemDescription = x.ItemDescription,
            //                           TotalQuantity = x.Total,
            //                           Category = x.Category

            //                       });

            //        return await result.ToListAsync();

            //var result = (from tp in _context.Transformation_Planning
            //              join warehouse in _context.WarehouseReceived on tp.Id equals warehouse.TransformId
            //              where tp.ProdPlan >= DateTime.Parse(DateFrom) && tp.ProdPlan <= DateTime.Parse(DateTo) && tp.Status == true
            //              select new
            //              {
            //                  TransformId = tp.Id,
            //                  ItemCode = tp.ItemCode,
            //                  ItemDescription = tp.ItemDescription,
            //                  Total = tp.Batch * tp.Quantity,
            //                  Category = "Formula",
            //                  ProdPlan = tp.ProdPlan
            //                  DateTransformed = warehouse.ReceivingDate
            //              }).Union(
            //             from tp in _context.Transformation_Preparation
            //             group tp by new { tp.ItemCode, tp.ItemDescription } into g
            //             select new
            //             {
            //                 TransformId = g.First().TransformId,
            //                 ItemCode = g.Key.ItemCode,
            //                 ItemDescription = g.Key.ItemDescription,
            //                 Total = g.Sum(x => x.WeighingScale),
            //                 Category = "Recipe",
            //                 ProdPlan = (DateTime?)null,
            //                 DateTransFormed = (DateTime?)null
            //             });

            //var filteredPlanning = _context.Transformation_Planning                      
            //          .Where(tp => tp.ProdPlan >= DateTime.Parse(DateFrom) && tp.ProdPlan <= DateTime.Parse(DateTo) && tp.Status == true);

            var result = (from tp in _context.Transformation_Planning
                          where tp.ProdPlan >= DateTime.Parse(DateFrom) && tp.ProdPlan <= DateTime.Parse(DateTo) && tp.Status == true
                          join warehouse in _context.WarehouseReceived on tp.Id equals warehouse.TransformId
                        
                          select new
                          {
                              TransformId = tp.Id,
                              ItemCode = tp.ItemCode,
                              ItemDescription = tp.ItemDescription,
                              ActualQuantity = tp.Batch * tp.Quantity,
                              TotalQuantity = tp.Batch * tp.Quantity,
                              Category = "Formula",
                              ProdPlan = tp.ProdPlan.ToString(),
                              DateTransformed = warehouse.ReceivingDate.ToString(),
                              Version = tp.Version,
                              Batch = tp.Batch


                          }).Union(
             from tp2 in _context.Transformation_Planning
             join tp in _context.Transformation_Preparation on tp2.Id equals tp.TransformId
             where tp2.ProdPlan >= DateTime.Parse(DateFrom) && tp2.ProdPlan <= DateTime.Parse(DateTo) && tp2.Status == true && tp.IsActive == true && tp.IsMixed == true
             group tp by new { tp.TransformId, tp.ItemCode, tp.QuantityNeeded, tp.ItemDescription, tp2.Version, tp2.Batch } into g
             select new
             {
                 TransformId = g.Key.TransformId,
                 ItemCode = g.Key.ItemCode,
                 ItemDescription = g.Key.ItemDescription,
                 ActualQuantity = g.Sum(x => x.WeighingScale),
                 TotalQuantity = g.Key.QuantityNeeded,
                 Category = "Recipe",
                 ProdPlan = (string)null,
                 DateTransformed = (string)null,
                 Version = g.Key.Version,
                 Batch = g.Key.Batch
             });


            return await result.Select(x => new TransformationReportTesting
            {
                TransformId = x.TransformId,
                ItemCode = x.ItemCode,
                ItemDescription = x.ItemDescription,
                ActualQuantity = x.ActualQuantity,
                TotalQuantity = x.TotalQuantity,
                Category = x.Category,
                PlanningDate = x.ProdPlan,
                DateTransformed = x.DateTransformed,
                Version = x.Version, 
                Batch = x.Batch
            }).ToListAsync();

            //return await result.Select(x => new TransformationReportTesting
            //{
            //    TransformId = x.TransformId,
            //    ItemCode = x.ItemCode,
            //    ItemDescription = x.ItemDescription,
            //    TotalQuantity = x.Total,
            //    Category = x.Category,
            //    PlanningDate = x.ProdPlan,
            //    DateTransformed = x.DateTransformed
            //}).ToListAsync(); 

        }

        public async Task<IReadOnlyList<MoveOrderReport>> MoveOrderReport(string DateFrom, string DateTo)
        {
            var orders = (from moveorder in _context.MoveOrders
                          where moveorder.PreparedDate >= DateTime.Parse(DateFrom) && moveorder.PreparedDate <= DateTime.Parse(DateTo) && moveorder.IsActive == true
                          join transactmoveorder in _context.TransactMoveOrder
                          on moveorder.OrderNo equals transactmoveorder.OrderNo into leftJ
                          from transactmoveorder in leftJ.DefaultIfEmpty()
                        
                          select new MoveOrderReport
                          {
                              MoveOrderId = moveorder.OrderNo,
                              CustomerCode = moveorder.FarmCode,
                              CustomerName = moveorder.FarmName,
                              ItemCode = moveorder.ItemCode,
                              ItemDescription = moveorder.ItemDescription,
                              Uom = moveorder.Uom,
                              Category = moveorder.Category,
                              Quantity = moveorder.QuantityOrdered,
                              ExpirationDate = moveorder.ExpirationDate.ToString(),
                              TransactionType = moveorder.DeliveryStatus,
                              MoveOrderBy = moveorder.PreparedBy,
                              MoveOrderDate = moveorder.ApprovedDate.ToString(),
                              PreparedDate = moveorder.PreparedDate.ToString(),
                              BatchNo = moveorder.BatchNo,
                              TransactedBy = transactmoveorder.PreparedBy,
                              TransactedDate = transactmoveorder.PreparedDate.ToString(),
                              OrderRemarks = moveorder.OrderRemarks
                          });

            return await orders.ToListAsync();

        }

        public async Task<IReadOnlyList<MiscellaneousReceiptReport>> MReceiptReport(string DateFrom, string DateTo)
        {

            var receipts = (from receiptHeader in _context.MiscellaneousReceipts                    
                            join receipt in _context.WarehouseReceived
                            on receiptHeader.Id equals receipt.MiscellaneousReceiptId into leftJ
                            from receipt in leftJ.DefaultIfEmpty()

                            where receipt.ReceivingDate >= DateTime.Parse(DateFrom) && receipt.ReceivingDate <= DateTime.Parse(DateTo) && receipt.IsActive == true && receipt.TransactionType == "MiscellaneousReceipt"

                            select new MiscellaneousReceiptReport
                            {

                                ReceiptId = receiptHeader.Id,
                                SupplierCode = receiptHeader.SupplierCode,
                                SupplierName = receiptHeader.Supplier,
                                Details = receiptHeader.Details,
                                Remarks = receiptHeader.Remarks,
                                ItemCode = receipt.ItemCode,
                                ItemDescription = receipt.ItemDescription,
                                Uom = receipt.Uom,
                                Category = receipt.LotCategory,
                                Quantity = receipt.ActualGood,
                                ExpirationDate = receipt.Expiration.ToString(),
                                TransactBy = receiptHeader.PreparedBy,
                                TransactDate = receipt.ReceivingDate.ToString(),
                                TransactionDate = receiptHeader.TransactionDate.ToString()

                            });

            return await receipts.ToListAsync();


        }

        public async Task<IReadOnlyList<MiscellaneousIssueReport>> MIssueReport(string DateFrom, string DateTo)
        {

            var issues = (from issue in _context.MiscellaneousIssues
                          join issuedetails in _context.MiscellaneousIssueDetails
                          on issue.Id equals issuedetails.IssuePKey into leftJ
                          from issuedetails in leftJ.DefaultIfEmpty()

                          where issuedetails.PreparedDate >= DateTime.Parse(DateFrom) && issuedetails.PreparedDate <= DateTime.Parse(DateTo) && issuedetails.IsActive == true && issuedetails.IsTransact == true

                          select new MiscellaneousIssueReport
                          {

                              IssueId = issue.Id,
                              CustomerCode = issue.CustomerCode,
                              CustomerName = issue.Customer,
                              Details = issue.Details,
                              Remarks = issue.Remarks,
                              ItemCode = issuedetails != null ? issuedetails.ItemCode : null,
                              ItemDescription = issuedetails != null ? issuedetails.ItemDescription : null,
                              Uom = issuedetails != null ? issuedetails.Uom : null,
                              Quantity = issuedetails != null ? issuedetails.Quantity : 0,
                              ExpirationDate = issuedetails != null ? issuedetails.ExpirationDate.ToString() : null,
                              TransactBy = issue.PreparedBy,
                              TransactDate = issuedetails != null ? issuedetails.PreparedDate.ToString() : null,
                              TransactionDate = issue.TransactionDate.ToString()

                          });

            return await issues.ToListAsync();
        }

        public async Task<IReadOnlyList<WarehouseReport>> NearlyExpireItemsReport(int expirydays)
        {
            var preparationOut = _context.Transformation_Preparation.Where(x => x.IsActive == true)                                                             
                .GroupBy(x => new
            {
                x.ItemCode,
                x.WarehouseId,

            }).Select(x => new ItemStocks
            {
                ItemCode = x.Key.ItemCode,
                Out = x.Sum(x => x.WeighingScale),
                WarehouseId = x.Key.WarehouseId
            });

            var moveorderOut = _context.MoveOrders.Where(x => x.IsActive == true)
                                                  .Where(x => x.IsPrepared == true)
                .GroupBy(x => new
            {
                x.ItemCode,
                x.WarehouseId,

            }).Select(x => new ItemStocks
            {
                ItemCode = x.Key.ItemCode,
                Out = x.Sum(x => x.QuantityOrdered),
                WarehouseId = x.Key.WarehouseId
            });

            var issueOut = _context.MiscellaneousIssueDetails.Where(x => x.IsActive == true)
                                                             .Where(x => x.IsTransact == true)
                .GroupBy(x => new
            {
                x.ItemCode,
                x.WarehouseId,

            }).Select(x => new ItemStocks
            {
                ItemCode = x.Key.ItemCode,
                Out = x.Sum(x => x.Quantity),
                WarehouseId = x.Key.WarehouseId
            });

            var warehouseInventory = (from warehouse in _context.WarehouseReceived
                                  where warehouse.ExpirationDays <= expirydays
                                  join preparation in preparationOut
                                  on warehouse.Id equals preparation.WarehouseId
                                  into leftJ
                                  from preparation in leftJ.DefaultIfEmpty()

                                  join moveorder in moveorderOut
                                  on warehouse.Id equals moveorder.WarehouseId
                                  into leftJ2
                                  from moveorder in leftJ2.DefaultIfEmpty()

                                  join issue in issueOut
                                  on warehouse.Id equals issue.WarehouseId
                                  into leftJ3
                                  from issue in leftJ3.DefaultIfEmpty()

                                      group new
                                  {
                                      warehouse,
                                      preparation,
                                      moveorder,
                                      issue
                                  }
                                  by new 
                                  {
                                      warehouse.Id,
                                      warehouse.PO_Number,
                                      warehouse.ItemCode,
                                      warehouse.ItemDescription,
                                      warehouse.ManufacturingDate,
                                      warehouse.ReceivingDate,
                                      warehouse.LotCategory,
                                      warehouse.Uom,
                                      warehouse.ActualGood,
                                      warehouse.Expiration,
                                      warehouse.ExpirationDays,
                                      warehouse.Supplier,
                                      warehouse.ReceivedBy,
                                      PreparationOut = preparation.Out != null ? preparation.Out : 0,
                                      MoveOrderOut = moveorder.Out != null ? moveorder.Out : 0,
                                      IssueOut = issue.Out != null ? issue.Out : 0


                                  } into total

                                  orderby total.Key.ItemCode, total.Key.ExpirationDays ascending

                                  select new WarehouseReport
                                  {
                                      WarehouseId = total.Key.Id,
                                      PONumber = total.Key.PO_Number,
                                      ItemCode = total.Key.ItemCode,
                                      ItemDescription = total.Key.ItemDescription,
                                      Uom = total.Key.Uom,
                                      Category = total.Key.LotCategory,
                                      ReceiveDate = total.Key.ReceivingDate.ToString(),
                                      ManufacturingDate = total.Key.ManufacturingDate.ToString(),
                                      Quantity = total.Key.ActualGood - total.Key.PreparationOut - total.Key.MoveOrderOut - total.Key.IssueOut,
                                      ExpirationDate = total.Key.Expiration.ToString(),
                                      ExpirationDays = total.Key.ExpirationDays,
                                      SupplierName = total.Key.Supplier,
                                      ReceivedBy = total.Key.ReceivedBy

                                  });

            return await warehouseInventory.Where(x => x.Quantity != 0)
                                           .ToListAsync();
                                
        }

        public async Task<IReadOnlyList<MoveOrderReport>> TransactedMoveOrderReport(string DateFrom, string DateTo) 
        {
            var orders = (from transact in _context.TransactMoveOrder
                          where transact.IsActive == true && transact.IsTransact == true && transact.PreparedDate >= DateTime.Parse(DateFrom) && transact.PreparedDate <= DateTime.Parse(DateTo)
                          join moveorder in _context.MoveOrders
                          on transact.OrderNo equals moveorder.OrderNo into leftJ
                          from moveorder in leftJ.DefaultIfEmpty()
                          where moveorder.IsActive == true

                          join customer in _context.Customers
                  on new { Code = moveorder.FarmCode, Name = moveorder.FarmName} equals new { Code = customer.CustomerCode, Name = customer.CustomerName } into leftJ2
                          from customer in leftJ2.DefaultIfEmpty()
                          where customer.IsActive == true

                          group new
                          {
                              transact,
                              moveorder,
                              customer
                          }

                          by new
                          {
                              moveorder.OrderNo,
                              customer.CustomerName,
                              customer.CustomerCode,
                              moveorder.ItemCode,
                              moveorder.ItemDescription,
                              moveorder.Uom,
                              moveorder.QuantityOrdered,
                              MoveOrderDate = moveorder.ApprovedDate.ToString(),
                              transact.PreparedBy,
                              moveorder.DeliveryStatus,
                              TransactedDate = transact.PreparedDate.ToString(),
                              moveorder.BatchNo,
                              DeliveryDate = transact.DeliveryDate.ToString(),
                              moveorder.OrderRemarks,
                              moveorder.Remarks
                              
                          } into total

                          select new MoveOrderReport
                          {
                              OrderNo = total.Key.OrderNo,
                              CustomerName = total.Key.CustomerName,
                              CustomerCode = total.Key.CustomerCode,
                              ItemCode = total.Key.ItemCode,
                              ItemDescription = total.Key.ItemDescription,
                              Uom = total.Key.Uom,
                              Quantity = total.Key.QuantityOrdered,
                              MoveOrderDate = total.Key.MoveOrderDate,
                              TransactedBy = total.Key.PreparedBy,
                              TransactionType = total.Key.DeliveryStatus,
                              TransactedDate = total.Key.TransactedDate,
                              BatchNo = total.Key.BatchNo,
                              DeliveryDate = total.Key.DeliveryDate,
                             OrderRemarks = total.Key.OrderRemarks,
                             Remarks = total.Key.Remarks
                             
                          }).OrderBy(x => x.TransactedDate);
        
            return await orders.ToListAsync();
        }

        public async Task<IReadOnlyList<CancelledOrderReport>> CancelledOrderedReports(string DateFrom, string DateTo)
        {

            var orders = (from ordering in _context.Orders
                          where ordering.OrderDate >= DateTime.Parse(DateFrom) && ordering.OrderDate <= DateTime.Parse(DateTo) &&
                          ordering.IsCancel == true && ordering.IsActive == false

                          select new CancelledOrderReport
                          {

                              OrderId = ordering.Id,
                              DateNeeded = ordering.DateNeeded.ToString(),
                              DateOrdered = ordering.OrderDate.ToString(),
                              CustomerCode = ordering.FarmCode,
                              CustomerName = ordering.FarmName,
                              ItemCode = ordering.ItemCode,
                              ItemDescription = ordering.ItemDescription,
                              QuantityOrdered = ordering.QuantityOrdered,
                              CancelledDate = ordering.CancelDate.ToString(),
                              CancelledBy = ordering.IsCancelBy,
                              Reason = ordering.Remarks
                          });

            return await orders.ToListAsync();

        }

        public async Task<IReadOnlyList<InventoryMovementReport>> InventoryMovementReports(string DateFrom, string DateTo, string PlusOne)
        {
            var dateToday = DateTime.Now.ToString("MM/dd/yyyy");
            

            var getWarehouseStock = _context.WarehouseReceived.Where(x => x.IsActive == true)
       .GroupBy(x => new
       {
           x.ItemCode,

       }).Select(x => new WarehouseInventory
       {
           ItemCode = x.Key.ItemCode,
           ActualGood = x.Sum(x => x.ActualGood)
       });

            var getMoveOrderOutByDate = _context.MoveOrders.Where(x => x.IsActive == true)
                                                           .Where(x => x.IsPrepared == true)
                                                           .Where(x => x.PreparedDate >= DateTime.Parse(DateFrom) && x.PreparedDate <= DateTime.Parse(DateTo) && x.ApprovedDate != null)
        .GroupBy(x => new
        {
            x.ItemCode,

        }).Select(x => new MoveOrderInventory
        {
            ItemCode = x.Key.ItemCode,
            QuantityOrdered = x.Sum(x => x.QuantityOrdered)

        });

            var getMoveOrderOutByDatePlus = _context.MoveOrders.Where(x => x.IsActive == true)
                                                         .Where(x => x.IsPrepared == true)
                                                         .Where(x => x.PreparedDate >= DateTime.Parse(PlusOne) && x.PreparedDate <= DateTime.Parse(dateToday) && x.ApprovedDate != null)
      .GroupBy(x => new
      {
          x.ItemCode,

      }).Select(x => new MoveOrderInventory
      {
          ItemCode = x.Key.ItemCode,
          QuantityOrdered = x.Sum(x => x.QuantityOrdered)

      });

            var getTransformationByDate = _context.Transformation_Preparation.Where(x => x.IsActive == true)
                                                                             .Where(x => x.PreparedDate >= DateTime.Parse(DateFrom) && x.PreparedDate <= DateTime.Parse(DateTo))
              .GroupBy(x => new
              {
                  x.ItemCode,

              }).Select(x => new TransformationInventory
              {
                  ItemCode = x.Key.ItemCode,
                  WeighingScale = x.Sum(x => x.WeighingScale)
              });


            var getTransformationByDatePlus = _context.Transformation_Preparation.Where(x => x.IsActive == true)
                                                                                 .Where(x => x.PreparedDate >= DateTime.Parse(PlusOne) && x.PreparedDate <= DateTime.Parse(dateToday))
              .GroupBy(x => new
              {
                  x.ItemCode,

              }).Select(x => new TransformationInventory
              {
                  ItemCode = x.Key.ItemCode,
                  WeighingScale = x.Sum(x => x.WeighingScale)
              });

            var getIssueOutByDate= _context.MiscellaneousIssueDetails.Where(x => x.IsActive == true)
                                                                     .Where(x => x.IsTransact == true)
                                                                     .Where(x => x.PreparedDate >= DateTime.Parse(DateFrom) && x.PreparedDate <= DateTime.Parse(DateTo))
             .GroupBy(x => new
             {
                 x.ItemCode,

             }).Select(x => new IssueInventory
             {
                 ItemCode = x.Key.ItemCode,
                 Quantity = x.Sum(x => x.Quantity)
             });


            var getIssueOutByDatePlus = _context.MiscellaneousIssueDetails.Where(x => x.IsActive == true)
                                                                     .Where(x => x.IsTransact == true)
                                                                     .Where(x => x.PreparedDate >= DateTime.Parse(PlusOne) && x.PreparedDate <= DateTime.Parse(dateToday))
             .GroupBy(x => new
             {
                 x.ItemCode,

             }).Select(x => new IssueInventory
             {
                 ItemCode = x.Key.ItemCode,
                 Quantity = x.Sum(x => x.Quantity)
             });


            var getReceivetIn = _context.WarehouseReceived.Where(x => x.IsActive == true)
                                                          .Where(x => x.TransactionType == "Receiving")
                                                          .Where(x => x.ReceivingDate >= DateTime.Parse(DateFrom) && x.ReceivingDate <= DateTime.Parse(DateTo))
      .GroupBy(x => new 
      {
          x.ItemCode,

      }).Select(x => new ReceiptInventory
      {
          ItemCode = x.Key.ItemCode,
          Quantity = x.Sum(x => x.ActualGood)
      });

            var getReceivetInPlus = _context.WarehouseReceived.Where(x => x.IsActive == true)
                                                               .Where(x => x.TransactionType == "Receiving")
                                                               .Where(x => x.ReceivingDate >= DateTime.Parse(PlusOne) && x.ReceivingDate <= DateTime.Parse(dateToday))
           .GroupBy(x => new
           {
               x.ItemCode,

           }).Select(x => new ReceiptInventory
           {
               ItemCode = x.Key.ItemCode,
               Quantity = x.Sum(x => x.ActualGood)
           });



            var getTransformIn = _context.WarehouseReceived.Where(x => x.IsActive == true)
                                                           .Where(x => x.TransactionType == "Transformation")
                                                           .Where(x => x.ReceivingDate >= DateTime.Parse(DateFrom) && x.ReceivingDate <= DateTime.Parse(DateTo))
   .GroupBy(x => new
   {
       x.ItemCode,

   }).Select(x => new ReceiptInventory
   {
       ItemCode = x.Key.ItemCode,
       Quantity = x.Sum(x => x.ActualGood)
   });

            var getTransformInPlus = _context.WarehouseReceived.Where(x => x.IsActive == true)
                                                         .Where(x => x.TransactionType == "Transformation")
                                                         .Where(x => x.ReceivingDate >= DateTime.Parse(PlusOne) && x.ReceivingDate <= DateTime.Parse(dateToday))
 .GroupBy(x => new
 {
     x.ItemCode,

 }).Select(x => new ReceiptInventory
 {
     ItemCode = x.Key.ItemCode,
     Quantity = x.Sum(x => x.ActualGood)
 });


            var getReceiptIn = _context.WarehouseReceived.Where(x => x.IsActive == true)
                                                         .Where(x => x.TransactionType == "MiscellaneousReceipt")
                                                         .Where(x => x.ReceivingDate >= DateTime.Parse(DateFrom) && x.ReceivingDate <= DateTime.Parse(DateTo))
       .GroupBy(x => new
       {
           x.ItemCode,

       }).Select(x => new ReceiptInventory
       {
           ItemCode = x.Key.ItemCode,
           Quantity = x.Sum(x => x.ActualGood)
       });

            var getReceiptInPlus = _context.WarehouseReceived.Where(x => x.IsActive == true)
                                                    .Where(x => x.TransactionType == "MiscellaneousReceipt")
                                                    .Where(x => x.ReceivingDate >= DateTime.Parse(PlusOne) && x.ReceivingDate <= DateTime.Parse(dateToday))
  .GroupBy(x => new
  {
      x.ItemCode,

  }).Select(x => new ReceiptInventory
  {
      ItemCode = x.Key.ItemCode,
      Quantity = x.Sum(x => x.ActualGood)
  });

            var getMoveOrderOut = _context.MoveOrders.Where(x => x.IsActive == true)
                                                     .Where(x => x.IsPrepared == true)
                   .GroupBy(x => new
                   {
                       x.ItemCode,

                   }).Select(x => new MoveOrderInventory
                   {
                       ItemCode = x.Key.ItemCode,
                       QuantityOrdered = x.Sum(x => x.QuantityOrdered)

                   });


            var getTransformation = _context.Transformation_Preparation.Where(x => x.IsActive == true)
              .GroupBy(x => new
              {
                  x.ItemCode,

              }).Select(x => new TransformationInventory
              {
                  ItemCode = x.Key.ItemCode,
                  WeighingScale = x.Sum(x => x.WeighingScale)
              });

            var getIssueOut = _context.MiscellaneousIssueDetails.Where(x => x.IsActive == true)
                                                              .Where(x => x.IsTransact == true)
              .GroupBy(x => new
              {
                  x.ItemCode,

              }).Select(x => new IssueInventory
              {
                  ItemCode = x.Key.ItemCode,
                  Quantity = x.Sum(x => x.Quantity)
              });

            var getSOH = (from warehouse in getWarehouseStock
                          join preparation in getTransformation
                          on warehouse.ItemCode equals preparation.ItemCode
                          into leftJ1
                          from preparation in leftJ1.DefaultIfEmpty()

                          join issue in getIssueOut
                          on warehouse.ItemCode equals issue.ItemCode
                          into leftJ2
                          from issue in leftJ2.DefaultIfEmpty()

                          join moveorder in getMoveOrderOut
                          on warehouse.ItemCode equals moveorder.ItemCode
                          into leftJ3
                          from moveorder in leftJ3.DefaultIfEmpty()

                          join receipt in getReceiptIn
                          on warehouse.ItemCode equals receipt.ItemCode
                          into leftJ4
                          from receipt in leftJ4.DefaultIfEmpty()


                          group new
                          {
                              warehouse,
                              preparation,
                              moveorder,
                              receipt,
                              issue
                          }

                          by new
                          {
                              warehouse.ItemCode

                          } into total

                          select new SOHInventory
                          {
                              ItemCode = total.Key.ItemCode,
                              SOH = total.Sum(x => x.warehouse.ActualGood == null ? 0 : x.warehouse.ActualGood) -
                                    total.Sum(x => x.preparation.WeighingScale == null ? 0 : x.preparation.WeighingScale) -
                                    total.Sum(x => x.moveorder.QuantityOrdered == null ? 0 : x.moveorder.QuantityOrdered)

                          });

            var movementInventory = (from rawmaterial in _context.RawMaterials
                                     join moveorder in getMoveOrderOutByDate
                                     on rawmaterial.ItemCode equals moveorder.ItemCode
                                     into leftJ
                                     from moveorder in leftJ.DefaultIfEmpty()

                                     join transformation in getTransformationByDate
                                     on rawmaterial.ItemCode equals transformation.ItemCode
                                     into leftJ2
                                     from transformation in leftJ2.DefaultIfEmpty()

                                     join issue in getIssueOutByDate
                                     on rawmaterial.ItemCode equals issue.ItemCode
                                     into leftJ3
                                     from issue in leftJ3.DefaultIfEmpty()

                                     join receive in getReceivetIn
                                     on rawmaterial.ItemCode equals receive.ItemCode
                                     into leftJ4
                                     from receive in leftJ4.DefaultIfEmpty()

                                     join transformIn in getTransformIn
                                     on rawmaterial.ItemCode equals transformIn.ItemCode
                                    into leftJ5
                                     from transformIn in leftJ5.DefaultIfEmpty()

                                     join receipt in getReceiptIn
                                     on rawmaterial.ItemCode equals receipt.ItemCode
                                     into leftJ6 
                                     from receipt in leftJ6.DefaultIfEmpty()

                                     join SOH in getSOH 
                                     on rawmaterial.ItemCode equals SOH.ItemCode 
                                     into leftJ7
                                     from SOH in leftJ7.DefaultIfEmpty()

                                     join receivePlus in getReceivetInPlus
                                      on rawmaterial.ItemCode equals receivePlus.ItemCode
                                     into leftJ8
                                     from receivePlus in leftJ8.DefaultIfEmpty()

                                     join transformPlus in getTransformInPlus
                                     on rawmaterial.ItemCode equals transformPlus.ItemCode
                                     into leftJ9
                                     from transformPlus in leftJ9.DefaultIfEmpty()

                                     join receiptPlus in getReceiptInPlus
                                     on rawmaterial.ItemCode equals receiptPlus.ItemCode
                                     into leftJ10
                                     from receiptPlus in leftJ10.DefaultIfEmpty()

                                     join moveorderPlus in getMoveOrderOutByDatePlus
                                      on rawmaterial.ItemCode equals moveorderPlus.ItemCode
                                     into leftJ11
                                     from moveorderPlus in leftJ11.DefaultIfEmpty()

                                     join  transformoutPlus in getTransformationByDatePlus
                                      on rawmaterial.ItemCode equals transformoutPlus.ItemCode
                                     into leftJ12
                                     from transformoutPlus in leftJ12.DefaultIfEmpty()

                                     join issuePlus in getIssueOutByDatePlus
                                      on rawmaterial.ItemCode equals issuePlus.ItemCode
                                     into leftJ13
                                     from issuePlus in leftJ13.DefaultIfEmpty()

                                     group new
                                     {
                                         rawmaterial,
                                         moveorder,
                                         transformation,
                                         issue,
                                         receive,
                                         transformIn,
                                         receipt,
                                         SOH,
                                         receivePlus,
                                         transformPlus,
                                         receiptPlus,
                                         moveorderPlus,
                                         transformoutPlus,
                                         issuePlus
                                     }
                                     by new
                                     {
                                         rawmaterial.ItemCode,
                                         rawmaterial.ItemDescription,
                                         rawmaterial.ItemCategory.ItemCategoryName,
                                         MoveOrder = moveorder.QuantityOrdered != null ? moveorder.QuantityOrdered : 0,
                                         Transformation = transformation.WeighingScale != null ? transformation.WeighingScale : 0,
                                         Issue = issue.Quantity != null ? issue.Quantity : 0,
                                         ReceiptIn = receipt.Quantity != null ? receipt.Quantity : 0,
                                         ReceiveIn = receive.Quantity != null ? receive.Quantity : 0,
                                         TransformIn = transformIn.Quantity != null ? transformIn.Quantity : 0,
                                         SOH = SOH.SOH != null ? SOH.SOH : 0, 
                                         ReceivePlus = receivePlus.Quantity != null ? receivePlus.Quantity : 0,
                                         TransformPlus = transformPlus.Quantity != null ? transformPlus.Quantity : 0,
                                         ReceiptPlus = receiptPlus.Quantity != null ? receiptPlus.Quantity : 0,
                                         MoveOrderPlus = moveorderPlus.QuantityOrdered != null ? moveorderPlus.QuantityOrdered : 0,
                                         TransformOutPlus = transformoutPlus.WeighingScale != null ? transformoutPlus.WeighingScale : 0,
                                         IssuePlus = issuePlus.Quantity != null ? issuePlus.Quantity : 0,



                                     } into total

                                     select new InventoryMovementReport
                                     {
                                         ItemCode = total.Key.ItemCode,
                                         ItemDescription = total.Key.ItemDescription,
                                         ItemCategory = total.Key.ItemCategoryName,
                                         TotalOut = total.Key.MoveOrder + total.Key.Transformation + total.Key.Issue,
                                         TotalIn = total.Key.ReceiveIn + total.Key.ReceiptIn + total.Key.TransformIn,
                                         Ending = (total.Key.ReceiveIn + total.Key.ReceiptIn + total.Key.TransformIn) - (total.Key.MoveOrder + total.Key.Transformation + total.Key.Issue),
                                         CurrentStock = total.Key.SOH,
                                         PurchasedOrder = total.Key.ReceivePlus + total.Key.TransformPlus + total.Key.ReceiptPlus,
                                         OthersPlus = total.Key.MoveOrderPlus + total.Key.TransformOutPlus + total.Key.IssuePlus

                                     });

            return await movementInventory.ToListAsync();

        }

        public async Task<IReadOnlyList<GeneralLedgerReportDto>> GeneralLedgerReport(string DateFrom, string DateTo)
        {
            var moveOrderConsol = _context.Orders
                .AsNoTracking()
                .GroupJoin(_context.MoveOrders,
                o => o.OrderNo,
                mo => mo.OrderNo,
                (ord, moveOrd) => new { ord, moveOrd })
                .SelectMany(x => x.moveOrd.DefaultIfEmpty(), (x, moveOrder) => new { x.ord, moveOrder })
                .Where(x => x.moveOrder.IsTransact && x.moveOrder.IsActive)
                .Select(x => new GeneralLedgerReportDto
                {
                    SyncId = x.moveOrder.Id,
                    Transaction_Date = x.ord.PreparedDate,
                    Item_Code = x.moveOrder.ItemCode,
                    Description = x.moveOrder.ItemDescription,
                    Uom = x.moveOrder.Uom,
                    Category = x.moveOrder.Category,
                    Quantity = x.moveOrder.QuantityOrdered != null ? (decimal?)Math.Round(x.moveOrder.QuantityOrdered, 2) : null,
                    Unit_Price = "",
                    Line_Amount = "",
                    Po = "",
                    Service_Provider_Code = "",
                    Service_Provider = x.moveOrder.PreparedBy,
                    Reason = "",
                    Reference_No = Convert.ToString(x.ord.OrderNo),
                    Sub_Unit = "",
                    Supplier = x.moveOrder.FarmName,
                    Company_Code = x.moveOrder.FarmCode,
                    Company_Name = x.moveOrder.CompanyName,
                    Department_Code = x.moveOrder.DepartmentCode,
                    Department_Name = x.moveOrder.DepartmentName,
                    Location_Code = x.moveOrder.LocationCode,
                    Location = x.moveOrder.LocationName,
                    Account_Title_Code = "",
                    Account_Title_Name = "",
                    Asset = "",
                    Asset_Cip = "",
                    System = "ELIXIR_PHARMACY",
                }).ToList();

            var receiptConsol = _context.MiscellaneousReceipts
                .AsNoTracking()
                .GroupJoin(_context.WarehouseReceived, r => r.Id, w => w.MiscellaneousReceiptId, (receipt, warehouse) => new { receipt, warehouse })
                .SelectMany(x => x.warehouse.DefaultIfEmpty(), (x, warehouse) => new { x.receipt, warehouse })
                .Where(x => x.warehouse.IsActive && x.warehouse.TransactionType == "MiscellaneousReceipt")
                .Select(x => new GeneralLedgerReportDto
                {
                    SyncId = x.warehouse.Id,
                    Transaction_Date = x.receipt.PreparedDate,
                    Item_Code = x.warehouse.ItemCode,
                    Description = x.warehouse.ItemDescription,
                    Uom = x.warehouse.Uom,
                    Category = "",
                    Quantity = x.warehouse.ActualGood,
                    Unit_Price = "",
                    Line_Amount = "",
                    Po = "",
                    Service_Provider_Code = "",
                    Service_Provider = x.receipt.PreparedBy,
                    Reason = "",
                    Reference_No = Convert.ToString(x.receipt.Id),
                    Sub_Unit = "",
                    Supplier = x.receipt.Supplier,
                    Company_Code = x.receipt.SupplierCode,
                    Company_Name = "",
                    Department_Code = "",
                    Department_Name = "",
                    Location_Code = "",
                    Location = "",
                    Account_Title_Code = "",
                    Account_Title_Name = "",
                    Asset = "",
                    Asset_Cip = "",
                    System = "ELIXIR_PHARMACY",
                });

            var issueConsol = _context.MiscellaneousIssues
                .AsNoTracking()
                .GroupJoin(_context.MiscellaneousIssueDetails, mi => mi.Id, md => md.IssuePKey, (issue, detail) => new { issue, detail })
                .SelectMany(x => x.detail.DefaultIfEmpty(), (x, detail) => new { x.issue, detail })
                .Where(x => x.detail.IsActive && x.detail.IsTransact == true)
                .Select(x => new GeneralLedgerReportDto
                {
                    SyncId = x.detail.Id,
                    Transaction_Date = x.detail.PreparedDate,
                    Item_Code = x.detail.ItemCode,
                    Description = x.detail.ItemDescription,
                    Uom = x.detail.Uom,
                    Category = "",
                    Quantity = Math.Round(x.detail.Quantity, 2),
                    Unit_Price = "",
                    Line_Amount = "",
                    Po = "",
                    Service_Provider_Code = "",
                    Service_Provider = x.detail.PreparedBy,
                    Reason = "",
                    Reference_No = Convert.ToString(x.detail.Id),
                    Sub_Unit = "",
                    Supplier = "",
                    Company_Code = "",
                    Company_Name = "",
                    Department_Code = "",
                    Department_Name = "",
                    Location_Code = "",
                    Location = "",
                    Account_Title_Code = "",
                    Account_Title_Name = "",
                    Asset = "",
                    Asset_Cip = "",
                    System = "ELIXIR_PHARMACY",
                });

            var transformConsol = _context.Transformation_Preparation
                .AsNoTracking()
                .Where(t => t.IsActive && t.IsMixed)
                .Select(x => new GeneralLedgerReportDto
                {
                    SyncId = x.TransformId,
                    Transaction_Date = x.PreparedDate,
                    Item_Code = x.ItemCode,
                    Description = x.ItemDescription,
                    Uom = "KG",
                    Category = "",
                    Quantity = Math.Round(x.QuantityNeeded, 2),
                    Unit_Price = "",
                    Line_Amount = "",
                    Po = "",
                    Service_Provider_Code = "",
                    Service_Provider = x.PreparedBy,
                    Reason = "",
                    Reference_No = Convert.ToString(x.Id),
                    Sub_Unit = "",
                    Supplier = "",
                    Company_Code = "",
                    Company_Name = "",
                    Department_Code = "",
                    Department_Name = "",
                    Location_Code = "",
                    Location = "",
                    Account_Title_Code = "",
                    Account_Title_Name = "",
                    Asset = "",
                    Asset_Cip = "",
                    System = "ELIXIR_PHARMACY",
                });


            if (!string.IsNullOrEmpty(DateFrom) && !string.IsNullOrEmpty(DateTo))
            {
                var dateFrom = DateTime.Parse(DateFrom).Date;
                var dateTo = DateTime.Parse(DateTo).Date;

                moveOrderConsol = moveOrderConsol
                    .Where(mo => mo.Transaction_Date >= dateFrom &&
                        mo.Transaction_Date < dateTo)
                    .ToList();

                receiptConsol = receiptConsol
                    .Where(rc => rc.Transaction_Date >= dateFrom &&
                        rc.Transaction_Date < dateTo);

                issueConsol = issueConsol
                    .Where(ic => ic.Transaction_Date >= dateFrom &&
                        ic.Transaction_Date < dateTo);

                transformConsol = transformConsol
                    .Where(tf => tf.Transaction_Date >= dateFrom &&
                        tf.Transaction_Date < dateTo);
            }
            //else
            //{
            //    moveOrderConsol = moveOrderConsol.Where(x => x.SyncId == null).ToList();
            //}

            var consolidateList = moveOrderConsol
                .Concat(await receiptConsol.ToListAsync())
                .Concat(await issueConsol.ToListAsync())
                .Concat(await transformConsol.ToListAsync());

            return consolidateList.ToList();
        }

        public async Task<IReadOnlyList<ConsolidateFinanceReportDto>> ConsolidateFinanceReport(string DateFrom, string DateTo, string Search)
        {
            var receivingConsol = _context.WarehouseReceived
                .AsNoTracking()
                .Where(wr => wr.TransactionType == "Receiving" && wr.IsActive)
                .Select(x => new ConsolidateFinanceReportDto
                {
                    Id = x.Id,
                    TransactionDate = x.ReceivingDate.Date,
                    ItemCode = x.ItemCode,
                    ItemDescription = x.ItemDescription,
                    Uom = x.Uom,
                    Category = "",
                    Quantity = x.ActualGood,
                    UnitCost = 0,
                    LineAmount = 0,
                    Source = x.PO_Number.ToString(),
                    TransactionType = "Receiving",
                    Reason = "",
                    Reference = "",
                    SupplierName = x.Supplier,
                    EncodedBy = x.ReceivedBy,
                    CompanyCode = "10",
                    CompanyName = "RDF Corporate Services",
                    DepartmentCode = "0010",
                    DepartmentName = "Corporate Common",
                    LocationCode = "0001",
                    LocationName = "Head Office",
                    AccountTitleCode = "117701",
                    AccountTitle = "Materials & Supplies Inventory",
                    EmpId = "",
                    Fullname = "",
                    AssetTag = "",
                    CIPNo = "",
                    Helpdesk = "",
                    Rush = ""
                }).ToList();

            var moveOrderConsol = from m in _context.MoveOrders
                                  join t in _context.TransactMoveOrder
                                  on m.OrderNo equals t.OrderNo
                                  join w in _context.WarehouseReceived
                                  on m.WarehouseId equals w.Id
                                  join u in _context.Users on t.PreparedBy equals u.FullName
                                  join p in _context.POSummary on w.PO_Number equals p.PO_Number



                                  where  m.IsTransact == true
                                  select new ConsolidateFinanceReportDto
                                  {
                                      Id = t.Id,
                                      TransactionDate = t.PreparedDate,
                                      ItemCode = m.ItemCode,
                                      ItemDescription = m.ItemDescription,
                                      Uom = m.Uom,
                                      Category = m.Category,
                                      Quantity = Math.Round(m.QuantityOrdered, 2),
                                      UnitCost = p.UnitPrice,
                                      LineAmount = Math.Round(m.QuantityOrdered * p.UnitPrice, 2),
                                      Source = Convert.ToString(t.OrderNo),
                                      TransactionType = "Move Order",
                                      Reason = "",
                                      Reference = m.OrderNo.ToString(),
                                      SupplierName = "",
                                      EncodedBy = t.PreparedBy,
                                      CompanyCode = m.CompanyCode,
                                      CompanyName = m.CompanyName,
                                      DepartmentCode = m.DepartmentCode,
                                      DepartmentName = m.DepartmentName,
                                      LocationCode = m.LocationCode,
                                      LocationName = m.LocationName,
                                      AccountTitle = "Inventory Transfer",
                                      AccountTitleCode = "115999",
                                      EmpId = "",
                                      Fullname = "",
                                      AssetTag = "",
                                      CIPNo = "",
                                      Helpdesk = "",
                                      Rush = "",
                                  };



            var receiptConsol = _context.MiscellaneousReceipts
                .AsNoTracking()
                .GroupJoin(_context.WarehouseReceived, r => r.Id, w => w.MiscellaneousReceiptId,
                    (receipt, warehouse) => new { receipt, warehouse })
                .SelectMany(x => x.warehouse.DefaultIfEmpty(), (x, warehouse) => new { x.receipt, warehouse })
                .Where(x => x.warehouse.IsActive && x.warehouse.TransactionType == "MiscellaneousReceipt")
                .Select(x => new ConsolidateFinanceReportDto
                {
                    Id = x.warehouse.Id,
                    TransactionDate = x.receipt.TransactionDate ,
                    ItemCode = x.warehouse.ItemCode,
                    ItemDescription = x.warehouse.ItemDescription,
                    Uom = x.warehouse.Uom,
                    Category = "",
                    Quantity = x.warehouse.ActualGood,
                    UnitCost = 0,
                    LineAmount = 0,
                    Source = Convert.ToString(x.receipt.Id),
                    TransactionType = "Miscellaneous Receipt",
                    Reason = x.receipt.Remarks,
                    Reference = x.receipt.Details,
                    SupplierName = x.receipt.Supplier,
                    EncodedBy = x.receipt.PreparedBy,
                    CompanyCode = "",
                    CompanyName =  "",
                    DepartmentCode = "",
                    DepartmentName = "",
                    LocationCode = "",
                    LocationName = "",
                    AccountTitle = "Inventory Transfer",
                    AccountTitleCode = "115999",
                    EmpId = "",
                    Fullname = x.warehouse.ReceivedBy,
                    AssetTag = "",
                    CIPNo = "",
                    Helpdesk = "",
                    //Remarks = x.receipt.Remarks,
                    Rush = ""
                });

            var issueConsol = _context.MiscellaneousIssues
                .AsNoTracking()
                .GroupJoin(
                    _context.MiscellaneousIssueDetails,
                    miscDetail => miscDetail.Id,
                    issueDetail => issueDetail.IssuePKey,
                    (miscDetail, issueDetails) => new { miscDetail, issueDetails }
                )
                .SelectMany(
                    x => x.issueDetails.DefaultIfEmpty(),
                    (x, issueDetail) => new { x.miscDetail, issueDetail }
                )
                .Where(x => x.issueDetail == null || x.issueDetail.IsActive)
                .Select(x => new ConsolidateFinanceReportDto
                {
                    Id = x.issueDetail != null ? x.issueDetail.Id : 0,
                    TransactionDate = x.miscDetail.TransactionDate ?? default(DateTime),
                    ItemCode = x.issueDetail != null ? x.issueDetail.ItemCode : "",
                    ItemDescription = x.issueDetail != null ? x.issueDetail.ItemDescription : "",
                    Uom = x.issueDetail != null ? x.issueDetail.Uom : "",
                    Category = "",
                    Quantity = x.issueDetail != null ? Math.Round(x.issueDetail.Quantity, 2) : 0,
                    UnitCost = 0,
                    LineAmount = 0,
                    Source = Convert.ToString(x.miscDetail.Id),
                    TransactionType = "Miscellaneous Issue",
                    Reason = x.issueDetail != null ? x.issueDetail.Remarks : "",
                    Reference = x.miscDetail.Details,
                    SupplierName = "",
                    EncodedBy = x.issueDetail != null ? x.issueDetail.PreparedBy : "",
                    CompanyCode = "",
                    CompanyName = "",
                    DepartmentCode = "",
                    DepartmentName = "",
                    LocationCode = "",
                    LocationName = "",
                    AccountTitle = "Inventory Transfer",
                    AccountTitleCode = "115999",
                    EmpId = "",
                    Fullname = x.issueDetail != null ? x.issueDetail.PreparedBy : "",
                    AssetTag = "",
                    CIPNo = "",
                    Helpdesk = "",
                    Rush = ""
                });


            var transformConsol = _context.Transformation_Preparation
                .AsNoTracking()
                .GroupJoin(_context.WarehouseReceived, m => m.WarehouseId, w => w.Id, (m, w) => new { m, w })
                .SelectMany(
                    x => x.w.DefaultIfEmpty(),
                    (x, w) => new { m = x.m, w })
                .Join(_context.POSummary, x => x.w.PO_Number, po => po.PO_Number, (p, po) => new { p.m, p.w, po })
                .Where(t => t.m.IsActive)
                .Select(x => new ConsolidateFinanceReportDto
                {
                    Id = x.m.Id,
                    TransactionDate = x.m.PreparedDate,
                    ItemCode = x.m.ItemCode,
                    ItemDescription = x.m.ItemDescription,
                    Uom = "KG",
                    Category = "",
                    Quantity = Math.Round(x.m.QuantityNeeded, 2),
                    UnitCost = x.po.UnitPrice,
                    LineAmount = x.po.UnitPrice * (Math.Round(x.m.QuantityNeeded, 2)),
                    Source = "",
                    TransactionType = "Transformation",
                    Reason = "",
                    Reference = "",
                    SupplierName = "",
                    EncodedBy = x.m.PreparedBy,
                    CompanyCode = "",
                    CompanyName = "",
                    DepartmentCode = "",
                    DepartmentName = "",
                    LocationCode = "",
                    LocationName = "",
                    AccountTitleCode = "115998",
                    AccountTitle = "Materials & Supplies Inventory",
                    EmpId = "",
                    Fullname = "",
                    AssetTag = "",
                    CIPNo = "",
                    Helpdesk = "",
                    Rush = ""
                });


            if (!string.IsNullOrEmpty(DateFrom) && !string.IsNullOrEmpty(DateTo))
            {
                var dateFrom = DateTime.Parse(DateFrom).Date;
                var dateTo = DateTime.Parse(DateTo).Date;

                receivingConsol = receivingConsol
                    .Where(x => x.TransactionDate >= dateFrom && x.TransactionDate <= dateTo)
                    .ToList()
                    ;

                moveOrderConsol = moveOrderConsol
                    .Where(x => x.TransactionDate >= dateFrom && x.TransactionDate <= dateTo)
                    ;

                receiptConsol = receiptConsol
                    .Where(x => x.TransactionDate >= dateFrom && x.TransactionDate <= dateTo)
                    ;

                issueConsol = issueConsol
                     .Where(x => x.TransactionDate >= dateFrom && x.TransactionDate <= dateTo)
                    ;

                transformConsol = transformConsol
                    .Where(x => x.TransactionDate >= dateFrom && x.TransactionDate <= dateTo)
                    ;

            }

            var consolidateList = receivingConsol
                .Concat(await moveOrderConsol.ToListAsync())
                .Concat(await receiptConsol.ToListAsync())
                .Concat(await issueConsol.ToListAsync())
                .Concat(await transformConsol.ToListAsync());

            var reports = consolidateList.Select(consol => new ConsolidateFinanceReportDto
            {
                Id = consol.Id,
                TransactionDate = consol.TransactionDate,
                ItemCode = consol.ItemCode,
                ItemDescription = consol.ItemDescription,
                Uom = consol.Uom,
                Category = consol.Category,
                Quantity = consol.Quantity,
                UnitCost = consol.UnitCost,
                LineAmount = consol.LineAmount,
                Source = consol.Source,
                TransactionType = consol.TransactionType,
                Reason = consol.Reason,
                Reference = consol.Reference,
                SupplierName = consol.SupplierName,
                EncodedBy = consol.EncodedBy,
                CompanyCode = consol.CompanyCode,
                CompanyName = consol.CompanyName,
                DepartmentCode = consol.DepartmentCode,
                DepartmentName = consol.DepartmentName,
                LocationCode = consol.LocationCode,
                LocationName = consol.LocationName,
                AccountTitleCode = consol.AccountTitleCode,
                AccountTitle = consol.AccountTitle,
                EmpId = consol.EmpId,
                Fullname = consol.Fullname,
                AssetTag = consol.AssetTag,
                CIPNo = consol.CIPNo,
                Helpdesk = consol.Helpdesk,
                Rush = consol.Rush
            });

            if (!string.IsNullOrEmpty(Search))
            {
                reports = reports.Where(x => x.ItemCode.ToLower().Contains(Search.ToLower())
                || x.ItemDescription.ToLower().Contains(Search.ToLower())
                || x.Source.ToString().Contains(Search)
                || x.TransactionType.ToLower().Contains(Search.ToLower()))
                      ;
            }

            reports = reports
                .OrderBy(x => x.TransactionDate)
                .ThenBy(x => x.ItemCode);


            return reports.ToList();

        }

        public async Task<IReadOnlyList<ConsolidateAuditReportDto>> ConsolidateAuditReport(string DateFrom, string DateTo, string Search)
        {
            var dateFrom = DateTime.Parse(DateFrom).Date;
            var dateTo = DateTime.Parse(DateTo).Date;

            var receivingConsol = _context.WarehouseReceived
                .Where(x => x.TransactionType == "Receiving" && x.IsActive == true)
                .Where(x => x.ReceivingDate.Date >= dateFrom && x.ReceivingDate.Date <= dateTo)
                .Select(x => new ConsolidateAuditReportDto
                {
                    Id = x.Id,
                    TransactionDate = x.ReceivingDate.Date.ToString(),
                    ItemCode = x.ItemCode,
                    ItemDescription = x.ItemDescription,
                    Uom = x.Uom,
                    Category = "",
                    Quantity = x.ActualGood,
                    UnitCost = "",
                    LineAmount = "",
                    Source = x.PO_Number.ToString(),
                    TransactionType = "Receiving",
                    Status = "",
                    Reason = "",
                    Reference = "",
                    SupplierName = x.Supplier,
                    EncodedBy = x.ReceivedBy,
                    CompanyCode = "10",
                    CompanyName = "RDF Corporate Services",
                    DepartmentCode = "0010",
                    DepartmentName = "Corporate Common",
                    LocationCode = "0001",
                    LocationName = "Head Office",
                    AccountTitleCode = "117701",
                    AccountTitle = "Materials & Supplies Inventory",
                    EmpId = "",
                    Fullname = "",
                    AssetTag = "",
                    CIPNo = "",
                    Helpdesk = 0,
                    Rush = ""

                }).ToList();


            var moveOrderConsol = _context.TransactMoveOrder
                .GroupJoin(
                    _context.MoveOrders,
                    transact => transact.OrderNo,
                    moveOrder => moveOrder.OrderNo,
                    (transact, moveOrders) => new { transact, moveOrders }
                )
                .SelectMany(
                    x => x.moveOrders.DefaultIfEmpty(),
                    (x, moveOrder) => new { x.transact, moveOrder }
                )
                .Where(x => x.transact.IsTransact == true && x.transact.IsActive == true && (x.moveOrder == null || x.moveOrder.IsActive == true))
                .Where(x => x.transact.DeliveryDate.Value >= dateFrom.Date && x.transact.DeliveryDate.Value <= dateTo)
                .Select(x => new ConsolidateAuditReportDto
                {
                    Id = x.transact.Id,
                    TransactionDate = x.transact.PreparedDate.Value.Date.ToString(),
                    ItemCode = x.moveOrder.ItemCode,
                    ItemDescription = x.moveOrder.ItemDescription,
                    Uom = x.moveOrder.Uom,
                    Category = x.moveOrder.Category,
                    Quantity = x.moveOrder != null ? Math.Round(x.moveOrder.QuantityOrdered, 2) : 0,
                    UnitCost = "0",
                    LineAmount = "0",
                    Source = x.transact.OrderNo.ToString(),
                    TransactionType = "Move Order",
                    Status = "Transacted",
                    Reason = string.Empty,
                    Reference = x.moveOrder.Remarks,
                    SupplierName = string.Empty,
                    EncodedBy = x.transact.PreparedBy,
                    CompanyCode = x.moveOrder.CompanyCode,
                    CompanyName = x.moveOrder.CompanyName,
                    DepartmentCode = x.moveOrder.DepartmentCode,
                    DepartmentName = x.moveOrder.DepartmentName,
                    LocationCode = x.moveOrder.LocationCode,
                    LocationName = x.moveOrder.LocationName,
                    AccountTitleCode = x.moveOrder.AccountCode,
                    AccountTitle = "",
                    EmpId = "",
                    Fullname = x.moveOrder.PreparedBy,
                    AssetTag = "",
                    CIPNo = "",
                    Helpdesk = 0,
                    Rush = ""
                }).ToList();


            var receiptConsol = _context.MiscellaneousReceipts
                .GroupJoin(_context.WarehouseReceived, receipt => receipt.Id, warehouse => warehouse.MiscellaneousReceiptId, (receipt, warehouse) => new { receipt, warehouse })
                .SelectMany(x => x.warehouse.DefaultIfEmpty(), (x, warehouse) => new { x.receipt, warehouse })
                .Where(x => x.receipt.TransactionDate.Value >= dateFrom && x.receipt.TransactionDate.Value <= dateTo)
                .Where(x => x.warehouse.IsActive == true && x.warehouse.TransactionType == "MiscellaneousReceipt")
                .Select(x => new ConsolidateAuditReportDto
                {
                    Id = x.warehouse.Id,
                    TransactionDate = x.receipt.TransactionDate.Value.ToString(),
                    ItemCode = x.warehouse.ItemCode,
                    ItemDescription = x.warehouse.ItemDescription,
                    Uom = x.warehouse.Uom,
                    Category = "",
                    Quantity = x.warehouse.ActualGood,
                    UnitCost = "",
                    LineAmount = "",
                    Source = x.receipt.Id.ToString(),
                    TransactionType = "Miscellaneous Receipt",
                    Status = "",
                    Reason = x.receipt.Remarks,
                    Reference = x.receipt.Details,
                    SupplierName = x.receipt.Supplier,
                    EncodedBy = x.receipt.PreparedBy,
                    CompanyCode = "",
                    CompanyName = "",
                    DepartmentCode = "",
                    DepartmentName = "",
                    LocationCode = "",
                    LocationName = "",
                    AccountTitleCode = "",
                    AccountTitle = "",
                    EmpId = "",
                    Fullname = "",
                    AssetTag = "",
                    CIPNo = "",
                    Helpdesk = 0,
                    //Remarks = x.receipt.Remarks,
                    Rush = ""
                }).ToList();


            var issueConsol = _context.MiscellaneousIssues
                .GroupJoin(
                    _context.MiscellaneousIssueDetails,
                    miscDetail => miscDetail.Id,
                    issue => issue.IssuePKey,
                    (miscDetail, issues) => new { miscDetail, issues }
                )
                .SelectMany(
                    x => x.issues.DefaultIfEmpty(),
                    (x, issue) => new { miscDetail = x.miscDetail, issue }
                )
                .Where(x => x.issue == null || x.issue.IsActive == true)
                .Where(x => x.miscDetail.TransactionDate.Value >= dateFrom && x.miscDetail.TransactionDate.Value <= dateTo)
                .Select(x => new ConsolidateAuditReportDto
                {
                    Id = x.issue.Id,
                    TransactionDate = x.miscDetail.TransactionDate.Value.ToString(),
                    ItemCode = x.issue.ItemCode,
                    ItemDescription = x.issue.ItemDescription,
                    Uom = x.issue.Uom,
                    Category = "",
                    Quantity = -Math.Round(x.issue.Quantity, 2) ,
                    UnitCost = "",
                    LineAmount = "",
                    Source = x.miscDetail.Id.ToString(),
                    TransactionType = "Miscellaneous Issue",
                    Status = "",
                    Reason = x.issue.Remarks,
                    Reference = x.miscDetail.Details,
                    SupplierName = "",
                    EncodedBy = x.issue.PreparedBy,
                    CompanyCode = "",
                    CompanyName = "",
                    DepartmentCode = "",
                    DepartmentName = "",
                    LocationCode = "",
                    LocationName = "",
                    AccountTitleCode = "",
                    AccountTitle = "",
                    EmpId = "",
                    Fullname = x.issue.PreparedBy,
                    AssetTag = "",
                    CIPNo = "",
                    Helpdesk = 0,
                    Rush = ""
                })
                .ToList();

            var transformConsol = _context.Transformation_Preparation
                .AsNoTracking()
                .Where(t => t.IsActive)
                .Select(x => new ConsolidateAuditReportDto
                {
                    Id = x.Id,
                    TransactionDate = x.PreparedDate.Date.ToString(),
                    ItemCode = x.ItemCode,
                    ItemDescription = x.ItemDescription,
                    Uom = "KG",
                    Category = "",
                    Quantity = -Math.Round(x.QuantityNeeded, 2),
                    UnitCost = "",
                    LineAmount = "",
                    Source = x.Id.ToString(),
                    TransactionType = "",
                    Status = "",
                    Reason = "",
                    Reference = "",
                    SupplierName = "",
                    EncodedBy = x.PreparedBy,
                    CompanyCode = "",
                    CompanyName = "",
                    DepartmentCode = "",
                    DepartmentName = "",
                    LocationCode = "",
                    LocationName = "",
                    AccountTitleCode = "",
                    AccountTitle = "",
                    EmpId = "",
                    Fullname = x.PreparedBy,
                    AssetTag = "",
                    CIPNo = "",
                    Helpdesk = 0,
                    Rush = ""
                });


            var consolidateList = receivingConsol
                .Concat(moveOrderConsol)
                .Concat(receiptConsol)
                .Concat(issueConsol)
                .Concat(transformConsol)
                .ToList();

            var reports = consolidateList.Select(consol => new ConsolidateAuditReportDto
            {
                Id = consol.Id,
                TransactionDate = consol.TransactionDate,
                ItemCode = consol.ItemCode,
                ItemDescription = consol.ItemDescription,
                Uom = consol.Uom,
                Category = consol.Category,
                Quantity = consol.Quantity,
                UnitCost = consol.UnitCost,
                LineAmount = consol.LineAmount,
                Source = consol.Source,
                TransactionType = consol.TransactionType,
                Reason = consol.Reason,
                Reference = consol.Reference,
                SupplierName = consol.SupplierName,
                EncodedBy = consol.EncodedBy,
                CompanyCode = consol.CompanyCode,
                CompanyName = consol.CompanyName,
                DepartmentCode = consol.DepartmentCode,
                DepartmentName = consol.DepartmentName,
                LocationCode = consol.LocationCode,
                LocationName = consol.LocationName,
                AccountTitleCode = consol.AccountTitleCode,
                AccountTitle = consol.AccountTitle,
                EmpId = consol.EmpId,
                Fullname = consol.Fullname,
                AssetTag = consol.AssetTag,
                CIPNo = consol.CIPNo,
                Helpdesk = consol.Helpdesk,
                Rush = consol.Rush
            });

            if (!string.IsNullOrEmpty(Search))
            {
                reports = reports.Where(x => x.ItemCode.ToLower().Contains(Search.ToLower())
                || x.ItemDescription.ToLower().Contains(Search.ToLower())
                || x.Source.ToString().Contains(Search)
                || x.TransactionType.ToLower().Contains(Search.ToLower())
                || x.Status.ToLower().Contains(Search.ToLower()))
                    .ToList();
            }

            reports = reports
                .OrderBy(x => x.TransactionDate)
                .ToList();


            return reports.ToList();

        }

        public async Task<IReadOnlyList<InventoryMovementReport>> ConsolidateInventoryMovementReport(string DateFrom, string DateTo, string PlusOne)
        {
            //var dateToday = DateTime.Now.ToString("MM/dd/yyyy");
            DateTime dateFromParsed = DateTime.Parse(DateFrom);
            DateTime dateToParsed = DateTime.Parse(DateTo);
            DateTime plusOneParsed = DateTime.Parse(PlusOne);
            DateTime dateTodayParsed = DateTime.Today;

            var getWarehouseStock = _context.WarehouseReceived.Where(x => x.IsActive == true)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new WarehouseInventory
                {
                    ItemCode = x.Key.ItemCode,
                    ActualGood = x.Sum(x => x.ActualGood)
                });

            var getMoveOrderOutByDate = _context.MoveOrders.Where(x => x.IsActive == true)
                .Where(x => x.IsPrepared == true)
                .Where(x => x.PreparedDate.Value.Date >= dateFromParsed && x.PreparedDate.Value.Date <= dateToParsed &&
                            x.ApprovedDate != null)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new MoveOrderInventory
                {
                    ItemCode = x.Key.ItemCode,
                    QuantityOrdered = x.Sum(x => x.QuantityOrdered)
                });

            var getMoveOrderOutByDatePlus = _context.MoveOrders.Where(x => x.IsActive == true)
                .Where(x => x.IsPrepared == true)
                .Where(x => x.PreparedDate.Value.Date >= plusOneParsed && x.PreparedDate.Value.Date <= dateTodayParsed &&
                            x.ApprovedDate != null)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new MoveOrderInventory
                {
                    ItemCode = x.Key.ItemCode,
                    QuantityOrdered = x.Sum(x => x.QuantityOrdered)
                });

            var getTransformationByDate = _context.Transformation_Preparation.Where(x => x.IsActive == true)
                .Where(x => x.PreparedDate >= dateFromParsed && x.PreparedDate <= dateToParsed)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new TransformationInventory
                {
                    ItemCode = x.Key.ItemCode,
                    WeighingScale = x.Sum(x => x.WeighingScale)
                });

            var getTransformationByDatePlus = _context.Transformation_Preparation.Where(x => x.IsActive == true)
                .Where(x => x.PreparedDate >= plusOneParsed && x.PreparedDate <= dateTodayParsed)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new TransformationInventory
                {
                    ItemCode = x.Key.ItemCode,
                    WeighingScale = x.Sum(x => x.WeighingScale)
                });

            var getIssueOutByDate = _context.MiscellaneousIssueDetails.Where(x => x.IsActive == true)
                .Where(x => x.IsTransact == true)
                .Where(x => x.PreparedDate.Date >= dateFromParsed && x.PreparedDate.Date <= dateToParsed)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new IssueInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.Quantity)
                });

            var getIssueOutByDatePlus = _context.MiscellaneousIssueDetails.Where(x => x.IsActive == true)
                .Where(x => x.IsTransact == true)
                .Where(x => x.PreparedDate.Date >= plusOneParsed && x.PreparedDate.Date <= dateTodayParsed)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new IssueInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.Quantity)
                });

            var getReceivetIn = _context.WarehouseReceived.Where(x => x.IsActive == true)
                .Where(x => x.TransactionType == "Receiving")
                .Where(x => x.ReceivingDate.Date >= dateFromParsed && x.ReceivingDate.Date <= dateToParsed)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new ReceiptInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.ActualGood)
                });

            var getReceivetInPlus = _context.WarehouseReceived.Where(x => x.IsActive == true)
                .Where(x => x.TransactionType == "Receiving")
                .Where(x => x.ReceivingDate.Date >= plusOneParsed && x.ReceivingDate.Date <= dateTodayParsed)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new ReceiptInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.ActualGood)
                });

            var getTransformIn = _context.WarehouseReceived.Where(x => x.IsActive == true)
                .Where(x => x.TransactionType == "Transformation")
                .Where(x => x.ReceivingDate.Date >= dateFromParsed && x.ReceivingDate.Date <= dateToParsed)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new ReceiptInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.ActualGood)
                });

            var getTransformInPlus = _context.WarehouseReceived.Where(x => x.IsActive == true)
                .Where(x => x.TransactionType == "Transformation")
                .Where(x => x.ReceivingDate.Date >= plusOneParsed && x.ReceivingDate.Date <= dateTodayParsed)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new ReceiptInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.ActualGood)
                });

            var getReceiptIn = _context.WarehouseReceived.Where(x => x.IsActive == true)
                .Where(x => x.TransactionType == "MiscellaneousReceipt")
                .Where(x => x.ReceivingDate.Date >= dateFromParsed && x.ReceivingDate.Date <= dateToParsed)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new ReceiptInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.ActualGood)
                });

            var getReceiptInPlus = _context.WarehouseReceived.Where(x => x.IsActive == true)
                .Where(x => x.TransactionType == "MiscellaneousReceipt")
                .Where(x => x.ReceivingDate.Date >= plusOneParsed && x.ReceivingDate.Date <= dateTodayParsed)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new ReceiptInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.ActualGood)
                });

            var getMoveOrderOut = _context.MoveOrders.Where(x => x.IsActive == true)
                .Where(x => x.IsPrepared == true)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new MoveOrderInventory
                {
                    ItemCode = x.Key.ItemCode,
                    QuantityOrdered = x.Sum(x => x.QuantityOrdered)
                });

            var getTransformation = _context.Transformation_Preparation.Where(x => x.IsActive == true)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new TransformationInventory
                {
                    ItemCode = x.Key.ItemCode,
                    WeighingScale = x.Sum(x => x.WeighingScale)
                });

            var getIssueOut = _context.MiscellaneousIssueDetails.Where(x => x.IsActive == true)
                .Where(x => x.IsTransact == true)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new IssueInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.Quantity)
                });

            var getSOH = (from warehouse in getWarehouseStock
                          join preparation in getTransformation
                              on warehouse.ItemCode equals preparation.ItemCode
                              into leftJ1
                          from preparation in leftJ1.DefaultIfEmpty()
                          join issue in getIssueOut
                              on warehouse.ItemCode equals issue.ItemCode
                              into leftJ2
                          from issue in leftJ2.DefaultIfEmpty()
                          join moveorder in getMoveOrderOut
                              on warehouse.ItemCode equals moveorder.ItemCode
                              into leftJ3
                          from moveorder in leftJ3.DefaultIfEmpty()
                          join receipt in getReceiptIn
                              on warehouse.ItemCode equals receipt.ItemCode
                              into leftJ4
                          from receipt in leftJ4.DefaultIfEmpty()
                          group new
                          {
                              warehouse,
                              preparation,
                              moveorder,
                              receipt,
                              issue
                          }
                              by new
                              {
                                  warehouse.ItemCode
                              }
                into total
                          select new SOHInventory
                          {
                              ItemCode = total.Key.ItemCode,
                              SOH = total.Sum(x => x.warehouse.ActualGood == 0 ? 0 : x.warehouse.ActualGood) -
                                    total.Sum(x => x.preparation.WeighingScale == 0 ? 0 : x.preparation.WeighingScale) -
                                    total.Sum(x => x.moveorder.QuantityOrdered == 0 ? 0 : x.moveorder.QuantityOrdered)
                          });

            // var getWarehouseStockById = _context.WarehouseReceived
            //     .Where(x => x.UnitCost > 0)
            //     .Select(x => new WarehouseInventory
            //     {
            //         WarehouseId = x.Id,
            //         ItemCode = x.ItemCode,
            //         UnitCost = x.UnitCost,
            //         ActualGood = x.ActualGood
            //     });

            var getMoveOrderOutid = _context.MoveOrders
                .Where(x => x.PreparedDate.HasValue && x.PreparedDate.Value.Date >= dateFromParsed && x.PreparedDate.Value.Date <= dateToParsed &&
                            x.ApprovedDate != null && x.IsActive && x.IsPrepared)
                .GroupBy(x => new
                {
                    x.WarehouseId,
                    x.ItemCode,
                })
                .Select(x => new MoveOrderInventory
                {
                    WarehouseId = x.Key.WarehouseId,
                    ItemCode = x.Key.ItemCode,
                    QuantityOrdered = x.Sum(y => y.QuantityOrdered),
                });

            var getIssueOutId = _context.MiscellaneousIssueDetails
                .Where(x => x.IsActive)
                .Where(x => x.PreparedDate != null && x.PreparedDate.Date >= dateFromParsed && x.PreparedDate.Date <= dateToParsed)
                .GroupBy(x => new
                {
                    x.WarehouseId,
                    x.ItemCode,
                })
                .Select(x => new
                {
                    WarehouseId = x.Key.WarehouseId,
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(y => y.Quantity),
                });

            // var getUnitPrice = (from warehouse in getWarehouseStockById
            //                     join moveorder in getMoveOrderOutid
            //                     on warehouse.WarehouseId equals moveorder.WarehouseId into leftJ1
            //                     from moveorder in leftJ1.DefaultIfEmpty()

            //                     join issue in getIssueOutId
            //                     on warehouse.WarehouseId equals issue.WarehouseId into leftJ2
            //                     from issue in leftJ2.DefaultIfEmpty()

            //                     group new
            //                     {
            //                         warehouse,
            //                         moveorder,
            //                         issue,
            //                     } by new
            //                     {
            //                         warehouse.WarehouseId,
            //                         warehouse.ItemCode,
            //                     } into x
            //                     select new UnitCostDTO
            //                     {
            //                         WarehouseId = x.Key.WarehouseId,
            //                         ItemCode = x.Key.ItemCode,
            //                         UnitCost = Math.Round((x.First().warehouse.UnitCost ?? 0m) * (x.First().warehouse.ActualGood != null ? x.First().warehouse.ActualGood : 0) - (x.First().moveorder.QuantityOrdered != null ? x.First().moveorder.QuantityOrdered : 0) - (x.First().issue.Quantity != null ? x.First().issue.Quantity : 0), 2),
            //                         ActualGood = (x.First().warehouse.ActualGood != null ? x.First().warehouse.ActualGood : 0) + ((x.First().moveorder.QuantityOrdered != null ? x.First().moveorder.QuantityOrdered : 0) - (x.First().issue.Quantity != null ? x.First().issue.Quantity : 0))
            //                     });

            // var getUnitpriceTotal = getUnitPrice
            //         .Where(x => x.UnitCost > 0)
            //         .GroupBy(x => new
            //         {
            //             x.ItemCode,
            //         })
            //         .Select(x => new UnitCostDTO
            //         {
            //             ItemCode = x.Key.ItemCode,
            //             UnitCost = Math.Round(x.Sum(y => y.UnitCost) / x.Sum(y => y.ActualGood), 2),
            //             ActualGood = x.Sum(y => y.ActualGood),
            //             TotalUnitPrice = x.Sum(y => y.UnitCost),
            //         });

            var movementInventory = (from rawmaterial in _context.RawMaterials
                                     join moveorder in getMoveOrderOutByDate
                                         on rawmaterial.ItemCode equals moveorder.ItemCode
                                         into leftJ
                                     from moveorder in leftJ.DefaultIfEmpty()
                                     join transformation in getTransformationByDate
                                         on rawmaterial.ItemCode equals transformation.ItemCode
                                         into leftJ2
                                     from transformation in leftJ2.DefaultIfEmpty()
                                     join issue in getIssueOutByDate
                                         on rawmaterial.ItemCode equals issue.ItemCode
                                         into leftJ3
                                     from issue in leftJ3.DefaultIfEmpty()
                                     join receive in getReceivetIn
                                         on rawmaterial.ItemCode equals receive.ItemCode
                                         into leftJ4
                                     from receive in leftJ4.DefaultIfEmpty()
                                     join transformIn in getTransformIn
                                         on rawmaterial.ItemCode equals transformIn.ItemCode
                                         into leftJ5
                                     from transformIn in leftJ5.DefaultIfEmpty()
                                     join receipt in getReceiptIn
                                         on rawmaterial.ItemCode equals receipt.ItemCode
                                         into leftJ6
                                     from receipt in leftJ6.DefaultIfEmpty()
                                     join SOH in getSOH
                                         on rawmaterial.ItemCode equals SOH.ItemCode
                                         into leftJ7
                                     from SOH in leftJ7.DefaultIfEmpty()
                                     join receivePlus in getReceivetInPlus
                                         on rawmaterial.ItemCode equals receivePlus.ItemCode
                                         into leftJ8
                                     from receivePlus in leftJ8.DefaultIfEmpty()
                                     join transformPlus in getTransformInPlus
                                         on rawmaterial.ItemCode equals transformPlus.ItemCode
                                         into leftJ9
                                     from transformPlus in leftJ9.DefaultIfEmpty()
                                     join receiptPlus in getReceiptInPlus
                                         on rawmaterial.ItemCode equals receiptPlus.ItemCode
                                         into leftJ10
                                     from receiptPlus in leftJ10.DefaultIfEmpty()
                                     join moveorderPlus in getMoveOrderOutByDatePlus
                                         on rawmaterial.ItemCode equals moveorderPlus.ItemCode
                                         into leftJ11
                                     from moveorderPlus in leftJ11.DefaultIfEmpty()
                                     join transformoutPlus in getTransformationByDatePlus
                                         on rawmaterial.ItemCode equals transformoutPlus.ItemCode
                                         into leftJ12
                                     from transformoutPlus in leftJ12.DefaultIfEmpty()
                                     join issuePlus in getIssueOutByDatePlus
                                         on rawmaterial.ItemCode equals issuePlus.ItemCode
                                         into leftJ13
                                     from issuePlus in leftJ13.DefaultIfEmpty()
                                         // join auc in getUnitpriceTotal
                                         //    on rawmaterial.ItemCode equals auc.ItemCode
                                     group new
                                     {
                                         rawmaterial,
                                         moveorder,
                                         transformation,
                                         issue,
                                         receive,
                                         transformIn,
                                         receipt,
                                         SOH,
                                         receivePlus,
                                         transformPlus,
                                         receiptPlus,
                                         moveorderPlus,
                                         transformoutPlus,
                                         issuePlus,
                                         // auc
                                     }
                                         by new
                                         {
                                             rawmaterial.ItemCode,
                                             rawmaterial.ItemDescription,
                                             rawmaterial.ItemCategory.ItemCategoryName,
                                             MoveOrder = moveorder.QuantityOrdered != null ? moveorder.QuantityOrdered : 0,
                                             Transformation = transformation.WeighingScale != null ? transformation.WeighingScale : 0,
                                             Issue = issue.Quantity != null ? issue.Quantity : 0,
                                             ReceiptIn = receipt.Quantity != null ? receipt.Quantity : 0,
                                             ReceiveIn = receive.Quantity != null ? receive.Quantity : 0,
                                             TransformIn = transformIn.Quantity != null ? transformIn.Quantity : 0,
                                             SOH = SOH.SOH != null ? SOH.SOH : 0,
                                             ReceivePlus = receivePlus.Quantity != null ? receivePlus.Quantity : 0,
                                             TransformPlus = transformPlus.Quantity != null ? transformPlus.Quantity : 0,
                                             ReceiptPlus = receiptPlus.Quantity != null ? receiptPlus.Quantity : 0,
                                             MoveOrderPlus = moveorderPlus.QuantityOrdered != null ? moveorderPlus.QuantityOrdered : 0,
                                             TransformOutPlus = transformoutPlus.WeighingScale != null ? transformoutPlus.WeighingScale : 0,
                                             IssuePlus = issuePlus.Quantity != null ? issuePlus.Quantity : 0,
                                             // AverageUnitCost = auc.UnitCost != null ? auc.UnitCost : 0,
                                             // TotalCost = auc.UnitCost != null ? auc.TotalUnitPrice : 0,
                                         }
                into total
                                     select new InventoryMovementReport
                                     {
                                         ItemCode = total.Key.ItemCode,
                                         ItemDescription = total.Key.ItemDescription,
                                         ItemCategory = total.Key.ItemCategoryName,
                                         TotalOut = total.Key.MoveOrder,
                                         TotalIn = total.Key.Issue,
                                         Ending = (total.Key.ReceiveIn + total.Key.ReceiptIn + total.Key.TransformIn) -
                                                  (total.Key.MoveOrder + total.Key.Transformation + total.Key.Issue),
                                         CurrentStock = total.Key.SOH - total.Key.Issue,
                                         PurchasedOrder = total.Key.ReceivePlus + total.Key.TransformPlus + total.Key.ReceiptPlus,
                                         OthersPlus = total.Key.MoveOrderPlus + total.Key.TransformOutPlus + total.Key.IssuePlus,
                                     });

            return await movementInventory.ToListAsync();

        }
        public async Task<PagedList<InventoryMovementReport>> InventoryMovementReportPagination(string DateFrom,
            string DateTo, string PlusOne, UserParams userParams)
        {
            var dateToday = DateTime.Now.ToString("MM/dd/yyyy");


            var getWarehouseStock = _context.WarehouseReceived.Where(x => x.IsActive == true)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new WarehouseInventory
                {
                    ItemCode = x.Key.ItemCode,
                    ActualGood = x.Sum(x => x.ActualGood)
                });

            var getMoveOrderOutByDate = _context.MoveOrders.Where(x => x.IsActive == true)
                .Where(x => x.IsPrepared == true)
                .Where(x => x.PreparedDate.Value.Date >= DateTime.Parse(DateFrom) && x.PreparedDate.Value.Date <= DateTime.Parse(DateTo) &&
                            x.ApprovedDate != null)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new MoveOrderInventory
                {
                    ItemCode = x.Key.ItemCode,
                    QuantityOrdered = x.Sum(x => x.QuantityOrdered)
                });

            var getMoveOrderOutByDatePlus = _context.MoveOrders.Where(x => x.IsActive == true)
                .Where(x => x.IsPrepared == true)
                .Where(x => x.PreparedDate.Value.Date >= DateTime.Parse(PlusOne) && x.PreparedDate.Value.Date <= DateTime.Parse(dateToday) &&
                            x.ApprovedDate != null)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new MoveOrderInventory
                {
                    ItemCode = x.Key.ItemCode,
                    QuantityOrdered = x.Sum(x => x.QuantityOrdered)
                });
            var getTransformationByDate = _context.Transformation_Preparation.Where(x => x.IsActive == true)
                .Where(x => x.PreparedDate >= DateTime.Parse(DateFrom) && x.PreparedDate <= DateTime.Parse(DateTo))
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new TransformationInventory
                {
                    ItemCode = x.Key.ItemCode,
                    WeighingScale = x.Sum(x => x.WeighingScale)
                });

            var getTransformationByDatePlus = _context.Transformation_Preparation.Where(x => x.IsActive == true)
                .Where(x => x.PreparedDate.Date >= DateTime.Parse(PlusOne) && x.PreparedDate.Date <= DateTime.Parse(dateToday))
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new TransformationInventory
                {
                    ItemCode = x.Key.ItemCode,
                    WeighingScale = x.Sum(x => x.WeighingScale)
                });

            var getIssueOutByDate = _context.MiscellaneousIssueDetails.Where(x => x.IsActive == true)
                .Where(x => x.IsTransact == true)
                .Where(x => x.PreparedDate.Date >= DateTime.Parse(DateFrom) && x.PreparedDate.Date <= DateTime.Parse(DateTo))
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new IssueInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.Quantity)
                });


            var getIssueOutByDatePlus = _context.MiscellaneousIssueDetails.Where(x => x.IsActive == true)
                .Where(x => x.IsTransact == true)
                .Where(x => x.PreparedDate.Date >= DateTime.Parse(PlusOne) && x.PreparedDate.Date <= DateTime.Parse(dateToday))
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new IssueInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.Quantity)
                });


            var getReceivetIn = _context.WarehouseReceived.Where(x => x.IsActive == true)
                .Where(x => x.TransactionType == "Receiving")
                .Where(x => x.ReceivingDate.Date >= DateTime.Parse(DateFrom) && x.ReceivingDate.Date <= DateTime.Parse(DateTo))
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new ReceiptInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.ActualGood)
                });

            var getReceivetInPlus = _context.WarehouseReceived.Where(x => x.IsActive == true)
                .Where(x => x.TransactionType == "Receiving")
                .Where(x => x.ReceivingDate.Date >= DateTime.Parse(PlusOne) && x.ReceivingDate.Date <= DateTime.Parse(dateToday))
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new ReceiptInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.ActualGood)
                });


            var getTransformIn = _context.WarehouseReceived.Where(x => x.IsActive == true)
                .Where(x => x.TransactionType == "Transformation")
                .Where(x => x.ReceivingDate >= DateTime.Parse(DateFrom) && x.ReceivingDate <= DateTime.Parse(DateTo))
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new ReceiptInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.ActualGood)
                });

            var getTransformInPlus = _context.WarehouseReceived.Where(x => x.IsActive == true)
                .Where(x => x.TransactionType == "Transformation")
                .Where(x => x.ReceivingDate >= DateTime.Parse(PlusOne) && x.ReceivingDate <= DateTime.Parse(dateToday))
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new ReceiptInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.ActualGood)
                });


            var getReceiptIn = _context.WarehouseReceived.Where(x => x.IsActive == true)
                .Where(x => x.TransactionType == "MiscellaneousReceipt")
                .Where(x => x.ReceivingDate.Date >= DateTime.Parse(DateFrom) && x.ReceivingDate.Date <= DateTime.Parse(DateTo))
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new ReceiptInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.ActualGood)
                });

            var getReceiptInPlus = _context.WarehouseReceived.Where(x => x.IsActive == true)
                .Where(x => x.TransactionType == "MiscellaneousReceipt")
                .Where(x => x.ReceivingDate.Date >= DateTime.Parse(PlusOne) && x.ReceivingDate.Date <= DateTime.Parse(dateToday))
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new ReceiptInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.ActualGood)
                });

            var getMoveOrderOut = _context.MoveOrders.Where(x => x.IsActive == true)
                .Where(x => x.IsPrepared == true)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new MoveOrderInventory
                {
                    ItemCode = x.Key.ItemCode,
                    QuantityOrdered = x.Sum(x => x.QuantityOrdered)
                });


            var getTransformation = _context.Transformation_Preparation.Where(x => x.IsActive == true)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new TransformationInventory
                {
                    ItemCode = x.Key.ItemCode,
                    WeighingScale = x.Sum(x => x.WeighingScale)
                });

            var getIssueOut = _context.MiscellaneousIssueDetails.Where(x => x.IsActive == true)
                .Where(x => x.IsTransact == true)
                .GroupBy(x => new
                {
                    x.ItemCode,
                }).Select(x => new IssueInventory
                {
                    ItemCode = x.Key.ItemCode,
                    Quantity = x.Sum(x => x.Quantity)
                });

            var getSOH = (from warehouse in getWarehouseStock
                          join moveorder in getMoveOrderOut
                              on warehouse.ItemCode equals moveorder.ItemCode
                              into leftJ3
                          from moveorder in leftJ3.DefaultIfEmpty()
                          group new
                          {
                              warehouse,
                              moveorder
                          }
                              by new
                              {
                                  warehouse.ItemCode
                              }
                into total
                          select new SOHInventory
                          {
                              ItemCode = total.Key.ItemCode,
                              SOH = total.Sum(x => x.warehouse.ActualGood == 0 ? 0 : x.warehouse.ActualGood) -
                                    total.Sum(x => x.moveorder.QuantityOrdered == 0 ? 0 : x.moveorder.QuantityOrdered)
                          });

            var movementInventory = (from rawmaterial in _context.RawMaterials
                                     join moveorder in getMoveOrderOutByDate
                                         on rawmaterial.ItemCode equals moveorder.ItemCode
                                         into leftJ
                                     from moveorder in leftJ.DefaultIfEmpty()
                                     join transformation in getTransformationByDate
                                         on rawmaterial.ItemCode equals transformation.ItemCode
                                         into leftJ2
                                     from transformation in leftJ2.DefaultIfEmpty()
                                     join issue in getIssueOutByDate
                                         on rawmaterial.ItemCode equals issue.ItemCode
                                         into leftJ3
                                     from issue in leftJ3.DefaultIfEmpty()
                                     join receive in getReceivetIn
                                         on rawmaterial.ItemCode equals receive.ItemCode
                                         into leftJ4
                                     from receive in leftJ4.DefaultIfEmpty()
                                     join transformIn in getTransformIn
                                         on rawmaterial.ItemCode equals transformIn.ItemCode
                                         into leftJ5
                                     from transformIn in leftJ5.DefaultIfEmpty()
                                     join receipt in getReceiptIn
                                         on rawmaterial.ItemCode equals receipt.ItemCode
                                         into leftJ6
                                     from receipt in leftJ6.DefaultIfEmpty()
                                     join SOH in getSOH
                                         on rawmaterial.ItemCode equals SOH.ItemCode
                                         into leftJ7
                                     from SOH in leftJ7.DefaultIfEmpty()
                                     join receivePlus in getReceivetInPlus
                                         on rawmaterial.ItemCode equals receivePlus.ItemCode
                                         into leftJ8
                                     from receivePlus in leftJ8.DefaultIfEmpty()
                                     join transformPlus in getTransformInPlus
                                         on rawmaterial.ItemCode equals transformPlus.ItemCode
                                         into leftJ9
                                     from transformPlus in leftJ9.DefaultIfEmpty()
                                     join receiptPlus in getReceiptInPlus
                                         on rawmaterial.ItemCode equals receiptPlus.ItemCode
                                         into leftJ10
                                     from receiptPlus in leftJ10.DefaultIfEmpty()
                                     join moveorderPlus in getMoveOrderOutByDatePlus
                                         on rawmaterial.ItemCode equals moveorderPlus.ItemCode
                                         into leftJ11
                                     from moveorderPlus in leftJ11.DefaultIfEmpty()
                                     join transformoutPlus in getTransformationByDatePlus
                                         on rawmaterial.ItemCode equals transformoutPlus.ItemCode
                                         into leftJ12
                                     from transformoutPlus in leftJ12.DefaultIfEmpty()
                                     join issuePlus in getIssueOutByDatePlus
                                         on rawmaterial.ItemCode equals issuePlus.ItemCode
                                         into leftJ13
                                     from issuePlus in leftJ13.DefaultIfEmpty()
                                     group new
                                     {
                                         rawmaterial,
                                         moveorder,
                                         transformation,
                                         issue,
                                         receive,
                                         transformIn,
                                         receipt,
                                         SOH,
                                         receivePlus,
                                         transformPlus,
                                         receiptPlus,
                                         moveorderPlus,
                                         transformoutPlus,
                                         issuePlus
                                     }
                                         by new
                                         {
                                             rawmaterial.ItemCode,
                                             rawmaterial.ItemDescription,
                                             rawmaterial.ItemCategory.ItemCategoryName,
                                             MoveOrder = moveorder.QuantityOrdered != null ? moveorder.QuantityOrdered : 0,
                                             Transformation = transformation.WeighingScale != null ? transformation.WeighingScale : 0,
                                             Issue = issue.Quantity != null ? issue.Quantity : 0,
                                             ReceiptIn = receipt.Quantity != null ? receipt.Quantity : 0,
                                             ReceiveIn = receive.Quantity != null ? receive.Quantity : 0,
                                             TransformIn = transformIn.Quantity != null ? transformIn.Quantity : 0,
                                             SOH = SOH.SOH != null ? SOH.SOH : 0,
                                             ReceivePlus = receivePlus.Quantity != null ? receivePlus.Quantity : 0,
                                             TransformPlus = transformPlus.Quantity != null ? transformPlus.Quantity : 0,
                                             ReceiptPlus = receiptPlus.Quantity != null ? receiptPlus.Quantity : 0,
                                             MoveOrderPlus = moveorderPlus.QuantityOrdered != null ? moveorderPlus.QuantityOrdered : 0,
                                             TransformOutPlus = transformoutPlus.WeighingScale != null ? transformoutPlus.WeighingScale : 0,
                                             IssuePlus = issuePlus.Quantity != null ? issuePlus.Quantity : 0,
                                         }
                into total
                                     select new InventoryMovementReport
                                     {
                                         ItemCode = total.Key.ItemCode,
                                         ItemDescription = total.Key.ItemDescription,
                                         ItemCategory = total.Key.ItemCategoryName,
                                         TotalOut = total.Key.MoveOrder,
                                         TotalIn = total.Key.Issue,
                                         Ending = (total.Key.ReceiveIn + total.Key.ReceiptIn + total.Key.TransformIn) -
                                                  (total.Key.MoveOrder + total.Key.Transformation + total.Key.Issue),
                                         CurrentStock = total.Key.SOH - total.Key.Issue,
                                         PurchasedOrder = total.Key.ReceivePlus + total.Key.ReceiptPlus,
                                         OthersPlus = total.Key.MoveOrderPlus + total.Key.TransformOutPlus + total.Key.IssuePlus
                                     });

            return await PagedList<InventoryMovementReport>.CreateAsync(movementInventory, userParams.PageNumber, userParams.PageSize);
        }
    }
}
