using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsUI
{
    [Serializable]
    public class Config
    {
        public string OutlookTargetFolder { get; set; }
        public bool MarkAsReadWhenMove { get; set; }
    }
}
