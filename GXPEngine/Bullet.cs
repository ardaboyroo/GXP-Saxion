﻿using GXPEngine.Core;
using System;
using System.Collections;
using System.Threading;

namespace GXPEngine
{
    class Bullet : Player
    {
        // You can tweak these
        private const int BULLETTIME = 500;     // lifetime of the bullet in milliseconds

        // Don't change these
        public static ArrayList bulletList = new ArrayList();
        public int lifeTime = BULLETTIME;        // time in milliseconds
        private float bulletSpeed;
        private int bulletIndex;

        public Bullet(Vector2 pos, float x, float y, int givenDistance, string Sprite = "Assets/circle.png", int columns = 1, int rows = 1) : base(Sprite, columns, rows)
        {
            scale = 0.25f;
            this.x = pos.x - width/2;
            this.y = pos.y - height/2;
            this.givenDistance = givenDistance;
            bulletIndex = bulletList.Count;

            float angle = Mathf.CalculateAngleRad(this.x, this.y, x, y);
            Direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(-angle));
            bulletList.Add(this);
        }

        private void CalculateSpeed()
        {
            if (lifeTime >= 0)
            {
                lifeTime -= Time.deltaTime;
                bulletSpeed = (float)givenDistance * ((float)Time.deltaTime / (float)BULLETTIME);
            }
            else
            {
                bulletList.RemoveAt(0);     // Remove the first bullet in the list
                LateDestroy();              // Detroy the bullet from GameObject
            }
        }

        public new void Update()
        {
            CalculateSpeed();

            // Apply the directional speed
            x += Direction.x * bulletSpeed;
            y += Direction.y * bulletSpeed;
        }

    }
}
