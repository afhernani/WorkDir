using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Tools.Reedit;

namespace workdir
{
    public partial class Form1 : Form
    {
        string _DefaultPhotoExts = ".jpg|.jpeg|.gif|.bmp|.tif|.png|.ico";
        string _DefaultWMExts = ".asf|.wma|.avi|.mp3|.mp2|.mpa|.mid|.midi|.rmi|.aif|.aifc|.aiff|.au|.snd|.wav|.cda|.wmv|.wm|.dvr-ms|.mpe|.mpeg|.mpg|.m1v|.vob|.flv|.mp4";
        string _FavoritoFolderPath = "";

        int _MaxThumbPerTheadRun = 10;
        Thread _CreateThumbList = null;
        List<ThumbableItem> _ThumbList = new List<ThumbableItem>();

        public Form1()
        {
            InitializeComponent();
            Thread splash=new Thread(new ThreadStart(doSplash));
            //Task sp = new Task(doSplash);
            splash.Start();
            //sp.Start();
            loadtreeView();
            Thread.Sleep(1000);

            //imagenMiniaturaToolStripMenuItem.Image = workdir.Properties.Resources.icon_check;
            saveAsFavoritoToolStripMenuItem.Image = workdir.Properties.Resources.folderFavoriteSmall;
            loadFavoritoToolStripMenuItem.Image = workdir.Properties.Resources.folderfavorito;
            loadRegFavorito();
            splash.Abort();
        }

        private void doSplash()
        {
            Splash sp = new Splash();
            sp.ShowDialog();
        }

        private void loadtreeView()
        {
            DriveInfo[] drivers = DriveInfo.GetDrives();
            DirectoryInfo dir;
            TreeNode rootNode;
            foreach (DriveInfo drive in drivers)
            {
                if (drive.DriveType == DriveType.Fixed)
                {
                    dir = new DirectoryInfo(drive.Name);
                    if (dir.Exists)
                    {
                        rootNode = new TreeNode(dir.Name, 0, 0);
                        rootNode.Tag = dir;
                        GetDirectorios(dir.GetDirectories(), rootNode, 0);
                        rootNode.Expand();
                        treeView1.Nodes.Add(rootNode);
                    }

                }
            }
        }

        private void GetDirectorios(DirectoryInfo[] directoryInfo, TreeNode rootNode, int nodenivel)
        {
            if (nodenivel > 1) return; //carga dos niveles maximo.
            TreeNode aNode;
            //Color TextColor;
            foreach (DirectoryInfo subDir in directoryInfo)
            {
                aNode = new TreeNode(subDir.Name, 0, 0);
                aNode.Tag = subDir;
                try
                {
                    GetDirectorios(subDir.GetDirectories(), aNode, nodenivel + 1);
                    rootNode.Nodes.Add(aNode);
                    // .rootNode.Expand();
                }
                catch
                {

                    ;
                }
            }
            if (rootNode.Level > 1) rootNode.Expand();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1_selectNode(e.Node);
        }

        private void treeView1_selectNode(TreeNode selectedNode)
        {
            listView1.Items.Clear();
            listView1.Columns.Clear();
            thumImageList.Images.Clear();
            if (_CreateThumbList != null) _CreateThumbList.Abort();
            _ThumbList.Clear();


            DirectoryInfo nodeDirInfo = (DirectoryInfo)selectedNode.Tag;
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;
            
            if (listView1.View == View.Details)
            {
                listView1.View = View.Details;
                listView1.Columns.Add("File", 150);
                listView1.Columns.Add("Size", 65);
                listView1.Columns.Add("Last Modified", 114);
            }
            
            if (selectedNode.GetNodeCount(false) == 0)
            {
                if (nodeDirInfo.GetDirectories().Count()>0)
                {
                    GetDirectorios(nodeDirInfo.GetDirectories(), selectedNode, 1);
                }
                //MessageBox.Show(selectedNode.GetNodeCount(false).ToString());
                //MessageBox.Show(nodeDirInfo.GetDirectories().Count().ToString());
            }
            foreach (FileInfo file in nodeDirInfo.GetFiles())
            {
                item = new ListViewItem(file.Name); //inicializa el indice.
                item.Tag = file; //asigna el objeto al cual se asocia.

                subItems=new ListViewItem.ListViewSubItem[]
                    {
                        new ListViewItem.ListViewSubItem(item, GetDisplaySize(file.Length)),
                        new ListViewItem.ListViewSubItem(item,file.LastAccessTime.ToShortDateString()+" "+
                            file.LastAccessTime.ToShortDateString())
                    };
                item.SubItems.AddRange(subItems); //adicionamos los subitems al item
                listView1.Items.Add(item);//adicionamos el item
                listView1.Items[listView1.Items.Count - 1].ImageIndex = listView1.Items.Count - 1;

                if (listView1.View != View.Details)
                {
                    if (IsPhoto(file.Extension))
                    {
                        _ThumbList.Add(new ThumbableItem(listView1.Items.Count - 1, file.FullName, file.Extension));
                        thumImageList.Images.Add((Bitmap)workdir.Properties.Resources.loading);
                    }
                    else if(IsAVi(file.Extension))
                    {
                        _ThumbList.Add(new ThumbableItem(listView1.Items.Count - 1, file.FullName, file.Extension));
                        thumImageList.Images.Add((Bitmap)workdir.Properties.Resources.video);
                    }
                    else
                    {
                        thumImageList.Images.Add((Bitmap)workdir.Properties.Resources.other);
                    }
                    
                }
            }
            if (listView1.View != View.Details)
            {
                RunThumGeneratorThread();
            }
        }

