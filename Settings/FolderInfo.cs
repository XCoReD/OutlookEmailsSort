using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsUI
{
    public class FolderInfo
    {
        public string Path;
        public string EntryID;

        public string Name
        {
            get
            {
                int l = Path.LastIndexOf('\\');
                return l != -1 ? Path.Substring(l + 1) : Path;
            }
        }
    }
}
