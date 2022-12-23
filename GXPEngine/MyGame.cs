using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using GXPEngine.Core;
using System.Runtime.CompilerServices;
using System.IO; // File IO
public class MyGame : Game
{
    Player myPlayer;
    int mouseX;
    int mouseY;

    Sprite lvl1;
    Sprite lvl2;
    Sprite lvl3;
    const int level1Distance = 150;
    const int level2Distance = 300;
    const int level3Distance = 500;
    
    public MyGame() : base(1200, 800, false)     // Create a window that's 800x600 and NOT fullscreen, VSync = false
    {
        //// Draw some things on a canvas:
        //EasyDraw canvas = new EasyDraw(800, 600);
        //canvas.Clear(Color.MediumPurple);
        //canvas.Fill(Color.Yellow);
        //canvas.Ellipse(width / 2, height / 2, 200, 200);
        //canvas.Fill(50);
        //canvas.TextSize(32);
        //canvas.TextAlign(CenterMode.Center, CenterMode.Center);
        //canvas.Text("Welcome!", width / 2, height / 2);

        // Add the canvas to the engine to display it:
        //AddChild(canvas);
        Console.WriteLine("MyGame initialized");
        myPlayer = new Player("circle.png", 1, 1);
        lvl1 = new Sprite("circle.png");
        lvl2 = new Sprite("circle.png");
        lvl3 = new Sprite("circle.png");
        AddChild(lvl1);
        AddChild(lvl2);
        AddChild(lvl3);

        AddChild(myPlayer);
    }

    void MoveMyPlayer()
    {
        if (Input.GetKey(Key.W))
        {
            myPlayer.Move(0, -5);
        }
        if (Input.GetKey(Key.A))
        {
            myPlayer.Move(-5, 0);
        }
        if (Input.GetKey(Key.S))
        {
            myPlayer.Move(0, 5);
        }
        if (Input.GetKey(Key.D))
        {
            myPlayer.Move(5, 0);
        }
        if (Input.GetMouseButtonDown(0))
        {
            myPlayer.CalculateDirection(mouseX, mouseY);
            myPlayer.CalculateSpeed(150);
        }
    }


    // For every game object, Update is called every frame, by the engine:
    void Update()
    {
        // Set the mouse positions every frame
        mouseX = Input.mouseX;
        mouseY = Input.mouseY;

        lvl1.x = 150;
        lvl2.x = 300;
        lvl3.x = 500;

        lvl1.y = lvl1.height;
        lvl2.y = lvl1.height * 2;
        lvl3.y = lvl1.height * 3;


        MoveMyPlayer();
        Console.WriteLine(currentFps);
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
        Console.WriteLine("� read this number: "+readNum);
        Console.ReadKey();
        */

        new MyGame().Start();                   // Create a "MyGame" and start it
    }
}