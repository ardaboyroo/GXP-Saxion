using GXPEngine;
using GXPEngine.Core;
using System;
using System.Drawing;
using System.Drawing.Printing;

namespace GXPEngine
{
    public class Player : AnimationSprite
    {
        const int TOTALTIME = 400;          // Time it takes for the player to completely stand still in milliseconds
        const float PERCENTAGE = 0.50f;          // What percentage of the given distance is used as slow down distance
        int time = TOTALTIME;
        float oldSpeed;

        private Vector2 pivot;
        private Vector2 Velocity;
        private float playerSpeed;
        int givenDistance;
        bool debounce = false;

        /* 
        
        TOTALTIME = 400

        PERCENTAGE = 75%

        givenDistance = 500

        */

        public void CalculatePivot()
        {
            // Calculate center coordinates
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

        public void CalculateDirection(int mouseX, int mouseY)      // Call this when you want to change direction
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

            /* 
             Sources for angle calculation:
             * https://gamedev.stackexchange.com/questions/80277/how-to-find-point-on-a-circle-thats-opposite-another-point
             * https://stackoverflow.com/questions/1311049/how-to-map-atan2-to-degrees-0-360
            */
        }

        public void CalculateSpeed(int givenDistance)
        {
            debounce = true;
            this.givenDistance = givenDistance;
            // If the player is slowing down or standing still reset the time on click
            if (time <= 0) { time = TOTALTIME; }
        }

        public void Update()
        {
            CalculatePivot();       // Calculate the center coordinates each frame

            if (debounce)
            {
                // Calculate the constant player speed for "TOTALTIME" amount of milliseconds
                if (time >= 0)
                {
                    time -= Time.deltaTime;
                    playerSpeed = (float)givenDistance * ((float)Time.deltaTime / (float)TOTALTIME);
                    oldSpeed = playerSpeed;
                }

                // Decrease the player speed until it is 0
                if (playerSpeed >= 0 && time < 0)
                {
                    playerSpeed -= oldSpeed * (5.0f / 100.0f);
                }

                // Reset the time and debounce when the player stands still
                if (playerSpeed < 0 && time < 0)
                {
                    //slowDownTime = TOTALTIME * PERCENTAGE;
                    time = TOTALTIME;
                    debounce = false;
                    playerSpeed = 0;    // To make sure that playerSpeed isn't less than 0
                }
            }
            Console.WriteLine("playerSpeed: {0} \ttime: {1} \tdecrease: {2}", playerSpeed, time, oldSpeed * (5.0f / 100.0f));

            // Apply the directional speed
            x += Velocity.x * playerSpeed;
            y += Velocity.y * playerSpeed;

        }
    }
}
