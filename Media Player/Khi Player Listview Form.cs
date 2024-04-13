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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.Unicode;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
        public Form1()
        {

            InitializeComponent();

            DarkTitleBarClass.UseImmersiveDarkMode(Handle, true);




            /*lyricsTextBox.BackColor = Color.FromArgb(41, 41, 41);
            lyricsTextBox.ForeColor = Color.White;
            renameTextBox.BackColor = Color.FromArgb(41, 41, 41);
            renameTextBox.ForeColor = Color.White;
            */

            musicListView.LargeImageList = new ImageList
            {
                ImageSize = new System.Drawing.Size(60, 60)
            };

            //AllSongsPlaylist = musicListView;
            /*
            playlist1.LargeImageList = new ImageList
            {
                ImageSize = new System.Drawing.Size(60, 60)
            };
            playlist2.LargeImageList = new ImageList
            {
                ImageSize = new System.Drawing.Size(60, 60)
            };
            playlist3.LargeImageList = new ImageList
            {
                ImageSize = new System.Drawing.Size(60, 60)
            };
            playlist4.LargeImageList = new ImageList
            {
                ImageSize = new System.Drawing.Size(60, 60)
            };
            playlist5.LargeImageList = new ImageList
            {
                ImageSize = new System.Drawing.Size(60, 60)
            };
            ListView.ListViewItemCollection playlist = new ListView.ListViewItemCollection(musicListView);
            
            playlist1 = musicListView;
            playlist2 = musicListView;
            playlist3 = musicListView;
            playlist4 = musicListView;
            playlist5 = musicListView;
            */

            //(var tempInfo, var tempArts) = AudioDataBase.ReadAudioDataBase();
            //musicListView = AllSongsPlaylist;

            List<string?>? existingDatabases = new List<string?>();

            //Checking to find active Databases to check

            if (System.IO.File.Exists(playlist1Path))
            { existingDatabases.Add(playlist1Path); }
            if (System.IO.File.Exists(playlist2Path))
            { existingDatabases.Add(playlist2Path); }
            if (System.IO.File.Exists(playlist3Path))
            { existingDatabases.Add(playlist3Path); }
            if (System.IO.File.Exists(playlist4Path))
            { existingDatabases.Add(playlist4Path); }
            if (System.IO.File.Exists(playlist5Path))
            { existingDatabases.Add(playlist5Path); }

            XmlDocument PlaylistDatabase;
            XmlElement? AllSongs;  //the document root node

            List<string>? playlistNames = new List<string>();

            foreach (string? playlistPath in existingDatabases)
            {
                PlaylistDatabase = new XmlDocument();
                PlaylistDatabase.Load(playlistPath);
                AllSongs = PlaylistDatabase.DocumentElement;
                playlistNames.Add(AllSongs.GetAttribute("playlist"));
                PlaylistDatabase = null;
            }
            //int count = playlistNames.Count;
            PlaylistDatabase = null;
            existingDatabases = null;
            //add the rightclick menu
            switch (playlistNames.Count)
            {
                case 1:
                    addToPlaylistButton.Enabled = true;
                    addToPlaylistButton.Visible = true;
                    playlist1Button.Text = playlistNames[0];
                    addToPlaylist1.Text = playlistNames[0];
                    playlist1Button.Enabled = true;
                    playlist1Button.Visible = true;
                    addToPlaylist1.Enabled = true;
                    addToPlaylist1.Visible = true;
                    nextPlaylistNumber = 2;
                    break;

                case 2:
                    addToPlaylistButton.Enabled = true;
                    addToPlaylistButton.Visible = true;
                    playlist1Button.Text = playlistNames[0];
                    playlist2Button.Text = playlistNames[1];
                    addToPlaylist1.Text = playlistNames[0];
                    addToPlaylist2.Text = playlistNames[1];
                    playlist1Button.Enabled = true;
                    playlist1Button.Visible = true;
                    addToPlaylist1.Enabled = true;
                    addToPlaylist1.Visible = true;
                    playlist2Button.Enabled = true;
                    playlist2Button.Visible = true;
                    addToPlaylist2.Enabled = true;
                    addToPlaylist2.Visible = true;
                    nextPlaylistNumber = 3;
                    break;

                case 3:
                    addToPlaylistButton.Enabled = true;
                    addToPlaylistButton.Visible = true;
                    playlist1Button.Text = playlistNames[0];
                    playlist2Button.Text = playlistNames[1];
                    playlist3Button.Text = playlistNames[2];
                    addToPlaylist1.Text = playlistNames[0];
                    addToPlaylist2.Text = playlistNames[1];
                    addToPlaylist3.Text = playlistNames[2];
                    playlist1Button.Enabled = true;
                    playlist1Button.Visible = true;
                    addToPlaylist1.Enabled = true;
                    addToPlaylist1.Visible = true;
                    playlist2Button.Enabled = true;
                    playlist2Button.Visible = true;
                    addToPlaylist2.Enabled = true;
                    addToPlaylist2.Visible = true;
                    playlist3Button.Enabled = true;
                    playlist3Button.Visible = true;
                    addToPlaylist3.Enabled = true;
                    addToPlaylist3.Visible = true;
                    nextPlaylistNumber = 4;
                    break;

                case 4:
                    addToPlaylistButton.Enabled = true;
                    addToPlaylistButton.Visible = true;
                    playlist1Button.Text = playlistNames[0];
                    playlist2Button.Text = playlistNames[1];
                    playlist3Button.Text = playlistNames[2];
                    playlist4Button.Text = playlistNames[3];
                    addToPlaylist1.Text = playlistNames[0];
                    addToPlaylist2.Text = playlistNames[1];
                    addToPlaylist3.Text = playlistNames[2];
                    addToPlaylist4.Text = playlistNames[3];
                    playlist1Button.Enabled = true;
                    playlist1Button.Visible = true;
                    addToPlaylist1.Enabled = true;
                    addToPlaylist1.Visible = true;
                    playlist2Button.Enabled = true;
                    playlist2Button.Visible = true;
                    addToPlaylist2.Enabled = true;
                    addToPlaylist2.Visible = true;
                    playlist3Button.Enabled = true;
                    playlist3Button.Visible = true;
                    addToPlaylist3.Enabled = true;
                    addToPlaylist3.Visible = true;
                    playlist4Button.Enabled = true;
                    playlist4Button.Visible = true;
                    addToPlaylist4.Enabled = true;
                    addToPlaylist4.Visible = true;
                    nextPlaylistNumber = 5;
                    break;

                case 5:
                    addToPlaylistButton.Enabled = true;
                    addToPlaylistButton.Visible = true;
                    playlist1Button.Text = playlistNames[0];
                    playlist2Button.Text = playlistNames[1];
                    playlist3Button.Text = playlistNames[2];
                    playlist4Button.Text = playlistNames[3];
                    playlist5Button.Text = playlistNames[4];
                    addToPlaylist1.Text = playlistNames[0];
                    addToPlaylist2.Text = playlistNames[1];
                    addToPlaylist3.Text = playlistNames[2];
                    addToPlaylist4.Text = playlistNames[3];
                    addToPlaylist5.Text = playlistNames[4];
                    playlist1Button.Enabled = true;
                    playlist1Button.Visible = true;
                    addToPlaylist1.Enabled = true;
                    addToPlaylist1.Visible = true;
                    playlist2Button.Enabled = true;
                    playlist2Button.Visible = true;
                    addToPlaylist2.Enabled = true;
                    addToPlaylist2.Visible = true;
                    playlist3Button.Enabled = true;
                    playlist3Button.Visible = true;
                    addToPlaylist3.Enabled = true;
                    addToPlaylist3.Visible = true;
                    playlist4Button.Enabled = true;
                    playlist4Button.Visible = true;
                    addToPlaylist4.Enabled = true;
                    addToPlaylist4.Visible = true;
                    playlist5Button.Enabled = true;
                    playlist5Button.Visible = true;
                    addToPlaylist5.Enabled = true;
                    addToPlaylist5.Visible = true;
                    //put sth for after tthis case 
                    break;

                default:

                    break;
            }

            searchMusicListView.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            searchMusicListView.AutoCompleteSource = AutoCompleteSource.CustomSource;


            if (File.Exists(allMusicDataBase))
            {
                AudioDataBase khiDatabase = new AudioDataBase();
                if (AudioDataBase.errorDetected == false)
                {

                    allMusicInfo = (string[][]?)khiDatabase.AllMusicInfo.Clone();
                    allMusicArts = (Image[]?)khiDatabase.AllMusicArts.Clone();
                    khiDatabase.Dispose();

                    AddToListView.PopulateListView(ref musicListView, allMusicInfo, allMusicArts, false);

                    foreach (string[]? music in allMusicInfo)
                    {
                        searchMusicListView.AutoCompleteCustomSource.Add(music[0]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[1]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[2]);
                    }

                    KhiPlayer = new PlayBackFunction();

                    allMusicArts = null;
                    //musicListView.Refresh();
                    //tempArts = null;
                    //tempInfo = null;
                    //khiDatabase.Dispose();
                    //musicListView.Enabled = false;
                    //GC.Collect();



                }
                else
                {
                    khiDatabase.Dispose();

                    string[][]? dataBaseInfo;
                    Image[]? Arts;
                    //string[]? artPaths;
                    (dataBaseInfo, Arts) = FilterDuplicates.CheckDataBaseAndRemoveDuplicates(musicListView);
                    allMusicInfo = (string[][])dataBaseInfo.Clone();
                    Arts = (Image[]?)Arts.Clone();
                    dataBaseInfo = null;
                    AddToListView.PopulateListView(ref musicListView, allMusicInfo, Arts, false);

                    foreach (string[]? music in allMusicInfo)
                    {
                        searchMusicListView.AutoCompleteCustomSource.Add(music[0]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[1]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[2]);
                    }

                    KhiPlayer = new PlayBackFunction();

                    allMusicArts = null;
                    dataBaseInfo = null;
                    Arts = null;
                    //artPaths = null;


                }
            }
        }

        static ListView AllSongsPlaylist = new ListView();
        static string[][]? allMusicInfo = new string[1][]; //just for initilization
        static Image[]? allMusicArts = new Image[1]; //same thing
        //static string[]? allArtFilePaths = new string[1];
        static ListViewGroup? AllSongsGroup = new ListViewGroup();

        static ListView playlist1 = new ListView();
        static string[][]? playlist1MusicInfo = new string[1][];
        //static Image[][]? playlist1MusicArts;

        static ListView playlist2 = new ListView();
        static string[][]? playlist2MusicInfo = new string[1][];
        //static Image[][]? playlist2MusicArts;

        static ListView playlist3 = new ListView();
        static string[][]? playlist3MusicInfo = new string[1][];
        //static Image[][]? playlist3MusicArts;

        static ListView playlist4 = new ListView();
        static string[][]? playlist4MusicInfo = new string[1][];
        //static Image[][]? playlist4MusicArts;

        static ListView playlist5 = new ListView();
        static string[][]? playlist5MusicInfo = new string[1][];
        //static Image[][] playlist5MusicArts;

        public static string[][]? searchPlaylist { get; set; }

        static int nextPlaylistNumber = Math.Clamp(1, 1, 5);

        public enum Playlists { allSongs, playlist1, playlist2, playlist3, playlist4, playlist5, searchPlaylist };
        public static Playlists CurrentPlaylist;

        // static ArrayList fullMusicPathArray = new ArrayList();
        //static string? selectedMusic;
        //static string[][]? selectedMusics;

        static uint selectedMusicsQue = 0; //only accepts >= 0 value, not negative integers

        //static int fullMusicPathArrayIndex;
        //static bool multipleSelectedItems = false;
        // static ListView fakeFullMusicPathList = new ListView(); // a list to work on and change in Classes before putting the finalized items into the main list
        // static ListView fakeFullMusicPathList2 = new ListView();
        //static ListView? tempListView;

        //static string applicationPath = Application.StartupPath;
        static string allMusicDataBase = Application.StartupPath + "AllMusicDataBase.xml";
        static string albumArtsPath = Application.StartupPath + "\\Album Arts\\";
        static string albumArtsThumbnailsPath = Application.StartupPath + "Album Arts Thumbnails\\";
        static string playlist1Path = System.Windows.Forms.Application.StartupPath + "playlist1DataBase.xml";
        static string playlist2Path = System.Windows.Forms.Application.StartupPath + "playlist2DataBase.xml";
        static string playlist3Path = System.Windows.Forms.Application.StartupPath + "playlist3DataBase.xml";
        static string playlist4Path = System.Windows.Forms.Application.StartupPath + "playlist4DataBase.xml";
        static string playlist5Path = System.Windows.Forms.Application.StartupPath + "playlist5DataBase.xml";
        static string[][] selectedItemsList;
        static int[] selectedItemsListIndex;

        static List<string[]> selectedItems = new List<string[]>();
        static List<int>? selectedItemsIndices = new List<int>();

        static string[] currentlySelectedSong = new string[6];
        static int currentlySelectedSongIndex = 0;
        static string[] currentlyPlayingSongInfo = new string[6];
        static Image? currentlyPlayingSongPic;

        //public static object[][]? musicsProperties;

        public static int seekBarFinalValue;
        public static int seekBarValueBeforeMove;
        public static string? currenSongTimePosition;

        static bool isShuffleEnabled = false;
        static bool isLoopEnabled = false;
        public enum LoopStates { NoLoop, SingleSongLoop, PlaylistLoop };
        public static LoopStates LoopState = LoopStates.NoLoop;

        static bool noSongSelected = false;
        static bool listUpdated = false;
        //static bool listViewFiltered = false;
        static bool isDarkMode = false;

        PlayBackFunction KhiPlayer;
        //static string[][]? testallmusic;
        //public static Khi_Player_Audio_Info_Form audioInfoPage = new Khi_Player_Audio_Info_Form();
        //static AudioDataBase MyDataBase = new AudioDataBase();

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
                filesList.Clear();


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
                    selectedMusicsData = null;
                }

                disposed = true;
            }

            ///<summary> 
            /// checks the entire Database for duplicates, removes them, and reads the new database in complete mode
            /// </summary>
            public static (string[][]?, Image[]?) CheckDataBaseAndRemoveDuplicates(ListView allSongsListView)
            {
                bool duplicatesFound = false;
                string[][] dataBaseInfo = AudioDataBase.ReadAudioDataBase("complete");

                List<string[]> CheckedFilesList = new List<string[]>();
                string[][]? checkedDataBaseInfo;
                string[]? artFileNames;
                Image[]? Arts;
                string[]? allArtFilePaths = new string[dataBaseInfo.Length];
                List<string> tempArtsPaths = new List<string>();
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
                //KhiConverter KhiKote = new KhiConverter(allSongs);
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
                //checkedDataBaseInfo = CheckedFilesList.ToArray();
                artsToRemove = null;


                GC.Collect();
                //(checkedDataBaseInfo, artFiles) = AudioDataBase.ReadAudioDataBase("complete");
                AudioDataBase khiDatabase = new AudioDataBase();
                checkedDataBaseInfo = (string[][])khiDatabase.AllMusicInfo.Clone();
                //artFileNames = (string[])khiDatabase.ArtFileNames.Clone();
                Arts = (Image[])khiDatabase.AllMusicArts.Clone();
                khiDatabase.Dispose();

                return (checkedDataBaseInfo, Arts);
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

            /*
                        static string[][] allMusicInfo;
                        static Image[][] allMusicArts;

                        static string[][] playlist1MusicInfo;
                        static Image[][] playlist1MusicArts;

                        static string[][] playlist2MusicInfo;
                        static Image[][] playlist2MusicArts;

                        static string[][] playlist3MusicInfo;
                        static Image[][] playlist3MusicArts;

                        static string[][] playlist4MusicInfo;
                        static Image[][] playlist4MusicArts;

                        static string[][] playlist5MusicInfo;
                        static Image[][] playlist5MusicArts;
            */


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

            ///<summary>
            ///Creates, writes to, and reads from playlist databases
            ///</summary>
            public AudioDataBase(string[][]? playlistMusicData, int playlistNumber, string? nameOfPlaylist)
            {
                string[][]? playlist;
                Image[]? playlistImages;
                string? name;
                int playlistcase;
                switch (playlistNumber)
                {
                    case 1:
                        playlist = playlist1MusicInfo;
                        playlistImages = playlist1MusicArts;
                        playlistcase = 1;
                        break;

                    case 2:
                        playlist = playlist2MusicInfo;
                        playlistImages = playlist2MusicArts;
                        playlistcase = 2;
                        break;

                    case 3:
                        playlist = playlist3MusicInfo;
                        playlistImages = playlist3MusicArts;
                        playlistcase = 3;
                        break;

                    case 4:
                        playlist = playlist4MusicInfo;
                        playlistImages = playlist4MusicArts;
                        playlistcase = 4;
                        break;

                    case 5:
                        playlist = playlist5MusicInfo;
                        playlistImages = playlist5MusicArts;
                        playlistcase = 5;
                        break;

                    default:
                        playlist = playlist1MusicInfo;
                        playlistImages = playlist1MusicArts;
                        playlistcase = 1;
                        break;

                }

                //string playlistPath = WriteAudioDataBase(playlistMusicData, playlistNumber, nameOfPlaylist);
                WriteAudioDataBase(playlistMusicData, playlistNumber, nameOfPlaylist);

                string? playlistPath;

                switch (playlistNumber)
                {
                    case 1:
                        playlistPath = playlist1Path;
                        break;

                    case 2:
                        playlistPath = playlist2Path;

                        break;

                    case 3:
                        playlistPath = playlist3Path;

                        break;

                    case 4:
                        playlistPath = playlist4Path;

                        break;

                    case 5:
                        playlistPath = playlist5Path;

                        break;

                    default:
                        playlistPath = playlist1Path;
                        break;

                }

                (name, playlist, playlistImages) = ReadPlaylistDataBase(playlistPath, "complete");
                playlistImages = GetMusicThumbnails(playlist);

                switch (playlistcase)
                {
                    case 1:
                        playlist1MusicInfo = playlist;
                        playlist1MusicArts = playlistImages;
                        break;

                    case 2:
                        playlist2MusicInfo = playlist;
                        playlist2MusicArts = playlistImages;
                        break;

                    case 3:
                        playlist3MusicInfo = playlist;
                        playlist3MusicArts = playlistImages;
                        break;
                    case 4:
                        playlist4MusicInfo = playlist;
                        playlist4MusicArts = playlistImages;
                        break;

                    case 5:
                        playlist5MusicInfo = playlist;
                        playlist5MusicArts = playlistImages;
                        break;

                    default:
                        playlist1MusicInfo = playlist;
                        playlist1MusicArts = playlistImages;
                        break;
                }

            }


            /*
            ///<summary>
            ///Creates, Writest, and reads from the specific playlist Data Base
            ///</summary>
            public AudioDataBase(string[][] musicsToPlaylist, int playlistNumber)
            {
                WriteAudioDataBase(musicsToPlaylist, playlistNumber);
                ReadAudioDataBase(playlistNumber);
                isDataBaseRead = true;
                //  ***** Later add the ToListView here
            } */

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
                        // TODO: Dispose managed resources here.
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

                    // TODO: Dispose unmanaged resources here.
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

                disposed = true;
                GC.Collect();
            }


            ///<summary>
            ///Gets the various properties of selected Audio Files  (e.g., title, artist, album, etc)
            ///</summary>
            public static string[][]? GetAudioFilesInfo(string[] draggedMusicsPaths)
            {
                //first is title, then artist, then album, then trackNumber, duration, then path, then lyrics

                string[][] tempMusicInfos = new string[draggedMusicsPaths.Length][];
                Image[] tempMusicArts = new Image[draggedMusicsPaths.Length];


                int i = 0;
                foreach (var audioPath in draggedMusicsPaths)
                {
                    string? artist, album, trackNumber, lyrics;
                    string title, duration, path; // these can't be empty
                    Image? art;
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
                        //
                        */
                        /*
                        // all of the infos except art in a string array
                        musicInfo[0] = title;
                        musicInfo[1] = artist;
                        musicInfo[2] = album;
                        musicInfo[3] = trackNumber.ToString();
                        musicInfo[4] = duration;
                        musicInfo[5] = path;
                        musicInfo[6] = lyrics;
                        */
                        //musicInfo[0] = title + System.Environment.NewLine + artist + System.Environment.NewLine + album;
                        musicInfo[0] = title;
                        musicInfo[1] = artist;
                        musicInfo[2] = album;
                        musicInfo[3] = path;
                        tempMusicInfos[i] = musicInfo;
                        //tempMusicArts[i] = art;


                        musicTags.Dispose();

                        i++;
                    }

                }

                //addedMusicArts = tempMusicArts;
                addedMusicInfo = tempMusicInfos;

                //to dispose
                //musicInfo = null;
                //tempMusicArts = null;
                //tempMusicInfos = null;
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
                //string? musicinfo;
                string? title, artist, album;
                Image? art;
                Image? thumbnail;
                string[][]? selectedMusicsData;
                Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                using (FilterDuplicates duplicateCheck = new FilterDuplicates(selectedMusicsInfo))
                {
                    selectedMusicsData = (string[][]?)duplicateCheck.selectedMusicsData.Clone();
                }
                /*
                FileStreamOptions options = new FileStreamOptions();
                options.Options = FileOptions.None;
                options.Share = FileShare.ReadWrite;
                options.Mode = FileMode.Append;
                options.Access = FileAccess.Write;
                options.BufferSize = 4096;
                options.PreallocationSize = 0;

                using (StreamWriter selectedDataWriter = new StreamWriter(allMusicDataBase, Encoding.UTF8, options))
                //System.IO.File.AppendText(allMusicDataBase))
                */

                string[] fileNames = new string[selectedMusicsData.Length];


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
                        /*
                        title = musicDataArray[0];
                        artist = musicDataArray[1];
                        album = musicDataArray[2];
                        duration = musicDataArray[3];
                        trackNumber = musicDataArray[4];
                        path = musicDataArray[5];
                        lyrics = musicDataArray[6];
                        */
                        //musicinfo = musicDataArray[0];
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
                            //art = Image.FromFile("C:\\Users\\Rushakh\\source\\repos\\Khi Player\\Khi Player\\MusicArt - NoCover.png");
                            art = Khi_Player.Properties.Resources.Khi_Player;
                            thumbnail = Khi_Player.Properties.Resources.Khi_Player.GetThumbnailImage(60, 60, myCallback, 0);
                            //Properties.Resources.MusicArt_NoCover; 
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

                        //picConverter.Dispose();
                        musicTags.Dispose();
                        name.Dispose();
                        //char[] chararray = ['/', '.', '\''];
                        //string songHeader = title.Replace(" ", "").Trim(chararray);

                        XmlElement Song = allMusicDataBaseDoc.CreateElement("ArrayOfString");  //Child of AllSongs, and parent to the other elements

                        if (allMusicDataBaseDoc.DocumentElement.HasChildNodes)
                        {
                            allMusicDataBaseDoc.DocumentElement.InsertAfter(Song, allMusicDataBaseDoc.DocumentElement.LastChild);
                        }
                        else
                        {
                            allMusicDataBaseDoc.DocumentElement.AppendChild(Song);
                        }
                        /*
                        XmlElement Title = allMusicDataBaseDoc.CreateElement("string");
                        XmlElement Artist = allMusicDataBaseDoc.CreateElement("string");
                        XmlElement Album = allMusicDataBaseDoc.CreateElement("string");
                        XmlElement Duration = allMusicDataBaseDoc.CreateElement("string");
                        XmlElement TrackNumber = allMusicDataBaseDoc.CreateElement("string");
                        XmlElement Path = allMusicDataBaseDoc.CreateElement("string");
                        XmlElement Lyrics = allMusicDataBaseDoc.CreateElement("string");
                        */
                        XmlElement Title = allMusicDataBaseDoc.CreateElement("string");
                        XmlElement Artist = allMusicDataBaseDoc.CreateElement("string");
                        XmlElement Album = allMusicDataBaseDoc.CreateElement("string");
                        XmlElement Path = allMusicDataBaseDoc.CreateElement("string");
                        XmlElement ArtPath = allMusicDataBaseDoc.CreateElement("string");
                        XmlElement ThumbnailPath = allMusicDataBaseDoc.CreateElement("string");

                        //XmlElement MusicInfo = allMusicDataBaseDoc.CreateElement("string");
                        /*
                        Title.InnerText = title;
                        Artist.InnerText = artist;
                        Album.InnerText = album;
                        Duration.InnerText = duration;
                        TrackNumber.InnerText = trackNumber.ToString();
                        Path.InnerText = path;
                        Lyrics.InnerText = lyrics;
                        */
                        //MusicInfo.InnerText = musicinfo;
                        /*
                        Song.AppendChild(Title);
                        Song.AppendChild(Artist);
                        Song.AppendChild(Album);
                        Song.AppendChild(Duration);
                        Song.AppendChild(TrackNumber);
                        Song.AppendChild(Path);
                        Song.AppendChild(Lyrics);
*/
                        //Song.AppendChild(MusicInfo);

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
                /*
                {
                    XmlSerializer dataBaseSerializer = new XmlSerializer(typeof(string[][]));
                    using (StreamWriter dataBaseWriter = new StreamWriter(allMusicDataBase, false, Encoding.UTF8))
                    {

                        dataBaseSerializer.Serialize(dataBaseWriter, selectedMusicsData);
                    }

                }
                */
                int addedCount = selectedMusicsData.Length;
                //to dispose
                selectedMusicsData = null;
                selectedMusicsInfo = null;
                options = null;
                allMusicDataBaseDoc = null;
                art = null;
                thumbnail = null;
                //filesList.Clear();
                // title = null;
                // artist = null;
                // album = null;
                // duration = null;
                // path = null;
                // lyrics = null;
                return addedCount;
            }

            ///<summary>
            ///Adds a single song's info to the specified playlist's data base
            ///</summary>
            public static string? WriteSongToPlaylistDatabase(string[]? playlistMusic, int playlistNumber, string? nameOfPlaylist)
            {
                string[]? checkedPlaylistMusic;
                string? title, artist, album, path, artPath, thumbnailPath;
                string? playlistName;
                string? playlist;

                switch (playlistNumber)
                {
                    case 1:
                        playlist = playlist1Path;
                        break;

                    case 2:
                        playlist = playlist2Path;

                        break;

                    case 3:
                        playlist = playlist3Path;

                        break;

                    case 4:
                        playlist = playlist4Path;

                        break;

                    case 5:
                        playlist = playlist5Path;

                        break;

                    default:
                        playlist = playlist1Path;
                        break;

                }

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

                    return playlist;
                }
                else
                {
                    playlist = null;
                    return playlist;
                }
            }

            ///<summary>
            ///Adds the selected songs' info to the specified playlist's data base
            ///</summary>
            public static async void WriteAudioDataBase(string[][]? playlistMusicData, int playlistNumber, string? nameOfPlaylist)
            {
                string[][]? checkedPlaylistMusicData;
                string title, artist, album, path, artPath, thumbnailPath;
                string? playlistName;
                string? playlist;

                switch (playlistNumber)
                {
                    case 1:
                        playlist = playlist1Path;
                        break;

                    case 2:
                        playlist = playlist2Path;

                        break;

                    case 3:
                        playlist = playlist3Path;

                        break;

                    case 4:
                        playlist = playlist4Path;

                        break;

                    case 5:
                        playlist = playlist5Path;

                        break;

                    default:
                        playlist = playlist1Path;
                        break;

                }

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
                });
                //return playlist;
            }


            ///<summary>
            /// Reads the data base that contains all music files info 
            ///</summary>

            //public static (string[][], Image[], string[]) ReadAudioDataBase()
            public static (string?, string[][]?, Image[]?) ReadPlaylistDataBase(string? playlistPath, string? ReadingMode)
            {
                string? playlistName;
                string[][]? tempPlaylistMusicInfo;
                Image[]? playlistArts;
                XmlDocument playlistDatabase = new XmlDocument();
                XmlElement playlistSongs;  //the document root node

                playlistDatabase.Load(playlistPath);
                playlistSongs = playlistDatabase.DocumentElement;
                playlistName = playlistSongs.GetAttribute("playlistName");

                if (ReadingMode == "added")
                {
                    int added;
                    //string[] musicElements = new string[4];
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

                else
                {



                    //string[] musicElements = new string[4];
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

            ///<summary>
            /// Reads the data base that contains all music files info in addition to album arts of the songs
            /// if the keyword "complete" is included or an empty string. If "added" is included only the recently 
            /// added files's info are read
            ///</summary>
            //public static (string[][], Image[], string[]) ReadAudioDataBase()
            public static string[][]? ReadAudioDataBase(string ReadingMode)
            {
                //allMusicInfo = Array.Empty<string[]>();
                string[][]? tempAllMusicInfos;
                //string[]? filesNames;
                //Image? art;

                if (ReadingMode == "added")
                {
                    int added;
                    //string[] musicElements = new string[4];
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



                    //string[] musicElements = new string[4];
                    XmlSerializer dataBaseSerializer = new XmlSerializer(typeof(string[][]));
                    using (StreamReader textReader = new StreamReader(allMusicDataBase, Encoding.UTF8))
                    {
                        tempAllMusicInfos = (string[][])dataBaseSerializer.Deserialize(textReader);
                    }


                    dataBaseSerializer = null;
                    return tempAllMusicInfos;
                }

                /*
                XmlDocument MusicDataBase = new XmlDocument();
                XmlElement AllSongs;  //the document root node
                MusicDataBase.Load(allMusicDataBase);
                AllSongs = MusicDataBase.DocumentElement;
                string? playlist = AllSongs.GetAttribute("playlist");
                string[][]? tempPlaylistData;
                //TextReader textReader = new StringReader(allMusicDataBase);
                XmlReader MusicDataBaseReader = XmlReader.Create(allMusicDataBase);
                string[][]? playlistMusics = new string[ AllSongs.ChildNodes.Count][];
                //KhiConverter KhiKote = new KhiConverter(allSongs);
                for (int i = 0; i < AllSongs.ChildNodes.Count; i++)
                {
                    playlistMusics[i][0] = AllSongs.ChildNodes[i].ChildNodes[0].InnerText;
                    playlistMusics[i][1] = AllSongs.ChildNodes[i].ChildNodes[1].InnerText;
                    playlistMusics[i][2] = AllSongs.ChildNodes[i].ChildNodes[2].InnerText;
                    playlistMusics[i][3] = AllSongs.ChildNodes[i].ChildNodes[3].InnerText;
                    playlistMusics[i][4] = AllSongs.ChildNodes[i].ChildNodes[4].InnerText;
                    playlistMusics[i][5] = AllSongs.ChildNodes[i].ChildNodes[5].InnerText;
                }

                return playlistMusics;
                */
                //string[][] playlistMusics = new string[allMusicInfo.Length][];

                /*
                var templistinfo = allMusicInfo.ToList<string[]>();
                templistinfo = templistinfo.Where(c => c != null).ToList();
                //allMusicInfo = templistinfo.ToArray();
                */

                //to dispose
                //tempMusicElements = null;
                //templistinfo.Clear();

                //return (tempAllMusicInfos, tempAllMusicArts,filesNames);
                //}

            }


            ///<summary>
            /// Reads PlaylistIni.xml, returns the paths of the xml data bases that contain playlists data
            ///</summary>
            public static string[]? PlaylistsIntitializer()
            {
                string[]? playlistPaths;
                XmlDocument PlaylistsIni = new XmlDocument();
                XmlElement AllPlaylists;  //the document root node
                if (System.IO.File.Exists(playListIni))
                {
                    PlaylistsIni.Load(allMusicDataBase);
                    AllPlaylists = PlaylistsIni.DocumentElement;
                    playlistPaths = new string[AllPlaylists.ChildNodes.Count];
                    for (int i = 0; i < AllPlaylists.ChildNodes.Count; i++)
                    {
                        playlistPaths[i] = AllPlaylists.ChildNodes[i].InnerText;
                    }
                }
                else
                {
                    playlistPaths = null;
                }




                return playlistPaths;
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

                    for (int i = 0; i < AllSongs.ChildNodes.Count; i++)
                    {
                        playlistData[i][0] = AllSongs.ChildNodes[i].ChildNodes[0].InnerText;
                        playlistData[i][1] = AllSongs.ChildNodes[i].ChildNodes[1].InnerText;
                        playlistData[i][2] = AllSongs.ChildNodes[i].ChildNodes[2].InnerText;
                        playlistData[i][3] = AllSongs.ChildNodes[i].ChildNodes[3].InnerText;
                        playlistData[i][4] = AllSongs.ChildNodes[i].ChildNodes[4].InnerText;
                        playlistData[i][5] = AllSongs.ChildNodes[i].ChildNodes[5].InnerText;
                    }
                }
                else
                {
                    playlistName = null;
                    playlistData = null;
                }

                return (playlistName, playlistData);
            }

            ///<summary>
            /// Removes an item (array of subitems) from the database, its duplicates and delete its corresponding cover art as well
            ///</summary>
            public static bool RemoveSong(string[] item, string artFileName, string artThumbnailName)
            {
                bool isRemoved = false;
                //int numberOfItems = 0;
                List<int> similarItemsIndices = new List<int>();
                string title = item[0];
                string? artist = item[1];
                string? album = item[2];
                string path = item[3];
                //var temputf8 = Encoding.UTF8.GetBytes(item[3]);   Just in case if the text needs to be in utf8 for them to match
                XmlElement AllSongs;  //the document root node
                XmlDocument musicDatabase = new XmlDocument();

                List<string?> existingDatabases = new List<string?>();

                //Checking to find active Databases to check
                if (System.IO.File.Exists(allMusicDataBase))
                { existingDatabases.Add(allMusicDataBase); }
                if (System.IO.File.Exists(playlist1Path))
                { existingDatabases.Add(playlist1Path); }
                if (System.IO.File.Exists(playlist2Path))
                { existingDatabases.Add(playlist2Path); }
                if (System.IO.File.Exists(playlist3Path))
                { existingDatabases.Add(playlist3Path); }
                if (System.IO.File.Exists(playlist4Path))
                { existingDatabases.Add(playlist4Path); }
                if (System.IO.File.Exists(playlist5Path))
                { existingDatabases.Add(playlist5Path); }

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
                    /*
                    if (similarItemsIndices.Count <= 1)
                    {
                        AllSongs.RemoveChild(AllSongs.ChildNodes[similarItemsIndices[0]]);
                        isRemoved = true;
                        allMusicDataBaseDoc.Save(allMusicDataBase);
                    }

                    else
                    {
                        allMusicDataBaseDoc.Save(allMusicDataBase);
                        allMusicDataBaseDoc.RemoveAll
                        string[][] tempAllMusicInfos;
                        List<string[]> allmusics = new List<string[]>();
                        XmlSerializer dataBaseSerializer = new XmlSerializer(typeof(string[][]));
                        using (StreamReader textReader = new StreamReader(allMusicDataBase, Encoding.UTF8))
                        {
                            tempAllMusicInfos = (string[][])dataBaseSerializer.Deserialize(textReader);
                        }
                        for (int y = 0; y < tempAllMusicInfos.Length; y++)
                        {
                            bool isDupli = false;
                            int z = 0; // counter for items that are not duplicate
                            if (similarItemsIndices.Count > 0)
                            {
                                if (similarItemsIndices.Contains(y))
                                {
                                    isDupli = true;
                                    similarItemsIndices.Remove(y);
                                }
                                if (isDupli == false)
                                {
                                    allmusics.Add(tempAllMusicInfos[z]);
                                    z++;
                                }
                            }
                            else
                            {
                                allmusics.Add(tempAllMusicInfos[z]);
                                z++;
                            }

                        }
                        //Now we have a list that contains the items that are not duplicates


                    }
                    */



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
                //

                return isRemoved;
            }

            ///<summary>
            /// Checks the Database for songs that Don't exists and Removes them
            ///</summary>
            public static bool RemoveInvalidDatabaseElements()
            {
                bool InvalidsFoundAndRemoved = false;
                List<string?> existingDatabases = new List<string?>();

                //Checking to find active Databases to check
                if (System.IO.File.Exists(allMusicDataBase))
                { existingDatabases.Add(allMusicDataBase); }
                if (System.IO.File.Exists(playlist1Path))
                { existingDatabases.Add(playlist1Path); }
                if (System.IO.File.Exists(playlist2Path))
                { existingDatabases.Add(playlist2Path); }
                if (System.IO.File.Exists(playlist3Path))
                { existingDatabases.Add(playlist3Path); }
                if (System.IO.File.Exists(playlist4Path))
                { existingDatabases.Add(playlist4Path); }
                if (System.IO.File.Exists(playlist5Path))
                { existingDatabases.Add(playlist5Path); }




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


                return InvalidsFoundAndRemoved;
            }

            ///<summary>
            /// Gets the previously prepared thumbnails of the audiofiles, and creates them again in case they don't exist
            ///</summary>
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
                        //AllMusicArts[i] = Image.FromFile(ArtFileNames[i]);
                        thumbnail = Image.FromFile(musicData[i][5]);
                        musicThumbnails[i] = (Image)thumbnail.Clone();

                        thumbnail = null;
                    }


                }

                return musicThumbnails;
            }

            /*
            ///<summary>
            /// Reads the data base of the specified playlist
            ///</summary>
            public static void ReadAudioDataBase(int playlistNumber)
            {
                switch (playlistNumber)
                {
                    case 1:

                        string tempplaylist = System.IO.File.ReadAllText(allMusicDataBase, Encoding.Unicode);
                        string[][] playlistMusics = new string[allMusicInfo.Length][];   // these 3 have the same indices
                        allMusicArts = new Image[allMusicInfo.Length][];       // playlistMusics, allMusicInfo, allMusicArts
                        allMusicInfo = new string[tempplaylist.Length][];
                        tempListView = new ListView();

                        string[] musicElements = new string[7];
                        string[] tempMusicElements = tempplaylist.Split('|', StringSplitOptions.None);
                        int musicNumbers = tempMusicElements.Length / (14 - 1);
                        string[][] allmusics = new string[musicNumbers][];
                        for (int x = 0; x <= musicNumbers; x++)
                        {
                            allmusics[x][0] = tempMusicElements[(x * 14 + 1)];
                            allmusics[x][1] = tempMusicElements[(x * 14 + 1) + 2];
                            allmusics[x][2] = tempMusicElements[(x * 14 + 1) + 4];
                            allmusics[x][3] = tempMusicElements[(x * 14 + 1) + 6];
                            allmusics[x][4] = tempMusicElements[(x * 14 + 1) + 8];
                            allmusics[x][5] = tempMusicElements[(x * 14 + 1) + 10];
                            allmusics[x][6] = tempMusicElements[(x * 14 + 1) + 12];
                        }

                        for (int i = 0; i <= allmusics.Length; i++)
                        {
                            playlistMusics[i] = allmusics[i];

                            playlist1MusicInfo[i] = allmusics[i];

                            //tempListView.Items.Clear();
                            tempListView.Items.Add(new ListViewItem(playlistMusics[i]));

                            TagLib.File musicTags = TagLib.File.Create(playlistMusics[i][4]);

                            var tempPics = musicTags.Tag.Pictures;
                            Image art;
                            if (tempPics.Length == 0)
                            {
                                ShellFile shellfile = ShellFile.FromFilePath(playlistMusics[i][4]);
                                art = shellfile.Thumbnail.ExtraLargeBitmap;
                            }
                            else
                            {
                                using (MemoryStream picConverter = new MemoryStream(tempPics[0].Data.Data))
                                {
                                    art = Image.FromStream(picConverter);
                                }
                            }
                            Image[] musicArt = { art };
                            playlist1MusicArts[i] = musicArt;
                        }

                        break;

                    case 2:

                        tempplaylist = System.IO.File.ReadAllText(allMusicDataBase);
                        playlistMusics = new string[allMusicInfo.Length][];   // these 3 have the same indices
                        allMusicArts = new Image[allMusicInfo.Length][];       // playlistMusics, allMusicInfo, allMusicArts
                        allMusicInfo = new string[tempplaylist.Length][];

                        musicElements = new string[7];
                        tempMusicElements = tempplaylist.Split('|', StringSplitOptions.None);
                        musicNumbers = tempMusicElements.Length / (14 - 1);
                        allmusics = new string[musicNumbers][];
                        for (int x = 0; x <= musicNumbers; x++)
                        {
                            allmusics[x][0] = tempMusicElements[(x * 14 + 1)];
                            allmusics[x][1] = tempMusicElements[(x * 14 + 1) + 2];
                            allmusics[x][2] = tempMusicElements[(x * 14 + 1) + 4];
                            allmusics[x][3] = tempMusicElements[(x * 14 + 1) + 6];
                            allmusics[x][4] = tempMusicElements[(x * 14 + 1) + 8];
                            allmusics[x][5] = tempMusicElements[(x * 14 + 1) + 10];
                            allmusics[x][6] = tempMusicElements[(x * 14 + 1) + 12];
                        }

                        for (int i = 0; i <= allmusics.Length; i++)
                        {
                            playlistMusics[i] = allmusics[i];

                            playlist2MusicInfo[i] = allmusics[i];
                            tempListView = new ListView();
                            //tempListView.Items.Clear();
                            tempListView.Items.Add(new ListViewItem(playlistMusics[i]));

                            TagLib.File musicTags = TagLib.File.Create(playlistMusics[i][4]);

                            var tempPics = musicTags.Tag.Pictures;
                            Image art;
                            if (tempPics.Length == 0)
                            {
                                ShellFile shellfile = ShellFile.FromFilePath(playlistMusics[i][4]);
                                art = shellfile.Thumbnail.ExtraLargeBitmap;
                            }
                            else
                            {
                                using (MemoryStream picConverter = new MemoryStream(tempPics[0].Data.Data))
                                {
                                    art = Image.FromStream(picConverter);
                                }
                            }
                            Image[] musicArt = { art };
                            playlist2MusicArts[i] = musicArt;
                        }

                        break;

                    case 3:

                        tempplaylist = System.IO.File.ReadAllText(allMusicDataBase);
                        playlistMusics = new string[allMusicInfo.Length][];   // these 3 have the same indices
                        allMusicArts = new Image[allMusicInfo.Length][];       // playlistMusics, allMusicInfo, allMusicArts
                        allMusicInfo = new string[tempplaylist.Length][];
                        tempListView = new ListView();

                        musicElements = new string[7];
                        tempMusicElements = tempplaylist.Split('|', StringSplitOptions.None);
                        musicNumbers = tempMusicElements.Length / (14 - 1);
                        allmusics = new string[musicNumbers][];
                        for (int x = 0; x <= musicNumbers; x++)
                        {
                            allmusics[x][0] = tempMusicElements[(x * 14 + 1)];
                            allmusics[x][1] = tempMusicElements[(x * 14 + 1) + 2];
                            allmusics[x][2] = tempMusicElements[(x * 14 + 1) + 4];
                            allmusics[x][3] = tempMusicElements[(x * 14 + 1) + 6];
                            allmusics[x][4] = tempMusicElements[(x * 14 + 1) + 8];
                            allmusics[x][5] = tempMusicElements[(x * 14 + 1) + 10];
                            allmusics[x][6] = tempMusicElements[(x * 14 + 1) + 12];
                        }

                        for (int i = 0; i <= allmusics.Length; i++)
                        {
                            playlistMusics[i] = allmusics[i];

                            playlist3MusicInfo[i] = allmusics[i];

                            tempListView.Items.Clear();
                            tempListView.Items.Add(new ListViewItem(playlistMusics[i]));

                            TagLib.File musicTags = TagLib.File.Create(playlistMusics[i][4]);

                            var tempPics = musicTags.Tag.Pictures;
                            Image art;
                            if (tempPics.Length == 0)
                            {
                                ShellFile shellfile = ShellFile.FromFilePath(playlistMusics[i][4]);
                                art = shellfile.Thumbnail.ExtraLargeBitmap;
                            }
                            else
                            {
                                using (MemoryStream picConverter = new MemoryStream(tempPics[0].Data.Data))
                                {
                                    art = Image.FromStream(picConverter);
                                }
                            }
                            Image[] musicArt = { art };
                            playlist3MusicArts[i] = musicArt;
                        }
                        break;

                    case 4:

                        tempplaylist = System.IO.File.ReadAllText(allMusicDataBase);
                        playlistMusics = new string[allMusicInfo.Length][];   // these 3 have the same indices
                        allMusicArts = new Image[allMusicInfo.Length][];       // playlistMusics, allMusicInfo, allMusicArts
                        allMusicInfo = new string[tempplaylist.Length][];
                        tempListView = new ListView();

                        musicElements = new string[7];
                        tempMusicElements = tempplaylist.Split('|', StringSplitOptions.None);
                        musicNumbers = tempMusicElements.Length / (14 - 1);
                        allmusics = new string[musicNumbers][];
                        for (int x = 0; x <= musicNumbers; x++)
                        {
                            allmusics[x][0] = tempMusicElements[(x * 14 + 1)];
                            allmusics[x][1] = tempMusicElements[(x * 14 + 1) + 2];
                            allmusics[x][2] = tempMusicElements[(x * 14 + 1) + 4];
                            allmusics[x][3] = tempMusicElements[(x * 14 + 1) + 6];
                            allmusics[x][4] = tempMusicElements[(x * 14 + 1) + 8];
                            allmusics[x][5] = tempMusicElements[(x * 14 + 1) + 10];
                            allmusics[x][6] = tempMusicElements[(x * 14 + 1) + 12];
                        }

                        for (int i = 0; i <= allmusics.Length; i++)
                        {
                            playlistMusics[i] = allmusics[i];

                            playlist4MusicInfo[i] = allmusics[i];

                            tempListView.Items.Clear();
                            tempListView.Items.Add(new ListViewItem(playlistMusics[i]));

                            TagLib.File musicTags = TagLib.File.Create(playlistMusics[i][4]);

                            var tempPics = musicTags.Tag.Pictures;
                            Image art;
                            if (tempPics.Length == 0)
                            {
                                ShellFile shellfile = ShellFile.FromFilePath(playlistMusics[i][4]);
                                art = shellfile.Thumbnail.ExtraLargeBitmap;
                            }
                            else
                            {
                                using (MemoryStream picConverter = new MemoryStream(tempPics[0].Data.Data))
                                {
                                    art = Image.FromStream(picConverter);
                                }
                            }
                            Image[] musicArt = { art };
                            playlist4MusicArts[i] = musicArt;
                        }
                        break;

                    case 5:

                        tempplaylist = System.IO.File.ReadAllText(allMusicDataBase);
                        playlistMusics = new string[allMusicInfo.Length][];   // these 3 have the same indices
                        allMusicArts = new Image[allMusicInfo.Length][];       // playlistMusics, allMusicInfo, allMusicArts
                        allMusicInfo = new string[tempplaylist.Length][];
                        tempListView = new ListView();

                        musicElements = new string[7];
                        tempMusicElements = tempplaylist.Split('|', StringSplitOptions.None);
                        musicNumbers = tempMusicElements.Length / (14 - 1);
                        allmusics = new string[musicNumbers][];
                        for (int x = 0; x <= musicNumbers; x++)
                        {
                            allmusics[x][0] = tempMusicElements[(x * 14 + 1)];
                            allmusics[x][1] = tempMusicElements[(x * 14 + 1) + 2];
                            allmusics[x][2] = tempMusicElements[(x * 14 + 1) + 4];
                            allmusics[x][3] = tempMusicElements[(x * 14 + 1) + 6];
                            allmusics[x][4] = tempMusicElements[(x * 14 + 1) + 8];
                            allmusics[x][5] = tempMusicElements[(x * 14 + 1) + 10];
                            allmusics[x][6] = tempMusicElements[(x * 14 + 1) + 12];
                        }

                        for (int i = 0; i <= allmusics.Length; i++)
                        {
                            playlistMusics[i] = allmusics[i];

                            playlist5MusicInfo[i] = allmusics[i];

                            tempListView.Items.Clear();
                            tempListView.Items.Add(new ListViewItem(playlistMusics[i]));

                            TagLib.File musicTags = TagLib.File.Create(playlistMusics[i][4]);

                            var tempPics = musicTags.Tag.Pictures;
                            Image art;
                            if (tempPics.Length == 0)
                            {
                                ShellFile shellfile = ShellFile.FromFilePath(playlistMusics[i][4]);
                                art = shellfile.Thumbnail.ExtraLargeBitmap;
                            }
                            else
                            {
                                using (MemoryStream picConverter = new MemoryStream(tempPics[0].Data.Data))
                                {
                                    art = Image.FromStream(picConverter);
                                }
                            }
                            Image[] musicArt = { art };
                            playlist5MusicArts[i] = musicArt;
                        }
                        break;

                }
            }

            public static void ToListView()
            {
                tempListView = new ListView();
                AllSongsPlaylist = tempListView;  // for now, should later change it using Switch and case for playlists

            } */
        }

        public class AddToListView
        {

            public AddToListView(ref ListView listview, string[][]? musicInfos, Image[]? musicThumbnails)
            {
                //should clear imagelist before this
                listview.BeginUpdate();



                int x = 0;
                foreach (var music in musicInfos)
                {

                    //musicListView.SmallImageList.Images.Add(Icon.ExtractAssociatedIcon(khiDatabase.ArtFileNames[x]));
                    //musicListView.LargeImageList.Images.Add(Image.FromFile(khiDatabase.ArtFileNames[x]));
                    listview.LargeImageList.Images.Add(musicThumbnails[x]);
                    //musicListView.SmallImageList.Images.Add(allMusicArts[x]);
                    ListViewItem song = new ListViewItem(music, x);
                    song.ToolTipText = music[0] + System.Environment.NewLine + music[1] + System.Environment.NewLine + music[2];
                    song.Name = music[3];
                    //song.BeginEdit();


                    listview.Items.Add(song);

                    song = null;
                    x++;
                }
            }

            public AddToListView(string[]? itemNames, ref ListView SourceListview, ref ListView TargetListview, bool cloneItems = false)
            {

                if (cloneItems == true)
                {

                }
                else
                {
                    int i = 0;
                    if (TargetListview.LargeImageList.Images.Count > 0)
                    {
                        i = TargetListview.LargeImageList.Images.Count;  // need to w8 and see, if i need to clone the items in case the image doesnt get transfered, i can use this
                    }
                    foreach (string? name in itemNames)
                    {
                        var theItem = SourceListview.Items.Find(name, false)[0];
                        //TargetListview.LargeImageList.Images.Add(SourceListview.LargeImageList.Images[theItem.ImageIndex]);


                        TargetListview.Items.Add(theItem);
                        i++;
                    }
                }
            }

            public static void PopulateListView(ref ListView listview, string[][]? musicInfos, Image[]? musicThumbnails, bool noImageMode = false)
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
        }

        public class KhiConverter : IDisposable
        {
            private bool disposed;
            public string[][]? AllInfo { get; set; }
            //public Image[]? AllMusicArts { get; set; }
            public string[]? artsPaths { get; set; }

            /// <summary>
            /// extracts a ListView control's Items and their Images
            /// </summary>
            public KhiConverter(ListView listView)
            {
                string[][]? tempAllmusic = new string[listView.Items.Count][];
                //Image[]? tempAllArt = new Image[listView.LargeImageList.Images.Count];
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
                        //tempMusic[4] = listView.Items[i].SubItems[4].Text;
                        //tempMusic[5] = listView.Items[i].SubItems[5].Text;
                        //tempMusic[6] = listView.Items[i].SubItems[6].Text.ReplaceLineEndings();

                        //tempArt = listView.LargeImageList.Images[i];
                        //tempArt = Image.FromFile(allArtFilePaths[i]);

                        tempAllmusic[i] = tempMusic;
                        //tempAllArt[i] = (Image?)tempArt.Clone();


                        i++;
                    }

                    //to dispose
                    //tempMusic = null;
                    //tempArt = null;

                }

                AllInfo = (string[][]?)tempAllmusic.Clone();
                //AllMusicArts = tempAllArt;
                //artsPaths = (string[]?)allArtFilePaths.Clone();
                artsPaths = new string[allMusicInfo.Length];
                i = 0;
                /*
                foreach (string[]? songInfo in allMusicInfo)
                {
                    artsPaths[i] = songInfo[4];
                    i++;
                }*/
                //to dispose
                tempAllmusic = null;
            }

            /// <summary>
            /// populates an existing ListView control items with a string jagged array and an Image array (their index will be idendical)
            /// </summary>
            public KhiConverter(string[][] allMusicInfo, Image[] allMusicArt, ListView listView)
            {

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

                    // Dispose unmanaged resources here.
                    AllInfo = null;
                    //AllMusicArts = null;
                    artsPaths = null;
                }

                disposed = true;
            }

        }

        public class PlayList
        {
            //write it in a way that creating objects results in a playlist or sth close
            // can make it inherit AudioFileData or can make audiofile data call it
            //
            //LinkedList<object> playlist = new LinkedList<object>();
            public List<string> playlistMusicsPaths = new List<string>();
            public List<string[]> myPlaylistList = new List<string[]>();
            public ListView myPlaylistListview = new ListView();
            private string playlistName { get; set; }
            private string playlistPath;
            private static string applicationPath;

            public PlayList(string nameOfPlaylist, string[][] selectedMusicsForPlaylist)
            {

                foreach (var music in selectedMusicsForPlaylist)
                {
                    myPlaylistList.Add(music);
                    myPlaylistListview.Items.Add(new ListViewItem(music));
                    playlistName = nameOfPlaylist.Replace(' ', '-');
                    applicationPath = Application.StartupPath;
                    playlistPath = applicationPath + "\\" + playlistName + ".txt";


                    if (!System.IO.File.Exists(playlistPath))
                    {
                        using (System.IO.File.Create(playlistPath)) //if this name doesn't exists, creates it
                        { }
                        //System.IO.File.SetAttributes(playlistPath, System.IO.File.GetAttributes(playlistPath) | FileAttributes.Hidden);

                        WritePlaylistToDataBase(playlistName, playlistPath, selectedMusicsForPlaylist);
                        //myPlaylistListview.Name = playlistName;
                        ReadPlaylist(playlistPath);

                    }
                    if (System.IO.File.Exists(playlistPath))
                    {
                        if (System.IO.File.ReadAllText(playlistPath) != "")
                        {
                            ReadPlaylist(playlistPath);
                        }
                        if (System.IO.File.ReadAllText(playlistPath) == "")
                        {
                            //System.IO.File.SetAttributes(playlistPath, System.IO.File.GetAttributes(playlistPath) | FileAttributes.Hidden);
                            //myPlaylistListview.Name = playlistName;
                            WritePlaylistToDataBase(playlistName, playlistPath, selectedMusicsForPlaylist);
                        }
                    }


                }

            }

            public static void WritePlaylistToDataBase(string playlistName, string playlistPath, string[][] selectedMusicsForPlaylist)
            {


                //From Here on the Playlist is written into a file

                List<string[]> filesList = new List<string[]>();
                string[][] playlistMusicsData;
                using (StreamReader duplicateChecker = new StreamReader(playlistPath))
                {
                    string tempTextFileData = duplicateChecker.ReadToEnd();
                    if (tempTextFileData != null)
                    {
                        int i = 0;
                        foreach (var playlistMusic in selectedMusicsForPlaylist) // To check if the added file path already exists in the txt
                        {
                            if (!tempTextFileData.Contains(playlistMusic[4]))
                            {
                                filesList.Add(playlistMusic);
                            }
                            i++;
                        }
                        playlistMusicsData = filesList.ToArray();
                    }
                    else
                    {
                        int i = 0;
                        playlistMusicsData = selectedMusicsForPlaylist;

                    }
                }



                using (StreamWriter playlistDataWriter = System.IO.File.AppendText(playlistPath))
                {
                    //fullMusicPathArray.Clear();

                    foreach (string[] playlistMusic in playlistMusicsData)
                    {
                        string title = playlistMusic[0];
                        string artist = playlistMusic[1];
                        string album = playlistMusic[2];
                        string duration = playlistMusic[3];
                        string path = playlistMusic[4];
                        string format = playlistMusic[5];
                        string lyrics = playlistMusic[6];


                        playlistDataWriter.WriteLine('|' + title + '|' + '|' + artist + '|' + '|' + album + '|' + '|' + duration + '|' + '|' +
                           path + '|' + '|' + format + '|' + '|' + lyrics + '|');
                        //fullMusicPathArray.Add(file);
                    }
                }
            }
            public static void ReadPlaylist(string playlistPath)
            {
                // MAKEE THESE FIELDS IN THE MAIN PROGRAM LATER

                string[] tempplaylist = System.IO.File.ReadAllLines(playlistPath);
                string[][] playlistMusics = new string[tempplaylist.Length][];   // these 2 have the same indices
                Image[][] allMusicArts = new Image[tempplaylist.Length][];       // these 2 have the same indices, so can use the index of the music that is playing or selected to show the pic

                //string tempread = playlistReader.ReadLine();
                for (int i = 0; i <= tempplaylist.Length; i++)
                {
                    string[] musicElements = new string[7];
                    string[] tempMusicElements = tempplaylist[i].Split('|', StringSplitOptions.None);
                    for (int x = 0; x <= tempMusicElements.Length; x++)
                    {
                        switch (x)
                        {
                            case 1:
                                musicElements[0] = tempMusicElements[1];
                                break;
                            case 3:
                                musicElements[1] = tempMusicElements[3];
                                break;
                            case 5:
                                musicElements[2] = tempMusicElements[5];
                                break;
                            case 7:
                                musicElements[3] = tempMusicElements[7];
                                break;
                            case 9:
                                musicElements[4] = tempMusicElements[9];
                                break;
                            case 11:
                                musicElements[5] = tempMusicElements[11];
                                break;
                            case 13:
                                musicElements[6] = tempMusicElements[13];
                                break;
                        }


                    }
                    playlistMusics[i] = musicElements;



                    TagLib.File musicTags = TagLib.File.Create(tempplaylist[i]);

                    var tempPics = musicTags.Tag.Pictures;
                    Image art;
                    if (tempPics.Length == 0)
                    {
                        //ShellFile shellfile = ShellFile.FromFilePath(musicElements[4]);
                        //art = shellfile.Thumbnail.ExtraLargeBitmap;
                        art = Properties.Resources.Khi_Player_Thumbnail;
                    }
                    else
                    {
                        using (MemoryStream picConverter = new MemoryStream(tempPics[0].Data.Data))
                        {
                            art = Image.FromStream(picConverter);
                        }
                    }
                    Image[] musicArt = { art };
                    allMusicArts[i] = musicArt;
                }

                // now PlaylistMusics contains all the music data written in the playlistName.txt

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
            public static string[][]? allmusics;
            //public static Image[]? allarts;
            public static string[]? allArtsPath;
            //public ListView currentPlaylist;
            public static AudioFileReader song;
            public static WaveOutEvent mediaPlayer = new WaveOutEvent();
            public static string playerState { get; set; }   // should have "setup" "playing" , "paused", "stopped", or "finished" value.


            public PlayBackFunction()
            {
                switch (Form1.CurrentPlaylist)
                {
                    case Playlists.allSongs:
                        PlaylistQueue = allMusicInfo;
                        break;

                    case Playlists.playlist1:
                        PlaylistQueue = playlist1MusicInfo;
                        break;

                    case Playlists.playlist2:
                        PlaylistQueue = playlist2MusicInfo;
                        break;

                    case Playlists.playlist3:
                        PlaylistQueue = playlist3MusicInfo;
                        break;

                    case Playlists.playlist4:
                        PlaylistQueue = playlist4MusicInfo;
                        break;

                    case Playlists.playlist5:
                        PlaylistQueue = playlist5MusicInfo;
                        break;


                    default: //the default is allSongs
                        PlaylistQueue = allMusicInfo;
                        break;

                }

            }

            /*  
              private static void MediaPlayer_PlaybackStopped(object? sender, StoppedEventArgs e)
              {
                  if (Status == States.Finished)
                  {
                      selectedMusicsQue++;
                      PlayBackFunction.MusicPlayBackControl(Controls.PlayPause);
                  }
              }*/

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
                //allmusics = null;
                //allArtsPath = null;
                mediaPlayer.Dispose();
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
                        //allmusics = null;
                        //allArtsPath = null;
                        mediaPlayer.Dispose();
                    }

                    // Dispose unmanaged resources here.
                    //allmusics = null;
                    //allArtsPath = null;
                    mediaPlayer.Dispose();
                }

                disposed = true;
                GC.Collect();
            }


            ///<summary>
            ///To Play, Stop, Skip and go back (Previous) multiple sound file based on selected items
            ///</summary>summary>
            public static void MusicPlayBackControl(Controls PressedControl)
            {

                //mediaPlayer.PlaybackStopped += MediaPlayer_PlaybackStopped;

                if (listUpdated == true)
                {
                    switch (Form1.CurrentPlaylist)
                    {
                        case Playlists.allSongs:
                            PlaylistQueue = allMusicInfo;
                            break;

                        case Playlists.playlist1:
                            PlaylistQueue = playlist1MusicInfo;
                            break;

                        case Playlists.playlist2:
                            PlaylistQueue = playlist2MusicInfo;
                            break;

                        case Playlists.playlist3:
                            PlaylistQueue = playlist3MusicInfo;
                            break;

                        case Playlists.playlist4:
                            PlaylistQueue = playlist4MusicInfo;
                            break;

                        case Playlists.playlist5:
                            PlaylistQueue = playlist5MusicInfo;
                            break;

                        case Playlists.searchPlaylist:
                            PlaylistQueue = searchPlaylist;
                            break;

                        default: //the default is allSongs
                            PlaylistQueue = allMusicInfo;
                            break;

                    }
                    listUpdated = false;
                }
                /*
                if (currentlySelectedSong == null && currentlyPlayingSongInfo == null)
                {
                    currentlyPlayingSongInfo = PlaylistQueue[0];
                    currentlyPlayingSongPic = Image.FromFile(PlaylistQueue[0][5]);
                }
                */
                //uint numberOfSongsToPlay = (uint)selectedItemsList.Length;
                int playStopNumberCount = 0;

                switch (PressedControl)
                {
                    case Controls.PlayPause:

                        if (mediaPlayer.PlaybackState == PlaybackState.Playing)
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

                                //song = new AudioFileReader(selectedItemsList[selectedMusicsQue][3]);
                                song = new AudioFileReader(PlaylistQueue[currentlySelectedSongIndex][3]);

                                mediaPlayer = new WaveOutEvent();

                                mediaPlayer.DesiredLatency = 100;
                                mediaPlayer.Init(song);
                                mediaPlayer.Play();
                                Status = States.Playing;
                            }



                        }
                        else
                        {

                            //  if (playStopNumberCount > 0) { selectedMusicsQue--; }
                            if (mediaPlayer.PlaybackState == PlaybackState.Paused)
                            {
                                if (currentlyPlayingSongInfo != PlaylistQueue[currentlySelectedSongIndex])
                                {
                                    song.Dispose();
                                    mediaPlayer.Dispose();
                                    currentlyPlayingSongInfo = null;
                                    currentlyPlayingSongPic = null;

                                    //song = new AudioFileReader(selectedItemsList[selectedMusicsQue][3]);
                                    song = new AudioFileReader(PlaylistQueue[currentlySelectedSongIndex][3]);

                                    mediaPlayer = new WaveOutEvent();

                                    mediaPlayer.DesiredLatency = 100;
                                    mediaPlayer.Init(song);
                                    mediaPlayer.Play();
                                    Status = States.Playing;
                                }
                                else
                                {
                                    mediaPlayer.Play();
                                    Status = States.Playing;
                                }
                            }
                            else
                            {
                                //song.Dispose();
                                //mediaPlayer.Dispose();
                                //currentlyPlayingSongInfo = null;
                                //currentlyPlayingSongPic = null;

                                //song = new AudioFileReader(selectedItemsList[selectedMusicsQue][3]);
                                selectedMusicsQue = (uint)currentlySelectedSongIndex;
                                song = new AudioFileReader(PlaylistQueue[currentlySelectedSongIndex][3]);



                                mediaPlayer = new WaveOutEvent();

                                mediaPlayer.DesiredLatency = 100;
                                mediaPlayer.Init(song);
                                mediaPlayer.Play();
                                Status = States.Playing;
                            }

                            playStopNumberCount = 0;

                        }
                        break;

                    case Controls.Skip:


                        song.Dispose();
                        mediaPlayer.Dispose();
                        currentlyPlayingSongInfo = null;
                        currentlyPlayingSongPic = null;
                        if (isLoopEnabled == true)
                        {
                            if (LoopState == LoopStates.SingleSongLoop)
                            {
                                //selectedMusicsQue = 0;
                                //selectedmusicque doesnt change
                            }
                            else
                            {
                                selectedMusicsQue++;
                                if (selectedMusicsQue > PlaylistQueue.Length - 1)
                                { selectedMusicsQue = 0; }
                            }
                            //song = new AudioFileReader(selectedItemsList[selectedMusicsQue][3]);
                            song = new AudioFileReader(PlaylistQueue[selectedMusicsQue][3]);

                            mediaPlayer = new WaveOutEvent();

                            mediaPlayer.DesiredLatency = 100;
                            mediaPlayer.Init(song);
                            mediaPlayer.Play();
                            Status = States.Playing;
                        }
                        else
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

                                //song = new AudioFileReader(selectedItemsList[selectedMusicsQue][3]);
                                song = new AudioFileReader(PlaylistQueue[selectedMusicsQue][3]);

                                mediaPlayer = new WaveOutEvent();

                                mediaPlayer.DesiredLatency = 100;
                                mediaPlayer.Init(song);
                                mediaPlayer.Play();
                                Status = States.Playing;

                            }
                        }

                        break;

                    case Controls.Previous:

                        selectedMusicsQue--;
                        song.Dispose();
                        mediaPlayer.Dispose();
                        currentlyPlayingSongInfo = null;
                        currentlyPlayingSongPic = null;

                        if (isLoopEnabled == true && selectedMusicsQue == 0)
                        {
                            selectedMusicsQue = (uint)PlaylistQueue.Length - 1;
                            //song = new AudioFileReader(selectedItemsList[selectedMusicsQue][3]);
                            song = new AudioFileReader(PlaylistQueue[selectedMusicsQue][3]);

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

                                //song = new AudioFileReader(selectedItemsList[selectedMusicsQue][3]);
                                song = new AudioFileReader(PlaylistQueue[selectedMusicsQue][3]);
                                mediaPlayer = new WaveOutEvent();

                                mediaPlayer.DesiredLatency = 100;
                                mediaPlayer.Init(song);
                                mediaPlayer.Play();
                                Status = States.Playing;

                            }

                        }
                        break;

                }
                //After a music is played, skiped, etc

                if (Status == States.Playing)
                {
                    currentlyPlayingSongInfo = PlaylistQueue[selectedMusicsQue];
                    currentlyPlayingSongPic = Image.FromFile(PlaylistQueue[selectedMusicsQue][4]);
                }


            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Dark Mode
            darkModeMenuItem_Click(sender, e);
            CurrentPlaylist = Playlists.allSongs;
            GC.Collect();

        }

        private void renameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (renameTextBox.TextBox.Text.Length > 0 && renameTextBox.TextBox.Text != "Playlist Name")
                {
                    // rename.TextChanged += rename_TextChanged;
                    string playlistName = renameTextBox.Text;
                    //MyPlayList newPlaylist = new MyPlayList(playlistName);
                    //playlistsToolBar.DropDownItems.Add(newPlaylist.myPlaylist);
                    renameTextBox.TextBox.Clear();
                    renameTextBox.Visible = false;
                    renameTextBox.TextBox.Text = "Playlist Name";

                    switch (nextPlaylistNumber)
                    {
                        case 1:
                            playlist1Button.Text = playlistName;
                            playlist1Button.Visible = true;
                            playlist1Button.Enabled = true;
                            addToPlaylistButton.Enabled = true;
                            addToPlaylistButton.Visible = true;
                            addToPlaylist1.Visible = true;
                            addToPlaylist1.Enabled = true;
                            addToPlaylist1.Text = playlistName;
                            nextPlaylistNumber++;


                            break;

                        case 2:
                            playlist2Button.Text = playlistName;
                            playlist2Button.Visible = true;
                            playlist2Button.Enabled = true;
                            addToPlaylistButton.Enabled = true;
                            addToPlaylistButton.Visible = true;
                            addToPlaylist2.Visible = true;
                            addToPlaylist2.Enabled = true;
                            addToPlaylist2.Text = playlistName;
                            nextPlaylistNumber++;

                            break;

                        case 3:
                            playlist3Button.Text = playlistName;
                            playlist3Button.Visible = true;
                            playlist3Button.Enabled = true;
                            addToPlaylistButton.Enabled = true;
                            addToPlaylistButton.Visible = true;
                            addToPlaylist3.Visible = true;
                            addToPlaylist3.Enabled = true;
                            addToPlaylist3.Text = playlistName;
                            nextPlaylistNumber++;

                            break;

                        case 4:
                            playlist4Button.Text = playlistName;
                            playlist4Button.Visible = true;
                            playlist4Button.Enabled = true;
                            addToPlaylistButton.Enabled = true;
                            addToPlaylistButton.Visible = true;
                            addToPlaylist4.Visible = true;
                            addToPlaylist4.Enabled = true;
                            addToPlaylist4.Text = playlistName;
                            nextPlaylistNumber++;

                            break;

                        case 5:
                            playlist5Button.Text = playlistName;
                            playlist5Button.Visible = true;
                            playlist5Button.Enabled = true;
                            addToPlaylistButton.Enabled = true;
                            addToPlaylistButton.Visible = true;
                            addToPlaylist5.Visible = true;
                            addToPlaylist5.Enabled = true;
                            addToPlaylist5.Text = playlistName;
                            nextPlaylistNumber++;

                            break;
                    }
                    if (nextPlaylistNumber > 5)
                    {
                        //WRITE LATER
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
                //playlistsToolBar.DropDown.Close();

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
            //string[]? artPaths;
            AudioDataBase.RemoveInvalidDatabaseElements();
            (dataBaseInfo, Arts) = FilterDuplicates.CheckDataBaseAndRemoveDuplicates(musicListView);
            allMusicInfo = (string[][])dataBaseInfo.Clone();

            if (dataBaseInfo.Length > 0)
            {
                //allArtFilePaths = (string[])artPaths.Clone();
                int x = 0;
                //musicListView.LargeImageList.Images.AddRange(khiDatabase.AllMusicArts);
                foreach (var music in dataBaseInfo)
                {

                    musicListView.LargeImageList.Images.Add(Arts[x]);
                    //musicListView.SmallImageList.Images.Add(allMusicArts[x]);
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
            //fullMusicPathList.Refresh();
        }

        private void ClearListItem_Click(object sender, EventArgs e)
        {
            musicListView.Items.Clear();
            //fullMusicPathArray.Clear();
            //System.IO.File.WriteAllText(allMusicDataBase, "");
        }

        private async void musicListView_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //e.AllowedEffect = System.Windows.Forms.DragDropEffects.Link;
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
                //allArtFilePaths = (string[]?)khiDatabase.ArtFileNames.Clone();
                var AddedMusicInfo = (string[][]?)khiDatabase.AddedMusicInfo.Clone();
                var AddedMusicArts = (Image[]?)khiDatabase.AddedMusicArts.Clone();
                //var AddedArtFilePaths = (string[]?)khiDatabase.AddedArtFileNames.Clone();
                khiDatabase.Dispose();
                //var tempArts = khiDatabase.AllMusicArts;
                int previousItemCount = musicListView.Items.Count;

                MemoryManageTimer.Stop();
                AddToListView.PopulateListView(ref musicListView, allMusicInfo, allMusicArts, false);

                foreach (string[]? music in AddedMusicInfo)
                {
                    searchMusicListView.AutoCompleteCustomSource.Add(music[0]);
                    searchMusicListView.AutoCompleteCustomSource.Add(music[1]);
                    searchMusicListView.AutoCompleteCustomSource.Add(music[2]);
                }

                listUpdated = true;
                //to dispose

                AddedMusicArts = null;
                AddedMusicInfo = null;
                //AddedArtFilePaths = null;
                draggedFiles = null;
                tempPathList.Clear();
                tempPathList = null;
                tempDraggedFiles = null;
                //khiDatabase.Dispose();

            }
            //AllSongsPlaylist.Items.Clear();

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
            //lyricsTextBox.Size = lyricsTextBox.MinimumSize;
        }

        private void showLyricsMenuItem_Click(object sender, EventArgs e)
        {
            showLyricsMenuItem.Checked = true;
            showAlbumArtMenuItem.Checked = false;
            pictureBox1.Enabled = false;
            pictureBox1.Visible = false;
            lyricsTextBox.Enabled = true;
            lyricsTextBox.Visible = true;
            //lyricsTextBox.Size = lyricsTextBox.MaximumSize;

        }

        private void PlayPause_Click(object sender, EventArgs e)
        {
            /*
            List<string[]> testList = new List<string[]>();
            
                if (selectedItems.Count > 0)
                {
                    if (noSongSelected == false)
                    {

                        selectedItemsList = selectedItems.ToArray();
                        selectedItemsListIndex = selectedItemsIndices.ToArray();

                    }
                    else
                    {

                        selectedItemsList = allMusicInfo;

                        int[] tempIndices = new int[musicListView.Items.Count];
                        for (int n = 0; n <= musicListView.Items.Count - 1; n++)
                        {
                            tempIndices[n] = n;
                        }

                        selectedItemsListIndex = tempIndices;
                        selectedItemsIndices.AddRange(tempIndices);
                        selectedItems.AddRange(allMusicInfo);

                    }

                }
                else //Plays All Songs if no song is selected 
                {    // Can later change into playing the current playlist when that part is done
                    noSongSelected = true;
                    selectedItemsList = allMusicInfo;
                    selectedItems.AddRange(allMusicInfo);
                    int[] tempIndices = new int[musicListView.Items.Count];
                    for (int n = 0; n <= musicListView.Items.Count - 1; n++)
                    {
                        tempIndices[n] = n;
                    }
                    selectedItemsListIndex = tempIndices;
                    selectedItemsIndices.AddRange(tempIndices);
                }
            

            for (int i= 0; i< musicListView.SelectedItems.Count; i++)
            {
                selectedItemsIndices.Add(musicListView.SelectedItems[i].Index);
            }
            */
            //int g = currentlyPlayingSongInfo.Length;
            //PlayBackFunction KhiPlayer = new PlayBackFunction(musicListView);

            PlayBackFunction.MusicPlayBackControl(PlayBackFunction.Controls.PlayPause);
            songLengthLabel.Text = PlayBackFunction.song.TotalTime.ToString("mm\\:ss");
            seekBar.Maximum = Convert.ToInt32(PlayBackFunction.song.TotalTime.TotalSeconds);
            songSeekTimer.Enabled = true;

            borderLabel.Visible = true;
            songTitleLabel.Text = currentlyPlayingSongInfo[0];
            songArtistLabel.Text = currentlyPlayingSongInfo[1];
            songAlbumLabel.Text = currentlyPlayingSongInfo[2];

            lyricsTextBox.Clear();
            string? lyrics;
            using (TagLib.File lyrictag = TagLib.File.Create(currentlyPlayingSongInfo[3]))
            {
                lyrics = lyrictag.Tag.Lyrics;
                if (lyrics != null)
                { lyrics = lyrics.ReplaceLineEndings(); }
                else { lyrics = "Oops! No Embedded Lyrics"; }
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
            /*
                var temp = selectedItems;
                temp = temp.OrderBy(x => x[0]).ToList();
                testList = temp;
                foreach (var test in testList)
                {
                    textBox1.AppendText(test[0] + " \r\n");
                }
            */
        }

        private void skip_Click(object sender, EventArgs e)
        {
            songSeekTimer.Enabled = false;
            PlayBackFunction.MusicPlayBackControl(PlayBackFunction.Controls.Skip);
            songSeekTimer.Enabled = true;
            // ***lyricsTextBox.Text = musicListView.SelectedItems[(int)selectedMusicsQue].SubItems[6].Text;
            if (PlayBackFunction.mediaPlayer.PlaybackState == PlaybackState.Playing || PlayBackFunction.mediaPlayer.PlaybackState == PlaybackState.Paused)
            {
                songTitleLabel.Text = currentlyPlayingSongInfo[0];
                songArtistLabel.Text = currentlyPlayingSongInfo[1];
                songAlbumLabel.Text = currentlyPlayingSongInfo[2];

                lyricsTextBox.Clear();
                string? lyrics;
                using (TagLib.File lyrictag = TagLib.File.Create(currentlyPlayingSongInfo[3]))
                {
                    lyrics = lyrictag.Tag.Lyrics;
                    if (lyrics != null)
                    { lyrics = lyrics.ReplaceLineEndings(); }
                    else { lyrics = "Oops! No Embedded Lyrics"; }
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

            PlayBackFunction.MusicPlayBackControl(PlayBackFunction.Controls.Previous);
            // ***lyricsTextBox.Text = musicListView.SelectedItems[(int)selectedMusicsQue - 2].SubItems[6].Text;

            if (PlayBackFunction.mediaPlayer.PlaybackState == PlaybackState.Playing || PlayBackFunction.mediaPlayer.PlaybackState == PlaybackState.Paused)
            {
                songTitleLabel.Text = currentlyPlayingSongInfo[0];
                songArtistLabel.Text = currentlyPlayingSongInfo[1];
                songAlbumLabel.Text = currentlyPlayingSongInfo[2];

                lyricsTextBox.Clear();
                string? lyrics;
                using (TagLib.File lyrictag = TagLib.File.Create(currentlyPlayingSongInfo[3]))
                {
                    lyrics = lyrictag.Tag.Lyrics;
                    if (lyrics != null)
                    { lyrics = lyrics.ReplaceLineEndings(); }
                    else { lyrics = "Oops! No Embedded Lyrics"; }
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
            //mediaPlayerPanel.BackgroundImage = currentlyPlayingSongPic;
        }

        private void musicListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        { //change this and use musiclistview.selecteditems instead and selecteditems (list)


            using (KhiConverter KhiKotes = new KhiConverter(musicListView))
            {
                var allInfo = KhiKotes.AllInfo;

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
                        //selectedItems.Remove(allInfo[e.ItemIndex]);
                        selectedItems.Clear();
                        selectedItemsList = Array.Empty<string[]>();
                        selectedItemsIndices.Clear();
                        selectedItemsListIndex = Array.Empty<int>();
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
                    selectedItemsList = Array.Empty<string[]>();
                    selectedItemsIndices.Clear();
                    selectedItemsListIndex = Array.Empty<int>();
                    currentlySelectedSong = Array.Empty<string>();
                    noSongSelected = true;
                }

                /*
                if (selectedItems.Count > 0 && selectedItems.Contains(allInfo[e.ItemIndex]))
                {
                    if (noSongSelected == true)
                    {
                        selectedItems.Clear();
                        selectedItemsIndices.Clear();
                        selectedItemsList = Array.Empty<string[]>();
                        selectedItemsListIndex = Array.Empty<int>();
                        currentlySelectedSong = Array.Empty<string>();
                        selectedItemsIndices.Add(e.ItemIndex);
                        selectedItems.Add(allInfo[e.ItemIndex]);
                        //currentlySelectedSong = allInfo[e.ItemIndex];
                        currentlySelectedSong = selectedItems[0];
                        noSongSelected = false;
                    }
                    else
                    {
                        if (selectedItems.Contains(allInfo[e.ItemIndex]))
                        {
                            selectedItems.Remove(allInfo[e.ItemIndex]);
                            selectedItemsIndices.Remove(e.ItemIndex);
                            if (selectedItems.Count == 0)
                            {
                                noSongSelected = true;
                                currentlySelectedSong = Array.Empty<string>();
                            }
                            else
                            {

                                currentlySelectedSong = selectedItems.Last().ToArray();
                            }

                        }

                    }
                }

                else
                {
                    noSongSelected = false;
                    //just to be safe
                    //if (selectedItems.Count !=0 || !selectedItems.Contains(allInfo[e.ItemIndex]))
                    //{
                        selectedItemsIndices.Add(e.ItemIndex);
                        selectedItems.Add(allInfo[e.ItemIndex]);
                        currentlySelectedSong = selectedItems[0];
                    //}
                    //selectedItems.Clear();
                    //selectedItemsIndices.Clear();
                   
                    //currentlySelectedSong = allInfo[e.ItemIndex];
                    //int fakeFullMusicPathListCountSelected = fakeFullMusicPathList.SelectedItems.Count;
                }*/
            }
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
                    AddToListView.PopulateListView(ref musicListView, AddedMusicInfo, AddedMusicArts, false);

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
                    selectedAudioFileInfo.art);

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
                    /* for (int i = 0; i < clickedItem.SubItems.Count; i++)
                     {
                         currentlySelectedSong[i] = clickedItem.SubItems[i].ToString();
                     }
                    */
                    using (KhiConverter KhiKote = new KhiConverter(musicListView))
                    {
                        currentlySelectedSong = KhiKote.AllInfo[clickedItem.Index];
                    }
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
                        if (PlayBackFunction.mediaPlayer.PlaybackState != PlaybackState.Playing || PlayBackFunction.mediaPlayer.PlaybackState != PlaybackState.Paused)
                        {
                            musicListView.SelectedItems.Clear();
                            selectedItems.Clear();
                            selectedItemsIndices.Clear();
                            selectedItemsList = Array.Empty<string[]>();
                            selectedItemsListIndex = Array.Empty<int>();
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

        private void removeItemButton_Click(object sender, EventArgs e)
        {
            //musicListView.SuspendLayout();
            GC.Collect();
            bool wasPlaying = false;
            musicListView.BeginUpdate();
            int i = musicListView.Items.Find(currentlySelectedSong[3], false)[0].Index;
            var pTest = allMusicInfo[i].Clone();
            var item = (string[])currentlySelectedSong.Clone();
            if (currentlyPlayingSongInfo == currentlySelectedSong)
            {
                wasPlaying = true;
                PlayBackFunction.mediaPlayer.Stop();
                PlayBackFunction.song.Dispose();
                currentlyPlayingSongPic = null;

            }

            musicListView.Items.RemoveAt(i);
            musicListView.LargeImageList.Images.RemoveAt(i);
            bool removed = AudioDataBase.RemoveSong(item, allMusicInfo[i][4], allMusicInfo[i][5]);

            /*
                        if (removed)
                        {
                            musicListView.Items.RemoveAt(i);
                        }
            */
            var tempAllMusics = allMusicInfo.ToList();
            tempAllMusics.RemoveAt(i);
            allMusicInfo = tempAllMusics.ToArray();

            //for (int x = 0; x< musicListView.Items.Count; x++)
            //{

            //}
            musicListView.EndUpdate();
            listUpdated = true;
            //musicListView.ResumeLayout();
            tempAllMusics.Clear();
            tempAllMusics = null;
            var test = allMusicInfo[i];
            int l = 0;
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
                selectedItemsList = Array.Empty<string[]>();
                selectedItemsListIndex = Array.Empty<int>();
                currentlySelectedSong = Array.Empty<string>();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

            GC.Collect();
            //textBox1.Text = musicListView.Items[0].Name;
        }

        private void showLargeIconMenuItem_Click(object sender, EventArgs e)
        {
            showLargeIconMenuItem.Checked = true;
            showDetailsMenuItem.Checked = false;
            showTileViewMenuItem.Checked = false;
            musicListView.View = View.LargeIcon;
            musicListView.Refresh();
        }

        private void showTileViewMenuItem_Click(object sender, EventArgs e)
        {
            showDetailsMenuItem.Checked = false;
            showLargeIconMenuItem.Checked = false;
            showTileViewMenuItem.Checked = true;
            musicListView.View = View.Tile;
        }

        private void showDetailsMenuItem_Click(object sender, EventArgs e)
        {
            showDetailsMenuItem.Checked = true;
            showLargeIconMenuItem.Checked = false;
            showTileViewMenuItem.Checked = false;
            musicListView.View = View.Details;
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
                if (PlayBackFunction.song.CurrentTime.TotalSeconds <= PlayBackFunction.song.TotalTime.TotalSeconds)
                {
                    currenSongTimePosition = PlayBackFunction.song.CurrentTime.ToString("mm\\:ss");

                    seekBar.Value = Convert.ToInt32(PlayBackFunction.song.CurrentTime.TotalSeconds);
                    currentTimeLabel.Text = currenSongTimePosition;
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

            seekBar.SuspendLayout();
        }

        private void seekBar_MouseUp(object sender, MouseEventArgs e)
        {
            int tempLength = Convert.ToInt32(PlayBackFunction.song.TotalTime.TotalSeconds);

            PlayBackFunction.song.Skip(seekBarFinalValue - seekBarValueBeforeMove);
            seekBar.ResumeLayout();
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

            //musicBrowser.ShowDialog();
            //musicBrowser.Filter = "Audio Files | *.mp3; *.wav; *.flac; *.aiff; *.wma; *.pcm; *.aac; *.oog; *.alac";


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


                //int index = tempPathsList.Count;
                string[] addedFiles = tempPathsList.ToArray();
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
                    AddToListView.PopulateListView(ref musicListView, AddedMusicInfo, AddedMusicArts, false);

                    foreach (string[]? music in AddedMusicInfo)
                    {
                        searchMusicListView.AutoCompleteCustomSource.Add(music[0]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[1]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[2]);
                    }

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
                this.BackColor = Color.FromArgb(41, 41, 41);
                this.ForeColor = Color.White;

                foreach (Control cont in this.Controls)
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
                foreach (ToolStripItem cont in rightClickMenu.Items)
                {
                    cont.BackColor = Color.FromArgb(41, 41, 41);
                    cont.ForeColor = Color.White;
                }

                //for Icons

                toggleLoop.BackgroundImage = Properties.Resources.Loop_Dark_Mode;
                PlayPause.BackgroundImage = Properties.Resources.Play_Pause__Dark_Mode;
                skip.BackgroundImage = Properties.Resources.Skip_Dark_Mode;
                previous.BackgroundImage = Properties.Resources.Previous_Dark_Mode;

                isDarkMode = true;
            }
            //turns dark mode to light mode
            else
            {
                this.BackColor = Color.White;
                this.ForeColor = Color.FromArgb(41, 41, 41);
                foreach (Control cont in this.Controls)
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
                foreach (ToolStripItem cont in rightClickMenu.Items)
                {
                    cont.BackColor = Color.White;
                    cont.ForeColor = Color.FromArgb(41, 41, 41);
                }

                //For Icons

                toggleLoop.BackgroundImage = Properties.Resources.loop;
                PlayPause.BackgroundImage = Properties.Resources.Play_Pause;
                skip.BackgroundImage = Properties.Resources.Skip;
                previous.BackgroundImage = Properties.Resources.Previous;

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

        private void playlist1Button_Click(object sender, EventArgs e)
        {
            string? name;
            Image[]? playlist1Images;
            if (System.IO.File.Exists(playlist1Path))
            {
                (name, playlist1MusicInfo, playlist1Images) = AudioDataBase.ReadPlaylistDataBase(playlist1Path, "complete");
                if (playlist1MusicInfo != null && playlist1MusicInfo.Length > 0)
                {
                    musicListView.Items.Clear();
                    musicListView.LargeImageList.Images.Clear();
                    AddToListView.PopulateListView(ref musicListView, playlist1MusicInfo, playlist1Images);

                    searchMusicListView.AutoCompleteCustomSource.Clear();

                    foreach (string[]? music in playlist1MusicInfo)
                    {
                        searchMusicListView.AutoCompleteCustomSource.Add(music[0]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[1]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[2]);
                    }
                    //musicListView.EndUpdate();
                    CurrentPlaylist = Playlists.playlist1;
                    listUpdated = true;
                }
                GC.Collect();
            }
        }

        private void addToPlaylist1_Click(object sender, EventArgs e)
        {
            string playlistName = playlist1Button.Text;
            List<string[]> tempPlaylistItems = new List<string[]>();
            /* if (musicListView.SelectedItems.Count == 1)
             {
                 string[]? tempPlaylistItem = new string[1];
                 tempPlaylistItem = (string[]?)allMusicInfo[musicListView.SelectedItems[0].Index].Clone();
                 AudioDataBase.WriteSongToPlaylistDatabase(tempPlaylistItem, 1, playlistName);
             }
             else*/
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
                    for (int i = 0; i < musicListView.SelectedItems.Count; i++)
                    {
                        string? itemPath = musicListView.SelectedItems[i].Name;
                        if (itemPath != "")
                        {
                            foreach (string[]? musicInfo in allMusicInfo)
                            {
                                if (musicInfo[3] == itemPath)
                                {
                                    tempPlaylistItems.Add(musicInfo);
                                }
                            }
                        }
                    }
                }
                AudioDataBase.WriteAudioDataBase(tempPlaylistItems.ToArray(), 1, playlistName);
            }
            else
            {
                //string[]? tempPlaylistItem = new string[6];
                //tempPlaylistItem = allMusicInfo[musicListView.SelectedItems[0].Index];
                //AudioDataBase.WriteSongToPlaylistDatabase(tempPlaylistItem, 1, playlistName);
            }

            //to dispose of
            tempPlaylistItems = null;
            playlistName = null;
        }

        private void addToPlaylist2_Click(object sender, EventArgs e)
        {
            string playlistName = playlist2Button.Text;
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
                    for (int i = 0; i < musicListView.SelectedItems.Count; i++)
                    {
                        string? itemPath = musicListView.SelectedItems[i].Name;
                        if (itemPath != "")
                        {
                            foreach (string[]? musicInfo in allMusicInfo)
                            {
                                if (musicInfo[3] == itemPath)
                                {
                                    tempPlaylistItems.Add(musicInfo);
                                }
                            }
                        }
                    }
                }

                AudioDataBase.WriteAudioDataBase(tempPlaylistItems.ToArray(), 2, playlistName);
            }
            else
            {
                // string[]? tempPlaylistItem = new string[6];
                // tempPlaylistItem = allMusicInfo[musicListView.SelectedItems[0].Index];
                // AudioDataBase.WriteSongToPlaylistDatabase(tempPlaylistItem, 2, playlistName);
            }

            //to dispose of
            tempPlaylistItems = null;
            playlistName = null;
        }

        private void addToPlaylist3_Click(object sender, EventArgs e)
        {
            string playlistName = playlist3Button.Text;
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
                    for (int i = 0; i < musicListView.SelectedItems.Count; i++)
                    {
                        string? itemPath = musicListView.SelectedItems[i].Name;
                        if (itemPath != "")
                        {
                            foreach (string[]? musicInfo in allMusicInfo)
                            {
                                if (musicInfo[3] == itemPath)
                                {
                                    tempPlaylistItems.Add(musicInfo);
                                }
                            }
                        }
                    }
                }
                AudioDataBase.WriteAudioDataBase(tempPlaylistItems.ToArray(), 3, playlistName);
            }
            else
            {
                //string[]? tempPlaylistItem = new string[6];
                //tempPlaylistItem = allMusicInfo[musicListView.SelectedItems[0].Index];
                //AudioDataBase.WriteSongToPlaylistDatabase(tempPlaylistItem, 3, playlistName);
            }

            //to dispose of
            tempPlaylistItems = null;
            playlistName = null;
        }

        private void addToPlaylist4_Click(object sender, EventArgs e)
        {
            string playlistName = playlist4Button.Text;
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
                    for (int i = 0; i < musicListView.SelectedItems.Count; i++)
                    {
                        string? itemPath = musicListView.SelectedItems[i].Name;
                        if (itemPath != "")
                        {
                            foreach (string[]? musicInfo in allMusicInfo)
                            {
                                if (musicInfo[3] == itemPath)
                                {
                                    tempPlaylistItems.Add(musicInfo);
                                }
                            }
                        }
                    }
                }
                AudioDataBase.WriteAudioDataBase(tempPlaylistItems.ToArray(), 4, playlistName);
            }
            else
            {
                //string[]? tempPlaylistItem = new string[6];
                //tempPlaylistItem = allMusicInfo[musicListView.SelectedItems[0].Index];
                //AudioDataBase.WriteSongToPlaylistDatabase(tempPlaylistItem, 4, playlistName);
            }

            //to dispose of
            tempPlaylistItems = null;
            playlistName = null;
        }

        private void addToPlaylist5_Click(object sender, EventArgs e)
        {
            string playlistName = playlist5Button.Text;
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
                    for (int i = 0; i < musicListView.SelectedItems.Count; i++)
                    {
                        string? itemPath = musicListView.SelectedItems[i].Name;
                        if (itemPath != "")
                        {
                            foreach (string[]? musicInfo in allMusicInfo)
                            {
                                if (musicInfo[3] == itemPath)
                                {
                                    tempPlaylistItems.Add(musicInfo);
                                }
                            }
                        }
                    }
                }
                AudioDataBase.WriteAudioDataBase(tempPlaylistItems.ToArray(), 5, playlistName);
            }
            else
            {
                //string[]? tempPlaylistItem = new string[6];
                //tempPlaylistItem = allMusicInfo[musicListView.SelectedItems[0].Index];
                //AudioDataBase.WriteSongToPlaylistDatabase(tempPlaylistItem, 5, playlistName);
            }

            //to dispose of
            tempPlaylistItems = null;
            playlistName = null;
        }

        private async void allSongsPlaylist_Click(object sender, EventArgs e)
        {
            //define a public static enum for currently viewed playlist
            listUpdated = true;
            CurrentPlaylist = Playlists.allSongs;

            allMusicInfo = ReadAudioDataBase("complete");
            Image[]? arts = GetMusicThumbnails(allMusicInfo);
            musicListView.Items.Clear();
            musicListView.LargeImageList.Images.Clear();

            AddToListView.PopulateListView(ref musicListView, allMusicInfo, arts);

            searchMusicListView.AutoCompleteCustomSource.Clear();


            foreach (string[]? music in allMusicInfo)
            {
                searchMusicListView.AutoCompleteCustomSource.Add(music[0]);
                searchMusicListView.AutoCompleteCustomSource.Add(music[1]);
                searchMusicListView.AutoCompleteCustomSource.Add(music[2]);
            }

            GC.Collect();

        }

        private void searchMusicListView_TextChanged(object sender, EventArgs e)
        {
            /*
            string? searchWord = searchMusicListView.Text;
            List<int> originalIndices = new List<int>();
            if (musicListView.Items.Count > 0)
            {
                if (searchWord != "")
                {
                    ListViewItem[]? foundItems = musicListView.Items.Find(searchWord, true);

                    if (foundItems.Length == 1)
                    {
                        musicListView.EnsureVisible(foundItems[0].Index);
                    }
                    if (foundItems.Length > 1)
                    {
                        foreach (var item in foundItems)
                        {
                            originalIndices.Add(item.Index);
                            musicListView.TopItem = item;
                        }


                    }
                    //musicListView.TopItem = musicListView.Items.Find(searchWord, true);
                    //should first, copy all of the items for backup

                    //then should show the found item as top item and i could optionally add codes for the list to scroll
                    //until the found item is in the middle of the screen

                    //could also show found items as drop down menu in the text box, or auto completed text
                }
            }



            if (searchMusicListView.Text == "" || searchMusicListView.Text == searchMusicListView.PlaceholderText)
            {
                //for now as a place Holder, will later write sth else
                allMusicInfo = ReadAudioDataBase("complete");
                Image[]? arts = GetMusicThumbnails(allMusicInfo);
                musicListView.Items.Clear();
                musicListView.LargeImageList.Images.Clear();
                AddToListView.PopulateListView(ref musicListView, allMusicInfo, arts);

                searchMusicListView.AutoCompleteCustomSource.Clear();

                foreach (string[]? music in allMusicInfo)
                {
                    searchMusicListView.AutoCompleteCustomSource.Add(music[0]);
                    searchMusicListView.AutoCompleteCustomSource.Add(music[1]);
                    searchMusicListView.AutoCompleteCustomSource.Add(music[2]);
                }

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

            if (e.KeyValue == (int)Keys.Enter)
            {
                //musicListView.Focus();
                List<int> originalIndices = new List<int>();

                if (searchWord != "")
                {
                    List<string[]?> allMusicInfoList = allMusicInfo.ToList();
                    List<int> foundItemsIndices = new List<int>();
                    //ListViewItem[]? foundItems = musicListView.Items.Find(searchWord, true);
                    int i = 0;
                    foreach (string[]? item in allMusicInfoList)
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
                    /*
                    if (foundItemsIndices.Count == 1)
                    {
                        musicListView.EnsureVisible(foundItemsIndices[0]);
                        
                    } */
                    if (foundItemsIndices.Count >= 1)
                    {
                        List<string[]?> foundItems = new List<string[]>();
                        List<Image?> foundItemsThumbnails = new List<Image?>();

                        foreach (var itemIndex in foundItemsIndices)
                        {
                            foundItems.Add(allMusicInfoList[itemIndex]);
                            foundItemsThumbnails.Add((Image?)musicListView.LargeImageList.Images[itemIndex].Clone());
                        }
                        searchPlaylist = foundItems.ToArray();
                        musicListView.Items.Clear();

                        //musicListView.LargeImageList.Images.Clear();

                        AddToListView.PopulateListView(ref musicListView, foundItems.ToArray(), foundItemsThumbnails.ToArray());

                        /* foreach (var itemIndex in foundItemsIndices)
                        {
                            originalIndices.Add(itemIndex);
                            musicListView.TopItem = musicListView.Items[itemIndex];
                        }*/


                    }
                    listUpdated = true;
                    CurrentPlaylist = Playlists.searchPlaylist;
                }

            }

            else if (e.KeyValue == (int)Keys.Escape || (searchMusicListView.Modified && searchWord == ""))
            {
                allMusicInfo = ReadAudioDataBase("complete");
                Image[]? arts = GetMusicThumbnails(allMusicInfo);
                musicListView.Items.Clear();
                musicListView.LargeImageList.Images.Clear();
                AddToListView.PopulateListView(ref musicListView, allMusicInfo, arts);
                musicListView.Focus();
                searchMusicListView.AutoCompleteCustomSource.Clear();

                foreach (string[]? music in allMusicInfo)
                {
                    searchMusicListView.AutoCompleteCustomSource.Add(music[0]);
                    searchMusicListView.AutoCompleteCustomSource.Add(music[1]);
                    searchMusicListView.AutoCompleteCustomSource.Add(music[2]);
                }
                listUpdated = true;
                CurrentPlaylist = Playlists.allSongs;
                GC.Collect();
            }

        }

        private void playlist2Button_Click(object sender, EventArgs e)
        {
            string? name;
            Image[]? playlist2Images;
            if (System.IO.File.Exists(playlist2Path))
            {
                (name, playlist2MusicInfo, playlist2Images) = AudioDataBase.ReadPlaylistDataBase(playlist2Path, "complete");
                if (playlist2MusicInfo != null && playlist2MusicInfo.Length > 0)
                {
                    musicListView.Items.Clear();
                    musicListView.LargeImageList.Images.Clear();
                    AddToListView.PopulateListView(ref musicListView, playlist2MusicInfo, playlist2Images);

                    searchMusicListView.AutoCompleteCustomSource.Clear();

                    foreach (string[]? music in playlist2MusicInfo)
                    {
                        searchMusicListView.AutoCompleteCustomSource.Add(music[0]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[1]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[2]);
                    }
                    //musicListView.EndUpdate();
                    CurrentPlaylist = Playlists.playlist2;
                    listUpdated = true;
                }
                GC.Collect();
            }
        }

        private void playlist3Button_Click(object sender, EventArgs e)
        {
            string? name;
            Image[]? playlist3Images;
            if (System.IO.File.Exists(playlist3Path))
            {
                (name, playlist3MusicInfo, playlist3Images) = AudioDataBase.ReadPlaylistDataBase(playlist3Path, "complete");
                if (playlist3MusicInfo != null && playlist3MusicInfo.Length > 0)
                {
                    musicListView.Items.Clear();
                    musicListView.LargeImageList.Images.Clear();
                    AddToListView.PopulateListView(ref musicListView, playlist3MusicInfo, playlist3Images);

                    searchMusicListView.AutoCompleteCustomSource.Clear();

                    foreach (string[]? music in playlist3MusicInfo)
                    {
                        searchMusicListView.AutoCompleteCustomSource.Add(music[0]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[1]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[2]);
                    }
                    //musicListView.EndUpdate();
                    CurrentPlaylist = Playlists.playlist3;
                    listUpdated = true;
                }
                GC.Collect();
            }
        }

        private void playlist4Button_Click(object sender, EventArgs e)
        {
            string? name;
            Image[]? playlist4Images;
            if (System.IO.File.Exists(playlist4Path))
            {
                (name, playlist4MusicInfo, playlist4Images) = AudioDataBase.ReadPlaylistDataBase(playlist4Path, "complete");
                if (playlist4MusicInfo != null && playlist4MusicInfo.Length > 0)
                {
                    musicListView.Items.Clear();
                    musicListView.LargeImageList.Images.Clear();
                    AddToListView.PopulateListView(ref musicListView, playlist4MusicInfo, playlist4Images);

                    searchMusicListView.AutoCompleteCustomSource.Clear();

                    foreach (string[]? music in playlist4MusicInfo)
                    {
                        searchMusicListView.AutoCompleteCustomSource.Add(music[0]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[1]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[2]);
                    }
                    //musicListView.EndUpdate();
                    CurrentPlaylist = Playlists.playlist4;
                    listUpdated = true;
                }
                GC.Collect();
            }
        }

        private void playlist5Button_Click(object sender, EventArgs e)
        {
            string? name;
            Image[]? playlist5Images;
            if (System.IO.File.Exists(playlist5Path))
            {
                (name, playlist5MusicInfo, playlist5Images) = AudioDataBase.ReadPlaylistDataBase(playlist5Path, "complete");
                if (playlist5MusicInfo != null && playlist5MusicInfo.Length > 0)
                {
                    musicListView.Items.Clear();
                    musicListView.LargeImageList.Images.Clear();
                    AddToListView.PopulateListView(ref musicListView, playlist5MusicInfo, playlist5Images);

                    searchMusicListView.AutoCompleteCustomSource.Clear();

                    foreach (string[]? music in playlist5MusicInfo)
                    {
                        searchMusicListView.AutoCompleteCustomSource.Add(music[0]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[1]);
                        searchMusicListView.AutoCompleteCustomSource.Add(music[2]);
                    }
                    //musicListView.EndUpdate();
                    CurrentPlaylist = Playlists.playlist5;
                    listUpdated = true;
                }
                GC.Collect();
            }
        }


        private void sortListArtistMenuItem_Click(object sender, EventArgs e)
        {
            if (allMusicInfo != null && allMusicInfo[0] != null)
            {
                List<string[]?> tempList = new List<string[]?>();

                Array.Sort(allMusicInfo, (x, y) => x[1].CompareTo(y[1]));

                musicListView.Items.Clear();
                musicListView.LargeImageList.Images.Clear();
                Image[]? tempAllArts = AudioDataBase.GetMusicThumbnails(allMusicInfo);
                AddToListView.PopulateListView(ref musicListView, allMusicInfo, tempAllArts);
                listUpdated = true;

            }
        }

        private void sortListTitleMenuItem_Click(object sender, EventArgs e)
        {
            if (allMusicInfo != null && allMusicInfo[0] != null)
            {
                List<string[]?> tempList = new List<string[]?>();

                Array.Sort(allMusicInfo, (x, y) => x[0].CompareTo(y[0]));

                musicListView.Items.Clear();
                musicListView.LargeImageList.Images.Clear();
                Image[]? tempAllArts = AudioDataBase.GetMusicThumbnails(allMusicInfo);
                AddToListView.PopulateListView(ref musicListView, allMusicInfo, tempAllArts);
                listUpdated = true;

            }
        }

        private void sortListAlbumMenuItem_Click(object sender, EventArgs e)
        {
            if (allMusicInfo != null && allMusicInfo[0] != null)
            {
                List<string[]?> tempList = new List<string[]?>();

                Array.Sort(allMusicInfo, (x, y) => x[2].CompareTo(y[2]));

                musicListView.Items.Clear();
                musicListView.LargeImageList.Images.Clear();
                Image[]? tempAllArts = AudioDataBase.GetMusicThumbnails(allMusicInfo);
                AddToListView.PopulateListView(ref musicListView, allMusicInfo, tempAllArts);
                listUpdated = true;

            }
        }
    }

}
