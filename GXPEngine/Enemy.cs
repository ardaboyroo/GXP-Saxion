using System;
using GXPEngine.Core;

namespace GXPEngine
{
    class Enemy : Player
    {
        private const float SPEED = 0.2f;       // Amount of speed for each millisecond
        
        public Enemy(int x, int y, string Sprite = "Assets/triangle.png", int columns = 1, int rows = 1) : base(Sprite, columns, rows, x, y)
        {
            
        }

        private void CalculateDirection()
        {
            rotation = Mathf.CalculateAngleDeg(x, y, playerPos.x, playerPos.y);
            float angle = Mathf.CalculateAngleRad(x, y, playerPos.x, playerPos.y);
            Direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(-angle));
        }

        private void CalculateSpeed()
        {
            speed = SPEED * Time.deltaTime;
        }

        public new void Update()
        {
            CalculateDirection();
            CalculateSpeed();
            x += Direction.x * speed;
            y += Direction.y * speed;
        }
    }
}
