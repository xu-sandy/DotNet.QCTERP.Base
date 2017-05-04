using Qct.ISevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qct.Objects.Entities;
using Qct.Objects.ValueObjects;
using System.Data;
using System.Web;
using Qct.IRepository;
using Qct.Infrastructure.Helpers;
using Qct.Infrastructure.Extensions;
using Qct.Infrastructure.Json;
using Qct.IServices;
using Qct.Infrastructure.Export;

namespace Qct.Services
{
    public class ImportSetService : IImportSetService
    {
        readonly IImportSetRepository _importSetRepository;
        public ImportSetService(IImportSetRepository importSetRepository)
        {
            _importSetRepository = importSetRepository;
        }
        public ImportSet GetByName(string tableName)
        {
            return _importSetRepository.GetByName(tableName);
        }

        public OperateResult ImportSet(ImportSet obj, HttpFileCollectionBase httpFiles, string fieldName, string columnName, ref Dictionary<string, char> fieldCols, ref DataTable dt)
        {
            if (httpFiles.Count <= 0 || httpFiles[0].ContentLength <= 0) return OperateResult.Fail("请先选择Excel文件");
            var stream = httpFiles[0].InputStream;
            var ext = httpFiles[0].FileName.Substring(httpFiles[0].FileName.LastIndexOf("."));
            if (!(ext.Equals(".xls", StringComparison.CurrentCultureIgnoreCase) ||
                ext.Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase)))
            {
                return OperateResult.Fail("请先选择Excel文件");
            }

            var path = "";
            var fullPath = FileHelper.SaveAttachPath(ref path, "temps");
            httpFiles[0].SaveAs(fullPath + httpFiles[0].FileName);
            fieldCols = fieldCols ?? new Dictionary<string, char>();
            if (!fieldName.IsNullOrEmpty() && !columnName.IsNullOrEmpty())
            {
                var fields = fieldName.Split(',');
                var columns = columnName.Split(',');
                if (fields.Length != columns.Length)
                {
                    return OperateResult.Fail("配置的字段和列数不一致");
                }
                for (var i = 0; i < fields.Length; i++)
                {
                    if (columns[i].IsNullOrEmpty()) continue;
                    fieldCols[fields[i]] = Convert.ToChar(columns[i]);
                }
                //if (fieldCols.Values.Distinct().Count() != fieldCols.Values.Count())
                //{
                //    op.Message = "配置的列存在重复!";
                //    return op;
                //}
                obj.FieldJson = fieldCols.Select(o => new { o.Key, o.Value }).ToJson();
            }
            _importSetRepository.AddOrUpdate(obj);
            dt = new ExportExcel().ToDataTable(stream, minRow: obj.MinRow, maxRow: obj.MaxRow.GetValueOrDefault());
            if (dt == null || dt.Rows.Count <= 0)
            {
                return OperateResult.Fail("无数据，无法导入!");
            }
            #region 去掉空格
            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.DataType == typeof(string))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!(dr[dc] is DBNull))
                        {
                            dr[dc] = dr[dc].ToString().Trim();
                        }
                    }
                }
            }
            #endregion
            #region 允许配置在同一列
            var cols = fieldCols.GroupBy(o => o.Value).Where(o => o.Count() > 1).ToList();//取重复列
            foreach (var item in cols)
            {
                System.Diagnostics.Debug.WriteLine(item.Key);//重复列value
                var idx = Convert.ToInt32(item.Key) - 65;
                foreach (var subitem in item)
                {
                    System.Diagnostics.Debug.WriteLine(subitem.Key);//重复列key
                    var lastValue = Convert.ToChar(fieldCols.Values.OrderBy(o => o).LastOrDefault() + 1);
                    if (dt.Columns[idx].CloneTo(subitem.Key))
                        fieldCols[subitem.Key] = lastValue;
                }
            }
            #endregion
            return OperateResult.Success();
        }
    }
}
