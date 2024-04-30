using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Khi_Player.SharedFieldsAndVariables;

namespace Khi_Player
{
    ///<summary> 
    /// For music playback controls, music queue, and relevant matters.
    /// </summary>
    public class PlayBackFunction
    {
        private static Random random = new Random();

        public PlayBackFunction()
        {
            PlaylistQueue = PlayList.GetCurrentPlaylist(CurrentPlaylistName);
        }

        public static void SongTimeValue(bool getFullLength = false)
        {
            try
            {
                if (getFullLength)
                {
                    if (song.TotalTime.Hours > 0)
                    { songLength = song.TotalTime.ToString("hh\\:mm\\:ss"); }
                    else
                    { songLength = song.TotalTime.ToString("mm\\:ss"); }
                    seekbarMax = (int)Math.Round(song.TotalTime.TotalSeconds, MidpointRounding.ToZero);
                }
                if (song != null && mediaPlayer.PlaybackState == PlaybackState.Playing && song.CurrentTime.TotalSeconds <= song.TotalTime.TotalSeconds)
                {
                    timeValue = (int)Math.Round(song.CurrentTime.TotalSeconds, MidpointRounding.ToZero);
                    if (timeValue >= 3600) { currenSongTimePosition = song.CurrentTime.ToString("hh\\:mm\\:ss"); }
                    else { currenSongTimePosition = song.CurrentTime.ToString("mm\\:ss"); }
                }
                else if (song != null && mediaPlayer.PlaybackState == PlaybackState.Playing && song.CurrentTime.TotalSeconds > song.TotalTime.TotalSeconds)
                {
                    timeValue = (int)Math.Round(song.TotalTime.TotalSeconds, MidpointRounding.ToZero);
                    if (timeValue >= 3600) { currenSongTimePosition = song.TotalTime.ToString("hh\\:mm\\:ss"); }
                    else { currenSongTimePosition = song.TotalTime.ToString("mm\\:ss"); }
                }
            }
            catch (Exception ex) 
            { }
        }

        /// <summary>
        /// creates a new instance of WaveOutEvent and AudioFileReader, plays the new song with the latency set to 100 ms, 
        /// and also optionally disposes the previous instances of WaveOutEvent and AudioFileReader 
        /// </summary>
        /// <param name="songPath"></param>
        /// <param name="disposePrevious"></param>
        private static void LoadAndPlayNewSong(string songPath, bool disposePrevious = true)
        {
            if (disposePrevious == true)
            {
                if (song != null && mediaPlayer != null)
                {
                    song.Dispose();
                    mediaPlayer.Dispose();
                    currentlyPlayingSongInfo = null;
                    currentlyPlayingSongPic = null;
                }
            }
            song = new AudioFileReader(songPath);
            mediaPlayer = new WaveOutEvent();
            mediaPlayer.NumberOfBuffers = 3;
            mediaPlayer.DesiredLatency = 200;
            mediaPlayer.Init(song);
            mediaPlayer.Play();
            Status = States.Playing;
        }

        /// <summary>
        /// checks if the list has been updated and/or is shuffled
        /// </summary>
        private static void CheckUpdateAndShuffle()
        {
            if (listUpdated == true)
            {
                PlaylistQueue = PlayList.GetCurrentPlaylist(CurrentPlaylistName);
                isShuffled = false;
                listUpdated = false;
            }
            if (isShuffleEnabled == true)
            {
                if (isShuffled == false) { ShufflePlaylist(); }
            }
        }

        /// <summary>
        /// shuffles the current playlist and orginizes the old song indices and the new ones
        /// </summary>
        public static void ShufflePlaylist()
        {
            //originalAndShuffledIndices.Clear();
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
            isShuffled = true;

            //for disposal
            randomIndex = null;
            //tempList = null;
        }

