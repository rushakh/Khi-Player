using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Khi_Player.SharedFieldsAndVariables;
using System.Xml.Serialization;
using System.Xml;

namespace Khi_Player
{
    ///<summary> 
    /// For creating, writing to, and reading from the database and the manipulation of the elements within it.
    /// </summary>
    public class AudioDataBase
    {
        private static bool ThumbnailCallback()
        {
            return false;
        }

        /// <summary>
        /// Gets the various properties of selected Audio Files  (e.g., title, artist, album, etc)
        /// </summary>
        /// <param name="draggedMusicsPaths"></param>
        /// <returns></returns>
        private static string[][]? GetAudioFilesInfo(string[] draggedMusicsPaths)
        {
            //1-title 2-artist 3-album 4-path 5- art path 6-thumbnail path 
            string[][] tempMusicInfos = new string[draggedMusicsPaths.Length][];
            try
            {
                int i = 0;
                foreach (var audioPath in draggedMusicsPaths)
                {
                    string? artist, album;
                    string title, path; // these can't be empty

                    string[] musicInfo = new string[4];

                    using (TagLib.File musicTags = TagLib.File.Create(audioPath, TagLib.ReadStyle.None))
                    {
                        //Check to see if the file or file tags are corrupted
                        if (musicTags.PossiblyCorrupt)
                        {
                            var corruptionReasons = musicTags.CorruptionReasons.ToArray();
                            // Add that it should return this string array if it is corrupted
                        }
                        else
                        {
                            path = audioPath;
                            if (musicTags.Tag.Title == null)
                            {
                                System.IO.FileInfo sth = new System.IO.FileInfo(path);
                                title = (string)sth.Name.Clone();
                            }
                            else { title = musicTags.Tag.Title; }
                            //for artists
                            var allArtists = musicTags.Tag.Performers;
                            if (allArtists.Length == 0)
                            {
                                artist = "";
                            }
                            else if (allArtists.Length > 1)
                            {
                                System.Windows.Forms.TextBox tempText = new System.Windows.Forms.TextBox();
                                foreach (var oneartist in allArtists)
                                {
                                    tempText.AppendText(oneartist);
                                    tempText.AppendText(" ");
                                }
                                artist = (string?)tempText.Text.Clone();
                                tempText.Dispose();
                            }
                            else { artist = (string?)musicTags.Tag.FirstPerformer.Clone(); }
                            //For Album
                            if (musicTags.Tag.Album == null) { album = ""; }
                            else { album = (string?)musicTags.Tag.Album.Clone(); }

                            musicInfo[0] = title;
                            musicInfo[1] = artist;
                            musicInfo[2] = album;
                            musicInfo[3] = path;
                            tempMusicInfos[i] = (string[]?)musicInfo.Clone();

                            musicTags.Dispose();
                        }
                        i++;
                    }
                }
            }
            catch (Exception e)
            {

            }
            string[][]? addedMusicInfo = (string[][]?)tempMusicInfos.Clone();

            return addedMusicInfo;
        }

