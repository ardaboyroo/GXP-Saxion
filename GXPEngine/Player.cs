using System;
using System.Collections;
using GXPEngine.Core;

namespace GXPEngine
{
    public class Player : AnimationSprite
    {
        // You can tweak these
        private const int TOTALTIME = 250;          // Time it takes for the player to start slowing down in milliseconds
        private const float PERCENTAGE = 1.0f;      // What percentage of the given distance is used as slow down distance

        // Don't change these
        public static Vector2 playerPos = new Vector2();
        public bool isMoving = false;

        protected Vector2 Direction;
        protected float speed;

        protected int givenDistance;
        private int time = TOTALTIME;
        private float slowDownTime = (float)TOTALTIME * PERCENTAGE;
        private float oldSpeed;
        private bool debounce = false;

        public Player(string Sprite, int columns, int rows, int x = 0, int y = 0) : base(Sprite, columns, rows)
        {
            SetOrigin(width / 2, height / 2);       // Set origin to the center
            this.x = x;
            this.y = y;
            playerPos.x = this.x;
            playerPos.y = this.y;
        }

        public void Move(int stepX, int stepY)
        {
            // WASD controls for debugging
            x += stepX;
            y += stepY;
        }

        public void CalculateDirection(float x, float y, int givenDistance)      // Call this when you want to change direction and speed
        {
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
                // slowDownTime < 1 because floats aren't exact and can leave very small numbers
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
            if (x < 0) { x = MyGame.screenSize.x; }
            if (x > MyGame.screenSize.x) { x = 0; }
            if (y < 0) { y = MyGame.screenSize.y; }
            if (y > MyGame.screenSize.y) { y = 0; }
        }

        public void Update()
        {
            CalculateSpeed();
            Borders();
            //Console.WriteLine("playerSpeed: {0} \ttime: {1} \tslowDownTime: {2} \tdecrease: {3} \tdeltaTime: {4}", playerSpeed, time, slowDownTime, (oldSpeed * PERCENTAGE) / ((float)TOTALTIME * PERCENTAGE) * Time.deltaTime, Time.deltaTime);

            // Apply the directional speed
            x += Direction.x * speed;
            y += Direction.y * speed;
            playerPos.x = x;
            playerPos.y = y;
        }
    }
}
