using GXPEngine;
using GXPEngine.Core;
using System;
using System.Drawing;
using System.Drawing.Printing;

namespace GXPEngine
{
    public class Player : AnimationSprite
    {
        // You can tweak these
        const int TOTALTIME = 200;          // Time it takes for the player to start slowing down in milliseconds
        const float PERCENTAGE = 2.0f;      // What percentage of the given distance is used as slow down distance

        // Don't change these
        private Vector2 pivot;
        private Vector2 Velocity;
        private int givenDistance;
        private int time = TOTALTIME;
        private float slowDownTime = (float)TOTALTIME * PERCENTAGE;
        private float playerSpeed;
        private float oldSpeed;
        private bool debounce = false;
        public bool isMoving = false;

        public void CalculatePivot()
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

        public float RadiansToDegrees(float angle)
        {
            // Turn radians into degrees
            angle *= 180 / Mathf.PI;

            // Map degrees from -180,180 to 0,360
            if (angle > 90) { angle = 450 - angle; }
            else { angle = 90 - angle; }

            // Rotate angle 90 degrees clockwise
            angle -= 90;
            if (angle < 0) { angle = 360 + angle; }

            return angle;
        }

        public float DegreesToRadians(float angle)
        {
            return angle * Mathf.PI / 180.0f - Mathf.PI / 2;
        }

        public void CalculateVelocity(int mouseX, int mouseY, int givenDistance)      // Call this when you want to change direction and speed
        {
            Vector2 C = new Vector2(0, 0);

            float deltaX = pivot.x - mouseX;
            float deltaY = pivot.y - mouseY;
            float dist = Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);     // Pythagoras theorem
            C.x = deltaX / dist;
            C.y = deltaY / dist;

            // Calculate angle
            float angle = Mathf.Atan2(C.x, C.y);

            // RAngle means reversed angle
            float RAngle = RadiansToDegrees(angle);

            // Flip angle while staying between 0-360 degrees
            if (RAngle + 180 < 360) { RAngle += 180; }
            else { RAngle -= 180; }


            // if mouse is exactly on the same pixel as the center make the   RAngle = 0   instead of   RAngle = NaN
            if (float.NaN.Equals(RAngle)) { RAngle = 0; }
            // Calculate velocity from angle
            Velocity = new Vector2(Mathf.Cos(DegreesToRadians(RAngle)), Mathf.Sin(DegreesToRadians(RAngle)));


            // This is for the constant speed and speed decrease calculation
            debounce = true;
            this.givenDistance = givenDistance;
                // If the player is slowing down or standing still reset the time on click
            if (time <= 0) { time = TOTALTIME; }
            slowDownTime = (float)TOTALTIME * PERCENTAGE;

            /* 
             Sources for angle calculation:
             * https://gamedev.stackexchange.com/questions/80277/how-to-find-point-on-a-circle-thats-opposite-another-point
             * https://stackoverflow.com/questions/1311049/how-to-map-atan2-to-degrees-0-360
            */
        }

        public void Update()
        {
            CalculatePivot();       // Calculate the center coordinates each frame


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
                if (playerSpeed > 0 && time < 0 && slowDownTime >= 0)
                {
                    isMoving = false;
                    slowDownTime -= Time.deltaTime;
                    float decrease = oldSpeed / ((float)TOTALTIME * PERCENTAGE) * Time.deltaTime;
                    playerSpeed -= decrease;
                }

                // Reset the time and debounce when the player stands still
                // slowDownTime < 1 because floats aren't exact and can leave very small numbers
                if (playerSpeed <= 0 && time < 0 && slowDownTime < 1)
                {
                    debounce = false;
                    playerSpeed = 0;    // To make sure that playerSpeed doesn't stay less than 0
                    time = TOTALTIME;
                    slowDownTime = (float)TOTALTIME * PERCENTAGE;
                }
            }

            //Console.WriteLine("playerSpeed: {0} \ttime: {1} \tslowDownTime: {2} \tdecrease: {3} \tdeltaTime: {4}", playerSpeed, time, slowDownTime, (oldSpeed * PERCENTAGE) / ((float)TOTALTIME * PERCENTAGE) * Time.deltaTime, Time.deltaTime);

            // Apply the directional speed
            x += Velocity.x * playerSpeed;
            y += Velocity.y * playerSpeed;

        }
    }
}
