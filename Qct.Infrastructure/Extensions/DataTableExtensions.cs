using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Infrastructure.Extensions
{
    public static class DataTableExtensions
    {
        public static void MoveTo(this DataColumnCollection dcs, string columnName, int index, ref DataTable dt)
        {
            if (dt == null || columnName.IsNullOrEmpty()) return;
            DataColumn resCol = null;
            for (var idx = 0; idx < dt.Columns.Count; idx++)
            {
                var col = dt.Columns[idx];
                if (string.Equals(col.ColumnName, columnName, StringComparison.CurrentCultureIgnoreCase))
                {
                    resCol = col;
                }
            }
            if (resCol == null) return;
            var data = new DataTable();
            for (var idx = 0; idx < dt.Columns.Count; idx++)
            {
                var col = dt.Columns[idx];
                if (idx == index)
                {
                    if (data.Columns.Contains(columnName))
                        data.Columns.Remove(columnName);
                    data.Columns.Add(resCol.ColumnName, resCol.DataType);
                    idx--;
                    index = -index;
                }
                else if (!data.Columns.Contains(col.ColumnName))
                    data.Columns.Add(col.ColumnName, col.DataType);
            }
            foreach (DataRow dr in dt.Rows)
            {
                var nr = data.NewRow();
                foreach (DataColumn col in data.Columns)
                {
                    nr[col.ColumnName] = dr[col.ColumnName];
                }
                data.Rows.Add(nr);
            }
            dt = data;
        }
        /// <summary>
        /// 复制新列
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static bool CloneTo(this DataColumn dc, string columnName)
        {
            var dt = dc.Table;
            if (dt == null || columnName.IsNullOrEmpty()) return false;
            if (dt.Columns.Contains(columnName)) return false;
            dt.Columns.Add(columnName);
            foreach (DataRow dr in dt.Rows)
            {
                dr[columnName] = dr[dc];
            }
            return true;
        }
        /// <summary>
        /// 对某列赋值，不存在不抛出异常信息
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        public static void SetValue(this DataRow row, string columnName, object value)
        {
            try
            {
                row[columnName] = value;
            }
            catch { };
        }
        /// <summary>
        /// 获取某列值，不存在不抛出异常信息
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column">index或name</param>
        /// <returns></returns>
        public static object GetValue(this DataRow row, dynamic column)
        {
            try
            {
                var obj = Convert.ToString(row[column]);
                if (!(obj as string).IsNullOrEmpty())
                    return obj;
            }
            catch { };
            return DBNull.Value;
        }
    }
}
