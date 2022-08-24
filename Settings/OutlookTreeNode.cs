using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsUI
{
    public class OutlookTreeNode
    {
        public string Name { get; set; }

        public List<OutlookTreeNode> Children;
    }
}
