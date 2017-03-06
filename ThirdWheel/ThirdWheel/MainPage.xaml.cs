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
        public TranslateTransform rTranslate = new TranslateTransform();
        public TranslateTransform lTranslate = new TranslateTransform();
        public TranslateTransform pTranslate = new TranslateTransform();

        public int puckY = 10;
        public int puckX = 10;

        public int rPoints = 0;
        public int lPoints = 0;

        public double lMove = 0;
        public double rMove = 0;

        public DispatcherTimer delayTimer;

        public async void Game(object sender, object e)
        {
            rPad.RenderTransform = rTranslate;
            lPad.RenderTransform = lTranslate;
            Puck.RenderTransform = pTranslate;

            Point puckPoint = Puck.TransformToVisual(Window.Current.Content).TransformPoint(new Point( 0, 0));

            if (pTranslate.Y > rect.ActualHeight * 0.5)
            {
                puckY = -10;
            }
            else if (pTranslate.Y < rect.ActualHeight * -0.5)
            {
                puckY = 10;
            }

            if (pTranslate.X > rect.ActualWidth * 0.5)
            {
                puckY = 10;
                puckX = 10;

                pTranslate.X = 0;
                pTranslate.Y = 0;

                lPoints = lPoints + 1;
                lScore.Text = lPoints.ToString();
            }
            else if (pTranslate.X < rect.ActualWidth * -0.5)
            {
                puckY = 10;
                puckX = 10;

                pTranslate.X = 0;
                pTranslate.Y = 0;

                rPoints = rPoints + 1;
                rScore.Text = rPoints.ToString();
            }

            if (pTranslate.Y > rTranslate.Y - (rPad.ActualHeight * 0.5) && pTranslate.Y < rTranslate.Y + (rPad.ActualHeight * 0.5) && pTranslate.X > (rect.ActualWidth / 2) - 25)
            {
                puckX = -10;
            }

            pTranslate.Y = pTranslate.Y + puckY;
            pTranslate.X = pTranslate.X + puckX;

            lTranslate.Y = lTranslate.Y + lMove * -10;
            rTranslate.Y = rTranslate.Y + rMove * -10;

            if (lTranslate.Y < (rect.ActualHeight * -0.5))
            {
                lTranslate.Y = (rect.ActualHeight * -0.5);
            }
            else if (lTranslate.Y > (rect.ActualHeight * 0.5))
            {
                lTranslate.Y = (rect.ActualHeight * 0.5);
            }

            if (rTranslate.Y < (rect.ActualHeight * -0.5))
            {
                rTranslate.Y = (rect.ActualHeight * -0.5);
            }
            else if (rTranslate.Y > (rect.ActualHeight * 0.5))
            {
                rTranslate.Y = (rect.ActualHeight * 0.5);
            }

            if (rPoints > 9)
            {
                rScore.Text = "WINNER";
                lScore.Text = "LOSER";
                puckX = 0;
                puckY = 0;
                if (!delayTimer.IsEnabled)
                    delayTimer.Start();
            }

            if (lPoints > 9)
            {
                lScore.Text = "WINNER";
                rScore.Text = "LOSER";
                puckX = 0;
                puckY = 0;
                if (!delayTimer.IsEnabled)
                    delayTimer.Start();
            }
        }

        public void GameReset(object sended, object d)
        {
            puckY = 10;
            puckX = 10;

            rPoints = 0;
            lPoints = 0;

            rScore.Text = "0";
            lScore.Text = "0";

            delayTimer.Stop();
        }

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
                textBox.Text = "hello";
            }

            catch (Exception)
            {
                textBox.Text = "hello";
            }
        }

        private void rect_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new DispatcherTimer();
            timer.Tick += UpdateColor;
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Start();

            var frameTimer = new DispatcherTimer();
            frameTimer.Tick += Game;
            frameTimer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            frameTimer.Start();

            delayTimer = new DispatcherTimer();
            delayTimer.Tick += GameReset;
            delayTimer.Interval = new TimeSpan(0, 0, 0, 3);

        }

    }
}
