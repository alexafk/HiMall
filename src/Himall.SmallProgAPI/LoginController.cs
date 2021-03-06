﻿using Himall.Application;
using Himall.CommonModel;
using Himall.Core.Helper;
using Himall.IServices;
using Himall.Model;
using Himall.SmallProgAPI.Model;
using Himall.Web.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http.Results;

namespace Himall.SmallProgAPI
{
    public class LoginController : BaseApiController
    {
        /// <summary>
        /// 根据OpenId判断是否有账号，根据OpenId进行登录
        /// </summary>
        /// <returns></returns>
        public JsonResult<Result<dynamic>> GetLoginByOpenId(string openId = "")
        {
            //string oauthType = "Himall.Plugin.OAuth.WeiXin";//默认小程序微信端登录
            string unionid = "";
            var wxuserinfo = ApiHelper.GetAppletUserInfo(Request);
            if (wxuserinfo != null)
            {
                unionid = wxuserinfo.unionId;
            }
            if (!string.IsNullOrEmpty(openId))
            {
                UserMemberInfo member = new UserMemberInfo();
                if (!string.IsNullOrWhiteSpace(unionid))
                {
                    member = Application.MemberApplication.GetMemberByUnionIdAndProvider(SmallProgServiceProvider, unionid) ?? new UserMemberInfo();
                }

                if (member.Id == 0)
                    member = Application.MemberApplication.GetMemberByOpenId(SmallProgServiceProvider, openId) ?? new UserMemberInfo();
                if (member.Id > 0)
                {
                    //信任登录并且已绑定             
                    string memberId = UserCookieEncryptHelper.Encrypt(member.Id, CookieKeysCollection.USERROLE_USER);
                    return GetMember(member, openId);
                }
                else
                {
                    //信任登录未绑定
                    return Json(ErrorResult<dynamic>("未绑定商城帐号"));
                }
            }
            return Json(ErrorResult<dynamic>(string.Empty));
        }
        /// <summary>
        ///账号密码登录
        /// </summary>
        /// <returns></returns>
        public JsonResult<Result<dynamic>> GetLoginByUserName(string openId = "", string userName = "", string password = "", string nickName = "")
        {
            if (!string.IsNullOrEmpty(openId) && !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                UserMemberInfo member = null;

                try
                {
                    member = ServiceProvider.Instance<IMemberService>.Create.Login(userName, password);
                }
                catch (Exception ex)
                {
                    return Json(ErrorResult<dynamic>(ex.Message));
                }
                if (member == null)
                {
                    return Json(ErrorResult<dynamic>("用户名或密码错误"));
                }
                else
                {
                    if (member != null)
                    {
                        bool IsUpdate = true;
                        //如果不是一键登录的 则绑定openId
                        if (!string.IsNullOrEmpty(openId))
                        {
                            //UserMemberInfo memberopen = ServiceProvider.Instance<IMemberService>.Create.GetMemberByOpenId(SmallProgServiceProvider, openId);
                            //if (memberopen == null)
                            //{
                            //    IsUpdate = true;
                            //    //新增openId记录

                            //}
                            //else
                            //{
                            //    if (memberopen.Id != member.Id)
                            //    {
                            MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo()
                            {
                                UserId = member.Id,
                                OpenId = openId,
                                ServiceProvider = SmallProgServiceProvider,
                                AppIdType = Himall.Model.MemberOpenIdInfo.AppIdTypeEnum.Normal,
                                UnionId = string.Empty
                            };
                            MemberApplication.UpdateOpenIdBindMember(memberOpenIdInfo);
                            //    }
                            //}
                        }

                        string memberId = UserCookieEncryptHelper.Encrypt(member.Id, CookieKeysCollection.USERROLE_USER);
                        var prom = DistributionApplication.GetPromoterByUserId(member.Id);
                        return GetMember(member, openId);
                    }
                }
            }
            return Json(ErrorResult<dynamic>(string.Empty));
        }
        /// <summary>
        ///一键登录
        /// </summary>
        /// <returns></returns>
        public JsonResult<Result<dynamic>> GetQuickLogin(string openId = "", string nickName = "", string headImage = "")
        {
            var wxuserinfo = ApiHelper.GetAppletUserInfo(Request);
            string unionid = string.Empty;
            if (wxuserinfo != null)
            {
                unionid = wxuserinfo.unionId;
            }
            string unionopenid = "";
            if (!string.IsNullOrEmpty(openId))
            {
                string username = DateTime.Now.ToString("yyMMddHHmmssffffff");
                var member = ServiceProvider.Instance<IMemberService>.Create.QuickRegister(username, string.Empty, nickName, SmallProgServiceProvider, openId, unionid, unionopenid: unionopenid, headImage: headImage);
                //string memberId = UserCookieEncryptHelper.Encrypt(member.Id, CookieKeysCollection.USERROLE_USER);
                //var prom = DistributionApplication.GetPromoterByUserId(member.Id);
                return GetMember(member, openId);

            }
            return Json(ErrorResult<dynamic>(string.Empty));
        }
        /// <summary>
        ///退出登录
        /// </summary>
        /// <returns></returns>
        public JsonResult<Result<int>> GetProcessLogout(string openId)
        {
            if (!string.IsNullOrEmpty(openId))
            {
                var member = Application.MemberApplication.GetMemberByOpenId(SmallProgServiceProvider, openId);
                if (member == CurrentUser)
                {
                    var cacheKey = WebHelper.GetCookie(CookieKeysCollection.HIMALL_USER);
                    if (!string.IsNullOrWhiteSpace(cacheKey))
                    {
                        //_iMemberService.DeleteMemberOpenId(userid, string.Empty);
                        WebHelper.DeleteCookie(CookieKeysCollection.HIMALL_USER);
                        WebHelper.DeleteCookie(CookieKeysCollection.SELLER_MANAGER);
                        //记录主动退出符号
                        WebHelper.SetCookie(CookieKeysCollection.HIMALL_ACTIVELOGOUT, "1", DateTime.MaxValue);
                        return JsonResult<int>();
                    }
                }
            }
            return JsonResult<int>();
        }

