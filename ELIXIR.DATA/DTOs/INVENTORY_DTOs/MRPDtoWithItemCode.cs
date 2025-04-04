using DocumentFormat.OpenXml.Vml;
using ELIXIR.DATA.DTOs.INVENTORY_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELIXIR.DATA.DTOs.INVENTORY_DTOs
{
    public class MRPDtoWithItemCode
    {
        public string Id { get; set; }
        public string ItemDescription { get; set; }
        public decimal BufferLevel { get; set; }
        public decimal Reserve { get; set; }
        public decimal AverageIssuance { get; set; }
    }
}


//select new MRPDtoWithItemCode
//{
//    ItemCode = total.Key.ItemCode,
//    ItemDescription = total.Key.ItemDescription,
//    Uom = total.Key.UOM_Code,
//    ItemCategory = total.Key.ItemCategoryName,
//    BufferLevel = total.Key.BufferLevel,
//    Price = total.Key.UnitPrice,
//    ReceiveIn = total.Key.WarehouseActualGood,
//    MoveOrderOut = total.Key.MoveOrderOut,
//    QCReceiving = total.Key.QcReceiving,
//    ReceiptIn = total.Key.ReceiptIn,
//    IssueOut = total.Key.IssueOut,
//    TotalPrice = Math.Round(Convert.ToDecimal(total.Key.TotalPrice), 2),
//    SOH = total.Key.SOH - total.Key.IssueOut,
//    Reserve = total.Key.Reserve - total.Key.IssueOut,
//    SuggestedPo = total.Key.SuggestedPo,
//    AverageIssuance = Math.Round(Convert.ToDecimal(total.Key.AverageIssuance), 2),
//    DaysLevel = Math.Round(Convert.ToDecimal(total.Key.Reserve / (total.Key.AverageIssuance != 0 ? total.Key.AverageIssuance : 1)), 2),
//    ReserveUsage = total.Key.ReserveUsage,
//    TransformFrom = total.Key.TransformFrom,
//    TransformTo = total.Key.TransformTo
//    //LastUsed = total.Key.LastUsed.ToString()                       
//});