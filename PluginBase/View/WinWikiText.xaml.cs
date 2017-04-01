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
using System.Windows.Shapes;

namespace WhatToDevelop.View
{
    /// <summary>
    /// WinWikiText.xaml 的交互逻辑
    /// </summary>
    public partial class WinWikiText : Window
    {
        private WikiCreeper creeper = new WikiCreeper();

        public WinWikiText()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            creeper.CreateList(txtPage.Text);
            creeper.WriteFile();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtPage.Text =
                "请按如下步骤操作：" + Environment.NewLine +
                "1. 打开 Wiki 页面" + Environment.NewLine +
                "2. 点击右键，查看原代码" + Environment.NewLine +
                "3. 全选，复制" + Environment.NewLine +
                "4. 粘贴到此处" + Environment.NewLine +
                "5. 单击下方按钮";
        }

        private void txtPage_GotFocus(object sender, RoutedEventArgs e)
        {
            txtPage.Clear();
        }
    }
}
