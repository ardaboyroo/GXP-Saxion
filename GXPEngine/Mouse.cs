
namespace GXPEngine
{
    class Mouse : GameObject
    {
        const int level1Time = 0;       // Starts at 0 until level2Time
        const int level2Time = 1000;     // Starts at level2Time until level3Time
        const int level3Time = 2500;     // Everything above level3Time

        public static new int x;
        public static new int y;

        private static int mouseTimer = 0;

        public static int MouseTimer()
        {
            if (Input.GetMouseButton(0))
            {
                mouseTimer += Time.deltaTime;
            }

            if (Input.GetMouseButtonUp(0))
            {
                mouseTimer = 0;
            }

            if (mouseTimer > level3Time)
            {
                return 3;
            }

            if (mouseTimer > level2Time && mouseTimer < level3Time)
            {
                return 2;
            }

            if (mouseTimer > level1Time && mouseTimer < level2Time)
            {
                return 1;
            }

            return 0;
        }

        public void Update()
        {
            // Set the mouse positions every frame
            x = Input.mouseX;
            y = Input.mouseY;
        }
    }
}
