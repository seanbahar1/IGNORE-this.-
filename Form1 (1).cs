using System;
using System.Media;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;
using WMPLib;
namespace House_Project_Test
{
    public partial class Form1 : Form
    {
     
        
        SpeechRecognitionEngine recEngine = new SpeechRecognitionEngine();
        SpeechSynthesizer synthesizer = new SpeechSynthesizer();
        string Data = "00000000";
        string TV = "0";
        string Win = "00";
        string AC = "00";
        string Light = "00";
        string Door = "0";

        //מקבל רשימה של שמות השירים בתקייה
        string[] songList;
        int whichSong = 0;
        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
        bool firstPlay = true; 

        void getSongList()
        {
            mediaPlayer.Hide();
            songList = Directory.GetFiles(@"./Music/", "*.mp*");
            foreach (string a in songList) Console.WriteLine(a);
            nameOfSongLabel.Hide();
        }

        bool audioPlaying = false;



        public Form1()
        {
            InitializeComponent();
            serialPort1.PortName = "COM5";
            serialPort1.BaudRate = 9600;
            serialPort1.Open();
            label1.Text = Data;
        }
            
        public void DataRefresh()
        {
            Data = Door + Light + AC + Win + TV;
            serialPort1.Encoding = Encoding.GetEncoding(28591); 
            byte[] buf = new byte[1];
            buf[0] = Convert.ToByte(Data.Substring(0, 8), 2);
            serialPort1. Write(buf, 0, 1);
            label1.Text = Data;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            Choices commands = new Choices();
            commands.Add(new string[] {"TV on","TV off","window up", "window dwon", "window stop", "AC up", "AC down", "AC stop","light high",
                "light low", "light off", "open door", "close door" });
            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(commands);
            Grammar grammar = new Grammar(gBuilder);

            recEngine.LoadGrammarAsync(grammar);
           // recEngine.SetInputToDefaultAudioDevice();
            recEngine.SpeechRecognized += RecEngine_SpeechRecognized;
            getSongList();
        }

