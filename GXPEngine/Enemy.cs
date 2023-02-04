using System;
using System.Threading;
using GXPEngine.Core;

namespace GXPEngine
{
    class Enemy : Character
    {
        private const float SPEED = 0.2f;       // Amount of speed for each millisecond
        private const int TOTALTIME = 100;          // Time it takes for the player to start slowing down in milliseconds
        private const float PERCENTAGE = 1.0f;      // What percentage of the given distance is used as slow down distance

        private bool debounce = false;
        private int time = TOTALTIME;
        private float slowDownTime = (float)TOTALTIME * PERCENTAGE;
        private float oldSpeed;
        public bool isAlive = true;
        private Sound explosionSFX;

        public Enemy(int x, int y, string Sprite = "Assets/triangle.png", int columns = 1, int rows = 1) : base(Sprite, columns, rows, x, y)
        {
            explosionSFX = new Sound("Assets/Explosion.wav");
        }


        private void CalculateDirection()
        {
            // Rotate the sprite to the player direction
            rotation = Mathf.CalculateAngleDeg(x, y, MyPlayer.playerPos.x, MyPlayer.playerPos.y);

            if (!debounce)
            {
                // Calculate the angle in which the Enemy sprite will move towards
                float angle = Mathf.CalculateAngleRad(x, y, MyPlayer.playerPos.x, MyPlayer.playerPos.y);
                Direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(-angle));
            }
        }


        public void CalculateKnockback(float x, float y, int givenDistance)      // Call this when you want to change direction and speed
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
            // speed = SPEED * Time.deltaTime;


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
            else
            {
                speed = SPEED * Time.deltaTime;
            }
        }

        private void CheckHealth()
        {
            if (health <= 0)
            {
                isAlive = false;
                explosionSFX.Play();
                LateDestroy();
            }
        }

        public void Update()
        {
            CalculateDirection();
            CalculateSpeed();
            CheckHealth();
            x += Direction.x * speed;
            y += Direction.y * speed;
        }
    }
}
