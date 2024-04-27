namespace Khi_Player
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            userBar = new ToolStrip();
            fileToolStripMenuItem = new ToolStripDropDownButton();
            addMusicsToolStripMenuItem = new ToolStripMenuItem();
            addFolderToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripDropDownButton();
            showLyricsMenuItem = new ToolStripMenuItem();
            showAlbumArtMenuItem = new ToolStripMenuItem();
            showTileViewMenuItem = new ToolStripMenuItem();
            showLargeIconMenuItem = new ToolStripMenuItem();
            showDetailsMenuItem = new ToolStripMenuItem();
            darkModeMenuItem = new ToolStripMenuItem();
            listToolStripMenuItem = new ToolStripDropDownButton();
            SortListMenuItem = new ToolStripMenuItem();
            sortListTitleMenuItem = new ToolStripMenuItem();
            sortListArtistMenuItem = new ToolStripMenuItem();
            sortListAlbumMenuItem = new ToolStripMenuItem();
            RescanMenuItem = new ToolStripMenuItem();
            ClearListItem = new ToolStripMenuItem();
            editLyricsToolStripButton = new ToolStripButton();
            applyEditStripButton = new ToolStripButton();
            clearListMenuItem = new ToolStripMenuItem();
            PlayPause = new Button();
            skip = new Button();
            previous = new Button();
            pictureBox1 = new PictureBox();
            button1 = new Button();
            musicListView = new ListView();
            musicTitle = new ColumnHeader();
            musicArtist = new ColumnHeader();
            musicAlbum = new ColumnHeader();
            rightClickMenu = new ContextMenuStrip(components);
            playMenuItem = new ToolStripMenuItem();
            showItemInfoButton = new ToolStripMenuItem();
            editItemTagsButton = new ToolStripMenuItem();
            removeItemButton = new ToolStripMenuItem();
            addToPlaylistButton = new ToolStripMenuItem();
            artListImageList = new ImageList(components);
            playlistToolbar = new ToolStrip();
            playlistIdentifierLabel = new ToolStripLabel();
            currentPlaylistLabel = new ToolStripLabel();
            playlistLabel = new ToolStripLabel();
            allSongsPlaylist = new ToolStripButton();
            renameTextBox = new ToolStripTextBox();
            addPlaylistButton = new ToolStripButton();
            editPlaylistButton = new ToolStripButton();
            mediaPlayerPanel = new Panel();
            borderLabel = new Label();
            songAlbumLabel = new Label();
            songArtistLabel = new Label();
            songTitleLabel = new Label();
            lyricsTextBox = new TextBox();
            musicControlBar = new Panel();
            stopButton = new Button();
            toggleShuffle = new Button();
            volumeLabel = new Label();
            songLengthLabel = new Label();
            currentTimeLabel = new Label();
            volumeBar = new TrackBar();
            toggleLoop = new Button();
            seekBar = new TrackBar();
            songToolTip = new ToolTip(components);
            songSeekTimer = new System.Windows.Forms.Timer(components);
            MemoryManageTimer = new System.Windows.Forms.Timer(components);
            searchMusicListView = new TextBox();
            userBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            rightClickMenu.SuspendLayout();
            playlistToolbar.SuspendLayout();
            mediaPlayerPanel.SuspendLayout();
            musicControlBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)volumeBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)seekBar).BeginInit();
            SuspendLayout();
            // 
            // userBar
            // 
            userBar.AutoSize = false;
            userBar.BackColor = SystemColors.Window;
            userBar.GripMargin = new Padding(0);
            userBar.GripStyle = ToolStripGripStyle.Hidden;
            userBar.ImageScalingSize = new Size(20, 20);
            userBar.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, viewToolStripMenuItem, listToolStripMenuItem, editLyricsToolStripButton, applyEditStripButton });
            userBar.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            userBar.Location = new Point(0, 0);
            userBar.Name = "userBar";
            userBar.Padding = new Padding(0, 0, 0, 2);
            userBar.RenderMode = ToolStripRenderMode.System;
            userBar.ShowItemToolTips = false;
            userBar.Size = new Size(946, 22);
            userBar.Stretch = true;
            userBar.TabIndex = 0;
            userBar.Text = "UserBar";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.AutoSize = false;
            fileToolStripMenuItem.AutoToolTip = false;
            fileToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addMusicsToolStripMenuItem, addFolderToolStripMenuItem });
            fileToolStripMenuItem.Image = (Image)resources.GetObject("fileToolStripMenuItem.Image");
            fileToolStripMenuItem.ImageScaling = ToolStripItemImageScaling.None;
            fileToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            fileToolStripMenuItem.Margin = new Padding(0);
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(45, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // addMusicsToolStripMenuItem
            // 
            addMusicsToolStripMenuItem.Name = "addMusicsToolStripMenuItem";
            addMusicsToolStripMenuItem.Size = new Size(132, 22);
            addMusicsToolStripMenuItem.Text = "Add Music";
            addMusicsToolStripMenuItem.Click += addMusicsToolStripMenuItem_Click;
            // 
            // addFolderToolStripMenuItem
            // 
            addFolderToolStripMenuItem.Name = "addFolderToolStripMenuItem";
            addFolderToolStripMenuItem.Size = new Size(132, 22);
            addFolderToolStripMenuItem.Text = "Add Folder";
            addFolderToolStripMenuItem.Click += addFolderToolStripMenuItem_Click;
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.AutoSize = false;
            viewToolStripMenuItem.AutoToolTip = false;
            viewToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { showLyricsMenuItem, showAlbumArtMenuItem, showTileViewMenuItem, showLargeIconMenuItem, showDetailsMenuItem, darkModeMenuItem });
            viewToolStripMenuItem.Image = (Image)resources.GetObject("viewToolStripMenuItem.Image");
            viewToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            viewToolStripMenuItem.Margin = new Padding(0);
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(45, 20);
            viewToolStripMenuItem.Text = "View";
            // 
            // showLyricsMenuItem
            // 
            showLyricsMenuItem.Name = "showLyricsMenuItem";
            showLyricsMenuItem.Size = new Size(161, 22);
            showLyricsMenuItem.Text = "Show Lyrics";
            showLyricsMenuItem.Click += showLyricsMenuItem_Click;
            // 
            // showAlbumArtMenuItem
            // 
            showAlbumArtMenuItem.Checked = true;
            showAlbumArtMenuItem.CheckState = CheckState.Checked;
            showAlbumArtMenuItem.Name = "showAlbumArtMenuItem";
            showAlbumArtMenuItem.Size = new Size(161, 22);
            showAlbumArtMenuItem.Text = "Show Album Art";
            showAlbumArtMenuItem.Click += showAlbumArtMenuItem_Click;
            // 
            // showTileViewMenuItem
            // 
            showTileViewMenuItem.Checked = true;
            showTileViewMenuItem.CheckState = CheckState.Checked;
            showTileViewMenuItem.Name = "showTileViewMenuItem";
            showTileViewMenuItem.Size = new Size(161, 22);
            showTileViewMenuItem.Text = "Tile View";
            showTileViewMenuItem.Click += showTileViewMenuItem_Click;
            // 
            // showLargeIconMenuItem
            // 
            showLargeIconMenuItem.Name = "showLargeIconMenuItem";
            showLargeIconMenuItem.Size = new Size(161, 22);
            showLargeIconMenuItem.Text = "Large Icon View";
            showLargeIconMenuItem.Click += showLargeIconMenuItem_Click;
            // 
            // showDetailsMenuItem
            // 
            showDetailsMenuItem.Name = "showDetailsMenuItem";
            showDetailsMenuItem.Size = new Size(161, 22);
            showDetailsMenuItem.Text = "Details View";
            showDetailsMenuItem.Click += showDetailsMenuItem_Click;
            // 
            // darkModeMenuItem
            // 
            darkModeMenuItem.CheckOnClick = true;
            darkModeMenuItem.Name = "darkModeMenuItem";
            darkModeMenuItem.Size = new Size(161, 22);
            darkModeMenuItem.Text = "Dark Mode";
            darkModeMenuItem.Click += darkModeMenuItem_Click;
            // 
            // listToolStripMenuItem
            // 
            listToolStripMenuItem.AutoSize = false;
            listToolStripMenuItem.AutoToolTip = false;
            listToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            listToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { SortListMenuItem, RescanMenuItem, ClearListItem });
            listToolStripMenuItem.Image = (Image)resources.GetObject("listToolStripMenuItem.Image");
            listToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            listToolStripMenuItem.Margin = new Padding(0);
            listToolStripMenuItem.Name = "listToolStripMenuItem";
            listToolStripMenuItem.Size = new Size(45, 20);
            listToolStripMenuItem.Text = "List";
            // 
            // SortListMenuItem
            // 
            SortListMenuItem.DropDownItems.AddRange(new ToolStripItem[] { sortListTitleMenuItem, sortListArtistMenuItem, sortListAlbumMenuItem });
            SortListMenuItem.Name = "SortListMenuItem";
            SortListMenuItem.Size = new Size(180, 22);
            SortListMenuItem.Text = "Sort";
            // 
            // sortListTitleMenuItem
            // 
            sortListTitleMenuItem.Name = "sortListTitleMenuItem";
            sortListTitleMenuItem.Size = new Size(150, 22);
            sortListTitleMenuItem.Text = "Sort By Title";
            sortListTitleMenuItem.Click += sortListTitleMenuItem_Click;
            // 
            // sortListArtistMenuItem
            // 
            sortListArtistMenuItem.Name = "sortListArtistMenuItem";
            sortListArtistMenuItem.Size = new Size(150, 22);
            sortListArtistMenuItem.Text = "Sort By Artist";
            sortListArtistMenuItem.Click += sortListArtistMenuItem_Click;
            // 
            // sortListAlbumMenuItem
            // 
            sortListAlbumMenuItem.Name = "sortListAlbumMenuItem";
            sortListAlbumMenuItem.Size = new Size(150, 22);
            sortListAlbumMenuItem.Text = "Sort By Album";
            sortListAlbumMenuItem.Click += sortListAlbumMenuItem_Click;
            // 
            // RescanMenuItem
            // 
            RescanMenuItem.Name = "RescanMenuItem";
            RescanMenuItem.Size = new Size(180, 22);
            RescanMenuItem.Text = "Rescan and Check";
            RescanMenuItem.ToolTipText = "Scans Database for Duplicates and Reloads it";
            RescanMenuItem.Click += RescanMenuItem_Click;
            // 
            // ClearListItem
            // 
            ClearListItem.Name = "ClearListItem";
            ClearListItem.Size = new Size(180, 22);
            ClearListItem.Text = "Clear List";
            ClearListItem.ToolTipText = "Clears All Songs List";
            ClearListItem.Click += ClearListItem_Click;
            // 
            // editLyricsToolStripButton
            // 
            editLyricsToolStripButton.AutoSize = false;
            editLyricsToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            editLyricsToolStripButton.Enabled = false;
            editLyricsToolStripButton.Image = (Image)resources.GetObject("editLyricsToolStripButton.Image");
            editLyricsToolStripButton.ImageTransparentColor = Color.Magenta;
            editLyricsToolStripButton.Margin = new Padding(350, 1, 0, 2);
            editLyricsToolStripButton.Name = "editLyricsToolStripButton";
            editLyricsToolStripButton.Size = new Size(60, 20);
            editLyricsToolStripButton.Text = "Edit Lyrics";
            editLyricsToolStripButton.Visible = false;
            editLyricsToolStripButton.Click += editLyricsToolStripButton_Click;
            // 
            // applyEditStripButton
            // 
            applyEditStripButton.AutoSize = false;
            applyEditStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            applyEditStripButton.Enabled = false;
            applyEditStripButton.Image = (Image)resources.GetObject("applyEditStripButton.Image");
            applyEditStripButton.ImageTransparentColor = Color.Magenta;
            applyEditStripButton.Name = "applyEditStripButton";
            applyEditStripButton.Size = new Size(40, 20);
            applyEditStripButton.Text = "Apply";
            applyEditStripButton.Visible = false;
            applyEditStripButton.Click += applyEditStripButton_Click;
            // 
            // clearListMenuItem
            // 
            clearListMenuItem.Name = "clearListMenuItem";
            clearListMenuItem.Size = new Size(180, 22);
            clearListMenuItem.Text = "Clear List";
            // 
            // PlayPause
            // 
            PlayPause.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            PlayPause.BackColor = SystemColors.Window;
            PlayPause.BackgroundImage = Properties.Resources.Play_Pause;
            PlayPause.BackgroundImageLayout = ImageLayout.Zoom;
            PlayPause.FlatAppearance.BorderColor = SystemColors.Highlight;
            PlayPause.FlatAppearance.BorderSize = 0;
            PlayPause.FlatStyle = FlatStyle.Flat;
            PlayPause.Location = new Point(46, 23);
            PlayPause.Margin = new Padding(2, 3, 2, 3);
            PlayPause.Name = "PlayPause";
            PlayPause.Size = new Size(40, 32);
            PlayPause.TabIndex = 1;
            PlayPause.UseVisualStyleBackColor = false;
            PlayPause.Click += PlayPause_Click;
            // 
            // skip
            // 
            skip.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            skip.BackColor = SystemColors.Window;
            skip.BackgroundImage = Properties.Resources.Skip;
            skip.BackgroundImageLayout = ImageLayout.Zoom;
            skip.FlatAppearance.BorderSize = 0;
            skip.FlatStyle = FlatStyle.Flat;
            skip.Location = new Point(137, 27);
            skip.Margin = new Padding(2, 3, 2, 3);
            skip.Name = "skip";
            skip.Size = new Size(32, 24);
            skip.TabIndex = 2;
            skip.UseVisualStyleBackColor = false;
            skip.Click += skip_Click;
            // 
            // previous
            // 
            previous.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            previous.BackColor = SystemColors.Window;
            previous.BackgroundImage = Properties.Resources.Previous;
            previous.BackgroundImageLayout = ImageLayout.Zoom;
            previous.FlatAppearance.BorderSize = 0;
            previous.FlatStyle = FlatStyle.Flat;
            previous.Location = new Point(10, 27);
            previous.Margin = new Padding(2, 3, 2, 3);
            previous.Name = "previous";
            previous.Size = new Size(32, 24);
            previous.TabIndex = 3;
            previous.UseVisualStyleBackColor = false;
            previous.Click += previous_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.ErrorImage = Properties.Resources.Khi_Player;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Margin = new Padding(0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(470, 408);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            pictureBox1.DoubleClick += pictureBox1_DoubleClick;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button1.BackColor = SystemColors.Window;
            button1.Location = new Point(274, 26);
            button1.Margin = new Padding(2, 3, 2, 3);
            button1.Name = "button1";
            button1.Size = new Size(47, 24);
            button1.TabIndex = 7;
            button1.Text = "GC";
            button1.UseVisualStyleBackColor = false;
            button1.Visible = false;
            button1.Click += button1_Click;
            // 
            // musicListView
            // 
            musicListView.AllowColumnReorder = true;
            musicListView.AllowDrop = true;
            musicListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            musicListView.BackColor = SystemColors.Window;
            musicListView.BackgroundImageTiled = true;
            musicListView.BorderStyle = BorderStyle.None;
            musicListView.CausesValidation = false;
            musicListView.Columns.AddRange(new ColumnHeader[] { musicTitle, musicArtist, musicAlbum });
            musicListView.ContextMenuStrip = rightClickMenu;
            musicListView.ForeColor = SystemColors.WindowText;
            musicListView.GridLines = true;
            musicListView.HideSelection = true;
            musicListView.Location = new Point(90, 36);
            musicListView.Margin = new Padding(0);
            musicListView.MaximumSize = new Size(380, 508);
            musicListView.Name = "musicListView";
            musicListView.ShowGroups = false;
            musicListView.ShowItemToolTips = true;
            musicListView.Size = new Size(380, 508);
            musicListView.TabIndex = 8;
            musicListView.TileSize = new Size(360, 65);
            musicListView.UseCompatibleStateImageBehavior = false;
            musicListView.View = View.Tile;
            musicListView.DrawItem += musicListView_DrawItem;
            musicListView.DrawSubItem += musicListView_DrawSubItem;
            musicListView.ItemActivate += PlayPause_Click;
            musicListView.ItemSelectionChanged += musicListView_ItemSelectionChanged;
            musicListView.DragDrop += musicListView_DragDrop;
            musicListView.DragEnter += musicListView_DragEnter;
            musicListView.DragOver += musicListView_DragOver;
            musicListView.Invalidated += musicListView_Invalidated;
            musicListView.KeyPress += musicListView_KeyPress;
            musicListView.MouseClick += musicListView_MouseClick;
            musicListView.MouseDown += musicListView_MouseDown;
            musicListView.MouseMove += musicListView_MouseMove;
            // 
            // musicTitle
            // 
            musicTitle.Text = "Title";
            musicTitle.Width = 408;
            // 
            // musicArtist
            // 
            musicArtist.Text = "Artist";
            musicArtist.Width = 408;
            // 
            // musicAlbum
            // 
            musicAlbum.Text = "Album";
            musicAlbum.Width = 408;
            // 
            // rightClickMenu
            // 
            rightClickMenu.BackColor = Color.FromArgb(41, 41, 41);
            rightClickMenu.ImageScalingSize = new Size(20, 20);
            rightClickMenu.Items.AddRange(new ToolStripItem[] { playMenuItem, showItemInfoButton, editItemTagsButton, removeItemButton, addToPlaylistButton });
            rightClickMenu.Name = "contextMenuStrip1";
            rightClickMenu.RenderMode = ToolStripRenderMode.System;
            rightClickMenu.Size = new Size(152, 114);
            rightClickMenu.Closed += rightClickMenu_Closed;
            rightClickMenu.Opening += rightClickMenu_Opening;
            // 
            // playMenuItem
            // 
            playMenuItem.ForeColor = Color.White;
            playMenuItem.Name = "playMenuItem";
            playMenuItem.Size = new Size(151, 22);
            playMenuItem.Text = "Play";
            // 
            // showItemInfoButton
            // 
            showItemInfoButton.Enabled = false;
            showItemInfoButton.Name = "showItemInfoButton";
            showItemInfoButton.Size = new Size(151, 22);
            showItemInfoButton.Text = "Show Info";
            showItemInfoButton.Click += showItemInfoButton_Click;
            // 
            // editItemTagsButton
            // 
            editItemTagsButton.Enabled = false;
            editItemTagsButton.Name = "editItemTagsButton";
            editItemTagsButton.Size = new Size(151, 22);
            editItemTagsButton.Text = "Edit Tags";
            // 
            // removeItemButton
            // 
            removeItemButton.Enabled = false;
            removeItemButton.Name = "removeItemButton";
            removeItemButton.Size = new Size(151, 22);
            removeItemButton.Text = "Remove";
            removeItemButton.Click += removeItemButton_Click;
            // 
            // addToPlaylistButton
            // 
            addToPlaylistButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            addToPlaylistButton.Enabled = false;
            addToPlaylistButton.Name = "addToPlaylistButton";
            addToPlaylistButton.Size = new Size(151, 22);
            addToPlaylistButton.Text = "Add To Playlist";
            addToPlaylistButton.Visible = false;
            // 
            // artListImageList
            // 
            artListImageList.ColorDepth = ColorDepth.Depth32Bit;
            artListImageList.ImageSize = new Size(200, 160);
            artListImageList.TransparentColor = Color.Transparent;
            // 
            // playlistToolbar
            // 
            playlistToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            playlistToolbar.AutoSize = false;
            playlistToolbar.BackColor = SystemColors.Window;
            playlistToolbar.Dock = DockStyle.None;
            playlistToolbar.GripStyle = ToolStripGripStyle.Hidden;
            playlistToolbar.ImageScalingSize = new Size(20, 20);
            playlistToolbar.Items.AddRange(new ToolStripItem[] { playlistIdentifierLabel, currentPlaylistLabel, playlistLabel, allSongsPlaylist, renameTextBox, addPlaylistButton, editPlaylistButton });
            playlistToolbar.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            playlistToolbar.Location = new Point(0, 20);
            playlistToolbar.Name = "playlistToolbar";
            playlistToolbar.Padding = new Padding(0, 50, 0, 0);
            playlistToolbar.RenderMode = ToolStripRenderMode.System;
            playlistToolbar.ShowItemToolTips = false;
            playlistToolbar.Size = new Size(91, 527);
            playlistToolbar.TabIndex = 10;
            playlistToolbar.Text = "Playlists";
            // 
            // playlistIdentifierLabel
            // 
            playlistIdentifierLabel.AutoSize = false;
            playlistIdentifierLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            playlistIdentifierLabel.Name = "playlistIdentifierLabel";
            playlistIdentifierLabel.Size = new Size(90, 25);
            playlistIdentifierLabel.Text = "Current Playlist:";
            // 
            // currentPlaylistLabel
            // 
            currentPlaylistLabel.AutoSize = false;
            currentPlaylistLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            currentPlaylistLabel.Name = "currentPlaylistLabel";
            currentPlaylistLabel.Size = new Size(90, 25);
            currentPlaylistLabel.Text = "All Songs";
            // 
            // playlistLabel
            // 
            playlistLabel.AutoSize = false;
            playlistLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            playlistLabel.Image = (Image)resources.GetObject("playlistLabel.Image");
            playlistLabel.ImageTransparentColor = Color.Magenta;
            playlistLabel.Margin = new Padding(0, 20, 0, 20);
            playlistLabel.Name = "playlistLabel";
            playlistLabel.Size = new Size(90, 25);
            playlistLabel.Text = "Playlists:";
            // 
            // allSongsPlaylist
            // 
            allSongsPlaylist.AutoToolTip = false;
            allSongsPlaylist.DisplayStyle = ToolStripItemDisplayStyle.Text;
            allSongsPlaylist.ImageTransparentColor = Color.Magenta;
            allSongsPlaylist.Name = "allSongsPlaylist";
            allSongsPlaylist.Size = new Size(90, 19);
            allSongsPlaylist.Text = "All Songs";
            allSongsPlaylist.Click += allSongsPlaylist_Click;
            // 
            // renameTextBox
            // 
            renameTextBox.Enabled = false;
            renameTextBox.ForeColor = Color.DimGray;
            renameTextBox.Name = "renameTextBox";
            renameTextBox.Size = new Size(88, 23);
            renameTextBox.Text = "Playlist Name";
            renameTextBox.TextBoxTextAlign = HorizontalAlignment.Center;
            renameTextBox.Visible = false;
            renameTextBox.KeyPress += renameTextBox_KeyPress;
            // 
            // addPlaylistButton
            // 
            addPlaylistButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            addPlaylistButton.Image = (Image)resources.GetObject("addPlaylistButton.Image");
            addPlaylistButton.ImageTransparentColor = Color.Magenta;
            addPlaylistButton.Margin = new Padding(0, 20, 0, 2);
            addPlaylistButton.Name = "addPlaylistButton";
            addPlaylistButton.Size = new Size(90, 19);
            addPlaylistButton.Text = "Add Playlist";
            addPlaylistButton.ToolTipText = "Add a Personal Playlist";
            addPlaylistButton.Click += addPlaylistButton_Click;
            // 
            // editPlaylistButton
            // 
            editPlaylistButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            editPlaylistButton.Enabled = false;
            editPlaylistButton.Image = (Image)resources.GetObject("editPlaylistButton.Image");
            editPlaylistButton.ImageTransparentColor = Color.Magenta;
            editPlaylistButton.Name = "editPlaylistButton";
            editPlaylistButton.Size = new Size(90, 19);
            editPlaylistButton.Text = "Edit Playlist";
            editPlaylistButton.Visible = false;
            editPlaylistButton.Click += editPlaylistButton_Click;
            // 
            // mediaPlayerPanel
            // 
            mediaPlayerPanel.AllowDrop = true;
            mediaPlayerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mediaPlayerPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            mediaPlayerPanel.BackgroundImageLayout = ImageLayout.Stretch;
            mediaPlayerPanel.Controls.Add(borderLabel);
            mediaPlayerPanel.Controls.Add(songAlbumLabel);
            mediaPlayerPanel.Controls.Add(songArtistLabel);
            mediaPlayerPanel.Controls.Add(songTitleLabel);
            mediaPlayerPanel.Controls.Add(lyricsTextBox);
            mediaPlayerPanel.Controls.Add(pictureBox1);
            mediaPlayerPanel.Location = new Point(473, 20);
            mediaPlayerPanel.Margin = new Padding(0);
            mediaPlayerPanel.Name = "mediaPlayerPanel";
            mediaPlayerPanel.Size = new Size(473, 524);
            mediaPlayerPanel.TabIndex = 11;
            // 
            // borderLabel
            // 
            borderLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            borderLabel.BackColor = SystemColors.Window;
            borderLabel.FlatStyle = FlatStyle.Flat;
            borderLabel.Font = new Font("Segoe UI", 6F, FontStyle.Bold);
            borderLabel.Location = new Point(92, 413);
            borderLabel.Margin = new Padding(0);
            borderLabel.Name = "borderLabel";
            borderLabel.Size = new Size(273, 8);
            borderLabel.TabIndex = 17;
            borderLabel.Text = "********************";
            borderLabel.TextAlign = ContentAlignment.TopCenter;
            borderLabel.Visible = false;
            // 
            // songAlbumLabel
            // 
            songAlbumLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            songAlbumLabel.FlatStyle = FlatStyle.Flat;
            songAlbumLabel.Font = new Font("Segoe UI", 14F);
            songAlbumLabel.Location = new Point(1, 489);
            songAlbumLabel.Margin = new Padding(1);
            songAlbumLabel.Name = "songAlbumLabel";
            songAlbumLabel.Padding = new Padding(1, 0, 8, 0);
            songAlbumLabel.Size = new Size(472, 35);
            songAlbumLabel.TabIndex = 16;
            songAlbumLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // songArtistLabel
            // 
            songArtistLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            songArtistLabel.FlatStyle = FlatStyle.Flat;
            songArtistLabel.Font = new Font("Segoe UI", 14F);
            songArtistLabel.Location = new Point(1, 455);
            songArtistLabel.Margin = new Padding(1);
            songArtistLabel.Name = "songArtistLabel";
            songArtistLabel.Padding = new Padding(1, 0, 8, 0);
            songArtistLabel.Size = new Size(472, 32);
            songArtistLabel.TabIndex = 15;
            songArtistLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // songTitleLabel
            // 
            songTitleLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            songTitleLabel.FlatStyle = FlatStyle.Flat;
            songTitleLabel.Font = new Font("Segoe UI", 14F);
            songTitleLabel.Location = new Point(1, 422);
            songTitleLabel.Margin = new Padding(1);
            songTitleLabel.Name = "songTitleLabel";
            songTitleLabel.Padding = new Padding(1, 0, 8, 0);
            songTitleLabel.Size = new Size(472, 32);
            songTitleLabel.TabIndex = 14;
            songTitleLabel.TextAlign = ContentAlignment.MiddleCenter;
            songTitleLabel.Click += songTitleLabel_Click;
            // 
            // lyricsTextBox
            // 
            lyricsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lyricsTextBox.BackColor = SystemColors.Window;
            lyricsTextBox.BorderStyle = BorderStyle.None;
            lyricsTextBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lyricsTextBox.ForeColor = SystemColors.WindowText;
            lyricsTextBox.Location = new Point(1, 0);
            lyricsTextBox.Margin = new Padding(0);
            lyricsTextBox.MinimumSize = new Size(285, 31);
            lyricsTextBox.Multiline = true;
            lyricsTextBox.Name = "lyricsTextBox";
            lyricsTextBox.ReadOnly = true;
            lyricsTextBox.ScrollBars = ScrollBars.Vertical;
            lyricsTextBox.Size = new Size(470, 386);
            lyricsTextBox.TabIndex = 8;
            lyricsTextBox.TextAlign = HorizontalAlignment.Center;
            lyricsTextBox.Visible = false;
            lyricsTextBox.DoubleClick += lyricsTextBox_DoubleClick;
            // 
            // musicControlBar
            // 
            musicControlBar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            musicControlBar.BackColor = SystemColors.Window;
            musicControlBar.Controls.Add(stopButton);
            musicControlBar.Controls.Add(toggleShuffle);
            musicControlBar.Controls.Add(volumeLabel);
            musicControlBar.Controls.Add(button1);
            musicControlBar.Controls.Add(songLengthLabel);
            musicControlBar.Controls.Add(currentTimeLabel);
            musicControlBar.Controls.Add(volumeBar);
            musicControlBar.Controls.Add(toggleLoop);
            musicControlBar.Controls.Add(seekBar);
            musicControlBar.Controls.Add(PlayPause);
            musicControlBar.Controls.Add(previous);
            musicControlBar.Controls.Add(skip);
            musicControlBar.Dock = DockStyle.Bottom;
            musicControlBar.Location = new Point(0, 544);
            musicControlBar.Margin = new Padding(0);
            musicControlBar.MinimumSize = new Size(0, 48);
            musicControlBar.Name = "musicControlBar";
            musicControlBar.Size = new Size(946, 58);
            musicControlBar.TabIndex = 12;
            // 
            // stopButton
            // 
            stopButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            stopButton.BackColor = SystemColors.Window;
            stopButton.BackgroundImage = Properties.Resources.Stop_Light_Mode;
            stopButton.BackgroundImageLayout = ImageLayout.Zoom;
            stopButton.FlatAppearance.BorderSize = 0;
            stopButton.FlatStyle = FlatStyle.Flat;
            stopButton.Location = new Point(91, 26);
            stopButton.Margin = new Padding(2, 3, 2, 3);
            stopButton.Name = "stopButton";
            stopButton.Size = new Size(32, 24);
            stopButton.TabIndex = 15;
            stopButton.UseVisualStyleBackColor = false;
            stopButton.Click += stopButton_Click;
            // 
            // toggleShuffle
            // 
            toggleShuffle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            toggleShuffle.BackColor = SystemColors.Window;
            toggleShuffle.BackgroundImage = Properties.Resources.Shuffle_Light_Mode;
            toggleShuffle.BackgroundImageLayout = ImageLayout.Zoom;
            toggleShuffle.FlatAppearance.BorderColor = SystemColors.Highlight;
            toggleShuffle.FlatAppearance.BorderSize = 0;
            toggleShuffle.FlatStyle = FlatStyle.Flat;
            toggleShuffle.Location = new Point(688, 21);
            toggleShuffle.Margin = new Padding(2, 3, 2, 3);
            toggleShuffle.Name = "toggleShuffle";
            toggleShuffle.Size = new Size(35, 30);
            toggleShuffle.TabIndex = 14;
            toggleShuffle.UseVisualStyleBackColor = false;
            toggleShuffle.Click += toggleShuffle_Click;
            // 
            // volumeLabel
            // 
            volumeLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            volumeLabel.Location = new Point(792, 28);
            volumeLabel.Margin = new Padding(2, 0, 2, 0);
            volumeLabel.Name = "volumeLabel";
            volumeLabel.Size = new Size(35, 19);
            volumeLabel.TabIndex = 13;
            volumeLabel.Text = "100";
            volumeLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // songLengthLabel
            // 
            songLengthLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            songLengthLabel.Location = new Point(884, 3);
            songLengthLabel.Margin = new Padding(2, 0, 2, 0);
            songLengthLabel.Name = "songLengthLabel";
            songLengthLabel.Size = new Size(39, 16);
            songLengthLabel.TabIndex = 10;
            songLengthLabel.Text = "00:00";
            songLengthLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // currentTimeLabel
            // 
            currentTimeLabel.Location = new Point(2, 3);
            currentTimeLabel.Margin = new Padding(2, 0, 2, 0);
            currentTimeLabel.Name = "currentTimeLabel";
            currentTimeLabel.Size = new Size(46, 16);
            currentTimeLabel.TabIndex = 9;
            currentTimeLabel.Text = "00:00";
            currentTimeLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // volumeBar
            // 
            volumeBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            volumeBar.AutoSize = false;
            volumeBar.BackColor = SystemColors.Window;
            volumeBar.Location = new Point(832, 27);
            volumeBar.Margin = new Padding(2);
            volumeBar.Maximum = 100;
            volumeBar.Name = "volumeBar";
            volumeBar.Size = new Size(104, 22);
            volumeBar.TabIndex = 11;
            volumeBar.TickStyle = TickStyle.None;
            volumeBar.Value = 100;
            volumeBar.Scroll += volumeBar_Scroll;
            // 
            // toggleLoop
            // 
            toggleLoop.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            toggleLoop.BackColor = SystemColors.Window;
            toggleLoop.BackgroundImage = Properties.Resources.loop;
            toggleLoop.BackgroundImageLayout = ImageLayout.Zoom;
            toggleLoop.FlatAppearance.BorderColor = SystemColors.Highlight;
            toggleLoop.FlatAppearance.BorderSize = 0;
            toggleLoop.FlatStyle = FlatStyle.Flat;
            toggleLoop.Location = new Point(737, 22);
            toggleLoop.Margin = new Padding(2, 3, 2, 3);
            toggleLoop.Name = "toggleLoop";
            toggleLoop.Size = new Size(35, 30);
            toggleLoop.TabIndex = 9;
            toggleLoop.UseVisualStyleBackColor = false;
            toggleLoop.Click += toggleLoop_Click;
            // 
            // seekBar
            // 
            seekBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            seekBar.AutoSize = false;
            seekBar.CausesValidation = false;
            seekBar.Enabled = false;
            seekBar.LargeChange = 0;
            seekBar.Location = new Point(46, 2);
            seekBar.Margin = new Padding(8, 2, 8, 2);
            seekBar.Maximum = 0;
            seekBar.Name = "seekBar";
            seekBar.Size = new Size(840, 19);
            seekBar.SmallChange = 0;
            seekBar.TabIndex = 12;
            seekBar.TickStyle = TickStyle.None;
            seekBar.Scroll += seekBar_Scroll;
            seekBar.MouseDown += seekBar_MouseDown;
            seekBar.MouseUp += seekBar_MouseUp;
            // 
            // songToolTip
            // 
            songToolTip.BackColor = Color.FromArgb(41, 41, 41);
            songToolTip.ForeColor = Color.White;
            songToolTip.Popup += songToolTip_Popup;
            // 
            // songSeekTimer
            // 
            songSeekTimer.Interval = 990;
            songSeekTimer.Tick += songSeekTimer_Tick;
            // 
            // MemoryManageTimer
            // 
            MemoryManageTimer.Enabled = true;
            MemoryManageTimer.Interval = 60000;
            MemoryManageTimer.Tick += MemoryManageTimer_Tick;
            // 
            // searchMusicListView
            // 
            searchMusicListView.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            searchMusicListView.BorderStyle = BorderStyle.None;
            searchMusicListView.Location = new Point(91, 20);
            searchMusicListView.Margin = new Padding(0);
            searchMusicListView.Name = "searchMusicListView";
            searchMusicListView.PlaceholderText = "Search Using Title, Artist, or Album";
            searchMusicListView.Size = new Size(382, 16);
            searchMusicListView.TabIndex = 13;
            searchMusicListView.TextChanged += searchMusicListView_TextChanged;
            searchMusicListView.KeyDown += searchMusicListView_KeyDown;
            searchMusicListView.KeyPress += searchMusicListView_KeyPress;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = SystemColors.Window;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(946, 602);
            Controls.Add(searchMusicListView);
            Controls.Add(musicControlBar);
            Controls.Add(mediaPlayerPanel);
            Controls.Add(playlistToolbar);
            Controls.Add(musicListView);
            Controls.Add(userBar);
            DoubleBuffered = true;
            ForeColor = SystemColors.WindowText;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2, 3, 2, 3);
            MinimumSize = new Size(962, 641);
            Name = "Form1";
            Text = "Khi Player";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            Resize += Form1_Resize;
            userBar.ResumeLayout(false);
            userBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            rightClickMenu.ResumeLayout(false);
            playlistToolbar.ResumeLayout(false);
            playlistToolbar.PerformLayout();
            mediaPlayerPanel.ResumeLayout(false);
            mediaPlayerPanel.PerformLayout();
            musicControlBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)volumeBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)seekBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        public ToolStripDropDownButton fileToolStripMenuItem;
        public ToolStripMenuItem addMusicsToolStripMenuItem;
        public Button PlayPause;
        public Button skip;
        public Button previous;
        public PictureBox pictureBox1;
        public Button Rescan;
        public Button clearListButton;
        private Button button1;
        public ListView musicListView;
        private ColumnHeader musicTitle;
        private ColumnHeader musicArtist;
        private ColumnHeader musicAlbum;
        private TabPage tabPage1;
        private ToolStripButton addPlaylistButton;
        private ToolStripLabel playlistLabel;
        private Panel mediaPlayerPanel;
        private ToolStripDropDownButton listToolStripMenuItem;
        //private ToolStripMenuItem Rescan;
        private ToolStripMenuItem clearListMenuItem;
        private ToolStripMenuItem RescanMenuItem;
        private ToolStripMenuItem ClearListItem;
        private Button toggleLoop;
        private ContextMenuStrip rightClickMenu;
        private ToolStripMenuItem removeItemButton;
        private ToolStripDropDownButton viewToolStripMenuItem;
        private ToolStripMenuItem showLyricsMenuItem;
        private ToolStripMenuItem showAlbumArtMenuItem;
        private Panel musicControlBar;
        private ProgressBar progressBar1;
        private TrackBar volumeBar;
        private ToolStripMenuItem editItemTagsButton;
        private ToolStripMenuItem showItemInfoButton;
        private ToolStripMenuItem showLargeIconMenuItem;
        private ToolStripMenuItem showDetailsMenuItem;
        public ImageList artListImageList;
        public ToolTip songToolTip;
        public TrackBar seekBar;
        public System.Windows.Forms.Timer songSeekTimer;
        private Label songLengthLabel;
        private Label currentTimeLabel;
        private Label volumeLabel;
        public System.Windows.Forms.Timer MemoryManageTimer;
        private Label songTitleLabel;
        private Label songAlbumLabel;
        private Label songArtistLabel;
        private Label borderLabel;
        private ToolStripMenuItem addFolderToolStripMenuItem;
        private ToolStripMenuItem darkModeMenuItem;
        public TextBox lyricsTextBox;
        private ToolStripMenuItem showTileViewMenuItem;
        public ToolStrip playlistToolbar;
        public ToolStripButton allSongsPlaylist;
        public ToolStripTextBox renameTextBox;
        public ToolStripButton editPlaylistButton;
        public ToolStrip userBar;
        public TextBox searchMusicListView;
        private ToolStripMenuItem SortListMenuItem;
        private ToolStripMenuItem sortListTitleMenuItem;
        private ToolStripMenuItem sortListArtistMenuItem;
        private ToolStripMenuItem sortListAlbumMenuItem;
        private Button toggleShuffle;
        public Button stopButton;
        public ToolStripMenuItem playMenuItem;
        public ToolStripMenuItem addToPlaylistButton;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripLabel playlistIdentifierLabel;
        private ToolStripLabel currentPlaylistLabel;
        public ToolStripButton editLyricsToolStripButton;
        public ToolStripButton applyEditStripButton;
    }
}
