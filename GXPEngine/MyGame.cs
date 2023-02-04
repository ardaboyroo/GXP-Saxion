using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using GXPEngine.Core;
using System.Runtime.CompilerServices;
using System.IO; // File IO
public class MyGame : Game
{
    public static readonly Vector2 screenSize = new Vector2(1216, 832);
    MyPlayer myPlayer;
    Cannon myCannon;
    Enemy myEnemy;

    const int level1Distance = 100;
    const int level2Distance = 300;
    const int level3Distance = 700;

    private int level = 0;

    public MyGame() : base((int)screenSize.x, (int)screenSize.y, false, false)     // Create a window that's 1200x800 and NOT fullscreen, VSync = false
    {
        targetFps = 60;       // Framerate
        AddChild(new Mouse());

        DrawTiles();

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

        myPlayer = new MyPlayer("Assets/circle.png", 1, 1, 600, 400);
        AddChild(myPlayer);

        myEnemy = new Enemy(0, 0, "Assets/triangle.png");
        AddChild(myEnemy);

        myCannon = new Cannon();
        AddChild(myCannon);
    }

    private void MoveMyPlayer()
    {
        int speed = 5;

        if (Input.GetKey(Key.W)) { myPlayer.Move(0, -speed); }
        if (Input.GetKey(Key.A)) { myPlayer.Move(-speed, 0); }
        if (Input.GetKey(Key.S)) { myPlayer.Move(0, speed); }
        if (Input.GetKey(Key.D)) { myPlayer.Move(speed, 0); }


        if (Input.GetMouseButtonUp(0) && level == 1)
        {
            if (!myPlayer.isMoving)     // Player won't be able to change direction while already moving
            {
                myPlayer.CalculateKnockback(Mouse.x, Mouse.y, level1Distance, true);
                new Bullet(MyPlayer.playerPos, Mouse.x, Mouse.y, 150);
            }
        }
        else if (Input.GetMouseButtonUp(0) && level == 2)
        {
            if (!myPlayer.isMoving)     // Player won't be able to change direction while already moving
            {
                myPlayer.CalculateKnockback(Mouse.x, Mouse.y, level2Distance, true);
                new Bullet(MyPlayer.playerPos, Mouse.x, Mouse.y, 600);
            }
        }
        else if (Input.GetMouseButtonUp(0) && level == 3)
        {
            if (!myPlayer.isMoving)     // Player won't be able to change direction while already moving
            {
                myPlayer.CalculateKnockback(Mouse.x, Mouse.y, level3Distance, true);
                new Bullet(MyPlayer.playerPos, Mouse.x, Mouse.y, 1000);
            }
        }

        level = Mouse.MouseTimer();
    }

    private void CollisionChecker()
    {
        // I use distance instead of hittest because it is more precise
        // Player hit enemy while not moving

        if (myEnemy.isAlive)
        {
            // Player hits enemy while not moving
            if (myPlayer.DistanceTo(myEnemy) <= 62 && !myPlayer.isMoving)
            {
                myPlayer.CalculateKnockback(myEnemy.x, myEnemy.y, 125, false);
                myEnemy.CalculateKnockback(myPlayer.x, myPlayer.y, 20);
                myPlayer.TakeDamage(25);
                Console.WriteLine("enemy hit you");
            }

            // Player hits enemy while moving
            if (myPlayer.DistanceTo(myEnemy) <= 62 && myPlayer.isMoving)
            {
                myEnemy.TakeDamage(100);
                Console.WriteLine("you hit enemy");
            }
        }
    }

    // For every game object, Update is called every frame, by the engine:
    public void Update()
    {
        MoveMyPlayer();
        CollisionChecker();

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
        reader.Close();
        */
        MyGame mygame = new MyGame();                   // Create a "MyGame" and start it
        mygame.Start();
    }

    private void DrawTiles()
    {
        // Borders
        for (int j = 0; j < screenSize.y; j += 64)
        {
            for (int i = 0; i < screenSize.x; i += 64)
            {

                // Outer border tiles

                if (j == 0 || j == screenSize.y - 64 || i == 0 || i == screenSize.x - 64)
                {
                    Sprite border = new Sprite("Assets/metal_plate2.png");
                    border.scale = 2;
                    border.x = i;
                    border.y = j;
                    AddChild(border);
                }

                // Inner tiles
                Sprite tile = new Sprite("Assets/Platform.png");
                tile.x = i;
                tile.y = j;
                tile.alpha = 0.4f;
                AddChild(tile);

            }
        }
    }
}