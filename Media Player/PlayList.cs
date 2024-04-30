using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Khi_Player.SharedFieldsAndVariables;

namespace Khi_Player
{
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
        /// returns the item info that exists in the existing playlists (a string array containing 6 strings) that corresponds
        /// to the provided index. if the index does not exist in the playlist returns null.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string[] GetItemInfoUsingIndex(int index)
        {
            string[]? item = null;
            string[][]? playlist;

            if (CurrentPlaylist == Playlists.allSongs)
            {
                playlist = PlayList.GetCurrentPlaylist();
            }
            else if (CurrentPlaylist == Playlists.DynamicPlaylists)
            {
                playlist = PlayList.GetPlaylist(null, CurrentPlaylistName);
            }
            else
            {
                if (CurrentPlaylistName != null && PlaylistsDict.ContainsKey(CurrentPlaylistName))
                {
                    playlist = PlayList.GetPlaylist(null, CurrentPlaylistName);
                }

                else
                {
                    playlist = PlayList.GetCurrentPlaylist();
                }
            }

            if (playlist.Length > index)
            {
                item = (string[]?)playlist[index].Clone();
            }
            return item;
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
}