        /// <summary>
        /// 获取客服电话
        /// </summary>
        /// <param name="context"></param>
        public JsonResult<Result<dynamic>> GetServicePhone()
        {
            SiteSettingsInfo siteSettings = SiteSettingApplication.GetSiteSettings();
            return JsonResult<dynamic>(new
            {
                ServicePhone = siteSettings.SitePhone
            });
        }
        /// <summary>
        /// 获取首页数据
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult<Result<dynamic>> GetIndexData(string openId = "", int pageIndex = 10, int pageSize = 1)
        {
            //CheckUserLogin();
            UserMemberInfo member = CurrentUser;
            SiteSettingsInfo sitesetting = SiteSettingApplication.GetSiteSettings();
            string homejson = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority + "/AppletHome/data/default.json";

            long vidnumber = sitesetting.XcxHomeVersionCode;
            return JsonResult<dynamic>(new
            {
                HomeTopicPath = homejson,
                Vid = vidnumber,
                QQMapKey = CommonConst.QQMapKey
            });
        }
        /// <summary>
        /// 检查版本号
        /// </summary>
        /// <param name="context"></param>
        public JsonResult<Result<int>> GetInitVeCode(string vid)
        {
            if (string.IsNullOrEmpty(vid))
            {
                return Json(ErrorResult<int>("版本号不允许为空", 100, 100));
            }

            SiteSettingsInfo sitesetting = SiteSettingApplication.GetSiteSettings();
            long xcxvid = sitesetting.XcxHomeVersionCode;

            if (xcxvid > long.Parse(vid))
            {
                return Json(ErrorResult("版本需要更新", 101, 101));
            }
            else
            {
                return JsonResult(100, "版本不需要更新", 100);
            }
        }

