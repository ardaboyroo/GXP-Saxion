using GXPEngine.Core;
using GXPEngine.Managers;
using System;
using System.Reflection.Emit;

namespace GXPEngine
{
    class Cannon : Character
    {
        public Cannon() : base("Assets/Cannons.png", 3, 1)
        {
            SetOrigin(width / 2, height * 0.75f);
        }

        private void UpdateRotation()
        {
            rotation = Mathf.CalculateAngleDeg(x, y, Mouse.x, Mouse.y);
        }

        private void UpdateSprite()
        {
            SetFrame(Mouse.MouseTimer() - 1);
        }

        public new void Update()
        {
            UpdateRotation();
            UpdateSprite();

            x = MyPlayer.playerPos.x;
            y = MyPlayer.playerPos.y;
        }
    }
}
