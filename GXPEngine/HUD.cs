using System;
using System.Drawing;
using GXPEngine;
using GXPEngine.Core;

namespace GXPEngine
{
    public class HUD : GameObject
    {
        EasyDraw scoreText;
        Font font;

        public HUD()
        {
            font = Utils.LoadFont("Assets/tf2build.ttf", 40);
            scoreText = new EasyDraw(250, 50);
            scoreText.TextFont(font);
            scoreText.TextAlign(CenterMode.Center, CenterMode.Center);
            scoreText.Fill(50, 50, 50);
            scoreText.Text("Kills: 0", true);
            scoreText.SetOrigin(125, 25);
            scoreText.SetXY(MyGame.screenSize.x/2, 35);
            AddChild(scoreText);
        }

        public void SetScore(int score)
        {
            scoreText.Text(String.Format("Kills: {0}", score), true);
        }
    }
}
