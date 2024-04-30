using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khi_Player
{
    public class SharedFieldsAndVariables
    {
        public enum Playlists { allSongs, searchPlaylist, DynamicPlaylists };
        public static Playlists CurrentPlaylist = Playlists.allSongs;

        public enum LoopStates { NoLoop, SingleSongLoop, PlaylistLoop };
        public static LoopStates LoopState = LoopStates.NoLoop;

        public enum SortOrders { TitleSort, ArtistSort, AlbumSort, CustomSort };
        public static SortOrders SortOrder = SortOrders.CustomSort;

        public static bool playingSelectedSong = false;
        public enum States : int { Setup = 0, Playing = 1, Paused = 2, Stopped = 3, Finished = 4 };
        public static States Status = States.Setup;
        public static string[][]? PlaylistQueue;
        public static int? playingSongIndex;
        public static int[]? shuffledIndices; // if shuffle in enabled, this will be shuffled, otherwise, it's just a list of indices
        public static List<int> playedSongs = new List<int>();
        public static AudioFileReader song;
        public static WaveOutEvent mediaPlayer = new WaveOutEvent();

        public static Dictionary<string, string[][]?> PlaylistsDict = new Dictionary<string, string[][]?>();
        public static string? CurrentPlaylistName;

        public static string[][]? allMusicInfo = new string[1][]; //just for initilization
        public static Image[]? allMusicArts = new Image[1]; //same thing

        public static string[][]? searchPlaylist = new string[1][];
        public static string[][]? PlaylistMusicInfo = new string[1][];

        public static uint selectedMusicsQue = 0; //only accepts >= 0 value, not negative integers

        public static string applicationPath = Application.StartupPath;
        public static string allMusicDataBase = Application.StartupPath + "AllMusicDataBase.xml";
        public static string albumArtsPath = Application.StartupPath + "\\Album Arts\\";
        public static string albumArtsThumbnailsPath = Application.StartupPath + "Album Arts Thumbnails\\";

        public static List<string[]> selectedItems = new List<string[]>();
        public static List<int>? selectedItemsIndices = new List<int>();

        public static string[] currentlySelectedSong = new string[6];
        public static int currentlySelectedSongIndex = 0;
        public static string[] currentlyPlayingSongInfo = new string[6];
        public static int currentlyPlayingSongIndex; // keeps the actual index not the shuffled one. this is to make it easier to remove items and avoid redundancies since keeping
                                              // a single int in memory is better than searching for it everytime
        public static Image? currentlyPlayingSongPic;
        public static string? songLength;

        public static int timeValue = 0;
        public static int seekbarMax;
        public static int seekBarFinalValue;
        public static int seekBarValueBeforeMove;
        public static string? currenSongTimePosition;
        public static int lastSongTimePosition = 0;

        public static string? lyrics = "Oops! No Embedded Lyrics";

        public static bool noSongSelected = false;
        public static bool listUpdated = false;
        public static bool isDarkMode = false;
        public static bool isShuffled = false;
        public static bool isShuffleEnabled = false;
        public static bool isLoopEnabled = false;
    }
}
