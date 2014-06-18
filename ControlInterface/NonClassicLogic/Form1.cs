﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NonClassicLogic
{
    public partial class Form1 : Form
    {
        //оступ от границы pictureBox'a при отрисовки
        const int INDENT = 40;
        //пусть 1 метр = 3 пикселям, тогда максимальная высота волны = 24 пикселя
        const int testWave = 24;


        //битмап вида сбоку
        static Bitmap sideView;
        //графика вида сбоку
        static Graphics sideViewGraphics;
        //битмап вида сверху
        static Bitmap topView;
        //графика вида сверху
        static Graphics topViewGraphics;

        public Form1()
        {
            InitializeComponent();
            //инициализация вида сбоку
            sideView = new Bitmap(sidewayViewPicture.Size.Width, sidewayViewPicture.Size.Height);
            sideViewGraphics = Graphics.FromImage(sideView);
            sidewayViewPicture.Image = sideView;
            //инициализация вида сверху
            topView = new Bitmap(topViewPicture.Size.Width, topViewPicture.Size.Height);
            topViewGraphics = Graphics.FromImage(topView);
            topViewPicture.Image = topView;

            //тесты...
            drawWaveSideway(testWave);
            //отрисовка корабля
            drawShipSideway(testWave);
            //отрисовка люльки крана и груза
            drawCraneWithCargoSideWay(sideView.Size.Width/2 - 40, -60, 30);


            //отрисовка корабля в виде сверху
            drawShipTopView();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void sidewayViewPicture_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        

        ////////////////////////////SIDEWAY////////////////////////////////

        //отрисовка всего вида сбоку по входным параметрам
        private void drawSideView(int waveHeight = 0, int cranePos = 0, int deltaWind = 0, int distance = 15)
        {
            drawWaveSideway(waveHeight);
            drawShipSideway(waveHeight);
            drawCraneWithCargoSideWay(cranePos, deltaWind, distance);
        }


        //отрисовка корабля в виде сбоку, высота волны входной параметр
        private void drawShipSideway(int waveHeight = 0)
        {
            // Create points that define polygon.
            Point point1 = new Point(0 + INDENT, sideView.Size.Height - 40 - waveHeight);
            Point point2 = new Point(0 + 40 + INDENT, sideView.Size.Height - waveHeight);
            Point point3 = new Point(sideView.Size.Width - 40 - INDENT, sideView.Size.Height - waveHeight);
            Point point4 = new Point(sideView.Size.Width - INDENT, sideView.Size.Height - 40 - waveHeight);
            Point point5 = new Point(sideView.Size.Width - 40 - INDENT, sideView.Size.Height - 40 - waveHeight);
            Point point6 = new Point(sideView.Size.Width - 40 - INDENT, sideView.Size.Height - 80 - waveHeight);
            Point point7 = new Point(sideView.Size.Width - 80 - INDENT, sideView.Size.Height - 80 - waveHeight);
            Point point8 = new Point(sideView.Size.Width - 80 - INDENT, sideView.Size.Height - 40 - waveHeight);
            Point[] curvePoints =
            {
                 point1,
                 point2,
                 point3,
                 point4,
                 point5,
                 point6,
                 point7,
                 point8
            };
            //заполнить область корабля белым цветом
            sideViewGraphics.FillPolygon(new SolidBrush(Color.White), curvePoints);
            //отрисовать корабль
            sideViewGraphics.DrawPolygon(new Pen(Color.Black, 3), curvePoints);
            //обновить изображение
            Invalidate();
        }

        //отрисовка волны
        private void drawWaveSideway(int waveHeight = 0)
        {
            Point[] curvePoints = new Point[10];
            int step = sideView.Size.Width / 4;
            int highPoints = 0;
            int lowPoints = 0;
            for (int i = 1; i < 10; i += 2) 
            { 
                curvePoints[i - 1] = new Point(lowPoints, sideView.Size.Height);
                curvePoints[i] = new Point(highPoints, sideView.Size.Height - waveHeight);
                highPoints += step;
                lowPoints += step;
            }
            sideViewGraphics.DrawCurve(new Pen(Color.Blue, 3), curvePoints);
            Invalidate();
        }

        //отрисовка люльки крана и груза (позиция люльки крана, смещение груза из-за ветра, расстояние от люльки до груза, по умолчанию 5 метров)
        private void drawCraneWithCargoSideWay(int cranePos = 0, int deltaWind = 0, int distance = 15)
        {
            //отрисовка люльки
            Point point1 = new Point(cranePos, 0);
            Point point2 = new Point(cranePos + 80, 0);
            Point point3 = new Point(cranePos + 80, 40);
            Point point4 = new Point(cranePos, 40);
            Point[] curvePoints = 
            {
                point1,
                point2,
                point3,
                point4
            };
            sideViewGraphics.FillPolygon(new SolidBrush(Color.White), curvePoints);
            sideViewGraphics.DrawPolygon(new Pen(Color.Black, 3), curvePoints);
            Invalidate();

            //отрисовка груза (длина = 36, высота = 9, точка соединения = 18)
            Point p1 = new Point(cranePos + deltaWind + 40 - 18, 40 + distance);
            Point p2 = new Point(cranePos + deltaWind + 40 - 18, 40 + 9 + distance);
            Point p3 = new Point(cranePos + deltaWind  + 40 + 18, 40 + 9 + distance);
            Point p4 = new Point(cranePos + deltaWind  + 40 + 18, 40 + distance);
            Point[] cargoPoints = 
            {
                p1,
                p2,
                p3,
                p4
            };
            sideViewGraphics.FillPolygon(new SolidBrush(Color.White), cargoPoints);
            sideViewGraphics.DrawPolygon(new Pen(Color.Black, 3), cargoPoints);
            Invalidate();

            //отрисовка каната от люльки до груза
            sideViewGraphics.DrawLine(new Pen(Color.Black, 3), new Point(cranePos + 40, 40), new Point(cranePos + 40 + deltaWind, 40 + distance));
        }



        ////////////////////////////TopView////////////////////////////////

        //отрисовка корабля в виде сверху
        private void drawShipTopView()
        {
            //ширина корабля 16 контейнеров = 2*2*16 = 64 пикселей
            Point point1 = new Point(topView.Size.Width / 2, INDENT);
            Point point2 = new Point((topView.Size.Width / 2) - 32, 20 + INDENT);
            Point point3 = new Point((topView.Size.Width / 2) - 32, topView.Size.Height - INDENT - 20);
            Point point4 = new Point(topView.Size.Width / 2, topView.Size.Height - INDENT);
            Point point5 = new Point((topView.Size.Width / 2) + 32, topView.Size.Height - INDENT - 20);
            Point point6 = new Point((topView.Size.Width / 2) + 32, 20 + INDENT);
            Point[] curvePoints = 
            {
                point1,
                point2,
                point3,
                point4,
                point5,
                point6
            };
            topViewGraphics.FillPolygon(new SolidBrush(Color.White), curvePoints);
            topViewGraphics.DrawPolygon(new Pen(Color.Black, 3), curvePoints);
        }

        //отрисовка крана с люлькой для вида сверху(позиция люльки, смещение груза)
        private void drawCraneTopView(int cranePos = 0, int deltaWind = 0)
        {
            
        }




        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void topViewPicture_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
