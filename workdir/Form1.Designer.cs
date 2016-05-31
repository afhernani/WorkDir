namespace workdir
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.favoritoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsFavoritoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFavoritoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imagenMiniaturaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.listView1 = new System.Windows.Forms.ListView();
            this.thumImageList = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView1);
            this.splitContainer1.Size = new System.Drawing.Size(486, 393);
            this.splitContainer1.SplitterDistance = 162;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(162, 393);
            this.treeView1.TabIndex = 0;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.favoritoToolStripMenuItem,
            this.imagenMiniaturaToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(169, 48);
            // 
            // favoritoToolStripMenuItem
            // 
            this.favoritoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsFavoritoToolStripMenuItem,
            this.loadFavoritoToolStripMenuItem});
            this.favoritoToolStripMenuItem.Name = "favoritoToolStripMenuItem";
            this.favoritoToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.favoritoToolStripMenuItem.Text = "Favorito";
            // 
            // saveAsFavoritoToolStripMenuItem
            // 
            this.saveAsFavoritoToolStripMenuItem.Name = "saveAsFavoritoToolStripMenuItem";
            this.saveAsFavoritoToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.saveAsFavoritoToolStripMenuItem.Text = "Save as Favorito";
            this.saveAsFavoritoToolStripMenuItem.Click += new System.EventHandler(this.saveAsFavoritoToolStripMenuItem_Click);
            // 
            // loadFavoritoToolStripMenuItem
            // 
            this.loadFavoritoToolStripMenuItem.Name = "loadFavoritoToolStripMenuItem";
            this.loadFavoritoToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.loadFavoritoToolStripMenuItem.Text = "Load Favorito";
            this.loadFavoritoToolStripMenuItem.Click += new System.EventHandler(this.loadFavoritoToolStripMenuItem_Click);
            // 
            // imagenMiniaturaToolStripMenuItem
            // 
            this.imagenMiniaturaToolStripMenuItem.Name = "imagenMiniaturaToolStripMenuItem";
            this.imagenMiniaturaToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.imagenMiniaturaToolStripMenuItem.Text = "Imagen miniatura";
            this.imagenMiniaturaToolStripMenuItem.Click += new System.EventHandler(this.imagenMiniaturaToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "icon-folder-load.gif");
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.LargeImageList = this.thumImageList;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(320, 393);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // thumImageList
            // 
            this.thumImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("thumImageList.ImageStream")));
            this.thumImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.thumImageList.Images.SetKeyName(0, "loading");
            this.thumImageList.Images.SetKeyName(1, "video");
            this.thumImageList.Images.SetKeyName(2, "others");
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 393);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Workdir";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList thumImageList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem favoritoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsFavoritoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFavoritoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imagenMiniaturaToolStripMenuItem;
    }
}

