using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mergePicture
{
    public partial class Form1 : Form
    {
        private List<Bitmap> bms;
        private Bitmap result;
        private string filter;

        public Form1()
        {
            InitializeComponent();
            this.filter = "Image Files(*.BMP;*.JPG;*.PNG;)|*.BMP;*.JPG;*.PNG|All files (*.*)|*.*";
            this.bms = new List<Bitmap>();

            pictureBox1.AllowDrop = true;
            pictureBox2.AllowDrop = true;
            pictureBox3.AllowDrop = true;

            test();
        }

        private void test()
        {
            if (this.bms.Count() == 0) btnImage2.Enabled = false;
            else btnImage2.Enabled = true;
            if (this.bms.Count() == 1) btnImage2.Enabled = false;
            else btnImage2.Enabled = true;
            if (this.bms.Count() == 2) btnImage2.Enabled = false;
            else btnImage2.Enabled = true;
        }

        private void btnImage1_Click(object sender, EventArgs e)
        {
            Bitmap bm = openImage(pictureBox1);
            if(bm == null && this.bms.Count() > 0)
            {
                this.bms.Remove(this.bms[0]);
            }
            else
            {
                if (this.bms.Count() == 0) this.bms.Add(bm);
                else this.bms[0] = bm;
            }
            btnImage2.Enabled = true;
        }


        private void btnImage2_Click(object sender, EventArgs e)
        {
            Bitmap bm = openImage(pictureBox2);
            if (bm == null && this.bms.Count() > 1)
            {
                this.bms.Remove(this.bms[1]);
            }
            else
            {
                if (this.bms.Count() == 1) this.bms.Add(bm);
                else this.bms[1] = bm;
            }
            //btnImage3.Enabled = true;
        }


        private void btnImage3_Click(object sender, EventArgs e)
        {
            Bitmap bm = openImage(pictureBox3);
            if (bm == null && this.bms.Count() > 2) this.bms.Remove(this.bms[2]);
            else
            {
                if(this.bms.Count() == 2) this.bms.Add(bm);
                else this.bms[2] = bm;
            }
        }



        private void mergeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                this.result = combine(this.bms);
                pictureBoxResult.Image = this.result;
                resizePictureBox(pictureBoxResult, this.result);
            }
            catch (Exception ex)
            {
                errorText(ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            this.bms.Clear();
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;

            pictureBoxResult.Image = null;
            pictureBoxResult.Height = 158;
            pictureBoxResult.Width =  518;

            labelError.Visible = false;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveImage(this.result);
        }


        private void testButton_Click(object sender, EventArgs e)
        {
            testLabel.Text = this.bms.Count().ToString();
        }


        ///////////////////////////////////////////////////

        private void errorText(string text = "Erreur")
        {
            labelError.Visible = true;
            labelError.Text = text;
        }

        private void resizePictureBox(PictureBox pb, Bitmap bm)
        {
            pb.Height = bm.Height / 8;
            pb.Width = bm.Width / 8;
        }

        private static Bitmap combine(List<Bitmap> sources)
        {
            List<int> imageHeights = new List<int>();
            List<int> imageWidths = new List<int>();

            foreach(Bitmap img in sources)
            {
                imageHeights.Add(img.Height);
                imageWidths.Add(img.Width);
            }

            Bitmap result = new Bitmap(imageWidths.Sum(), imageHeights.Max());
            
            using (Graphics g = Graphics.FromImage(result))
            {
                int width = 0;
                for(int i=0; i<sources.Count; i++)
                {
                    if(i==0) g.DrawImage(sources[i], 0, 0);
                    else g.DrawImage(sources[i], width, 0);
                    width += sources[i].Width;
                }
            }
            return result;
        }

        private Bitmap openImage(PictureBox pb)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = this.filter;

            Bitmap bm = null;
            if (ofd.ShowDialog() == DialogResult.OK) bm = new Bitmap(ofd.OpenFile());   
            pb.Image = bm;
            
            return bm;
        }

        private void saveImage(Image img)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = this.filter;
            sfd.FileName = "combinedImage";

            if(sfd.ShowDialog() == DialogResult.OK) img.Save(sfd.FileName);
        }

        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(DataFormats.FileDrop);
            if (data != null)
            {
                var filename = data as string[];
                if (filename.Length > 0)
                {

                }
            }
        }
    }
}
