using System;
using System.Web;
using Qct.Infrastructure.Helpers;
using Qct.Objects.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Qct.Services
{
    public class CommonService
    {
        /// <summary>
        /// 导入提示信息
        /// </summary>
        /// <param name="errls">错误列表</param>
        /// <param name="count">导入总记录数</param>
        /// <param name="data">返回客户端数据</param>
        /// <param name="isSuccess">是否成功提示</param>
        /// <returns></returns>
        public static OperateResult GenerateImportHtml(List<string> errls, int count, object data = null, bool isSuccess = true)
        {
            var op = new OperateResult();
            if (errls != null && errls.Any())
            {
                op.Descript = "<dl><dt>以下数据导入失败：</dt>{0}</dl>";
                string str = "";
                foreach (var err in errls)
                {
                    str += "<dd>" + err + "</dd>";
                }
                op.Descript = string.Format(op.Descript, str);
                var scount = count - errls.Count;
                var ecount = errls.Count;
                if (errls.Any(o => o.Contains("异常")))
                {
                    scount = 0;
                    ecount = count;
                    errls.RemoveAll(o => o.Contains("异常"));
                }
                var html = "<ul><li>{2}导入{0}条数据,余{1}条导入失败!</li><li><a href=\"javascript:void(0)\" onclick=\"viewErr()\">查看失败记录!</a></li></ul>";
                op.Message = string.Format(html, scount, "<font color='red'>" + ecount + "</font>", isSuccess ? "成功" : "可");
                op.Successed = false;
            }
            else
            {
                op.Message = "<ul><li>成功导入" + count + "条数据!</li></ul>";
                op.Successed = true;
            }
            op.Message = System.Web.HttpUtility.UrlEncode(op.Message);
            op.Descript = System.Web.HttpUtility.UrlEncode(op.Descript);
            op.Data = data;
            return op;
        }
    }
}
