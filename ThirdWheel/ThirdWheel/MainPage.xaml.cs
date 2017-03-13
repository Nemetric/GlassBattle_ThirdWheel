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

        public int puckY = 5;
        public int puckX = 5;

        public int rPoints = 0;
        public int lPoints = 0;

        public double lMove = 0;
        public double rMove = 0;

        public string lName = "Donald";
        public string rName = "Bernie";

        public DispatcherTimer delayTimer;

        //Both names must be filled to restart the match
        //Names are showen at the begining of each match
        //10 points is endgame
        //If there is no user in line, set the Name to null
        //Input is handled though lMove and rMove
        //should've probably commented in an organised manor, but yolo

        public async void Game(object sender, object c)
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

            Random fluid = new Random();

            if (pTranslate.X > rect.ActualWidth * 0.5)
            {
                puckX = -10;

                pTranslate.X = 0;
                pTranslate.Y = 0;

                lPoints = lPoints + 1;
                lScore.Text = lPoints.ToString();

                puckX = puckX + fluid.Next(-6, 6);
            }
            else if (pTranslate.X < rect.ActualWidth * -0.5)
            {
                puckX = 10;

                pTranslate.X = 0;
                pTranslate.Y = 0;

                rPoints = rPoints + 1;
                rScore.Text = rPoints.ToString();

                puckX = puckX + fluid.Next(-6, 6) / 2;
            }

            if (pTranslate.Y > rTranslate.Y - (rPad.ActualHeight * 0.5) && pTranslate.Y < rTranslate.Y + (rPad.ActualHeight * 0.5) && pTranslate.X > (rect.ActualWidth / 2) - 25)
            {
                puckX = -10;
                puckY = puckY + fluid.Next(-6, 6) / 2;
            }

            if (pTranslate.Y > lTranslate.Y - (lPad.ActualHeight * 0.5) && pTranslate.Y < lTranslate.Y + (lPad.ActualHeight * 0.5) && pTranslate.X < (rect.ActualWidth / -2) + 25)
            {
                puckX = 10;
                puckY = puckY + fluid.Next(-6, 6);
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
                while (lName == null && rName == null)
                {

                }
                if (!delayTimer.IsEnabled && lName != null && rName != null)
                    delayTimer.Start();
            }

            if (lPoints > 9)
            {
                lScore.Text = "WINNER";
                rScore.Text = "LOSER";
                puckX = 0;
                puckY = 0;
                while (lName == null && rName == null)
                {

                }
                if (!delayTimer.IsEnabled && lName != null && rName != null)
                    delayTimer.Start();
            }

            rMove = ((slider.Value - 100) / 50);

        }

        public void GameReset(object sended, object d)
        {
            puckY = 10;
            puckX = 10;

            rPoints = 0;
            lPoints = 0;

            rScore.Text = rName;
            lScore.Text = lName;

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
