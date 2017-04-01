using Grabacr07.KanColleWrapper.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WhatToDevelop.Model
{
    public class ViewItem : Raw.ItemInfo, INotifyPropertyChanged
    {
        /// <summary>
        /// 突出显示
        /// </summary>
        public bool Favorite
        {
            get { return MainViewModel.Current.Favorite.Contains(Name); }
            set { MainViewModel.Current.SaveFavorite(value); }
        }

        /// <summary>
        /// 显示该装备的日期
        /// </summary>
        public IEnumerable<int> Weekdays => ItemNo2Info.SelectMany(i => i.Weekdays.Split(',').Select(int.Parse)).Distinct();

        /// <summary>
        /// 改修目标
        /// </summary>
        public List<ViewTarget> Targets { get; private set; } = new List<ViewTarget>();

        public bool OtherTarget { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged = (se, ev) => { };

        public ViewItem(Raw.ItemInfo item)
        {
            SlotItemIconType whatever;

            Name = item.Name;
            ItemNo2Info = item.ItemNo2Info;
            Type = item.Type;
            Icon = Enum.TryParse(item.Icon, out whatever) ? item.Icon : SlotItemIconType.Unknown.ToString();

            // 分组
            Targets.AddRange(ItemNo2Info
                .GroupBy(i => i.Target)
                .Select(g =>
                {
                    int CurrentWeekday = Convert.ToInt32(MainViewModel.Current.Date.DayOfWeek);
                    ViewTarget tar = new ViewTarget();
                    tar.Target = g.Key;
                    tar.Weekdays = g.SelectMany(j => j.Weekdays.Split(',').Select(int.Parse)).Distinct();
                    tar.Enabled = tar.Weekdays.Contains(CurrentWeekday);
                    tar.No2 = string.Join(" ", g
                        .Where(j => j.Weekdays.Contains(CurrentWeekday.ToString()))
                        .Select(j => j.Name));
                    tar.Costs = g.First().Cost.Split(',');
                    return tar;
                }));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Targets)));

            // 检测其它改修方案
            OtherTarget = ItemNo2Info.Any(j => !Targets.Select(k => k.Target).Contains(j.Target));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(OtherTarget)));
        }
    }
}
