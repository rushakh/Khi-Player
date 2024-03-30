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
            viewToolStripMenuItem = new ToolStripDropDownButton();
            showLyricsMenuItem = new ToolStripMenuItem();
            showAlbumArtMenuItem = new ToolStripMenuItem();
            showLargeIconMenuItem = new ToolStripMenuItem();
            showDetailsMenuItem = new ToolStripMenuItem();
            listToolStripMenuItem = new ToolStripDropDownButton();
            RescanMenuItem = new ToolStripMenuItem();
            ClearListItem = new ToolStripMenuItem();
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
            showItemInfoButton = new ToolStripMenuItem();
            editItemTagsButton = new ToolStripMenuItem();
            removeItemButton = new ToolStripMenuItem();
            artListImageList = new ImageList(components);
            playlistToolbar = new ToolStrip();
            playlistLabel = new ToolStripLabel();
            toolStripButton1 = new ToolStripButton();
            addPlaylistButton = new ToolStripButton();
            renameTextBox = new ToolStripTextBox();
            playlist1Button = new ToolStripButton();
            playlist2Button = new ToolStripButton();
            playlist3Button = new ToolStripButton();
            playlist4Button = new ToolStripButton();
            playlist5Button = new ToolStripButton();
            editPlaylistButton = new ToolStripButton();
            mediaPlayerPanel = new Panel();
            textBox1 = new TextBox();
            lyricsTextBox = new TextBox();
            panel1 = new Panel();
            volumeBar = new TrackBar();
            toggleLoop = new Button();
            seekBar = new TrackBar();
            songToolTip = new ToolTip(components);
            userBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            rightClickMenu.SuspendLayout();
            playlistToolbar.SuspendLayout();
            mediaPlayerPanel.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)volumeBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)seekBar).BeginInit();
            SuspendLayout();
            // 
            // userBar
            // 
            userBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            userBar.Dock = DockStyle.None;
            userBar.ImageScalingSize = new Size(20, 20);
            userBar.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, viewToolStripMenuItem, listToolStripMenuItem });
            userBar.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            userBar.Location = new Point(0, 0);
            userBar.Name = "userBar";
            userBar.Size = new Size(159, 27);
            userBar.TabIndex = 0;
            userBar.Text = "UserBar";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addMusicsToolStripMenuItem });
            fileToolStripMenuItem.Image = (Image)resources.GetObject("fileToolStripMenuItem.Image");
            fileToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // addMusicsToolStripMenuItem
            // 
            addMusicsToolStripMenuItem.Name = "addMusicsToolStripMenuItem";
            addMusicsToolStripMenuItem.Size = new Size(162, 26);
            addMusicsToolStripMenuItem.Text = "Add Music";
            addMusicsToolStripMenuItem.Click += addMusicsToolStripMenuItem_Click;
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { showLyricsMenuItem, showAlbumArtMenuItem, showLargeIconMenuItem, showDetailsMenuItem });
            viewToolStripMenuItem.Image = (Image)resources.GetObject("viewToolStripMenuItem.Image");
            viewToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(55, 24);
            viewToolStripMenuItem.Text = "View";
            // 
            // showLyricsMenuItem
            // 
            showLyricsMenuItem.Name = "showLyricsMenuItem";
            showLyricsMenuItem.Size = new Size(200, 26);
            showLyricsMenuItem.Text = "Show Lyrics";
            showLyricsMenuItem.Click += showLyricsMenuItem_Click;
            // 
            // showAlbumArtMenuItem
            // 
            showAlbumArtMenuItem.Checked = true;
            showAlbumArtMenuItem.CheckState = CheckState.Checked;
            showAlbumArtMenuItem.Name = "showAlbumArtMenuItem";
            showAlbumArtMenuItem.Size = new Size(200, 26);
            showAlbumArtMenuItem.Text = "Show Album Art";
            showAlbumArtMenuItem.Click += showAlbumArtMenuItem_Click;
            // 
            // showLargeIconMenuItem
            // 
            showLargeIconMenuItem.Name = "showLargeIconMenuItem";
            showLargeIconMenuItem.Size = new Size(200, 26);
            showLargeIconMenuItem.Text = "Large Icon View";
            showLargeIconMenuItem.Click += showLargeIconMenuItem_Click;
            // 
            // showDetailsMenuItem
            // 
            showDetailsMenuItem.Checked = true;
            showDetailsMenuItem.CheckState = CheckState.Checked;
            showDetailsMenuItem.Name = "showDetailsMenuItem";
            showDetailsMenuItem.Size = new Size(200, 26);
            showDetailsMenuItem.Text = "Details View";
            showDetailsMenuItem.Click += showDetailsMenuItem_Click;
            // 
            // listToolStripMenuItem
            // 
            listToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            listToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { RescanMenuItem, ClearListItem });
            listToolStripMenuItem.Image = (Image)resources.GetObject("listToolStripMenuItem.Image");
            listToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            listToolStripMenuItem.Name = "listToolStripMenuItem";
            listToolStripMenuItem.Size = new Size(45, 24);
            listToolStripMenuItem.Text = "List";
            // 
            // RescanMenuItem
            // 
            RescanMenuItem.Name = "RescanMenuItem";
            RescanMenuItem.Size = new Size(152, 26);
            RescanMenuItem.Text = "Rescan";
            RescanMenuItem.ToolTipText = "Rescans All Songs";
            RescanMenuItem.Click += RescanMenuItem_Click;
            // 
            // ClearListItem
            // 
            ClearListItem.Name = "ClearListItem";
            ClearListItem.Size = new Size(152, 26);
            ClearListItem.Text = "Clear List";
            ClearListItem.ToolTipText = "Clears All Songs List";
            ClearListItem.Click += ClearListItem_Click;
            // 
            // clearListMenuItem
            // 
            clearListMenuItem.Name = "clearListMenuItem";
            clearListMenuItem.Size = new Size(180, 22);
            clearListMenuItem.Text = "Clear List";
            // 
            // PlayPause
            // 
            PlayPause.Location = new Point(12, 25);
            PlayPause.Margin = new Padding(3, 4, 3, 4);
            PlayPause.Name = "PlayPause";
            PlayPause.Size = new Size(102, 31);
            PlayPause.TabIndex = 1;
            PlayPause.Text = "Play/Pause";
            PlayPause.UseVisualStyleBackColor = true;
            PlayPause.Click += PlayPause_Click;
            // 
            // skip
            // 
            skip.Location = new Point(422, 25);
            skip.Margin = new Padding(3, 4, 3, 4);
            skip.Name = "skip";
            skip.Size = new Size(53, 31);
            skip.TabIndex = 2;
            skip.Text = "Skip";
            skip.UseVisualStyleBackColor = true;
            skip.Click += skip_Click;
            // 
            // previous
            // 
            previous.Location = new Point(241, 25);
            previous.Margin = new Padding(3, 4, 3, 4);
            previous.Name = "previous";
            previous.Size = new Size(50, 31);
            previous.TabIndex = 3;
            previous.Text = "Previous";
            previous.UseVisualStyleBackColor = true;
            previous.Click += previous_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Location = new Point(22, 46);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(360, 314);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            pictureBox1.DoubleClick += pictureBox1_DoubleClick;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button1.Location = new Point(202, 405);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(130, 31);
            button1.TabIndex = 7;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // musicListView
            // 
            musicListView.Activation = ItemActivation.TwoClick;
            musicListView.AllowColumnReorder = true;
            musicListView.AllowDrop = true;
            musicListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            musicListView.BackColor = SystemColors.Control;
            musicListView.BackgroundImageTiled = true;
            musicListView.Columns.AddRange(new ColumnHeader[] { musicTitle, musicArtist, musicAlbum });
            musicListView.ContextMenuStrip = rightClickMenu;
            musicListView.FullRowSelect = true;
            musicListView.GridLines = true;
            musicListView.Location = new Point(118, 33);
            musicListView.Margin = new Padding(3, 4, 3, 4);
            musicListView.MaximumSize = new Size(410, 5000);
            musicListView.Name = "musicListView";
            musicListView.OwnerDraw = true;
            musicListView.ShowGroups = false;
            musicListView.ShowItemToolTips = true;
            musicListView.Size = new Size(410, 492);
            musicListView.TabIndex = 8;
            musicListView.TileSize = new Size(380, 65);
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
            rightClickMenu.ImageScalingSize = new Size(20, 20);
            rightClickMenu.Items.AddRange(new ToolStripItem[] { showItemInfoButton, editItemTagsButton, removeItemButton });
            rightClickMenu.Name = "contextMenuStrip1";
            rightClickMenu.Size = new Size(145, 76);
            rightClickMenu.Closed += rightClickMenu_Closed;
            rightClickMenu.Opening += rightClickMenu_Opening;
            // 
            // showItemInfoButton
            // 
            showItemInfoButton.Enabled = false;
            showItemInfoButton.Name = "showItemInfoButton";
            showItemInfoButton.Size = new Size(144, 24);
            showItemInfoButton.Text = "Show Info";
            showItemInfoButton.Click += showItemInfoButton_Click;
            // 
            // editItemTagsButton
            // 
            editItemTagsButton.Enabled = false;
            editItemTagsButton.Name = "editItemTagsButton";
            editItemTagsButton.Size = new Size(144, 24);
            editItemTagsButton.Text = "Edit Tags";
            // 
            // removeItemButton
            // 
            removeItemButton.Enabled = false;
            removeItemButton.Name = "removeItemButton";
            removeItemButton.Size = new Size(144, 24);
            removeItemButton.Text = "Remove";
            removeItemButton.Click += removeItemButton_Click;
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
            playlistToolbar.Dock = DockStyle.None;
            playlistToolbar.ImageScalingSize = new Size(20, 20);
            playlistToolbar.Items.AddRange(new ToolStripItem[] { playlistLabel, toolStripButton1, addPlaylistButton, renameTextBox, playlist1Button, playlist2Button, playlist3Button, playlist4Button, playlist5Button, editPlaylistButton });
            playlistToolbar.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            playlistToolbar.Location = new Point(0, 33);
            playlistToolbar.Name = "playlistToolbar";
            playlistToolbar.Size = new Size(114, 492);
            playlistToolbar.TabIndex = 10;
            playlistToolbar.Text = "toolStrip1";
            // 
            // playlistLabel
            // 
            playlistLabel.AutoSize = false;
            playlistLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            playlistLabel.Image = (Image)resources.GetObject("playlistLabel.Image");
            playlistLabel.ImageTransparentColor = Color.Magenta;
            playlistLabel.Name = "playlistLabel";
            playlistLabel.Size = new Size(98, 25);
            playlistLabel.Text = "Playlists:";
            playlistLabel.TextAlign = ContentAlignment.TopLeft;
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(112, 24);
            toolStripButton1.Text = "All Songs";
            // 
            // addPlaylistButton
            // 
            addPlaylistButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            addPlaylistButton.Image = (Image)resources.GetObject("addPlaylistButton.Image");
            addPlaylistButton.ImageTransparentColor = Color.Magenta;
            addPlaylistButton.Name = "addPlaylistButton";
            addPlaylistButton.Size = new Size(112, 24);
            addPlaylistButton.Text = "Add Playlist";
            addPlaylistButton.ToolTipText = "Add a Personal Playlist";
            addPlaylistButton.Click += addPlaylistButton_Click;
            // 
            // renameTextBox
            // 
            renameTextBox.ForeColor = Color.DimGray;
            renameTextBox.Name = "renameTextBox";
            renameTextBox.Size = new Size(110, 27);
            renameTextBox.Text = "Playlist Name";
            renameTextBox.TextBoxTextAlign = HorizontalAlignment.Center;
            renameTextBox.KeyPress += renameTextBox_KeyPress;
            // 
            // playlist1Button
            // 
            playlist1Button.DisplayStyle = ToolStripItemDisplayStyle.Text;
            playlist1Button.Enabled = false;
            playlist1Button.Image = (Image)resources.GetObject("playlist1Button.Image");
            playlist1Button.ImageTransparentColor = Color.Magenta;
            playlist1Button.Name = "playlist1Button";
            playlist1Button.Size = new Size(112, 24);
            playlist1Button.Text = "Playlist 1";
            // 
            // playlist2Button
            // 
            playlist2Button.DisplayStyle = ToolStripItemDisplayStyle.Text;
            playlist2Button.Enabled = false;
            playlist2Button.Image = (Image)resources.GetObject("playlist2Button.Image");
            playlist2Button.ImageTransparentColor = Color.Magenta;
            playlist2Button.Name = "playlist2Button";
            playlist2Button.Size = new Size(112, 24);
            playlist2Button.Text = "Playlist 2";
            // 
            // playlist3Button
            // 
            playlist3Button.DisplayStyle = ToolStripItemDisplayStyle.Text;
            playlist3Button.Enabled = false;
            playlist3Button.Image = (Image)resources.GetObject("playlist3Button.Image");
            playlist3Button.ImageTransparentColor = Color.Magenta;
            playlist3Button.Name = "playlist3Button";
            playlist3Button.Size = new Size(112, 24);
            playlist3Button.Text = "Playlist 3";
            // 
            // playlist4Button
            // 
            playlist4Button.DisplayStyle = ToolStripItemDisplayStyle.Text;
            playlist4Button.Enabled = false;
            playlist4Button.Image = (Image)resources.GetObject("playlist4Button.Image");
            playlist4Button.ImageTransparentColor = Color.Magenta;
            playlist4Button.Name = "playlist4Button";
            playlist4Button.Size = new Size(112, 24);
            playlist4Button.Text = "Playlist 4";
            // 
            // playlist5Button
            // 
            playlist5Button.DisplayStyle = ToolStripItemDisplayStyle.Text;
            playlist5Button.Enabled = false;
            playlist5Button.Image = (Image)resources.GetObject("playlist5Button.Image");
            playlist5Button.ImageTransparentColor = Color.Magenta;
            playlist5Button.Name = "playlist5Button";
            playlist5Button.Size = new Size(112, 24);
            playlist5Button.Text = "Playlist 5";
            // 
            // editPlaylistButton
            // 
            editPlaylistButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            editPlaylistButton.Image = (Image)resources.GetObject("editPlaylistButton.Image");
            editPlaylistButton.ImageTransparentColor = Color.Magenta;
            editPlaylistButton.Name = "editPlaylistButton";
            editPlaylistButton.Size = new Size(112, 24);
            editPlaylistButton.Text = "Edit Playlist";
            // 
            // mediaPlayerPanel
            // 
            mediaPlayerPanel.AllowDrop = true;
            mediaPlayerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mediaPlayerPanel.AutoSize = true;
            mediaPlayerPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            mediaPlayerPanel.BackgroundImageLayout = ImageLayout.Stretch;
            mediaPlayerPanel.Controls.Add(textBox1);
            mediaPlayerPanel.Controls.Add(lyricsTextBox);
            mediaPlayerPanel.Controls.Add(button1);
            mediaPlayerPanel.Controls.Add(pictureBox1);
            mediaPlayerPanel.Location = new Point(550, 33);
            mediaPlayerPanel.Margin = new Padding(3, 4, 3, 4);
            mediaPlayerPanel.Name = "mediaPlayerPanel";
            mediaPlayerPanel.Size = new Size(420, 495);
            mediaPlayerPanel.TabIndex = 11;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(48, 443);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Both;
            textBox1.Size = new Size(318, 49);
            textBox1.TabIndex = 10;
            textBox1.Visible = false;
            // 
            // lyricsTextBox
            // 
            lyricsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lyricsTextBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lyricsTextBox.Location = new Point(1, 19);
            lyricsTextBox.Margin = new Padding(3, 4, 3, 4);
            lyricsTextBox.MinimumSize = new Size(356, 39);
            lyricsTextBox.Multiline = true;
            lyricsTextBox.Name = "lyricsTextBox";
            lyricsTextBox.ReadOnly = true;
            lyricsTextBox.ScrollBars = ScrollBars.Vertical;
            lyricsTextBox.Size = new Size(416, 310);
            lyricsTextBox.TabIndex = 8;
            lyricsTextBox.TextAlign = HorizontalAlignment.Center;
            lyricsTextBox.Visible = false;
            lyricsTextBox.DoubleClick += lyricsTextBox_DoubleClick;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.Controls.Add(volumeBar);
            panel1.Controls.Add(toggleLoop);
            panel1.Controls.Add(seekBar);
            panel1.Controls.Add(PlayPause);
            panel1.Controls.Add(previous);
            panel1.Controls.Add(skip);
            panel1.Location = new Point(0, 540);
            panel1.MinimumSize = new Size(0, 60);
            panel1.Name = "panel1";
            panel1.Size = new Size(982, 60);
            panel1.TabIndex = 12;
            // 
            // volumeBar
            // 
            volumeBar.AutoSize = false;
            volumeBar.BackColor = Color.LightPink;
            volumeBar.Location = new Point(781, 21);
            volumeBar.Name = "volumeBar";
            volumeBar.Size = new Size(130, 27);
            volumeBar.TabIndex = 11;
            // 
            // toggleLoop
            // 
            toggleLoop.Location = new Point(642, 25);
            toggleLoop.Margin = new Padding(3, 4, 3, 4);
            toggleLoop.Name = "toggleLoop";
            toggleLoop.Size = new Size(86, 31);
            toggleLoop.TabIndex = 9;
            toggleLoop.Text = "Loop";
            toggleLoop.UseVisualStyleBackColor = true;
            toggleLoop.Click += toggleLoop_Click;
            // 
            // seekBar
            // 
            seekBar.AutoSize = false;
            seekBar.Dock = DockStyle.Top;
            seekBar.Location = new Point(0, 0);
            seekBar.Name = "seekBar";
            seekBar.Size = new Size(982, 24);
            seekBar.TabIndex = 12;
            seekBar.TickStyle = TickStyle.None;
            // 
            // songToolTip
            // 
            songToolTip.Popup += songToolTip_Popup;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(982, 600);
            Controls.Add(panel1);
            Controls.Add(mediaPlayerPanel);
            Controls.Add(playlistToolbar);
            Controls.Add(musicListView);
            Controls.Add(userBar);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Khi Player";
            Load += Form1_Load;
            userBar.ResumeLayout(false);
            userBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            rightClickMenu.ResumeLayout(false);
            playlistToolbar.ResumeLayout(false);
            playlistToolbar.PerformLayout();
            mediaPlayerPanel.ResumeLayout(false);
            mediaPlayerPanel.PerformLayout();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)volumeBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)seekBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip userBar;
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
        private ToolStrip playlistToolbar;
        private ToolStripButton toolStripButton1;
        private ToolStripButton addPlaylistButton;
        private ToolStripTextBox renameTextBox;
        private ToolStripLabel playlistLabel;
        private ToolStripButton playlist1Button;
        private ToolStripButton playlist2Button;
        private ToolStripButton playlist3Button;
        private ToolStripButton playlist4Button;
        private ToolStripButton playlist5Button;
        private Panel mediaPlayerPanel;
        private TextBox lyricsTextBox;
        private ToolStripDropDownButton listToolStripMenuItem;
        //private ToolStripMenuItem Rescan;
        private ToolStripMenuItem clearListMenuItem;
        private ToolStripMenuItem RescanMenuItem;
        private ToolStripMenuItem ClearListItem;
        private Button toggleLoop;
        private ToolStripButton editPlaylistButton;
        private ContextMenuStrip rightClickMenu;
        private ToolStripMenuItem removeItemButton;
        private ToolStripDropDownButton viewToolStripMenuItem;
        private ToolStripMenuItem showLyricsMenuItem;
        private ToolStripMenuItem showAlbumArtMenuItem;
        private TextBox textBox1;
        private Panel panel1;
        private ProgressBar progressBar1;
        private TrackBar seekBar;
        private TrackBar volumeBar;
        private ToolStripMenuItem editItemTagsButton;
        private ToolStripMenuItem showItemInfoButton;
        private ToolStripMenuItem showLargeIconMenuItem;
        private ToolStripMenuItem showDetailsMenuItem;
        public ImageList artListImageList;
        public ToolTip songToolTip;
    }
}
