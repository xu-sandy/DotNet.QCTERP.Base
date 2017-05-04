using System;

namespace Qct.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class ExcelAttribute : Attribute
    {
        private string _columnName;

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }
        private int _columnIndex;

        public int ColumnIndex
        {
            get { return _columnIndex; }
            set { _columnIndex = value; }
        }

        public string Title { get; set; }

        public ExcelAttribute(string columnName, int columnIndex)
        {
            _columnName = columnName;
            _columnIndex = columnIndex;
        }

        public ExcelAttribute(string title)
        {
            Title = title;
        }
    }
}
