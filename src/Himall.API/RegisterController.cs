﻿using Himall.Application;
using Himall.Core;
using Himall.Core.Helper;
using Himall.IServices;
using Himall.Model;
using Himall.Web.Framework;
using System;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Mvc;
using Himall.API.Model.ParamsModel;
using Himall.CommonModel;

namespace Himall.API
{
    public class RegisterController : BaseApiController
    {
        private static string _encryptKey = Guid.NewGuid().ToString("N");
        [System.Web.Mvc.HttpPost]
        public object PostRegisterUser(RegisterUserModel user)
        {
            dynamic result = new Result();
            try
            {
                var email = "";
                //普通注册
                if (user.userName != null && user.password != null && user.userName != "" && user.password != "")
                {
                    string userName = user.userName;
                    string password = user.password;
                    email = user.email;
                    string code = user.code;

                    var pluginId = "";

                    if (!string.IsNullOrEmpty(email) && Core.Helper.ValidateHelper.IsEmail(email))
                    {
                        pluginId = "Himall.Plugin.Message.Email";

                        var cache = CacheKeyCollection.MemberPluginCheck(email, pluginId);
                        var cacheCode = Core.Cache.Get<string>(cache);
                        if (cacheCode == null || cacheCode != code)
                        {
                            return new { success = false, ErrorMsg = "验证码输入错误或者已经超时" };
                        }
                    }


                    Regex reg = new Regex("^[a-zA-Z0-9_\u4e00-\u9fa5]+$");
                    if (!reg.IsMatch(userName) || userName.Length < 4 || userName.Length > 20)
                    {
                        throw new HimallException("用户名由4-20个中文英文数字字母下划线组成");
                    }

                    var member = ServiceProvider.Instance<IMemberService>.Create.Register(userName, password, string.Empty, email, 0);

                    if (member == null)
                    {
                        result = ErrorResult("注册失败", 104);
                    }
                    else
                    {
                        //手机注册直接绑定手机
                        if (Core.Helper.ValidateHelper.IsMobile(userName))
                        {
                            pluginId = "Himall.Plugin.Message.SMS";
                            member.CellPhone = userName;
                            ServiceProvider.Instance<IMemberService>.Create.UpdateMember(member);
                            ServiceProvider.Instance<IMessageService>.Create.UpdateMemberContacts(new MemberContactsInfo() { Contact = userName, ServiceProvider = pluginId, UserId = member.Id, UserType = MemberContactsInfo.UserTypes.General });
                        }

                        //注册赠送优惠券
                        int num = this.RegisterSendCoupon(member.Id, member.UserName);

                        result.success = true;
                        result.UserId = member.Id.ToString();
                        result.CouponNum = num;
                        string memberId = UserCookieEncryptHelper.Encrypt(member.Id, "Mobile");
                        //WebHelper.SetCookie(CookieKeysCollection.HIMALL_USER_KEY(platformType), memberId, DateTime.MaxValue);
                    }
                }
                //信任登录并且不绑定，后台给一个快速注册
                else
                {
                    string username = DateTime.Now.ToString("yyMMddHHmmssffffff");
                    var member = ServiceProvider.Instance<IMemberService>.Create.QuickRegister(username, string.Empty, user.oauthNickName, user.oauthType, user.oauthOpenId, user.unionid, user.sex, user.headimgurl, MemberOpenIdInfo.AppIdTypeEnum.Normal);

                    //注册赠送优惠券
                    int num = this.RegisterSendCoupon(member.Id, member.UserName);

                    string memberId = UserCookieEncryptHelper.Encrypt(member.Id, "Mobile");
                    //WebHelper.SetCookie(CookieKeysCollection.HIMALL_USER_KEY(platformType), memberId);
                    result.success = true;
                    result.UserId = member.Id.ToString();
                    result.CouponNum = num;
                }


            }
            catch (Exception ex)
            {
                result = ErrorResult(ex.Message, 104);
            }
            return result;
        }
        #region 重写方法
        protected override bool CheckContact(string contact, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (!string.IsNullOrWhiteSpace(contact))
            {
                var userMenberInfo = Application.MemberApplication.GetMemberByContactInfo(contact);
                if (userMenberInfo != null)
                {
                    errorMessage = "手机或邮箱号码已经存在";
                    Cache.Insert(_encryptKey + contact, string.Format("{0}:{1:yyyyMMddHHmmss}", userMenberInfo.Id, userMenberInfo.CreateDate), DateTime.Now.AddHours(1));
                    return false;
                }
                else
                {//不存在，可以注册
                    return true;
                }
            }
            return false;
        }
        #endregion
        /// <summary>
        /// 注册赠送优惠券
        /// </summary>
        /// <returns></returns>
        private int RegisterSendCoupon(long Id, string UserName)
        {
            Log.Info("注册赠送优惠券方法进入");
            var iCouponSendByRegisterService = ServiceProvider.Instance<ICouponSendByRegisterService>.Create;
            var iCouponService = ServiceProvider.Instance<ICouponService>.Create;
            return CouponApplication.RegisterSendCoupon(Id, UserName);
        }

        public object GetRegisterType()
        {
            var siteSetting = ServiceProvider.Instance<ISiteSettingService>.Create.GetSiteSettings();
            return new { success = true, RegisterType = siteSetting.RegisterType, MobileVerifOpen = siteSetting.MobileVerifOpen, EmailVerifOpen = siteSetting.EmailVerifOpen, RegisterEmailRequired = siteSetting.RegisterEmailRequired };
        }
    }
}