        private void RunThumGeneratorThread()
        {
            if (_ThumbList.Count>0)
            {
                _CreateThumbList = new Thread(new ThreadStart(CreateThumbList));
                _CreateThumbList.Start();
            }
        }

        private void CreateThumbList()
        {
            int NumItemstoProcess = (_MaxThumbPerTheadRun < _ThumbList.Count ? _MaxThumbPerTheadRun : _ThumbList.Count);
            for (int i = 0; i < NumItemstoProcess; i++)
            {
                if (IsPhoto(_ThumbList[i].FileExtension))
                {
                    Bitmap orgImage;
                    if (_ThumbList[i].FileFullName.IndexOf("_en@@") != -1)
                    {
                        //fichero encriptado. no existe
                        orgImage = (Bitmap)Bitmap.FromFile(_ThumbList[i].FileFullName);
                    }
                    else
                    {
                        orgImage = (Bitmap)Bitmap.FromFile(_ThumbList[i].FileFullName);
                    }
                    _ThumbList[i].ThumbImage = CreateThumbnail(orgImage);
                }
                else if(IsAVi(_ThumbList[i].FileExtension))
                {
                    
                    Bitmap _image = ImportMediaThreadProc(_ThumbList[i].FileFullName, 4, 15);
                    //_image.Save(System.IO.Path.GetTempFileName()+".jpeg"); //C:\Users\hernani\AppData\Local\Temp crea un fichero temporal aleatorio.
                    //MessageBox.Show(System.IO.Path.GetTempPath());
                    _ThumbList[i].ThumbImage = CreateThumbnail(_image);
                }
            }
            UpdateLargeIcons(NumItemstoProcess);
        }

        private Image CreateThumbnail(Bitmap imgPhoto)
        {
            Bitmap thumbBmp = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(thumbBmp);
            g.FillRectangle(System.Drawing.Brushes.White, 0, 0, 100, 100);
            //ajustando para la maxima calidad.
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            int thumbWidth, thumbHeight;
            if (imgPhoto.Width>=imgPhoto.Height) //reduce el ancho a 100 manteniendo la altura proporcional
            {
                thumbWidth = 100;
                thumbHeight=(int)((double)imgPhoto.Height*(100.0/imgPhoto.Width));
            }
            else //reduce el alto a 100 manteniendo la proporción del ancho.
            {
                thumbHeight = 100;
                thumbWidth = (int)((double)imgPhoto.Width * (100.0 / imgPhoto.Height));
            }
            //dibujamos la imagen en el bitmap vacio, no usamos getThumbnailImage() del original por que es de baja calidad.
            int top = (100 - thumbHeight) / 2;
            int left = (100 - thumbWidth) / 2;

            g.DrawImage(imgPhoto, new Rectangle(left, top, thumbWidth, thumbHeight),
                new Rectangle(0, 0, imgPhoto.Width, imgPhoto.Height), GraphicsUnit.Pixel);
            g.Dispose();
            return thumbBmp;
        }

        private delegate void UpadateLargeIconsDelegate(int ItemsProcessed);

        private void UpdateLargeIcons(int ItemsProcessed)
        {
            if (listView1.InvokeRequired)
            {
                UpadateLargeIconsDelegate d = new UpadateLargeIconsDelegate(UpdateLargeIcons);
                listView1.BeginInvoke(d, new object[] { ItemsProcessed });
            }
            else
            {
                for (int i = 0; i < ItemsProcessed; i++)
                {
                    thumImageList.Images[_ThumbList[i].ThumbListIndex] = _ThumbList[i].ThumbImage;
                }
                listView1.Refresh();
                //elimina proceso thumbs
                for (int j = 0; j < ItemsProcessed; j++)
                {
                    _ThumbList.RemoveAt(0);
                }
                //si todavia existe thumbs sin procesar generarlos
                if (_ThumbList.Count>0)
                {
                    RunThumGeneratorThread();
                }
            }

        }

        private string GetDisplaySize(long fileSize)
        {
            string retSize = fileSize.ToString();
            if (fileSize > 1048576)
            {
                retSize = ((double)fileSize / 1048576).ToString("n1") + " MB";
            }
            else if (fileSize>1024)
            {
                retSize = ((double)fileSize / 1024).ToString("n1") + " KB";
            }
            return retSize;
        }

