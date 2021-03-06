﻿using System;
using CSJ2K;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.IO;

namespace OpenSatelliteProject.IMTools {
    public class ImageAssembler {

        readonly Image16 image;
        int posY;
        int segmentHeight;
        ulong epoch;

        public int Width {
            get { return image.Width; }
        }

        public int Height {
            get { return image.Height; }
        }

        public bool Done {
            get {
                return posY >= image.Height;
            } 
        }

        public Image16 Image {
            get {
                return image;
            }
        }

        public ulong Epoch {
            get {
                return epoch;
            }
        }

        public ImageAssembler (int width, int height, ulong epoch = 0) {
            image = new Image16 (width, height);
            posY = 0;
            segmentHeight = 0;
            this.epoch = epoch;
        }

        public void AppendJ2K(string filename, bool resizeIfNeeded = false) {
            try {
                var j2k = J2kImage.FromFile(filename);
                image.DrawImage (j2k.GetComponent(0), j2k.Width, j2k.Height, 0, posY, resizeIfNeeded);
                posY += j2k.Height;
                segmentHeight = j2k.Height;
            } catch (Exception e) {
                Console.WriteLine ($"Corrupted Segment ({e.GetType().Name}): {e.Message}");
                posY += segmentHeight;
            }
        }

        public void DrawAt(string filename, int x, int y, bool resizeIfNeeded = false) {
            try {
                var j2k = J2kImage.FromFile(filename);
                image.DrawImage (j2k.GetComponent(0), j2k.Width, j2k.Height, x, y, resizeIfNeeded);
            } catch (Exception e) {
                Console.WriteLine ($"Corrupted Segment ({e.GetType().Name}): {e.Message}");
            }
        }

        public void DrawAt(Image16 img, int x, int y, bool resizeIfNeeded = false) {
            image.DrawImage (img, x, y, resizeIfNeeded);
        }

        public void SavePGM(string filename) {
            image.SavePGM (filename);
        }

        public void SavePNG(string filename) {
            var bmp = image.ToBitmap ();
            bmp.Save (filename, ImageFormat.Png);
            bmp.Dispose ();
        }

        public void SaveJPG(string filename) {
            var bmp = image.ToBitmap ();
            bmp.Save (filename, ImageFormat.Jpeg);
            bmp.Dispose ();
        }


        public async Task AsyncSavePGM(string filename) {
            await Task.Run (() => {
                image.SavePGM (filename);
                // Console.WriteLine($"File {filename} saved.");
            });
        }

        public async Task AsyncSavePNG(string filename) {
            await Task.Run (() => {
                var bmp = image.ToBitmap ();
                bmp.Save (filename, ImageFormat.Png);
                bmp.Dispose ();
                // Console.WriteLine($"File {filename} saved.");
            });
        }

        public async Task AsyncSaveJPG(string filename) {
            await Task.Run (() => {
                var bmp = image.ToBitmap ();
                bmp.Save (filename, ImageFormat.Jpeg);
                bmp.Dispose ();
                // Console.WriteLine($"File {filename} saved.");
            });
        }
    }
}

