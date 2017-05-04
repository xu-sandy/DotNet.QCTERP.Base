using Qct.Objects.Entities;
using Qct.Objects.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Qct.IServices
{
    public interface IImportSetService
    {
        OperateResult ImportSet(ImportSet obj, HttpFileCollectionBase httpFiles, string fieldName, string columnName, ref Dictionary<string, char> fieldCols, ref System.Data.DataTable dt);
        ImportSet GetByName(string tableName);
    }
}
