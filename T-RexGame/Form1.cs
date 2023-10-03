using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T_RexGame
{
    public partial class Form1 : Form
    {
        //Variables
        bool jumping = false;
        int jumpSpeed;
        int force = 12;
        int score = 0;
        int highScore = 0;
        int obstacleSpeed = 10;
        Random rand = new Random(); //Genera un numero random entre un maximo y un minimo
        int position;
        bool isGameOver = false;



        public Form1()
        {
            InitializeComponent();

            GameReset(); //el Juego se inicia junto con el constructor
        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            trex.Top += jumpSpeed;

            txtScore.Text = "Score: " + score;

            if(jumping == true && force < 0)
            {
                jumping = false;
            }

            if(jumping == true)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                jumpSpeed = 12;
            }

            //Mido donde cae el trex

            if(trex.Top > 354 && jumping == false)
            {
                force = 12;
                trex.Top = 355;
                jumpSpeed = 0;
            }

            foreach(Control x in this.Controls)
            {
                 if(x is PictureBox && (string)x.Tag == "Obstacle")
                {
                    x.Left -= obstacleSpeed;

                    if (x.Left < -100)
                    {
                        x.Left = this.ClientSize.Width + rand.Next(200, 500) + (x.Width * 15);
                        score++;
                    }

                    if(trex.Bounds.IntersectsWith(x.Bounds))
                    {
                        gameTimer.Stop();
                        trex.Image = Properties.Resources.dead;
                        txtScore.Text += " Presione R para reinciar el juego!";
                        isGameOver = true;

                        if (score > highScore)
                        {
                            highScore = score;

                            txtHighScore.Text = "Highscore: " + highScore;
                            txtHighScore.ForeColor = Color.Maroon;
                        }
                    }
                }
            }

            if(score > 5)
            {
                obstacleSpeed = 15;
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (jumping == true)
            {
                jumping = false;
            }

            if(e.KeyCode == Keys.R && isGameOver == true)
            {
                GameReset();    
            }
        }

        private void GameReset()
        {
            force = 12;
            jumpSpeed = 0;
            jumping = false;
            score = 0;
            obstacleSpeed = 10;
            txtScore.Text = "Score: " + score;
            trex.Image = Properties.Resources.running;
            isGameOver = false;
            trex.Top = 355;

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "Obstacle")
                {
                    position = this.ClientSize.Width + rand.Next(500, 800) + (x.Width * 10); //Guardo el ancho del form y le sumo un numero random entre 500 y 800, para agregar obstaculos mas adelante

                    x.Left = position;
                }
            }

            gameTimer.Start();
        }

    }
}
