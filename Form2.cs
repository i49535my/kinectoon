﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

// ビットマップを生成するため
using System.Drawing.Imaging;

// Kinectのセンサクラス
// Microsoft.Kinectを参照に追加
using Microsoft.Kinect;

// メモリ管理の最適化のため
// System.Runtime.Serializationを参照に追加
using System.Runtime.InteropServices;

namespace ShapeGame
{
    public partial class Form2 : Form
    {

        /// <summary>
        /// Kinectのセンサクラス
        /// </summary>
        KinectSensor kinect;

        /// <summary>
        /// Kinectから取得したRGBデータ
        /// （byte型配列）
        /// </summary>
        byte[] imageData;

        /// <summary>
        /// 実画像
        /// （ビットマップ形式）
        /// </summary>
        Bitmap imageBitmap;

        public Form2()
        {
            InitializeComponent();

            // ビットマップの初期化
            imageBitmap = new Bitmap(640, 480);

            // kinectの初期化
            kinect = KinectSensor.KinectSensors[0];

            // カラー画像の取得を開始する
            kinect.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(ColorFrameReady);
            kinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

            // kinectを起動する
            kinect.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 画像の保存
            if ( (MainWindow.X)%2 != 0)
            {
                imageBitmap.Save("仮" + MainWindow.X + ".bmp");
                MainWindow.picadd = MainWindow.path;
                MainWindow.path = @"C:\H28創造工学\ShapeGame\bin\Debug\仮" + MainWindow.X + ".bmp";
                
    }
            else if ((MainWindow.X)%2 == 0)
            {
                imageBitmap.Save("かり" + MainWindow.X + ".bmp");
                MainWindow.picadd = MainWindow.path;
                MainWindow.path = @"C:\H28創造工学\ShapeGame\bin\Debug\かり" + MainWindow.X + ".bmp";
                
            }
            MainWindow.X++;
            MainWindow.END = -1;
            this.Close();

        }

        void ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            // kinectからカラーイメージを取得
            ColorImageFrame image = e.OpenColorImageFrame();

            // imageがnullだった場合処理しない
            if (image != null)
            {
                // imageData配列の初期化
                imageData = new byte[image.PixelDataLength];

                // imageのピクセルデータをpixelDataへコピーする
                image.CopyPixelDataTo(imageData);

                // imageDataからビットマップへ変換する
                imageBitmap = toBitmap(imageData, imageBitmap.Width, imageBitmap.Height);

                // ピクチャーボックスへ反映
                pictureBox1.Image = imageBitmap;

            }
        }

        /// 取得データをビットマップデータに変換
        /// </summary>
        /// <param name="pixels">kinectで取得したbyte[]配列</param>
        /// <param name="width">横サイズ</param>
        /// <param name="height">縦サイズ</param>
        /// <returns></returns>
        public static Bitmap toBitmap(byte[] pixels, int width, int height)
        {
            // pixelsに何も入っていない場合nullを返す
            if (pixels == null)
                return null;

            // ビットマップの初期化
            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);

            // システムメモリへロック
            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

            // メモリデータのコピー
            Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);

            // システムメモリのロック解除
            bitmap.UnlockBits(data);

            return bitmap;
        }

    }
}
