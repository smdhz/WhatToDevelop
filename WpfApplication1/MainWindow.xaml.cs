using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WTD.Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroRadiance.UI.Controls.MetroWindow
    {
        private WhatToDevelop.MainViewModel model = new WhatToDevelop.MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "grabacr.net",
                "KanColleViewer"));
            if (!dir.Exists)
                dir.Create();
            panel.DataContext = model;
        }
    }
}
