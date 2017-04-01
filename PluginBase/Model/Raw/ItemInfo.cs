using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDevelop.Model.Raw
{
    public partial class ItemInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Icon { get; set; }

        public ICollection<ItemNo2Info> ItemNo2Info { get; set; } = new List<ItemNo2Info>();
    }
}
