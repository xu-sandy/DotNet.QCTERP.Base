using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Qct.Infrastructure.Helpers;
using Qct.Infrastructure.Extensions;

namespace Qct.ERP.Retailing
{
    public enum ButtonType
    {
        Normal,
        Menu,
    }
    public enum ActionType
    {
        Add,
        Edit,
        Delete,
        Other
    }
    public class ButtonInfo
    {
        public string Text { get; set; }
        public bool Hide { get; set; }
        public string OnClick { get; set; }
        public string IconCls { get; set; }
        public short Sort { get; set; }
        public ButtonType Type { get; set; }
        public ActionType ActionType { get; set; }
        /// <summary>
        /// 菜单类型时指定下拉的ID
        /// </summary>
        public string MenuId { get; set; }
    }
    /// <summary>
    /// 操作按钮信息
    /// </summary>
    public class OpButtonInfo
    {
        public List<ButtonInfo> Buttons { get; set; }
        public bool HideSearch { get; set; }
        public int SearchHeight { get; set; }
        public bool FirstLoadData { get; set; }
        /// <summary>
        /// 操作基本按钮
        /// </summary>
        /// <param name="buttons">操作按钮</param>
        /// <param name="hideSearch">隐藏查询栏</param>
        /// <param name="searchHeight">查询栏高度</param>
        /// <param name="firstLoadData">打开界面加载数据</param>
        public OpButtonInfo(List<ButtonInfo> buttons=null, bool hideSearch = false, int searchHeight = 43, bool firstLoadData=true)
        {
            HideSearch = hideSearch;
            SearchHeight = searchHeight;
            FirstLoadData = firstLoadData;
            Buttons = buttons;
        }
    }
    public static class Extends
    {
        /// <summary>
        /// 设置/获取工具栏基本按钮信息
        /// </summary>
        /// <param name="viewData"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static OpBtnInfo OpBtnInfo(this ViewDataDictionary viewData, OpBtnInfo info = null)
        {
            if (info == null) return viewData["opbtnInfo"] as OpBtnInfo;
            viewData["opbtnInfo"] = info;
            return null;
        }
        /// <summary>
        /// 设置/获取工具栏基本按钮信息
        /// </summary>
        /// <param name="viewData"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static OpButtonInfo OpBtnInfos(this ViewDataDictionary viewData, OpButtonInfo info = null)
        {
            if (info == null)
            {
                var btn= viewData["opbtnInfos"] as OpButtonInfo;
                if(btn!=null)
                {
                    if (btn.Buttons == null)
                        btn.Buttons = Buttons;
                    else
                    {
                        SetButton(btn, ActionType.Add);
                        SetButton(btn, ActionType.Delete);
                        SetButton(btn, ActionType.Edit);
                    }
                }
                return btn;
            }
            viewData["opbtnInfos"] = info;
            return null;
        }
        /// <summary>
        /// 对默认按钮赋默认值
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="actionType"></param>
        static void SetButton(OpButtonInfo btn, ActionType actionType)
        {
            var b = btn.Buttons.FirstOrDefault(o => o.ActionType == actionType);//传入
            var btnDef = Buttons.FirstOrDefault(o => o.ActionType == actionType);//默认
            if (b == null)
            {
                btn.Buttons.Add(btnDef);
            }
            else
            {
                if (b.OnClick.IsNullOrEmpty()) b.OnClick = btnDef.OnClick;
                if (b.Text.IsNullOrEmpty()) b.Text = btnDef.Text;
                if (b.IconCls.IsNullOrEmpty()) b.IconCls = btnDef.IconCls;
                if (b.Sort > 0) b.Sort = btnDef.Sort;
                b.Type = ButtonType.Normal;
            }
        }
        static List<ButtonInfo> Buttons=new List<ButtonInfo>() {
            new ButtonInfo(){OnClick="qcts.manager.addItem()",IconCls="icon-add",Type=ButtonType.Normal,Sort=1,ActionType=ActionType.Add,Text="新增"},
            new ButtonInfo(){OnClick="qcts.manager.removeItem()",IconCls="icon-delete",Type=ButtonType.Normal,Sort=2,ActionType=ActionType.Delete,Text="删除"},
            new ButtonInfo(){OnClick="", IconCls="icon-edit",Type=ButtonType.Normal,Sort=3,ActionType=ActionType.Edit,Text="修改"},
        };
    }
}