using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace TileMapCSharp
{
    class GenerateMap
    {
        static GenerateMap _instance = null;
        private List<int> tilemap;
        private Image tilesets = null;
        private Image map = null;
        private int rmapWidth = 0;
        private int rmapHeight = 0;
        private GenerateMap()
        {
            this.tilemap = new List<int>();
        }
        public static GenerateMap getInstance()
        {
            if (_instance == null)
                _instance = new GenerateMap();
            return _instance;
        }
        public bool loadImage(string url)
        {
            this.tilesets = Image.FromFile(url);
            return true;
        }
        public bool loadTilemap(string url)
        {
            foreach (string line in File.ReadLines(url))
            {
                this.rmapHeight++; 
                string[] splitter = line.Split(' ');
                this.rmapWidth = 0;
                foreach (string str in splitter)
                {
                    Debug.WriteLine(str);
                    Debug.WriteLine("*");
                    try
                    {
                        this.tilemap.Add(int.Parse(str));
                        this.rmapWidth++;
                    }
                    catch(FormatException e)
                    {
                        Debug.WriteLine(str);
                    }
                }
                this.tilemap.Add(-1);
            }
            return true;
        }
        public bool reset()
        {
            tilemap = new List<int>();
            tilesets = null;
            rmapWidth = 0;
            rmapHeight = 0;
            return true;
        }
        private Bitmap cropImage(Rectangle rect)
        {
            Bitmap croppedImg = null;
            Bitmap bmpImg = (Bitmap)this.tilesets;
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
        public bool generateMap(int tilesetSize)
        {
            int tilesetCount = this.tilesets.Width / tilesetSize;
            Bitmap map = new Bitmap(this.rmapWidth * tilesetSize, this.rmapHeight * tilesetSize);
            Debug.WriteLine(this.rmapWidth);
            Debug.WriteLine(this.rmapHeight);
            using (var g = Graphics.FromImage(map))
            {
                int y = 0;
                int x = 0;
                for (int i = 0; i < this.tilemap.Count; i++)
                {
                    
                    int temp = this.tilemap[i];
                    if (temp == -1)
                    {
                        x = 0;
                        y++;
                        continue;
                    }
                    Bitmap croppedImg = cropImage(new Rectangle(temp * tilesetSize, 0, tilesetSize, tilesetSize));
                    g.DrawImage(croppedImg, x * tilesetSize, y * tilesetSize);
                    x++;
                }
                this.map = map;
            }
            return true;
        }
        public Image getMap()
        {
           
            return this.map;
        }

    }

}
