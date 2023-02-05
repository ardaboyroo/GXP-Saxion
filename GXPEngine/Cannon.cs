using GXPEngine.Core;
using GXPEngine.Managers;
using System;
using System.Reflection.Emit;

namespace GXPEngine
{
    class Cannon : Character
    {
        private Sound cannonSFX;

        public Cannon() : base("Assets/Cannons.png", 3, 1)
        {
            SetOrigin(width / 2, height * 0.75f);
            cannonSFX = new Sound("Assets/Cannon.wav");
        }

        public void CannonSFX(float Volume, float Frequency = 1)
        {
            SoundChannel temp = cannonSFX.Play();
            temp.Volume = Volume;
            temp.Frequency *= Frequency;
        }

        private void CheckHealth()
        {
            if (!playerIsAlive)
            {
                LateDestroy();
            }
        }

        private void UpdateRotation()
        {
            rotation = Mathf.CalculateAngleDeg(x, y, Mouse.x, Mouse.y);
        }

        private void UpdateSprite()
        {
            SetFrame(Mouse.MouseTimer() - 1);
        }

        public void Update()
        {
            CheckHealth();
            UpdateRotation();
            UpdateSprite();

            x = MyPlayer.playerPos.x;
            y = MyPlayer.playerPos.y;
        }
    }
}
