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
    
    public partial class ShopBranchSkusInfo:BaseModel
    {
        int _id;
        public int Id { get{ return _id; } set{ _id=value;} }
        public long ProductId { get; set; }
        public string SkuId { get; set; }
        public long ShopId { get; set; }
        public long ShopBranchId { get; set; }
        public int Stock { get; set; }
        public Himall.CommonModel.ShopBranchSkuStatus Status { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}