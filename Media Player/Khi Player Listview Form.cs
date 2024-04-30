using Microsoft.Win32;
using NAudio.Wave;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Xml;
using static Khi_Player.AudioDataBase;
using static Khi_Player.SharedFieldsAndVariables;

namespace Khi_Player
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// the Default ini of the application, loads the data base and populates the ListView, 
        /// and initilizes the playlist buttons.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            DarkTitleBarClass.UseImmersiveDarkMode(Handle, true);
            musicListView.LargeImageList = new ImageList
            {
                ImageSize = new System.Drawing.Size(60, 60)
            };
            searchMusicListView.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            searchMusicListView.AutoCompleteSource = AutoCompleteSource.CustomSource;
            List<string?>? existingDatabases = new List<string?>();
            string[] tempExistingDatabases = System.IO.Directory.GetFiles(applicationPath, "*.xml*", SearchOption.TopDirectoryOnly);
            string? tempPath;
            foreach (string? database in tempExistingDatabases)
            {
                //just to make sure
                if (System.IO.Path.GetExtension(database).ToUpper() == ".XML" && database != allMusicDataBase)
                {
                    existingDatabases.Add(database);
                }
            }
            if (Settings1.Default.SortOrder != 3)
            {
                int order = Settings1.Default.SortOrder;
                SharedFieldsAndVariables.SortOrder = (SortOrders)order;
                int sortColumn;
                switch (order)
                {
                    case 0:
                        //this is sort based on title
                        sortColumn = 0;
                        break;
                    case 1:
                        //this is sort based on artist
                        sortColumn = 1;
                        break;
                    case 2:
                        //this is sort based on album
                        sortColumn = 2;
                        break;
                    default:
                        //this is sort based on title
                        sortColumn = 0;
                        break;
                }
            }
            if (File.Exists(allMusicDataBase))
            {
                string[][]? info;
                Image[]? arts;
                (info, arts) = AudioDataBase.MainDataBaseIni(SharedFieldsAndVariables.SortOrder);
                if (info != null)
                {
                    allMusicInfo = (string[][]?)info.Clone();
                    PopulateListView(ref allMusicInfo, ref arts, false);
                    SearchBarAutoCompleteSource(allMusicInfo);
                    KhiPlayer = new PlayBackFunction();
                }
                else
                {
                    string[][]? dataBaseInfo;
                    Image[]? Arts;

                    (dataBaseInfo, Arts) = FilterDuplicates.TryRepairingDataBase(SharedFieldsAndVariables.SortOrder);
                    allMusicInfo = (string[][])dataBaseInfo.Clone();
                    PopulateListView(ref allMusicInfo, ref Arts, false);
                    SearchBarAutoCompleteSource(allMusicInfo);
                    KhiPlayer = new PlayBackFunction();
                }
            }
            DynamicPlaylistsButtonsIni();
        }

        /// <summary>
        /// for when the program is started by openning an audio file and not opened directly
        /// it also has the codes for the default init. Meaning that the application pretty much starts 
        /// in default mode but is directly given an audio filepath to play (and add to the data base). 
        /// </summary>
        /// <param name="filePath"></param>
        public Form1(string filePath)
        {
            InitializeComponent();
        }

        PlayBackFunction KhiPlayer;

        /// <summary>
        /// to prepare the custom source for the search bar
        /// </summary>
        /// <param name="playlist"></param>
        public async void SearchBarAutoCompleteSource(string[][] playlist)
        {
            List<string?> tempTips = new List<string?>();
            await Task.Run(() =>
            {
                foreach (string[]? music in playlist)
                {
                    tempTips.Add(music[0]);
                    tempTips.Add(music[1]);
                    tempTips.Add(music[2]);
                }
            });
            searchMusicListView.AutoCompleteCustomSource.AddRange(tempTips.ToArray());
        }

        /// <summary>
        /// to avoid constantly repeating codes needed for populating musicListView with audio info from the data base
        /// </summary>
        /// <param name="musicInfos"></param>
        /// <param name="musicThumbnails"></param>
        /// <param name="noImageMode"></param>
        public void PopulateListView( ref string[][]? musicInfos, ref Image[]? musicThumbnails, bool noImageMode = false)
        {
            if (musicInfos != null && musicInfos.Length > 0)
            {
                musicListView.BeginUpdate();

                if (noImageMode)
                {
                    ListViewItem[] newItems = new ListViewItem[musicInfos.Length];
                    int x = 0;
                    foreach (string[]? music in musicInfos)
                    {
                        ListViewItem song = new ListViewItem(music);
                        song.Name = music[3];
                        song.ToolTipText = music[0] + System.Environment.NewLine + music[1] + System.Environment.NewLine + music[2];
                        newItems[x] = song;
                    }
                    musicListView.Items.AddRange(newItems);
                }
                else
                {
                    int i = 0;
                    if (musicListView.LargeImageList == null)
                    {
                        musicListView.LargeImageList = new ImageList
                        {
                            ImageSize = new System.Drawing.Size(60, 60)
                        };
                    }
                    if (musicListView.LargeImageList.Images.Count > 0)  // this is for the listview is gonna be appended and not cleared beforehand
                    {
                        i = musicListView.LargeImageList.Images.Count;
                    }

                    if (musicThumbnails != null)
                    {
                        musicListView.LargeImageList.Images.AddRange(musicThumbnails);
                    }
                    else
                    {

                    }
                    ListViewItem[] newItems = new ListViewItem[musicInfos.Length];
                    int x = 0;
                    foreach (string[]? music in musicInfos)
                    {
                        ListViewItem song = new ListViewItem(music, i);
                        song.Name = music[3];
                        song.ToolTipText = music[0] + System.Environment.NewLine + music[1] + System.Environment.NewLine + music[2];
                        newItems[x] = song;
                        i++;
                        x++;
                    }
                    musicListView.Items.AddRange(newItems);
                }
                musicListView.EndUpdate();
            }
        }

        /// <summary>
        /// to create the buttons for the dynamically created playlists
        /// </summary>
        public void DynamicPlaylistsButtonsIni()
        {
            List<string?>? existingDatabases = new List<string?>();
            string[] tempExistingDatabases = System.IO.Directory.GetFiles(applicationPath, "*.xml*", SearchOption.TopDirectoryOnly);

            foreach (string? database in tempExistingDatabases)
            {
                //just to make sure
                if (System.IO.Path.GetExtension(database).ToUpper() == ".XML" && database != allMusicDataBase)
                {
                    existingDatabases.Add(database);
                }
            }

            if (existingDatabases.Count > 0)
            {
                // now we have all of the existing xml files in the directory
                XmlDocument playlistDatabase;
                XmlElement playlistSongs;  //the document root node
                string? playlistName;
                List<string>? playlistNames = new List<string>();
                int i = 1;
                foreach (string? playlistPath in existingDatabases)
                {
                    playlistDatabase = new XmlDocument();
                    playlistDatabase.Load(playlistPath);
                    playlistSongs = playlistDatabase.DocumentElement;
                    playlistName = playlistSongs.GetAttribute("playlist");
                    playlistNames.Add(playlistName);
                    CreateDynamicPlaylistButton(playlistName);
                    playlistDatabase = null;
                }
            }
        }

        /// <summary>
        /// to enable or disable Darkmode. set <see langword="true"/> to activate DarkMode, and <see langword="false"/>to disable it.
        /// </summary>
        /// <param name="EnableDisable"></param>
        public void DarkMode(bool EnableDisable)
        {
            if (EnableDisable)
            {
                BackColor = Color.FromArgb(41, 41, 41);
                ForeColor = Color.White;

                foreach (Control cont in Controls)
                {
                    cont.BackColor = Color.FromArgb(41, 41, 41);
                    cont.ForeColor = Color.White;
                }
                foreach (Control cont in musicControlBar.Controls)
                {
                    cont.BackColor = Color.FromArgb(41, 41, 41);
                    cont.ForeColor = Color.White;
                }
                foreach (Control cont in mediaPlayerPanel.Controls)
                {
                    cont.BackColor = Color.FromArgb(41, 41, 41);
                    cont.ForeColor = Color.White;
                }
                foreach (ToolStripItem cont in playlistToolbar.Items)
                {
                    cont.BackColor = Color.FromArgb(41, 41, 41);
                    cont.ForeColor = Color.White;
                }
                foreach (ToolStripItem cont in userBar.Items)
                {
                    cont.BackColor = Color.FromArgb(41, 41, 41);
                    cont.ForeColor = Color.White;
                }
                foreach (ToolStripItem cont in fileToolStripMenuItem.DropDown.Items)
                {
                    cont.BackColor = Color.FromArgb(41, 41, 41);
                    cont.ForeColor = Color.White;
                }
                foreach (ToolStripItem cont in viewToolStripMenuItem.DropDown.Items)
                {
                    cont.BackColor = Color.FromArgb(41, 41, 41);
                    cont.ForeColor = Color.White;
                }
                foreach (ToolStripItem cont in listToolStripMenuItem.DropDown.Items)
                {
                    cont.BackColor = Color.FromArgb(41, 41, 41);
                    cont.ForeColor = Color.White;
                }
                foreach (ToolStripItem cont in rightClickMenu.Items)
                {
                    cont.BackColor = Color.FromArgb(41, 41, 41);
                    cont.ForeColor = Color.White;
                }
                foreach (ToolStripItem cont in addToPlaylistButton.DropDown.Items)
                {
                    cont.BackColor = Color.FromArgb(41, 41, 41);
                    cont.ForeColor = Color.White;
                }

                addToPlaylistButton.DropDown.BackColor = Color.FromArgb(41, 41, 41);
                addToPlaylistButton.DropDown.ForeColor = Color.White;
                viewToolStripMenuItem.DropDown.BackColor = Color.FromArgb(41, 41, 41);
                viewToolStripMenuItem.DropDown.ForeColor = Color.White;
                SortListMenuItem.DropDown.BackColor = Color.FromArgb(41, 41, 41);
                SortListMenuItem.DropDown.ForeColor = Color.White;
                fileToolStripMenuItem.DropDown.BackColor = Color.FromArgb(41, 41, 41);
                fileToolStripMenuItem.DropDown.ForeColor = Color.White;
                listToolStripMenuItem.DropDown.BackColor = Color.FromArgb(41, 41, 41);
                listToolStripMenuItem.DropDown.ForeColor = Color.White;
                editPlaylistButton.BackColor = Color.FromArgb(41, 41, 41);
                editPlaylistButton.ForeColor = Color.White;
                applyEditStripButton.BackColor = Color.FromArgb(41, 41, 41);
                applyEditStripButton.ForeColor = Color.White;
                //for Icons

                toggleLoop.BackgroundImage = Properties.Resources.Loop_Dark_Mode;
                PlayPause.BackgroundImage = Properties.Resources.Play_Pause__Dark_Mode;
                skip.BackgroundImage = Properties.Resources.Skip_Dark_Mode;
                previous.BackgroundImage = Properties.Resources.Previous_Dark_Mode;
                toggleShuffle.BackgroundImage = Properties.Resources.Shuffle_Dark_Mode;
                stopButton.BackgroundImage = Properties.Resources.Stop_Dark_Mode;

                isDarkMode = true;
            }
            //turns dark mode to light mode
            else
            {
                BackColor = Color.White;
                ForeColor = Color.FromArgb(41, 41, 41);
                foreach (Control cont in Controls)
                {
                    cont.BackColor = Color.White;
                    cont.ForeColor = Color.FromArgb(41, 41, 41);
                }
                foreach (Control cont in musicControlBar.Controls)
                {
                    cont.BackColor = Color.White;
                    cont.ForeColor = Color.FromArgb(41, 41, 41);
                }
                foreach (Control cont in mediaPlayerPanel.Controls)
                {
                    cont.BackColor = Color.White;
                    cont.ForeColor = Color.FromArgb(41, 41, 41);
                }
                foreach (ToolStripItem cont in playlistToolbar.Items)
                {
                    cont.BackColor = Color.White;
                    cont.ForeColor = Color.FromArgb(41, 41, 41);
                }
                foreach (ToolStripItem cont in userBar.Items)
                {
                    cont.BackColor = Color.White;
                    cont.ForeColor = Color.FromArgb(41, 41, 41);
                }
                foreach (ToolStripItem cont in fileToolStripMenuItem.DropDown.Items)
                {
                    cont.BackColor = Color.White;
                    cont.ForeColor = Color.FromArgb(41, 41, 41);
                }
                foreach (ToolStripItem cont in viewToolStripMenuItem.DropDown.Items)
                {
                    cont.BackColor = Color.White;
                    cont.ForeColor = Color.FromArgb(41, 41, 41);
                }
                foreach (ToolStripItem cont in listToolStripMenuItem.DropDown.Items)
                {
                    cont.BackColor = Color.White;
                    cont.ForeColor = Color.FromArgb(41, 41, 41);
                }
                foreach (ToolStripItem cont in rightClickMenu.Items)
                {
                    cont.BackColor = Color.White;
                    cont.ForeColor = Color.FromArgb(41, 41, 41);
                }
                foreach (ToolStripItem cont in addToPlaylistButton.DropDownItems)
                {
                    cont.BackColor = Color.White;
                    cont.ForeColor = Color.FromArgb(41, 41, 41);
                }
                addToPlaylistButton.DropDown.BackColor = Color.White;
                addToPlaylistButton.DropDown.ForeColor = Color.FromArgb(41, 41, 41);
                viewToolStripMenuItem.DropDown.BackColor = Color.White;
                viewToolStripMenuItem.DropDown.ForeColor = Color.FromArgb(41, 41, 41);
                SortListMenuItem.DropDown.BackColor = Color.White;
                SortListMenuItem.DropDown.ForeColor = Color.FromArgb(41, 41, 41);
                fileToolStripMenuItem.DropDown.BackColor = Color.White;
                fileToolStripMenuItem.DropDown.ForeColor = Color.FromArgb(41, 41, 41);
                listToolStripMenuItem.DropDown.BackColor = Color.White;
                listToolStripMenuItem.DropDown.ForeColor = Color.FromArgb(41, 41, 41);
                editPlaylistButton.BackColor = Color.White;
                editPlaylistButton.ForeColor = Color.FromArgb(41, 41, 41);
                applyEditStripButton.BackColor = Color.White;
                applyEditStripButton.ForeColor = Color.FromArgb(41, 41, 41);

                //For Icons
                toggleLoop.BackgroundImage = Properties.Resources.loop;
                PlayPause.BackgroundImage = Properties.Resources.Play_Pause;
                skip.BackgroundImage = Properties.Resources.Skip;
                previous.BackgroundImage = Properties.Resources.Previous;
                toggleShuffle.BackgroundImage = Properties.Resources.Shuffle_Light_Mode;
                stopButton.BackgroundImage = Properties.Resources.Stop_Light_Mode;

                isDarkMode = false;
            }
        }

        /// <summary>
        /// Should call after calling for Playback to update UI related to music infos
        /// </summary>
        public void HandlePlaybackUI ()
        {
            if (seekBar.Enabled == false) { seekBar.Enabled = true; }
            PlayBackFunction.SongTimeValue(true);

            seekBar.Maximum = seekbarMax;
            seekBar.Value = timeValue;
            songLengthLabel.Text = songLength;
            currentTimeLabel.Text = currenSongTimePosition;
            songSeekTimer.Enabled = true;
            borderLabel.Visible = true;
            songTitleLabel.Text = currentlyPlayingSongInfo[0];
            songArtistLabel.Text = currentlyPlayingSongInfo[1];
            songAlbumLabel.Text = currentlyPlayingSongInfo[2];

            editLyricsToolStripButton.Enabled = true;
            editLyricsToolStripButton.Visible = true;

            if (Status == States.Playing)
            {
                if (currentlyPlayingSongPic != null)
                {
                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Image.Dispose();
                        //pictureBox1.Image = null;
                    }
                    pictureBox1.Image = currentlyPlayingSongPic;
                }
                else
                {
                    pictureBox1.Image = Properties.Resources.Khi_Player;
                }
            }
            lyricsTextBox.Clear();
            lyricsTextBox.Text = lyrics;
        }

        /// <summary>
        /// Creates a button using the playlistName, attaches it to a common event handler
        /// and adds it to the playlistToolbar
        /// </summary>
        /// <param name="playlistName"></param>
        public async void CreateDynamicPlaylistButton(string playlistName)
        {
            ToolStripButton newPlaylistButton = new ToolStripButton();
            newPlaylistButton.AutoSize = false;
            newPlaylistButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            newPlaylistButton.Text = playlistName;
            newPlaylistButton.Size = new System.Drawing.Size(90, 19);
            newPlaylistButton.Name = playlistName + "Button";
            newPlaylistButton.AutoToolTip = false;
            newPlaylistButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            newPlaylistButton.Enabled = true;
            newPlaylistButton.Visible = true;
            newPlaylistButton.Tag = (string)playlistName;

            int index = playlistToolbar.Items.Count - 3; //inserts it before the rename textbox
            playlistToolbar.Items.Insert(index, newPlaylistButton);

            //Add to the righClickMenu for Adding to Playlist
            ToolStripButton addToNewPlaylistButton = new ToolStripButton();
            addToNewPlaylistButton.Name = "addTo" + playlistName;
            addToNewPlaylistButton.Text = "Add to " + playlistName;
            addToNewPlaylistButton.Tag = playlistName; //to have an easier time later
            addToNewPlaylistButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            addToNewPlaylistButton.AutoToolTip = false;
            addToNewPlaylistButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            addToNewPlaylistButton.Enabled = true;
            addToNewPlaylistButton.Visible = true;

            if (isDarkMode)
            {
                newPlaylistButton.BackColor = Color.FromArgb(41, 41, 41);
                newPlaylistButton.ForeColor = Color.White;
                addToNewPlaylistButton.BackColor = Color.FromArgb(41, 41, 41);
                addToNewPlaylistButton.ForeColor = Color.White;
            }
            else
            {
                newPlaylistButton.BackColor = Color.White;
                newPlaylistButton.ForeColor = Color.FromArgb(41, 41, 41);
                addToNewPlaylistButton.BackColor = Color.White;
                addToNewPlaylistButton.ForeColor = Color.FromArgb(41, 41, 41);
            }

            addToNewPlaylistButton.Click += DynamicAddToPlaylistButton_Click;
            addToPlaylistButton.DropDownItems.Add(addToNewPlaylistButton);
            newPlaylistButton.Click += DynamicPlaylistButton_Click;

            if (addToPlaylistButton.Enabled == false || addToPlaylistButton.Visible == false)
            {
                addToPlaylistButton.Enabled = true;
                addToPlaylistButton.Visible = true;
            }
        }

        /// <summary>
        /// For making the Form follow the windows theme. Copy Pasted from StackOverflow: 
        /// https://stackoverflow.com/questions/11862315/changing-the-color-of-the-title-bar-in-winform
        /// </summary>
        internal class DarkTitleBarClass
        {
            [DllImport("dwmapi.dll")]
            private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr,
            ref int attrValue, int attrSize);

            private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
            private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

            internal static bool UseImmersiveDarkMode(IntPtr handle, bool enabled)
            {
                if (IsWindows10OrGreater(17763))
                {
                    var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
                    if (IsWindows10OrGreater(18985))
                    {
                        attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
                    }
                    int useImmersiveDarkMode = enabled ? 1 : 0;
                    return DwmSetWindowAttribute(handle, attribute, ref useImmersiveDarkMode, sizeof(int)) == 0;
                }
                return false;
            }

            private static bool IsWindows10OrGreater(int build = -1)
            {
                return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= build;
            }
        }

        /// <summary>
        /// For getting most of the important metadata of the Audio and showing it in a seperate Form, Khi Player Audio Info Form
        /// will later add Editing functions to it
        /// </summary>
        public class AudioInfo : IDisposable
        {
            private bool disposed;
            public string? title, artist, album, trackNumber, genre, duration, bitrate, sampleRate,
                     encoding, channel, path, format, lyrics;
            public Image? art;
            public object[]? selectedAudioInfo = new object[14];
            public string[]? musicInfo = new string[13];

            ///<summary>
            /// Gets the various tags of the Audio file at the provided location
            ///</summary>
            public AudioInfo(string audioPath)
            {
                TagLib.File musicTags = TagLib.File.Create(audioPath);
                //Check to see if the file or file tags are corrupted
                if (musicTags.PossiblyCorrupt)
                {
                    var corruptionReasons = musicTags.CorruptionReasons.ToArray();
                    // Add that it should return this string array if it is corrupted
                }
                else
                {
                    //getting a picture of the audio file
                    var tempPics = musicTags.Tag.Pictures;
                    if (tempPics.Length > 0)
                    {
                        using (MemoryStream picConverter = new MemoryStream(tempPics[0].Data.Data))
                        {
                            art = Image.FromStream(picConverter);
                        }
                    }
                    else { art = Khi_Player.Properties.Resources.MusicArt_NoCover; }

                    //to get the duration of the audio
                    using (AudioFileReader durationReader = new AudioFileReader(audioPath))
                    {
                        if (musicTags.Properties.Duration.Hours < 1)
                        {
                            var mins = durationReader.TotalTime.Minutes.ToString();
                            var secs = durationReader.TotalTime.Seconds.ToString("00");
                            duration = mins + ":" + secs;
                        }
                        else
                        {
                            var hours = durationReader.TotalTime.Hours.ToString();
                            var mins = durationReader.TotalTime.Minutes.ToString("00");
                            var secs = durationReader.TotalTime.Seconds.ToString("00");
                            duration = hours + ":" + mins + ":" + secs;
                        }
                    }
                    // to get bitrate
                    bitrate = musicTags.Properties.AudioBitrate.ToString() + "kbps";
                    //to get sampling rate
                    sampleRate = musicTags.Properties.AudioSampleRate.ToString() + " Hz";
                    //to get encoding
                    var audioFileICodec = musicTags.Properties.Codecs.GetEnumerator();
                    audioFileICodec.MoveNext();
                    encoding = audioFileICodec.Current.Description;
                    // to get the number of audio channels (Mono vs Stereo)
                    channel = musicTags.Properties.AudioChannels.ToString();

                    //to get track number in Album
                    uint tempTrack = musicTags.Tag.Track;
                    if (tempTrack == 0) { trackNumber = "0"; }
                    else { trackNumber = tempTrack.ToString(); }

                    path = audioPath;
                    var tempformat = audioPath.Split('.');
                    if (tempformat.Length > 1) { format = audioPath.Split('.')[tempformat.Length - 1].ToUpper(); }
                    else { format = audioPath.Split('.')[1].ToUpper(); }
                    if (musicTags.Tag.Title == null)
                    {
                        System.IO.FileInfo sth = new System.IO.FileInfo(path);
                        title = sth.Name;
                    }
                    else { title = musicTags.Tag.Title; }
                    //for artists
                    var allArtists = musicTags.Tag.Performers;
                    if (allArtists.Length == 0) { artist = ""; }
                    if (allArtists.Length > 1)
                    {
                        System.Windows.Forms.TextBox tempText = new System.Windows.Forms.TextBox();
                        foreach (var oneartist in allArtists)
                        {
                            tempText.AppendText(oneartist);
                            tempText.AppendText(" ");
                        }
                        artist = tempText.Text;
                        tempText.Dispose();
                    }
                    else { artist = musicTags.Tag.FirstPerformer; }

                    //For Album
                    if (musicTags.Tag.Album == null) { album = ""; }
                    else { album = musicTags.Tag.Album; }
                    //For genre
                    if (musicTags.Tag.Genres.Length == 0) { genre = ""; }
                    else { genre = musicTags.Tag.Genres[0]; }
                    var temp = musicTags.Tag.Lyrics;
                    if (temp != null) { lyrics = musicTags.Tag.Lyrics.ReplaceLineEndings(); }
                    // all of the infos except art in a string array
                    musicInfo[0] = title;
                    musicInfo[1] = artist;
                    musicInfo[2] = album;
                    musicInfo[3] = trackNumber;
                    musicInfo[4] = genre;
                    musicInfo[5] = duration;
                    musicInfo[6] = bitrate;
                    musicInfo[7] = sampleRate;
                    musicInfo[8] = encoding;
                    musicInfo[9] = channel;
                    musicInfo[10] = path;
                    musicInfo[11] = format;
                    musicInfo[12] = lyrics;

                    //all the infos including art in an object array
                    // TURN THIS into a seperate class later
                    for (int x = 0; x < 13; x++)
                    {
                        selectedAudioInfo[x] = (object)musicInfo[x];
                    }
                    selectedAudioInfo[13] = (object)art;
                }
                musicTags.Dispose();
            }

            ~AudioInfo()
            {
                this.Dispose(false);
            }

            /// <summary>
            /// The dispose method that implements IDisposable.
            /// </summary>
            public void Dispose()
            {
                musicInfo = null;
                selectedAudioInfo = null;
                title = null;
                artist = null;
                album = null;
                art = null;
                bitrate = null;
                trackNumber = null;
                sampleRate = null;
                encoding = null;
                channel = null;
                path = null;
                format = null;
                lyrics = null;
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// The virtual dispose method that allows
            /// classes inherithed from this one to dispose their resources.
            /// </summary>
            /// <param name="disposing"></param>
            protected virtual void Dispose(bool disposing)
            {
                if (!disposed)
                {
                    if (disposing)
                    {
                        // Dispose managed resources here.
                        musicInfo = null;
                        selectedAudioInfo = null;
                        title = null;
                        artist = null;
                        album = null;
                        art = null;
                        bitrate = null;
                        trackNumber = null;
                        sampleRate = null;
                        encoding = null;
                        channel = null;
                        path = null;
                        format = null;
                        lyrics = null;
                    }
                    // Dispose unmanaged resources here.
                }
                disposed = true;
            }
        }
       
        /// <summary>
        /// for now a faulty class; was meant to scan the user's system for audio files
        /// </summary>
        public class MusicScanner
        {
            public static void ScanSystemForAudioFiles()
            {
                string[][]? musicsPaths = null;
                List<string> tempPathsList = new List<string>();
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                List<string?> availableDrives = new List<string?>();
                foreach (DriveInfo drive in allDrives)
                {
                    if (drive.IsReady)
                    {
                        if (drive.DriveType == DriveType.Fixed)
                        {
                            availableDrives.Add(drive.Name);
                        }
                    }
                }

                if (availableDrives.Count > 0)
                {
                    List<string[]?> allDirectories = new List<string[]?>();
                    foreach (string? drive in availableDrives)
                    {
                        if (drive == "C:\\")
                        {
                            string[]? tempDir;
                            tempDir = System.IO.Directory.GetDirectories("C:\\Users");
                            foreach (string? dir in tempDir)
                            {
                                if (dir.Contains("Download") || dir.Contains("Music"))
                                {
                                    allDirectories.Add(System.IO.Directory.GetDirectories("C:\\Users"));
                                }
                            }
                        }
                        else
                        {
                            allDirectories.Add(System.IO.Directory.GetDirectories(drive));
                        }
                    }

                    foreach (string[]? directories in allDirectories)
                    {
                        List<string[]> temp = new List<string[]>();
                        foreach (string? directory in directories)
                        {
                            //to skip system folders. there are probably better ways to do this, but for now this should suffice
                            if (directory.Contains("Windows") || directory.Contains("Program Files (x86)") || directory.Contains("Program Files") || directory.Contains("PerfLogs") 
                                || directory.Contains("ProgramData") || directory.Contains("Recycle.Bin") || directory.Contains("Config") || directory.Contains("Application Data") 
                                || directory.Contains("System") || directory.Contains("Volume Information"))
                            { continue; }
                            else
                            {
                                temp.Add(System.IO.Directory.GetFiles(directory, "*.mp3*", SearchOption.AllDirectories));
                                temp.Add(System.IO.Directory.GetFiles(directory, "*.wav*", SearchOption.AllDirectories));
                                temp.Add(System.IO.Directory.GetFiles(directory, "*.flac*", SearchOption.AllDirectories));
                                temp.Add(System.IO.Directory.GetFiles(directory, "*.aiff*", SearchOption.AllDirectories));
                                temp.Add(System.IO.Directory.GetFiles(directory, "*.wma*", SearchOption.AllDirectories));
                                temp.Add(System.IO.Directory.GetFiles(directory, "*.pcm*", SearchOption.AllDirectories));
                                temp.Add(System.IO.Directory.GetFiles(directory, "*.aac*", SearchOption.AllDirectories));
                                temp.Add(System.IO.Directory.GetFiles(directory, "*.oog*", SearchOption.AllDirectories));
                                temp.Add(System.IO.Directory.GetFiles(directory, "*.alac*", SearchOption.AllDirectories));

                                List<string?> tempDirectoryFiles = new List<string?>();

                                foreach (string[] pathArray in temp)
                                {
                                    foreach (string? path in pathArray)
                                    {
                                        tempDirectoryFiles.Add(path);
                                    }
                                }

                                if (tempDirectoryFiles.Count > 0)
                                {
                                    string? tempPath;
                                    foreach (var file in tempDirectoryFiles)
                                    {
                                        tempPath = System.IO.Path.GetExtension(file).Trim().ToLower();
                                        if (tempPath == ".mp3" || tempPath == ".wav" || tempPath == ".flac" || tempPath == ".aiff" || tempPath == ".wma" || tempPath == ".pcm" || tempPath == ".aac" || tempPath == ".oog" || tempPath == ".alac")
                                        { tempPathsList.Add(file); }

                                    }
                                }
                            }
                        }
                    }
                }

                string[] addedFiles = tempPathsList.ToArray();
                string[][]? AddedMusicInfo;
                Image[]? AddedMusicArts;

                (AddedMusicInfo, AddedMusicArts) = AudioDataBase.AddSongsToAudioDataBase(addedFiles, true);
            }

            /// <summary>
            /// scans the system hard drives, first for available and ready drives, and then goes through the directories in search of audio 
            /// files, sends the found items to the AudioDataBase Class and populates the allMusicInfo Array
            /// </summary>
            /// <returns></returns>
            public static async void ScanSystemForAudioFilesAsync()
            {
                string[][]? musicsPaths = null;
                List<string> tempPathsList = new List<string>();

                await Task.Run(async () =>
                {
                    DriveInfo[] allDrives = DriveInfo.GetDrives();

                    List<string?> availableDrives = new List<string?>();
                    foreach (DriveInfo drive in allDrives)
                    {
                        if (drive.IsReady)
                        {
                            if (drive.DriveType == DriveType.Fixed)
                            {
                                availableDrives.Add(drive.Name);
                            }
                        }
                    }

                    if (availableDrives.Count > 0)
                    {
                        List<string[]?> allDirectories = new List<string[]?>();
                        foreach (string? drive in availableDrives)
                        {
                            allDirectories.Add(System.IO.Directory.GetDirectories(drive));
                        }

                        foreach (string[]? directories in allDirectories)
                        {
                            List<string[]> temp = new List<string[]>();
                            foreach (string? directory in directories)
                            {
                                //to skip system folders. there are probably better ways to do this, but for now this should suffice
                                if (directory.Contains("Windows") || directory.Contains("Program Files (x86)") || directory.Contains("Program Files") || directory.Contains("PerfLogs") || directory.Contains("ProgramData") || directory.Contains("Recycle.Bin"))
                                { continue; }
                                else
                                {
                                    temp.Add(System.IO.Directory.GetFiles(directory, "*.mp3*", SearchOption.AllDirectories));
                                    temp.Add(System.IO.Directory.GetFiles(directory, "*.wav*", SearchOption.AllDirectories));
                                    temp.Add(System.IO.Directory.GetFiles(directory, "*.flac*", SearchOption.AllDirectories));
                                    temp.Add(System.IO.Directory.GetFiles(directory, "*.aiff*", SearchOption.AllDirectories));
                                    temp.Add(System.IO.Directory.GetFiles(directory, "*.wma*", SearchOption.AllDirectories));
                                    temp.Add(System.IO.Directory.GetFiles(directory, "*.pcm*", SearchOption.AllDirectories));
                                    temp.Add(System.IO.Directory.GetFiles(directory, "*.aac*", SearchOption.AllDirectories));
                                    temp.Add(System.IO.Directory.GetFiles(directory, "*.oog*", SearchOption.AllDirectories));
                                    temp.Add(System.IO.Directory.GetFiles(directory, "*.alac*", SearchOption.AllDirectories));

                                    List<string?> tempDirectoryFiles = new List<string?>();

                                    foreach (string[] pathArray in temp)
                                    {
                                        foreach (string? path in pathArray)
                                        {
                                            tempDirectoryFiles.Add(path);
                                        }
                                    }

                                    if (tempDirectoryFiles.Count > 0)
                                    {
                                        string? tempPath;
                                        foreach (var file in tempDirectoryFiles)
                                        {
                                            tempPath = System.IO.Path.GetExtension(file).Trim().ToLower();
                                            if (tempPath == ".mp3" || tempPath == ".wav" || tempPath == ".flac" || tempPath == ".aiff" || tempPath == ".wma" || tempPath == ".pcm" || tempPath == ".aac" || tempPath == ".oog" || tempPath == ".alac")
                                            { tempPathsList.Add(file); }
                                        }
                                    }

                                }
                            }
                        }
                    }
                });

                string[] addedFiles = tempPathsList.ToArray();
                string[][]? AddedMusicInfo;
                Image[]? AddedMusicArts;
                (AddedMusicInfo, AddedMusicArts) = AudioDataBase.AddSongsToAudioDataBase(addedFiles, true, SharedFieldsAndVariables.SortOrder);
            }
            //GC.Collect();          
        }
              
        private void Form1_Load(object sender, EventArgs e)
        {
            //Dark Mode
            if (Settings1.Default.DarkMode == true)
            {
                DarkMode(true);
                isDarkMode = true;
            }
            else
            {
                DarkMode(false);
                isDarkMode = false;
            }
            /* // NOT WORKING FOR NOW, DON'T UNCOMMENT OR USE THIS CLASS
            //To Scan System
            MusicScanner.ScanSystemForAudioFiles();
            var tempImages = AudioDataBase.GetMusicThumbnails(allMusicInfo);
            FormEditor.PopulateListView(ref musicListView, ref allMusicInfo, ref tempImages, false);
            FormEditor.SearchBarAutoCompleteSource(allMusicInfo);
            //for disposal
            tempImages = null;
            */
        }

        public void DynamicPlaylistButton_Click(object sender, EventArgs e)
        {
            ToolStripButton clickedButton = sender as ToolStripButton;
            string? name = clickedButton.Text;
            string playlistPath = applicationPath + name + ".xml";
            Image[]? playlistImages;

            if (System.IO.File.Exists(playlistPath))
            {
                int playlistsCount = PlaylistsDict.Count;
                string[][]? newPlaylistInfo;
                (name, newPlaylistInfo) = AudioDataBase.ReadPlaylist(playlistPath);

                if (newPlaylistInfo != null)
                {
                    CurrentPlaylist = Playlists.DynamicPlaylists; //the dynamically created buttons had their tags set to the playlist name they correspond to, and are
                                                                  // added to PlaylistsDic dictionary
                    currentPlaylistLabel.Text = name;
                    if (SharedFieldsAndVariables.SortOrder != SortOrders.CustomSort)
                    {
                        int order = (int)SharedFieldsAndVariables.SortOrder;
                        int sortColumn;
                        switch (order)
                        {
                            case 0:
                                //this is sort based on title
                                sortColumn = 0;
                                break;
                            case 1:
                                //this is sort based on artist
                                sortColumn = 1;
                                break;
                            case 2:
                                //this is sort based on album
                                sortColumn = 2;
                                break;
                            default:
                                //this is sort based on title
                                sortColumn = 0;
                                break;
                        }
                        newPlaylistInfo = PlayList.SortPlaylist(newPlaylistInfo, sortColumn);
                    }
                    playlistImages = (Image[])AudioDataBase.GetMusicThumbnails(newPlaylistInfo).Clone();
                    if (!PlaylistsDict.ContainsKey((string)clickedButton.Tag))
                    {
                        PlaylistsDict.Add((string)clickedButton.Tag, (string[][])newPlaylistInfo.Clone());
                    }
                    CurrentPlaylistName = (string)name.Clone();
                    musicListView.Items.Clear();
                    musicListView.LargeImageList.Images.Clear();
                    PopulateListView(ref newPlaylistInfo, ref playlistImages);
                    searchMusicListView.AutoCompleteCustomSource.Clear();
                    SearchBarAutoCompleteSource(newPlaylistInfo);
                    listUpdated = true;
                }
            }
            else { System.Windows.Forms.MessageBox.Show("Please Add songs to this playlist"); }
        }

        public void DynamicAddToPlaylistButton_Click(object sender, EventArgs e)
        {
            ToolStripButton clickedButton = sender as ToolStripButton;
            string? PlaylistName = clickedButton.Tag as string;
            string? playlistPath = applicationPath + PlaylistName + ".xml";
            List<string[]> tempPlaylistItems = new List<string[]>();

            if (musicListView.SelectedItems.Count >= 1)
            {
                if (CurrentPlaylist == Playlists.allSongs)
                {
                    for (int i = 0; i < musicListView.SelectedItems.Count; i++)
                    {
                        tempPlaylistItems.Add(allMusicInfo[musicListView.SelectedItems[i].Index]);
                    }
                }
                else
                {
                    string[][]? tempCurrentPlaylist = PlayList.GetCurrentPlaylist();
                    for (int i = 0; i < musicListView.SelectedItems.Count; i++)
                    {
                        tempPlaylistItems.Add(tempCurrentPlaylist[musicListView.SelectedItems[i].Index]);
                    }
                }
                AudioDataBase.WriteAudioDataBase(tempPlaylistItems.ToArray(), playlistPath, PlaylistName);
                listUpdated = true;
                //for disposal
                tempPlaylistItems.Clear();
            }
        }

        /// <summary>
        /// for picking the name and creation of a playlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (renameTextBox.TextBox.Text.Length > 0 && renameTextBox.TextBox.Text != "Playlist Name")
                {
                    bool isAcceptable = true;
                    bool alreadyExists = false;
                    char[] unacceptableChars = { '/', '\\', ':', '*', '?', '\"', '<', '>', '|' };

                    foreach (char unacceptableChar in unacceptableChars)
                    {
                        if (renameTextBox.Text.Contains(unacceptableChar)) { isAcceptable = false; }
                    }
                    if (System.IO.File.Exists(applicationPath + renameTextBox.Text + ".xml")) { alreadyExists = true; }
                    //because playlistname will also be used to create the database
                    if (isAcceptable && alreadyExists == false)
                    {
                        string playlistName = renameTextBox.Text;
                        renameTextBox.TextBox.Clear();
                        renameTextBox.Visible = false;
                        renameTextBox.TextBox.Text = "Playlist Name";
                        AudioDataBase.CreatePlaylistDataBase(playlistName);
                        CreateDynamicPlaylistButton(playlistName);
                    }
                    else if (isAcceptable == false)
                    {
                        System.Windows.Forms.MessageBox.Show("Playlist name cannot contain \r\n '/', '\\', ':', '*', '?', '\"', '<', '>', '|' ");
                        renameTextBox.TextBox.Text = "Playlist Name";
                    }
                    else if (alreadyExists == true)
                    {
                        System.Windows.Forms.MessageBox.Show("A Playlist with that name already exists, please choose another name");
                        renameTextBox.TextBox.Text = "Playlist Name";
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Type Playlist's Name");
                    renameTextBox.TextBox.Text = "Playlist Name";
                }
            }
            if (e.KeyChar == (char)Keys.Escape)
            {
                renameTextBox.TextBox.Clear();
                renameTextBox.Visible = false;
            }
        }

        private void addPlaylistButton_Click(object sender, EventArgs e)
        {
            renameTextBox.Enabled = true;
            renameTextBox.Visible = true;
            renameTextBox.Focus();
        }


        private void RescanMenuItem_Click(object sender, EventArgs e)
        {
            MemoryManageTimer.Stop();
            musicListView.BeginUpdate();
            musicListView.LargeImageList.Images.Clear();
            musicListView.Items.Clear();

            string[][]? dataBaseInfo;
            Image[]? Arts;

            AudioDataBase.RemoveInvalidDatabaseElements();
            (dataBaseInfo, Arts) = FilterDuplicates.TryRepairingDataBase();
            allMusicInfo = (string[][])dataBaseInfo.Clone();

            if (dataBaseInfo.Length > 0)
            {
                int x = 0;
                foreach (var music in dataBaseInfo)
                {
                    musicListView.LargeImageList.Images.Add(Arts[x]);
                    ListViewItem song = new ListViewItem(music, x);
                    song.ToolTipText = music[0] + System.Environment.NewLine + music[1] + System.Environment.NewLine + music[2];
                    song.Name = music[3];
                    musicListView.Items.Add(song);
                    x++;
                }
                KhiPlayer = new PlayBackFunction();
            }
            else { System.Windows.Forms.MessageBox.Show("Please add music "); }

            musicListView.EndUpdate();
            MemoryManageTimer.Start();
        }

        private void ClearListItem_Click(object sender, EventArgs e)
        {
            //this was just here for testing
            // should remove it
            musicListView.Items.Clear();
        }

        private async void musicListView_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            MemoryManageTimer.Stop();
            bool dragDropSuccess = false;
            int listviewItemsCount = musicListView.Items.Count;
            string[][]? AddedMusicInfo;
            Image[]? AddedMusicArts = new Image[1];

            await Task.Run(() =>
            {
                string[] tempDraggedFiles = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop);
                List<string> tempPathList = new List<string>();
                string[] draggedFiles;

                foreach (string filePath in tempDraggedFiles)
                {
                    var temp = System.IO.Path.GetExtension(filePath).Trim().ToLower();
                    if (temp == ".mp3" || temp == ".wav" || temp == ".flac" || temp == ".aiff" || temp == ".wma" || temp == ".pcm" || temp == ".aac" || temp == ".oog" || temp == ".alac")
                    { tempPathList.Add(filePath); }
                }
                draggedFiles = tempPathList.ToArray();               
                if (listviewItemsCount == 0)
                {
                    (AddedMusicInfo, AddedMusicArts) = AudioDataBase.AddSongsToAudioDataBase(draggedFiles, false, SharedFieldsAndVariables.SortOrder);
                    allMusicInfo = (string[][]?)AddedMusicInfo.Clone();
                }
                else
                {
                    (AddedMusicInfo, AddedMusicArts) = AudioDataBase.AddSongsToAudioDataBase(draggedFiles, true, SharedFieldsAndVariables.SortOrder);
                    allMusicInfo = (string[][]?)AddedMusicInfo.Clone();
                }
                //for disposal
                tempPathList.Clear();
            });

            PopulateListView(ref allMusicInfo, ref AddedMusicArts, false);
            searchMusicListView.AutoCompleteCustomSource.Clear();
            SearchBarAutoCompleteSource(allMusicInfo);
            listUpdated = true;
            MemoryManageTimer.Start();
        }

        private void toggleLoop_Click(object sender, EventArgs e)
        {
            if (isLoopEnabled == false)
            {
                isLoopEnabled = true;
                if (isDarkMode == true)
                {
                    toggleLoop.BackgroundImage = Properties.Resources.Loop_Single_Dark_Mode;
                }
                else
                {
                    toggleLoop.BackgroundImage = Properties.Resources.Loop_Single_Light_Mode;
                }

                LoopState = LoopStates.SingleSongLoop;
                toggleLoop.ForeColor = Color.Blue;
                toggleLoop.FlatAppearance.BorderColor = Color.Blue;
                toggleLoop.FlatAppearance.BorderSize = 1;
            }
            else if (isLoopEnabled = true && LoopState == LoopStates.SingleSongLoop)
            {
                LoopState = LoopStates.PlaylistLoop;

                if (isDarkMode == true)
                {
                    toggleLoop.BackgroundImage = Properties.Resources.Loop_Dark_Mode;
                }
                else
                {
                    toggleLoop.BackgroundImage = Properties.Resources.loop;
                }
            }
            else if (isLoopEnabled = true && LoopState == LoopStates.PlaylistLoop)
            {
                isLoopEnabled = false;
                LoopState = LoopStates.NoLoop;
                toggleLoop.ForeColor = default;
                toggleLoop.FlatAppearance.BorderColor = default;
                toggleLoop.FlatAppearance.BorderSize = 0;
            }
        }

        private void musicListView_MouseDown(object sender, MouseEventArgs e)
        {
            //codes that are no longer relavent
            /*
            if (e.Button == MouseButtons.Right)
            {
                if (e.Location.IsEmpty == false)
                {
                    // FOR DOING STUFF TO THAT 1 PARTICULAR ITEM
                    musicListView.FindNearestItem(SearchDirectionHint.Left, e.Location);
                }
            } */

            var clicked = musicListView.GetItemAt(e.X, e.Y);
            if (musicListView.GetItemAt(e.X, e.Y) != null)
            {
                currentlySelectedSongIndex = clicked.Index;
            }
        }

        private void musicListView_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.KeyState == ((int)DragDropKeyStates.LeftMouseButton))
            {
                e.Effect = System.Windows.Forms.DragDropEffects.Move;
            }
        }

        private void showAlbumArtMenuItem_Click(object sender, EventArgs e)
        {
            showAlbumArtMenuItem.Checked = true;
            showLyricsMenuItem.Checked = false;
            pictureBox1.Enabled = true;
            pictureBox1.Visible = true;
            lyricsTextBox.Enabled = false;
            lyricsTextBox.Visible = false;
        }

        private void showLyricsMenuItem_Click(object sender, EventArgs e)
        {
            showLyricsMenuItem.Checked = true;
            showAlbumArtMenuItem.Checked = false;
            pictureBox1.Enabled = false;
            pictureBox1.Visible = false;
            lyricsTextBox.Enabled = true;
            lyricsTextBox.Visible = true;
        }

        private async void PlayPause_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                PlayBackFunction.PlayPauseMusic();
            });
            HandlePlaybackUI();
        }

        private async void skip_Click(object sender, EventArgs e)
        {
            if (Status == States.Playing || Status == States.Paused ||
                Status == States.Stopped || Status == States.Finished)
            {
                await Task.Run(() =>
                {
                    PlayBackFunction.SkipMusic();
                });
                HandlePlaybackUI();
            }
        }

        private async void previous_Click(object sender, EventArgs e)
        {
            if (Status == States.Playing || Status == States.Paused ||
                Status == States.Stopped || Status == States.Finished)
            {
                await Task.Run(() =>
                {
                    PlayBackFunction.PreviousMusic();
                });
                HandlePlaybackUI();
            }
        }

        private void musicListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            string[][]? allInfo = PlayList.GetCurrentPlaylist(CurrentPlaylistName);

            if (musicListView.SelectedItems.Count > 0)
            {
                if (musicListView.SelectedItems.Contains(e.Item))
                {
                    selectedItems.Add(allInfo[e.ItemIndex]);
                    selectedItemsIndices.Add(e.ItemIndex);
                    currentlySelectedSongIndex = e.ItemIndex;
                }
                else
                {
                    selectedItems.Clear();
                    selectedItemsIndices.Clear();
                    currentlySelectedSong = Array.Empty<string>();

                    for (int i = 0; i < musicListView.SelectedItems.Count; i++)
                    {
                        selectedItems.Add(allInfo[musicListView.SelectedItems[i].Index]);
                        selectedItemsIndices.Add(e.ItemIndex);
                    }
                }
                currentlySelectedSong = selectedItems[0];
                noSongSelected = false;
            }

            if (musicListView.SelectedItems.Count == 0)
            {
                selectedItems.Clear();
                selectedItemsIndices.Clear();
                currentlySelectedSong = Array.Empty<string>();
                noSongSelected = true;
            }
        }

        private async void addMusicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemoryManageTimer.Stop();
            System.Windows.Forms.OpenFileDialog musicBrowser = new System.Windows.Forms.OpenFileDialog();
            musicBrowser.Filter = "Audio Files | *.mp3; *.wav; *.flac; *.aiff; *.wma; *.pcm; *.aac; *.oog; *.alac";
            musicBrowser.Multiselect = true;
            int listviewItemsCount = musicListView.Items.Count;

            if (musicBrowser.ShowDialog() == DialogResult.OK)
            {
                string[] tempAddedFilesNames = musicBrowser.FileNames;
                int index = tempAddedFilesNames.Length;
                string[][]? AddedMusicInfo;
                Image[]? AddedMusicArts = new Image[1];
                await Task.Run(() =>
                {
                    string[] addedFiles = new string[index];
                    addedFiles = tempAddedFilesNames;

                    if (listviewItemsCount == 0)
                    {
                        (AddedMusicInfo, AddedMusicArts) = AudioDataBase.AddSongsToAudioDataBase(addedFiles, false, SharedFieldsAndVariables.SortOrder);
                        allMusicInfo = (string[][]?)AddedMusicInfo.Clone();
                    }
                    else
                    {
                        (AddedMusicInfo, AddedMusicArts) = AudioDataBase.AddSongsToAudioDataBase(addedFiles, true, SharedFieldsAndVariables.SortOrder);
                        allMusicInfo = (string[][]?)AddedMusicInfo.Clone();
                    }
                    //for disposal
                    musicBrowser.Dispose();
                });
                PopulateListView(ref allMusicInfo, ref AddedMusicArts, false);
                searchMusicListView.AutoCompleteCustomSource.Clear();
                SearchBarAutoCompleteSource(allMusicInfo);
                listUpdated = true;
            }
            MemoryManageTimer.Start();
        }

        private async void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image[]? AddedMusicArts = new Image[1];
            MemoryManageTimer.Stop();
            int listviewItemsCount = musicListView.Items.Count;
            OpenFolderDialog musicBrowser = new OpenFolderDialog();
            musicBrowser.Multiselect = true;

            if (musicBrowser.ShowDialog() == true)
            {
                string[] tempAddedFolderNames = musicBrowser.FolderNames;
                List<string> tempPathsList = new List<string>();
                await Task.Run(() =>
                {
                    if (tempAddedFolderNames.Length > 0)
                    {
                        foreach (var folder in tempAddedFolderNames)
                        {
                            string[]? tempDirectoryFiles = System.IO.Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
                            if (tempDirectoryFiles.Length > 0)
                            {
                                string? tempPath;
                                foreach (var file in tempDirectoryFiles)
                                {
                                    tempPath = System.IO.Path.GetExtension(file).Trim().ToLower();

                                    if (tempPath == ".mp3" || tempPath == ".wav" || tempPath == ".flac" || tempPath == ".aiff" ||
                                        tempPath == ".wma" || tempPath == ".pcm" || tempPath == ".aac" || tempPath == ".oog" ||
                                        tempPath == ".alac")

                                    { tempPathsList.Add(file); }
                                }
                            }
                        }
                    }

                    string[] addedFiles = tempPathsList.ToArray();
                    string[][]? AddedMusicInfo;
                    if (listviewItemsCount == 0)
                    {
                        (AddedMusicInfo, AddedMusicArts) = AudioDataBase.AddSongsToAudioDataBase(addedFiles, false ,SharedFieldsAndVariables.SortOrder);
                        allMusicInfo = (string[][]?)AddedMusicInfo.Clone();
                    }
                    else
                    {
                        (AddedMusicInfo, AddedMusicArts) = AudioDataBase.AddSongsToAudioDataBase(addedFiles, true, SharedFieldsAndVariables.SortOrder);
                        allMusicInfo = (string[][]?)AddedMusicInfo.Clone();
                    }

                    //for disposal
                    tempPathsList.Clear();

                });

                PopulateListView(ref allMusicInfo, ref AddedMusicArts, false);
                searchMusicListView.AutoCompleteCustomSource.Clear();
                SearchBarAutoCompleteSource(allMusicInfo);
                listUpdated = true;

                //to dispose
                musicBrowser = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                MemoryManageTimer.Start();
            }
        }

        private void rightClickMenu_Opening(object sender, CancelEventArgs e)
        {
            if (musicListView.SelectedItems.Count > 0)
            {

            }
        }

        private void showItemInfoButton_Click(object sender, EventArgs e)
        {
            using (AudioInfo selectedAudioFileInfo = new AudioInfo(currentlySelectedSong[3]))
            {
                Khi_Player_Audio_Info_Form audioInfoPage = new Khi_Player_Audio_Info_Form(selectedAudioFileInfo.title,
                    selectedAudioFileInfo.artist, selectedAudioFileInfo.album, selectedAudioFileInfo.trackNumber,
                    selectedAudioFileInfo.genre, selectedAudioFileInfo.duration, selectedAudioFileInfo.bitrate,
                    selectedAudioFileInfo.sampleRate, selectedAudioFileInfo.encoding, selectedAudioFileInfo.channel,
                    selectedAudioFileInfo.path, selectedAudioFileInfo.format, selectedAudioFileInfo.lyrics,
                    selectedAudioFileInfo.art, isDarkMode);

                audioInfoPage.ShowDialog();
            }
        }

        private void musicListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var clickedItem = musicListView.GetItemAt(e.X, e.Y);
                if (musicListView.SelectedItems.Contains(clickedItem))
                {
                    var playlist = PlayList.GetCurrentPlaylist();
                    currentlySelectedSong = (string[]?)playlist[clickedItem.Index].Clone();
                    currentlySelectedSongIndex = clickedItem.Index;
                    showItemInfoButton.Enabled = true;
                    editItemTagsButton.Enabled = true;
                    removeItemButton.Enabled = true;
                }
            }
            else
            {
                var clickedItem = musicListView.GetItemAt(e.X, e.Y);
                if (rightClickMenu.IsDisposed && !rightClickMenu.Enabled)
                {
                    if (clickedItem == null)
                    {
                        if (mediaPlayer.PlaybackState != PlaybackState.Playing || mediaPlayer.PlaybackState != PlaybackState.Paused)
                        {
                            musicListView.SelectedItems.Clear();
                            selectedItems.Clear();
                            selectedItemsIndices.Clear();
                        }
                        currentlySelectedSong = Array.Empty<string>();
                        GC.Collect();
                    }
                }
            }
        }

        private void rightClickMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            showItemInfoButton.Enabled = false;
            editItemTagsButton.Enabled = false;
            removeItemButton.Enabled = false;
        }

        private async void removeItemButton_Click(object sender, EventArgs e)
        {           
            if (musicListView.SelectedItems.Count > 0)
            {
                bool wasPlaying = false;
                bool allItemsSelected = false;
                List<int> toBeRemovedItemsIndices = new List<int>();
                List<object> listviewItemsToRemove = new List<object>(); //since the item indices changes after every removal, better to use the items themselves for this task

                if (musicListView.SelectedItems.Count == musicListView.Items.Count) { allItemsSelected = true; }
                foreach (ListViewItem removingItem in musicListView.SelectedItems)
                {
                    listviewItemsToRemove.Add(removingItem);
                    int ToRemoveIndex = removingItem.Index;
                    toBeRemovedItemsIndices.Add(ToRemoveIndex);
                }
                //if one of the songs that are to be removed is currently playing, will stop playback, and dispose the picture so that no error are encountared
                if (currentlyPlayingSongInfo != null && toBeRemovedItemsIndices.Contains(currentlyPlayingSongIndex)
                    && (Status == States.Playing || Status == States.Paused ||
                    Status == States.Stopped || Status == States.Finished))
                {
                    wasPlaying = true;
                    songSeekTimer.Stop();
                    mediaPlayer.Stop();
                    song.Dispose();
                    if (pictureBox1.Image != null) { pictureBox1.Image.Dispose(); }
                }
                else { wasPlaying = false; }
                musicListView.BeginUpdate();
                //removing the items from the listview itself
                if (allItemsSelected == true) { musicListView.Items.Clear(); }
                else
                {
                    foreach (var item in listviewItemsToRemove)
                    {
                        musicListView.Items.Remove((ListViewItem)item);
                    }
                }
                musicListView.EndUpdate();
                //to prevent errors while deleting files
                GC.Collect();
                GC.WaitForPendingFinalizers();
                AudioDataBase.RemoveSongs(toBeRemovedItemsIndices, wasPlaying, allItemsSelected);
                await Task.Delay(1000);
                listUpdated = true;
                isShuffled = false;
                
                if (wasPlaying == true && allItemsSelected == false && musicListView.Items.Count > 0)
                {                
                    PlayPause.PerformClick();
                }
                else if (allItemsSelected == true)
                {
                    if (pictureBox1.Image != null) { pictureBox1.Image = null; }
                    searchMusicListView.AutoCompleteCustomSource.Clear();
                    Status = States.Setup;
                    songSeekTimer.Enabled = false;
                    seekBar.Value = 0;
                    seekBar.Enabled = false;
                    currentTimeLabel.Text = "00:00";
                    songLengthLabel.Text = "00:00";
                    songTitleLabel.Text = "";
                    songArtistLabel.Text = "";
                    songAlbumLabel.Text = "";
                    borderLabel.Visible = false;
                }
                //for disposal
                listviewItemsToRemove.Clear();
            }            
        }

        private void musicListView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                musicListView.SelectedItems.Clear();
                selectedItems.Clear();
                selectedItemsIndices.Clear();
                currentlySelectedSong = Array.Empty<string>();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GC.Collect();
        }

        private void showLargeIconMenuItem_Click(object sender, EventArgs e)
        {
            showLargeIconMenuItem.Checked = true;
            showDetailsMenuItem.Checked = false;
            showTileViewMenuItem.Checked = false;
            musicListView.View = View.LargeIcon;
            musicListView.FullRowSelect = true;
            //musicListView.Refresh();
        }

        private void showTileViewMenuItem_Click(object sender, EventArgs e)
        {
            showDetailsMenuItem.Checked = false;
            showLargeIconMenuItem.Checked = false;
            showTileViewMenuItem.Checked = true;
            musicListView.View = View.Tile;
            musicListView.FullRowSelect = false;
        }

        private void showDetailsMenuItem_Click(object sender, EventArgs e)
        {
            showDetailsMenuItem.Checked = true;
            showLargeIconMenuItem.Checked = false;
            showTileViewMenuItem.Checked = false;
            musicListView.View = View.Details;
            musicListView.FullRowSelect = true;
        }

        private void musicListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            /*
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle();
            rect.Size = new System.Drawing.Size(musicListView.TileSize.Width, musicListView.TileSize.Height);
            rect.Location = new System.Drawing.Point(e.Bounds.X, e.Bounds.Y);
            Pen pen = new Pen(Color.Empty, 0 / 7);
            Brush brush = new SolidBrush(Color.Empty);
            e.Graphics.DrawRectangle(pen, rect);
            e.Graphics.FillRectangle(brush, rect);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default;
            StringFormat stringformat = new StringFormat();
            stringformat = StringFormat.GenericDefault;
            stringformat.FormatFlags = StringFormatFlags.LineLimit;
            e.Graphics.DrawString(e.Item.Text, e.Item.Font, new SolidBrush(Color.Black), rect, stringformat);
            */
            //e.Graphics.Save();
            // if (e.State == ListViewItemStates.Default)
            // {
            //e.DrawText(TextFormatFlags.TextBoxControl);
            // }

            //using microsoft example 
            /*
            if ((e.State & ListViewItemStates.Selected) != 0)
            {
                // Draw the background and focus rectangle for a selected item.
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                e.DrawFocusRectangle();
            }
            else
            {
                // Draw the background for an unselected item.
                using (LinearGradientBrush brush =
                    new LinearGradientBrush(e.Bounds, Color.Empty,
                    Color.Empty, LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }
            }
            */
            // Draw the item text for views other than the Details view.
            e.DrawDefault = true;

            /*
            if (musicListView.View != View.Details)
            {
                musicListView.SuspendLayout();
                e.Graphics.DrawImage(musicListView.LargeImageList.Images[e.ItemIndex], e.Bounds.X, e.Bounds.Y, 60, 60);
                
                //e.DrawText(TextFormatFlags.TextBoxControl);
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default;
                StringFormat stringformat = new StringFormat();
                stringformat = StringFormat.GenericDefault;
                stringformat.FormatFlags = StringFormatFlags.LineLimit;
                e.Graphics.DrawString(e.Item.Text, e.Item.Font, new SolidBrush(Color.Black), e.Bounds.X + 61, e.Bounds.Y , stringformat);
                e.Graphics.Save();
            }
            //e.State == ListViewItemStates.Hot
            //musicListView.EndUpdate();
            musicListView.ResumeLayout();*/
        }

        private void musicListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
            /*
            
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle();
            rect.Size = new System.Drawing.Size (e.Item.Bounds.Width, 50);
            Pen pen = new Pen(Color.Empty, 0/7);
            e.Graphics.DrawRectangle(pen, rect);
            pen.Color = Color.LightBlue;

            //e.DrawFocusRectangle(rect) ;
            e.DrawText(TextFormatFlags.TextBoxControl);
            //e.State = ListViewItemStates.Hot;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            e.Graphics.Save();
            e.DrawText(TextFormatFlags.TextBoxControl);
            */
            //e.DrawDefault = true;
        }

        void musicListView_Invalidated(object sender, InvalidateEventArgs e)
        {/*
            foreach (ListViewItem item in musicListView.Items)
            {
                if (item == null) return;
                item.Tag = null;
            }*/
        }

        private void songToolTip_Popup(object sender, PopupEventArgs e)
        {
            //songToolTip.SetToolTip(musicListView)
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (pictureBox1.Visible == true && pictureBox1.Enabled == true)
            {
                pictureBox1.Enabled = false;
                pictureBox1.Visible = false;
                lyricsTextBox.Visible = true;
                lyricsTextBox.Enabled = true;
                showAlbumArtMenuItem.Checked = false;
                showLyricsMenuItem.Checked = true;
            }

        }

        private void lyricsTextBox_DoubleClick(object sender, EventArgs e)
        {
            if (lyricsTextBox.Enabled == true && lyricsTextBox.Visible == true)
            {
                pictureBox1.Enabled = true;
                pictureBox1.Visible = true;
                lyricsTextBox.Visible = false;
                lyricsTextBox.Enabled = false;
                showAlbumArtMenuItem.Checked = true;
                showLyricsMenuItem.Checked = false;
            }
        }

        public void seekBar_Scroll(object sender, EventArgs e)
        {
            seekBarFinalValue = seekBar.Value;
        }

        private void songSeekTimer_Tick(object sender, EventArgs e)
        {
            if (Status == States.Playing || Status == States.Paused)
            {
                if (seekBar.Value == seekBar.Maximum)
                { Status = States.Finished; }
                PlayBackFunction.SongTimeValue();
                try
                {
                    currentTimeLabel.Text = currenSongTimePosition;
                    seekBar.Value = timeValue;
                    lastSongTimePosition = timeValue;
                }
                catch { }
            }
            else if (Status == States.Finished)
            {
                skip.PerformClick();
            }
            else
            {
                currenSongTimePosition = new TimeSpan(0, 00, 00).ToString("mm\\:ss");
                seekBar.Value = 0;
                currentTimeLabel.Text = "00:00";
                songLengthLabel.Text = "00:00";
            }
        }

        private void seekBar_MouseDown(object sender, MouseEventArgs e)
        {
            seekBarValueBeforeMove = seekBar.Value;
        }

        private void seekBar_MouseUp(object sender, MouseEventArgs e)
        {
            int clickedValue = (int)(Math.Round((((double)e.X) / (seekBar.ClientSize.Width - 4 )) * (seekBar.Maximum - seekBar.Minimum), MidpointRounding.ToZero));
            if (clickedValue <= seekBar.Maximum)
            {
                seekBar.Value = clickedValue;
                song.Skip(clickedValue - timeValue -1);
            }
        }

        private void volumeBar_Scroll(object sender, EventArgs e)
        {
            mediaPlayer.Volume = ((float)volumeBar.Value) / 100;
            volumeLabel.Text = volumeBar.Value.ToString();
        }

        private void MemoryManageTimer_Tick(object sender, EventArgs e)
        {
            GC.Collect();
        }
        
        private void darkModeMenuItem_Click(object sender, EventArgs e)
        {
            //turns lightmode to dark mode
            if (!darkModeMenuItem.Checked)
            {
                DarkMode(true);
                isDarkMode = true;
            }
            //turns dark mode to light mode
            else
            {
                DarkMode(false);
                isDarkMode = false;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            /*
            var formsize = this.Size;
            if (this.Size.Width > this.MinimumSize.Width && this.Size.Height > this.MinimumSize.Height)
            {
                int size = 14;
                lyricsTextBox.Font = new Font("Segoe UI", size);
                songTitleLabel.Font = new Font("Segoe UI", size);
                songArtistLabel.Font = new Font("Segoe UI", size);
                songAlbumLabel.Font = new Font("Segoe UI", size);
                borderLabel.Font = new Font("Segoe UI", size);
            }
            else
            {
                int size = 9;
                lyricsTextBox.Font = new Font("Segoe UI", size);
                songTitleLabel.Font = new Font("Segoe UI", size);
                songArtistLabel.Font = new Font("Segoe UI", size);
                songAlbumLabel.Font = new Font("Segoe UI", size);
                new Font("Segoe UI", size);
            }*/
        }


        private async void allSongsPlaylist_Click(object sender, EventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(allMusicDataBase))
                {
                    CurrentPlaylist = Playlists.allSongs;
                    currentPlaylistLabel.Text = "All Songs";
                    allMusicInfo = ReadAudioDataBase("complete");

                    if (SharedFieldsAndVariables.SortOrder != SortOrders.CustomSort)
                    {
                        int order = (int)SharedFieldsAndVariables.SortOrder;
                        int sortColumn;

                        switch (order)
                        {
                            case 0:
                                //this is sort based on title
                                sortColumn = 0;
                                break;
                            case 1:
                                //this is sort based on artist
                                sortColumn = 1;
                                break;
                            case 2:
                                //this is sort based on album
                                sortColumn = 2;
                                break;

                            default:
                                //this is sort based on title
                                sortColumn = 0;
                                break;
                        }
                        PlayList.SortPlaylist(CurrentPlaylist, sortColumn);
                    }

                    Image[]? arts = GetMusicThumbnails(allMusicInfo);
                    musicListView.Items.Clear();
                    musicListView.LargeImageList.Images.Clear();
                    PopulateListView(ref allMusicInfo, ref arts);
                    searchMusicListView.AutoCompleteCustomSource.Clear();
                    SearchBarAutoCompleteSource(allMusicInfo);
                    listUpdated = true;
                    isShuffled = false;

                    GC.Collect();
                }
            }
            catch { }
        }


        private void searchMusicListView_KeyDown(object sender, KeyEventArgs e)
        {
            string? searchWord = searchMusicListView.Text;

            if (e.KeyValue == (int)Keys.Enter || e.KeyValue == (int)Keys.Tab)
            {
                var previousPlaylist = CurrentPlaylist;
                List<int> originalIndices = new List<int>();

                if (searchWord != "")
                {
                    List<string[]?> playlist = PlayList.GetCurrentPlaylist().ToList();
                    List<int> foundItemsIndices = new List<int>();
                    //ListViewItem[]? foundItems = musicListView.Items.Find(searchWord, true);
                    int i = 0;
                    foreach (string[]? item in playlist)
                    {
                        foreach (string? subitem in item)
                        {
                            if (subitem == searchWord || subitem.ToLower() == searchWord.ToLower() ||
                                subitem.Contains(searchWord) || subitem.ToLower().Contains(searchWord.ToLower()))
                            {
                                if (!foundItemsIndices.Contains(i))
                                {
                                    foundItemsIndices.Add(i);
                                }
                            }
                        }
                        i++;
                    }

                    if (foundItemsIndices.Count >= 1)
                    {
                        List<string[]?> foundItems = new List<string[]>();
                        List<Image?> foundItemsThumbnails = new List<Image?>();

                        foreach (var itemIndex in foundItemsIndices)
                        {
                            foundItems.Add(playlist[itemIndex]);
                            foundItemsThumbnails.Add((Image?)musicListView.LargeImageList.Images[itemIndex].Clone());
                        }
                        searchPlaylist = foundItems.ToArray();
                        musicListView.Items.Clear();

                        string[][]? foundItemsArray = foundItems.ToArray();
                        Image[]? foundItemsThumbnailsArray = foundItemsThumbnails.ToArray();
                        PopulateListView(ref foundItemsArray, ref foundItemsThumbnailsArray);

                        //for disposal
                        foundItemsThumbnails.Clear();
                    }
                    listUpdated = true;
                    CurrentPlaylist = Playlists.searchPlaylist;

                    //for disposal
                    GC.Collect();
                }
                else
                {
                    //string[][]? playlist = PlayList.GetPlaylist(previousPlaylist);
                    allMusicInfo = ReadAudioDataBase("complete");

                    Image[]? arts = GetMusicThumbnails(allMusicInfo);
                    musicListView.Items.Clear();
                    musicListView.LargeImageList.Images.Clear();
                    PopulateListView(ref allMusicInfo, ref arts);
                    musicListView.Focus();
                    searchMusicListView.AutoCompleteCustomSource.Clear();
                    SearchBarAutoCompleteSource(allMusicInfo);

                    listUpdated = true;
                    CurrentPlaylist = Playlists.allSongs;
                }
            }
            else if (e.KeyValue == (int)Keys.Escape)
            {
                allMusicInfo = ReadAudioDataBase("complete");
                Image[]? arts = GetMusicThumbnails(allMusicInfo);
                musicListView.Items.Clear();
                musicListView.LargeImageList.Images.Clear();
                PopulateListView(ref allMusicInfo, ref arts);
                musicListView.Focus();
                searchMusicListView.AutoCompleteCustomSource.Clear();
                SearchBarAutoCompleteSource(allMusicInfo);

                listUpdated = true;
                CurrentPlaylist = Playlists.allSongs;
            }

        }


        private void sortListArtistMenuItem_Click(object sender, EventArgs e)
        {
            var playlist = PlayList.GetCurrentPlaylist();

            if (playlist != null && playlist.Length > 0 && playlist[0] != null)
            {
                playlist = PlayList.SortPlaylist(CurrentPlaylist, 1);

                //for now faster and cleaner to simply clear and repopulate instead of changing the indices
                musicListView.Items.Clear();
                musicListView.LargeImageList.Images.Clear();
                Image[]? tempAllArts = (Image[]?)AudioDataBase.GetMusicThumbnails(PlayList.GetCurrentPlaylist()).Clone();
                PopulateListView(ref playlist, ref tempAllArts);
                listUpdated = true;
                if (CurrentPlaylist != Playlists.allSongs && CurrentPlaylist != Playlists.searchPlaylist) { playlist = null; }
                SharedFieldsAndVariables.SortOrder = SortOrders.ArtistSort;
            }          
        }

        private void sortListTitleMenuItem_Click(object sender, EventArgs e)
        {
            var playlist = PlayList.GetCurrentPlaylist();

            if (playlist != null && playlist.Length >0 &&playlist[0] != null)
            {
                playlist = PlayList.SortPlaylist(CurrentPlaylist, 0);
                musicListView.Items.Clear();
                musicListView.LargeImageList.Images.Clear();
                Image[]? tempAllArts = (Image[]?)AudioDataBase.GetMusicThumbnails(PlayList.GetCurrentPlaylist()).Clone();
                PopulateListView(ref playlist, ref tempAllArts);
                listUpdated = true;
                if (CurrentPlaylist != Playlists.allSongs && CurrentPlaylist != Playlists.searchPlaylist) { playlist = null; }
                SharedFieldsAndVariables.SortOrder = SortOrders.TitleSort;
            }           
        }

        private void sortListAlbumMenuItem_Click(object sender, EventArgs e)
        {
            var playlist = PlayList.GetCurrentPlaylist();

            if (playlist != null && playlist.Length > 0 && playlist[0] != null)
            {
                playlist = PlayList.SortPlaylist(CurrentPlaylist, 2);
                musicListView.Items.Clear();
                musicListView.LargeImageList.Images.Clear();
                Image[]? tempAllArts = (Image[]?)AudioDataBase.GetMusicThumbnails(PlayList.GetCurrentPlaylist()).Clone();
                PopulateListView(ref playlist, ref tempAllArts);
                listUpdated = true;
                if (CurrentPlaylist != Playlists.allSongs && CurrentPlaylist != Playlists.searchPlaylist) { playlist = null; }
                SharedFieldsAndVariables.SortOrder = SortOrders.AlbumSort;
            }    
        }

        private async void toggleShuffle_Click(object sender, EventArgs e)
        {
            if (isShuffleEnabled == true)
            {
                isShuffleEnabled = false;
                toggleShuffle.ForeColor = Color.Blue;
                toggleShuffle.FlatAppearance.BorderColor = Color.Blue;
                toggleShuffle.FlatAppearance.BorderSize = 0;
            }
            else
            {
                isShuffleEnabled = true;
                toggleShuffle.ForeColor = Color.Blue;
                toggleShuffle.FlatAppearance.BorderColor = Color.Blue;
                toggleShuffle.FlatAppearance.BorderSize = 1;
                await Task.Run(() =>
                {
                    PlayBackFunction.ShufflePlaylist();
                });
            }
        }

        private async void stopButton_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                PlayBackFunction.StopMusic();
            });
            seekBar.Value = 0;
            songSeekTimer.Enabled = false;
            seekBar.Enabled = false;
            currentTimeLabel.Text = "00:00";
            songLengthLabel.Text = "00:00";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isDarkMode)
            {
                Settings1.Default.DarkMode = false;
            }
            if (isShuffleEnabled)
            {
                Settings1.Default.Shuffle = true;
            }
            if (isLoopEnabled)
            {
                Settings1.Default.Loop = true;
                Settings1.Default.LoopMode = (int)LoopState;
            }
            Settings1.Default.SortOrder = (int)SharedFieldsAndVariables.SortOrder;
            Settings1.Default.Save();
        }

        private void editPlaylistButton_Click(object sender, EventArgs e)
        {

        }

        private async void songTitleLabel_Click(object sender, EventArgs e)
        {
            var tempPlaylist = PlayList.GetCurrentPlaylist();
            var playlist = tempPlaylist.ToList();
            int index = 0;

            await Task.Run(() =>
            {
                bool notFoundInCurrentPlaylist = false;
                int i = 0;
                foreach (var song in playlist)
                {
                    if (song[3] == currentlyPlayingSongInfo[3])
                    {
                        index = i;
                        notFoundInCurrentPlaylist = false;
                        break;
                    }
                    i++;
                }

                if (notFoundInCurrentPlaylist == true)
                {

                }
            });
            musicListView.EnsureVisible(index);
        }

        private void editLyricsToolStripButton_Click(object sender, EventArgs e)
        {
            lyricsTextBox.ReadOnly = false;
            applyEditStripButton.Enabled = true;
            applyEditStripButton.Visible = true;
        }

        private void applyEditStripButton_Click(object sender, EventArgs e)
        {
            string? lyrics = lyricsTextBox.Text;
            long songPositionBackup = song.Position;
            string[]? playingSongBackup = currentlyPlayingSongInfo;
            PlayBackFunction.StopMusic();
            mediaPlayer.Dispose();
            song.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            try
            {
                using (TagLib.File lyrictag = TagLib.File.Create(currentlyPlayingSongInfo[3]))
                {
                    lyrictag.Tag.Lyrics = lyrics;
                    lyrictag.Save();
                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Could Not Edit File");
                //throw;
            }
            applyEditStripButton.Enabled = false;
            applyEditStripButton.Visible = false;
            lyricsTextBox.ReadOnly = true;

            currentlySelectedSong = currentlyPlayingSongInfo;
            PlayPause.PerformClick();
            song.Position = songPositionBackup;
        }
    }
}