        private void RecEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch(e.Result.Text)
            {
                case "TV on":
                    TV = "1";
                    synthesizer.SpeakAsync("turning the TV on");
                    break;
                case "TV off":
                    TV = "0";
                    synthesizer.SpeakAsync("turning the TV off");
                    break;
                case "window up":
                    Win = "10";
                    synthesizer.SpeakAsync("rolling up the window");
                    break;
                case "window down":
                    Win = "01";
                    synthesizer.SpeakAsync("rolling down the window");
                    break;
                case "window stop":
                    Win = "00";
                    synthesizer.SpeakAsync("stopping rolling the window");
                    break;
                case "AC up":
                    if (AC == "00")
                        {
                        AC = "01";
                        synthesizer.SpeakAsync("the AC is in level one");
                        }
                        else if (AC == "01")
                        {
                            AC = "10";
                            synthesizer.SpeakAsync("the AC is in level two");
                        }
                        else if (AC == "10")
                        {
                            AC = "11";
                            synthesizer.SpeakAsync("the AC is in level three");
                        }
                        else
                            synthesizer.SpeakAsync("the AC is in the highst level");
                    break;
                case "AC down":
                    if (AC == "11")
                    {
                        AC = "10";
                        synthesizer.SpeakAsync("the AC is in level two");
                    }
                    else if (AC == "10")
                    {
                        AC = "01";
                        synthesizer.SpeakAsync("the AC is in level one");
                    }
                    else if (AC == "01")
                    {
                        AC = "00";
                        synthesizer.SpeakAsync("the AC is in level zero");
                    }
                    else
                        synthesizer.SpeakAsync("the AC is off");
                    break;
                case "AC stop":
                    AC = "00";
                    synthesizer.SpeakAsync("turrning the AC off");
                    break;
                case "light high":
                    Light = "10";
                    synthesizer.SpeakAsync("setting light high");
                    break;
                case "light low":
                    Light = "01";
                    synthesizer.SpeakAsync("setting light low");
                    break;
                case "light off":
                    Light = "00";
                    synthesizer.SpeakAsync("turning the light off");
                    break;
                case "open door":
                    Door = "1";
                    synthesizer.SpeakAsync("opening the door");
                    break;
                case "close door":
                    Door = "0";
                    synthesizer.SpeakAsync("closing the door");
                    break;

            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void TV_On_Click(object sender, EventArgs e)
        {
            TV = "1";
            synthesizer.SpeakAsync("turning the TV on");
            DataRefresh();
        }

        private void TV_Off_Click(object sender, EventArgs e)
        {
            TV = "0";
            synthesizer.SpeakAsync("turning the TV off");
            DataRefresh();

        }

        private void WIN_Up_Click(object sender, EventArgs e)
        {
            Win = "10";
            synthesizer.SpeakAsync("rolling up the window");
            DataRefresh();
        }

        private void WIN_Down_Click(object sender, EventArgs e)
        {
            Win = "01";
            synthesizer.SpeakAsync("rolling the window down");
            DataRefresh();
        }

        private void WIN_Stp_Click(object sender, EventArgs e)
        {
            Win = "00";
            synthesizer.SpeakAsync("stopping the window");
            DataRefresh();
        }

        private void AC_UP_Click(object sender, EventArgs e)
        {
            if (AC == "00")
            {
                AC = "01";
                synthesizer.SpeakAsync("the AC is in level one");
            }
            else if (AC == "01")
            {
                AC = "10";
                synthesizer.SpeakAsync("the AC is in level two");
            }
            else if (AC == "10")
            {
                AC = "11";
                synthesizer.SpeakAsync("the AC is in level three");
            }
            else
                synthesizer.SpeakAsync("the AC is in the highst level");
                DataRefresh();
        }

        private void AC_Down_Click(object sender, EventArgs e)
        {
            if (AC == "11")
            {
                AC = "10";
                synthesizer.SpeakAsync("the AC is in level two");
            }
            else if (AC == "10")
            {
                AC = "01";
                synthesizer.SpeakAsync("the AC is in level one");
            }
            else if (AC == "01")
            {
                AC = "00";
                synthesizer.SpeakAsync("the AC is in level zero");
            }
            else
                synthesizer.SpeakAsync("the AC is off");
            DataRefresh();
        }

        private void AC_Stp_Click(object sender, EventArgs e)
        {
            AC = "00";
            synthesizer.SpeakAsync("turnnig the AC off");
            DataRefresh();
        }

        private void Light_High_Click(object sender, EventArgs e)
        {
            Light = "10";
            synthesizer.SpeakAsync("setting light high");
            DataRefresh();
        }

        private void Light_Low_Click(object sender, EventArgs e)
        {
            Light = "01";
            synthesizer.SpeakAsync("setting light low");
            DataRefresh();
        }

        private void Light_Off_Click(object sender, EventArgs e)
        {
            Light = "00";
            synthesizer.SpeakAsync("turnnig light off");
            DataRefresh();
        }

        private void Door_Cls_Click(object sender, EventArgs e)
        {
            Door = "0";
            synthesizer.SpeakAsync("closing the door");
            DataRefresh();
        }

        private void Door_Opn_Click(object sender, EventArgs e)
        {
            Door = "1";
            synthesizer.SpeakAsync("opening the door");
            DataRefresh();
        }

        private void label1_Click(object sender, EventArgs e)
        {
           
        }

        private void voice_enb_Click(object sender, EventArgs e)
        {
            recEngine.RecognizeAsync(RecognizeMode.Multiple);
            voice_dis.Enabled = true;
            voice_enb.Enabled = false;
        }

        private void voice_dis_Click(object sender, EventArgs e)
        {
            recEngine.RecognizeAsyncStop();
            voice_enb.Enabled = true;
            voice_dis.Enabled = false;

        }

        //buttonAudio1
        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void PlayAndPasue_Click(object sender, EventArgs e)
        {
            if (audioPlaying == false)
            {
                mediaPlayer.Show();
                if(firstPlay)
                mediaPlayer.URL = "./Music/" + songList[whichSong].Replace("./Music/", "");
                firstPlay = false;
                mediaPlayer.Ctlcontrols.play();
                mediaPlayer.uiMode = "none";
                nameOfSongLabel.Text = songList[whichSong].Replace("./Music/", "").Replace(".mp3", "");
                nameOfSongLabel.Show();
                audioPlaying = true;
                playAndPasue.Text = "||";
                Console.WriteLine(mediaPlayer.settings.volume);
                synthesizer.SpeakAsync("music started");
            }
            else if (audioPlaying == true)
            {
                mediaPlayer.Ctlcontrols.pause();
                audioPlaying = false;
                playAndPasue.Text = ">";
                synthesizer.SpeakAsync("music stopped");
            }

        }

        private void MediaPlayer_Enter(object sender, EventArgs e)
        {

        }

        //prev
        private void Button2_Click(object sender, EventArgs e)
        {
            if (whichSong == 0) whichSong = songList.Length-1;
            else whichSong--;
            mediaPlayer.URL = "./Music/" + songList[whichSong].Replace("./Music/", "");
            nameOfSongLabel.Text = songList[whichSong].Replace("./Music/", "").Replace(".mp3", "");
            synthesizer.SpeakAsync("previous song");
        }

        private void NextSong_Click(object sender, EventArgs e)
        {
            if (whichSong == (songList.Length - 1)) whichSong = 0;
            else whichSong++;
            mediaPlayer.URL = "./Music/" + songList[whichSong].Replace("./Music/", "");
            nameOfSongLabel.Text = songList[whichSong].Replace("./Music/", "").Replace(".mp3", "");
            synthesizer.SpeakAsync("next song");
        }

        private void VolumeTrackBar_Scroll(object sender, EventArgs e)
        {

            mediaPlayer.settings.volume = volumeTrackBar.Value;
        }
    }
}
