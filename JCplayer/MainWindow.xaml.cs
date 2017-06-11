using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using Microsoft.Win32;
using System.IO;

// jak zrobić, aby zaznaczony elememnt na ListBox był aktualny w MyMediaElement?
// dodawanie wielu plików do ListBox
// dwukrotne kliknięcie na element z listy powoduje odtwarzanie utworu

namespace JCplayer
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool userIsDraggingSlider = false;
        bool filesPlaying;

        public MainWindow()
        {
            InitializeComponent();
            
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick (object sender, EventArgs e)
        {
            if ((MyMediaElement.Source != null) && (MyMediaElement.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
                {
                SeekSlider.Minimum = 0;
                SeekSlider.Maximum = MyMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                SeekSlider.Value = MyMediaElement.Position.TotalSeconds;
                }
        }

        
        void Play (object sender, RoutedEventArgs e)
        {
            ToggleButton tb = (ToggleButton)sender;
            if ((bool)tb.IsChecked)
            {
                MyMediaElement.Play();
                filesPlaying = true;
            }
            else
            {
                MyMediaElement.Pause();
                filesPlaying = false;
            }
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            MyMediaElement.Stop();
            filesPlaying = false;
        }

        private void VolSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            MyMediaElement.Volume = (double) VolSlider.Value;
        }
        
        //dodawanie utworów do listy
        private void add_Click(object sender, RoutedEventArgs e)
        {
            Stream checkStream = null;
            OpenFileDialog okienko = new OpenFileDialog();

            okienko.InitialDirectory = "C:\\";
            okienko.Filter = "Mp3 Audio Files (*.mp3)|*.mp3| Windows Media File (*.wma)|*.wma| WAV Audio File (*.wav)|*.wav| All Files (*.*)|*.*";
            okienko.FilterIndex = 1;
            //okienko.RestoreDirectory = false;
            okienko.Multiselect = true;

            if (okienko.ShowDialog() == true)
            {
                MyMediaElement.Source = new Uri(okienko.FileName);
            }
            ListBoxItem item = new ListBoxItem();
            var Name = okienko.FileName;
            item.Content = Name;
            SongList.Items.Add(item);

        }
        
        //obsluga SeekSlidera
        private void SeekSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            Time.Text = TimeSpan.FromSeconds(SeekSlider.Value).ToString(@"hh\:mm\:ss");
        }

        private void SeekSlider_DragStarted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void SeekSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            MyMediaElement.Position = TimeSpan.FromSeconds(SeekSlider.Value);
        }

        //glosnosc za pomoca kolka myszy
        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MyMediaElement.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }
        //odtwarzanie utworu po dwukrotnym kliknieciu
        //private void SongList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    MyMediaElement.Stop();
        //    string mediaPath = ((ListBoxItem)SongList.SelectedValue).Content.ToString();
        //    MyMediaElement.Source = new Uri(mediaPath);
        //    MyMediaElement.Play();
        //}
    }
        

        
    
}
