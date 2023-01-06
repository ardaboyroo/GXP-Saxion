using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using GXPEngine.Core;
using System.Runtime.CompilerServices;
using System.IO; // File IO
public class MyGame : Game
{
    public static Vector2 screenSize = new Vector2(1200, 800);
    Player myPlayer;
    Cannon myCannon;
    int mouseX;
    int mouseY;

    Sprite lvl1;
    Sprite lvl2;
    Sprite lvl3;
    int mouseTimer = 0;
    const int level1Distance = 100;
    const int level2Distance = 300;
    const int level3Distance = 700;
    const int level1Time = 0;       // Starts at 0 until level2Time
    const int level2Time = 300;     // Starts at level2Time until level3Time
    const int level3Time = 750;     // Everything above level3Time

    public MyGame() : base((int)screenSize.x, (int)screenSize.y, false, false)     // Create a window that's 800x600 and NOT fullscreen, VSync = false
    {
        targetFps = 60;       // Framerate
        /*
        // Draw some things on a canvas:
        EasyDraw canvas = new EasyDraw(800, 600);
        canvas.Clear(Color.MediumPurple);
        canvas.Fill(Color.Yellow);
        canvas.Ellipse(width / 2, height / 2, 200, 200);
        canvas.Fill(50);
        canvas.TextSize(32);
        canvas.TextAlign(CenterMode.Center, CenterMode.Center);
        canvas.Text("Welcome!", width / 2, height / 2);

        // Add the canvas to the engine to display it:
        AddChild(canvas); 
        */
        myPlayer = new Player("Assets/circle.png", 1, 1);
        myCannon = new Cannon();
        AddChild(myPlayer);
        AddChild(myCannon);

        lvl1 = new Sprite("Assets/circle.png");
        lvl2 = new Sprite("Assets/circle.png");
        lvl3 = new Sprite("Assets/triangle.png");
        Enemy myEnemy = new Enemy(500, 300, "Assets/triangle.png");
        AddChild(myEnemy);
        AddChild(lvl1);
        AddChild(lvl2);
        AddChild(lvl3);
    }

    private int MouseTimer()
    {
        if (Input.GetMouseButton(0))
        {
            mouseTimer += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0) && mouseTimer > level1Time && mouseTimer < level2Time)
        {
            mouseTimer = 0;
            return 1;
        }

        if (Input.GetMouseButtonUp(0) && mouseTimer > level2Time && mouseTimer < level3Time)
        {
            mouseTimer = 0;
            return 2;
        }

        if (Input.GetMouseButtonUp(0) && mouseTimer > level3Time)
        {
            mouseTimer = 0;
            return 3;
        }

        return 0;
    }

    void MoveMyPlayer(int level)
    {
        int speed = 5;

        if (Input.GetKey(Key.W)) { myPlayer.Move(0, -speed); }
        if (Input.GetKey(Key.A)) { myPlayer.Move(-speed, 0); }
        if (Input.GetKey(Key.S)) {  myPlayer.Move(0, speed); }
        if (Input.GetKey(Key.D))
        {
            myPlayer.Move(speed, 0);
        }

        if (level == 1)
        {
            if (!myPlayer.isMoving)     // This is so that the player won't be able to change direction while already moving
            {
                myPlayer.CalculateDirection(mouseX, mouseY, level1Distance);
                new Bullet(new Vector2(myPlayer.x, myPlayer.y), mouseX, mouseY, 750);
            }
        }
        else if (level == 2)
        {
            if (!myPlayer.isMoving)     // This is so that the player won't be able to change direction while already moving
            {
                myPlayer.CalculateDirection(mouseX, mouseY, level2Distance);
                new Bullet(new Vector2(myPlayer.x, myPlayer.y), mouseX, mouseY, 750);
            }
        }
        else if (level == 3)
        {
            if (!myPlayer.isMoving)     // This is so that the player won't be able to change direction while already moving
            {
                myPlayer.CalculateDirection(mouseX, mouseY, level3Distance);
                new Bullet(new Vector2(myPlayer.x, myPlayer.y), mouseX, mouseY, 750);
            }
        }
    }


    // For every game object, Update is called every frame, by the engine:
    void Update()
    {
        // Set the mouse positions every frame
        mouseX = Input.mouseX;
        mouseY = Input.mouseY;

        lvl1.x = level1Distance;
        lvl2.x = level2Distance;
        lvl3.x = level3Distance;

        lvl1.y = lvl1.height;
        lvl2.y = lvl1.height * 2;
        lvl3.y = lvl1.height * 3;

        MoveMyPlayer(MouseTimer());


        foreach (Bullet i in Bullet.bulletList)
        {
            // Check if the current bullet is already added
            if (!HasChild(i))
            {
                AddChild(i);
            }
        }
    }

    static void Main()                          // Main() is the first method that's called when the program is run
    {
        /*
        StreamWriter writer = new StreamWriter("myFile.txt");

        
        Console.WriteLine("Please enter a number");
        string input = Console.ReadLine();
        int number = int.Parse(input); // this may give an exception!

        writer.WriteLine((number * 2).ToString());
        writer.Close();
        
        StreamReader reader = new StreamReader("myFile.txt");
        string input2 = reader.ReadLine();
        int readNum = int.Parse(input2); // may give exception
        Console.WriteLine("I read this number: "+readNum);
        Console.ReadKey();
        */
        new MyGame().Start();                   // Create a "MyGame" and start it
    }
}