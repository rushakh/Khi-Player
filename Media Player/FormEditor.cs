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
                CurrentForm.editPlaylistButton.BackColor = Color.FromArgb(41, 41, 41);
                CurrentForm.editPlaylistButton.ForeColor = Color.White;
                CurrentForm.applyEditStripButton.BackColor = Color.FromArgb(41, 41, 41);
                CurrentForm.applyEditStripButton.ForeColor = Color.White;
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
                CurrentForm.editPlaylistButton.BackColor = Color.White;
                CurrentForm.editPlaylistButton.ForeColor = Color.FromArgb(41, 41, 41);
                CurrentForm.applyEditStripButton.BackColor = Color.White;
                CurrentForm.applyEditStripButton.ForeColor = Color.FromArgb(41, 41, 41);

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
                    ListViewItem[] newItems = new ListViewItem[musicInfos.Length];
                    int x = 0;
                    foreach (string[]? music in musicInfos)
                    {
                        ListViewItem song = new ListViewItem(music);
                        song.Name = music[3];
                        song.ToolTipText = music[0] + System.Environment.NewLine + music[1] + System.Environment.NewLine + music[2];
                        newItems[x] = song;
                        //listview.Items.Add(song);
                        //song = null;
                    }
                    listview.Items.AddRange(newItems);
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

                    if (musicThumbnails != null)
                    {
                        listview.LargeImageList.Images.AddRange(musicThumbnails);
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
                        //listview.Items.Add(song);
                        i++;
                        x++;
                        //song = null;
                    }
                    CurrentForm.musicListView.Items.AddRange(newItems);
                    //listview.Items.AddRange(newItems); 
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
}
