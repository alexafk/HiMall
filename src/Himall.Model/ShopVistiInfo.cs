//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Himall.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ShopVistiInfo:BaseModel
    {
        long _id;
        public long Id { get{ return _id; } set{ _id=value;} }
        public long ShopId { get; set; }
        public System.DateTime Date { get; set; }
        public long VistiCounts { get; set; }
        public long SaleCounts { get; set; }
        public decimal SaleAmounts { get; set; }
        public long OrderUserCount { get; set; }
        public long OrderCount { get; set; }
        public long OrderProductCount { get; set; }
        public decimal OrderAmount { get; set; }
        public long OrderPayUserCount { get; set; }
        public long OrderPayCount { get; set; }
        public bool StatisticFlag { get; set; }
    }
}
