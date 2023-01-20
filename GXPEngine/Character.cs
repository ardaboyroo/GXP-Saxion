using System;
using System.Collections;
using GXPEngine.Core;

namespace GXPEngine
{
    public class Character : AnimationSprite
    {
        protected Vector2 Direction;
        protected float speed;
        protected int givenDistance;
        public int health = 100;

        public Character(string Sprite, int columns, int rows, int x = 555, int y = 555) : base(Sprite, columns, rows)
        {
            SetOrigin(width / 2, height / 2);       // Set origin to the center
            this.x = x;
            this.y = y;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
        }
    }
}
