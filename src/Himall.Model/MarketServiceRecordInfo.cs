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
    
    public partial class MarketServiceRecordInfo:BaseModel
    {
        long _id;
        public long Id { get{ return _id; } set{ _id=value;} }
        public long MarketServiceId { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public long SettlementFlag { get; set; }
        public decimal Price { get; set; }
        public System.DateTime BuyTime { get; set; }
    
        public virtual ActiveMarketServiceInfo ActiveMarketServiceInfo { get; set; }
    }
}