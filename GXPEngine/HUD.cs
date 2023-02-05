using System;
using System.Drawing;
using GXPEngine;
using GXPEngine.Core;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace GXPEngine
{
    public class HUD : GameObject
    {
        EasyDraw scoreText;
        EasyDraw highScoreText;
        Font font;
        private string file = "Assets/HighScore.txt";
        StreamWriter writer;
        StreamReader reader;

        public HUD(int CASE)
        {

            font = Utils.LoadFont("Assets/tf2build.ttf", 40);
            highScoreText = new EasyDraw(500, 50);
            highScoreText.TextFont(font);
            highScoreText.TextAlign(CenterMode.Center, CenterMode.Center);
            highScoreText.Fill(255, 50, 50);
            highScoreText.Text(string.Format("Highest Kills: {0}", GetHighScore()), true);
            highScoreText.SetOrigin(highScoreText.width / 2, highScoreText.height / 2);
            highScoreText.SetXY(MyGame.screenSize.x / 2, MyGame.screenSize.y / 2);

            scoreText = new EasyDraw(250, 50);
            scoreText.TextFont(font);
            scoreText.TextAlign(CenterMode.Center, CenterMode.Center);
            scoreText.Fill(70, 70, 70);
            scoreText.Text("Kills: 0", true);
            scoreText.SetOrigin(125, 25);
            scoreText.SetXY(MyGame.screenSize.x / 2, 35);

            switch (CASE)
            {
                case 0:
                    AddChild(highScoreText);
                    break;

                case 1:
                    AddChild(scoreText);
                    break;
                case 2:
                    AddChild(highScoreText);
                    AddChild(scoreText);
                    break;
            }
        }

        public void SetScore(int score)
        {
            if (scoreText != null)
            {
                scoreText.Text(String.Format("Kills: {0}", score), true);
            }
        }

        public void SetHighScore(int score)
        {

            // Make sure file exists and has something in it
            if (!File.Exists(file) || new FileInfo(file).Length == 0)
            {
                writer = new StreamWriter(file);
                writer.WriteLine(0.ToString());
                writer.Close();
            }

            // Read the number from file and check if it less than given score
            // If less then change to new score
            reader = new StreamReader(file);
            int readNum = int.Parse(reader.ReadLine());
            if (readNum < score)
            {
                reader.Close();
                Console.WriteLine("Updated score");
                Console.WriteLine("new score: {0}", score);
                writer = new StreamWriter(file);
                writer.WriteLine(score.ToString());
                writer.Close();
            }

            reader = new StreamReader(file);
            string output = reader.ReadLine();
            Console.WriteLine("output: {0}", output.ToString());
            reader.Close();
        }
        
        public int GetHighScore()
        {
            // Make sure file exists and has something in it
            if (!File.Exists(file) || new FileInfo(file).Length == 0)
            {
                writer = new StreamWriter(file);
                writer.WriteLine(0.ToString());
                writer.Close();
            }

            reader = new StreamReader(file);
            int score = int.Parse(reader.ReadLine());
            reader.Close();

            return score;
        }
    }
}