        public JsonResult<Result<List<dynamic>>> GetIndexProductData(string openId = "", int pageIndex = 1, int pageSize = 10)
        {
            //CheckUserLogin();
            var homeProducts = ServiceProvider.Instance<IWXSmallProgramService>.Create.GetWXSmallHomeProducts().OrderBy(item => item.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            decimal discount = 1M;
            long SelfShopId = 0;
            ShoppingCartInfo CartInfo = new ShoppingCartInfo();
            var ids = homeProducts.Select(d => d.Id).ToArray();
            List<dynamic> productList = new List<dynamic>();
            List<ShoppingCartItem> cartitems = new List<ShoppingCartItem>();
            long userId = 0;
            if (CurrentUser != null)
            {
                userId = CurrentUser.Id;
                discount = CurrentUser.MemberDiscount;
                var shopInfo = ShopApplication.GetSelfShop();
                SelfShopId = shopInfo.Id;
                CartInfo = ServiceProvider.Instance<ICartService>.Create.GetCart(CurrentUser.Id);
                cartitems = CartApplication.GetCartQuantityByIds(CurrentUser.Id, ids);
            }
            var limit = LimitTimeApplication.GetLimitProducts();
            //var fight = FightGroupApplication.GetFightGroupPrice();

            foreach (var item in homeProducts)
            {
                long activeId = 0;
                int activetype = 0;
                item.ImagePath = Core.HimallIO.GetRomoteProductSizeImage(Core.HimallIO.GetImagePath(item.ImagePath), 1, (int)Himall.CommonModel.ImageSize.Size_350);
                if (item.ShopId == SelfShopId)
                    item.MinSalePrice = item.MinSalePrice * discount;
                var limitBuy = ServiceProvider.Instance<ILimitTimeBuyService>.Create.GetLimitTimeMarketItemByProductId(item.Id);
                if (limitBuy != null)
                {
                    item.MinSalePrice = limitBuy.MinPrice;
                    activeId = limitBuy.Id;
                    activetype = 1;
                }
                int quantity = 0;
                quantity = cartitems.Where(d => d.ProductId == item.Id).Sum(d => d.Quantity);
                //火拼
                //if (activeInfo != null)
                //{
                //    item.MinSalePrice = activeInfo.MiniGroupPrice;
                //    activeId = activeInfo.Id;
                //    activetype = 2;
                //}
                long stock = 0;

                var productInfo = ServiceProvider.Instance<IProductService>.Create.GetProduct(item.Id);
                if (productInfo != null)
                {
                    stock = productInfo.SKUInfo.Sum(x => x.Stock);
                    if (productInfo.MaxBuyCount > 0)
                    {
                        stock = productInfo.MaxBuyCount;
                    }
                }
                if (productInfo.AuditStatus == ProductInfo.ProductAuditStatus.Audited)
                {
                    var ChoiceProducts = new
                    {
                        ProductId = item.Id,
                        ProductName = item.ProductName,
                        SalePrice = item.MinSalePrice.ToString("0.##"),
                        ThumbnailUrl160 = item.ImagePath,
                        MarketPrice = item.MarketPrice.ToString("0.##"),
                        CartQuantity = quantity,
                        HasSKU = item.HasSKU,
                        SkuId = GetSkuIdByProductId(item.Id),//d.Himall_Products d.Field<string>("SkuId"),
                        ActiveId = activeId,
                        ActiveType = activetype,//获取该商品是否参与活动
                        Stock = stock
                    };
                    productList.Add(ChoiceProducts);
                }
            }
            return JsonResult(productList);
        }

        private JsonResult<Result<dynamic>> GetMember(UserMemberInfo member, string openId)
        {
            var model = ServiceProvider.Instance<IMemberService>.Create.GetUserCenterModel(member.Id);
            var memgradeid = ServiceProvider.Instance<IMemberGradeService>.Create.GetMemberGradeByUserId(member.Id);
            string gradeName = model.GradeName == null ? "" : model.GradeName;
            //获取会员等待付款订单数
            int waitPayCount = Convert.ToInt32(model.WaitPayOrders);
            //获取会员待收货数量
            int waitFinishCount = Convert.ToInt32(model.WaitReceivingOrders);
            //获取会员待发货数量
            int waitSendCount = Convert.ToInt32(model.WaitDeliveryOrders);
            //获取会员待评论数量
            int waitReviewCount = Convert.ToInt32(model.WaitEvaluationOrders);
            //获取会员售后数量
            int afterSalesCount = model.RefundCount;
            //获取会员未使用的优惠券数目
            int couponsCount = model.UserCoupon;
            return JsonResult<dynamic>(new
            {
                couponsCount = couponsCount,
                picture = Core.HimallIO.GetRomoteImagePath(member.Photo),
                points = model.Intergral,
                waitPayCount = waitPayCount,
                waitSendCount = waitSendCount,
                waitFinishCount = waitFinishCount,
                waitReviewCount = waitReviewCount,
                afterSalesCount = afterSalesCount,
                realName = string.IsNullOrEmpty(member.ShowNick) ? (string.IsNullOrEmpty(member.RealName) ? member.UserName : member.RealName) : member.ShowNick,
                gradeId = memgradeid,
                gradeName = gradeName,
                UserName = member.UserName,
                UserId = member.Id,
                OpenId = openId,
                ServicePhone = Application.SiteSettingApplication.GetSiteSettings().SitePhone
            });
        }

        private string GetSkuIdByProductId(long productId = 0)
        {
            string skuId = "";
            if (productId > 0)
            {
                var Skus = ServiceProvider.Instance<IProductService>.Create.GetSKUs(productId);
                foreach (var item in Skus)
                {
                    skuId = item.Id;//取最后或默认
                }
            }
            return skuId;
        }

        public JsonResult<Result<WeiXinOpenIdModel>> GetOpenId(string appid, string secret, string js_code)
        {
            string requestUrl = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appid + "&secret=" + secret + "&js_code=" + js_code + "&grant_type=authorization_code";
            string result = "";
            var response = Himall.Core.Helper.WebHelper.GetURLResponse(requestUrl);
            using (Stream receiveStream = response.GetResponseStream())
            {

                using (StreamReader readerOfStream = new StreamReader(receiveStream, System.Text.Encoding.UTF8))
                {
                    result = readerOfStream.ReadToEnd();
                }
            }
            var openModel = JsonConvert.DeserializeObject<WeiXinOpenIdModel>(result);
            return JsonResult(openModel);
        }
    }
}
