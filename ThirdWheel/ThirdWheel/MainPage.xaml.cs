using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ThirdWheel
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>


    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        //a = opacity

        private async void UpdateColor(object sender, object e)
        {

            try
            {
                HttpClient httpclient = new HttpClient();

                var response = await httpclient.GetAsync("http://glassbattleapp.azurewebsites.net/display/whatnext/1");

                var ResponseText = await response.Content.ReadAsStringAsync();

                var asdf = JsonConvert.DeserializeObject<Devices>(ResponseText);

                string hexin = asdf.Color;
                string r = hexin.Substring(1, 2);
                string g = hexin.Substring(3, 2);
                string b = hexin.Substring(5, 2);

                rect.Fill = new SolidColorBrush(Color.FromArgb(255, Convert.ToByte(r, 16), Convert.ToByte(g, 16), Convert.ToByte(b, 16)));
                textBox.Text = asdf.Text;
            }
            catch (Exception)
            {

            }
        }

        private void rect_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new DispatcherTimer();
            timer.Tick += UpdateColor;
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Start();
        }
    }
}
