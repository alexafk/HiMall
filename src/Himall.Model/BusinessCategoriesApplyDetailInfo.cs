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
    
    public partial class BusinessCategoriesApplyDetailInfo:BaseModel
    {
        long _id;
        public long Id { get{ return _id; } set{ _id=value;} }
        public decimal CommisRate { get; set; }
        public long CategoryId { get; set; }
        public long ApplyId { get; set; }
    
        public virtual BusinessCategoriesApplyInfo Himall_BusinessCategoriesApply { get; set; }
        public virtual CategoryInfo Himall_Categories { get; set; }
    }
}
