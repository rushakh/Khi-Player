using Microsoft.VisualBasic;
using Microsoft.Win32;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Buffers.Text;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.Unicode;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static Khi_Player.Form1;
using static Khi_Player.Form1.AudioDataBase;
using Application = System.Windows.Forms.Application;
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


            KhiEditor = new FormEditor(this);


            if (File.Exists(allMusicDataBase))
            {
                AudioDataBase khiDatabase = new AudioDataBase();
                if (AudioDataBase.errorDetected == false)
                {

                    allMusicInfo = (string[][]?)khiDatabase.AllMusicInfo.Clone();
                    allMusicArts = (Image[]?)khiDatabase.AllMusicArts.Clone();
                    khiDatabase.Dispose();

                    FormEditor.PopulateListView(ref musicListView, ref allMusicInfo, ref allMusicArts, false);

                    FormEditor.SearchBarAutoCompleteSource(allMusicInfo);

                    KhiPlayer = new PlayBackFunction();

                    allMusicArts = null;

                }
                else
                {
                    khiDatabase.Dispose();

                    string[][]? dataBaseInfo;
                    Image[]? Arts;

                    (dataBaseInfo, Arts) = FilterDuplicates.TryRepairingDataBase(musicListView);
                    allMusicInfo = (string[][])dataBaseInfo.Clone();
                    Arts = (Image[]?)Arts.Clone();

                    FormEditor.PopulateListView(ref musicListView, ref allMusicInfo, ref Arts, false);

                    FormEditor.SearchBarAutoCompleteSource(allMusicInfo);

                    KhiPlayer = new PlayBackFunction();

                    allMusicArts = null;
                    dataBaseInfo = null;
                    Arts = null;
                    //artPaths = null;


                }
            }
            FormEditor.DynamicPlaylistsButtonsIni();

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

        public enum Playlists { allSongs, searchPlaylist, DynamicPlaylists };
        public static Playlists CurrentPlaylist = Playlists.allSongs;

        public enum LoopStates { NoLoop, SingleSongLoop, PlaylistLoop };
        public static LoopStates LoopState = LoopStates.NoLoop;

        public enum SortOrders { CustomSort, TitleSort, ArtistSort, AlbumSort };
        public static SortOrders SortOrder = SortOrders.CustomSort;

        public static Dictionary<string, string[][]?> PlaylistsDict = new Dictionary<string, string[][]?>();
        public static string? CurrentPlaylistName;

        static string[][]? allMusicInfo = new string[1][]; //just for initilization
        static Image[]? allMusicArts = new Image[1]; //same thing
        
        public static string[][]? searchPlaylist = new string[1][];
        public static string[][]? PlaylistMusicInfo = new string[1][];

        static uint selectedMusicsQue = 0; //only accepts >= 0 value, not negative integers

        static string applicationPath = Application.StartupPath;
        static string allMusicDataBase = Application.StartupPath + "AllMusicDataBase.xml";
        static string albumArtsPath = Application.StartupPath + "\\Album Arts\\";
        static string albumArtsThumbnailsPath = Application.StartupPath + "Album Arts Thumbnails\\";

        static List<string[]> selectedItems = new List<string[]>();
        static List<int>? selectedItemsIndices = new List<int>();

        static string[] currentlySelectedSong = new string[6];
        static int currentlySelectedSongIndex = 0;
        static string[] currentlyPlayingSongInfo = new string[6];
        static Image? currentlyPlayingSongPic;

        public static int seekBarFinalValue;
        public static int seekBarValueBeforeMove;
        public static string? currenSongTimePosition;
        public static int lastSongTimePosition = 0;

        static bool noSongSelected = false;
        static bool listUpdated = false;
        static bool isDarkMode = false;
        static bool isShuffled = false;
        static bool isShuffleEnabled = false;
        static bool isLoopEnabled = false;

        PlayBackFunction KhiPlayer;
        public static FormEditor KhiEditor;


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
            //newPlaylistButton.Tag = ((int)Playlists.DynamicPlaylists);

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
        /// this class is just here so the codes that are repeated and change the controls or UI
        /// can be put into one place in order to have cleaner code and make maintainance easier (written in a functional sort of way)
        /// </summary>
        public class FormEditor
        {

            public static Form1 CurrentForm;

            /// <summary>
            /// Need to pass the current form to the constructor once so that methods can function
            /// (since they directly edit the controls instead of providing the data)
            /// </summary>
            /// <param name="passFormToConstructor"></param>
            public FormEditor(Form1 passFormToConstructor)
            {
                CurrentForm = passFormToConstructor;
            }

            /// <summary>
            /// to enable or disable Darkmode. set <see langword="true"/> to activate DarkMode, and <see langword="false"/>to disable it.
            /// </summary>
            /// <param name="EnableDisable"></param>
            public static void DarkMode(bool EnableDisable)
            {
                if (EnableDisable)
                {
                    CurrentForm.BackColor = Color.FromArgb(41, 41, 41);
                    CurrentForm.ForeColor = Color.White;

                    foreach (Control cont in CurrentForm.Controls)
                    {
                        cont.BackColor = Color.FromArgb(41, 41, 41);
                        cont.ForeColor = Color.White;

                    }
                    foreach (Control cont in CurrentForm.musicControlBar.Controls)
                    {
                        cont.BackColor = Color.FromArgb(41, 41, 41);
                        cont.ForeColor = Color.White;
                    }
                    foreach (Control cont in CurrentForm.mediaPlayerPanel.Controls)
                    {
                        cont.BackColor = Color.FromArgb(41, 41, 41);
                        cont.ForeColor = Color.White;
                    }
                    foreach (ToolStripItem cont in CurrentForm.playlistToolbar.Items)
                    {
                        cont.BackColor = Color.FromArgb(41, 41, 41);
                        cont.ForeColor = Color.White;
                    }
                    foreach (ToolStripItem cont in CurrentForm.userBar.Items)
                    {
                        cont.BackColor = Color.FromArgb(41, 41, 41);
                        cont.ForeColor = Color.White;

                    }
                    foreach (ToolStripItem cont in CurrentForm.fileToolStripMenuItem.DropDown.Items)
                    {
                        cont.BackColor = Color.FromArgb(41, 41, 41);
                        cont.ForeColor = Color.White;
                    }
                    foreach (ToolStripItem cont in CurrentForm.viewToolStripMenuItem.DropDown.Items)
                    {
                        cont.BackColor = Color.FromArgb(41, 41, 41);
                        cont.ForeColor = Color.White;
                    }
                    foreach (ToolStripItem cont in CurrentForm.listToolStripMenuItem.DropDown.Items)
                    {
                        cont.BackColor = Color.FromArgb(41, 41, 41);
                        cont.ForeColor = Color.White;
                    }
                    foreach (ToolStripItem cont in CurrentForm.rightClickMenu.Items)
                    {
                        cont.BackColor = Color.FromArgb(41, 41, 41);
                        cont.ForeColor = Color.White;
                    }
                    foreach (ToolStripItem cont in CurrentForm.addToPlaylistButton.DropDown.Items)
                    {
                        cont.BackColor = Color.FromArgb(41, 41, 41);
                        cont.ForeColor = Color.White;
                    }

                    CurrentForm.addToPlaylistButton.DropDown.BackColor = Color.FromArgb(41, 41, 41);
                    CurrentForm.addToPlaylistButton.DropDown.ForeColor = Color.White;
                    CurrentForm.viewToolStripMenuItem.DropDown.BackColor = Color.FromArgb(41, 41, 41);
                    CurrentForm.viewToolStripMenuItem.DropDown.ForeColor = Color.White;
                    CurrentForm.SortListMenuItem.DropDown.BackColor = Color.FromArgb(41, 41, 41);
                    CurrentForm.SortListMenuItem.DropDown.ForeColor = Color.White;
                    CurrentForm.fileToolStripMenuItem.DropDown.BackColor = Color.FromArgb(41, 41, 41);
                    CurrentForm.fileToolStripMenuItem.DropDown.ForeColor = Color.White;
                    CurrentForm.listToolStripMenuItem.DropDown.BackColor = Color.FromArgb(41, 41, 41);
                    CurrentForm.listToolStripMenuItem.DropDown.ForeColor = Color.White;

                    //for Icons

                    CurrentForm.toggleLoop.BackgroundImage = Properties.Resources.Loop_Dark_Mode;
                    CurrentForm.PlayPause.BackgroundImage = Properties.Resources.Play_Pause__Dark_Mode;
                    CurrentForm.skip.BackgroundImage = Properties.Resources.Skip_Dark_Mode;
                    CurrentForm.previous.BackgroundImage = Properties.Resources.Previous_Dark_Mode;
                    CurrentForm.toggleShuffle.BackgroundImage = Properties.Resources.Shuffle_Dark_Mode;
                    CurrentForm.stopButton.BackgroundImage = Properties.Resources.Stop_Dark_Mode;

                    isDarkMode = true;
                }
                //turns dark mode to light mode
                else
                {
                    CurrentForm.BackColor = Color.White;
                    CurrentForm.ForeColor = Color.FromArgb(41, 41, 41);
                    foreach (Control cont in CurrentForm.Controls)
                    {
                        cont.BackColor = Color.White;
                        cont.ForeColor = Color.FromArgb(41, 41, 41);

                    }
                    foreach (Control cont in CurrentForm.musicControlBar.Controls)
                    {
                        cont.BackColor = Color.White;
                        cont.ForeColor = Color.FromArgb(41, 41, 41);
                    }
                    foreach (Control cont in CurrentForm.mediaPlayerPanel.Controls)
                    {
                        cont.BackColor = Color.White;
                        cont.ForeColor = Color.FromArgb(41, 41, 41);
                    }
                    foreach (ToolStripItem cont in CurrentForm.playlistToolbar.Items)
                    {
                        cont.BackColor = Color.White;
                        cont.ForeColor = Color.FromArgb(41, 41, 41);
                    }
                    foreach (ToolStripItem cont in CurrentForm.userBar.Items)
                    {
                        cont.BackColor = Color.White;
                        cont.ForeColor = Color.FromArgb(41, 41, 41);
                    }
                    foreach (ToolStripItem cont in CurrentForm.fileToolStripMenuItem.DropDown.Items)
                    {
                        cont.BackColor = Color.White;
                        cont.ForeColor = Color.FromArgb(41, 41, 41);
                    }
                    foreach (ToolStripItem cont in CurrentForm.viewToolStripMenuItem.DropDown.Items)
                    {
                        cont.BackColor = Color.White;
                        cont.ForeColor = Color.FromArgb(41, 41, 41);
                    }
                    foreach (ToolStripItem cont in CurrentForm.listToolStripMenuItem.DropDown.Items)
                    {
                        cont.BackColor = Color.White;
                        cont.ForeColor = Color.FromArgb(41, 41, 41);
                    }
                    foreach (ToolStripItem cont in CurrentForm.rightClickMenu.Items)
                    {
                        cont.BackColor = Color.White;
                        cont.ForeColor = Color.FromArgb(41, 41, 41);

                    }
                    foreach (ToolStripItem cont in CurrentForm.addToPlaylistButton.DropDownItems)
                    {
                        cont.BackColor = Color.White;
                        cont.ForeColor = Color.FromArgb(41, 41, 41);
                    }
                    CurrentForm.addToPlaylistButton.DropDown.BackColor = Color.White;
                    CurrentForm.addToPlaylistButton.DropDown.ForeColor = Color.FromArgb(41, 41, 41);
                    CurrentForm.viewToolStripMenuItem.DropDown.BackColor = Color.White;
                    CurrentForm.viewToolStripMenuItem.DropDown.ForeColor = Color.FromArgb(41, 41, 41);
                    CurrentForm.SortListMenuItem.DropDown.BackColor = Color.White;
                    CurrentForm.SortListMenuItem.DropDown.ForeColor = Color.FromArgb(41, 41, 41);
                    CurrentForm.fileToolStripMenuItem.DropDown.BackColor = Color.White;
                    CurrentForm.fileToolStripMenuItem.DropDown.ForeColor = Color.FromArgb(41, 41, 41);
                    CurrentForm.listToolStripMenuItem.DropDown.BackColor = Color.White;
                    CurrentForm.listToolStripMenuItem.DropDown.ForeColor = Color.FromArgb(41, 41, 41);

                    //For Icons

                    CurrentForm.toggleLoop.BackgroundImage = Properties.Resources.loop;
                    CurrentForm.PlayPause.BackgroundImage = Properties.Resources.Play_Pause;
                    CurrentForm.skip.BackgroundImage = Properties.Resources.Skip;
                    CurrentForm.previous.BackgroundImage = Properties.Resources.Previous;
                    CurrentForm.toggleShuffle.BackgroundImage = Properties.Resources.Shuffle_Light_Mode;
                    CurrentForm.stopButton.BackgroundImage = Properties.Resources.Stop_Light_Mode;

                    isDarkMode = false;
                }
            }


            /// <summary>
            /// to create the buttons for the dynamically created playlists
            /// </summary>
            public static void DynamicPlaylistsButtonsIni()
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
                        CurrentForm.CreateDynamicPlaylistButton(playlistName);
                        playlistDatabase = null;
                        
                    }
                }
            }

            /// <summary>
            /// to avoid constantly repeating codes needed for populating musicListView with audio info from the data base
            /// </summary>
            /// <param name="listview"></param>
            /// <param name="musicInfos"></param>
            /// <param name="musicThumbnails"></param>
            /// <param name="noImageMode"></param>
            public static void PopulateListView(ref ListView listview, ref string[][]? musicInfos, ref Image[]? musicThumbnails, bool noImageMode = false)
            {
                if (musicInfos != null && musicInfos.Length > 0)
                {
                    listview.BeginUpdate();

                    if (noImageMode)
                    {
                        foreach (string[]? music in musicInfos)
                        {
                            ListViewItem song = new ListViewItem(music);
                            song.Name = music[3];
                            song.ToolTipText = music[0] + System.Environment.NewLine + music[1] + System.Environment.NewLine + music[2];
                            listview.Items.Add(song);
                            song = null;
                        }
                    }
                    else
                    {
                        int i = 0;
                        if (listview.LargeImageList == null)
                        {
                            listview.LargeImageList = new ImageList
                            {
                                ImageSize = new System.Drawing.Size(60, 60)
                            };
                        }
                        if (listview.LargeImageList.Images.Count > 0)  // this is for the listview is gonna be appended and not cleared beforehand
                        {
                            i = listview.LargeImageList.Images.Count;
                        }

                        listview.LargeImageList.Images.AddRange(musicThumbnails);


                        foreach (string[]? music in musicInfos)
                        {

                            ListViewItem song = new ListViewItem(music, i);
                            song.Name = music[3];
                            song.ToolTipText = music[0] + System.Environment.NewLine + music[1] + System.Environment.NewLine + music[2];
                            listview.Items.Add(song);
                            i++;
                            song = null;
                        }

                    }
                    listview.EndUpdate();
                }

            }

            /// <summary>
            /// to prepare the custom source for the search bar
            /// </summary>
            /// <param name="playlist"></param>
            public static async void SearchBarAutoCompleteSource(string[][] playlist)
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

                CurrentForm.searchMusicListView.AutoCompleteCustomSource.AddRange(tempTips.ToArray());
                tempTips = null;

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
                    if (tempformat.Length > 1)
                    {
                        format = audioPath.Split('.')[tempformat.Length - 1].ToUpper();
                    }
                    else
                    {
                        format = audioPath.Split('.')[1].ToUpper();
                    }

                    if (musicTags.Tag.Title == null)
                    {
                        System.IO.FileInfo sth = new System.IO.FileInfo(path);
                        title = sth.Name;
                    }
                    else
                    {
                        title = musicTags.Tag.Title;
                    }

                    //for artists
                    var allArtists = musicTags.Tag.Performers;
                    if (allArtists.Length == 0)
                    {
                        artist = "";
                    }
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
                    else  //why? idk 
                    { artist = musicTags.Tag.FirstPerformer; }

                    //For Album
                    if (musicTags.Tag.Album == null)
                    {
                        album = "";
                    }
                    else
                    {
                        album = musicTags.Tag.Album;
                    }

                    //For genre
                    if (musicTags.Tag.Genres.Length == 0)
                    {
                        genre = "";
                    }
                    else
                    {
                        genre = musicTags.Tag.Genres[0];
                    }
                    var temp = musicTags.Tag.Lyrics;
                    if (temp != null)
                    {
                        lyrics = musicTags.Tag.Lyrics.ReplaceLineEndings();
                    }
                    //


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


        public class FilterDuplicates : IDisposable
        {
            private bool disposed;
            public string[][]? selectedMusicsData;

            public FilterDuplicates(string[][] selectedMusicsInfo)
            {
                //string[][] selectedMusicsData;
                List<string[]> filesList = new List<string[]>();


                if (System.IO.File.Exists(allMusicDataBase))
                {
                    XmlDocument MusicDataBase = new XmlDocument();
                    XmlElement AllSongs;  //the document root node
                    MusicDataBase.Load(allMusicDataBase);
                    AllSongs = MusicDataBase.DocumentElement;
                    foreach (var music in selectedMusicsInfo)
                    {
                        bool isDuplicate = false;
                        for (int i = 0; i < AllSongs.ChildNodes.Count; i++)
                        {
                            isDuplicate = false;

                            if (AllSongs.ChildNodes[i].ChildNodes[3].InnerText == music[3])
                            {
                                isDuplicate = true;
                                break;
                            }
                        }
                        if (isDuplicate == false)
                        {
                            filesList.Add(music);
                        }
                    }

                    selectedMusicsData = filesList.ToArray();
                }

                else
                {
                    int i = 0;
                    selectedMusicsData = (string[][]?)selectedMusicsInfo.Clone();

                }

            }

            /// <summary>
            /// Destructor
            /// </summary>
            ~FilterDuplicates()
            {
                this.Dispose(false);
            }

            /// <summary>
            /// The dispose method that implements IDisposable.
            /// </summary>
            public void Dispose()
            {
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
                        selectedMusicsData = null;
                    }

                    // Dispose unmanaged resources here.
                }

                disposed = true;
            }

            /// <summary>
            /// checks the entire Database for duplicates, removes them, and reads the new database in complete mode
            /// </summary>
            /// <param name="allSongsListView"></param>
            /// <returns></returns>
            public static (string[][]?, Image[]?) TryRepairingDataBase(ListView allSongsListView)
            {
                CheckDataBaseAndRemoveDuplicates();

                string[][]? checkedDataBaseInfo;
                List<string[]> CheckedFilesList = new List<string[]>();
                Image[]? Arts;



                checkedDataBaseInfo = AudioDataBase.ReadAudioDataBase("complete");

                Arts = AudioDataBase.GetMusicThumbnails(checkedDataBaseInfo);


                return (checkedDataBaseInfo, Arts);
            }

            ///<summary> 
            /// checks the entire Database for duplicates and removes them (in addition to the art files)
            /// </summary>
            private static async void CheckDataBaseAndRemoveDuplicates()
            {
                bool duplicatesFound = false;
                string[][] dataBaseInfo = AudioDataBase.ReadAudioDataBase("complete");

                string[]? artFileNames;

                string[]? allArtFilePaths = new string[dataBaseInfo.Length];
                List<string> tempArtsPaths = new List<string>();

                await Task.Run(() =>
                {
                    foreach (var songInfo in dataBaseInfo)
                    {
                        tempArtsPaths.Add(songInfo[4]);
                        tempArtsPaths.Add(songInfo[5]);
                    }
                    allArtFilePaths = (string[])tempArtsPaths.ToArray().Clone();
                    tempArtsPaths.Clear();
                    tempArtsPaths = null;

                    List<string> artsToRemove = new List<string>();
                    XmlDocument MusicDataBase = new XmlDocument();
                    XmlElement AllSongs;  //the document root node
                    MusicDataBase.Load(allMusicDataBase);
                    AllSongs = MusicDataBase.DocumentElement;

                    foreach (var music in dataBaseInfo)
                    {
                        bool isDuplicate = false;
                        int g = 0;
                        for (int i = 0; i < AllSongs.ChildNodes.Count; i++)
                        {


                            if (AllSongs.ChildNodes[i].ChildNodes[3].InnerText == music[3])
                            {
                                g++;
                                if (g == 1) { continue; }
                                isDuplicate = true;
                                duplicatesFound = true;

                                AllSongs.RemoveChild(AllSongs.ChildNodes[i]);

                                //For removing the pic
                                artsToRemove.Add(music[4]);
                                artsToRemove.Add(music[5]);


                            }


                            i--; // since the nodes will rearrange themselves, if the counter continues normally
                                 // it will skip an item, hence the need for this

                        }

                    }

                    if (artsToRemove.Count > 0)
                    {
                        foreach (var artfile in artsToRemove)
                        {
                            if (System.IO.File.Exists(artfile))
                            {
                                System.IO.File.Delete(artfile);
                            }
                        }
                    }

                    MusicDataBase.Save(AudioDataBase.allMusicDataBase);

                    artsToRemove = null;
                });

                GC.Collect();

            }

            public static string[]? FilterPlaylistDuplicates(string[] selectedMusicInfo, string? playlistPath)
            {
                string[]? checkedMusicInfo;
                if (System.IO.File.Exists(playlistPath))
                {
                    XmlDocument playlistDataBase = new XmlDocument();
                    XmlElement playlistSongs;  //the document root node
                    playlistDataBase.Load(playlistPath);
                    playlistSongs = playlistDataBase.DocumentElement;

                    bool isDuplicate = false;
                    for (int i = 0; i < playlistSongs.ChildNodes.Count; i++)
                    {
                        isDuplicate = false;

                        if (playlistSongs.ChildNodes[i].ChildNodes[3].InnerText == selectedMusicInfo[3])
                        {
                            isDuplicate = true;
                            break;
                        }
                    }
                    if (isDuplicate == false)
                    {
                        checkedMusicInfo = selectedMusicInfo;
                    }
                    else
                    {
                        checkedMusicInfo = null;
                    }
                }
                else
                {
                    checkedMusicInfo = selectedMusicInfo;
                }

                return checkedMusicInfo;
            }

            public static string[][]? FilterPlaylistDuplicates(string[][] selectedMusicsInfo, string? playlistPath)
            {
                List<string[]> tempChecked = new List<string[]>();
                string[][]? checkedMusicInfo;

                List<string[]> filesList = new List<string[]>();
                filesList.Clear();


                if (System.IO.File.Exists(playlistPath))
                {
                    XmlDocument playlistDataBase = new XmlDocument();
                    XmlElement playlistSongs;  //the document root node
                    playlistDataBase.Load(playlistPath);
                    playlistSongs = playlistDataBase.DocumentElement;
                    foreach (var music in selectedMusicsInfo)
                    {
                        bool isDuplicate = false;
                        for (int i = 0; i < playlistSongs.ChildNodes.Count; i++)
                        {
                            isDuplicate = false;

                            if (playlistSongs.ChildNodes[i].ChildNodes[3].InnerText == music[3])
                            {
                                isDuplicate = true;
                                break;
                            }
                        }
                        if (isDuplicate == false)
                        {
                            filesList.Add(music);
                        }
                    }

                    checkedMusicInfo = filesList.ToArray();
                }

                else
                {
                    int i = 0;
                    checkedMusicInfo = (string[][]?)selectedMusicsInfo.Clone();


                }

                return checkedMusicInfo;
            }
        }

        ///<summary> 
        /// Allows writing Dragged files' paths into a txt file and extraction of their properties
        /// </summary>
        public class AudioDataBase : IDisposable
        {
            public string[][]? AllMusicInfo { get; set; }
            public System.Drawing.Image[]? AllMusicArts { get; set; }
            public string[]? ArtFileNames { get; set; }
            public string[][]? AddedMusicInfo { get; set; }
            public System.Drawing.Image[]? AddedMusicArts { get; set; }
            public string[]? AddedArtFileNames { get; set; }
            public static int AddedSongCount = 0;

            public static bool errorDetected = false;
            public static bool isDataBaseRead { get; internal set; } = false;

            public string[][]? playlist1MusicInfo { get; set; }
            public System.Drawing.Image[]? playlist1MusicArts { get; set; }
            public string[][]? playlist2MusicInfo { get; set; }
            public System.Drawing.Image[]? playlist2MusicArts { get; set; }
            public string[][]? playlist3MusicInfo { get; set; }
            public System.Drawing.Image[]? playlist3MusicArts { get; set; }
            public string[][]? playlist4MusicInfo { get; set; }
            public System.Drawing.Image[]? playlist4MusicArts { get; set; }
            public string[][]? playlist5MusicInfo { get; set; }
            public System.Drawing.Image[]? playlist5MusicArts { get; set; }



            public static bool ThumbnailCallback()
            {
                return false;
            }

            internal static string applicationPath = System.Windows.Forms.Application.StartupPath;
            internal static string albumArtsPath = applicationPath + "Album Arts\\";
            internal static string albumArtsThumbnailsPath = applicationPath + "Album Arts Thumbnails\\";
            internal static string allMusicDataBase = System.Windows.Forms.Application.StartupPath + "AllMusicDataBase.xml";
            internal static string playListIni = System.Windows.Forms.Application.StartupPath + "PlaylistIni.xml";


            internal string[]? addedMusicsPaths;
            internal static string[][]? addedMusicInfo = new string[1][];
            internal static System.Drawing.Image[]? addedMusicArts = new System.Drawing.Image[1];
            private bool disposed;



            /// <summary>
            /// Reads from the data base, Use after the data base has been created
            /// </summary>
            public AudioDataBase()
            {

                if (System.IO.File.Exists(allMusicDataBase))
                {
                    RemoveInvalidDatabaseElements();

                    XmlDocument tempMusicDataBase = new XmlDocument();
                    tempMusicDataBase.Load(allMusicDataBase);
                    if (tempMusicDataBase.DocumentElement.ChildNodes.Count > 0)
                    {
                        // they are simply initialized so they can be properly disposed later without error
                        AddedArtFileNames = new string[1];
                        AddedMusicArts = new Image[1];
                        AddedMusicInfo = new string[1][];

                        AllMusicInfo = ReadAudioDataBase("complete");

                        AllMusicArts = GetMusicThumbnails(AllMusicInfo);

                        if (errorDetected == true)
                        { isDataBaseRead = false; }
                        else
                        { isDataBaseRead = true; }


                    }
                    else // if the data base doesn't have any elements, then it is practically empty and there is no need for any operation
                    {
                        isDataBaseRead = false;

                    }
                }
                else // if the data base doesn't exist, it will be created when a song is added
                {
                    isDataBaseRead = false;

                }
            }

            ///<summary>
            ///Creates, writes to, and reads from Data Base, using the provided paths of audio files
            ///</summary>
            public AudioDataBase(string[] addSongsPaths)
            {
                Image thumbnail;
                AllMusicArts = new Image[1];
                addedMusicsPaths = addSongsPaths;
                GetAudioFilesInfo(addSongsPaths);

                AddedSongCount = WriteAudioDataBase(addedMusicInfo);
                GC.Collect();

                AddedMusicInfo = ReadAudioDataBase("added");

                //
                if (allMusicInfo != null && allMusicInfo[0] != null)
                {
                    AllMusicInfo = ReadAudioDataBase("complete");
                }
                else
                {
                    AllMusicInfo = AddedMusicInfo;
                }

                AddedMusicArts = GetMusicThumbnails(AddedMusicInfo);

                isDataBaseRead = true;
            }

            /// <summary>
            /// Destructor
            /// </summary>
            ~AudioDataBase()
            {
                AllMusicArts = null;
                AllMusicInfo = null;
                ArtFileNames = null;
                AddedMusicInfo = null;
                AddedMusicArts = null;
                AddedArtFileNames = null;
                addedMusicArts = null;
                addedMusicInfo = null;
                addedMusicsPaths = null;
                Dispose(false);
                GC.Collect();
            }

            /// <summary>
            /// The dispose method that implements IDisposable.
            /// </summary>
            public void Dispose()
            {
                AllMusicArts = null;
                AllMusicInfo = null;
                ArtFileNames = null;
                AddedMusicInfo = null;
                AddedMusicArts = null;
                AddedArtFileNames = null;
                addedMusicArts = null;
                addedMusicInfo = null;
                addedMusicsPaths = null;
                Dispose(true);

                GC.SuppressFinalize(this);
                GC.Collect();
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
                        AllMusicArts = null;
                        AllMusicInfo = null;
                        ArtFileNames = null;
                        AddedMusicInfo = null;
                        AddedMusicArts = null;
                        AddedArtFileNames = null;
                        addedMusicArts = null;
                        addedMusicInfo = null;
                        addedMusicsPaths = null;


                    }

                    // Dispose unmanaged resources here.

                }

                disposed = true;
                GC.Collect();
            }


            ///<summary>
            ///Gets the various properties of selected Audio Files  (e.g., title, artist, album, etc)
            ///</summary>
            public static string[][]? GetAudioFilesInfo(string[] draggedMusicsPaths)
            {
                //1-title 2-artist 3-album 4-path 5- art path 6-thumbnail path 

                string[][] tempMusicInfos = new string[draggedMusicsPaths.Length][];
                Image[] tempMusicArts = new Image[draggedMusicsPaths.Length];


                int i = 0;
                foreach (var audioPath in draggedMusicsPaths)
                {
                    string? artist, album;
                    string title, path; // these can't be empty

                    string[] musicInfo = new string[4];

                    TagLib.File musicTags = TagLib.File.Create(audioPath);
                    //Check to see if the file or file tags are corrupted
                    if (musicTags.PossiblyCorrupt)
                    {
                        var corruptionReasons = musicTags.CorruptionReasons.ToArray();
                        // Add that it should return this string array if it is corrupted
                    }
                    else
                    {
                        /*
                            //getting a picture of the audio file
                            var tempPics = musicTags.Tag.Pictures;

                            if (tempPics.Length > 0)
                            {
                                using (MemoryStream picConverter = new MemoryStream(tempPics[0].Data.Data))
                                {
                                    art = Image.FromStream(picConverter);
                                }
                            }
                            else { art = Properties.Resources.MusicArt_NoCover; }

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

                            //to get track number in Album
                            uint tempTrack = musicTags.Tag.Track;
                            if (tempTrack == 0) { trackNumber = ""; }
                            else { trackNumber = tempTrack.ToString(); }
                        */
                        path = audioPath;


                        if (musicTags.Tag.Title == null)
                        {
                            System.IO.FileInfo sth = new System.IO.FileInfo(path);
                            title = (string)sth.Name.Clone();

                        }
                        else
                        {
                            title = musicTags.Tag.Title;
                        }

                        //for artists
                        var allArtists = musicTags.Tag.Performers;
                        if (allArtists.Length == 0)
                        {
                            artist = "";
                        }
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
                        else  //why? idk 
                        { artist = musicTags.Tag.FirstPerformer; }

                        //For Album
                        if (musicTags.Tag.Album == null)
                        {
                            album = "";
                        }
                        else
                        {
                            album = musicTags.Tag.Album;
                        }

                        /*
                        if (musicTags.Tag.Lyrics != null)
                        {
                            lyrics = musicTags.Tag.Lyrics.ReplaceLineEndings();
                        }
                        else { lyrics = ""; }
                        */

                        musicInfo[0] = title;
                        musicInfo[1] = artist;
                        musicInfo[2] = album;
                        musicInfo[3] = path;
                        tempMusicInfos[i] = musicInfo;

                        musicTags.Dispose();

                        i++;
                    }

                }

                addedMusicInfo = (string[][]?)tempMusicInfos.Clone();

                //to dispose
                //musicInfo = null;
                //tempMusicArts = null;
                tempMusicInfos = null;
                //lyrics = null;
                //title = null;
                //artist = null;
                //album = null;
                //art = null;

                return addedMusicInfo;

            }


            ///<summary>
            ///Adds the selected or dragged songs' info to the data base
            ///</summary>
            public static int WriteAudioDataBase(string[][] selectedMusicsInfo)
            {
                string path;
                string? title, artist, album;
                Image? art;
                Image? thumbnail;
                string[][]? selectedMusicsData;

                Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                using (FilterDuplicates duplicateCheck = new FilterDuplicates(selectedMusicsInfo))
                {
                    selectedMusicsData = (string[][]?)duplicateCheck.selectedMusicsData.Clone();
                }

                string[]? fileNames = new string[selectedMusicsData.Length];


                XmlElement AllSongs;  //the document root node
                XmlDocument allMusicDataBaseDoc = new XmlDocument();
                if (System.IO.File.Exists(allMusicDataBase))
                {
                    allMusicDataBaseDoc.Load(allMusicDataBase);
                    if (allMusicDataBaseDoc.DocumentElement != null)
                    {
                        AllSongs = allMusicDataBaseDoc.DocumentElement;
                    }
                    else
                    {
                        AllSongs = allMusicDataBaseDoc.CreateElement("ArrayOfArrayOfString");
                    }
                }
                else
                {
                    AllSongs = allMusicDataBaseDoc.CreateElement("ArrayOfArrayOfString");

                    allMusicDataBaseDoc.AppendChild(AllSongs);

                }
                AllSongs.SetAttribute("playlist", "All Songs Playlist");

                if (!System.IO.File.Exists(albumArtsPath))
                {
                    System.IO.Directory.CreateDirectory(albumArtsPath);
                }
                if (!System.IO.Directory.Exists(albumArtsThumbnailsPath))
                {
                    System.IO.Directory.CreateDirectory(albumArtsThumbnailsPath);
                }

                FileStreamOptions options = new FileStreamOptions();
                options.Options = FileOptions.None;
                options.Access = FileAccess.ReadWrite;
                options.Share = FileShare.ReadWrite;
                options.Mode = FileMode.OpenOrCreate;
                options.BufferSize = 4096;
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.ConformanceLevel = ConformanceLevel.Document;
                settings.Encoding = Encoding.UTF8;
                settings.Indent = true;
                settings.NewLineChars = System.Environment.NewLine;

                using (StreamWriter datastream = new StreamWriter(allMusicDataBase, Encoding.UTF8, options))
                {
                    XmlWriter dataBaseWriter = XmlWriter.Create(datastream, settings);
                    int i = 0;
                    foreach (string[] musicDataArray in selectedMusicsData)
                    {

                        title = musicDataArray[0];
                        artist = musicDataArray[1];
                        album = musicDataArray[2];
                        path = musicDataArray[3];
                        // FOR ART
                        TagLib.File musicTags = TagLib.File.Create(path);
                        var tempPics = musicTags.Tag.Pictures;

                        MemoryStream picConverter;
                        if (tempPics.Length > 0)
                        {
                            picConverter = new MemoryStream(tempPics[0].Data.Data);
                            art = (Image)Image.FromStream(picConverter).Clone();
                            thumbnail = art.GetThumbnailImage(60, 60, myCallback, 0);

                        }
                        else
                        {
                            art = Khi_Player.Properties.Resources.Khi_Player;
                            thumbnail = Khi_Player.Properties.Resources.Khi_Player.GetThumbnailImage(60, 60, myCallback, 0);
                        }
                        //"\\" + 
                        var tempName = System.IO.Path.GetFileName(path).Split('.');
                        int dotcount;
                        TextBox name = new TextBox();
                        if (tempName.Length > 2)
                        {
                            dotcount = tempName.Length - 1;
                            for (int z = 0; z < dotcount; z++)
                            {
                                name.AppendText(tempName[z]);
                                if (z + 1 < dotcount) { name.AppendText("."); }
                            }
                            fileNames[i] = name.Text;

                        }
                        else
                        {
                            dotcount = 1;
                            fileNames[i] = System.IO.Path.GetFileName(path).Split('.')[dotcount - 1];  //actually incomplete so it can be added to albumArtsPath
                        }



                        string imagePath = albumArtsPath + fileNames[i] + ".bmp";
                        string imageThumbnailPath = albumArtsThumbnailsPath + fileNames[i] + ".bmp";

                        using (FileStream artSaver = new FileStream(imagePath, FileMode.Create))
                        {
                            art.Save(artSaver, art.RawFormat);
                        }
                        fileNames[i] = imagePath;

                        //for saving thumbnails
                        using (FileStream thumbnailSaver = new FileStream(imageThumbnailPath, FileMode.Create))
                        {
                            thumbnail.Save(thumbnailSaver, art.RawFormat);
                        }

                        musicTags.Dispose();
                        name.Dispose();

                        XmlElement Song = allMusicDataBaseDoc.CreateElement("ArrayOfString");  //Child of AllSongs, and parent to the other elements

                        if (allMusicDataBaseDoc.DocumentElement.HasChildNodes)
                        {
                            allMusicDataBaseDoc.DocumentElement.InsertAfter(Song, allMusicDataBaseDoc.DocumentElement.LastChild);
                        }
                        else
                        {
                            allMusicDataBaseDoc.DocumentElement.AppendChild(Song);
                        }

                        XmlElement Title = allMusicDataBaseDoc.CreateElement("string");
                        XmlElement Artist = allMusicDataBaseDoc.CreateElement("string");
                        XmlElement Album = allMusicDataBaseDoc.CreateElement("string");
                        XmlElement Path = allMusicDataBaseDoc.CreateElement("string");
                        XmlElement ArtPath = allMusicDataBaseDoc.CreateElement("string");
                        XmlElement ThumbnailPath = allMusicDataBaseDoc.CreateElement("string");


                        Title.InnerText = title;
                        Artist.InnerText = artist;
                        Album.InnerText = album;
                        Path.InnerText = path;
                        ArtPath.InnerText = imagePath;
                        ThumbnailPath.InnerText = imageThumbnailPath;

                        Song.AppendChild(Title);
                        Song.AppendChild(Artist);
                        Song.AppendChild(Album);
                        Song.AppendChild(Path);
                        Song.AppendChild(ArtPath);
                        Song.AppendChild(ThumbnailPath);

                        i++;
                    }
                    allMusicDataBaseDoc.Save(dataBaseWriter);
                    dataBaseWriter.Close();
                    dataBaseWriter.Dispose();
                }

                int addedCount = selectedMusicsData.Length;
               
                //for disposal

                selectedMusicsData = null;
                selectedMusicsInfo = null;
                options = null;
                allMusicDataBaseDoc = null;
                art = null;
                thumbnail = null;
                fileNames = null;
                title = null;
                artist = null;
                album = null;
                path = null;

                return addedCount;
            }

            /// <summary>
            /// Adds a single song's info to the specified playlist's data base, returns null if the song already exists in the database.
            /// </summary>
            /// <param name="playlistMusic"></param>
            /// <param name="playlist"></param>
            /// <param name="nameOfPlaylist"></param>
            /// <returns></returns>
            public static string? WriteSongToPlaylistDatabase(string[]? playlistMusic, string playlist, string? nameOfPlaylist)
            {
                string[]? checkedPlaylistMusic;
                string? title, artist, album, path, artPath, thumbnailPath;
                string? playlistName;


                checkedPlaylistMusic = FilterDuplicates.FilterPlaylistDuplicates(playlistMusic, playlist);
                XmlDocument playlistDatabase = new XmlDocument();
                XmlElement playlistSongs;  //the document root node
                if (checkedPlaylistMusic != null)
                {
                    if (System.IO.File.Exists(playlist))
                    {
                        playlistDatabase.Load(playlist);
                        if (playlistDatabase.DocumentElement != null)
                        {
                            playlistSongs = playlistDatabase.DocumentElement;
                            if (playlistSongs.HasAttributes)
                            {
                                playlistName = playlistSongs.GetAttribute("playlistName");
                            }
                        }
                        else
                        {
                            playlistSongs = playlistDatabase.CreateElement("ArrayOfArrayOfString");
                            playlistSongs.SetAttribute("playlist", nameOfPlaylist);
                        }
                    }
                    else
                    {
                        playlistSongs = playlistDatabase.CreateElement("ArrayOfArrayOfString");
                        playlistSongs.SetAttribute("playlist", nameOfPlaylist);
                        playlistDatabase.AppendChild(playlistSongs);

                    }

                    XmlElement Song = playlistDatabase.CreateElement("ArrayOfString");
                    if (playlistDatabase.DocumentElement.HasChildNodes)
                    {
                        playlistDatabase.DocumentElement.InsertAfter(Song, playlistDatabase.DocumentElement.LastChild);
                    }
                    else
                    {
                        playlistDatabase.DocumentElement.AppendChild(Song);
                    }

                    title = checkedPlaylistMusic[0];
                    artist = checkedPlaylistMusic[1];
                    album = checkedPlaylistMusic[2];
                    path = checkedPlaylistMusic[3];
                    artPath = checkedPlaylistMusic[4];
                    thumbnailPath = checkedPlaylistMusic[5];

                    XmlElement Title = playlistDatabase.CreateElement("string");
                    XmlElement Artist = playlistDatabase.CreateElement("string");
                    XmlElement Album = playlistDatabase.CreateElement("string");
                    XmlElement Path = playlistDatabase.CreateElement("string");
                    XmlElement ArtPath = playlistDatabase.CreateElement("string");
                    XmlElement ThumbnailPath = playlistDatabase.CreateElement("string");

                    Title.InnerText = title;
                    Artist.InnerText = artist;
                    Album.InnerText = album;
                    Path.InnerText = path;
                    ArtPath.InnerText = artPath;
                    ThumbnailPath.InnerText = thumbnailPath;

                    Song.AppendChild(Title);
                    Song.AppendChild(Artist);
                    Song.AppendChild(Album);
                    Song.AppendChild(Path);
                    Song.AppendChild(ArtPath);
                    Song.AppendChild(ThumbnailPath);

                    FileStreamOptions options = new FileStreamOptions();
                    options.Options = FileOptions.None;
                    options.Access = FileAccess.ReadWrite;
                    options.Share = FileShare.ReadWrite;
                    options.Mode = FileMode.OpenOrCreate;
                    options.BufferSize = 4096;
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.ConformanceLevel = ConformanceLevel.Document;
                    settings.Encoding = Encoding.UTF8;
                    settings.Indent = true;
                    settings.NewLineChars = System.Environment.NewLine;

                    using (StreamWriter datastream = new StreamWriter(playlist, Encoding.UTF8, options))
                    {
                        XmlWriter dataBaseWriter = XmlWriter.Create(datastream, settings);
                        playlistDatabase.Save(dataBaseWriter);
                        dataBaseWriter.Dispose();
                    }

                    //for disposal
                    checkedPlaylistMusic = null;
                    playlistDatabase = null;
                    title = null;
                    artist = null;
                    album = null;
                    path = null;
                    artPath = null;
                    thumbnailPath = null;
                    playlistName = null;

                    return playlist;
                }
                else
                {
                    return null;
                }

               
            }

            ///<summary>
            ///Use this for now: Async, Adds the selected songs' info to the specified playlist's data base
            ///</summary>
            public static async void WriteAudioDataBase(string[][]? playlistMusicData, string? playlistPath, string? nameOfPlaylist)
            {
                string[][]? checkedPlaylistMusicData;
                string title, artist, album, path, artPath, thumbnailPath;
                string? playlistName;
                string? playlist = playlistPath;

                // Duplicate Check
                checkedPlaylistMusicData = FilterDuplicates.FilterPlaylistDuplicates(playlistMusicData, playlist);

                await Task.Run(() =>
                {
                    XmlDocument playlistDatabase = new XmlDocument();
                    XmlElement playlistSongs;  //the document root node
                    if (System.IO.File.Exists(playlist))
                    {
                        playlistDatabase.Load(playlist);
                        if (playlistDatabase.DocumentElement != null)
                        {
                            playlistSongs = playlistDatabase.DocumentElement;
                            if (playlistSongs.HasAttributes)
                            {
                                playlistName = playlistSongs.GetAttribute("playlistName");
                            }
                        }
                        else
                        {
                            playlistSongs = playlistDatabase.CreateElement("ArrayOfArrayOfString");
                            playlistSongs.SetAttribute("playlist", nameOfPlaylist);
                        }
                    }
                    else
                    {
                        playlistSongs = playlistDatabase.CreateElement("ArrayOfArrayOfString");
                        playlistSongs.SetAttribute("playlist", nameOfPlaylist);
                        playlistDatabase.AppendChild(playlistSongs);

                    }

                    foreach (var music in checkedPlaylistMusicData)
                    {
                        XmlElement Song = playlistDatabase.CreateElement("ArrayOfString");
                        if (playlistSongs.HasChildNodes)
                        {
                            playlistSongs.InsertAfter(Song, playlistSongs.LastChild);
                        }
                        else
                        {
                            playlistSongs.AppendChild(Song);
                        }

                        title = music[0];
                        artist = music[1];
                        album = music[2];
                        path = music[3];
                        artPath = music[4];
                        thumbnailPath = music[5];

                        XmlElement Title = playlistDatabase.CreateElement("string");
                        XmlElement Artist = playlistDatabase.CreateElement("string");
                        XmlElement Album = playlistDatabase.CreateElement("string");
                        XmlElement Path = playlistDatabase.CreateElement("string");
                        XmlElement ArtPath = playlistDatabase.CreateElement("string");
                        XmlElement ThumbnailPath = playlistDatabase.CreateElement("string");

                        Title.InnerText = title;
                        Artist.InnerText = artist;
                        Album.InnerText = album;
                        Path.InnerText = path;
                        ArtPath.InnerText = artPath;
                        ThumbnailPath.InnerText = thumbnailPath;

                        Song.AppendChild(Title);
                        Song.AppendChild(Artist);
                        Song.AppendChild(Album);
                        Song.AppendChild(Path);
                        Song.AppendChild(ArtPath);
                        Song.AppendChild(ThumbnailPath);
                    }

                    FileStreamOptions options = new FileStreamOptions();
                    options.Options = FileOptions.None;
                    options.Access = FileAccess.ReadWrite;
                    options.Share = FileShare.ReadWrite;
                    options.Mode = FileMode.OpenOrCreate;
                    options.BufferSize = 4096;
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.ConformanceLevel = ConformanceLevel.Document;
                    settings.Encoding = Encoding.UTF8;
                    settings.Indent = true;
                    settings.NewLineChars = System.Environment.NewLine;

                    using (StreamWriter datastream = new StreamWriter(playlist, Encoding.UTF8, options))
                    {
                        XmlWriter dataBaseWriter = XmlWriter.Create(datastream, settings);
                        playlistDatabase.Save(dataBaseWriter);
                        dataBaseWriter.Dispose();
                    }

                    //for disposal
                    playlistDatabase = null;
                });

                //for disposal
                checkedPlaylistMusicData = null;
                title = null;
                artist = null;
                album = null;
                path = null;
                artPath = null;
                thumbnailPath = null;
                playlistName = null;
            }

            /// <summary>
            /// Synchronously Adds the selected songs' info to the specified playlist's data base
            /// </summary>
            /// <param name="playlistMusicData"></param>
            /// <param name="playlist"></param>
            /// <param name="nameOfPlaylist"></param>
            public static void WriteAudioDataBaseSync(string[][]? playlistMusicData, string playlist, string? nameOfPlaylist)
            {
                string[][]? checkedPlaylistMusicData;
                string title, artist, album, path, artPath, thumbnailPath;
                string? playlistName;
                
                // Duplicate Check
                checkedPlaylistMusicData = FilterDuplicates.FilterPlaylistDuplicates(playlistMusicData, playlist);

                    XmlDocument playlistDatabase = new XmlDocument();
                    XmlElement playlistSongs;  //the document root node
                    if (System.IO.File.Exists(playlist))
                    {
                        playlistDatabase.Load(playlist);
                        if (playlistDatabase.DocumentElement != null)
                        {
                            playlistSongs = playlistDatabase.DocumentElement;
                            if (playlistSongs.HasAttributes)
                            {
                                playlistName = playlistSongs.GetAttribute("playlistName");
                            }
                        }
                        else
                        {
                            playlistSongs = playlistDatabase.CreateElement("ArrayOfArrayOfString");
                            playlistSongs.SetAttribute("playlist", nameOfPlaylist);
                        }
                    }
                    else
                    {
                        playlistSongs = playlistDatabase.CreateElement("ArrayOfArrayOfString");
                        playlistSongs.SetAttribute("playlist", nameOfPlaylist);
                        playlistDatabase.AppendChild(playlistSongs);

                    }

                    foreach (var music in checkedPlaylistMusicData)
                    {
                        XmlElement Song = playlistDatabase.CreateElement("ArrayOfString");
                        if (playlistSongs.HasChildNodes)
                        {
                            playlistSongs.InsertAfter(Song, playlistSongs.LastChild);
                        }
                        else
                        {
                            playlistSongs.AppendChild(Song);
                        }

                        title = music[0];
                        artist = music[1];
                        album = music[2];
                        path = music[3];
                        artPath = music[4];
                        thumbnailPath = music[5];

                        XmlElement Title = playlistDatabase.CreateElement("string");
                        XmlElement Artist = playlistDatabase.CreateElement("string");
                        XmlElement Album = playlistDatabase.CreateElement("string");
                        XmlElement Path = playlistDatabase.CreateElement("string");
                        XmlElement ArtPath = playlistDatabase.CreateElement("string");
                        XmlElement ThumbnailPath = playlistDatabase.CreateElement("string");

                        Title.InnerText = title;
                        Artist.InnerText = artist;
                        Album.InnerText = album;
                        Path.InnerText = path;
                        ArtPath.InnerText = artPath;
                        ThumbnailPath.InnerText = thumbnailPath;

                        Song.AppendChild(Title);
                        Song.AppendChild(Artist);
                        Song.AppendChild(Album);
                        Song.AppendChild(Path);
                        Song.AppendChild(ArtPath);
                        Song.AppendChild(ThumbnailPath);
                    }

                    FileStreamOptions options = new FileStreamOptions();
                    options.Options = FileOptions.None;
                    options.Access = FileAccess.ReadWrite;
                    options.Share = FileShare.ReadWrite;
                    options.Mode = FileMode.OpenOrCreate;
                    options.BufferSize = 4096;
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.ConformanceLevel = ConformanceLevel.Document;
                    settings.Encoding = Encoding.UTF8;
                    settings.Indent = true;
                    settings.NewLineChars = System.Environment.NewLine;

                    using (StreamWriter datastream = new StreamWriter(playlist, Encoding.UTF8, options))
                    {
                        XmlWriter dataBaseWriter = XmlWriter.Create(datastream, settings);
                        playlistDatabase.Save(dataBaseWriter);
                        dataBaseWriter.Dispose();
                    }

                //for disposal
                checkedPlaylistMusicData = null;
                playlistDatabase = null;
                title = null;
                artist = null;
                album = null;
                path = null;
                artPath = null;
                thumbnailPath = null;
                playlistName = null;
            }


            /// <summary>
            /// Reads the specified database. returns the name of the playlist, the thumbnails and 
            /// audio info of recently added audio if "added" is included and all of the audio files' if "complete" is included
            /// </summary>
            /// <param name="playlistPath"></param>
            /// <param name="ReadingMode"></param>
            /// <returns></returns>
            public static (string?, string[][]?, Image[]?) ReadPlaylistDataBase(string? playlistPath, string? ReadingMode)
            {
                string? playlistName;
                string[][]? tempPlaylistMusicInfo;
                Image[]? playlistArts;
                XmlDocument playlistDatabase = new XmlDocument();
                XmlElement playlistSongs;  //the document root node

                playlistDatabase.Load(playlistPath);
                playlistSongs = playlistDatabase.DocumentElement;
                playlistName = playlistSongs.GetAttribute("playlist");

                if (ReadingMode == "added")
                {
                    int added;
                    XmlSerializer dataBaseSerializer = new XmlSerializer(typeof(string[][]));
                    using (StreamReader textReader = new StreamReader(playlistPath, Encoding.UTF8))
                    {
                        tempPlaylistMusicInfo = (string[][])dataBaseSerializer.Deserialize(textReader);
                    }


                    added = AddedSongCount;
                    string[][]? tempAddedMusicInfos = new string[added][];
                    int z = 0;
                    for (int i = tempPlaylistMusicInfo.Length - added; i < tempPlaylistMusicInfo.Length; i++)
                    {
                        tempAddedMusicInfos[z] = (string[])tempPlaylistMusicInfo[i].Clone();
                        z++;
                    }
                    
                    dataBaseSerializer = null;

                }

                else //in case of "complete", written like this for simplicity since it didn't need to be complicated as it is only a reading process
                {

                    XmlSerializer dataBaseSerializer = new XmlSerializer(typeof(string[][]));
                    using (StreamReader textReader = new StreamReader(playlistPath, Encoding.UTF8))
                    {
                        tempPlaylistMusicInfo = (string[][])dataBaseSerializer.Deserialize(textReader);
                    }

                    dataBaseSerializer = null;
                }
                playlistArts = GetMusicThumbnails(tempPlaylistMusicInfo);

                return (playlistName, tempPlaylistMusicInfo, playlistArts);
            }

            /// <summary>
            /// Reads the data base that contains all music files info. returns the name of the playlist, the thumbnails and 
            /// audio info of recently added audio if "added" is included and all of the audio files' if "complete" is included
            /// </summary>
            /// <param name="ReadingMode"></param>
            /// <returns></returns>
            //public static (string[][], Image[], string[]) ReadAudioDataBase()
            public static string[][]? ReadAudioDataBase(string ReadingMode)
            {
                string[][]? tempAllMusicInfos;

                if (ReadingMode == "added")
                {
                    int added;
                    XmlSerializer dataBaseSerializer = new XmlSerializer(typeof(string[][]));
                    using (StreamReader textReader = new StreamReader(allMusicDataBase, Encoding.UTF8))
                    {
                        tempAllMusicInfos = (string[][])dataBaseSerializer.Deserialize(textReader);
                    }


                    added = AddedSongCount;
                    string[][]? tempAddedMusicInfos = new string[added][];
                    int z = 0;
                    for (int i = tempAllMusicInfos.Length - added; i < tempAllMusicInfos.Length; i++)
                    {
                        tempAddedMusicInfos[z] = (string[])tempAllMusicInfos[i].Clone();
                        z++;
                    }

                    dataBaseSerializer = null;
                    return tempAddedMusicInfos;
                }

                else
                {

                    XmlSerializer dataBaseSerializer = new XmlSerializer(typeof(string[][]));
                    using (StreamReader textReader = new StreamReader(allMusicDataBase, Encoding.UTF8))
                    {
                        tempAllMusicInfos = (string[][])dataBaseSerializer.Deserialize(textReader);
                    }


                    dataBaseSerializer = null;
                    return tempAllMusicInfos;
                }

            }

            /// <summary>
            /// Simply creates a data base with the playlist name that is provided.
            /// </summary>
            /// <param name="playlistName"></param>
            public static async void CreatePlaylistDataBase(string playlistName)
            {
                await Task.Run(() =>
                {
                    XmlDocument playlistDatabase = new XmlDocument();
                    XmlElement playlistSongs = playlistDatabase.CreateElement("ArrayOfArrayOfString");

                    playlistSongs.SetAttribute("playlist", playlistName);
                    playlistDatabase.AppendChild(playlistSongs);

                    FileStreamOptions options = new FileStreamOptions();
                    options.Options = FileOptions.None;
                    options.Access = FileAccess.ReadWrite;
                    options.Share = FileShare.ReadWrite;
                    options.Mode = FileMode.OpenOrCreate;
                    options.BufferSize = 4096;
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.ConformanceLevel = ConformanceLevel.Document;
                    settings.Encoding = Encoding.UTF8;
                    settings.Indent = true;
                    settings.NewLineChars = System.Environment.NewLine;

                    string? playlistDatabasePath = applicationPath + playlistName + ".xml";
                    using (StreamWriter datastream = new StreamWriter(playlistDatabasePath, Encoding.UTF8, options))
                    {
                        XmlWriter dataBaseWriter = XmlWriter.Create(datastream, settings);
                        playlistDatabase.Save(dataBaseWriter);
                        dataBaseWriter.Close();
                        dataBaseWriter = null;
                    }

                    //for disposal
                    playlistDatabase = null;
                    playlistDatabasePath = null;
                });
            }


            /// <summary>
            /// Reads a data base without fetching the thumbnails, returns the data and playlist's name
            /// </summary>
            /// <param name="playlistPath"></param>
            /// <returns></returns>
            public static (string?, string[][]?) ReadPlaylist(string? playlistPath)
            {
                string? playlistName;
                string[][]? playlistData;

                XmlDocument PlaylistDatabase = new XmlDocument();
                XmlElement AllSongs;  //the document root node
                PlaylistDatabase.Load(playlistPath);
                AllSongs = PlaylistDatabase.DocumentElement;

                if (AllSongs.HasChildNodes)
                {
                    playlistName = AllSongs.GetAttribute("playlist");
                    playlistData = new string[AllSongs.ChildNodes.Count][];
                    string[] info;

                    for (int i = 0; i < AllSongs.ChildNodes.Count; i++)
                    {
                        info = new string[6];

                        info[0] = AllSongs.ChildNodes[i].ChildNodes[0].InnerText;
                        info[1] = AllSongs.ChildNodes[i].ChildNodes[1].InnerText;
                        info[2] = AllSongs.ChildNodes[i].ChildNodes[2].InnerText;
                        info[3] = AllSongs.ChildNodes[i].ChildNodes[3].InnerText;
                        info[4] = AllSongs.ChildNodes[i].ChildNodes[4].InnerText;
                        info[5] = AllSongs.ChildNodes[i].ChildNodes[5].InnerText;

                        playlistData[i] = info;
                    }
                }
                else
                {
                    playlistName = null;
                    playlistData = null;
                }

                //for disposal
                PlaylistDatabase = null;


                return (playlistName, playlistData);
            }

            /// <summary>
            /// Removes an item (array of subitems) from the database, its duplicates and delete its corresponding cover art as well
            /// </summary>
            /// <param name="item"></param>
            /// <param name="artFileName"></param>
            /// <param name="artThumbnailName"></param>
            /// <returns></returns>
            public static bool RemoveSong(string[] item, string artFileName, string artThumbnailName, bool fromAllPlaylists = true, string? playlistName = null)
            {
                bool isRemoved = false;
                List<int> similarItemsIndices = new List<int>();
                string title = item[0];
                string? artist = item[1];
                string? album = item[2];
                string path = item[3];

                XmlElement AllSongs;  //the document root node
                XmlDocument musicDatabase = new XmlDocument();



                //Checking to find active Databases to check
                List<string?> existingDatabases = new List<string?>();

                string[] tempExistingDatabases = System.IO.Directory.GetFiles(applicationPath, "*.xml*", SearchOption.TopDirectoryOnly);

                foreach (string? database in tempExistingDatabases)
                {
                    //just to make sure
                    if (System.IO.Path.GetExtension(database).ToUpper() == ".XML")
                       {
                            if (fromAllPlaylists == false)
                            {
                                if (playlistName != null && database.ToUpper() == playlistName.ToUpper())
                                {
                                    existingDatabases.Add(database);
                                    break;
                                }
                            }
                            else
                            {
                                existingDatabases.Add(database);
                            }
                    }
                }
            


                foreach (string? database in existingDatabases)
                {

                    musicDatabase.Load(database);
                    AllSongs = musicDatabase.DocumentElement;

                    for (int i = 0; i < AllSongs.ChildNodes.Count; i++)
                    {

                        if (AllSongs.ChildNodes[i].ChildNodes[3].InnerText == path)
                        {
                            similarItemsIndices.Add(i);
                            AllSongs.RemoveChild(AllSongs.ChildNodes[i]);
                            i--; // since the nodes will rearrange themselves, if the counter continues normally
                                 // it will skip an item, hence the need for this
                        }
                    }
                    
                    if (System.IO.File.Exists(artFileName))
                    {
                        System.IO.File.Delete(artFileName);
                    }

                    if (System.IO.File.Exists(artThumbnailName))
                    {
                        System.IO.File.Delete(artThumbnailName);
                    }

                    musicDatabase.Save(database);
                }

                //for disposal
                musicDatabase = null;
                similarItemsIndices = null;
                title = null;
                artist = null;
                album = null;

                return isRemoved;
            }

            ///<summary>
            /// Checks the Database for songs that Don't exists and Removes them
            ///</summary>
            public static async void RemoveInvalidDatabaseElements()
            {
                await Task.Run(() =>
                {
                    bool InvalidsFoundAndRemoved = false;

                    List<string?>? existingDatabases = new List<string?>();
                    string[] tempExistingDatabases = System.IO.Directory.GetFiles(applicationPath, "*.xml*", SearchOption.TopDirectoryOnly);

                    foreach (string? database in tempExistingDatabases)
                    {
                        //just to make sure
                        if (System.IO.Path.GetExtension(database).ToUpper() == ".XML")
                        {
                            existingDatabases.Add(database);
                        }
                    }


                    if (existingDatabases.Count > 0)
                    {
                        // Options and Settings for saving the files later
                        FileStreamOptions options = new FileStreamOptions();
                        options.Options = FileOptions.None;
                        options.Access = FileAccess.ReadWrite;
                        options.Share = FileShare.ReadWrite;
                        options.Mode = FileMode.OpenOrCreate;
                        options.BufferSize = 4096;
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.ConformanceLevel = ConformanceLevel.Document;
                        settings.Encoding = Encoding.UTF8;
                        settings.Indent = true;
                        settings.NewLineChars = System.Environment.NewLine;

                        foreach (string? database in existingDatabases)
                        {
                            bool isModified = false;
                            string? path;
                            List<string> artsToRemove = new List<string>();
                            XmlDocument musicDatabase = new XmlDocument();
                            XmlElement AllSongs;  //the document root node
                            musicDatabase.Load(database);
                            AllSongs = musicDatabase.DocumentElement;

                            if (AllSongs.HasChildNodes)
                            {
                                for (int i = 0; i < AllSongs.ChildNodes.Count; i++)
                                {
                                    //checks each of the items to see if the audi file exists, deletes the item if it doesn't
                                    if (!System.IO.File.Exists(AllSongs.ChildNodes[i].ChildNodes[3].InnerText))
                                    {
                                        //Getting the cover art and thumbnail path before deleting the item
                                        artsToRemove.Add(AllSongs.ChildNodes[i].ChildNodes[4].InnerText);
                                        artsToRemove.Add(AllSongs.ChildNodes[i].ChildNodes[4].InnerText);

                                        AllSongs.RemoveChild(AllSongs.ChildNodes[i]);

                                        //Removing a node changes the index of all the elements after it, so, this index will be refilled immediatly and hence
                                        //the need for not adding to the counter
                                        i--;
                                        isModified = true;
                                        InvalidsFoundAndRemoved = true;
                                    }
                                }

                                if (artsToRemove.Count > 0)
                                {
                                    foreach (string? artAndThumbnailPath in artsToRemove)
                                    {
                                        if (artAndThumbnailPath != null && System.IO.File.Exists(artAndThumbnailPath))
                                        {
                                            System.IO.File.Delete(artAndThumbnailPath);
                                        }
                                    }
                                }
                            }
                            // Saving the database only if there were changes
                            if (isModified)
                            {
                                using (StreamWriter datastream = new StreamWriter(database, Encoding.UTF8, options))
                                {
                                    XmlWriter dataBaseWriter = XmlWriter.Create(datastream, settings);
                                    musicDatabase.Save(dataBaseWriter);
                                    dataBaseWriter.Dispose();
                                }
                            }

                            //Setting isModified back to false, for the next database if there is any
                            isModified = false;

                            //to dispose of

                            musicDatabase = null;
                            AllSongs = null;
                            artsToRemove.Clear();
                            artsToRemove = null;

                        }
                        // to dispose of
                        options = null;
                        settings = null;
                    }

                    //to Dispose of
                    existingDatabases.Clear();
                    existingDatabases = null;
                });

                //return InvalidsFoundAndRemoved;
            }

            /// <summary>
            /// Gets the previously prepared thumbnails of the audiofiles, and creates them again in case they don't exist
            /// </summary>
            /// <param name="musicData"></param>
            /// <returns></returns>
            public static Image[]? GetMusicThumbnails(string[][]? musicData)
            {

                Image[]? musicThumbnails = new Image[musicData.Length];

                Image artPic;
                Image thumbnail;

                for (int i = 0; i < musicData.Length; i++)
                {

                    if (System.IO.File.Exists(musicData[i][5]))
                    {
                        thumbnail = Image.FromFile(musicData[i][5]);
                        musicThumbnails[i] = (Image)thumbnail.Clone();
                        thumbnail = null;

                    }
                    // if the previously prepared art and thumbnail don't exist for any reason, checks the file for embedded art, and if there still isn't any
                    // cover art, uses the default image of the application
                    else
                    {

                        Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                        TagLib.File musicTags = TagLib.File.Create(musicData[i][3]);
                        var tempPics = musicTags.Tag.Pictures;
                        Image? art;
                        MemoryStream picConverter;

                        //checking the file for embedded Art
                        if (tempPics.Length > 0)
                        {
                            picConverter = new MemoryStream(tempPics[0].Data.Data);
                            art = (Image)Image.FromStream(picConverter).Clone();
                            thumbnail = art.GetThumbnailImage(60, 60, myCallback, 0);

                            using (FileStream artSaver = new FileStream(musicData[i][4], FileMode.Create))
                            {
                                art.Save(artSaver, art.RawFormat);
                            }
                            picConverter.Dispose();

                            //for thumbnail
                            using (FileStream thumbnailSaver = new FileStream(musicData[i][5], FileMode.Create))
                            {
                                thumbnail.Save(thumbnailSaver, art.RawFormat);
                            }
                            art = null;
                            thumbnail = null;
                        }
                        else
                        {
                            art = Khi_Player.Properties.Resources.Khi_Player;
                            thumbnail = art.GetThumbnailImage(60, 60, myCallback, 0);
                            using (FileStream artSaver = new FileStream(musicData[i][4], FileMode.Create))
                            {
                                art.Save(artSaver, art.RawFormat);
                            }

                            //for thumbnail
                            using (FileStream thumbnailSaver = new FileStream(musicData[i][5], FileMode.Create))
                            {
                                thumbnail.Save(thumbnailSaver, art.RawFormat);
                            }
                            art = null;
                            thumbnail = null;
                        }


                        art = null;
                        musicTags.Dispose();
                        tempPics = null;
                        thumbnail = null;
 
                        thumbnail = Image.FromFile(musicData[i][5]);
                        musicThumbnails[i] = (Image)thumbnail.Clone();

                        thumbnail = null;
                    }

                }


                return musicThumbnails;
            }

        }
         
        
        /// <summary>
        /// for now useless. was used to get the audio info from the listview items instead of 
        /// keeping all of the info in memory after the first time the data base was read. it's
        /// not updated and maintained, though might use it again in the future, hence why it still exists
        /// </summary>
        public class KhiConverter : IDisposable
        {
            private bool disposed;
            public string[][]? AllInfo { get; set; }
            public string[]? artsPaths { get; set; }

            /// <summary>
            /// extracts a ListView control's Items and their Images
            /// </summary>
            public KhiConverter(ListView listView)
            {
                string[][]? tempAllmusic = new string[listView.Items.Count][];
                int i = 0;
                if (listView.Items.Count > 1)
                {

                    foreach (var musicItem in listView.Items)
                    {
                        string[] tempMusic = new string[4];
                        //Image? tempArt;

                        tempMusic[0] = listView.Items[i].SubItems[0].Text;
                        tempMusic[1] = listView.Items[i].SubItems[1].Text;
                        tempMusic[2] = listView.Items[i].SubItems[2].Text;
                        tempMusic[3] = listView.Items[i].SubItems[3].Text;


                        tempAllmusic[i] = tempMusic;

                        i++;
                    }

                    //to dispose
                    //tempMusic = null;
                    //tempArt = null;

                }

                AllInfo = (string[][]?)tempAllmusic.Clone();
                artsPaths = new string[allMusicInfo.Length];
                i = 0;

                //to dispose
                tempAllmusic = null;
            }

            /// <summary>
            /// Destructor
            /// </summary>
            ~KhiConverter()
            {
                this.Dispose(false);
            }

            /// <summary>
            /// The dispose method that implements IDisposable.
            /// </summary>
            public void Dispose()
            {
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
                        AllInfo = null;
                        //AllMusicArts = null;
                        artsPaths = null;
                    }
                }

                disposed = true;
            }

        }

        /// <summary>
        /// Various Functions regarding the playlists, including getting the current playlist and sorting.
        /// will later add other functions
        /// </summary>
        public class PlayList
        {

            /// <summary>
            /// returns the mentioned playlist array or null if the playlist doesn't have a database for any reason.
            /// in case the chosen playlist is a dynamically added playlist, then the ictionary key which is the playlist's name is required
            /// </summary>
            /// <param name="playlist"></param>
            /// <param name="playlistDictionaryKey"></param>
            /// <returns></returns>
            public static string[][]? GetPlaylist(Playlists? playlist, string playlistDictionaryKey = null)
            {
                ref string[][]? chosenPlaylist = ref allMusicInfo;
                if (playlist == Playlists.allSongs)
                { return chosenPlaylist; }
                else if (playlist == Playlists.searchPlaylist)
                {
                    chosenPlaylist = ref searchPlaylist;
                    return chosenPlaylist;
                }

                else if (playlist == Playlists.DynamicPlaylists && playlistDictionaryKey != null)
                {
                    string? name = playlistDictionaryKey;
                    string[][]? playlistInfo = PlaylistsDict.GetValueOrDefault(name);
                    return playlistInfo;

                }
                else if (playlist == null && playlistDictionaryKey != null)
                {
                    string? name = playlistDictionaryKey;
                    string[][]? playlistInfo = PlaylistsDict.GetValueOrDefault(name);
                    return playlistInfo;
                }
                else
                {
                    return null;
                }

            }

            /// <summary>
            /// returns the currently in use playlist
            /// </summary>
            /// <returns></returns>
            public static string[][]? GetCurrentPlaylist(string playlistDictionaryKey = null)
            {
                if (CurrentPlaylist == Playlists.allSongs)
                {
                    ref string[][]? currentPlaylist = ref allMusicInfo;
                    return currentPlaylist;
                }
                else if (CurrentPlaylist == Playlists.searchPlaylist)
                {
                    ref string[][]? currentPlaylist = ref searchPlaylist;
                    return currentPlaylist;
                }
                else if (CurrentPlaylist == Playlists.DynamicPlaylists)
                {
                    string[][]? currentPlaylist = GetPlaylist(CurrentPlaylist, CurrentPlaylistName);
                    return currentPlaylist;
                }
                else
                { return null; }

            }

            /// <summary>
            /// sorts the mentioned playlist based on the determined column
            /// </summary>
            /// <param name="playlist"></param>
            /// <param name="columnNumber"></param>
            public static string[][]? SortPlaylist(Playlists playlist, int columnNumber)
            {
                string[][]? toBeSortedPlaylist;
                if (playlist == Playlists.allSongs)
                {
                    toBeSortedPlaylist = allMusicInfo;
                }
                else if (playlist == Playlists.searchPlaylist)
                {
                    toBeSortedPlaylist = searchPlaylist;
                }
                else
                {
                    toBeSortedPlaylist = GetPlaylist(playlist);
                }
                Array.Sort(toBeSortedPlaylist, (x, y) => x[columnNumber].CompareTo(y[columnNumber]));
                return toBeSortedPlaylist;

            }

            /// <summary>
            /// sorts the passed playlist based on the determined column
            /// </summary>
            /// <param name="playlist"></param>
            /// <param name="columnNumber"></param>
            public static string[][]? SortPlaylist(string[][]? playlistInfo, int columnNumber)
            {
                string[][]? toBeSortedPlaylist;

                Array.Sort(playlistInfo, (x, y) => x[columnNumber].CompareTo(y[columnNumber]));
                return playlistInfo;

            }

        }

        ///<summary> 
        /// For playing and Controlling sound files
        /// </summary>
        public class PlayBackFunction
        {
            private bool disposed;
            public enum CurrentlyPlayingSong;
            public enum States : int { Setup = 0, Playing = 1, Paused = 2, Stopped = 3, Finished = 4 };
            public static States Status = States.Setup;

            public enum Controls { PlayPause, Skip, Previous, Stop };


            public static string[][]? PlaylistQueue;
            public static int? playingSongIndex;
            public static int[]? shuffledIndices; // if shuffle in enabled, this will be shuffled, otherwise, it's just a list of indices
            public static Dictionary<int, int> originalAndShuffledIndices = new Dictionary<int, int>();
            public static List<int> playedSongs = new List<int>();
            public static AudioFileReader song;
            public static WaveOutEvent mediaPlayer = new WaveOutEvent();
            static Random random = new Random();


            public PlayBackFunction()
            {
                PlaylistQueue = PlayList.GetCurrentPlaylist(CurrentPlaylistName);
            }


            /// <summary>
            /// Destructor
            /// </summary>
            ~PlayBackFunction()
            {
                this.Dispose(false);
            }

            /// <summary>
            /// The dispose method that implements IDisposable.
            /// </summary>
            public void Dispose()
            {
                this.Dispose(true);
                originalAndShuffledIndices = null;
                PlaylistQueue = null;
                shuffledIndices = null;

                mediaPlayer.Dispose();
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
                        originalAndShuffledIndices = null;
                        PlaylistQueue = null;
                        shuffledIndices = null;
                        mediaPlayer.Dispose();
                    }

                }

                disposed = true;
            }

            /// <summary>
            /// shuffles the current playlist and orginizes the old song indices and the new ones
            /// </summary>
            private static void ShufflePlaylist ()
            {
                originalAndShuffledIndices.Clear();


                int[]? randomIndex = new int[PlaylistQueue.Length];


                for (int i = 0; i < PlaylistQueue.Length; i++)
                {
                    randomIndex[i] = i;
                }
                random.Shuffle(randomIndex); // will do it twice
                random.Shuffle(randomIndex);

                shuffledIndices = (int[]?)randomIndex.Clone();


                bool selectedItemFound = false;

                int shuffleCounter = 0;
                foreach (int i in randomIndex)
                {
                    originalAndShuffledIndices.Add(shuffleCounter, i);
                    shuffleCounter++;
                }

                isShuffled = true;

                //for disposal
                randomIndex = null;
                //tempList = null;
            }

            /// <summary>
            /// To Play, Puase, Stop, Skip and go back (Previous) the audio files that populate the musicListView
            /// </summary>
            /// <param name="PressedControl"></param>
            public static void MusicPlayBackControl(Controls PressedControl)
            {
                bool playingSelectedSong = false;

                if (listUpdated == true)
                {
                    PlaylistQueue = PlayList.GetCurrentPlaylist(CurrentPlaylistName);
                    isShuffled = false;

                    listUpdated = false;
                }
                if (isShuffleEnabled == true)
                {
                    if (isShuffled == false)
                    {
                        ShufflePlaylist();

                    }
                    else
                    {

                    }
                }

                int playStopNumberCount = 0;

                switch (PressedControl)
                {
                    case Controls.PlayPause:

                        if (mediaPlayer.PlaybackState == PlaybackState.Playing) // will pause the playback if music is being played
                        {
                            mediaPlayer.Pause();
                            Status = States.Paused;

                            if (currentlySelectedSongIndex != null && currentlyPlayingSongInfo != PlaylistQueue[currentlySelectedSongIndex])
                            {

                                song.Dispose();
                                mediaPlayer.Dispose();
                                currentlyPlayingSongInfo = null;
                                currentlyPlayingSongPic = null;
                                selectedMusicsQue = (uint)currentlySelectedSongIndex;

                                song = new AudioFileReader(PlaylistQueue[currentlySelectedSongIndex][3]);

                                mediaPlayer = new WaveOutEvent();

                                mediaPlayer.DesiredLatency = 100;
                                mediaPlayer.Init(song);
                                mediaPlayer.Play();
                                Status = States.Playing;
                                playingSelectedSong = true;
                            }

                        }

                        else // will play the selected song (or the entire playlistQue) or resume the paused playback
                        {
                            if (mediaPlayer.PlaybackState == PlaybackState.Paused) // will resume playback if it is paused or play the recently selected song
                            {
                                if (currentlyPlayingSongInfo != PlaylistQueue[currentlySelectedSongIndex]) // if playback is paused AND a new song is selected will play the new song instead of resuming
                                {
                                    song.Dispose();
                                    mediaPlayer.Dispose();
                                    currentlyPlayingSongInfo = null;
                                    currentlyPlayingSongPic = null;

                                    selectedMusicsQue = (uint)currentlySelectedSongIndex;
                                    song = new AudioFileReader(PlaylistQueue[currentlySelectedSongIndex][3]);

                                    mediaPlayer = new WaveOutEvent();

                                    mediaPlayer.DesiredLatency = 100;
                                    mediaPlayer.Init(song);
                                    mediaPlayer.Play();
                                    Status = States.Playing;
                                    playingSelectedSong = true;
                                }
                                else // to resume playback of paused
                                {
                                    mediaPlayer.Play();
                                    Status = States.Playing;
                                }
                            }
                            else // plays the recently selected song
                            {
                                selectedMusicsQue = (uint)currentlySelectedSongIndex;

                                song = new AudioFileReader(PlaylistQueue[currentlySelectedSongIndex][3]);
                                mediaPlayer = new WaveOutEvent();

                                mediaPlayer.DesiredLatency = 100;
                                mediaPlayer.Init(song);
                                mediaPlayer.Play();
                                Status = States.Playing;
                                playingSelectedSong = true;
                            }

                            playStopNumberCount = 0;

                        }
                        break;

                    case Controls.Skip: //skips to the next song in the playlistQue, the pecifics depends on the current state of loop and shuffle

                        string? dupliPlayCheck;
                        if (song != null)
                        {
                            dupliPlayCheck = song.FileName;
                            song.Dispose();
                            mediaPlayer.Dispose();
                        }
                        else
                        { dupliPlayCheck = null; }

                        currentlyPlayingSongInfo = null;
                        currentlyPlayingSongPic = null;

                        if (isLoopEnabled == true) // if loop is enabled 
                        {
                            if (LoopState == LoopStates.SingleSongLoop) //if loop is enabled and set to single loop, will not skip to next song
                            {

                            }
                            else //if loop is enabled and set to playlist Loop, skips to the next song in the playlist and incase the playlist has ended, reshuffles the playlistQue
                                 //and begins playing from the beginning of the playlist
                            {
                                selectedMusicsQue++;
                                if (selectedMusicsQue > (PlaylistQueue.Length - 1))
                                { 
                                    selectedMusicsQue = 0;
                                    if (isShuffleEnabled)
                                    {
                                        ShufflePlaylist();
                                    }
                                
                                }
                            }

                            if (isShuffleEnabled == true) // if shuffle is enabled
                            {
                                if (dupliPlayCheck != null && PlaylistQueue[shuffledIndices[selectedMusicsQue]][3] == dupliPlayCheck) // if shuffle is enabled and the next song is the same as
                                                                                                                                      // the previous song (in case shuffle was enabled after playback
                                                                                                                                      // had already started) will skip once more
                                { selectedMusicsQue++; }

                                song = new AudioFileReader(PlaylistQueue[shuffledIndices[selectedMusicsQue]][3]);
                            }
                            else // if shuffle is not enabled, will simply start the new song
                            {
                                song = new AudioFileReader(PlaylistQueue[selectedMusicsQue][3]);
                            }

                            mediaPlayer = new WaveOutEvent();

                            mediaPlayer.DesiredLatency = 100;
                            mediaPlayer.Init(song);
                            mediaPlayer.Play();
                            Status = States.Playing;
                        }
                        else // if loop is not enabled skips to the next song. in case the currently playing song was the last in que, will
                             // declare the end of playlist.
                        {
                            selectedMusicsQue++;
                            if (selectedMusicsQue > PlaylistQueue.Length - 1)
                            {
                                System.Windows.Forms.MessageBox.Show("End of Playlist Reached \r\n Enable Loop for unintrupted playback");
                                Status = States.Finished;
                                selectedMusicsQue = 0;
                            }
                            else
                            {

                                if (isShuffleEnabled == true)
                                {
                                    song = new AudioFileReader(PlaylistQueue[shuffledIndices[selectedMusicsQue]][3]);
                                }
                                else
                                {
                                    song = new AudioFileReader(PlaylistQueue[selectedMusicsQue][3]);
                                }

                                mediaPlayer = new WaveOutEvent();

                                mediaPlayer.DesiredLatency = 100;
                                mediaPlayer.Init(song);
                                mediaPlayer.Play();
                                Status = States.Playing;

                            }
                        }

                        break;

                    case Controls.Previous: //functions the same way as Skip but just .. well goes to the previous song instead
                        string? duplPlayCheck;

                        if (song != null)
                        {
                            duplPlayCheck = song.FileName;
                            song.Dispose();
                            mediaPlayer.Dispose();
                        }
                        else
                        { duplPlayCheck = null; }
                        currentlyPlayingSongInfo = null;
                        currentlyPlayingSongPic = null;

                        if (isLoopEnabled == true && selectedMusicsQue == 0)
                        {
                            selectedMusicsQue = (uint)PlaylistQueue.Length - 1;

                            if (isShuffleEnabled == true)
                            {
                                if (duplPlayCheck != null && PlaylistQueue[shuffledIndices[selectedMusicsQue]][3] == duplPlayCheck)
                                { selectedMusicsQue--; }

                                song = new AudioFileReader(PlaylistQueue[shuffledIndices[selectedMusicsQue]][3]);
                            }
                            else
                            {
                                song = new AudioFileReader(PlaylistQueue[selectedMusicsQue][3]);
                            }

                            mediaPlayer = new WaveOutEvent();

                            mediaPlayer.DesiredLatency = 100;
                            mediaPlayer.Init(song);
                            mediaPlayer.Play();
                            Status = States.Playing;
                        }
                        else
                        {
                            if (selectedMusicsQue == 0)
                            {
                                System.Windows.Forms.MessageBox.Show("End of Playlist Reached \r\n Enable Loop for unintrupted playback");
                                Status = States.Finished;
                                selectedMusicsQue = 0;
                            }
                            else
                            {
                                selectedMusicsQue--;

                                if (isShuffleEnabled == true)
                                {
                                    if (duplPlayCheck != null && PlaylistQueue[shuffledIndices[selectedMusicsQue]][3] == duplPlayCheck)
                                    { selectedMusicsQue--; }
                                    song = new AudioFileReader(PlaylistQueue[shuffledIndices[selectedMusicsQue]][3]);
                                }
                                else
                                {
                                    song = new AudioFileReader(PlaylistQueue[selectedMusicsQue][3]);
                                }
                                mediaPlayer = new WaveOutEvent();

                                mediaPlayer.DesiredLatency = 100;
                                mediaPlayer.Init(song);
                                mediaPlayer.Play();
                                Status = States.Playing;

                            }

                        }
                        break;

                    case Controls.Stop: // stops playback if music was playing or was paused
                        if (Status == States.Playing || Status == States.Paused)
                        {
                            mediaPlayer.Stop();
                            Status = States.Stopped;
                        }
                        break;

                }
                
                //After a music is played, skiped, etc. will make set the value of of the currently playing song info and cover art
                //so it can be used by other functions. The condition for this behavior is set to playing to avoid setting the value
                //again if a song that was paused is unpaused

                if (Status == States.Playing)
                {
                    if (playingSelectedSong == true)
                    {
                        currentlyPlayingSongInfo = PlaylistQueue[selectedMusicsQue];
                        currentlyPlayingSongPic = Image.FromFile(PlaylistQueue[selectedMusicsQue][4]);
                    }
                    else if (isShuffleEnabled)
                    {
                        currentlyPlayingSongInfo = PlaylistQueue[originalAndShuffledIndices.GetValueOrDefault((int)selectedMusicsQue)];
                        currentlyPlayingSongPic = Image.FromFile(PlaylistQueue[originalAndShuffledIndices.GetValueOrDefault((int)selectedMusicsQue)][4]);
                    }
                    else
                    {
                        currentlyPlayingSongInfo = PlaylistQueue[selectedMusicsQue];
                        currentlyPlayingSongPic = Image.FromFile(PlaylistQueue[selectedMusicsQue][4]);
                    }

                }


            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Dark Mode
            if (Settings1.Default.DarkMode == true)
            {

                FormEditor.DarkMode(true);
                isDarkMode = true;

            }
            else
            {
                FormEditor.DarkMode(false);
                isDarkMode = false;
            }

            CurrentPlaylist = Playlists.allSongs;

            //CurrentPlaylist = Playlists.allSongs;
            if (Settings1.Default.SortOrder != 0)
            {
                int order = Settings1.Default.SortOrder;
                SortOrder = (SortOrders)order;
                int sortColumn;

                switch (order)
                {
                    case 1:
                        //this is sort based on title
                        sortColumn = 0;
                        break;
                    case 2:
                        //this is sort based on artist
                        sortColumn = 1;
                        break;
                    case 3:
                        //this is sort based on album
                        sortColumn = 2;
                        break;

                    default:
                        //this is sort based on title
                        sortColumn = 0;
                        break;
                }

                var playlist = PlayList.GetCurrentPlaylist();
                if (playlist != null && playlist[0] != null)
                {
                    List<string[]?> tempList = new List<string[]?>();
                    PlayList.SortPlaylist(CurrentPlaylist, sortColumn);
                    playlist = PlayList.GetCurrentPlaylist();
                    if (CurrentPlaylist == Playlists.allSongs) { allMusicInfo = (string[][])playlist.Clone(); }
                    musicListView.Items.Clear();
                    musicListView.LargeImageList.Images.Clear();
                    Image[]? tempAllArts = AudioDataBase.GetMusicThumbnails(playlist);

                    FormEditor.PopulateListView(ref musicListView, ref playlist, ref tempAllArts);
                    listUpdated = true;

                }


            }
            GC.Collect();

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

                    if (SortOrder != SortOrders.CustomSort)
                    {
                        int order = (int)SortOrder;
                        int sortColumn;

                        switch (order)
                        {
                            case 1:
                                //this is sort based on title
                                sortColumn = 0;
                                break;
                            case 2:
                                //this is sort based on artist
                                sortColumn = 1;
                                break;
                            case 3:
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
                    //PlaylistMusicInfo = 
                    musicListView.Items.Clear();
                    musicListView.LargeImageList.Images.Clear();
                    FormEditor.PopulateListView(ref musicListView, ref newPlaylistInfo, ref playlistImages );

                    searchMusicListView.AutoCompleteCustomSource.Clear();

                    FormEditor.SearchBarAutoCompleteSource(newPlaylistInfo);

                    listUpdated = true;

                    //for disposal
                    newPlaylistInfo = null;
                    playlistImages = null;
                    playlistPath = null;
                    name = null;
                }
                GC.Collect();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please Add songs to this playlist");
            }

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
                tempPlaylistItems = null;
                PlaylistName = null;
                playlistPath = null;
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
                        if (renameTextBox.Text.Contains(unacceptableChar))
                        {
                            isAcceptable = false;
                        }

                    }
                    if (System.IO.File.Exists(applicationPath + renameTextBox.Text + ".xml"))
                    {
                        alreadyExists = true;
                    }

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
            (dataBaseInfo, Arts) = FilterDuplicates.TryRepairingDataBase(musicListView);
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

                    song = null;
                    x++;
                }

                KhiPlayer = new PlayBackFunction();

                allMusicArts = null;
                dataBaseInfo = null;
                //artPaths = null;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please add music ");
            }
            musicListView.EndUpdate();
            GC.Collect();
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
                AudioDataBase khiDatabase = new AudioDataBase(draggedFiles);
                if (khiDatabase.AddedMusicInfo.Length > 0)
                {

                    var AddedMusicInfo = (string[][]?)khiDatabase.AddedMusicInfo.Clone();
                    var AddedMusicArts = (Image[]?)khiDatabase.AddedMusicArts.Clone();

                    if (musicListView.Items.Count == 0)
                    {
                        allMusicInfo = (string[][])AddedMusicInfo.Clone();
                    }
                    else
                    {
                        allMusicInfo = (string[][])khiDatabase.AllMusicInfo.Clone();
                    }

                    khiDatabase.Dispose();
                    dragDropSuccess = true;

                    //for disposal
                    AddedMusicArts = null;
                    AddedMusicInfo = null;
                    draggedFiles = null;
                    tempPathList.Clear();
                    tempPathList = null;
                    tempDraggedFiles = null;
                }
            });

            FormEditor.PopulateListView(ref musicListView, ref allMusicInfo, ref allMusicArts, false);
            searchMusicListView.AutoCompleteCustomSource.Clear();
            FormEditor.SearchBarAutoCompleteSource(allMusicInfo);

            listUpdated = true;
            allMusicArts = null;


            GC.Collect();
            MemoryManageTimer.Start();
        }

        private void musicListView_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {

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

        private void PlayPause_Click(object sender, EventArgs e)
        {
            PlayBackFunction.MusicPlayBackControl(PlayBackFunction.Controls.PlayPause);
            seekBar.Enabled = true;
            if (PlayBackFunction.song.TotalTime.Hours > 0)
            { songLengthLabel.Text = PlayBackFunction.song.TotalTime.ToString("hh\\:mm\\:ss"); }
            else
            { songLengthLabel.Text = PlayBackFunction.song.TotalTime.ToString("mm\\:ss"); }

            seekBar.Maximum = Convert.ToInt32(PlayBackFunction.song.TotalTime.TotalSeconds);
            songSeekTimer.Enabled = true;

            borderLabel.Visible = true;
            songTitleLabel.Text = currentlyPlayingSongInfo[0];
            songArtistLabel.Text = currentlyPlayingSongInfo[1];
            songAlbumLabel.Text = currentlyPlayingSongInfo[2];

            lyricsTextBox.Clear();
            string? lyrics = "Oops! No Embedded Lyrics";
            try
            {
                using (TagLib.File lyrictag = TagLib.File.Create(currentlyPlayingSongInfo[3]))
                {
                    lyrics = lyrictag.Tag.Lyrics;
                    if (lyrics != null)
                    { lyrics = lyrics.ReplaceLineEndings(); }
                    else { lyrics = "Oops! No Embedded Lyrics"; }
                }
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("File Error, please remove this file from the application");
                //throw;
            }

            lyricsTextBox.Text = lyrics;

            if (currentlyPlayingSongPic != null)
            {
                pictureBox1.Image = null;
                pictureBox1.Image = currentlyPlayingSongPic;
            }
            else
            {
                pictureBox1.Image = Properties.Resources.Khi_Player;
            }

            //testList.Clear();
            GC.Collect();

        }

        private void skip_Click(object sender, EventArgs e)
        {
            songSeekTimer.Enabled = false;
            PlayBackFunction.MusicPlayBackControl(PlayBackFunction.Controls.Skip);
            if (seekBar.Enabled == false) { seekBar.Enabled = true; }
            if (PlayBackFunction.song.TotalTime.Hours > 0)
            { songLengthLabel.Text = PlayBackFunction.song.TotalTime.ToString("hh\\:mm\\:ss"); }
            else
            { songLengthLabel.Text = PlayBackFunction.song.TotalTime.ToString("mm\\:ss"); }

            seekBar.Maximum = Convert.ToInt32(PlayBackFunction.song.TotalTime.TotalSeconds);
            songSeekTimer.Enabled = true;

            if (PlayBackFunction.mediaPlayer.PlaybackState == PlaybackState.Playing || PlayBackFunction.mediaPlayer.PlaybackState == PlaybackState.Paused)
            {
                songTitleLabel.Text = currentlyPlayingSongInfo[0];
                songArtistLabel.Text = currentlyPlayingSongInfo[1];
                songAlbumLabel.Text = currentlyPlayingSongInfo[2];

                lyricsTextBox.Clear();
                string? lyrics = "Oops! No Embedded Lyrics";
                try
                {
                    using (TagLib.File lyrictag = TagLib.File.Create(currentlyPlayingSongInfo[3]))
                    {
                        lyrics = lyrictag.Tag.Lyrics;
                        if (lyrics != null)
                        { lyrics = lyrics.ReplaceLineEndings(); }
                        else { lyrics = "Oops! No Embedded Lyrics"; }
                    }
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("File Error, please remove this file from the application");
                    //throw;
                }
                lyricsTextBox.Text = lyrics;

                if (currentlyPlayingSongPic != null)
                {
                    pictureBox1.Image = null;
                    pictureBox1.Image = currentlyPlayingSongPic;
                }
                else
                {
                    pictureBox1.Image = Properties.Resources.Khi_Player;
                }
            }
            GC.Collect();
        }

        private void previous_Click(object sender, EventArgs e)
        {
            songSeekTimer.Enabled = false;
            PlayBackFunction.MusicPlayBackControl(PlayBackFunction.Controls.Previous);

            if (PlayBackFunction.song.TotalTime.Hours > 0)
            { songLengthLabel.Text = PlayBackFunction.song.TotalTime.ToString("hh\\:mm\\:ss"); }
            else
            { songLengthLabel.Text = PlayBackFunction.song.TotalTime.ToString("mm\\:ss"); }

            seekBar.Maximum = Convert.ToInt32(PlayBackFunction.song.TotalTime.TotalSeconds);
            songSeekTimer.Enabled = true;

            if (PlayBackFunction.mediaPlayer.PlaybackState == PlaybackState.Playing || PlayBackFunction.mediaPlayer.PlaybackState == PlaybackState.Paused)
            {
                songTitleLabel.Text = currentlyPlayingSongInfo[0];
                songArtistLabel.Text = currentlyPlayingSongInfo[1];
                songAlbumLabel.Text = currentlyPlayingSongInfo[2];

                lyricsTextBox.Clear();
                string? lyrics = "Oops! No Embedded Lyrics";
                try
                {
                    using (TagLib.File lyrictag = TagLib.File.Create(currentlyPlayingSongInfo[3]))
                    {
                        lyrics = lyrictag.Tag.Lyrics;
                        if (lyrics != null)
                        { lyrics = lyrics.ReplaceLineEndings(); }
                        else { lyrics = "Oops! No Embedded Lyrics"; }
                    }
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("File Error, please remove this file from the application");
                    //throw;
                }
                lyricsTextBox.Text = lyrics;

                if (currentlyPlayingSongPic != null)
                {
                    pictureBox1.Image = null;
                    pictureBox1.Image = currentlyPlayingSongPic;
                }
                else
                {
                    pictureBox1.Image = Properties.Resources.Khi_Player;
                }
            }

            GC.Collect();

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

            //for disposal
            allInfo = null;

        }



        private void addMusicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog musicBrowser = new System.Windows.Forms.OpenFileDialog();
            musicBrowser.Filter = "Audio Files | *.mp3; *.wav; *.flac; *.aiff; *.wma; *.pcm; *.aac; *.oog; *.alac";
            musicBrowser.Multiselect = true;

            if (musicBrowser.ShowDialog() == DialogResult.OK)
            {
                string[] tempAddedFilesNames = musicBrowser.FileNames;

                int index = musicBrowser.FileNames.Length;
                string[] addedFiles = new string[index];
                addedFiles = tempAddedFilesNames;
                AudioDataBase khiDatabase = new AudioDataBase(addedFiles);
                if (khiDatabase.AddedMusicInfo.Length > 0)
                {
                    //allArtFilePaths = (string[]?)khiDatabase.ArtFileNames.Clone();
                    var AddedMusicInfo = (string[][]?)khiDatabase.AddedMusicInfo.Clone();
                    var AddedMusicArts = (Image[]?)khiDatabase.AddedMusicArts.Clone();
                    //var AddedArtFilePaths = (string[]?)khiDatabase.AddedArtFileNames.Clone();
                    if (musicListView.Items.Count == 0)
                    {
                        allMusicInfo = (string[][])AddedMusicInfo.Clone();
                    }
                    else
                    {
                        allMusicInfo = (string[][])khiDatabase.AllMusicInfo.Clone();
                    }
                    khiDatabase.Dispose();
                    //var tempArts = khiDatabase.AllMusicArts;
                    FormEditor.PopulateListView(ref musicListView, ref AddedMusicInfo, ref AddedMusicArts, false);

                    foreach (string[]? music in AddedMusicInfo)
                    {
                        searchMusicListView.AutoCompleteCustomSource.Add(music[0]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[1]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[2]);
                    }

                    listUpdated = true;
                    //to dispose
                    allMusicArts = null;
                    addedFiles = null;
                    GC.Collect();

                }
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

                    //for disposal
                    playlist = null;
                    clickedItem = null;
                }
            }
            else
            {
                var clickedItem = musicListView.GetItemAt(e.X, e.Y);
                if (rightClickMenu.IsDisposed && !rightClickMenu.Enabled)
                {
                    if (clickedItem == null)
                    {
                        if (PlayBackFunction.mediaPlayer.PlaybackState != PlaybackState.Playing || PlayBackFunction.mediaPlayer.PlaybackState != PlaybackState.Paused)
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
            
            GC.Collect();
            bool wasPlaying = false;
            bool removeFromAllPlaylists;

            int i = musicListView.Items.Find(currentlySelectedSong[3], false)[0].Index;
            var item = (string[])currentlySelectedSong.Clone();
            if (currentlyPlayingSongInfo == currentlySelectedSong)
            {
                wasPlaying = true;
                PlayBackFunction.mediaPlayer.Stop();
                PlayBackFunction.song.Dispose();
                currentlyPlayingSongPic = null;

            }

            musicListView.BeginUpdate();
            musicListView.Items.RemoveAt(i);
            //musicListView.LargeImageList.Images.RemoveAt(i);

            string[][]? playlist;

            await Task.Run(() =>
            {
                if (CurrentPlaylist == Playlists.allSongs)
                {
                    playlist = PlayList.GetCurrentPlaylist();
                    removeFromAllPlaylists = true;
                }
                else if (CurrentPlaylist == Playlists.DynamicPlaylists)
                {
                    playlist = PlayList.GetPlaylist(null, CurrentPlaylistName);
                    removeFromAllPlaylists = false;
                }
                else
                {
                    if (PlaylistsDict.ContainsKey(CurrentPlaylistName))
                    {
                        playlist = PlayList.GetPlaylist(null, CurrentPlaylistName);
                        removeFromAllPlaylists = false;
                    }
                    else
                    {
                        playlist = PlayList.GetCurrentPlaylist();
                        removeFromAllPlaylists = true;
                    }
                }

                bool removed = AudioDataBase.RemoveSong(item, playlist[i][4], playlist[i][5], removeFromAllPlaylists, CurrentPlaylistName);

                var tempPlaylist = playlist.ToList();
                tempPlaylist.RemoveAt(i);
                playlist = tempPlaylist.ToArray();


                if (CurrentPlaylist == Playlists.allSongs)
                {
                    allMusicInfo = (string[][])playlist.Clone();
                }
                else if (CurrentPlaylist == Playlists.DynamicPlaylists)
                {
                    if (PlaylistsDict.ContainsKey(CurrentPlaylistName))
                    {
                        PlaylistsDict[CurrentPlaylistName] = (string[][])playlist.Clone();
                    }
                }
                else //if it's search playlist, it can be any playlist so will have to check
                {
                    if (PlaylistsDict.ContainsKey(CurrentPlaylistName))
                    {
                        PlaylistsDict[CurrentPlaylistName] = (string[][])playlist.Clone();
                    }
                    else //if it's not in the dynamic playlists then it can only be all songs playlist
                    {
                        allMusicInfo = (string[][])playlist.Clone();
                    }
                }

                //for disposal
                tempPlaylist.Clear();
                tempPlaylist = null;
            });

            musicListView.EndUpdate();
            listUpdated = true;

            if (wasPlaying) 
            {
                currentlySelectedSong[0] = musicListView.Items[i].SubItems[0].Text;
                currentlySelectedSong[1] = musicListView.Items[i].SubItems[1].Text;
                currentlySelectedSong[2] = musicListView.Items[i].SubItems[2].Text;
                currentlySelectedSong[3] = musicListView.Items[i].SubItems[3].Text;
                PlayPause_Click(sender, e);
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

        private void musicListView_MouseMove(object sender, MouseEventArgs e)
        {/*
            ListViewItem item = musicListView.GetItemAt(e.X, e.Y);
            if (item != null && item.Tag == null)
            {
                musicListView.Invalidate(item.Bounds);
                item.Tag = "tagged";
            }
            */
            //songToolTip.SetToolTip () = musicListView.GetItemAt(e.X, e.Y).ToolTipText;
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
            if (PlayBackFunction.Status == PlayBackFunction.States.Playing || PlayBackFunction.Status == PlayBackFunction.States.Paused)
            {
                if (PlayBackFunction.song.CurrentTime.TotalSeconds < PlayBackFunction.song.TotalTime.TotalSeconds)
                {
                    var timeValue = (int)Math.Round(PlayBackFunction.song.CurrentTime.TotalSeconds, MidpointRounding.AwayFromZero);
                    currenSongTimePosition = PlayBackFunction.song.CurrentTime.ToString("mm\\:ss");
                    currentTimeLabel.Text = currenSongTimePosition;
                    seekBar.Value = timeValue;
                    lastSongTimePosition = timeValue;

                }
                else
                {
                    songSeekTimer.Enabled = false;
                    skip_Click(sender, e);
                    songSeekTimer.Enabled = true;

                }
            }
            else
            {

                currenSongTimePosition = new TimeSpan(0, 00, 00).ToString("mm\\:ss");
                seekBar.Value = 0;
                songSeekTimer.Enabled = false;
                currentTimeLabel.Text = "00:00";
                songLengthLabel.Text = "00:00";
            }

        }

        private void seekBar_MouseDown(object sender, MouseEventArgs e)
        {
            seekBarValueBeforeMove = seekBar.Value;
            songSeekTimer.Enabled = false;

        }

        private void seekBar_MouseUp(object sender, MouseEventArgs e)
        {
            int clickedValue = (int)((((double)e.X) / (seekBar.Size.Width - 2)) * (seekBar.Maximum - seekBar.Minimum));
            seekBar.Value = clickedValue;
            PlayBackFunction.song.Skip(clickedValue - seekBarValueBeforeMove);
            songSeekTimer.Enabled = true;
        }

        private void volumeBar_Scroll(object sender, EventArgs e)
        {
            PlayBackFunction.mediaPlayer.Volume = ((float)volumeBar.Value) / 100;
            volumeLabel.Text = volumeBar.Value.ToString();
        }

        private void MemoryManageTimer_Tick(object sender, EventArgs e)
        {
            if (AudioDataBase.isDataBaseRead)
            {
                GC.Collect();
            }
        }

        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFolderDialog musicBrowser = new OpenFolderDialog();
            musicBrowser.Multiselect = true;

            if (musicBrowser.ShowDialog() == true)
            {
                string[] tempAddedFolderNames = musicBrowser.FolderNames;
                List<string> tempPathsList = new List<string>();

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
                                if (tempPath == ".mp3" || tempPath == ".wav" || tempPath == ".flac" || tempPath == ".aiff" || tempPath == ".wma" || tempPath == ".pcm" || tempPath == ".aac" || tempPath == ".oog" || tempPath == ".alac")
                                { tempPathsList.Add(file); }

                            }
                        }
                    }
                }

                string[] addedFiles = tempPathsList.ToArray();
                AudioDataBase khiDatabase = new AudioDataBase(addedFiles);
                if (khiDatabase.AddedMusicInfo.Length > 0)
                {
                    var AddedMusicInfo = (string[][]?)khiDatabase.AddedMusicInfo.Clone();
                    var AddedMusicArts = (Image[]?)khiDatabase.AddedMusicArts.Clone();

                    if (musicListView.Items.Count == 0)
                    {
                        allMusicInfo = (string[][])AddedMusicInfo.Clone();
                    }
                    else
                    {
                        allMusicInfo = (string[][])khiDatabase.AllMusicInfo.Clone();
                    }

                    khiDatabase.Dispose();

                    FormEditor.PopulateListView(ref musicListView, ref AddedMusicInfo, ref AddedMusicArts, false);
                    FormEditor.SearchBarAutoCompleteSource(AddedMusicInfo);

                    listUpdated = true;

                    //to dispose
                    allMusicArts = null;
                    AddedMusicArts = null;
                    AddedMusicInfo = null;
                    musicBrowser = null;
                    tempAddedFolderNames = null;
                    tempPathsList.Clear();
                    tempPathsList = null;
                    addedFiles = null;

                    GC.Collect();

                }
            }
        }

        private void darkModeMenuItem_Click(object sender, EventArgs e)
        {
            //turns lightmode to dark mode
            if (!darkModeMenuItem.Checked)
            {
                FormEditor.DarkMode(true);
                isDarkMode = true;
            }
            //turns dark mode to light mode
            else
            {
                FormEditor.DarkMode(false);
                isDarkMode = false;
            }
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
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
                int size = 12;

                lyricsTextBox.Font = new Font("Segoe UI", size);
                songTitleLabel.Font = new Font("Segoe UI", size);
                songArtistLabel.Font = new Font("Segoe UI", size);
                songAlbumLabel.Font = new Font("Segoe UI", size);
                new Font("Segoe UI", size);
            }

        }


        private async void allSongsPlaylist_Click(object sender, EventArgs e)
        {

            CurrentPlaylist = Playlists.allSongs;
            currentPlaylistLabel.Text = "All Songs";
            allMusicInfo = ReadAudioDataBase("complete");

            if (SortOrder != SortOrders.CustomSort)
            {
                int order = (int)SortOrder;
                int sortColumn;

                switch (order)
                {
                    case 1:
                        //this is sort based on title
                        sortColumn = 0;
                        break;
                    case 2:
                        //this is sort based on artist
                        sortColumn = 1;
                        break;
                    case 3:
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

            FormEditor.PopulateListView(ref musicListView, ref allMusicInfo, ref arts);

            searchMusicListView.AutoCompleteCustomSource.Clear();
            FormEditor.SearchBarAutoCompleteSource(allMusicInfo);

            listUpdated = true;

            isShuffled = false;


            GC.Collect();

        }

        private void searchMusicListView_TextChanged(object sender, EventArgs e)
        {
            /*
            string? searchWord = searchMusicListView.Text;
            if (searchWord == "")
            {
                allMusicInfo = ReadAudioDataBase("complete");
                Image[]? arts = GetMusicThumbnails(allMusicInfo);
                musicListView.Items.Clear();
                musicListView.LargeImageList.Images.Clear();
                FormEditor.PopulateListView(ref musicListView, ref allMusicInfo, ref arts);
                musicListView.Focus();
                searchMusicListView.AutoCompleteCustomSource.Clear();

                FormEditor.SearchBarAutoCompleteSource(allMusicInfo);
                listUpdated = true;
                CurrentPlaylist = Playlists.allSongs;
                GC.Collect();
            }
            */
        }

        private void searchMusicListView_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void searchMusicListView_KeyDown(object sender, KeyEventArgs e)
        {
            string? searchWord = searchMusicListView.Text;

            if (e.KeyValue == (int)Keys.Enter || e.KeyValue == (int)Keys.Tab)
            {
                //musicListView.Focus();
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
                        FormEditor.PopulateListView(ref musicListView, ref foundItemsArray, ref foundItemsThumbnailsArray);

                        
                        //for disposal
                        foundItems = null;
                        foundItemsThumbnails = null;
                        foundItemsArray = null;
                        foundItemsThumbnailsArray = null;

                    }

                    listUpdated = true;
                    CurrentPlaylist = Playlists.searchPlaylist;

                    //for disposal
                    foundItemsIndices = null;
                    playlist = null;

                    GC.Collect();
                }
                else
                {
                    //string[][]? playlist = PlayList.GetPlaylist(previousPlaylist);
                    allMusicInfo = ReadAudioDataBase("complete");

                    Image[]? arts = GetMusicThumbnails(allMusicInfo);
                    musicListView.Items.Clear();
                    musicListView.LargeImageList.Images.Clear();
                    FormEditor.PopulateListView(ref musicListView, ref allMusicInfo, ref arts);
                    musicListView.Focus();
                    searchMusicListView.AutoCompleteCustomSource.Clear();

                    FormEditor.SearchBarAutoCompleteSource(allMusicInfo);
                    

                    listUpdated = true;
                    CurrentPlaylist = Playlists.allSongs;

                    //for disposal
                    arts = null;

                    GC.Collect();
                }

            }
            else if (e.KeyValue == (int)Keys.Escape)
            {
                allMusicInfo = ReadAudioDataBase("complete");
                Image[]? arts = GetMusicThumbnails(allMusicInfo);
                musicListView.Items.Clear();
                musicListView.LargeImageList.Images.Clear();
                FormEditor.PopulateListView(ref musicListView, ref allMusicInfo, ref arts);
                musicListView.Focus();
                searchMusicListView.AutoCompleteCustomSource.Clear();

                FormEditor.SearchBarAutoCompleteSource(allMusicInfo);

                listUpdated = true;
                CurrentPlaylist = Playlists.allSongs;

                //for disposal
                arts = null;

                GC.Collect();
            }

        }


        private void sortListArtistMenuItem_Click(object sender, EventArgs e)
        {
            var playlist = PlayList.GetCurrentPlaylist();

            if (playlist != null && playlist[0] != null)
            {
                playlist = PlayList.SortPlaylist(CurrentPlaylist, 1);

                //for now faster and cleaner to simply clear and repopulate instead of changing the indices
                musicListView.Items.Clear();
                musicListView.LargeImageList.Images.Clear();
                Image[]? tempAllArts = (Image[]?)AudioDataBase.GetMusicThumbnails(PlayList.GetCurrentPlaylist()).Clone();
                FormEditor.PopulateListView(ref musicListView, ref playlist, ref tempAllArts);
                listUpdated = true;
                SortOrder = SortOrders.ArtistSort;

                //for disposal
                tempAllArts = null;
                if (CurrentPlaylist != Playlists.allSongs && CurrentPlaylist != Playlists.searchPlaylist) { playlist = null; }

            }
        }

        private void sortListTitleMenuItem_Click(object sender, EventArgs e)
        {
            var playlist = PlayList.GetCurrentPlaylist();

            if (playlist != null && playlist[0] != null)
            {
                playlist = PlayList.SortPlaylist(CurrentPlaylist, 0);

                musicListView.Items.Clear();
                musicListView.LargeImageList.Images.Clear();
                Image[]? tempAllArts = (Image[]?)AudioDataBase.GetMusicThumbnails(PlayList.GetCurrentPlaylist()).Clone();
                FormEditor.PopulateListView(ref musicListView, ref playlist, ref tempAllArts);

                listUpdated = true;
                SortOrder = SortOrders.TitleSort;

                //for disposal
                tempAllArts = null;
                if (CurrentPlaylist != Playlists.allSongs && CurrentPlaylist != Playlists.searchPlaylist) { playlist = null; }
            }
        }

        private void sortListAlbumMenuItem_Click(object sender, EventArgs e)
        {
            var playlist = PlayList.GetCurrentPlaylist();

            if (playlist != null && playlist[0] != null)
            {
                playlist = PlayList.SortPlaylist(CurrentPlaylist, 2);

                musicListView.Items.Clear();
                musicListView.LargeImageList.Images.Clear();
                Image[]? tempAllArts = (Image[]?)AudioDataBase.GetMusicThumbnails(PlayList.GetCurrentPlaylist()).Clone();
                FormEditor.PopulateListView(ref musicListView, ref playlist, ref tempAllArts);
                listUpdated = true;
                SortOrder = SortOrders.AlbumSort;

                //for disposal
                tempAllArts = null;
                if (CurrentPlaylist != Playlists.allSongs && CurrentPlaylist != Playlists.searchPlaylist) { playlist = null; }
            }
        }

        private void toggleShuffle_Click(object sender, EventArgs e)
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
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            PlayBackFunction.MusicPlayBackControl(PlayBackFunction.Controls.Stop);
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
            Settings1.Default.SortOrder = (int)SortOrder;
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

            //for disposal
            tempPlaylist = null;
            playlist = null;


        }
    }

}
