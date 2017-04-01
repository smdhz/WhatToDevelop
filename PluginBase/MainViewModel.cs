using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WhatToDevelop
{
    public class MainViewModel : Livet.ViewModel
    {
        public static MainViewModel Current;
        private List<Model.Raw.ItemInfo> Content;

        #region 交互属性
        public ObservableCollection<Model.ViewItem> List { get; private set; } = new ObservableCollection<Model.ViewItem>();

        private Model.ViewItem _selected;

        public Model.ViewItem SelectedItem
        {
            get { return _selected; }
            set
            {
                _selected = value;
                RaisePropertyChanged();
                RaisePropertyChanged("AvailableVisibility");
            }
        }

        public bool HasError => _error != null;
        private string _error = null;
        public string ErrorDescr
        {
            get { return _error; }
            private set
            {
                _error = value;
                RaisePropertyChanged();
            }
        }

        private DateTime _date = DateTime.Today;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                RaisePropertyChanged("WeekCn");
                RaisePropertyChanged("WeekJp");
                RefreshList();
            }
        }
        public bool AvailableVisibility => SelectedItem?.OtherTarget ?? false;
        public string WeekCn
        { get { return weekCn[Convert.ToInt32(Date.DayOfWeek)]; } }
        public string WeekJp
        { get { return weekJp[Convert.ToInt32(Date.DayOfWeek)]; } }

        public List<string> Favorite { get; private set; } = new List<string>();
        #endregion

        private static readonly FileInfo file = new FileInfo(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "grabacr.net",
            "KanColleViewer",
            "WTD_Favorite.xml"));

        public MainViewModel()
        {
            Current = this;
            Task.Run(() =>
            {
                if (!file.Exists)
                    return;

                XmlSerializer se = new XmlSerializer(typeof(List<string>));
                using (FileStream fs = new FileStream(file.FullName, FileMode.Open))
                    Favorite = se.Deserialize(fs) as List<string>;
            });
            Task.Run(() => 
            { 
                using (HttpClient client = new HttpClient())
                {
                    DataContractJsonSerializer se = new DataContractJsonSerializer(typeof(Model.Raw.OdataContainer));
                    using (Stream rep = client.GetStreamAsync("https://www.smdhz.cf/odata/ItemInfoes?$expand=ItemNo2Info").Result)
                        Content = (se.ReadObject(rep) as Model.Raw.OdataContainer).value.ToList();
                }
                RefreshList();
            });
        }

        private readonly static string[] weekCn = new string[7] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
        private readonly static string[] weekJp = new string[7] { "日曜日", "月曜日", "火曜日", "水曜日", "木曜日", "金曜日", "土曜日" };
        
        /// <summary>
        /// 刷新道具列表
        /// </summary>
        public void RefreshList()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => List.Clear());

            foreach (Model.ViewItem i in Content
                .Where(j => j.ItemNo2Info.SelectMany(k => k.Weekdays.Split(',').Select(int.Parse)).Contains(Convert.ToInt32(Date.DayOfWeek)))
                .Select(j => new Model.ViewItem(j))
                .OrderByDescending(j => j.Favorite)
                .ThenBy(j => Enum.Parse(typeof(Grabacr07.KanColleWrapper.Models.SlotItemIconType), j.Icon))
                .ToList())
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() => List.Add(i));
            }
        }
        
        /// <summary>
        /// 保持偏好信息
        /// </summary>
        /// <param name="value"></param>
        public void SaveFavorite(bool value)
        {
            if (value)
                Favorite.Add(SelectedItem.Name);
            else
                Favorite.Remove(SelectedItem.Name);

            if (!file.Directory.Exists)
                file.Directory.Create();
            XmlSerializer se = new XmlSerializer(typeof(List<string>));
            using (FileStream fs = new FileStream(file.FullName, FileMode.Create))
                se.Serialize(fs, Favorite);

            RefreshList();
        }
    }
}