        /// <summary>
        /// turns the musicInfo array, turns the elements into xml elements and writes to the data base  at the provided path
        /// </summary>
        /// <param name="checkedPlaylistMusicData"></param>
        /// <param name="playlistPath"></param>
        /// <param name="nameOfPlaylist"></param>
        private static void XmlDataBaseWriter(string[][]? checkedPlaylistMusicData, string playlistPath, string? nameOfPlaylist)
        {
            string? playlistName;
            //string[][]? checkedPlaylistMusicData;
            string title, artist, album, path, artPath, thumbnailPath;

            XmlDocument playlistDatabase = new XmlDocument();
            XmlElement playlistSongs;  //the document root node
            if (System.IO.File.Exists(playlistPath))
            {
                playlistDatabase.Load(playlistPath);
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

            using (StreamWriter datastream = new StreamWriter(playlistPath, Encoding.UTF8, options))
            {
                XmlWriter dataBaseWriter = XmlWriter.Create(datastream, settings);
                playlistDatabase.Save(dataBaseWriter);
                dataBaseWriter.Dispose();
            }
        }

        /// <summary>
        /// Adds the selected or dragged songs' info to the data base
        /// </summary>
        /// <param name="selectedMusicsInfo"></param>
        /// <returns></returns>
        private static int WriteAudioDataBase(string[][] selectedMusicsInfo)
        {
            string path;
            string? title, artist, album;
            Image? art;
            Image? thumbnail;
            string[][]? selectedMusicsData;
            Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);

            selectedMusicsData = (string[][]?)FilterDuplicates.FilterPlaylistDuplicates(selectedMusicsInfo, allMusicDataBase).Clone();
            if (selectedMusicsData.Length > 0)
            {
                string[]? fileNames = new string[selectedMusicsData.Length];
                List<string[]?> musicData = new List<string[]?>();

                int i = 0;
                foreach (string[] musicDataArray in selectedMusicsData)
                {
                    title = musicDataArray[0];
                    artist = musicDataArray[1];
                    album = musicDataArray[2];
                    path = musicDataArray[3];
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
                        fileNames[i] = (string?)name.Text.Clone();
                    }
                    else
                    {
                        dotcount = 1;
                        fileNames[i] = System.IO.Path.GetFileName(path).Split('.')[dotcount - 1];  //actually incomplete so it can be added to albumArtsPath
                    }
                    // FOR ART
                    using (TagLib.File musicTags = TagLib.File.Create(path))
                    {
                        var tempPics = musicTags.Tag.Pictures;
                        //MemoryStream picConverter = new MemoryStream(tempPics[0].Data.Data);
                        MemoryStream picConverter = new MemoryStream();
                        if (tempPics.Length > 0)
                        {
                            picConverter = new MemoryStream(tempPics[0].Data.Data);
                            if (picConverter.CanRead == false || picConverter.CanWrite == false || tempPics[0].Type == TagLib.PictureType.NotAPicture)
                            {
                                art = Khi_Player.Properties.Resources.Khi_Player;
                                thumbnail = Khi_Player.Properties.Resources.Khi_Player.GetThumbnailImage(60, 60, myCallback, 0);
                            }
                            else
                            {
                                art = (Image)Image.FromStream(picConverter).Clone();
                                thumbnail = art.GetThumbnailImage(60, 60, myCallback, 0);
                            }
                        }
                        else
                        {
                            art = Khi_Player.Properties.Resources.Khi_Player;
                            thumbnail = Khi_Player.Properties.Resources.Khi_Player.GetThumbnailImage(60, 60, myCallback, 0);
                        }
                        musicTags.Dispose();
                        string imagePath = albumArtsPath + fileNames[i] + ".bmp";
                        string imageThumbnailPath = albumArtsThumbnailsPath + fileNames[i] + ".bmp";

                        using (FileStream artSaver = new FileStream(imagePath, FileMode.Create, FileAccess.ReadWrite))
                        {
                            art.Save(artSaver, art.RawFormat);
                            artSaver.Dispose();
                        }
                        fileNames[i] = imagePath;

                        //for saving thumbnails
                        using (FileStream thumbnailSaver = new FileStream(imageThumbnailPath, FileMode.Create, FileAccess.ReadWrite))
                        {
                            thumbnail.Save(thumbnailSaver, art.RawFormat);
                            thumbnailSaver.Dispose();
                        }
                        picConverter.Dispose();
                        name.Dispose();

                        string[]? songInfo = new string[6];
                        songInfo[0] = (string?)title.Clone();
                        songInfo[1] = (string?)artist.Clone();
                        songInfo[2] = (string?)album.Clone();
                        songInfo[3] = (string?)path.Clone();
                        songInfo[4] = (string?)imagePath.Clone();
                        songInfo[5] = (string?)imageThumbnailPath.Clone();
                        musicData.Add(songInfo);
                    }
                }
                selectedMusicsData = musicData.ToArray();
                XmlDataBaseWriter(selectedMusicsData, allMusicDataBase, "All Songs Playlist");
                int addedCount = selectedMusicsData.Count();

                //for disposal
                musicData.Clear();

                return addedCount;
            }
            else { return 0; }
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
                return playlist;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Use this for now: Async, Adds the selected songs' info to the specified playlist's data base
        /// </summary>
        /// <param name="playlistMusicData"></param>
        /// <param name="playlistPath"></param>
        /// <param name="nameOfPlaylist"></param>
        public static async void WriteAudioDataBase(string[][]? playlistMusicData, string? playlistPath, string? nameOfPlaylist)
        {
            string[][]? checkedPlaylistMusicData;
            string? playlist = playlistPath;

            // Duplicate Check
            checkedPlaylistMusicData = FilterDuplicates.FilterPlaylistDuplicates(playlistMusicData, playlist);
            if (checkedPlaylistMusicData.Length > 0)
            {
                await Task.Run(() =>
                {
                    XmlDataBaseWriter(checkedPlaylistMusicData, playlist, nameOfPlaylist);
                });
            }
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

            // Duplicate Check
            checkedPlaylistMusicData = FilterDuplicates.FilterPlaylistDuplicates(playlistMusicData, playlist);
            if (checkedPlaylistMusicData.Length > 0)
            {
                //write to data base
                XmlDataBaseWriter(checkedPlaylistMusicData, playlist, nameOfPlaylist);
            }
        }


