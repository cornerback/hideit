using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace HideIt
{
    public partial class AudioPlayerUI : Form
    {
        public AudioPlayerUI()
        {
            InitializeComponent();
        }

        private const string TITLE = "Audio Player";

        private HideIt.Audio.WaveOutPlayer m_Player;
        private HideIt.Audio.WaveFormat m_Format;
        private Stream m_AudioStream;

        private void Filler(IntPtr data, int size)
        {
            byte[] b = new byte[size];
            if (m_AudioStream != null)
            {
                int pos = 0;
                while (pos < size)
                {
                    int toget = size - pos;
                    int got = m_AudioStream.Read(b, pos, toget);
                    if (got < toget)
                        m_AudioStream.Position = 0; // loop if the file ends
                    pos += got;
                }
            }
            else
            {
                for (int i = 0; i < b.Length; i++)
                    b[i] = 0;
            }
            System.Runtime.InteropServices.Marshal.Copy(b, 0, data, size);
        }

        private void Stop()
        {
            if (m_Player != null)
                try
                {
                    m_Player.Dispose();
                }
                finally
                {
                    m_Player = null;
                }
        }

        private void Play()
        {
            Stop();
            if (m_AudioStream != null)
            {
                m_AudioStream.Position = 0;
                m_Player = new HideIt.Audio.WaveOutPlayer(-1, m_Format, 16384, 3, new HideIt.Audio.BufferFillEventHandler(Filler));
            }
        }

        private void CloseFile()
        {
            Stop();
            if (m_AudioStream != null)
                try
                {
                    m_AudioStream.Close();
                }
                finally
                {
                    m_AudioStream = null;
                }
        }

        private void OpenFile()
        {
            if (OpenDlg.ShowDialog() == DialogResult.OK)
            {

                CloseFile();
                try
                {
                    HideIt.Audio.PlayerWaveStream S = new HideIt.Audio.PlayerWaveStream(OpenDlg.FileName);
                    if (S.Length <= 0)
                        throw new Exception("Invalid WAV file");
                    m_Format = S.Format;
                    if (m_Format.wFormatTag != (short)HideIt.Audio.WaveFormats.Pcm && m_Format.wFormatTag != (short)HideIt.Audio.WaveFormats.Float)
                        throw new Exception("Olny PCM files are supported");

                    m_AudioStream = S;

                ///Get file name from path
                    this.lbl_audioPoemName.Text = Path.GetFileName(this.OpenDlg.FileName);
                }
                catch (Exception e)
                {
                    CloseFile();
                    MessageBox.Show(e.Message);
                }
            }
        }
        private void btn_browse_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void btn_play_Click(object sender, EventArgs e)
        {
            Play();

            ///enable the picturebox to display animated .gif while playing audio 
            picBoxAudio1.Enabled = true;
            picBoxAudio2.Enabled = true;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            Stop();
            ///disable the picturebox to stop display when audio is stoped
            picBoxAudio1.Enabled = false;
            picBoxAudio2.Enabled = false;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            ///close the form & release all its resources
            this.Dispose(); 
            if (m_Player != null)
                try
                {
                    m_Player.Dispose();
                }
                finally
                {
                    m_Player = null;
                }
        }
    }
}
