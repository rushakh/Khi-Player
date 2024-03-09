using NAudio;
using NAudio.Wave;
using TagLib;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Numerics;
using TagLib.Ogg.Codecs;
using TagLib.Riff;
using System.Drawing.Configuration;
using System.Drawing.Text;
using System.Drawing;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Xml.Linq;
using Microsoft.WindowsAPICodePack.Shell;


namespace Media_Player
{



    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static ListView AllSongsPlaylist = new ListView();
        static string[][] allMusicInfo = new string[1000][];
        static Image[][] allMusicArts = new Image[1000][];

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

        static int nextPlaylistNumber = Math.Clamp(1, 1, 5);


        static ArrayList fullMusicPathArray = new ArrayList();
        static string? selectedMusic;
        static string?[][] selectedMusics;

        static uint selectedMusicsQue = 0; //only accepts >= 0 value, not negative integers

        static int fullMusicPathArrayIndex;
        static bool multipleSelectedItems = false;
        static ListView fakeFullMusicPathList = new ListView(); // a list to work on and change in Classes before putting the finalized items into the main list
        static ListView fakeFullMusicPathList2 = new ListView();
        static ListView tempListView = new ListView();

        static string applicationPath = Application.StartupPath;
        static string allMusicDataBase = Application.StartupPath + "AllMusicDataBase.txt";

        static string[][] selectedItemsList;
        static int[] selectedItemsListIndex;


        public static object[][]? musicsProperties;

        static bool isLoopEnabled = false;
        //static AudioDataBase MyDataBase = new AudioDataBase();

        ///<summary> 
        /// Allows writing Dragged files' paths into a txt file and extraction of their properties
        /// </summary>
        public class AudioDataBase
        {

            internal static string applicationPath = Application.StartupPath;

            internal static string allMusicDataBase = Application.StartupPath + "AllMusicDataBase.txt";
            internal static string playlist1Path = Application.StartupPath + "playlist1.txt";
            internal static string playlist2Path = Application.StartupPath + "playlist2.txt";
            internal static string playlist3Path = Application.StartupPath + "playlist3.txt";
            internal static string playlist4Path = Application.StartupPath + "playlist4.txt";
            internal static string playlist5Path = Application.StartupPath + "playlist5.txt";

            static List<string> musicPathsList = new List<string>();
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
            static string[] addedMusicsPaths;
            static string[][] addedMusicInfo;
            static Image[][] addedMusicArts;

            ///<summary>
            ///Creates, writes to, and reads from Data Base
            ///</summary>
            public AudioDataBase(string[] addSongsPaths)
            {
                AllSongsPlaylist.AllowDrop = true;
                AllSongsPlaylist.AllowColumnReorder = true;
                AllSongsPlaylist.View = View.Details;


                allMusicInfo = new string[100][];
                allMusicArts = new Image[100][];
                GetAudioFilesInfo(addSongsPaths);
                WriteAudioDataBase(addedMusicInfo);
                //System.IO.File.SetAttributes(allMusicDataBase, System.IO.File.GetAttributes(allMusicDataBase) | FileAttributes.Hidden);
                ReadAudioDataBase();
                //ToListView();
            }
            ///<summary>
            ///Creates, writes to, and reads from Data Base, Don't use this the first time calling this class
            ///</summary>
            public AudioDataBase()
            {
                //  if (allMusicInfo[0][4] != "")
                // {
                GetAudioFilesInfo();
                WriteAudioDataBase();
                //System.IO.File.SetAttributes(allMusicDataBase, System.IO.File.GetAttributes(allMusicDataBase) | FileAttributes.Hidden);
                ReadAudioDataBase();
                //ToListView() ;
                // }
                // else
                // {
                //     WriteAudioDataBase();  //just creates the data base
                //     //System.IO.File.SetAttributes(allMusicDataBase, System.IO.File.GetAttributes(allMusicDataBase) | FileAttributes.Hidden);
                // }
            }

            ///<summary>
            ///Creates, Writest, and reads from the specific playlist Data Base
            ///</summary>
            public AudioDataBase(string[][] musicsToPlaylist, int playlistNumber)
            {
                WriteAudioDataBase(musicsToPlaylist, playlistNumber);
                ReadAudioDataBase(playlistNumber);
                //  ***** Later add the ToListView here
            }