        /// <summary>
        /// plays the selected music or pauses the playing music
        /// </summary>
        public static void PlayPauseMusic()
        {
            CheckUpdateAndShuffle();
            if (mediaPlayer.PlaybackState == PlaybackState.Playing) // will pause the playback if music is being played
            {
                mediaPlayer.Pause();
                Status = States.Paused;

                if (currentlySelectedSongIndex != null && currentlyPlayingSongInfo != PlaylistQueue[currentlySelectedSongIndex])
                {
                    LoadAndPlayNewSong(PlaylistQueue[currentlySelectedSongIndex][3], true);
                    selectedMusicsQue = (uint)currentlySelectedSongIndex;
                    playingSelectedSong = true;
                }
            }
            else // will play the selected song (or the entire playlistQue) or resume the paused playback
            {
                if (mediaPlayer.PlaybackState == PlaybackState.Paused) // will resume playback if it is paused or play the recently selected song
                {
                    if (currentlyPlayingSongInfo != PlaylistQueue[currentlySelectedSongIndex]) // if playback is paused AND a new song is selected will play the new song instead of resuming
                    {
                        LoadAndPlayNewSong(PlaylistQueue[currentlySelectedSongIndex][3], true);
                        selectedMusicsQue = (uint)currentlySelectedSongIndex;
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
                    LoadAndPlayNewSong(PlaylistQueue[currentlySelectedSongIndex][3]);
                    playingSelectedSong = true;
                }
            }
            SetPlayingMusicArtAndInfo();
        }

        /// <summary>
        /// skips to the next song in the playlistQue, the pecifics depends on the current state of loop and shuffle
        /// </summary>
        public static void SkipMusic()
        {
            CheckUpdateAndShuffle();
            string? dupliPlayCheck;
            if (song != null) { dupliPlayCheck = song.FileName; }
            else              { dupliPlayCheck = null; }

            if (isLoopEnabled == true) // if loop is enabled 
            {
                //if loop state is set to single song loop does nothing. if loop is enabled and set to
                //playlist Loop, skips to the next song in the playlist and incase the playlist has ended,
                //reshuffles the playlistQue and begins playing from the beginning of the playlist
                if (LoopState != LoopStates.SingleSongLoop) 
                {
                    selectedMusicsQue++;
                    if (selectedMusicsQue > (PlaylistQueue.Length - 1))
                    {
                        selectedMusicsQue = 0;
                        if (isShuffleEnabled) { ShufflePlaylist(); }
                    }
                }
                string newSong;
                if (isShuffleEnabled == true) // if shuffle is enabled
                {
                    // if shuffle is enabled and the next song is the same as
                    // the previous song (in case shuffle was enabled after playback
                    // had already started) will skip once more
                    if (dupliPlayCheck != null && PlaylistQueue[shuffledIndices[selectedMusicsQue]][3] == dupliPlayCheck)
                    { selectedMusicsQue++; }
                    newSong = PlaylistQueue[shuffledIndices[selectedMusicsQue]][3];
                }
                // if shuffle is not enabled, will simply start the new song
                else { newSong = PlaylistQueue[selectedMusicsQue][3]; }
                LoadAndPlayNewSong(newSong, true);
            }
            else // if loop is not enabled skips to the next song. in case the currently playing song was the last in que, will
                 // declare the end of playlist.
            {
                selectedMusicsQue++;
                if (selectedMusicsQue > PlaylistQueue.Length - 1)
                {
                    mediaPlayer.Dispose();
                    song.Dispose();
                    currentlyPlayingSongPic = null;
                    currentlyPlayingSongInfo = null;
                    System.Windows.Forms.MessageBox.Show("End of Playlist Reached \r\n Enable Loop for unintrupted playback");
                    Status = States.Finished;
                    selectedMusicsQue = 0;
                }
                else
                {
                    string newSong;
                    if (isShuffleEnabled == true) { newSong = PlaylistQueue[shuffledIndices[selectedMusicsQue]][3]; }
                    else { newSong = PlaylistQueue[selectedMusicsQue][3]; }
                    LoadAndPlayNewSong(newSong, true);
                }
            }
            SetPlayingMusicArtAndInfo();
        }

        /// <summary>
        /// functions the same way as Skip but just .. well goes to the previous song instead
        /// </summary>
        public static void PreviousMusic()
        {
            CheckUpdateAndShuffle();
            string? duplPlayCheck;
            if (song != null) { duplPlayCheck = song.FileName; }
            else { duplPlayCheck = null; }

            if (isLoopEnabled == true && selectedMusicsQue == 0)
            {
                selectedMusicsQue = (uint)PlaylistQueue.Length - 1;
                string newSong;
                if (isShuffleEnabled == true)
                {
                    if (duplPlayCheck != null && PlaylistQueue[shuffledIndices[selectedMusicsQue]][3] == duplPlayCheck)
                    { selectedMusicsQue--; }
                    newSong = PlaylistQueue[shuffledIndices[selectedMusicsQue]][3];
                }
                else { newSong = PlaylistQueue[selectedMusicsQue][3]; }
                LoadAndPlayNewSong(newSong, true);
            }
            else
            {
                if (selectedMusicsQue == 0)
                {
                    song.Dispose();
                    mediaPlayer.Dispose();
                    currentlyPlayingSongInfo = null;
                    currentlyPlayingSongPic = null;
                    System.Windows.Forms.MessageBox.Show("End of Playlist Reached \r\n Enable Loop for unintrupted playback");
                    Status = States.Finished;
                    selectedMusicsQue = 0;
                }
                else
                {
                    selectedMusicsQue--;
                    string newSong;
                    if (isShuffleEnabled == true)
                    {
                        if (duplPlayCheck != null && PlaylistQueue[shuffledIndices[selectedMusicsQue]][3] == duplPlayCheck)
                        { selectedMusicsQue--; }
                        newSong = PlaylistQueue[shuffledIndices[selectedMusicsQue]][3];
                    }
                    else { newSong = PlaylistQueue[selectedMusicsQue][3]; }
                    LoadAndPlayNewSong(newSong, true);
                }
            }
            SetPlayingMusicArtAndInfo();
        }

        /// <summary>
        /// stops playback if music was playing or was paused
        /// </summary>
        public static void StopMusic()
        {
            if (Status == States.Playing || Status == States.Paused)
            {
                mediaPlayer.Stop();
                Status = States.Stopped;
            }
        }

        /// <summary>
        /// After a music is played, skiped, etc. will make set the value of of the currently playing song info and cover art 
        /// so it can be used by other functions. The condition for this behavior is set to playing to avoid setting the value 
        /// again if a song that was paused is unpaused
        /// </summary>
        private static void SetPlayingMusicArtAndInfo()
        {
            if (Status == States.Playing)
            {
                System.IO.FileStream imageStream;  //using file stream so that no connection remains with the file itself that can cause error if the user wants to remove a song
                string imagePath;
                currentlyPlayingSongIndex = (int)selectedMusicsQue;
                try
                {
                    if (playingSelectedSong == true)
                    {
                        currentlyPlayingSongInfo = PlaylistQueue[selectedMusicsQue];
                        imagePath = currentlyPlayingSongInfo[4];
                        using (imageStream = new System.IO.FileStream(imagePath, FileMode.Open))
                        {
                            currentlyPlayingSongPic = (Image)Image.FromStream(imageStream).Clone();
                        }
                        imageStream.Dispose();
                        playingSelectedSong = false;
                    }
                    else if (isShuffleEnabled)
                    {
                        currentlyPlayingSongInfo = PlaylistQueue[shuffledIndices[(int)selectedMusicsQue]];
                        imagePath = currentlyPlayingSongInfo[4];
                        using (imageStream = new System.IO.FileStream(imagePath, FileMode.Open))
                        {
                            currentlyPlayingSongPic = (Image)Image.FromStream(imageStream).Clone();
                        }
                        imageStream.Dispose();
                    }
                    else
                    {
                        currentlyPlayingSongInfo = PlaylistQueue[selectedMusicsQue];
                        imagePath = currentlyPlayingSongInfo[4];
                        using (imageStream = new System.IO.FileStream(imagePath, FileMode.Open))
                        {
                            currentlyPlayingSongPic = (Image)Image.FromStream(imageStream).Clone();
                        }
                        imageStream.Dispose();
                    }
                }
                catch { }
                SongTimeValue(true);
                timeValue = 0;

                try
                {
                    using (TagLib.File lyrictag = TagLib.File.Create(currentlyPlayingSongInfo[3]))
                    {
                        lyrics = lyrictag.Tag.Lyrics;
                        if (lyrics != null && lyrics != "")
                        { lyrics = lyrics.ReplaceLineEndings(); }
                        else { lyrics = "Oops! No Embedded Lyrics"; }
                    }
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("File Error, please remove this file from the application");
                    //throw;
                }
            }
        }
    }
}
