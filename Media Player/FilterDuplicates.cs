using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Khi_Player.SharedFieldsAndVariables;
using System.Xml;

namespace Khi_Player
{
    /// <summary>
    /// a class for comparing newly acquired Music infos (string[][]) with the info within the Databases
    /// and removing the duplicates. can also check the data bases directly and remove duplicates or try to repair them
    /// </summary>
    public class FilterDuplicates
    {
        /// <summary>
        /// checks the entire Database for duplicates, removes them, and reads the new database in complete mode, optionally 
        /// retyrnung the info sorted
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static (string[][]?, Image[]?) TryRepairingDataBase(SortOrders sort = SortOrders.CustomSort)
        {
            CheckDataBaseAndRemoveDuplicates();
            string[][]? checkedDataBaseInfo;
            Image[]? Arts;
            checkedDataBaseInfo = AudioDataBase.ReadAudioDataBase("complete");
            if (sort != SortOrders.CustomSort) { checkedDataBaseInfo = PlayList.SortPlaylist(checkedDataBaseInfo, (int)sort); }
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
                        else if (AllSongs.ChildNodes[i].ChildNodes[0].InnerText == music[0]
                                && AllSongs.ChildNodes[i].ChildNodes[1].InnerText == music[1]
                                && AllSongs.ChildNodes[i].ChildNodes[2].InnerText == music[2])
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

                MusicDataBase.Save(allMusicDataBase);
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
                    else if (playlistSongs.ChildNodes[i].ChildNodes[0].InnerText == selectedMusicInfo[0]
                            && playlistSongs.ChildNodes[i].ChildNodes[1].InnerText == selectedMusicInfo[1]
                            && playlistSongs.ChildNodes[i].ChildNodes[2].InnerText == selectedMusicInfo[2])
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

        /// <summary>
        /// Compares the musicInfos against the provided playlist and filters the duplicates
        /// </summary>
        /// <param name="selectedMusicsInfo"></param>
        /// <param name="playlistPath"></param>
        /// <returns></returns>
        public static string[][]? FilterPlaylistDuplicates(string[][] selectedMusicsInfo, string? playlistPath)
        {
            List<string[]> tempChecked = new List<string[]>();
            string[][]? checkedMusicInfo;
            List<string[]> filesList = new List<string[]>();
            filesList.Clear();

            if (System.IO.File.Exists(playlistPath) && selectedMusicsInfo != null && selectedMusicsInfo.Length > 0)
            {
                if (selectedMusicsInfo[0] == null)
                {
                    int i = 0;
                    checkedMusicInfo = selectedMusicsInfo;
                    return checkedMusicInfo;
                }
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
                        else if (playlistSongs.ChildNodes[i].ChildNodes[0].InnerText == music[0]
                            && playlistSongs.ChildNodes[i].ChildNodes[1].InnerText == music[1]
                            && playlistSongs.ChildNodes[i].ChildNodes[2].InnerText == music[2])
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
}