            ///<summary>
            /// Gets the various properties of all Audio Files  (e.g., title, artist, album, etc)
            ///</summary>
            public static void GetAudioFilesInfo()
            {
                string title, artist, album, duration, path, format, lyrics;
                Image art;
                allMusicInfo = new string[musicPathsList.Count][];
                allMusicArts = new Image[musicPathsList.Count][];
                object[] musicProperties = new object[8];

                int i = 0;
                foreach (string music in musicPathsList.ToArray<string>())
                {
                    string[] musicInfo = new string[7];
                    //getting a picture of the audio file
                    TagLib.File musicTags = TagLib.File.Create(music);
                    var tempPics = musicTags.Tag.Pictures;


                    using (MemoryStream picConverter = new MemoryStream(tempPics[0].Data.Data))
                    {
                        art = Image.FromStream(picConverter);
                    }

                    Image[] musicArt = { art };


                    //to get the duration of the audio
                    using (AudioFileReader durationReader = new AudioFileReader(music))
                    {
                        var mins = durationReader.TotalTime.Minutes.ToString();
                        var secs = durationReader.TotalTime.Seconds.ToString("00");
                        duration = mins + " : " + secs;
                    }

                    path = music;
                    format = music.Split('.')[1].ToUpper();
                    title = musicTags.Tag.Title;
                    artist = musicTags.Tag.FirstPerformer;
                    album = musicTags.Tag.Album;
                    lyrics = musicTags.Tag.Lyrics;

                    // all the string infos
                    musicInfo[0] = title;
                    musicInfo[1] = artist;
                    musicInfo[2] = album;
                    musicInfo[3] = duration;
                    musicInfo[4] = path;
                    musicInfo[5] = format;
                    musicInfo[6] = lyrics;

                    allMusicInfo[i] = musicInfo;
                    allMusicArts[i] = musicArt;

                    //ALL of the data
                    musicProperties[0] = title;
                    musicProperties[1] = artist;
                    musicProperties[2] = album;
                    musicProperties[3] = duration;
                    musicProperties[4] = path;
                    musicProperties[5] = format;
                    musicProperties[6] = lyrics;
                    musicProperties[7] = art;

                    //musicsProperties[i] = musicProperties;



                    i++;
                }


            }

            ///<summary>
            ///Gets the various properties of selected Audio Files  (e.g., title, artist, album, etc)
            ///</summary>
            public static void GetAudioFilesInfo(string[] draggedMusicsPaths)
            {
                string title, artist, album, duration, path, format, lyrics;
                Image art;

                addedMusicsPaths = draggedMusicsPaths;
                addedMusicInfo = new string[draggedMusicsPaths.Length][];
                addedMusicArts = new Image[draggedMusicsPaths.Length][];
                object[] addedMusicsProperties = new object[8];

                int i = 0;

                foreach (string music in addedMusicsPaths)
                {
                    string[] musicInfo = new string[7];
                    //getting a picture of the audio file
                    TagLib.File musicTags = TagLib.File.Create(music);
                    var tempPics = musicTags.Tag.Pictures;


                    using (MemoryStream picConverter = new MemoryStream(tempPics[0].Data.Data))
                    {
                        art = Image.FromStream(picConverter);
                    }

                    Image[] musicArt = { art };


                    //to get the duration of the audio
                    using (AudioFileReader durationReader = new AudioFileReader(music))
                    {
                        var mins = durationReader.TotalTime.Minutes.ToString();
                        var secs = durationReader.TotalTime.Seconds.ToString("00");
                        duration = mins + " : " + secs;
                    }

                    path = music;
                    format = music.Split('.')[1].ToUpper();
                    title = musicTags.Tag.Title;
                    artist = musicTags.Tag.FirstPerformer;
                    album = musicTags.Tag.Album;
                    lyrics = musicTags.Tag.Lyrics;

                    // all the string infos
                    musicInfo[0] = title;
                    musicInfo[1] = artist;
                    musicInfo[2] = album;
                    musicInfo[3] = duration;
                    musicInfo[4] = path;
                    musicInfo[5] = format;
                    musicInfo[6] = lyrics;

                    addedMusicInfo[i] = musicInfo;
                    addedMusicArts[i] = musicArt;

                    allMusicInfo[i] = addedMusicInfo[i];
                    allMusicArts[i] = addedMusicArts[i];

                    //ALL of the data
                    addedMusicsProperties[0] = title;
                    addedMusicsProperties[1] = artist;
                    addedMusicsProperties[2] = album;
                    addedMusicsProperties[3] = duration;
                    addedMusicsProperties[4] = path;
                    addedMusicsProperties[5] = format;
                    addedMusicsProperties[6] = lyrics;
                    addedMusicsProperties[7] = art;

                    //musicsProperties[i] = musicProperties;



                    i++;
                }
            }


