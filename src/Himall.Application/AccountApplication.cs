﻿using Himall.CommonModel;
using Himall.Core;
using Himall.DTO;
using Himall.IServices;
using Himall.DTO.QueryModel;
using Himall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Himall.Application
{
    /// <summary>
    /// 结算相关服务应用
    /// </summary>
    public class AccountApplication
    {
        private static IAccountService _iAccountService= ObjectContainer.Current.Resolve<IAccountService>();

        public static QueryPageModel<AccountInfo> GetAccounts(AccountQuery query)
        {
            return _iAccountService.GetAccounts(query);
        }

        public static AccountInfo GetAccount(long id)
        {
            return _iAccountService.GetAccount(id);
        }
        /// <summary>
        /// 获取结算订单明细列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static QueryPageModel<AccountDetailInfo> GetAccountDetails(AccountQuery query)
        {
            return _iAccountService.GetAccountDetails(query);
        }
        /// <summary>
        /// 取服务费用
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static QueryPageModel<AccountMetaModel> GetAccountMeta(AccountQuery query)
        {
            return _iAccountService.GetAccountMeta(query);
        }
        /// <summary>
        /// 取分销佣金
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static QueryPageModel<BrokerageModel> GetBrokerageList(AccountQuery query)
        {
            return _iAccountService.GetBrokerageList(query);
        }
        /// <summary>
        /// 确认结算
        /// </summary>
        /// <param name="id"></param>
        /// <param name="managerRemark"></param>
        public static void ConfirmAccount(long id, string managerRemark)
        {
            _iAccountService.ConfirmAccount(id, managerRemark);
        }
    }
}
