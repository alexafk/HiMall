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
    
    public partial class AttributeInfo:BaseModel
    {
        public AttributeInfo()
        {
            this.AttributeValueInfo = new HashSet<AttributeValueInfo>();
            this.ProductAttributesInfo = new HashSet<ProductAttributeInfo>();
        }
    
        long _id;
        public long Id { get{ return _id; } set{ _id=value;} }
        public long TypeId { get; set; }
        public string Name { get; set; }
        public long DisplaySequence { get; set; }
        public bool IsMust { get; set; }
        public bool IsMulti { get; set; }
    
        public virtual ICollection<AttributeValueInfo> AttributeValueInfo { get; set; }
        public virtual ProductTypeInfo TypesInfo { get; set; }
        public virtual ICollection<ProductAttributeInfo> ProductAttributesInfo { get; set; }
    }
}