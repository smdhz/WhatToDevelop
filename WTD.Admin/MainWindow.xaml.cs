using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WTD.Admin
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private DataService.Container db = new DataService.Container(new Uri("http://smdhz.cf/odata"));
        private List<DataService.ItemInfo> list;

        public MainWindow()
        {
            InitializeComponent();
        }

        private DataService.ItemInfo _item = new DataService.ItemInfo();
        public DataService.ItemInfo Item
        {
            get { return _item; }
            set
            {
                _item = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Item)));
            }
        } 
        public System.Collections.ObjectModel.ObservableCollection<DataService.ItemNo2Info> No2infoes { get; set; } 
            = new System.Collections.ObjectModel.ObservableCollection<DataService.ItemNo2Info>();

        public List<string> Types => list.Select(i => i.Type).Distinct().ToList();
        public List<string> Icons => list.Select(i => i.Icon).Distinct().ToList();

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                Dispatcher.Invoke(() =>
                {
                    _status = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Status)));
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Status = "正在连接 " + db.BaseUri;
            db.AddToItemInfoes(Item);
            foreach (DataService.ItemNo2Info i in No2infoes)
            {
                i.Item = Item.Name;
                db.AddToItemNo2Infoes(i);
            }

            Status = "已保存 " + string.Join(" ", db.SaveChanges().GroupBy(i => i.StatusCode).Select(i => i.Key + " x" + i.Count()));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
            db.BuildingRequest += (se, ev) => ev.Headers.Add("Authorization", "BasicAuth 9anofJrJVW7FKNCUaJ3hF");
            Task.Run(() => btnReset_Click(sender, e));
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            Status = "正在连接 " + db.BaseUri;
            list = db.ItemInfoes.ToList();
            Status = list.Count + " 条记录已获取";
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Types)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Icons)));

            Item = new DataService.ItemInfo();
            No2infoes.Clear();
        }
    }
}
