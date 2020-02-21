using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App15
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public class DataModel : INotifyPropertyChanged
        {
            public string icon_url { get; set; }
            public bool ssml_support { get; set; }
            public string data_name { get; set; }
            public string data_type { get; set; }
            public int itemIndex { get; set; }
            private bool _isSelected;
            public bool isSelected
            {
                get => _isSelected;
                set
                {
                    _isSelected = value;
                    this.OnPropertyChanged(nameof(isSelected));
                }
            }
            public ICommand ClickCommand { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class TestDataList
        {
            public string data_type { get; set; }
            public string data_name { get; set; }
            public bool ssml_support { get; set; }
            public string icon_url { get; set; }
        }

        DataModel[] dms;
        TestDataList[] list;
        int listLength;
        int previousSelectedIndex;

        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;

            InitUIData();
        }

        private async void InitUIData()
        {
            // Put Json file in below path, changed according to your project.
            // C:\Users\xuejd1\AppData\Local\Packages\5edc05e9-c011-4f5f-8660-54a7fcce95a1_sews9q4cd2dha\LocalState\test.json
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile testJsonFile = await storageFolder.GetFileAsync("test.json");
            string payload = await Windows.Storage.FileIO.ReadTextAsync(testJsonFile);

            list = JsonData<TestDataList[]>(payload);
            listLength = list.Length;
            dms = new DataModel[listLength];

            wrapGrid.Children.Clear();
            for (int i = 0; i < listLength; i++)
            {
                dms[i] = new DataModel();
                dms[i].icon_url = list[i].icon_url;
                dms[i].ssml_support = list[i].ssml_support;
                dms[i].data_name = list[i].data_name;
                dms[i].data_type = list[i].data_type;
                dms[i].isSelected = false;
                dms[i].itemIndex = i;
                dms[i].ClickCommand = new RelayCommand<object>(ClickItemButton, true);

                ContentControl ccUnit = new ContentControl();
                ccUnit.ContentTemplate = this.Resources["singleUnit"] as DataTemplate;
                ccUnit.Content = dms[i];
                wrapGrid.Children.Add(ccUnit);
            }
        }

        private async void ClickItemButton(object param)
        {
            int index = (int)param;
            SetMarker(index);
        }

        private void SetMarker(int index)
        {
            ContentControl ccUnit = wrapGrid.Children[previousSelectedIndex] as ContentControl;
            var previousContent = ccUnit.Content as DataModel;
            previousContent.isSelected = false;

            ContentControl currentUnit = wrapGrid.Children[index] as ContentControl;
            var currentContent = currentUnit.Content as DataModel;
            currentContent.isSelected = true;

            previousSelectedIndex = index;
        }

        private ContentControl InitCCUnit(int index, bool setMarked)
        {
            ContentControl ccUnit = new ContentControl();
            ccUnit.ContentTemplate = this.Resources["singleUnit"] as DataTemplate;

            if (setMarked)
                dms[index].isSelected = true;
            else
                dms[index].isSelected = false;

            ccUnit.Content = dms[index];
            return ccUnit;
        }

        public static T JsonData<T>(string jsonString)
        {
            if (jsonString != null)
            {
                JsonSerializer json = JsonSerializer.Create();
                T root = json.Deserialize<T>(new JsonTextReader(new StringReader(jsonString)));
                return root;
            }
            return default(T);
        }
    }
}
