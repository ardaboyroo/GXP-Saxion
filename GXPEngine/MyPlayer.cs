using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    class MyPlayer : Character
    {
        // You can tweak these
        private const int TOTALTIME = 250;          // Time it takes for the player to start slowing down in milliseconds
        private const float PERCENTAGE = 1.0f;      // What percentage of the given distance is used as slow down distance

        // Don't change these
        public static Vector2 playerPos = new Vector2();
        public bool isMoving = false;

        private int time = TOTALTIME;
        private float slowDownTime = (float)TOTALTIME * PERCENTAGE;
        private float oldSpeed;
        private bool debounce = false;


        public MyPlayer(string Sprite, int columns, int rows, int x = 0, int y = 0) : base(Sprite, columns, rows, x, y)
        {
            playerPos.x = this.x;
            playerPos.y = this.y;
        }

        public void Move(int stepX, int stepY)
        {
            // WASD controls for debugging
            x += stepX;
            y += stepY;
        }

        public void CalculateKnockback(float x, float y, int givenDistance, bool isDeadly)      // Call this when you want to change direction and speed
        {
            isMoving = isDeadly;

            float angle = Mathf.CalculateAngleRad(this.x, this.y, x, y);
            angle = Mathf.ReverseAngleRad(angle);

            // Calculate velocity from angle
            Direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(-angle));


            // This is for the constant speed and speed decrease calculation
            debounce = true;
            this.givenDistance = givenDistance;

            // If the player is slowing down or standing still reset the time on click
            if (time <= 0) { time = TOTALTIME; }
            slowDownTime = (float)TOTALTIME * PERCENTAGE;
        }

        private void CheckHealth()
        {
            if (health <= 0)
            {
                playerIsAlive = false;
                LateDestroy();
            }
        }

        private void CalculateSpeed()
        {
            // This is for calculating the constant speed and decreasing speed each frame with deltaTime
            if (debounce)
            {
                // Calculate the constant player speed for "TOTALTIME" amount of milliseconds
                if (time >= 0)
                {
                    time -= Time.deltaTime;
                    speed = (float)givenDistance * ((float)Time.deltaTime / (float)TOTALTIME);
                    oldSpeed = speed;     // Use the last calculated speed for slowDown
                }

                // Decrease the player speed until it is 0
                else if (speed > 0 && time < 0 && slowDownTime >= 0)
                {
                    isMoving = false;
                    slowDownTime -= Time.deltaTime;
                    float decrease = oldSpeed / ((float)TOTALTIME * PERCENTAGE) * Time.deltaTime;
                    speed -= decrease;
                }

                // Reset the time and debounce when the player stands still
                // slowDownTime < 1 instead because floats aren't exact and can leave very small numbers
                else if (speed <= 0 && time < 0 && slowDownTime < 1)
                {
                    debounce = false;
                    speed = 0;    // To make sure that playerSpeed doesn't stay less than 0
                    time = TOTALTIME;
                    slowDownTime = (float)TOTALTIME * PERCENTAGE;
                }
            }
        }

        private void Borders()
        {
            if (x < 96) { if (Direction.x < 0) { Direction.x *= -1; } }
            if (x > MyGame.screenSize.x - 96) { if (Direction.x > 0) { Direction.x *= -1; } }
            if (y < 96) { if (Direction.y < 0) { Direction.y *= -1; } }
            if (y > MyGame.screenSize.y - 96) { if (Direction.y > 0) { Direction.y *= -1; } }
        }

        public void Update()
        {
            CheckHealth();
            CalculateSpeed();
            Borders();

            // Apply the directional speed
            x += Direction.x * speed;
            y += Direction.y * speed;
            playerPos.x = x;
            playerPos.y = y;
        }
    }
}
