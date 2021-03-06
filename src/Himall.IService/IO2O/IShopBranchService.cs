﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Himall.Model;
using Himall.CommonModel;
using Himall.DTO.QueryModel;

namespace Himall.IServices
{

    public interface IShopBranchService : IService
    {
        /// <summary>
        /// 添加分店
        /// </summary>
        /// <param name="shopBranchInfo"></param>
        void AddShopBranch(ShopBranchInfo shopBranchInfo);

        /// <summary>
        /// 添加分店管理员
        /// </summary>
        /// <param name="shopBranchManagersInfo"></param>
        void AddShopBranchManagers(ShopBranchManagersInfo shopBranchManagersInfo);
        /// <summary>
        /// 更新门店信息
        /// </summary>
        /// <param name="shopBranch"></param>
        void UpdateShopBranch(ShopBranchInfo shopBranch);
        /// <summary>
        /// 更新门店管理员密码
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="pwd"></param>
        /// <param name="pwdSalt"></param>
        void UpdateShopBranchManagerPwd(long branchId, string userName, string pwd, string pwdSalt);

        /// <summary>
        /// 更新指定门店管理员的密码
        /// </summary>
        /// <param name="managerId"></param>
        /// <param name="password"></param>
        void UpdateShopBranchManagerPwd(long managerId, string password);

        /// <summary>
        /// 删除门店
        /// </summary>
        /// <param name="id"></param>
        void DeleteShopBranch(long id);
        /// <summary>
        /// 判断门店名称是否重复
        /// </summary>
        /// <param name="shopId">商家店铺ID</param>
        /// <param name="shopBranchName">门店名字</param>
        /// <returns></returns>
        bool Exists(long shopId, long shopBranchId, string shopBranchName);

        /// <summary>
        /// 根据查询条件判断是否有门店
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        bool Exists(ShopBranchQuery query);
        /// <summary>
        /// 根据门店ID获取门店
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ShopBranchInfo GetShopBranchById(long id);

        /// <summary>
        /// 根据门店IDs获取门店
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        List<ShopBranchInfo> GetShopBranchByIds(IEnumerable<long> Ids);

        /// <summary>
        /// 根据门店联系方式获取门店信息
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        ShopBranchInfo GetShopBranchByContact(string contact);

        /// <summary>
        /// 根据门店ID取门店管理员
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        IEnumerable<ShopBranchManagersInfo> GetShopBranchManagers(long branchId);
        /// <summary>
        /// 根据ID取门店管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ShopBranchManagersInfo GetShopBranchManagersById(long id);
        /// <summary>
        /// 根据用户名、密码、门店ID,取管理员信息
        /// </summary>
        /// <param name="shopBranchId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        ShopBranchManagersInfo GetShopBranchManagersByName(string userName);
        /// <summary>
        /// 分页查询门店信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        QueryPageModel<ShopBranchInfo> GetShopBranchs(ShopBranchQuery query);
        /// <summary>
        /// 取商家所有门店
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        IEnumerable<ShopBranchInfo> GetShopBranchByShopId(long shopId);
        /// <summary>
        /// 根据分店id获取分店信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<ShopBranchInfo> GetShopBranchs(IEnumerable<long> ids);
        /// <summary>
        /// 冻结门店
        /// </summary>
        /// <param name="shopBranchId"></param>
        void FreezeShopBranch(long shopBranchId);
        /// <summary>
        /// 解冻门店
        /// </summary>
        /// <param name="shopBranchId"></param>
        void UnFreezeShopBranch(long shopBranchId);

        /// <summary>
        /// 获取分店经营的商品SKU
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="shopBranchIds"></param>
        /// <param name="status">null表示所有</param>
        /// <returns></returns>
        List<ShopBranchSkusInfo> GetSkus(long shopId, IEnumerable<long> shopBranchIds, ShopBranchSkuStatus? status = ShopBranchSkuStatus.Normal, IEnumerable<string> skuids = null);
        /// <summary>
        /// 根据门店id、skuid获取门店sku信息
        /// </summary>
        /// <param name="shopBranchIds"></param>
        /// <param name="skuids"></param>
        /// <returns></returns>
        List<ShopBranchSkusInfo> GetSkusByBranchIds(IEnumerable<long> shopBranchIds, IEnumerable<string> skuids = null);
        /// <summary>
        /// 根据skuid取门店SKU
        /// </summary>
        /// <param name="skuIds"></param>
        /// <returns></returns>
        List<ShopBranchSkusInfo> GetSkusByIds(long shopBranchId, IEnumerable<string> skuIds);
        /// <summary>
        /// 添加门店sku
        /// </summary>
        /// <param name="infos"></param>
        void AddSkus(IEnumerable<ShopBranchSkusInfo> infos);
        /// <summary>
        /// 设置门店sku库存
        /// </summary>
        /// <param name="shopBranchId"></param>
        /// <param name="skuIds"></param>
        void SetStock(long shopBranchId, IEnumerable<string> skuIds, IEnumerable<int> stock);
        /// <summary>
        /// 设置门店商品库存
        /// </summary>
        /// <param name="shopBranchId"></param>
        /// <param name="pids"></param>
        /// <param name="stock"></param>
        void SetProductStock(long shopBranchId, IEnumerable<long> pids, int stock);
        /// <summary>
        /// 增加库存
        /// </summary>
        /// <param name="shopBranchId"></param>
        /// <param name="skuIds"></param>
        /// <param name="stock"></param>
        void AddStock(long shopBranchId, IEnumerable<string> skuIds, IEnumerable<int> stock);
        /// <summary>
        /// 增加门店商品库存
        /// </summary>
        /// <param name="shopBranchId"></param>
        /// <param name="pids"></param>
        /// <param name="stock"></param>
        void AddProductStock(long shopBranchId, IEnumerable<long> pids, int stock);
        /// <summary>
        /// 减少库存
        /// </summary>
        /// <param name="shopBranchId"></param>
        /// <param name="skuIds"></param>
        /// <param name="stock"></param>
        void ReduceStock(long shopBranchId, IEnumerable<string> skuIds, IEnumerable<int> stock);
        /// <summary>
        /// 减少门店商品库存
        /// </summary>
        /// <param name="shopBranchId"></param>
        /// <param name="pids"></param>
        /// <param name="stock"></param>
        void ReduceProductStock(long shopBranchId, IEnumerable<long> pids, int stock);
        /// <summary>
        /// 设置门店SKU状态
        /// </summary>
        /// <param name="shopBranchId"></param>
        /// <param name="pIds"></param>
        /// <param name="status"></param>
        void SetBranchProductStatus(long shopBranchId, IEnumerable<long> pIds, ShopBranchSkuStatus status);
        /// <summary>
        /// 设置门店SKU状态
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="status"></param>
        void SetBranchProductStatus(long productId, ShopBranchSkuStatus status);
        /// <summary>
        /// 搜索门店商品
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        QueryPageModel<ProductInfo> SearchProduct(ShopBranchProductQuery search);
        /// <summary>
        /// 搜索门店是否存在该商品
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        bool CheckProductIsExist(long shopBranchId, long productId);
        /// <summary>
        /// 查询门店的sku
        /// </summary>
        /// <param name="shopBranchId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        IEnumerable<ShopBranchSkusInfo> SearchShopBranchSkus(long shopBranchId, DateTime? startDate, DateTime? endDate);
        
