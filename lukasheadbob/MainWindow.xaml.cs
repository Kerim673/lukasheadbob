using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Collections.ObjectModel;
using Utilities;

namespace lukasheadbob
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double KeysPerSecond;
        public static double KeyPressesThisSecond;

        private static System.Timers.Timer KeyTimer;

        public MainWindow()
        {
            KeyStats Stats = new KeyStats();

            InitializeComponent();
            MEPlayer.DataContext = Stats;
            globalKeyboardHook gkh;
            MEPlayer.SpeedRatio = 1; 

            gkh = new globalKeyboardHook();
            gkh.HookedKeys.Add(Keys.X);
            gkh.HookedKeys.Add(Keys.Z);
            gkh.KeyDown += new System.Windows.Forms.KeyEventHandler(gkh_KeyDown);
            //gkh.hook();
            SetTimer();
        }

        private void SetTimer()
        {
            KeyTimer = new System.Timers.Timer(1000);

            KeyTimer.Elapsed += OnTimedEvent;
            KeyTimer.AutoReset = true;
            KeyTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            KeysPerSecond = KeyPressesThisSecond;

            if(KeyPressesThisSecond > 3)
            {
                KeyPressesThisSecond = 3;
            }

            Dispatcher.Invoke(new Action(() => { MEPlayer.SpeedRatio = KeyPressesThisSecond; Tetris.Text = KeyPressesThisSecond.ToString(); }));
            KeyPressesThisSecond = 0.5;
            //System.Windows.MessageBox.Show(KeyStats.CPS[0].kps.ToString());

            //KeyPressesThisSecond = 1;
            //MEPlayer.SpeedRatio = KeysPerSecond;
        }

        public void gkh_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //System.Windows.MessageBox.Show(e.KeyData.ToString());
            KeyPressesThisSecond = KeyPressesThisSecond + 0.5;
            e.Handled = true;
        }

        private void MEPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            MEPlayer.Position = TimeSpan.Zero;
            MEPlayer.Play();
        }
    }

    public class KeyStats
    {
        public ObservableCollection<Click> Clicks { get; set; }
        public static ObservableCollection<Click> CPS;


        public int KPS { get { return kps; } set { kps = value; } }
        int kps = 0;

        public KeyStats()
        { 
            CPS = new ObservableCollection<Click>();
            CPS.Add(new Click(0));
        }


        public class Click
        {
            public int kps;
            public Click(int number)
            {
                kps = number;
            }
        }
    }
}
