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
using FonyRunwoman.Classes;
using NAudio;

namespace FonyRunwoman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private NAudio.Wave.BlockAlignReductionStream stream = null;

        private NAudio.Wave.DirectSoundOut output = null;

        TimeSpan TimeSpan = new TimeSpan();

        public MainWindow()
        {
            InitializeComponent();
   
        }

        //button to add songs to the player
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Audio File (*.mp3;*.wav) | *.mp3;*.wav;";
            //if a song is selected execute the following
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //dispose of previous song data
                DisposeWave();

                //check if mp3 or wav
                if (ofd.FileName.EndsWith(".mp3"))
                {
                    //take the pcm data into a new stream and play the mp3 file
                    NAudio.Wave.WaveStream pcm = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(new NAudio.Wave.Mp3FileReader(ofd.FileName));
                    stream = new NAudio.Wave.BlockAlignReductionStream(pcm);
                }
                else if (ofd.FileName.EndsWith(".wav"))
                {

                    NAudio.Wave.WaveStream pcm = new NAudio.Wave.WaveChannel32(new NAudio.Wave.WaveFileReader(ofd.FileName));
                    stream = new NAudio.Wave.BlockAlignReductionStream(pcm);
                }
                else throw new InvalidOperationException("Not a correct audio file type");

                output = new NAudio.Wave.DirectSoundOut();
                output.Init(stream);
                output.Play();
                txtNowPlaying.Text = "Now playing: " + ofd.SafeFileName;
               
                //doesn't really do anything atm. WIP to try get a working trackbar 

                //string duration = stream.TotalTime.ToString("mm\\:ss");
                //trackPosition.Maximum = (int)stream.TotalTime.TotalSeconds;

                //string curTime = stream.CurrentTime.ToString("mm\\:ss");
                //int curTimeSeconds = (int)stream.CurrentTime.TotalSeconds;

            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            //check if playstate is paused, if paused then play
            if(output != null)
            {
                if (output.PlaybackState == NAudio.Wave.PlaybackState.Paused) output.Play();
            }
            else
            {
                txtNowPlaying.Text = "Nothing to play...";
            }
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            //check if playstate is playing, if playing then pause
            if(output != null)
            {
                if(output.PlaybackState == NAudio.Wave.PlaybackState.Playing) output.Pause();
            }
            else
            {
                txtNowPlaying.Text = "Nothing to pause...";
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
            if(stream != null)
            {
                stream.Dispose();
                stream = null;
            }


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisposeWave();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            DisposeWave();

        }


    }
}
