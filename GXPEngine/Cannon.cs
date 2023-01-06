using GXPEngine.Managers;
using System;

namespace GXPEngine
{
    class Cannon : Player
    {
        public Cannon() : base("Assets/Cannons.png", 3, 1)
        {

        }

        public new void Update()
        {
            x = playerPos.x; 
            y = playerPos.y;
        }
    }
}
