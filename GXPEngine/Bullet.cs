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

        public Bullet(Vector2 pos, float X, float Y, int givenDistance, string Sprite = "Assets/circle.png", int columns = 1, int rows = 1) : base(Sprite, columns, rows)
        {
            this.x = pos.x;
            this.y = pos.y;
            Console.WriteLine(pivot);
            scale = 0.25f;

            float angle = Mathf.CalculateAngle(x, y, X, Y);
            Velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(-angle));
            bulletList.Add(this);
        }

        public void Update()
        {
            // Apply the directional speed
            x += Velocity.x * bulletSpeed;
            y += Velocity.y * bulletSpeed;
        }
    }
}
