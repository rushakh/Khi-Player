namespace Media_Player
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
            listToolStripMenuItem = new ToolStripDropDownButton();
            RescanMenuItem = new ToolStripMenuItem();
            ClearListItem = new ToolStripMenuItem();
            clearListMenuItem = new ToolStripMenuItem();
            PlayStop = new Button();
            skip = new Button();
            previous = new Button();
            pictureBox1 = new PictureBox();
            button1 = new Button();
            musicListView = new ListView();
            musicTitle = new ColumnHeader();
            musicArtist = new ColumnHeader();
            musicAlbum = new ColumnHeader();
            musicDuration = new ColumnHeader();
            musicPath = new ColumnHeader();
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
            toggleLoop = new Button();
            lyricsTextBox = new TextBox();
            rightClickMenu = new ContextMenuStrip(components);
            rightClickRemoveButton = new ToolStripMenuItem();
            userBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            playlistToolbar.SuspendLayout();
            mediaPlayerPanel.SuspendLayout();
            rightClickMenu.SuspendLayout();
            SuspendLayout();
            // 
            // userBar
            // 
            userBar.ImageScalingSize = new Size(20, 20);
            userBar.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, listToolStripMenuItem });
            userBar.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            userBar.Location = new Point(0, 0);
            userBar.Name = "userBar";
            userBar.Size = new Size(914, 27);
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
            // PlayStop
            // 
            PlayStop.Location = new Point(137, 249);
            PlayStop.Margin = new Padding(3, 4, 3, 4);
            PlayStop.Name = "PlayStop";
            PlayStop.Size = new Size(86, 31);
            PlayStop.TabIndex = 1;
            PlayStop.Text = "Play/Stop";
            PlayStop.UseVisualStyleBackColor = true;
            // 
            // skip
            // 
            skip.Location = new Point(230, 249);
            skip.Margin = new Padding(3, 4, 3, 4);
            skip.Name = "skip";
            skip.Size = new Size(86, 31);
            skip.TabIndex = 2;
            skip.Text = "Skip";
            skip.UseVisualStyleBackColor = true;
            // 
            // previous
            // 
            previous.Location = new Point(45, 249);
            previous.Margin = new Padding(3, 4, 3, 4);
            previous.Name = "previous";
            previous.Size = new Size(86, 31);
            previous.TabIndex = 3;
            previous.Text = "Previous";
            previous.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(87, 83);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(194, 159);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.Location = new Point(219, 344);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(86, 31);
            button1.TabIndex = 7;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // musicListView
            // 
            musicListView.AllowColumnReorder = true;
            musicListView.AllowDrop = true;
            musicListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            musicListView.Columns.AddRange(new ColumnHeader[] { musicTitle, musicArtist, musicAlbum, musicDuration, musicPath });
            musicListView.FullRowSelect = true;
            musicListView.GridLines = true;
            musicListView.Location = new Point(118, 33);
            musicListView.Margin = new Padding(3, 4, 3, 4);
            musicListView.Name = "musicListView";
            musicListView.Size = new Size(407, 565);
            musicListView.TabIndex = 8;
            musicListView.TileSize = new Size(5, 20);
            musicListView.UseCompatibleStateImageBehavior = false;
            musicListView.View = View.Details;
            musicListView.SelectedIndexChanged += musicListView_SelectedIndexChanged;
            musicListView.DragDrop += musicListView_DragDrop;
            musicListView.DragEnter += musicListView_DragEnter;
            musicListView.DragOver += musicListView_DragOver;
            musicListView.MouseDown += musicListView_MouseDown;
            // 
            // musicTitle
            // 
            musicTitle.Text = "Title";
            musicTitle.Width = 80;
            // 
            // musicArtist
            // 
            musicArtist.Text = "Artist";
            musicArtist.Width = 80;
            // 
            // musicAlbum
            // 
            musicAlbum.Text = "Album";
            musicAlbum.Width = 80;
            // 
            // musicDuration
            // 
            musicDuration.Text = "Duration";
            musicDuration.Width = 40;
            // 
            // musicPath
            // 
            musicPath.Text = "Path";
            musicPath.Width = 70;
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
            playlistToolbar.Size = new Size(114, 567);
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
            mediaPlayerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            mediaPlayerPanel.Controls.Add(toggleLoop);
            mediaPlayerPanel.Controls.Add(lyricsTextBox);
            mediaPlayerPanel.Controls.Add(previous);
            mediaPlayerPanel.Controls.Add(PlayStop);
            mediaPlayerPanel.Controls.Add(button1);
            mediaPlayerPanel.Controls.Add(skip);
            mediaPlayerPanel.Controls.Add(pictureBox1);
            mediaPlayerPanel.Location = new Point(527, 33);
            mediaPlayerPanel.Margin = new Padding(3, 4, 3, 4);
            mediaPlayerPanel.Name = "mediaPlayerPanel";
            mediaPlayerPanel.Size = new Size(387, 567);
            mediaPlayerPanel.TabIndex = 11;
            // 
            // toggleLoop
            // 
            toggleLoop.Location = new Point(17, 315);
            toggleLoop.Margin = new Padding(3, 4, 3, 4);
            toggleLoop.Name = "toggleLoop";
            toggleLoop.Size = new Size(86, 31);
            toggleLoop.TabIndex = 9;
            toggleLoop.Text = "Loop";
            toggleLoop.UseVisualStyleBackColor = true;
            toggleLoop.Click += toggleLoop_Click;
            // 
            // lyricsTextBox
            // 
            lyricsTextBox.Location = new Point(17, 19);
            lyricsTextBox.Margin = new Padding(3, 4, 3, 4);
            lyricsTextBox.MaximumSize = new Size(356, 205);
            lyricsTextBox.MinimumSize = new Size(356, 39);
            lyricsTextBox.Multiline = true;
            lyricsTextBox.Name = "lyricsTextBox";
            lyricsTextBox.ReadOnly = true;
            lyricsTextBox.ScrollBars = ScrollBars.Vertical;
            lyricsTextBox.Size = new Size(356, 39);
            lyricsTextBox.TabIndex = 8;
            lyricsTextBox.TextAlign = HorizontalAlignment.Center;
            // 
            // rightClickMenu
            // 
            rightClickMenu.ImageScalingSize = new Size(20, 20);
            rightClickMenu.Items.AddRange(new ToolStripItem[] { rightClickRemoveButton });
            rightClickMenu.Name = "contextMenuStrip1";
            rightClickMenu.Size = new Size(133, 28);
            // 
            // rightClickRemoveButton
            // 
            rightClickRemoveButton.Name = "rightClickRemoveButton";
            rightClickRemoveButton.Size = new Size(132, 24);
            rightClickRemoveButton.Text = "Remove";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(mediaPlayerPanel);
            Controls.Add(playlistToolbar);
            Controls.Add(musicListView);
            Controls.Add(userBar);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Khi Media Player";
            Load += Form1_Load;
            userBar.ResumeLayout(false);
            userBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            playlistToolbar.ResumeLayout(false);
            playlistToolbar.PerformLayout();
            mediaPlayerPanel.ResumeLayout(false);
            mediaPlayerPanel.PerformLayout();
            rightClickMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip userBar;
        public ToolStripDropDownButton fileToolStripMenuItem;
        public ToolStripMenuItem addMusicsToolStripMenuItem;
        public Button PlayStop;
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
        private ColumnHeader musicDuration;
        private ColumnHeader musicPath;
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
        private ToolStripMenuItem rightClickRemoveButton;
    }
}
