using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace zy_cutPicture
{
    public partial class SequenceForm : Form
    {
        private List<PictureBoxX> pictureBoxes = new List<PictureBoxX>();
        public SequenceForm()
        {
            InitializeComponent();
        }

        public void init()
        {
            panelWorkArea.AllowDrop = true;
            panelWorkArea.DragEnter += panelWorkArea_DragEnter;
            panelWorkArea.DragDrop += panelWorkArea_DragDrop;
        }

        private void Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "图片文件 (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    AddPictureBox(filePath);
                }
            }
        }

        private void AddPictureBox(string filePath)
        {
            try
            {
                PictureBoxX pictureBox = new PictureBoxX();
                pictureBox.Image = Image.FromFile(filePath);
                pictureBox.ImageX = pictureBox.Image;
                pictureBox.Image = null;
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox.Location = new Point(10, 10);
                pictureBox.BackColor = Color.Transparent;
                pictureBox.MouseDown += PictureBox_MouseDown;
                pictureBox.MouseMove += PictureBox_MouseMove;
                pictureBox.MouseUp += PictureBox_MouseUp;
                panelWorkArea.Controls.Add(pictureBox);
                panelWorkArea.Paint += panelWorkArea_Paint;
                pictureBoxes.Add(pictureBox);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载图片时出错: {ex.Message}");
            }
        }
        private void panelWorkArea_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < pictureBoxes.Count; i++)
            {
                Graphics g = e.Graphics;
                Image img = pictureBoxes[i].ImageX;
                g.DrawImage(img, new Point(pictureBoxes[i].Location.X, pictureBoxes[i].Location.Y));
            }

        }
        private Point lastMousePosition;
        private bool isDragging = false;

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastMousePosition = e.Location;
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                PictureBox pictureBox = (PictureBox)sender;
                int deltaX = e.X - lastMousePosition.X;
                int deltaY = e.Y - lastMousePosition.Y;
                pictureBox.Left += deltaX;
                pictureBox.Top += deltaY;
                panelWorkArea.Invalidate();
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
                panelWorkArea.Invalidate();
            }
        }

        private void panelWorkArea_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void panelWorkArea_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string filePath in files)
            {
                if (Path.GetExtension(filePath).ToLower() == ".jpg" ||
                    Path.GetExtension(filePath).ToLower() == ".jpeg" ||
                    Path.GetExtension(filePath).ToLower() == ".png" ||
                    Path.GetExtension(filePath).ToLower() == ".bmp")
                {
                    AddPictureBox(filePath);
                }
            }
        }
        private void newMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void openMenuItem_Click(object sender, EventArgs e)
        {
            this.Open();
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAsMenuItem_Click(object sender, EventArgs e)
        {

        }

        public class PictureBoxX : PictureBox
        {
            public Image _imageX;
            public Image ImageX
            {
                get
                {
                    return _imageX;
                }
                set
                {
                    _imageX = value;
                }
            }          
            public int LayerOder;
        }
    }
}
