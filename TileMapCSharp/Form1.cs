using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace TileMapCSharp
{
    public partial class Form1 : Form
    { 
        private Image tilesets;
        private string fileUrl;
        private int tilesetSize;
        private GenerateTilesets generateTilesets = null;
        private GenerateMap generateMap = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            generateTilesets = GenerateTilesets.getInstance();
            generateMap = GenerateMap.getInstance();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.fileUrl = openFileDialog1.FileName;
                textBox3.Text = this.fileUrl;
            }

        }
        private void generatorReset()
        {
            this.generateTilesets.reset();
            this.generateMap.reset();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //Clear generators' data
            generatorReset();
            //Clear output textboxq
            this.textBox2.Text = "";
            this.tilesetSize = int.Parse(textBox1.Text);
            if (this.fileUrl == null)
                return;
            //Load sample image
            generateTilesets.loadImage(this.fileUrl);
            //Begin generating tilesets with  given parameters
            generateTilesets.generateTilesets(this.tilesetSize);
            //Get tilemap and show it do textbox
            List<int> tilemap = generateTilesets.getTilemap();
            //Debug
            MessageBox.Show(tilemap.Count.ToString());
            for (int i = 0; i < tilemap.Count; i++)
            {
                if (tilemap[i] == -1)
                    this.textBox2.Text += "\r\n";
                else
                {
                    this.textBox2.Text += tilemap[i].ToString();
                    this.textBox2.Text += " ";
                }
            }
            //Display tilesets
            this.tilesets = generateTilesets.getTilesetImage();
            this.pictureBox1.Image = this.tilesets;
            
            

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, this.textBox2.Text);
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image.Save(saveFileDialog1.FileName, ImageFormat.Png);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox2.Text = File.ReadAllText(openFileDialog1.FileName);
                this.generateMap.loadTilemap(openFileDialog1.FileName);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Clear generators' data
            this.generateMap.loadImage(this.fileUrl);
            this.generateMap.generateMap(int.Parse(this.textBox1.Text));
            this.pictureBox1.Image = this.generateMap.getMap();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
