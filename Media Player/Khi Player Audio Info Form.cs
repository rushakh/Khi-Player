namespace Khi_Player
{
    public partial class Khi_Player_Audio_Info_Form : Form
    {
        /*  public string? title, artist, album, trackNumber, genre, duration, bitrate, sampleRate,
                          encoding, channel, path, format, lyrics;
          public Image art;
        */
        public string? title, artist, album, trackNumber, genre, duration, bitrate, sampleRate,
                     encoding, channel, path, format, lyrics;
        public Image? art;

        public object[] audioFileProperties = new object[14];
        public string[] audioInfo = new string[13];



        public Khi_Player_Audio_Info_Form(string? title, string? artist, string? album,
                                          string? trackNumber, string? genre, string? duration,
                                          string? bitrate, string? sampleRate, string? encoding, string? channel,
                                          string? path, string? format, string? lyrics, Image? art, bool darkMode)
        {

            InitializeComponent();

            if (darkMode)
            {
                this.BackColor = Color.FromArgb(41, 41, 41);
                this.ForeColor = Color.White;
                foreach (Control cont in this.Controls)
                {
                    cont.BackColor = Color.FromArgb(41, 41, 41);
                    cont.ForeColor = Color.White;

                }
                

            }

            audioArtBox.Image = art;
            textBox1.Text = title;
            textBox2.Text = artist;
            textBox3.Text = album;
            textBox4.Text = trackNumber;
            textBox5.Text = genre;
            textBox6.Text = duration;
            textBox7.Text = bitrate;
            textBox8.Text = sampleRate;
            textBox9.Text = encoding;
            textBox10.Text = channel;
            textBox11.Text = path;
            textBox12.Text = format;
            textBox13.Text = lyrics;
        }

        public void Khi_Player_Audio_Info_Form_Load(object sender, EventArgs e)
        {


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void Khi_Player_Audio_Info_Form_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
