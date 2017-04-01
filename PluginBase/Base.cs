using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using Grabacr07.KanColleViewer.Composition;

namespace WhatToDevelop
{
    [Export(typeof(IPlugin))]
    [Export(typeof(ITool))]
    [ExportMetadata("Guid", "72590506-297e-4131-9653-4d013f036224")]
    [ExportMetadata("Title", "WhatToDevelop")]
    [ExportMetadata("Description", "明石工厂助手")]
    [ExportMetadata("Version", "2.1")]
    [ExportMetadata("Author", "Mystic Monkey")]
    public class Base : IPlugin, ITool
    {
        private MainViewModel model;

        public string Name => "WTD";

        public object View => new View.MainPanel() { DataContext = model };

        public void Initialize() { model = new MainViewModel(); }
    }
}
