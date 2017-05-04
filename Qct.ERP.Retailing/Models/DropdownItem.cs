using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Qct.ERP.Retailing
{
    public class DropdownItem
    {
        public DropdownItem() { }
        public DropdownItem(string value):this(value,value)
        {
        }
        public DropdownItem(string value, string text)
        {
            this.Value = value;
            this.Text = text;
            this.IsSelected = false;
        }
        public DropdownItem(string value, string text,bool isSelected):this(value,text)
        {
            this.IsSelected = isSelected;
        }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("selected")]
        public bool IsSelected { get; set; }
    }
}