        /// <summary>
        /// 获取周边门店-分页
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        QueryPageModel<ShopBranchInfo> GetNearShopBranchs(ShopBranchQuery query);
        /// <summary>
        /// 搜索周边门店-分页
        /// </summary>
        /// <param name="search"></param>
        /// <returns>关键字搜索</returns>
        QueryPageModel<ShopBranchInfo> SearchNearShopBranchs(ShopBranchQuery search);
        /// <summary>
        /// 搜索周边门店-分页
        /// </summary>
        /// <param name="search"></param>
        /// <returns>标签搜索</returns>
        QueryPageModel<ShopBranchInfo> TagsSearchNearShopBranchs(ShopBranchQuery search);
        /// <summary>
        /// 根据商品搜索周边门店-分页
        /// </summary>
        /// <param name="search"></param>
        /// <returns>标签搜索</returns>
        QueryPageModel<ShopBranchInfo> StoreByProductNearShopBranchs(ShopBranchQuery search);
        
        QueryPageModel<ShopBranchInfo> GetShopBranchsAll(ShopBranchQuery query);
        double GetLatLngDistancesFromAPI(string fromLatLng, string latlng);
        /// <summary>
        /// 搜索门店-不分页
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        List<ShopBranchInfo> GetShopBranchsNoPager(ShopBranchQuery query);
        /// <summary>
        /// 获取门店配送范围在同一区域的门店
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        QueryPageModel<ShopBranchInfo> GetArealShopBranchsAll(int areaId, int shopId, float latitude, float longitude);
        /// <summary>
        /// 自动分配订单到门店
        /// </summary>
        /// <param name="query"></param>
        /// <param name="skuIds"></param>
        /// <param name="counts"></param>
        /// <returns></returns>
        ShopBranchInfo GetAutoMatchShopBranch(ShopBranchQuery query, string[] skuIds, int[] counts);
        /// <summary>
        /// 获取代理商品的门店编号集
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        List<long> GetAgentShopBranchIds(long productId);

        /// <summary>
        /// 推荐门店
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool RecommendShopBranch(long[] ids);

        /// <summary>
        /// 推荐门店排序
        /// </summary>
        /// <param name="oriShopBranchId"></param>
        /// <param name="newShopBranchId"></param>
        /// <returns></returns>
        bool RecommendChangeSequence(long oriShopBranchId, long newShopBranchId);

        /// <summary>
        /// 取消推荐门店
        /// </summary>
        /// <param name="shopBranchId">门店ID</param>
        /// <returns></returns>
        bool ResetShopBranchRecommend(long shopBranchId);

        #region 门店标签
        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        List<ShopBranchTagInfo> GetAllShopBranchTagInfo();
        /// <summary>
        /// 新增标签
        /// </summary>
        /// <param name="shopBranchTagInfo"></param>
        void AddShopBranchTagInfo(ShopBranchTagInfo shopBranchTagInfo);
        /// <summary>
        /// 修改标签
        /// </summary>
        /// <param name="shopBranchTagInfo"></param>
        /// <returns></returns>
        bool UpdateShopBranchTagInfo(ShopBranchTagInfo shopBranchTagInfo);
        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="ShopBranchTagInfoId"></param>
        /// <returns></returns>
        bool DeleteShopBranchTagInfo(long shopBranchTagInfoId);
        /// <summary>
        /// 获取门店所有标签
        /// </summary>
        /// <param name="shopBranchId"></param>
        /// <returns></returns>
        List<ShopBranchTagInfo> GetShopBranchTagsTitle(long shopBranchId);
        /// <summary>
        /// 批量设置门店标签
        /// </summary>
        /// <param name="shopBranchIds"></param>
        /// <param name="shopBranchTagIds"></param>
        void AddShopBranchInTagInfo(long[] shopBranchIds, long[] shopBranchTagIds);
        /// <summary>
        /// 删除门店所有标签
        /// </summary>
        /// <param name="shopBranchIds"></param>
        void DeleteShopBranchInTagInfo(long[] shopBranchIds);
        /// <summary>
        /// 获取评分
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ShopStoreServiceMark GetServiceMark(long id);

        #endregion
    }
}
