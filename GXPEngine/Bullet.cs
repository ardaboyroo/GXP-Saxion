using GXPEngine.Core;
using System;
using System.Collections;
using System.Threading;

namespace GXPEngine
{
    class Bullet : Player
    {
        public static ArrayList bulletList = new ArrayList();
        float bulletSpeed = 10;

        public Bullet(float x, float y, string Sprite = "Assets/circle.png", int columns = 1, int rows = 1) : base(Sprite, columns, rows)
        {
            this.x = x;
            this.y = y;
            scale = 0.25f;

            bulletList.Add(this);
        }

        public void Update()
        {

        }
    }
}
