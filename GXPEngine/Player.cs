using System;
using System.Collections;
using GXPEngine.Core;

namespace GXPEngine
{
    public class Player : AnimationSprite
    {
        // You can tweak these
        const int TOTALTIME = 250;          // Time it takes for the player to start slowing down in milliseconds
        const float PERCENTAGE = 1.0f;      // What percentage of the given distance is used as slow down distance

        // Don't change these
        public Vector2 pivot;
        public bool isMoving = false;

        protected Vector2 Velocity;
        protected float playerSpeed;

        private int givenDistance;
        private int time = TOTALTIME;
        private float slowDownTime = (float)TOTALTIME * PERCENTAGE;
        private float oldSpeed;
        private bool debounce = false;

        private void CalculatePivot()
        {
            // Calculate and set center coordinates
            pivot = new Vector2(x + width / 2, y + height / 2);
        }

        public Player(string Sprite, int columns, int rows, int X = 0, int Y = 0) : base(Sprite, columns, rows)
        {
            x = X;
            y = Y;
            CalculatePivot();       // Calculate the center points before the beginning of the first loop
        }

        public void Move(int stepX, int stepY)
        {
            x += stepX;
            y += stepY;
        }

        public void CalculateVelocity(float x, float y, int givenDistance)      // Call this when you want to change direction and speed
        {
            float angle = Mathf.CalculateAngle(pivot.x, pivot.y, x, y);
            angle = Mathf.ReverseAngle(angle);

            // Calculate velocity from angle
            Velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(-angle));



            // This is for the constant speed and speed decrease calculation
            debounce = true;
            this.givenDistance = givenDistance;
            // If the player is slowing down or standing still reset the time on click
            if (time <= 0) { time = TOTALTIME; }
            slowDownTime = (float)TOTALTIME * PERCENTAGE;
        }

        private void CalculateSpeed()
        {
            // This is for calculating the constant speed and decreasing speed each frame with deltaTime
            if (debounce)
            {
                // Calculate the constant player speed for "TOTALTIME" amount of milliseconds
                if (time >= 0)
                {
                    isMoving = true;
                    time -= Time.deltaTime;
                    playerSpeed = (float)givenDistance * ((float)Time.deltaTime / (float)TOTALTIME);
                    oldSpeed = playerSpeed;     // Use the last calculated speed for slowDown
                }

                // Decrease the player speed until it is 0
                else if (playerSpeed > 0 && time < 0 && slowDownTime >= 0)
                {
                    isMoving = false;
                    slowDownTime -= Time.deltaTime;
                    float decrease = oldSpeed / ((float)TOTALTIME * PERCENTAGE) * Time.deltaTime;
                    playerSpeed -= decrease;
                }

                // Reset the time and debounce when the player stands still
                // slowDownTime < 1 because floats aren't exact and can leave very small numbers
                else if (playerSpeed <= 0 && time < 0 && slowDownTime < 1)
                {
                    debounce = false;
                    playerSpeed = 0;    // To make sure that playerSpeed doesn't stay less than 0
                    time = TOTALTIME;
                    slowDownTime = (float)TOTALTIME * PERCENTAGE;
                }
            }
        }

        public void Update()
        {
            CalculatePivot();       // Calculate the center coordinates each frame
            CalculateSpeed();
            //Console.WriteLine("playerSpeed: {0} \ttime: {1} \tslowDownTime: {2} \tdecrease: {3} \tdeltaTime: {4}", playerSpeed, time, slowDownTime, (oldSpeed * PERCENTAGE) / ((float)TOTALTIME * PERCENTAGE) * Time.deltaTime, Time.deltaTime);

            // Apply the directional speed
            x += Velocity.x * playerSpeed;
            y += Velocity.y * playerSpeed;

        }
    }
}