        private void imagenMiniaturaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imagenMiniaturaToolStripMenuItem.Image != null)
            {
                imagenMiniaturaToolStripMenuItem.Image = null;
                listView1.View = View.Details;
            }
            else
            {
                imagenMiniaturaToolStripMenuItem.Image = workdir.Properties.Resources.icon_check;
                listView1.View = View.Tile;
            }
            if (treeView1.SelectedNode != null)
            {
                treeView1_selectNode(treeView1.SelectedNode); //refrescamos la situacion
            }
        }
        private bool IsAVi(string p)
        {
            return _DefaultWMExts.ToUpper().Contains(p.ToUpper());
        }
        private bool IsPhoto(string p)
        {
            return _DefaultPhotoExts.ToUpper().Contains(p.ToUpper());
        }

        private void saveAsFavoritoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode !=null)
            {
                DirectoryInfo nodeDirInfo = (DirectoryInfo)treeView1.SelectedNode.Tag;
                _FavoritoFolderPath = nodeDirInfo.FullName;
                Reedit.SetRegistryKey("FavFolderPath", _FavoritoFolderPath);
                loadFavoritoToolStripMenuItem.Enabled = true;
                loadFavoritoToolStripMenuItem.ToolTipText = _FavoritoFolderPath;
            }
        }

        private void loadRegFavorito() 
        {
            _FavoritoFolderPath = Reedit.GetRegistryKey("FavFolderPath");
            if (_FavoritoFolderPath !="")
            {
                loadFavoritoToolStripMenuItem.ToolTipText = _FavoritoFolderPath;
            }
            else
            {
                loadFavoritoToolStripMenuItem.Enabled = false;
            }
        }

        private void loadFavoritoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_FavoritoFolderPath != "")
            {
                string[] arrPath = _FavoritoFolderPath.Split('\\');
                arrPath[0] += "\\";
                TreeNodeCollection nodeList = treeView1.Nodes;
                TreeNode selectedNode = null;
                DirectoryInfo dirInfo;
                bool nodefound;
                for (int i = 0; i < arrPath.Length; i++)
                {
                    nodefound = false;
                    for (int j = 0; j < nodeList.Count; j++)
                    {
                        dirInfo = (DirectoryInfo)nodeList[j].Tag;
                        if (dirInfo.Name==arrPath[i])
                        {
                            selectedNode = nodeList[j];
                            selectedNode.Expand();
                            nodefound = true;
                            if (selectedNode.GetNodeCount(false)==0)
                            {
                                if (dirInfo.GetDirectories().Count() > 0)
                                {
                                    GetDirectorios(dirInfo.GetDirectories(), selectedNode, 1);
                                }
                            }
                            break;
                        }
                        else
                        {
                            nodeList[j].Collapse();
                        }
                    }
                    if (!nodefound || selectedNode.Nodes.Count ==0)
                    {
                        break;
                    }
                    else
                    {
                        nodeList = selectedNode.Nodes;
                    }
                }
                if (selectedNode != null)
                {
                    selectedNode.Expand();
                    treeView1.SelectedNode = selectedNode;
                    treeView1_selectNode(selectedNode);
                }
            }
        }
        #region mediathumbail

        private Bitmap ImportMediaThreadProc(string mediaFile, int waitTime, int position)
        {
            MediaPlayer player = new MediaPlayer { Volume = 0, ScrubbingEnabled = true };
            player.Open(new Uri(mediaFile));
            player.Pause();
            player.Position = TimeSpan.FromMilliseconds(position * 1000);
            //We need to give MediaPlayer some time to load. The efficiency of the MediaPlayer depends                 
            //upon the capabilities of the machine it is running on
            System.Threading.Thread.Sleep(waitTime * 1000);

            //120 = thumbnail width, 90 = thumbnail height and 96x96 = horizontal x vertical DPI
            //An in actual application, you wouldn's probably use hard coded values!
            RenderTargetBitmap rtb = new RenderTargetBitmap(240, 180, 0, 0, PixelFormats.Default);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawVideo(player, new Rect(0, 0, 240, 180));
            }
            rtb.Render(dv);
            Duration duration = player.NaturalDuration;
            int videoLength = 0;
            if (duration.HasTimeSpan)
            {
                videoLength = (int)duration.TimeSpan.TotalSeconds;
            }
            BitmapFrame frame = BitmapFrame.Create(rtb).GetCurrentValueAsFrozen() as BitmapFrame;
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(frame as BitmapFrame);
            //We cannot create the thumbnail here, as we are not in the UI thread right now
            //It is the responsibility of the calee to close the MemoryStream
            //We will instead call a method which will do its stuff on the UI thread!
            MemoryStream memoryStream = new MemoryStream();
            encoder.Save(memoryStream);
            //CreateMediaItem(memoryStream, mediaFile, videoLength);
            //memoryStream.Position = 0;
            player.Close();
            return new Bitmap(memoryStream);
        }

        #endregion mediathumbail

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_CreateThumbList != null)
            {
                _CreateThumbList.Abort();
            }
        }

    }
}
