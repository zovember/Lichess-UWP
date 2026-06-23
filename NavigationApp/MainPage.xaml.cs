using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Newtonsoft.Json.Linq;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace NavigationApp
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            ServerClient Client = new ServerClient();
            Client.Error_ReadStreamIncomingEvents += ErrorInternet;
            Client.GameStartEvent += NewGamePage;

            _ = Client.ReadStreamIncomingEvents();
        }        

        public void NewGamePage(JObject jsonObj)
        {
            GameInfo gameInfo = new GameInfo(jsonObj);
            Frame.Navigate(typeof(GamePage), gameInfo);
        }

        private async void Forward_Click(object sender, RoutedEventArgs e)
        {
            ServerClient Client = new ServerClient();
            bool status = await Client.CreateSeek(false, 10, 0, "random");
            if (!status)
            {
                ErrorInternet();
            }
        }

        private void NewBoardBtn(object sender, RoutedEventArgs e)
        {
            GameInfo gameInfo = new GameInfo();
            Frame.Navigate(typeof(GamePage), gameInfo);
        }

        public void ErrorInternet()
        {
            Toaster.Show("Проверьте подключение к интернету");
        }
    }
}
