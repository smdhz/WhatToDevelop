using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace WhatToDevelop.View
{
    /// <summary>
    /// WeekIndicator.xaml 的交互逻辑
    /// </summary>
    public partial class WeekIndicator : UserControl
    {
        public static readonly DependencyProperty ValuesProperty =
            DependencyProperty.Register("Values", typeof(IEnumerable<int>), typeof(WeekIndicator), new UIPropertyMetadata(RefreshUI));

        public WeekIndicator()
        {
            InitializeComponent();
        }

        public IEnumerable<int> Values
        {
            get { return (IEnumerable<int>)this.GetValue(ValuesProperty); }
            set { this.SetValue(ValuesProperty, value); }
        }

        private static void RefreshUI(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            WeekIndicator control = sender as WeekIndicator;

            List<Rectangle> weekBolcks = new List<Rectangle>() { control.rec0, control.rec1, control.rec2, control.rec3, control.rec4, control.rec5, control.rec6 };
            IEnumerable<int> weekdays = e.NewValue as IEnumerable<int>;

            foreach(Rectangle i in weekBolcks)
            {
                int index = weekBolcks.IndexOf(i);
                string key = "Fail";

                if (weekdays.Contains(index))
                {
                    if (index == Convert.ToInt32(MainViewModel.Current.Date.DayOfWeek))
                        key = "Today";
                    else if ((index == Convert.ToInt32(MainViewModel.Current.Date.AddDays(1).DayOfWeek)))
                        key = "Tomorrow";
                    else
                        key = "Available";
                }
                i.Style = control.FindResource(key) as Style;
            }
        }
    }
}
