﻿using Himall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himall.DTO.QueryModel
{
    /// <summary>
    /// 门店查询参数
    /// </summary>
    public class ShopBranchProductQuery : QueryBase
    {
        public long? CategoryId { get; set; }

        public string BrandNameKeyword { get; set; }

        public IEnumerable<ProductInfo.ProductAuditStatus> AuditStatus { get; set; }

        public ProductInfo.ProductSaleStatus? SaleStatus { get; set; }
        /// <summary>
        /// 门店商品状态
        /// </summary>
        public Himall.CommonModel.ShopBranchSkuStatus? ShopBranchProductStatus { get; set; }
        public string KeyWords { get; set; }

        public IEnumerable<long> Ids { get; set; }

        public string ShopName { get; set; }

        public long? ShopId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public long? ShopCategoryId { get; set; }

        public bool IsLimitTimeBuy { get; set; }

        public string ProductCode { get; set; }
        /// <summary>
        /// 排序
        /// <para>默认 编号 2 添加时间 3 销量 5 分类</para>
        /// </summary>
        public int OrderKey { get; set; }
        public bool OrderType { get; set; }
        /// <summary>
        /// 是否超出警戒库存
        /// </summary>
        public bool? IsOverSafeStock { get; set; }
        /// <summary>
        /// 不包含草稿箱
        /// </summary>
        public bool NotIncludedInDraft { get; set; }

        public long? shopBranchId { get; set; }
        public long? productId { get; set; }
        /// <summary>
        /// 需要移除的商品
        /// </summary>
        public long? RproductId { get; set; }
        public bool? HasLadderProduct { get; set; }
    }
}
