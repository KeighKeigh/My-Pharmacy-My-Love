using DocumentFormat.OpenXml.Bibliography;
using ELIXIR.DATA.CORE.INTERFACES.INVENTORY_INTERFACE;
using ELIXIR.DATA.DATA_ACCESS_LAYER.HELPERS;
using ELIXIR.DATA.DATA_ACCESS_LAYER.MODELS.INVENTORY_MODEL;
using ELIXIR.DATA.DATA_ACCESS_LAYER.MODELS.WAREHOUSE_MODEL;
using ELIXIR.DATA.DATA_ACCESS_LAYER.STORE_CONTEXT;
using ELIXIR.DATA.DTOs.INVENTORY_DTOs;
using ELIXIR.DATA.DTOs.MISCELLANEOUS_DTOs;
using ELIXIR.DATA.DTOs.TRANSFORMATION_DTOs;
using ELIXIR.DATA.DTOs.WAREHOUSE_DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELIXIR.DATA.DATA_ACCESS_LAYER.REPOSITORIES.INVENTORY_REPOSITORY
{
    public class MiscellaneousRepository : IMiscellaneous
    {
        private readonly StoreContext _context;

        public MiscellaneousRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<bool> AddMiscellaneousReceipt(MiscellaneousReceipt receipt)
        {
            var one = await _context.OneChargings.FirstOrDefaultAsync(x => x.code == receipt.OneChargingCode);
            if (one != null)
            {

                receipt.LocationCode = one.location_code;
                receipt.LocationName = one.location_name;
                receipt.CompanyCode = one.company_code;
                receipt.CompanyName = one.company_name;
                receipt.DepartmentCode = one.department_code;
                receipt.DepartmentName = one.department_name;
                receipt.BusinessUnitCode = one.business_unit_code;
                receipt.BusinessUnitName = one.business_unit_name;
                receipt.DepartmentUnitCode = one.department_unit_code;
                receipt.DepartmentUnitName = one.department_unit_name;
                receipt.SubUnitCode = one.sub_unit_code;
                receipt.SubUnitName = one.sub_unit_name;

            }
            await _context.MiscellaneousReceipts.AddAsync(receipt);
            return true;
        }

        public async Task<bool> AddMiscellaneousReceiptInWarehouse(WarehouseReceiving receive)
        {
            await _context.WarehouseReceived.AddAsync(receive);
            return true;
        }

        public async Task<bool> AddWarehouseReceiveForReceipt(WarehouseReceiving warehouse)
        {
            await _context.WarehouseReceived.AddAsync(warehouse);
            return true;
        }

        public async Task<IReadOnlyList<MReceiptDto>> GetAllMiscellanousReceipt(bool status)
        {

            var receipt = _context.MiscellaneousReceipts.Where(x => x.IsActive == status)
                .Select(x => new MReceiptDto
                {
                Id = x.Id,
                SupplierCode = x.SupplierCode,
                SupplierName = x.Supplier,
                TotalQuantity = x.TotalQuantity,
                PreparedDate = x.PreparedDate.ToString("MM/dd/yyyy"),
                Remarks = x.Remarks,
                LocationCode = x.LocationCode,
                LocationName = x.LocationName,
                CompanyCode = x.CompanyCode,
                CompanyName = x.CompanyName,
                DepartmentCode = x.DepartmentCode,
                DepartmentName = x.DepartmentName,
                BusinessUnitCode = x.BusinessUnitCode,
                BusinessUnitName = x.BusinessUnitName,
                DepartmentUnitCode = x.DepartmentUnitCode,
                DepartmentUnitName = x.DepartmentUnitName,
                SubUnitCode = x.SubUnitCode,
                SubUnitName = x.SubUnitName,
                AccountCode = x.AccountCode,
                AccountTitle = x.AccountTitle,
                
                });

            return await receipt.ToListAsync();
        }

        public async Task<bool> ActivateMiscellaenousReceipt(MiscellaneousReceipt receipt)
        {
            var existing = await _context.MiscellaneousReceipts.Where(x => x.Id == receipt.Id)
                                                               .FirstOrDefaultAsync();

            var existingWH = await _context.WarehouseReceived.Where(x => x.MiscellaneousReceiptId == receipt.Id)
                                                             .ToListAsync();

            if (existing == null)
                return false;

            existing.IsActive = true;

            foreach(var items in existingWH)
            {
                items.IsActive = true;
            }


            return true;
        }

        public async Task<bool> InActivateMiscellaenousReceipt(MiscellaneousReceipt receipt)
        {
            var existing = await _context.MiscellaneousReceipts.Where(x => x.Id == receipt.Id)
                                                               .FirstOrDefaultAsync();
              
            var existingWH = await _context.WarehouseReceived.Where(x => x.MiscellaneousReceiptId == receipt.Id)
                                                             .ToListAsync();

            if (existing == null)
                return false;

            existing.IsActive = false;

            foreach (var items in existingWH)
            {
                items.IsActive = false;
            }

            return true;
        }

        public async Task<PagedList<MReceiptDto>> GetAllMReceiptWithPagination(UserParams userParams, bool status)
        {

            var receipt = _context.MiscellaneousReceipts.OrderByDescending(x => x.PreparedDate)
                                                        .Where(x => x.IsActive == status)
                .Select(x => new MReceiptDto
                {
                    Id = x.Id,
                    SupplierCode = x.SupplierCode,
                    SupplierName = x.Supplier,
                    TotalQuantity = x.TotalQuantity,
                    PreparedDate = x.PreparedDate.ToString("MM/dd/yyyy"),
                    Remarks = x.Remarks,
                    PreparedBy = x.PreparedBy,
                    IsActive = x.IsActive,
                    LocationCode = x.LocationCode,
                    LocationName = x.LocationName,
                    CompanyCode = x.CompanyCode,
                    CompanyName = x.CompanyName,
                    DepartmentCode = x.DepartmentCode,
                    DepartmentName = x.DepartmentName,
                    BusinessUnitCode = x.BusinessUnitCode,
                    BusinessUnitName = x.BusinessUnitName,
                    DepartmentUnitCode = x.DepartmentUnitCode,
                    DepartmentUnitName = x.DepartmentUnitName,
                    SubUnitCode = x.SubUnitCode,
                    SubUnitName = x.SubUnitName,
                    AccountCode = x.AccountCode,
                    AccountTitle = x.AccountTitle,
                });


            return await PagedList<MReceiptDto>.CreateAsync(receipt, userParams.PageNumber, userParams.PageSize);
        }



        public async Task<PagedList<MReceiptDto>> GetAllMReceiptWithPaginationOrig(UserParams userParams, string search, bool status)
        {
            var receipt = _context.MiscellaneousReceipts.OrderByDescending(x => x.PreparedDate)
                                                        .Where(x => x.IsActive == status)
            .Select(x => new MReceiptDto
            {
                Id = x.Id,
                SupplierCode = x.SupplierCode,
                SupplierName = x.Supplier,
                TotalQuantity = x.TotalQuantity,
                PreparedDate = x.PreparedDate.ToString("MM/dd/yyyy"),
                Remarks = x.Remarks,
                PreparedBy = x.PreparedBy,
                IsActive = x.IsActive,
                LocationCode = x.LocationCode,
                LocationName = x.LocationName,
                CompanyCode = x.CompanyCode,
                CompanyName = x.CompanyName,
                DepartmentCode = x.DepartmentCode,
                DepartmentName = x.DepartmentName,
                BusinessUnitCode = x.BusinessUnitCode,
                BusinessUnitName = x.BusinessUnitName,
                DepartmentUnitCode = x.DepartmentUnitCode,
                DepartmentUnitName = x.DepartmentUnitName,
                SubUnitCode = x.SubUnitCode,
                SubUnitName = x.SubUnitName,
                AccountCode = x.AccountCode,
                AccountTitle = x.AccountTitle,

            })
              .Where(x => (Convert.ToString(x.Id)).ToLower()
              .Contains(search.Trim().ToLower()));
             

            return await PagedList<MReceiptDto>.CreateAsync(receipt, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<IReadOnlyList<MReceiptDto>> GetWarehouseDetailsByMReceipt(int id)
        {

            var receipt = (from warehouse in _context.WarehouseReceived
                           where warehouse.MiscellaneousReceiptId == id && warehouse.IsActive == true
                           join receiptParent in _context.MiscellaneousReceipts
                           on warehouse.MiscellaneousReceiptId equals receiptParent.Id into leftJ
                           from receiptParent in leftJ.DefaultIfEmpty()

                           select new MReceiptDto
                           {

                               Id = receiptParent.Id,
                               WarehouseId = warehouse.Id,
                               ItemCode = warehouse.ItemCode,
                               ItemDescription = warehouse.ItemDescription,
                               TotalQuantity = warehouse.ActualGood,
                               SupplierCode = receiptParent.SupplierCode,
                               SupplierName = receiptParent.Supplier,
                               PreparedDate = receiptParent.PreparedDate.ToString(),
                               PreparedBy = receiptParent.PreparedBy,
                               Remarks = receiptParent.Remarks,
                               DepartmentCode = receiptParent.DepartmentCode,
                               DepartmentName = receiptParent.DepartmentName,
                               LocationCode = receiptParent.LocationCode,
                               LocationName = receiptParent.LocationName,
                               CompanyCode = receiptParent.CompanyCode,
                               CompanyName = receiptParent.CompanyName,
                               BusinessUnitCode = receiptParent.BusinessUnitCode,
                               BusinessUnitName = receiptParent.BusinessUnitName,
                               DepartmentUnitCode = receiptParent.DepartmentUnitCode,
                               DepartmentUnitName = receiptParent.DepartmentUnitName,
                               SubUnitCode = receiptParent.SubUnitCode,
                               SubUnitName = receiptParent.SubUnitName,

                           });

            return await receipt.ToListAsync();
                                                     
        }

        

        //-----------------------MISCELLANEOUS ISSUE-----------------------------//

        public async Task<bool> AddMiscellaneousIssue(MiscellaneousIssue issue)
        {
            var one = await _context.OneChargings.FirstOrDefaultAsync(x => x.code == issue.OneChargingCode);
            if ( one != null)
            {

                issue.LocationCode = one.location_code;
                issue.LocationName = one.location_name;
                issue.CompanyCode = one.company_code;
                issue.CompanyName = one.company_name;
                issue.DepartmentCode = one.department_code;
                issue.DepartmentName = one.department_name;
                issue.BusinessUnitCode = one.business_unit_code;
                issue.BusinessUnitName = one.business_unit_name;
                issue.DepartmentUnitCode = one.department_unit_code;
                issue.DepartmentUnitName = one.department_unit_name;
                issue.SubUnitCode = one.sub_unit_code;
                issue.SubUnitName = one.sub_unit_name;

            }
            await _context.MiscellaneousIssues.AddAsync(issue);
            return true;
        }

        public async Task<bool> AddMiscellaneousIssueDetails(MiscellaneousIssueDetails details)
        {
            await _context.MiscellaneousIssueDetails.AddAsync(details);
            return true;
        }

        public async Task<PagedList<MIssueDto>> GetAllMIssueWithPagination(UserParams userParams, bool status)
        {

            var issue = _context.MiscellaneousIssues.OrderByDescending(x => x.PreparedDate)
                                                    .Where(x => x.IsActive == status)
                .Select(x => new MIssueDto
                {
                    IssuePKey = x.Id,
                    Customer = x.Customer,
                    CustomerCode = x.CustomerCode,
                    TotalQuantity = x.TotalQuantity,
                    PreparedDate = x.PreparedDate.ToString("MM/dd/yyyy"),
                    Remarks = x.Remarks,
                    PreparedBy = x.PreparedBy,
                    IsActive = x.IsActive,
                    LocationCode = x.LocationCode,
                    LocationName = x.LocationName,
                    CompanyCode = x.CompanyCode,
                    CompanyName = x.CompanyName,
                    DepartmentCode = x.DepartmentCode,
                    DepartmentName = x.DepartmentName,
                    BusinessUnitCode = x.BusinessUnitCode,
                    BusinessUnitName = x.BusinessUnitName,
                    DepartmentUnitCode = x.DepartmentUnitCode,
                    DepartmentUnitName = x.DepartmentUnitName,
                    SubUnitCode = x.SubUnitCode,
                    SubUnitName = x.SubUnitName,
                    AccountCode = x.AccountCode,
                    AccountTitle = x.AccountTitle,
                    
                });

            return await PagedList<MIssueDto>.CreateAsync(issue, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<PagedList<MIssueDto>> GetAllMIssueWithPaginationOrig(UserParams userParams, string search, bool status)
        {
            var issue = _context.MiscellaneousIssues.OrderByDescending(x => x.PreparedDate)
                                                    .Where(x => x.IsActive == status)
             .Select(x => new MIssueDto
             {
                 IssuePKey = x.Id,
                 Customer = x.Customer,
                 CustomerCode = x.CustomerCode,
                 TotalQuantity = x.TotalQuantity,
                 PreparedDate = x.PreparedDate.ToString("MM/dd/yyyy"),
                 Remarks = x.Remarks,
                 PreparedBy = x.PreparedBy,
                 IsActive = x.IsActive,
                 LocationCode = x.LocationCode,
                 LocationName = x.LocationName,
                 CompanyCode = x.CompanyCode,
                 CompanyName = x.CompanyName,
                 DepartmentCode = x.DepartmentCode,
                 DepartmentName = x.DepartmentName,
                 BusinessUnitCode = x.BusinessUnitCode,
                 BusinessUnitName = x.BusinessUnitName,
                 DepartmentUnitCode = x.DepartmentUnitCode,
                 DepartmentUnitName = x.DepartmentUnitName,
                 SubUnitCode = x.SubUnitCode,
                 SubUnitName = x.SubUnitName,
                 AccountCode = x.AccountCode,
                 AccountTitle = x.AccountTitle,

             }).Where(x => (Convert.ToString(x.IssuePKey)).ToLower()
               .Contains(search.Trim().ToLower()));

            return await PagedList<MIssueDto>.CreateAsync(issue, userParams.PageNumber, userParams.PageSize);

        }

        public async Task<bool> ActivateMiscellaenousIssue(MiscellaneousIssue issue)
        {
            var existing = await _context.MiscellaneousIssues.Where(x => x.Id == issue.Id)
                                                             .FirstOrDefaultAsync();

            var existingdetails = await _context.MiscellaneousIssueDetails.Where(x => x.IssuePKey == issue.Id)
                                                                          .ToListAsync();

            if (existing == null)
                return false;

            existing.IsActive = true;

            foreach (var items in existingdetails)
            {
                items.IsActive = true;
            }


            return true;
        }

        public async Task<bool> InActivateMiscellaenousIssue(MiscellaneousIssue issue)
        {
            var existing = await _context.MiscellaneousIssues.Where(x => x.Id == issue.Id)
                                                           .FirstOrDefaultAsync();

            var existingdetails = await _context.MiscellaneousIssueDetails.Where(x => x.IssuePKey == issue.Id)
                                                                          .ToListAsync();

            if (existing == null)
                return false;

            existing.IsActive = false;

            foreach (var items in existingdetails)
            {
                items.IsActive = false;
            }
            return true;
        }

        public async Task<IReadOnlyList<MIssueDto>> GetAllDetailsInMiscellaneousIssue(int id)
        {
            var warehouse = _context.MiscellaneousIssueDetails
                                                              .Where(x => x.IssuePKey == id)
                                                    .Select(x => new MIssueDto
                                                    {
                                                        IssuePKey = x.IssuePKey,
                                                        Customer = x.Customer,
                                                        CustomerCode = x.CustomerCode,
                                                        PreparedDate = x.PreparedDate.ToString(),
                                                        PreparedBy = x.PreparedBy,
                                                        ItemCode = x.ItemCode,
                                                        ItemDescription = x.ItemDescription,
                                                        TotalQuantity = x.Quantity,
                                                        Remarks = x.Remarks

                                                    });


            //var issues = _context.MiscellaneousIssues.Where(x => x.IsActive == true)
            //                                 .Select(x => new MIssueDto
            //                                 {
            //                                     Id = x.Id,
            //                                     Customer = x.Customer,
            //                                     CustomerCode = x.Customer,
            //                                     PreparedDate = x.PreparedDate.ToString(),
            //                                     CompanyCode = x.CompanyCode,
            //                                     CompanyName = x.CompanyName,
            //                                     DepartmentCode = x.DepartmentCode,
            //                                     DepartmentName = x.DepartmentName,
            //                                     LocationCode = x.LocationCode,
            //                                     LocationName = x.LocationName,
            //                                     //AccountCode = x.AccountCode,
            //                                     //AccountTitles = x.AccountTitles,
            //                                     //EmpId = x.EmpId,
            //                                     //FullName = x.FullName,
            //                                     BusinessUnitName = x.BusinessUnitName,
            //                                     BusinessUnitCode = x.BusinessUnitCode,
            //                                     DepartmentUnitName = x.DepartmentUnitName,
            //                                     DepartmentUnitCode = x.DepartmentUnitCode,
            //                                     SubUnitName = x.SubUnitName,
            //                                     SubUnitCode = x.SubUnitCode,


            //                                 });


            //var warehouse = _context.MiscellaneousIssueDetails
            //    .GroupJoin(issues, misc => misc.IssuePKey, issue => issue.Id, (misc, issue) => new { misc, issue })
            //    .SelectMany(x => x.issue.DefaultIfEmpty(), (x, issue) => new { x.misc, issue })
            //    .Where(x => x.misc.IssuePKey == id)
            //    .Select(x => new MIssueDto
            //    {
            //        IssuePKey = x.misc.IssuePKey,
            //        Customer = x.issue.Customer,
            //        CustomerCode = x.issue.CustomerCode,
            //        PreparedDate = x.misc.PreparedDate.ToString(),
            //        PreparedBy = x.misc.PreparedBy,
            //        ItemCode = x.misc.ItemCode,
            //        ItemDescription = x.misc.ItemDescription,
            //        TotalQuantity = x.misc.Quantity,
            //        Remarks = x.misc.Remarks,

            //        Uom = x.misc.Uom,

            //        CompanyCode = x.issue.CompanyCode,
            //        CompanyName = x.issue.CompanyName,
            //        DepartmentCode = x.issue.DepartmentCode,
            //        DepartmentName = x.issue.DepartmentName,
            //        LocationCode = x.issue.LocationCode,
            //        LocationName = x.issue.LocationName,
                    
            //        BusinessUnitName = x.issue.BusinessUnitName,
            //        BusinessUnitCode = x.issue.BusinessUnitCode,
            //        DepartmentUnitName = x.issue.DepartmentUnitName,
            //        DepartmentUnitCode = x.issue.DepartmentUnitCode,
            //        SubUnitName = x.issue.SubUnitName,
            //        SubUnitCode = x.issue.SubUnitCode,




            //    });
            return await warehouse.ToListAsync();
        }


        public async Task<IReadOnlyList<MIssueDto>> GetAvailableStocksForIssue(string itemcode)
        {

            var getWarehouseStocks = _context.WarehouseReceived.Where(x => x.IsActive == true)
         .GroupBy(x => new
         {
             x.Id,
             x.ItemCode,
             x.ActualGood,
             x.Expiration

         }).Select(x => new WarehouseInventory
         {
             WarehouseId = x.Key.Id,
             ItemCode = x.Key.ItemCode,
             ActualGood = x.Key.ActualGood,
             ExpirationDate = x.Key.Expiration.ToString()

         });

            var transformOut = _context.Transformation_Preparation.Where(x => x.IsActive == true)
                                                               //   .Where(x => x.IsMixed == true)
                        .GroupBy(x => new
                        {
                            x.WarehouseId,
                            x.ItemCode
                        })
                        .Select(x => new TransformationInventory
                        {
                            WarehouseId = x.Key.WarehouseId,
                            ItemCode = x.Key.ItemCode,
                            WeighingScale = x.Sum(x => x.WeighingScale)
                        });
            var moveorderOut = _context.MoveOrders.Where(x => x.IsActive == true)
                                                  .Where(x => x.IsPrepared == true)
                        .GroupBy(x => new
                        {
                            x.WarehouseId,
                            x.ItemCode
                        })
                        .Select(x => new MoveOrderInventory
                        {
                            WarehouseId = x.Key.WarehouseId,
                            ItemCode = x.Key.ItemCode,
                            QuantityOrdered = x.Sum(x => x.QuantityOrdered)
                        });

            var issueOut = _context.MiscellaneousIssueDetails.Where(x => x.IsActive == true)
                                                    //       .Where(x => x.IsTransact == true)
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



            var getAvailable = (from warehouse in getWarehouseStocks
                                join transform in transformOut
                                on warehouse.WarehouseId equals transform.WarehouseId
                                into leftJ1
                                from transform in leftJ1.DefaultIfEmpty()

                                join moveorder in moveorderOut
                                on warehouse.WarehouseId equals moveorder.WarehouseId
                                into leftJ2
                                from moveorder in leftJ2.DefaultIfEmpty()

                                join issue in issueOut
                                on warehouse.WarehouseId equals issue.WarehouseId
                                into leftJ3
                                from issue in leftJ3.DefaultIfEmpty()

                                group new
                                {
                                    warehouse,
                                    transform,
                                    moveorder,
                                    issue
                                }

                                by new
                                {
                                    warehouse.WarehouseId,
                                    warehouse.ItemCode,
                                    warehouse.ExpirationDate,
                                    WarehouseActualGood = warehouse.ActualGood != null ? warehouse.ActualGood : 0,
                                    TransformOut = transform.WeighingScale != null ? transform.WeighingScale : 0,
                                    MoveOrderOut = moveorder.QuantityOrdered != null ? moveorder.QuantityOrdered : 0,
                                    IssueOut = issue.Out != null ? issue.Out : 0

                                } into total

                                select new MIssueDto
                                {
                                    WarehouseId = total.Key.WarehouseId,
                                    ItemCode = total.Key.ItemCode,
                                    RemainingStocks = total.Key.WarehouseActualGood - total.Key.TransformOut - total.Key.MoveOrderOut - total.Key.IssueOut,
                                    ExpirationDate = total.Key.ExpirationDate

                                }).Where(x => x.RemainingStocks != 0)
                                  .Where(x => x.ItemCode == itemcode);


            return await getAvailable.ToListAsync();

        }

        public async Task<bool> UpdateIssuePKey(MiscellaneousIssueDetails details)
        {
            var existing = await _context.MiscellaneousIssueDetails.Where(x => x.Id == details.Id)
                                                                   .FirstOrDefaultAsync();

            if (existing == null)
                return false;

            existing.IssuePKey = details.IssuePKey;
            existing.IsTransact = true;

            return true;

        }

        public async Task<IReadOnlyList<MIssueDto>> GetAllAvailableIssue(int empid)
        {
            var employee = await _context.Users.Where(x => x.Id == empid)
                                         .FirstOrDefaultAsync();

            var items = _context.MiscellaneousIssueDetails.Where(x => x.IsActive == true)
                                                          .Where(x => x.IsTransact != true)
                                                          .Where(x => x.PreparedBy == employee.FullName)
                .Select(x => new MIssueDto
                {
                    Id = x.Id,
                    ItemCode = x.ItemCode,
                    ItemDescription = x.ItemDescription,
                    Uom = x.Uom,
                    TotalQuantity = x.Quantity,
                    ExpirationDate = x.ExpirationDate.ToString()

                });

            return await items.ToListAsync();           
        }

        public async Task<bool> CancelIssuePerItemCode(MiscellaneousIssueDetails issue)
        {
            var items = await _context.MiscellaneousIssueDetails.Where(x => x.Id == issue.Id)
                                                                .FirstOrDefaultAsync();
            if (items == null)
                return false;

            items.IsActive = false;

            return true;
        }

        public async Task<bool> ValidateMiscellaneousReceiptInIssue(MiscellaneousReceipt receipt)
        {
            var validate = await _context.WarehouseReceived.Where(x => x.MiscellaneousReceiptId == receipt.Id)
                                                           .ToListAsync();

            foreach(var items in validate)
            {
                var issue = await _context.MiscellaneousIssueDetails.Where(x => x.WarehouseId == items.Id)
                                                                    .FirstOrDefaultAsync();

                if (issue != null)
                    return false;
            }

            return true;

        }
    }
}
