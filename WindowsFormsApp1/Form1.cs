using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Emgu.CV.VideoCapture _capture;
        private CascadeClassifier _cascadeClassifier;

        public Form1()
        {
            InitializeComponent();
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _capture = new VideoCapture();
            Application.Idle += new EventHandler(FaceRecognition);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //_capture.Stop();
            //imgCamUser.Image = null;
            //Application.Exit();
            Application.Idle -= new EventHandler(FaceRecognition);
            if (_capture != null)
            {
                _capture.Dispose();
            }
        }

        private void FaceRecognition(object sender, EventArgs e)
        {  //run this until application closed (close button click on image viewer)
            //imgCamUser.Image = _capture.QueryFrame(); //draw the image obtained from camera

            //Draw face marker
            _cascadeClassifier = new CascadeClassifier(Application.StartupPath + "/haarcascade_frontalface_alt_tree.xml");
            using (var imageFrame = _capture.QueryFrame().ToImage<Bgr, Byte>())
            {
                if (imageFrame != null)
                {
                    var grayframe = imageFrame.Convert<Gray, byte>();
                    var faces = _cascadeClassifier.DetectMultiScale(grayframe, 1.2, 3, Size.Empty); //the actual face detection happens here 1)convert in greyscale 2)[1.1]factor near to 1(slow detection but precise) but must be over 1 3)[10]higher fewer false positive
                    foreach (var face in faces)
                    {
                        imageFrame.Draw(face, new Bgr(Color.BurlyWood), 3); //the detected face(s) is highlighted here using a box that is drawn around it/them

                    }
                }
                imgCamUser.Image = imageFrame;
            }
        }

    }
}
