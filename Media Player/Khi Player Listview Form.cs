using Microsoft.WindowsAPICodePack.Shell;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Dynamic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using Application = System.Windows.Forms.Application;




namespace Khi_Player
{



    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
            //musicListView.View = View.SmallIcon;
            /*
                        musicListView.SmallImageList = new ImageList
                         {
                             ImageSize = new System.Drawing.Size(60, 60)
                         };
            */
            musicListView.LargeImageList = new ImageList
            {
                ImageSize = new System.Drawing.Size(60, 60)
            };

            //(var tempInfo, var tempArts) = AudioDataBase.ReadAudioDataBase();
            //musicListView = AllSongsPlaylist;

            if (File.Exists(allMusicDataBase))
            {
                AudioDataBase khiDatabase = new AudioDataBase();
                if (khiDatabase.AllMusicInfo.Length > 0)
                {

                    allMusicInfo = (string[][]?)khiDatabase.AllMusicInfo.Clone();
                    allMusicArts = (Image[]?)khiDatabase.AllMusicArts.Clone();
                    allArtFilePaths = (string[]?)khiDatabase.ArtFileNames.Clone();
                    khiDatabase.Dispose();
                    //var tempArts = khiDatabase.AllMusicArts;

                    musicListView.BeginUpdate();
                    musicListView.Items.Clear();
                    int x = 0;
                    //musicListView.LargeImageList.Images.AddRange(khiDatabase.AllMusicArts);
                    foreach (var music in allMusicInfo)
                    {

                        //musicListView.SmallImageList.Images.Add(Icon.ExtractAssociatedIcon(khiDatabase.ArtFileNames[x]));
                        //musicListView.LargeImageList.Images.Add(Image.FromFile(khiDatabase.ArtFileNames[x]));
                        musicListView.LargeImageList.Images.Add(allMusicArts[x]);
                        //musicListView.SmallImageList.Images.Add(allMusicArts[x]);
                        ListViewItem song = new ListViewItem(music, x);
                        song.ToolTipText = music[0] + System.Environment.NewLine + music[1] + System.Environment.NewLine + music[2];
                        song.Name = music[3];
                        //song.BeginEdit();


                        musicListView.Items.Add(song);

                        song = null;
                        x++;
                    }
                    musicListView.EndUpdate();
                    KhiPlayer = new PlayBackFunction(musicListView);

                    allMusicArts = null;
                    //musicListView.Refresh();
                    //tempArts = null;
                    //tempInfo = null;
                    //khiDatabase.Dispose();
                    //musicListView.Enabled = false;
                    //GC.Collect();


                }
            }
        }

        //static ListView? AllSongsPlaylist = new ListView();
        static string[][]? allMusicInfo = new string[1][]; //just for initilization
        static Image[]? allMusicArts = new Image[1]; //same thing
        static string[]? allArtFilePaths = new string[1];
        /*
        static ListView playlist1 = new ListView();
        static string[][]? playlist1MusicInfo;
        static Image[][]? playlist1MusicArts;

        static ListView playlist2 = new ListView();
        static string[][]? playlist2MusicInfo;
        static Image[][]? playlist2MusicArts;

        static ListView playlist3 = new ListView();
        static string[][]? playlist3MusicInfo;
        static Image[][]? playlist3MusicArts;

        static ListView playlist4 = new ListView();
        static string[][]? playlist4MusicInfo;
        static Image[][]? playlist4MusicArts;

        static ListView playlist5 = new ListView();
        static string[][]? playlist5MusicInfo;
        static Image[][] playlist5MusicArts;
        */
        static int nextPlaylistNumber = Math.Clamp(1, 1, 5);


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
        static string[][] selectedItemsList;
        static int[] selectedItemsListIndex;

        static List<string[]> selectedItems = new List<string[]>();
        static List<int>? selectedItemsIndices = new List<int>();

        static string[] currentlySelectedSong = new string[4];

        static string[] currentlyPlayingSongInfo = new string[4];
        static Image? currentlyPlayingSongPic;

        //public static object[][]? musicsProperties;

        static bool isShuffleEnabled = false;
        static bool isLoopEnabled = false;
        static bool noSongSelected = false;
        static bool listUpdated = false;

        PlayBackFunction KhiPlayer;
        //static string[][]? testallmusic;
        //public static Khi_Player_Audio_Info_Form audioInfoPage = new Khi_Player_Audio_Info_Form();
        //static AudioDataBase MyDataBase = new AudioDataBase();



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
                    MusicDataBase.Load(allMusicDataBase);
                    using (StreamReader duplicateChecker = new StreamReader(allMusicDataBase, Encoding.UTF8))
                    {
                        XmlNodeList? tempFileData = MusicDataBase.DocumentElement.ChildNodes;

                        if (tempFileData.Count > 0)
                        {
                            //int i = 0;
                            foreach (var music in selectedMusicsInfo) // To check if the added file path already exists in the txt
                            {
                                bool isDuplicate = false;
                                for (int i = 0; i < tempFileData.Count; i++)
                                {  //CHANGE THIS 

                                    if (tempFileData.Item(i).ChildNodes.Item(0).InnerText == music[0] &&
                                        tempFileData.Item(i).ChildNodes.Item(3).InnerText == music[3] &&
                                        tempFileData.Item(i).ChildNodes.Item(1).InnerText == music[1] &&
                                        tempFileData.Item(i).ChildNodes.Item(2).InnerText == music[2])
                                    {
                                        isDuplicate = true;
                                        //filesList.Add(music);
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
                        //to dispose
                        tempFileData = null;
                        MusicDataBase.XmlResolver = null;
                    }
                }

                else
                {
                    selectedMusicsData = (string[][]?)selectedMusicsInfo.Clone();
                }

                //to dispose
                selectedMusicsInfo = null;
                filesList.Clear();
                filesList = null;

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
        }

        ///<summary> 
        /// Allows writing Dragged files' paths into a txt file and extraction of their properties
        /// </summary>
        public class AudioDataBase : IDisposable
        {
            public string[][]? AllMusicInfo { get; set; }
            public System.Drawing.Image[]? AllMusicArts { get; set; }
            public string[] ArtFileNames { get; set; }
            public string[][]? AddedMusicInfo { get; set; }
            public System.Drawing.Image[]? AddedMusicArts { get; set; }
            public string[] AddedArtFileNames { get; set; }
            public static int AddedSongCount = 0;
            public bool isDataBaseRead { get; internal set; }

            private bool disposed;
            internal static string applicationPath = System.Windows.Forms.Application.StartupPath;
            internal static string albumArtsPath = applicationPath + "Album Arts\\";
            internal static string allMusicDataBase = System.Windows.Forms.Application.StartupPath + "AllMusicDataBase.xml";
            internal static string playlist1Path = System.Windows.Forms.Application.StartupPath + "playlist1DataBase.xml";
            internal static string playlist2Path = System.Windows.Forms.Application.StartupPath + "playlist2DataBase.xml";
            internal static string playlist3Path = System.Windows.Forms.Application.StartupPath + "playlist3DataBase.xml";
            internal static string playlist4Path = System.Windows.Forms.Application.StartupPath + "playlist4DataBase.xml";
            internal static string playlist5Path = System.Windows.Forms.Application.StartupPath + "playlist5DataBase.xml";

            internal string[]? addedMusicsPaths;
            internal static string[][]? addedMusicInfo = new string[1][];
            internal static System.Drawing.Image[]? addedMusicArts = new System.Drawing.Image[1];


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
                    XmlDocument tempMusicDataBase = new XmlDocument();
                    tempMusicDataBase.Load(allMusicDataBase);
                    if (tempMusicDataBase.DocumentElement.ChildNodes.Count > 0)
                    {

                        (AllMusicInfo, ArtFileNames) = ReadAudioDataBase("complete");
                        AllMusicArts = new Image[AllMusicInfo.Length];
                        Image artPic;
                        for (int i = 0; i < AllMusicInfo.Length; i++)
                        {
                            //artPic = Image.FromFile(ArtFileNames[i]);


                            AllMusicArts[i] = Image.FromFile(ArtFileNames[i]);

                        }


                        isDataBaseRead = true;

                    }
                    else
                    { isDataBaseRead = false; }
                }
            }

            ///<summary>
            ///Creates, writes to, and reads from Data Base, using the provided paths of audio files
            ///</summary>
            public AudioDataBase(string[] addSongsPaths)
            {
                addedMusicsPaths = addSongsPaths;
                GetAudioFilesInfo(addSongsPaths);
                //ArtFileNames = WriteAudioDataBase(addedMusicInfo);
                AddedArtFileNames = WriteAudioDataBase(addedMusicInfo);
                GC.Collect();
                //System.IO.File.SetAttributes(allMusicDataBase, System.IO.File.GetAttributes(allMusicDataBase) | FileAttributes.Hidden);
                //(AllMusicInfo, AllMusicArts,ArtFileNames) = ReadAudioDataBase();
                AddedSongCount = AddedArtFileNames.Length;
                (AddedMusicInfo, AddedArtFileNames) = ReadAudioDataBase("added");
                (AllMusicInfo, ArtFileNames) = ReadAudioDataBase("complete");

                AddedMusicArts = new Image[AddedMusicInfo.Length];
                Image artPic;
                for (int i = 0; i < AddedMusicInfo.Length; i++)
                {
                    //artPic = Image.FromFile(AddedArtFileNames[i]);

                    AddedMusicArts[i] = Image.FromFile(AddedArtFileNames[i]);

                }
                /*
                AllMusicArts = new Image[AllMusicInfo.Length];
                Image artimage;
                for (int i = 0; i < AllMusicInfo.Length; i++)
                {
                    artimage = Image.FromFile(ArtFileNames[i]);


                    AllMusicArts[i] = artimage;

                }*/
                isDataBaseRead = true;
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
                        addedMusicArts = null;
                        addedMusicInfo = null;
                        addedMusicsPaths = null;


                    }

                    // TODO: Dispose unmanaged resources here.
                    AllMusicArts = null;
                    AllMusicInfo = null;
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
            public static string[] WriteAudioDataBase(string[][] selectedMusicsInfo)
            {
                string path;
                //string? musicinfo;
                string? title, artist, album;
                string[][]? selectedMusicsData;

                using (FilterDuplicates duplicateCheck = new FilterDuplicates(selectedMusicsInfo))
                {
                    selectedMusicsData = duplicateCheck.selectedMusicsData;
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
                        Image? art;
                        MemoryStream picConverter;
                        if (tempPics.Length > 0)
                        {
                            picConverter = new MemoryStream(tempPics[0].Data.Data);
                            art = (Image)Image.FromStream(picConverter).Clone();

                        }
                        else
                        {
                            //art = Image.FromFile("C:\\Users\\Rushakh\\source\\repos\\Khi Player\\Khi Player\\MusicArt - NoCover.png");
                            art = Khi_Player.Properties.Resources.Khi_Player;
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
                                if (z+1 <= dotcount) { name.AppendText("."); }
                            }
                            fileNames[i] = name.Text;

                        }
                        else 
                        { 
                            dotcount = 1;
                            fileNames[i] = System.IO.Path.GetFileName(path).Split('.')[dotcount - 1];  //actually incomplete so it can be added to albumArtsPath
                        }
                        
                        
                        if (!System.IO.File.Exists(albumArtsPath))
                        {
                            System.IO.Directory.CreateDirectory(albumArtsPath);
                        }
                        string sth = albumArtsPath + fileNames[i] + ".bmp";
                        using (FileStream artSaver = new FileStream(sth, FileMode.Create))
                        {
                            // ImageFormatConverter imageFormatConverter  = new ImageFormatConverter();
                            // var converted =imageFormatConverter.ConvertTo(art, typeof(Bitmap));
                            art.Save(artSaver, art.RawFormat);

                        }
                        fileNames[i] = sth;
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
                        //XmlElement MusicInfo = allMusicDataBaseDoc.CreateElement("string");
                        XmlElement Path = allMusicDataBaseDoc.CreateElement("string");
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
                        Title.InnerText = title;
                        Artist.InnerText = artist;
                        Album.InnerText = album;
                        Path.InnerText = path;
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
                        Song.AppendChild(Title);
                        Song.AppendChild(Artist);
                        Song.AppendChild(Album);
                        Song.AppendChild(Path);

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
                //to dispose
                selectedMusicsData = null;
                selectedMusicsInfo = null;

                //filesList.Clear();
                // title = null;
                // artist = null;
                // album = null;
                // duration = null;
                // path = null;
                // lyrics = null;
                return fileNames;
            }

            ///<summary>
            ///Adds the selected songs' info to the specified playlist's data base
            ///</summary>
            public static void WriteAudioDataBase(string[][] playlistMusicData, int playlistNumber)
            {
                switch (playlistNumber)
                {
                    case 1:

                        List<string[]> filesList = new List<string[]>();
                        string[][] selectedMusicsData;
                        filesList.Clear();
                        using (StreamReader duplicateChecker = new StreamReader(playlist1Path))
                        {
                            string tempTextFileData = duplicateChecker.ReadToEnd();
                            if (tempTextFileData != null)
                            {
                                int i = 0;
                                foreach (var music in playlistMusicData) // To check if the added file path already exists in the txt
                                {
                                    if (!tempTextFileData.Contains(music[4]))
                                    {
                                        filesList.Add(music);
                                    }
                                    i++;
                                }
                                selectedMusicsData = filesList.ToArray();
                            }
                            else
                            {
                                int i = 0;
                                selectedMusicsData = playlistMusicData;
                                selectedMusicsData = filesList.ToArray();

                            }
                        }

                        using (StreamWriter selectedDataWriter = System.IO.File.AppendText(playlist1Path))
                        {
                            //fullMusicPathArray.Clear();

                            foreach (string[] musicinfo in selectedMusicsData)
                            {
                                string title = musicinfo[0];
                                string artist = musicinfo[1];
                                string album = musicinfo[2];
                                string duration = musicinfo[3];
                                string path = musicinfo[4];
                                string format = musicinfo[5];
                                string lyrics = musicinfo[6];


                                selectedDataWriter.WriteLine('|' + title + '|' + '|' + artist + '|' + '|' + album + '|' + '|' + duration + '|' + '|' +
                                   path + '|' + '|' + format + '|' + '|' + lyrics + '|');
                                //fullMusicPathArray.Add(file);
                            }
                        }
                        //System.IO.File.SetAttributes(playlist1Path, System.IO.File.GetAttributes(playlist1Path) | FileAttributes.Hidden);
                        break;

                    case 2:

                        filesList = new List<string[]>();

                        filesList.Clear();
                        using (StreamReader duplicateChecker = new StreamReader(playlist2Path))
                        {
                            string tempTextFileData = duplicateChecker.ReadToEnd();
                            if (tempTextFileData != null)
                            {
                                int i = 0;
                                foreach (var music in playlistMusicData) // To check if the added file path already exists in the txt
                                {
                                    if (!tempTextFileData.Contains(music[4]))
                                    {
                                        filesList.Add(music);
                                    }
                                    i++;
                                }
                                selectedMusicsData = filesList.ToArray();
                            }
                            else
                            {
                                int i = 0;
                                selectedMusicsData = playlistMusicData;
                                selectedMusicsData = filesList.ToArray();

                            }
                        }

                        using (StreamWriter selectedDataWriter = System.IO.File.AppendText(playlist2Path))
                        {
                            //fullMusicPathArray.Clear();

                            foreach (string[] musicinfo in selectedMusicsData)
                            {
                                string title = musicinfo[0];
                                string artist = musicinfo[1];
                                string album = musicinfo[2];
                                string duration = musicinfo[3];
                                string path = musicinfo[4];
                                string format = musicinfo[5];
                                string lyrics = musicinfo[6];


                                selectedDataWriter.WriteLine('|' + title + '|' + '|' + artist + '|' + '|' + album + '|' + '|' + duration + '|' + '|' +
                                   path + '|' + '|' + format + '|' + '|' + lyrics + '|');
                                //fullMusicPathArray.Add(file);
                            }
                        }
                        //System.IO.File.SetAttributes(playlist2Path, System.IO.File.GetAttributes(playlist2Path) | FileAttributes.Hidden);
                        break;

                    case 3:

                        filesList = new List<string[]>();

                        filesList.Clear();
                        using (StreamReader duplicateChecker = new StreamReader(playlist3Path))
                        {
                            string tempTextFileData = duplicateChecker.ReadToEnd();
                            if (tempTextFileData != null)
                            {
                                int i = 0;
                                foreach (var music in playlistMusicData) // To check if the added file path already exists in the txt
                                {
                                    if (!tempTextFileData.Contains(music[4]))
                                    {
                                        filesList.Add(music);
                                    }
                                    i++;
                                }
                                selectedMusicsData = filesList.ToArray();
                            }
                            else
                            {
                                int i = 0;
                                selectedMusicsData = playlistMusicData;
                                selectedMusicsData = filesList.ToArray();

                            }
                        }

                        using (StreamWriter selectedDataWriter = System.IO.File.AppendText(playlist3Path))
                        {
                            //fullMusicPathArray.Clear();

                            foreach (string[] musicinfo in selectedMusicsData)
                            {
                                string title = musicinfo[0];
                                string artist = musicinfo[1];
                                string album = musicinfo[2];
                                string duration = musicinfo[3];
                                string path = musicinfo[4];
                                string format = musicinfo[5];
                                string lyrics = musicinfo[6];


                                selectedDataWriter.WriteLine('|' + title + '|' + '|' + artist + '|' + '|' + album + '|' + '|' + duration + '|' + '|' +
                                   path + '|' + '|' + format + '|' + '|' + lyrics + '|');
                                //fullMusicPathArray.Add(file);
                            }
                        }
                        //System.IO.File.SetAttributes(playlist3Path, System.IO.File.GetAttributes(playlist3Path) | FileAttributes.Hidden);
                        break;

                    case 4:

                        filesList = new List<string[]>();

                        filesList.Clear();
                        using (StreamReader duplicateChecker = new StreamReader(playlist4Path))
                        {
                            string tempTextFileData = duplicateChecker.ReadToEnd();
                            if (tempTextFileData != null)
                            {
                                int i = 0;
                                foreach (var music in playlistMusicData) // To check if the added file path already exists in the txt
                                {
                                    if (!tempTextFileData.Contains(music[4]))
                                    {
                                        filesList.Add(music);
                                    }
                                    i++;
                                }
                                selectedMusicsData = filesList.ToArray();
                            }
                            else
                            {
                                int i = 0;
                                selectedMusicsData = playlistMusicData;
                                selectedMusicsData = filesList.ToArray();

                            }
                        }

                        using (StreamWriter selectedDataWriter = System.IO.File.AppendText(playlist4Path))
                        {
                            //fullMusicPathArray.Clear();

                            foreach (string[] musicinfo in selectedMusicsData)
                            {
                                string title = musicinfo[0];
                                string artist = musicinfo[1];
                                string album = musicinfo[2];
                                string duration = musicinfo[3];
                                string path = musicinfo[4];
                                string format = musicinfo[5];
                                string lyrics = musicinfo[6];


                                selectedDataWriter.WriteLine('|' + title + '|' + '|' + artist + '|' + '|' + album + '|' + '|' + duration + '|' + '|' +
                                   path + '|' + '|' + format + '|' + '|' + lyrics + '|');
                                //fullMusicPathArray.Add(file);
                            }
                        }
                        //System.IO.File.SetAttributes(playlist4Path, System.IO.File.GetAttributes(playlist4Path) | FileAttributes.Hidden);
                        break;

                    case 5:

                        filesList = new List<string[]>();

                        filesList.Clear();
                        using (StreamReader duplicateChecker = new StreamReader(playlist5Path))
                        {
                            string tempTextFileData = duplicateChecker.ReadToEnd();
                            if (tempTextFileData != null)
                            {
                                int i = 0;
                                foreach (var music in playlistMusicData) // To check if the added file path already exists in the txt
                                {
                                    if (!tempTextFileData.Contains(music[4]))
                                    {
                                        filesList.Add(music);
                                    }
                                    i++;
                                }
                                selectedMusicsData = filesList.ToArray();
                            }
                            else
                            {
                                int i = 0;
                                selectedMusicsData = playlistMusicData;
                                selectedMusicsData = filesList.ToArray();

                            }
                        }

                        using (StreamWriter selectedDataWriter = System.IO.File.AppendText(playlist5Path))
                        {

                            foreach (string[] musicinfo in selectedMusicsData)
                            {
                                string title = musicinfo[0];
                                string artist = musicinfo[1];
                                string album = musicinfo[2];
                                string duration = musicinfo[3];
                                string path = musicinfo[4];
                                string format = musicinfo[5];
                                string lyrics = musicinfo[6];


                                selectedDataWriter.WriteLine('|' + title + '|' + '|' + artist + '|' + '|' + album + '|' + '|' + duration + '|' + '|' +
                                   path + '|' + '|' + format + '|' + '|' + lyrics + '|');
                            }
                        }
                        break;

                }

            }


            ///<summary>
            /// Reads the data base that contains all music files info 
            ///</summary>
            //public static (string[][], Image[], string[]) ReadAudioDataBase()
            public static string[][] ReadAudioDataBase()
            {
                //allMusicInfo = Array.Empty<string[]>();
                string[][]? tempAllMusicInfos;
                Image[]? tempAllMusicArts;
                Image? art;


                //string tempplaylist = System.IO.File.ReadAllText(allMusicDataBase, Encoding.UTF8);
                string[] musicElements = new string[4];
                XmlSerializer dataBaseSerializer = new XmlSerializer(typeof(string[][]));
                using (StreamReader textReader = new StreamReader(allMusicDataBase, Encoding.UTF8))
                {
                    tempAllMusicInfos = (string[][])dataBaseSerializer.Deserialize(textReader);
                }

                //tempAllMusicArts = new Image[tempAllMusicInfos.Length];
                //int x = 0;
                //string[] filesNames = new string[tempAllMusicInfos.Length];

                //foreach (string[] songinfo in tempAllMusicInfos)
                //{
                //  tempAllMusicInfos[x][0] = songinfo[0].ReplaceLineEndings();
                //string path = songinfo[1];
                /*
                filesNames[x] = albumArtsPath + "\\" + System.IO.Path.GetFileName(path).Split('.')[0] + ".bmp";
                if (!System.IO.File.Exists(filesNames[0]))
                {
                    filesNames[x] = filesNames[x].Split('.')[0] + ".png";
                }
                art = Image.FromFile(filesNames[x]);
                tempAllMusicArts[x] = art;
                */
                //songinfo[6] = songinfo[6].ReplaceLineEndings();

                /*
                TagLib.File musicTags = TagLib.File.Create(songinfo[5]);

                var tempPics = musicTags.Tag.Pictures;

                if (tempPics.Length > 0)
                {
                    using (MemoryStream picConverter = new MemoryStream(tempPics[0].Data.Data))
                    {
                        art = Image.FromStream(picConverter);
                    }
                }
                else { art = Properties.Resources.MusicArt_NoCover; } */
                //art = Properties.Resources.MusicArt_NoCover;
                //tempAllMusicArts[x] = art;

                // musicTags.Dispose();
                //     x++;
                // }

                dataBaseSerializer = null;

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
                return tempAllMusicInfos;
            }

            ///<summary>
            /// Reads the data base that contains all music files info in addition to album arts of the songs
            /// if the keyword "complete" is included or an empty string. If "added" is included only the recently 
            /// added files's info are read
            ///</summary>
            //public static (string[][], Image[], string[]) ReadAudioDataBase()
            public static (string[][]?, string[]?) ReadAudioDataBase(string ReadingMode)
            {
                //allMusicInfo = Array.Empty<string[]>();
                string[][]? tempAllMusicInfos;
                string[]? filesNames;
                Image? art;

                if (ReadingMode == "added")
                {
                    //string[] musicElements = new string[4];
                    XmlSerializer dataBaseSerializer = new XmlSerializer(typeof(string[][]));
                    using (StreamReader textReader = new StreamReader(allMusicDataBase, Encoding.UTF8))
                    {
                        tempAllMusicInfos = (string[][])dataBaseSerializer.Deserialize(textReader);
                    }
                    int added = AddedSongCount;
                    string[][]? tempAddedMusicInfos = new string[added][];
                    int z = 0;
                    for (int i = tempAllMusicInfos.Length - added; i < tempAllMusicInfos.Length; i++)
                    {
                        tempAddedMusicInfos[z] = (string[])tempAllMusicInfos[i].Clone();
                        z++;
                    }
                    //tempAllMusicInfos = null;
                    int x = 0;
                    filesNames = new string[tempAddedMusicInfos.Length];

                    foreach (string[] songinfo in tempAddedMusicInfos)
                    {
                        //tempAllMusicInfos[x][0] = songinfo[0].ReplaceLineEndings();
                        string path = songinfo[3];
                        var tempName = System.IO.Path.GetFileName(path).Split('.');
                        int dotcount;
                        TextBox name = new TextBox();
                        if (tempName.Length > 2)
                        {
                            dotcount = tempName.Length - 1;
                            for (int y = 0; y < dotcount; y++)
                            {
                                name.AppendText(tempName[y]);
                                if (y + 1 <= dotcount) { name.AppendText("."); }
                            }
                            filesNames[x] = name.Text;

                        }
                        else
                        {
                            dotcount = 1;
                            filesNames[x] = System.IO.Path.GetFileName(path).Split('.')[dotcount - 1];  //actually incomplete so it can be added to albumArtsPath
                        }
                        filesNames[x] = albumArtsPath + filesNames[x] + ".bmp";
                        
                        x++;

                        name.Dispose();
                    }
                    
                    dataBaseSerializer = null;
                    return (tempAddedMusicInfos, filesNames);
                }

                else
                {



                    //string[] musicElements = new string[4];
                    XmlSerializer dataBaseSerializer = new XmlSerializer(typeof(string[][]));
                    using (StreamReader textReader = new StreamReader(allMusicDataBase, Encoding.UTF8))
                    {
                        tempAllMusicInfos = (string[][])dataBaseSerializer.Deserialize(textReader);
                    }

                    int x = 0;
                    filesNames = new string[tempAllMusicInfos.Length];

                    foreach (string[] songinfo in tempAllMusicInfos)
                    {
                        //tempAllMusicInfos[x][0] = songinfo[0].ReplaceLineEndings();
                        string path = songinfo[3];
                        var tempName = System.IO.Path.GetFileName(path).Split('.');
                        int dotcount;
                        TextBox name = new TextBox();
                        if (tempName.Length > 2)
                        {
                            dotcount = tempName.Length - 1;
                            for (int z = 0; z < dotcount; z++)
                            {
                                name.AppendText(tempName[z]);
                                if (z + 1 <= dotcount) { name.AppendText("."); }
                            }
                            filesNames[x] = name.Text;

                        }
                        else
                        {
                            dotcount = 1;
                            filesNames[x] = System.IO.Path.GetFileName(path).Split('.')[dotcount - 1];  //actually incomplete so it can be added to albumArtsPath
                        }

                        filesNames[x] = albumArtsPath + filesNames[x] + ".bmp";

                        x++;

                        name.Dispose();
                    }
                    dataBaseSerializer = null;
                    return (tempAllMusicInfos, filesNames);
                }




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
                artsPaths = (string[]?)allArtFilePaths.Clone();
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
                        ShellFile shellfile = ShellFile.FromFilePath(musicElements[4]);
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
            public static string[][]? allmusics;
            //public static Image[]? allarts;
            public static string[]? allArtsPath;
            //public ListView currentPlaylist;
            public static AudioFileReader song;
            public static WaveOutEvent mediaPlayer = new WaveOutEvent();
            public static string playerState { get; set; }   // should have "setup" "playing" , "paused", "stopped", or "finished" value.

            public PlayBackFunction(ListView playlist)
            {
                using (KhiConverter KhiKotes = new KhiConverter(playlist))
                {
                    allmusics = (string[][]?)KhiKotes.AllInfo.Clone();
                    //allarts = KhiKotes.AllMusicArts;
                    allArtsPath = (string[]?)KhiKotes.artsPaths.Clone();
                    playerState = "setup";
                }

                // selectedMusicsQue = (uint)selectedItemsList.Length;
                /*   if (selectedItems.Count > 0)
                   {
                       string tempPath = selectedItems[0][3];
                       song = new AudioFileReader(tempPath);
                   }
                   else
                   {
                       noSongSelected = true;
                   }
                   //song = new AudioFileReader(selectedItemsList[selectedMusicsQue - 1][3]); */
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
            public static void MusicPlayBackControl(string playBackControl)
            {


                uint numberOfSongsToPlay = (uint)selectedItemsList.Length;
                int playStopNumberCount = 0;
                switch (playBackControl)
                {
                    case "PlayPause":

                        if (mediaPlayer.PlaybackState == PlaybackState.Playing)
                        {
                            mediaPlayer.Pause();

                            if (currentlyPlayingSongInfo != currentlySelectedSong)
                            {
                                song.Dispose();
                                mediaPlayer.Dispose();
                                currentlyPlayingSongInfo = null;
                                currentlyPlayingSongPic = null;

                                song = new AudioFileReader(selectedItemsList[selectedMusicsQue][3]);
                                mediaPlayer = new WaveOutEvent();
                                mediaPlayer.Init(song);
                                mediaPlayer.Play();
                                playerState = "playing";
                                currentlyPlayingSongInfo = selectedItemsList[selectedMusicsQue];
                                currentlyPlayingSongPic = Image.FromFile(allArtsPath[selectedItemsListIndex[selectedMusicsQue]]);
                                //allarts[selectedItemsListIndex[selectedMusicsQue]];
                            }
                            else
                            {
                                playerState = "paused";
                            }

                        }
                        else
                        {
                            if (noSongSelected == false)
                            {
                                //song.Dispose();
                                //mediaPlayer.Dispose();
                            }
                            //  if (playStopNumberCount > 0) { selectedMusicsQue--; }
                            if (mediaPlayer.PlaybackState == PlaybackState.Paused)
                            {
                                if (currentlyPlayingSongInfo != currentlySelectedSong)
                                {
                                    song.Dispose();
                                    mediaPlayer.Dispose();
                                    currentlyPlayingSongInfo = null;
                                    currentlyPlayingSongPic = null;

                                    song = new AudioFileReader(selectedItemsList[selectedMusicsQue][3]);
                                    mediaPlayer = new WaveOutEvent();
                                    mediaPlayer.Init(song);
                                    mediaPlayer.Play();
                                    playerState = "playing";
                                    currentlyPlayingSongInfo = selectedItemsList[selectedMusicsQue];
                                    currentlyPlayingSongPic = Image.FromFile(allArtsPath[selectedItemsListIndex[selectedMusicsQue]]);
                                    //currentlyPlayingSongPic = allarts[selectedItemsListIndex[selectedMusicsQue]];
                                }
                                else
                                {
                                    mediaPlayer.Play();
                                    playerState = "playing";
                                }
                            }
                            else
                            {
                                //song.Dispose();
                                //mediaPlayer.Dispose();
                                //currentlyPlayingSongInfo = null;
                                //currentlyPlayingSongPic = null;

                                song = new AudioFileReader(selectedItemsList[selectedMusicsQue][3]);
                                mediaPlayer = new WaveOutEvent();
                                mediaPlayer.Init(song);
                                mediaPlayer.Play();
                                playerState = "playing";
                                currentlyPlayingSongInfo = selectedItemsList[selectedMusicsQue];
                                currentlyPlayingSongPic = Image.FromFile(allArtsPath[selectedItemsListIndex[selectedMusicsQue]]);
                                //currentlyPlayingSongPic = allarts[selectedItemsListIndex[selectedMusicsQue]];
                            }

                            playStopNumberCount = 0;

                        }
                        break;

                    case "Skip":
                        mediaPlayer.Stop();
                        song.Position = 0;
                        selectedMusicsQue++;
                        song.Dispose();
                        mediaPlayer.Dispose();
                        currentlyPlayingSongInfo = null;
                        currentlyPlayingSongPic = null;
                        if (isLoopEnabled == true && selectedMusicsQue > selectedItemsList.Length - 1)
                        {
                            selectedMusicsQue = 0;
                            song = new AudioFileReader(selectedItemsList[selectedMusicsQue][3]);
                            mediaPlayer = new WaveOutEvent();

                            mediaPlayer.Init(song);
                            mediaPlayer.Play();
                            playerState = "playing";
                            currentlyPlayingSongInfo = selectedItemsList[selectedMusicsQue];
                            currentlyPlayingSongPic = Image.FromFile(allArtsPath[selectedItemsListIndex[selectedMusicsQue]]);
                            //currentlyPlayingSongPic = allarts[selectedItemsListIndex[selectedMusicsQue]];
                        }
                        else
                        {
                            //selectedMusicsQue++;
                            if (selectedMusicsQue > selectedItemsList.Length - 1)
                            {
                                System.Windows.Forms.MessageBox.Show("End of Playlist Reached");
                                playerState = "finished";
                                selectedMusicsQue = 0;
                            }
                            else
                            {

                                song = new AudioFileReader(selectedItemsList[selectedMusicsQue][3]);
                                mediaPlayer = new WaveOutEvent();

                                mediaPlayer.Init(song);
                                mediaPlayer.Play();
                                playerState = "playing";
                                currentlyPlayingSongInfo = selectedItemsList[selectedMusicsQue];
                                currentlyPlayingSongPic = Image.FromFile(allArtsPath[selectedItemsListIndex[selectedMusicsQue]]);
                                //currentlyPlayingSongPic = allarts[selectedItemsListIndex[selectedMusicsQue]];
                            }
                        }




                        break;

                    case "Previous":

                        mediaPlayer.Stop();
                        song.Position = 0;
                        song.Dispose();
                        mediaPlayer.Dispose();
                        currentlyPlayingSongInfo = null;
                        currentlyPlayingSongPic = null;
                        if (isLoopEnabled == true && selectedMusicsQue == 0)
                        {
                            selectedMusicsQue = (uint)selectedItemsList.Length - 1;
                            song = new AudioFileReader(selectedItemsList[selectedMusicsQue][3]);
                            mediaPlayer = new WaveOutEvent();
                            mediaPlayer.Init(song);
                            mediaPlayer.Play();
                            playerState = "playing";
                            currentlyPlayingSongInfo = selectedItemsList[selectedMusicsQue];
                            currentlyPlayingSongPic = Image.FromFile(allArtsPath[selectedItemsListIndex[selectedMusicsQue]]);
                            //currentlyPlayingSongPic = allarts[selectedItemsListIndex[selectedMusicsQue]];
                        }
                        else
                        {
                            if (selectedMusicsQue == 0)
                            {
                                System.Windows.Forms.MessageBox.Show("End of Playlist Reached");
                                playerState = "finished";
                                selectedMusicsQue = 0;
                            }
                            else
                            {
                                selectedMusicsQue--;
                                song = new AudioFileReader(selectedItemsList[selectedMusicsQue][3]);
                                mediaPlayer = new WaveOutEvent();

                                mediaPlayer.Init(song);
                                mediaPlayer.Play();
                                playerState = "playing";
                                currentlyPlayingSongInfo = selectedItemsList[selectedMusicsQue];
                                currentlyPlayingSongPic = Image.FromFile(allArtsPath[selectedItemsListIndex[selectedMusicsQue]]);
                                //currentlyPlayingSongPic = allarts[selectedItemsListIndex[selectedMusicsQue]];

                            }

                        }
                        break;

                }
                //After a music is played, skiped, etc


            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
                            nextPlaylistNumber++;


                            break;

                        case 2:
                            playlist2Button.Text = playlistName;
                            playlist2Button.Visible = true;
                            playlist2Button.Enabled = true;
                            nextPlaylistNumber++;

                            break;

                        case 3:
                            playlist3Button.Text = playlistName;
                            playlist3Button.Visible = true;
                            playlist3Button.Enabled = true;
                            nextPlaylistNumber++;

                            break;

                        case 4:
                            playlist4Button.Text = playlistName;
                            playlist4Button.Visible = true;
                            playlist4Button.Enabled = true;
                            nextPlaylistNumber++;

                            break;

                        case 5:
                            playlist5Button.Text = playlistName;
                            playlist5Button.Visible = true;
                            playlist5Button.Enabled = true;
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
            using (AudioDataBase KhiDatabase = new AudioDataBase())
            {
                //musicListView = AllSongsPlaylist;
                if (KhiDatabase.isDataBaseRead)
                {
                    int x = 0;
                    foreach (var music in KhiDatabase.AllMusicInfo)
                    {
                        //musicListView.SmallImageList.Images.Add(KhiDatabase.AllMusicArts[x]);
                        //musicListView.LargeImageList.Images.Add(KhiDatabase.AllMusicArts[x]);
                        musicListView.Items.Add(new ListViewItem(music, x));
                        x++;
                    }
                    musicListView.Refresh();
                }
            }

            if (musicListView.Items.Count < 1)
            {
                System.Windows.Forms.MessageBox.Show("Please add music ");
            }
            //fullMusicPathList.Refresh();
        }

        private void ClearListItem_Click(object sender, EventArgs e)
        {
            musicListView.Items.Clear();
            //fullMusicPathArray.Clear();
            //System.IO.File.WriteAllText(allMusicDataBase, "");
        }

        private void musicListView_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
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
                allArtFilePaths = (string[]?)khiDatabase.ArtFileNames.Clone();
                var AddedMusicInfo = (string[][]?)khiDatabase.AddedMusicInfo.Clone();
                var AddedMusicArts = (Image[]?)khiDatabase.AddedMusicArts.Clone();
                var AddedArtFilePaths = (string[]?)khiDatabase.AddedArtFileNames.Clone();
                khiDatabase.Dispose();
                //var tempArts = khiDatabase.AllMusicArts;
                int previousItemCount = musicListView.Items.Count;
                musicListView.BeginUpdate();
                //musicListView.Items.Clear();
                int x = 0;
                //musicListView.LargeImageList.Images.AddRange(khiDatabase.AllMusicArts);
                foreach (var music in AddedMusicInfo)
                {

                    //musicListView.SmallImageList.Images.Add(Icon.ExtractAssociatedIcon(khiDatabase.ArtFileNames[x]));
                    //musicListView.LargeImageList.Images.Add(Image.FromFile(khiDatabase.ArtFileNames[x]));
                    musicListView.LargeImageList.Images.Add(AddedMusicArts[x]);
                    //musicListView.SmallImageList.Images.Add(allMusicArts[x]);
                    ListViewItem song = new ListViewItem(music, x + previousItemCount);
                    song.ToolTipText = music[0] + System.Environment.NewLine + music[1] + System.Environment.NewLine + music[2];
                    song.Name = music[3];
                    //song.BeginEdit();


                    musicListView.Items.Add(song);

                    song = null;
                    x++;
                }
                musicListView.EndUpdate();
                listUpdated = true;
                //to dispose
                AddedMusicArts = null;
                AddedMusicInfo = null;
                AddedArtFilePaths = null;
                draggedFiles = null;
                tempPathList.Clear();
                tempPathList = null;
                tempDraggedFiles = null;
                //khiDatabase.Dispose();
                GC.Collect();
            }
            //AllSongsPlaylist.Items.Clear();

        }

        private void musicListView_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {

        }

        private void toggleLoop_Click(object sender, EventArgs e)
        {
            if (isLoopEnabled == false)
            {
                isLoopEnabled = true;
                toggleLoop.ForeColor = Color.Blue;
            }
            else
            {
                isLoopEnabled = false;
                toggleLoop.ForeColor = default;
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
            textBox1.Text = string.Empty;
            List<string[]> testList = new List<string[]>();
            using (KhiConverter KhiKotes = new KhiConverter(musicListView))
            {
                var allInfo = KhiKotes.AllInfo;
                if (selectedItems.Count > 0)
                {
                    if (noSongSelected == false)
                    {

                        selectedItemsList = selectedItems.ToArray();
                        selectedItemsListIndex = selectedItemsIndices.ToArray();

                    }
                    else
                    {

                        selectedItemsList = KhiKotes.AllInfo;

                        int[] tempIndices = new int[musicListView.Items.Count];
                        for (int n = 0; n <= musicListView.Items.Count - 1; n++)
                        {
                            tempIndices[n] = n;
                        }

                        selectedItemsListIndex = tempIndices;
                        selectedItemsIndices.AddRange(tempIndices);
                        selectedItems.AddRange(KhiKotes.AllInfo);

                    }

                }
                else //Plays All Songs if no song is selected 
                {    // Can later change into playing the current playlist when that part is done
                    noSongSelected = true;
                    selectedItemsList = KhiKotes.AllInfo;
                    selectedItems.AddRange(KhiKotes.AllInfo);
                    int[] tempIndices = new int[musicListView.Items.Count];
                    for (int n = 0; n <= musicListView.Items.Count - 1; n++)
                    {
                        tempIndices[n] = n;
                    }
                    selectedItemsListIndex = tempIndices;
                    selectedItemsIndices.AddRange(tempIndices);
                }
            }


            //PlayBackFunction KhiPlayer = new PlayBackFunction(musicListView);
            if (listUpdated == true)
            {
                if (currentlyPlayingSongInfo[0][3] != null && currentlyPlayingSongInfo[0][0] != null)
                {
                    var infoBackup = (string[])currentlyPlayingSongInfo.Clone();
                    var picBackup = (Image?)currentlyPlayingSongPic.Clone();
                    //KhiPlayer.Dispose();
                    PlayBackFunction khiPlayer = new PlayBackFunction(musicListView);
                    currentlyPlayingSongInfo = infoBackup;
                    currentlyPlayingSongPic = picBackup;
                }
                else
                {
                    //KhiPlayer.Dispose();
                    PlayBackFunction khiPlayer = new PlayBackFunction(musicListView);
                }

                listUpdated = false;
            }
            PlayBackFunction.MusicPlayBackControl("PlayPause");

            lyricsTextBox.Clear();
            string? lyrics;
            using (TagLib.File lyrictag = TagLib.File.Create(currentlyPlayingSongInfo[3]))
            {
                lyrics = lyrictag.Tag.Lyrics;
                if (lyrics != null)
                { lyrics = lyrics.ReplaceLineEndings(); }
            }
            lyricsTextBox.Text = lyrics;

            if (currentlyPlayingSongPic != null)
            {
                pictureBox1.Image = null;
                pictureBox1.Image = currentlyPlayingSongPic;
            }

            testList.Clear();
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
            if (listUpdated == true)
            {
                if (currentlyPlayingSongInfo[0][3] != null)
                {
                    var infoBackup = (string[])currentlyPlayingSongInfo.Clone();
                    var picBackup = (Image?)currentlyPlayingSongPic.Clone();
                    KhiPlayer.Dispose();
                    PlayBackFunction khiPlayer = new PlayBackFunction(musicListView);
                    currentlyPlayingSongInfo = infoBackup;
                    currentlyPlayingSongPic = picBackup;
                }
                else
                {
                    KhiPlayer.Dispose();
                    PlayBackFunction khiPlayer = new PlayBackFunction(musicListView);
                }

                listUpdated = false;
            }
            PlayBackFunction.MusicPlayBackControl("Skip");

            // ***lyricsTextBox.Text = musicListView.SelectedItems[(int)selectedMusicsQue].SubItems[6].Text;
            string? lyrics;
            using (TagLib.File lyrictag = TagLib.File.Create(currentlyPlayingSongInfo[3]))
            {
                lyrics = lyrictag.Tag.Lyrics;
                if (lyrics != null)
                { lyrics.ReplaceLineEndings(); }
            }
            lyricsTextBox.Text = lyrics;

            if (currentlyPlayingSongPic != null)
            {
                pictureBox1.Image = null;
                pictureBox1.Image = currentlyPlayingSongPic;
            }
        }

        private void previous_Click(object sender, EventArgs e)
        {
            if (listUpdated == true)
            {
                if (currentlyPlayingSongInfo[0][3] != null)
                {
                    var infoBackup = (string[])currentlyPlayingSongInfo.Clone();
                    var picBackup = (Image?)currentlyPlayingSongPic.Clone();
                    KhiPlayer.Dispose();
                    PlayBackFunction khiPlayer = new PlayBackFunction(musicListView);
                    currentlyPlayingSongInfo = infoBackup;
                    currentlyPlayingSongPic = picBackup;
                }
                else
                {
                    KhiPlayer.Dispose();
                    PlayBackFunction khiPlayer = new PlayBackFunction(musicListView);
                }

                listUpdated = false;
            }
            PlayBackFunction.MusicPlayBackControl("Previous");
            // ***lyricsTextBox.Text = musicListView.SelectedItems[(int)selectedMusicsQue - 2].SubItems[6].Text;
            string? lyrics;
            using (TagLib.File lyrictag = TagLib.File.Create(currentlyPlayingSongInfo[3]))
            {
                lyrics = lyrictag.Tag.Lyrics;
                if (lyrics != null)
                { lyrics.ReplaceLineEndings(); }
            }
            lyricsTextBox.Text = lyrics;

            if (currentlyPlayingSongPic != null)
            {
                pictureBox1.Image = null;
                pictureBox1.Image = currentlyPlayingSongPic;
            }

            mediaPlayerPanel.BackgroundImage = currentlyPlayingSongPic;
        }

        private void musicListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            using (KhiConverter KhiKotes = new KhiConverter(musicListView))
            {
                var allInfo = KhiKotes.AllInfo;
                if (selectedItems.Count > 0 && selectedItems.Contains(allInfo[e.ItemIndex]))
                {
                    if (noSongSelected == true)
                    {
                        selectedItems.Clear();
                        selectedItemsIndices.Clear();
                        selectedItemsList = Array.Empty<string[]>();
                        selectedItemsListIndex = Array.Empty<int>();
                        //currentlySelectedSong = Array.Empty<string>();
                        selectedItemsIndices.Add(e.ItemIndex);
                        selectedItems.Add(allInfo[e.ItemIndex]);
                        currentlySelectedSong = allInfo[e.ItemIndex];
                        noSongSelected = false;
                    }
                    else
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

                else
                {
                    noSongSelected = false;
                    //just to be safe
                    selectedItems.Clear();
                    selectedItemsIndices.Clear();
                    selectedItemsIndices.Add(e.ItemIndex);
                    selectedItems.Add(allInfo[e.ItemIndex]);
                    currentlySelectedSong = allInfo[e.ItemIndex];
                    //int fakeFullMusicPathListCountSelected = fakeFullMusicPathList.SelectedItems.Count;
                }
            }
        }



        private void addMusicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog musicBrowser = new OpenFileDialog();
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
                    allArtFilePaths = (string[]?)khiDatabase.ArtFileNames.Clone();
                    var AddedMusicInfo = (string[][]?)khiDatabase.AddedMusicInfo.Clone();
                    var AddedMusicArts = (Image[]?)khiDatabase.AddedMusicArts.Clone();
                    var AddedArtFilePaths = (string[]?)khiDatabase.AddedArtFileNames.Clone();
                    khiDatabase.Dispose();
                    //var tempArts = khiDatabase.AllMusicArts;
                    int previousItemCount = musicListView.Items.Count;
                    musicListView.BeginUpdate();
                    //musicListView.Items.Clear();
                    int x = 0;
                    //musicListView.LargeImageList.Images.AddRange(khiDatabase.AllMusicArts);
                    foreach (var music in AddedMusicInfo)
                    {

                        //musicListView.SmallImageList.Images.Add(Icon.ExtractAssociatedIcon(khiDatabase.ArtFileNames[x]));
                        //musicListView.LargeImageList.Images.Add(Image.FromFile(khiDatabase.ArtFileNames[x]));
                        musicListView.LargeImageList.Images.Add(AddedMusicArts[x]);
                        //musicListView.SmallImageList.Images.Add(allMusicArts[x]);
                        ListViewItem song = new ListViewItem(music, x + previousItemCount);
                        song.ToolTipText = music[0] + System.Environment.NewLine + music[1] + System.Environment.NewLine + music[2];
                        song.Name = music[3];
                        //song.BeginEdit();


                        musicListView.Items.Add(song);

                        song = null;
                        x++;
                    }
                    musicListView.EndUpdate();
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
                if (rightClickMenu.IsDisposed || !rightClickMenu.Enabled)
                {
                    if (clickedItem == null)
                    {
                        musicListView.SelectedItems.Clear();
                        selectedItems.Clear();
                        selectedItemsIndices.Clear();
                        selectedItemsList = Array.Empty<string[]>();
                        selectedItemsListIndex = Array.Empty<int>();
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
            musicListView.View = View.LargeIcon;
            musicListView.Refresh();
        }

        private void showDetailsMenuItem_Click(object sender, EventArgs e)
        {
            showDetailsMenuItem.Checked = true;
            showLargeIconMenuItem.Checked = false;
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
                //lyricsTextBox.Size = lyricsTextBox.MaximumSize;
                lyricsTextBox.Visible = true;
                lyricsTextBox.Enabled = true;
            }

        }

        private void lyricsTextBox_DoubleClick(object sender, EventArgs e)
        {
            if (lyricsTextBox.Enabled == true && lyricsTextBox.Visible == true )
            {
                pictureBox1.Enabled = true;
                pictureBox1.Visible = true;
                lyricsTextBox.Visible = false;
                lyricsTextBox.Enabled = false;
            }
        }
    }

}
