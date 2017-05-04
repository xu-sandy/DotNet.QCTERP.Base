using Newtonsoft.Json;
using Qct.Objects.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Objects.ValueObjects
{
    [NotMapped]
    public class SysLimitsModel: SysLimits
    {
        public string PTitle { get; set; }
        public int PDepth { get; set; }
        public string ItemsStr { get; set; }
        [JsonProperty("children")]
        public List<SysLimitsModel> Childs { get; set; }
        public bool HasChild { get; set; }
        [JsonProperty("LastChildren")]
        public List<SysLimitsModel> LastChilds { get; set; }
    }
}
