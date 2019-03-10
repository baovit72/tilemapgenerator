using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
namespace TileMapCSharp
{
    class GenerateTilesets
    {
        private Image currentImage = null;
        private List<int> tilemap;
        private int tilesetSize;
        private List<Image> tilesets = null;
        static private GenerateTilesets _instance = null;
        private GenerateTilesets()
        {
            tilesets = new List<Image>();
            tilemap = new List<int>();
        }
        static public GenerateTilesets getInstance()
        {
            if (_instance == null)
            {
                _instance = new GenerateTilesets();
            }
            return _instance;
        }
        public List<int> getTilemap()
        {
            return this.tilemap;
        }
        public bool reset()
        {
            currentImage = null;
            tilemap = new List<int>();
            tilesets = new List<Image>();
            tilesetSize = 0;
            return true;
        }
        public bool loadImage(string imgUrl)
        {
            this.currentImage = Image.FromFile(imgUrl);
           
            if (this.currentImage != null)
                return true;
            return false;
        }
        private bool compareImages(Bitmap imgA, Bitmap imgB)
        {
            if (imgA.Size != imgB.Size)
                return false;
            for (int y = 0; y < imgA.Size.Height; y++)
            {
                for (int x = 0; x < imgB.Size.Width; x++)
                {
                    if (imgA.GetPixel(x, y) != imgB.GetPixel(x, y))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private Bitmap cropImage(Rectangle rect)
        {
            Bitmap croppedImg = null;
            Bitmap bmpImg = (Bitmap)this.currentImage;
            System.Drawing.Imaging.PixelFormat format = bmpImg.PixelFormat;
            try
            {
                croppedImg = bmpImg.Clone(rect, format);
            }
            catch (OutOfMemoryException e)
            {
                Debug.WriteLine(rect.X);
                Debug.WriteLine(rect.Y);
            }
            return croppedImg;
        }
        public bool generateTilesets(int tileSize)
        {
            if (this.currentImage == null)
                return false;
            this.tilesetSize = tileSize;
            //Sample image (lv1.png) size is 1536 x 384 and do assume it is divisible by 32  or 16
            //Crop image to pieces
            int columnSize = this.currentImage.Width / tileSize;
            int rowSize = this.currentImage.Height / tileSize;
            //Kernel tileSize x tileSize goes from left to right, top to bottom
            for (int i = 0; i < rowSize; i++)
            {
                for (int j = 0; j < columnSize; j++)
                {
                    Rectangle rect = new Rectangle(j * tileSize, i * tileSize, tileSize, tileSize);
                    Bitmap croppedImg = cropImage(rect);

                    bool flag = false;
                    for (int k = 0; k < this.tilesets.Count; k++)
                    {
                        if (compareImages((Bitmap)this.tilesets[k], croppedImg))
                        {
                            this.tilemap.Add(k);
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        this.tilesets.Add(croppedImg);
                        this.tilemap.Add(this.tilesets.Count -1);
                    }


                }
                this.tilemap.Add(-1);
            }
            return true;
        }
        public Image getTilesetImage()
        {
            Bitmap image = null;
            var bitmap = new Bitmap(this.tilesetSize * this.tilesets.Count, this.tilesetSize);
            using (var g = Graphics.FromImage(bitmap))
            {
                for (int i = 0; i < this.tilesets.Count; i++)
                {
                    g.DrawImage(this.tilesets[i], i * this.tilesetSize, 0);
                }
                 
            }
            return image = bitmap;


        }
    }
}