            ///<summary>
            /// Overwrites all of the scanned songs' info into the data base
            ///</summary>
            public static void WriteAudioDataBase()
            {
                List<string[]> filesList = new List<string[]>();
                //string[][] playlistMusicsData;

                using (StreamWriter playlistDataWriter = System.IO.File.CreateText(allMusicDataBase))
                {
                    //fullMusicPathArray.Clear();
                    if (allMusicInfo.Length != 0)
                    {
                        foreach (string[] musicInfo in allMusicInfo)
                        {
                            string title = musicInfo[0];
                            string artist = musicInfo[1];
                            string album = musicInfo[2];
                            string duration = musicInfo[3];
                            string path = musicInfo[4];
                            string format = musicInfo[5];
                            string lyrics = musicInfo[6];


                            playlistDataWriter.WriteLine('|' + title + '|' + '|' + artist + '|' + '|' + album + '|' + '|' + duration + '|' + "\n" +
                                '|' + path + '|' + '|' + format + '|' + "\n" +
                                '|' + lyrics + '|');

                            // This works like this: 
                            // |   | |   | |   | |   | |   | |    | |     |
                            // title, arti, albu, time, path, forma, lyrics
                            //0 1  2  3  4  5  6  7  8  9  10  11 12  13
                        }
                    }
                }
            }
            ///<summary>
            ///Adds the selected or dragged songs' info to the data base
            ///</summary>
            public static void WriteAudioDataBase(string[][] selectedMusicsInfo)
            {
                List<string[]> filesList = new List<string[]>();
                string[][] selectedMusicsData;
                filesList.Clear();

                if (System.IO.File.Exists(allMusicDataBase))
                {
                    using (StreamReader duplicateChecker = new StreamReader(allMusicDataBase))
                    {
                        string tempTextFileData = duplicateChecker.ReadToEnd();
                        if (tempTextFileData != null)
                        {
                            int i = 0;
                            foreach (var music in selectedMusicsInfo) // To check if the added file path already exists in the txt
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
                            selectedMusicsData = selectedMusicsInfo;


                        }
                    }
                }
                else
                {
                    selectedMusicsData = selectedMusicsInfo;
                }


                using (StreamWriter selectedDataWriter = System.IO.File.AppendText(allMusicDataBase))
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
                           path + '|' + '|' + format + '|' + '|' + lyrics + '|');  // odd numbers contain the tags, 1, 3, 5, 7, 9, 11, 13 for each music
                        //fullMusicPathArray.Add(file);
                    }
                }
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
                        //System.IO.File.SetAttributes(playlist5Path, System.IO.File.GetAttributes(playlist5Path) | FileAttributes.Hidden);
                        break;

                }

            }


            ///<summary>
            /// Reads the data base that contains all music files info 
            ///</summary>
            public static void ReadAudioDataBase()
            {
                string tempplaylist = System.IO.File.ReadAllText(allMusicDataBase);
                string[] musicElements = new string[7];
                string[] tempMusicElements = tempplaylist.Split('|', StringSplitOptions.None);
                int musicNumbers = tempMusicElements.Length / (14 - 1);

                string[][] playlistMusics = new string[musicNumbers][];   // these 3 have the same indices
                allMusicArts = new Image[musicNumbers][];       // playlistMusics, allMusicInfo, allMusicArts
                allMusicInfo = new string[musicNumbers][];


                //string[][] allmusics = new string[musicNumbers][];

                for (int x = 0; x < musicNumbers; x++)
                {
                    string[] music = new string[7];
                    music[0] = tempMusicElements[(x * 14 + 1)];
                    music[1] = tempMusicElements[(x * 14 + 1) + 2];
                    music[2] = tempMusicElements[(x * 14 + 1) + 4];
                    music[3] = tempMusicElements[(x * 14 + 1) + 6];
                    music[4] = tempMusicElements[(x * 14 + 1) + 8];
                    music[5] = tempMusicElements[(x * 14 + 1) + 10];
                    music[6] = tempMusicElements[(x * 14 + 1) + 12];
                    //allmusics[x] = music;
                    playlistMusics[x] = music;
                    allMusicInfo[x] = music;

                    //AllSongsPlaylist.Items.Add(new ListViewItem(playlistMusics[x]));

                    TagLib.File musicTags = TagLib.File.Create(music[4]);

                    var tempPics = musicTags.Tag.Pictures;
                    Image art;
                    if (tempPics.Length == 0)
                    {
                        ShellFile shellfile = ShellFile.FromFilePath(music[4]);
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
                    allMusicArts[x] = musicArt;
                }

                // for (int i = 0; i< allmusics.Length; i++)
                // { 


                //tempListView.Items.Clear();
                //tempListView.Items.Add(new ListViewItem(playlistMusics[i]));

                //}
            }

            ///<summary>
            /// Reads the data base of the specified playlist
            ///</summary>
            public static void ReadAudioDataBase(int playlistNumber)
            {
                switch (playlistNumber)
                {
                    case 1:

                        string tempplaylist = System.IO.File.ReadAllText(allMusicDataBase);
                        string[][] playlistMusics = new string[allMusicInfo.Length][];   // these 3 have the same indices
                        allMusicArts = new Image[allMusicInfo.Length][];       // playlistMusics, allMusicInfo, allMusicArts
                        allMusicInfo = new string[tempplaylist.Length][];

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
                            playlist2MusicArts[i] = musicArt;
                        }

                        break;

                    case 3:

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
                AllSongsPlaylist = tempListView;  // for now, should later change it using Switch and case for playlists

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
                /*   string[] reReadingMusicPaths = System.IO.File.ReadAllLines(playlistPath);
                   foreach (string readingMusicPath in reReadingMusicPaths)
                   {
                       //fullMusicPathArray.Add(readingMusicPath);
                   }
                */
                /*   int n = 0;
                   string[] musicPaths = new string[fullMusicPathArray.Count];
                   foreach (var item in fullMusicPathArray)
                   {
                       musicPaths[n] = item.ToString();
                       n++;
                   }

                   audioFilesPaths = musicPaths;
                */


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
             static AudioFileReader song;
             static WaveOutEvent mediaPlayer = new WaveOutEvent();
             static ListView currentPlaylist = new ListView();

            public PlayBackFunction(ListView playlist)
            {
                currentPlaylist = playlist;
                selectedMusicsQue = (uint)selectedItemsList.Length;
                song = new AudioFileReader(selectedItemsList[selectedMusicsQue - 1][4]);
            }

            ///<summary> 
            /// To Play, Stop, Skip and go back (Previous) multiple sound file based on selected items
            /// </summary>
         /*   public static void PlayBackFunctionMultiple(string playBackControl)
            {
                uint numberOfSongsToPlay = (uint)selectedItemsList.Length;

                switch (playBackControl)
                {
                    case "PlayStop":


                        int playStopNumberCount = 0;
                        if (mediaPlayer.PlaybackState == PlaybackState.Playing)
                        {
                            mediaPlayer.Stop();
                            song.Position = 0;

                            playStopNumberCount++;

                        }
                        else
                        {
                            if (playStopNumberCount > 0) { selectedMusicsQue--; }
                            else { selectedMusicsQue++; }

                            mediaPlayer.Init(song);
                            mediaPlayer.Play();

                        }


                        break;

                    case "Skip":
                        selectedMusicsQue++;
                        mediaPlayer.Stop();
                        song.Position = 0;
                        mediaPlayer.Init(song);
                        mediaPlayer.Play();
                        break;

                    case "Previous":
                        selectedMusicsQue--;
                        mediaPlayer.Stop();
                        song.Position = 0;
                        mediaPlayer.Init(song);
                        mediaPlayer.Play();

                        break;
                }

                numberOfSongsToPlay--;
            }
         */

            ///<summary>
            ///To Play, Stop, Skip and go back (Previous) multiple sound file based on selected items
            ///</summary>summary>
            public static void MusicPlayBackControl(string playBackControl)
            {
                uint numberOfSongsToPlay = (uint)selectedItemsList.Length;

                switch (playBackControl)
                {
                    case "PlayStop":
                        int playStopNumberCount = 0;
                        if (mediaPlayer.PlaybackState == PlaybackState.Playing)
                        {
                            mediaPlayer.Stop();

                            song.Position = 0;

                            if (numberOfSongsToPlay > 1)
                            { playStopNumberCount++; }
                            song.Dispose();
                            mediaPlayer.Dispose();
                            song = new AudioFileReader(selectedItemsList[selectedMusicsQue][4]);
                            mediaPlayer = new WaveOutEvent();

                        }
                        else
                        {
                            song.Dispose();
                            mediaPlayer.Dispose();
                            if (playStopNumberCount > 0) { selectedMusicsQue--; }

                            song = new AudioFileReader(selectedItemsList[selectedMusicsQue -1][4]);
                            mediaPlayer = new WaveOutEvent();
                            mediaPlayer.Init(song);
                            mediaPlayer.Play();
                            if (playStopNumberCount == 0) { selectedMusicsQue++; }
                            playStopNumberCount = 0;

                        }
                        break;

                    case "Skip":
                        mediaPlayer.Stop();
                        song.Position = 0;

                        ListViewItem currentSong = new ListViewItem(selectedItemsList[selectedMusicsQue]);
                        int currentSongIndex = currentPlaylist.Items.IndexOf(currentSong);
                        selectedMusicsQue++;
                        /*
                            currentSongIndex++;
                            if (currentSongIndex > currentPlaylist.Items.Count - 1) { currentSongIndex = 0; }

                            var tempSelected = currentPlaylist.SelectedItems[currentSongIndex].SubItems;
                            string[] nextSong =
                                {
                                tempSelected[0].ToString(),
                                tempSelected[1].ToString(),
                                tempSelected[2].ToString(),
                                tempSelected[3].ToString(),
                                tempSelected[4].ToString(),
                                tempSelected[5].ToString(),
                                tempSelected[6].ToString(),

                             };

                            selectedItemsList[selectedMusicsQue] = nextSong;
                        */
                        song.Dispose();
                        mediaPlayer.Dispose();
                        if (isLoopEnabled == true && selectedMusicsQue > selectedItemsList.Length)
                        {
                            selectedMusicsQue = 0;
                            song = new AudioFileReader(selectedItemsList[selectedMusicsQue][4]);
                            mediaPlayer = new WaveOutEvent();

                            mediaPlayer.Init(song);
                            mediaPlayer.Play();
                        }
                        else
                        {
                            MessageBox.Show("End of Playlist Reached");
                        }




                        break;

                    case "Previous":
                        mediaPlayer.Stop();
                        song.Position = 0;
                        selectedMusicsQue--;
                        /*
                            currentSong = new ListViewItem(selectedItemsList[selectedMusicsQue]);
                            currentSongIndex = currentPlaylist.Items.IndexOf(currentSong);

                            if (currentSongIndex < 0) { currentSongIndex = currentPlaylist.Items.Count - 1; }

                             tempSelected = currentPlaylist.SelectedItems[currentSongIndex].SubItems;
                            string[] previousSong =
                                {
                                tempSelected[0].ToString(),
                                tempSelected[1].ToString(),
                                tempSelected[2].ToString(),
                                tempSelected[3].ToString(),
                                tempSelected[4].ToString(),
                                tempSelected[5].ToString(),
                                tempSelected[6].ToString(),

                             };
                            selectedItemsList[selectedMusicsQue] = previousSong;
                        */

                        song.Dispose();
                        mediaPlayer.Dispose();
                        if (isLoopEnabled == true && selectedMusicsQue <= 0)
                        {
                            song = new AudioFileReader(selectedItemsList[selectedMusicsQue][4]);
                            mediaPlayer = new WaveOutEvent();
                            mediaPlayer.Init(song);
                            mediaPlayer.Play();
                        }
                        else
                        {
                            MessageBox.Show("End of Playlist Reached");
                        }
                        break;

                }
                //After a music is played, skiped, etc


            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Initiates the Data Base when Application loads
            //AudioFileData.DataBaseInit();
            if (System.IO.File.Exists(allMusicDataBase))
            {
                if (System.IO.File.ReadAllText(allMusicDataBase) != "")
                {
                    AudioDataBase.ReadAudioDataBase();
                    foreach (var musicInfos in allMusicInfo)
                    {
                        musicListView.Items.Add(new ListViewItem(musicInfos));
                    }

                    musicListView.Refresh();
                }
            }


        }

        private void renameTextBox_Click(object sender, EventArgs e)
        {

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
                    MessageBox.Show("Type Playlist's Name");
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
            if (System.IO.File.Exists(allMusicDataBase))
            {
                if (System.IO.File.ReadAllText(allMusicDataBase) != "")
                {
                    // fakeFullMusicPathList = fullMusicPathList;
                    string[] musicspaths = System.IO.File.ReadAllLines(allMusicDataBase);
                    // fullMusicPathList = MusicList.FullMusicList(musicspaths);
                }

            }
            if (!System.IO.File.Exists(allMusicDataBase))
            {
                using (System.IO.File.Create(allMusicDataBase))
                { }

            }
            if (System.IO.File.ReadAllText(allMusicDataBase) == "")
            {
                MessageBox.Show("Please add music ");
            }
            //fullMusicPathList.Refresh();
        }

        private void ClearListItem_Click(object sender, EventArgs e)
        {
            musicListView.Items.Clear();
            fullMusicPathArray.Clear();
            System.IO.File.WriteAllText(allMusicDataBase, "");
        }

        private void musicListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            fakeFullMusicPathList = musicListView;
            //Array.Clear(selectedItemsList);
            //Array.Clear(selectedItemsListIndex);
            //Array.Clear(selectedMusics);
            selectedItemsList = new string[musicListView.SelectedItems.Count][];
            selectedItemsListIndex = new int[musicListView.SelectedItems.Count];

            int i = 0;
            foreach (var item in musicListView.SelectedItems)
            {
                //selectedItemsList[i] = musicListView.SelectedItems[i].SubItems.
                /*var tempSelected = musicListView.SelectedItems[i].SubItems;
                string[] tempSelectedSubArray =
                    { tempSelected[0].ToString(),
                      tempSelected[1].ToString(),
                      tempSelected[2].ToString(),
                      tempSelected[3].ToString(),
                      tempSelected[4].ToString(),
                      tempSelected[5].ToString(),
                      tempSelected[6].ToString(),

                    };
                selectedItemsList[i] = tempSelectedSubArray;*/
                selectedItemsList[i] = allMusicInfo[musicListView.SelectedItems[i].Index];

                selectedItemsListIndex[i] = musicListView.SelectedItems[i].Index;

                if (allMusicArts[musicListView.SelectedItems[i].Index] != Array.Empty<Image>())
                { pictureBox1.Image = allMusicArts[musicListView.SelectedItems[i].Index][0]; }
                i++;
            }
            int fakeFullMusicPathListCountSelected = fakeFullMusicPathList.SelectedItems.Count;

            int x = 0;
            /*
            string[] tempstringarray = new string[selectedItemsListIndex.Length];
            foreach (int indexItem in selectedItemsListIndex)
            {
                tempstringarray[x] = fullMusicPathArray[indexItem].ToString();
                x++;
            }
            selectedMusics = tempstringarray;
            if (selectedMusics.Length > 1) { multipleSelectedItems = true; }
            */
            selectedMusics = selectedItemsList;
            if (selectedMusics.Length > 1) { multipleSelectedItems = true; }
        }

        private void musicListView_DragDrop(object sender, DragEventArgs e)
        {
            string[] draggedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            //string[] musicPaths = AudioFileData.WriteToDataBase(draggedFiles);
            //ListView templist = new ListView();
            // templist = MusicList.FullMusicList(musicPaths);
            //musicListView = templist;
            //AudioFileData.GetAudioFilesInfo();
            AudioDataBase myDataBase = new AudioDataBase(draggedFiles);
            //AllSongsPlaylist = tempListView;
            //musicListView = AllSongsPlaylist;
            //musicListView.Items.Clear();
            foreach (var musicInfos in allMusicInfo)
            {
                musicListView.Items.Add(new ListViewItem(musicInfos));
            }

            musicListView.Refresh();



        }

        private void musicListView_DragOver(object sender, DragEventArgs e)
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

            if (e.Button == MouseButtons.Right)
            {
                if (e.Location.IsEmpty == false)
                {
                    // FOR DOING STUFF TO THAT 1 PARTICULAR ITEM
                    musicListView.FindNearestItem(SearchDirectionHint.Left, e.Location);
                }
            }
        }

        private void musicListView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
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

        private void PlayStop_Click(object sender, EventArgs e)
        {

            PlayBackFunction playBack = new PlayBackFunction(AllSongsPlaylist);
            PlayBackFunction.MusicPlayBackControl("PlyStop");

                lyricsTextBox.Text = musicListView.SelectedItems[0].SubItems[6].Text;
            
            

        }
    }

}
