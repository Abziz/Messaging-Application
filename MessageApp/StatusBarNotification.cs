using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MessageApp
{
    public class StatusBlock
    {
        public static readonly StatusBlockStyle Default = new StatusBlockStyle { Background = Brushes.LightGray, Foreground = Brushes.DarkSlateGray };
        public static readonly StatusBlockStyle Success = new StatusBlockStyle { Background = Brushes.LightGreen, Foreground = Brushes.DarkGreen };
        public static readonly StatusBlockStyle Danger = new StatusBlockStyle { Background = Brushes.IndianRed, Foreground = Brushes.DarkRed };
        public static readonly StatusBlockStyle Warning = new StatusBlockStyle { Background = Brushes.Gold, Foreground = Brushes.DarkGoldenrod };
        public double AlertSpeedInSeconds { get; set; } = 0.5;
        public uint AlertDuration { get; set; } = 5000; // in millies
        public uint AlertHeight { get; set; } = 18; //default

        private Timer statusTimer;
        private TextBlock statusBar;
        public StatusBlock(TextBlock statusBar)
        {
            this.statusBar = statusBar;
        }

        public void Alert(string msg, StatusBlockStyle style = null)
        {
            if (statusTimer == null)
            {
                statusTimer = new Timer(AlertDuration);
                statusTimer.Elapsed += AlertOffHandler;
            }
            if (style == null)
            {
                style = Default;
            }

            if (statusTimer.Enabled)
            {
                statusTimer.Stop();

            }
            statusTimer.Start();
            statusBar.Height = 0;
            statusBar.Text = msg;
            statusBar.Background = style.Background;
            statusBar.Foreground = style.Foreground;
            DoubleAnimation anim = new DoubleAnimation();
            anim.From = 0;
            anim.To = AlertHeight;
            anim.Duration = TimeSpan.FromSeconds(0.5);
            statusBar.BeginAnimation(TextBox.HeightProperty, anim);
        }

        private void AlertOffHandler(Object source, ElapsedEventArgs e)
        {
            statusBar.Dispatcher.BeginInvoke(
                (Action)(() =>
                {
                    DoubleAnimation anim = new DoubleAnimation();
                    anim.From = statusBar.Height;
                    anim.To = 0;
                    anim.Duration = TimeSpan.FromSeconds(0.5);
                    statusBar.BeginAnimation(TextBox.HeightProperty, anim);
                }));
        }
    }

    public class StatusBlockStyle
    {
        public Brush Foreground { get; set; }
        public Brush Background { get; set; }
    }
}
