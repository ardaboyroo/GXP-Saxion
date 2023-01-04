using System;

namespace GXPEngine
{
    class Enemy : Player
    {


        public Enemy(int x, int y, string Sprite = "Assets/triangle.png", int columns = 1, int rows = 1) : base(Sprite, columns, rows, x, y)
        {

        }

        private void CalculateDirection()
        {
            rotation = Mathf.CalculateAngleDeg(x, y, playerPos.x, playerPos.y);
        }

        private void CalculateSpeed()
        {

        }

        public new void Update()
        {
            CalculateDirection();
            CalculateSpeed();
        }
    }
}