        /// <summary>
        /// Reads the specified database. returns the name of the playlist, the thumbnails and 
        /// audio info of recently added audio if "added" is included and all of the audio files' if "complete" is included
        /// </summary>
        /// <param name="playlistPath"></param>
        /// <param name="ReadingMode"></param>
        /// <returns></returns>
        public static (string?, string[][]?, Image[]?) ReadPlaylistDataBase(string? playlistPath, string? ReadingMode, int AddedSongCount = 0)
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
            }
            else //in case of "complete", written like this for simplicity since it didn't need to be complicated as it is only a reading process
            {

                XmlSerializer dataBaseSerializer = new XmlSerializer(typeof(string[][]));
                using (StreamReader textReader = new StreamReader(playlistPath, Encoding.UTF8))
                {
                    tempPlaylistMusicInfo = (string[][])dataBaseSerializer.Deserialize(textReader);
                }
            }
            playlistArts = GetMusicThumbnails(tempPlaylistMusicInfo);
            return (playlistName, tempPlaylistMusicInfo, playlistArts);
        }

        /// <summary>
        /// Reads the data base that contains all music files info. returns audio info of recently added audio if "added" 
        /// is included alongside the then umber of songs that were added. Reads all of the audio files' if "complete" is included
        /// </summary>
        /// <param name="ReadingMode"></param>
        /// <param name="AddedSongCount"></param>
        /// <returns></returns>
        public static string[][]? ReadAudioDataBase(string ReadingMode, int AddedSongCount = 0)
        {
            string[][]? tempAllMusicInfos;

            if (ReadingMode == "added")
            {
                int added = AddedSongCount;
                XmlSerializer dataBaseSerializer = new XmlSerializer(typeof(string[][]));
                using (StreamReader textReader = new StreamReader(allMusicDataBase, Encoding.UTF8))
                {
                    tempAllMusicInfos = (string[][])dataBaseSerializer.Deserialize(textReader);
                }
                string[][]? tempAddedMusicInfos = new string[added][];
                int z = 0;
                for (int i = tempAllMusicInfos.Length - added; i < tempAllMusicInfos.Length; i++)
                {
                    tempAddedMusicInfos[z] = (string[])tempAllMusicInfos[i].Clone();
                    z++;
                }
                return tempAddedMusicInfos;
            }
            else
            {
                XmlSerializer dataBaseSerializer = new XmlSerializer(typeof(string[][]));
                using (StreamReader textReader = new StreamReader(allMusicDataBase, Encoding.UTF8))
                {
                    tempAllMusicInfos = (string[][])dataBaseSerializer.Deserialize(textReader);
                }
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
                }
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

            return (playlistName, playlistData);
        }

        /// <summary>
        /// Removes an item (array of subitems) by using the info provided by a full info array (a string array containing title, artist, album, path, artpath and thumbnail path)
        /// from the database, its duplicates and delete its corresponding cover art as well
        /// </summary>
        /// <param name="item"></param>
        /// <param name="fromAllPlaylists"></param>
        /// <param name="playlistName"></param>
        /// <returns></returns>
        private static bool RemoveSongFromDataBase(string[] item, bool fromAllPlaylists = true, string? playlistName = null)
        {
            bool isRemoved = false;
            List<int> similarItemsIndices = new List<int>();
            string title = item[0];
            string? artist = item[1];
            string? album = item[2];
            string path = item[3];
            string? artThumbnailName = item[5];
            string? artFileName = item[4];

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
                    else { existingDatabases.Add(database); }
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
                //to remove the pics
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
            return isRemoved;
        }

        /// <summary>
        /// Removes the selected items from the current playlist data base (from all databases if the current playlist is AllSongs), 
        /// their arts, thumbnails, and the playlist data currently in use. if the selected items contain a song that was being played 
        /// then a new song will be selected to play. 
        /// </summary>
        /// <param name="toBeRemovedItemsIndices"></param>
        /// <param name="wasPlaying"></param>
        /// <param name="allItemsSelected"></param>
        public static async void RemoveSongs(List<int> toBeRemovedItemsIndices, bool wasPlaying = false, bool allItemsSelected = false)
        {
            await Task.Run(() =>
            {
                
                bool removeFromAllPlaylists = false;
                List<string[]?> toBeRemovedItems = new List<string[]?>();
                string[][]? playlist;

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
                List<string[]?> tempPlaylist = new List<string[]?>();

                if (allItemsSelected == true) { playlist = null; }
                else
                {
                    //getting the info of the items that should be removed
                    foreach (var index in toBeRemovedItemsIndices)
                    {
                        if (playlist.Length > index)
                        {
                            string[]? tempItem = (string[]?)playlist[index].Clone();
                            if (tempItem != null)
                            {
                                toBeRemovedItems.Add(tempItem);
                            }
                        }
                    }
                    //to remove the items from the playlist the app is using. Alternatively I can just reRead the database but let's go with this for now
                    if (toBeRemovedItemsIndices.Count == 1)
                    {
                        tempPlaylist = playlist.ToList();
                        tempPlaylist.RemoveAt(toBeRemovedItemsIndices[0]);
                        playlist = tempPlaylist.ToArray();
                    }
                    else
                    {
                        foreach (int index in toBeRemovedItemsIndices)
                        {
                            playlist[index] = null;
                        }
                        foreach (var music in playlist)
                        {
                            if (music != null)
                            {
                                tempPlaylist.Add(music);
                            }
                        }
                        playlist = tempPlaylist.ToArray();
                    }
                    
                    //the trimmed and updated playlist array will replace the current playlist
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
                }
                //to select a new song for playback even a song was playing when remove was clicked
                if (wasPlaying == true)
                {
                    if (allItemsSelected == false && currentlySelectedSong != null && currentlySelectedSong.Length > 0)
                    {
                        if (allItemsSelected) { currentlySelectedSong = null; }
                        else
                        {
                            int i = currentlyPlayingSongIndex;
                            currentlySelectedSong = (string[])playlist[i].Clone();
                        }
                    }
                } //a problem with this is that the music that will be played might be further in the list or behind the original song that was being
                  //played. solving this won't take much time --> put aside for later

                //removing the items, their arts and thumbnails from the data base
                foreach (string[]? item in toBeRemovedItems)
                {
                    AudioDataBase.RemoveSongFromDataBase(item, removeFromAllPlaylists, CurrentPlaylistName);
                }
                //for disposal
                tempPlaylist.Clear();
                toBeRemovedItems.Clear();
                toBeRemovedItemsIndices.Clear();
            });
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
                        artsToRemove.Clear();
                    }
                }
                //to Dispose of
                existingDatabases.Clear();
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
                    using (TagLib.File musicTags = TagLib.File.Create(musicData[i][3]))
                    {
                        var tempPics = musicTags.Tag.Pictures;
                        Image? art;
                        MemoryStream picConverter;

                        //checking the file for embedded Art
                        if (tempPics.Length > 0)
                        {
                            picConverter = new MemoryStream(tempPics[0].Data.Data);
                            art = (Image)Image.FromStream(picConverter).Clone();
                            thumbnail = art.GetThumbnailImage(60, 60, myCallback, 0);

                            using (FileStream artSaver = new FileStream(musicData[i][4], FileMode.Create, FileAccess.ReadWrite))
                            {
                                art.Save(artSaver, art.RawFormat);
                                artSaver.Dispose();
                            }

                            //for thumbnail
                            using (FileStream thumbnailSaver = new FileStream(musicData[i][5], FileMode.Create, FileAccess.ReadWrite))
                            {
                                thumbnail.Save(thumbnailSaver, art.RawFormat);
                                thumbnailSaver.Dispose();
                            }
                            picConverter.Dispose();
                        }
                        else
                        {
                            art = Khi_Player.Properties.Resources.Khi_Player;
                            thumbnail = art.GetThumbnailImage(60, 60, myCallback, 0);
                            using (FileStream artSaver = new FileStream(musicData[i][4], FileMode.Create, FileAccess.ReadWrite))
                            {
                                art.Save(artSaver, art.RawFormat);
                            }

                            //for thumbnail
                            using (FileStream thumbnailSaver = new FileStream(musicData[i][5], FileMode.Create, FileAccess.ReadWrite))
                            {
                                thumbnail.Save(thumbnailSaver, art.RawFormat);
                                thumbnailSaver.Dispose();
                            }
                        }
                        musicTags.Dispose();
                    }
                    thumbnail = Image.FromFile(musicData[i][5]);
                    musicThumbnails[i] = (Image)thumbnail.Clone();
                }
            }
            return musicThumbnails;
        }

        /// <summary>
        /// Creates, writes to, and reads from the main Data Base, using the provided paths of audio files. returns the info and 
        /// thumbnails in the specified sort order, or in the order they were added if not specified. 
        /// </summary>
        /// <param name="songPaths"></param>
        /// <param name="readDataBaseFully"></param>
        /// <returns></returns>
        public static (string[][]?, Image[]?) AddSongsToAudioDataBase(string[]? songPaths, bool readDataBaseFully = false, SortOrders sort = SortOrders.CustomSort)
        {
            if (!System.IO.Directory.Exists(albumArtsPath)) { System.IO.Directory.CreateDirectory(albumArtsPath); }
            if (!System.IO.Directory.Exists(albumArtsThumbnailsPath)) { System.IO.Directory.CreateDirectory(albumArtsThumbnailsPath); }
            string[][]? addedMusicInfoIncomplete = GetAudioFilesInfo(songPaths);
            if (addedMusicInfoIncomplete != null && addedMusicInfoIncomplete.Length > 0 && addedMusicInfoIncomplete[0] != null)
            {
                int AddedSongCount = WriteAudioDataBase(addedMusicInfoIncomplete);

                if (readDataBaseFully == false)
                {
                    string[][]? AddedMusicInfo = ReadAudioDataBase("added", AddedSongCount);
                    if (sort != SortOrders.CustomSort) { AddedMusicInfo = PlayList.SortPlaylist(AddedMusicInfo, (int)sort); }
                    Image[]? AddedMusicArts = GetMusicThumbnails(AddedMusicInfo);
                    return (AddedMusicInfo, AddedMusicArts);
                }
                else
                {
                    string[][]? AllMusicInfo = ReadAudioDataBase("complete");
                    if (sort != SortOrders.CustomSort) { AllMusicInfo = PlayList.SortPlaylist(AllMusicInfo, (int)sort); }
                    Image[]? AllMusicArts = GetMusicThumbnails(AllMusicInfo);
                    return (AllMusicInfo, AllMusicArts);
                }
            }
            else
            {
                string[][]? AllMusicInfo = ReadAudioDataBase("complete");
                if (sort != SortOrders.CustomSort) { AllMusicInfo = PlayList.SortPlaylist(AllMusicInfo, (int)sort); }
                Image[]? AllMusicArts = GetMusicThumbnails(AllMusicInfo);
                return (AllMusicInfo, AllMusicArts);
            }
        }

        /// <summary>
        /// Reads from the data base, Use after the data base has been created.
        /// if the main data base exists and info exists, reads it, and return all the info and 
        /// the corresponding thumbnails, otherwise returns null. 
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static (string[][]?, Image[]?) MainDataBaseIni(SortOrders sort = SortOrders.CustomSort)
        {
            string[][]? AllMusicInfo;
            Image[]? AllMusicArts;
            if (System.IO.File.Exists(allMusicDataBase))
            {
                RemoveInvalidDatabaseElements();
                XmlDocument tempMusicDataBase = new XmlDocument();
                tempMusicDataBase.Load(allMusicDataBase);
                if (tempMusicDataBase.DocumentElement.ChildNodes.Count > 0)
                {
                    AllMusicInfo = ReadAudioDataBase("complete");
                    if (sort != SortOrders.CustomSort) { AllMusicInfo = PlayList.SortPlaylist(AllMusicInfo, (int)sort); }
                    AllMusicArts = GetMusicThumbnails(AllMusicInfo);
                    return ((string[][]?)AllMusicInfo.Clone(), (Image[]?)AllMusicArts.Clone());
                }
                else
                {
                    return (null, null);
                }
            }
            else
            {
                return (null, null);
            }
        }
    }
}
