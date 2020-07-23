using FonyRunwoman.Classes;
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

namespace FonyRunwoman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LinkedList<Song> songDll = new LinkedList<Song>();

        private NAudio.Wave.BlockAlignReductionStream stream = null;

        private NAudio.Wave.DirectSoundOut output = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Song mysongs = new Song();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Audio File (*.mp3;*.wav) | *.mp3;*.wav;";
            //if a song is selected execute the following
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {     
                //add song title and url to song object
                mysongs.gsSongName = ofd.SafeFileName;
                mysongs.gsSongURL = ofd.FileName;
                //add song to dll
                songDll.AddLast(mysongs);
                //clear list and redisplay 
                lstSongBox.Items.Clear();
                DisplaySongs();
            }
        }

        private void DisplaySongs()
        {
            foreach(Song song in songDll)
            {
                lstSongBox.Items.Add(song.gsSongName);
            }
        }

        private void DisposeWave()
        {
            //dispose of song from stream. To ensure no memory leaks 
            if (stream == null)
            {
                txtNowPlaying.Text = "Nothing to stop...";
            }
            if (output != null)
            {
                if (output.PlaybackState == NAudio.Wave.PlaybackState.Playing) output.Stop();
                output.Dispose();
                output = null;
            }
            if (stream != null)
            {
                stream.Dispose();
                stream = null;
            }
        }
        //dispose of song on Close
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisposeWave();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            DisposeWave();
            btnPlay.IsEnabled = false;
            btnStop.IsEnabled = false;
            btnPause.IsEnabled = false;
            txtNowPlaying.Text = "Nothing Playing.";
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            
            if (output != null)
            {
                if (output.PlaybackState == NAudio.Wave.PlaybackState.Paused) output.Play();
            }
        }
        
        private void btnPause_Click_1(object sender, RoutedEventArgs e)
        {
            if (output != null)
            {
                if (output.PlaybackState == NAudio.Wave.PlaybackState.Playing) output.Pause();
            }
        }

        private void lstSongBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach(Song find in songDll)
            {
                if(lstSongBox.SelectedItem.ToString() == find.gsSongName)
                {
                    DisposeWave();
                    //check if mp3 or wav
                    if (find.gsSongURL.EndsWith(".mp3"))
                    {
                        //take the pcm data into a new stream and play the mp3 file
                        NAudio.Wave.WaveStream pcm = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(new NAudio.Wave.Mp3FileReader(find.gsSongURL));
                        stream = new NAudio.Wave.BlockAlignReductionStream(pcm);
                    }
                    else if (find.gsSongURL.EndsWith(".wav"))
                    {
                        //do the same as the above but with a wav file
                        NAudio.Wave.WaveStream pcm = new NAudio.Wave.WaveChannel32(new NAudio.Wave.WaveFileReader(find.gsSongURL));
                        stream = new NAudio.Wave.BlockAlignReductionStream(pcm);
                    }
                    else throw new InvalidOperationException("Not a correct audio file type");
                    output = new NAudio.Wave.DirectSoundOut();
                    output.Init(stream);
                    output.Play();

                    btnPlay.IsEnabled = true;
                    btnStop.IsEnabled = true;
                    btnPause.IsEnabled = true;

                    txtNowPlaying.Text = "Now playing: " + lstSongBox.SelectedItem.ToString();

                }
            }
        }
    }
}
