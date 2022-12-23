using GXPEngine;
using GXPEngine.Core;
using System;
using System.Drawing;
using System.Drawing.Printing;

namespace GXPEngine
{
    public class Player : AnimationSprite
    {
        private Vector2 pivot;
        private Vector2 Velocity;
        private float playerSpeed;

        const int TOTALTIME = 400;          // Time it takes for the player to completely stand still in milliseconds
        const int PERCENTAGE = 75;      // What percentage of the total distance is traveled at a constant speed
        int time = TOTALTIME;

        bool debounce = false;
        int givenDistance;
        float tets = 0.0f;

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

            // Flip angle while staying between 0-360 degrees
            // RAngle means reversed angle
            float RAngle = RadiansToDegrees(angle);
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
                    playerSpeed = (float)givenDistance * (float)Time.deltaTime / (float)TOTALTIME;
                    tets = playerSpeed;
                }
                // Decrease the player speed until it is 0
                if (playerSpeed >= 0 && time < 0)
                {
                    playerSpeed -= tets * (float)(5.0 / 100.0);
                }
                // Reset the time and debounce when the player stands still
                if (playerSpeed < 0 && time < 0)
                {
                    time = TOTALTIME;
                    debounce = false;
                    playerSpeed = 0;    // To make sure that playerSpeed isn't less than 0
                }
            }
            //Console.WriteLine("playerSpeed: {0} \ntets: {1} \ndecrease: {2}", playerSpeed, tets, tets * (float)(5.0/100.0));

            // Apply the directional speed
            x += Velocity.x * playerSpeed;
            y += Velocity.y * playerSpeed;

        }
    }
}
