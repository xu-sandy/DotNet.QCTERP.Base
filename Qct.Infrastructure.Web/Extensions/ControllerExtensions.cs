using Qct.Objects.ValueObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Qct.Infrastructure.Web.Extensions
{
    public static class ControllerExtensions
    {
        public static JsonResult ToDataGrid(this Controller controller, object list, int rowCount, object footers = null)
        {
            list = list ?? new ArrayList();
            object obj = null;
            if (footers == null)
                obj = new { total = rowCount, rows = list };
            else
                obj = new { total = rowCount, rows = list, footer = footers };
            return new JsonNetResult(obj);
        }


        public static JsonResult ToJsonResult(this Controller controller, object data)
        {
            return new JsonNetResult(data);
        }
        public static JsonResult ToJsonResult(this Controller controller, object data, string contentType)
        {
            return new JsonNetResult(data, contentType);
        }

        public static JsonResult ToJsonResult(this Controller controller, object data, JsonRequestBehavior behavior, string contentType, Encoding encoding)
        {
            return new JsonNetResult(data, behavior, contentType, encoding);
        }
        /// <summary>
        /// 操作结果（默认成功）
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="op">自定义结果</param>
        /// <returns></returns>
        public static JsonResult ToJsonOperateResult(this ControllerBase controller, OperateResult op=null)
        {
            if (op == null) op = OperateResult.Success();
            return new JsonNetResult(op);
        }

        /// <summary>
        /// 插入一条空或回显默认项
        /// </summary>
        /// <param name="ies"></param>
        /// <param name="emptyTitle">显示名称，请选择或全部</param>
        /// <param name="emptyValue">对应值</param>
        /// <param name="selectValue">选中项</param>
        /// <returns></returns>
        public static IList<SelectListItem> ToSelectTitle(this Controller controller, IEnumerable<SelectListItem> ies, string emptyTitle = "",
            string emptyValue = "",object selectValue = null)
        {
            if (ies == null) return null;
            var list = ies.ToList();
            if (!string.IsNullOrWhiteSpace(emptyTitle))
                list.Insert(0, new SelectListItem() { Text = emptyTitle, Value = emptyValue, Selected = true });
            if (selectValue != null)
            {
                var obj = list.FirstOrDefault(o => o.Value == selectValue.ToString());
                if (obj != null)
                {
                    list.ForEach(o => o.Selected = false);
                    obj.Selected = true;
                }
            }
            return list;
        }
        /// <summary>
        /// 把枚举转成下拉数据
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="type"></param>
        /// <param name="emptyTitle"></param>
        /// <param name="emptyValue"></param>
        /// <param name="selectValue"></param>
        /// <returns></returns>
        public static IList<SelectListItem> EnumToSelectList(this Controller controller, Type type, string emptyTitle = "", string emptyValue="", byte? selectValue = null)
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            if (!string.IsNullOrWhiteSpace(emptyTitle))
                list.Insert(0, new SelectListItem() { Text = emptyTitle, Value = emptyValue, Selected = true });
            string[] t = Enum.GetNames(type);
            foreach (string f in t)
            {
                var text = f;
                var obj = Enum.Parse(type, f);
                var field = type.GetField(f);
                object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (objs != null && objs.Length > 0)
                {
                    System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
                    text = da.Description;
                }
                var d = Convert.ToInt32(Convert.ChangeType(obj, type));
                var item = new SelectListItem() { Text = text, Value = d.ToString() };
                if (selectValue.HasValue)
                    item.Selected = d == selectValue.Value;
                list.Add(item);
            }
            return list;
        }
    }
}
