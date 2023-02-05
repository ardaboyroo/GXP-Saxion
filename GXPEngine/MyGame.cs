using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using GXPEngine.Core;
using System.Runtime.CompilerServices;
using System.IO; // File IO
using System.Collections.Generic;
using System.Collections;

public class MyGame : Game
{
    public static readonly Vector2 screenSize = new Vector2(1216, 832);
    private bool gameStarted = false;
    private int scene = 0;
    private int score = 0;
    Mouse mouse;
    MyPlayer myPlayer;
    Cannon myCannon;

    private ArrayList enemyList = new ArrayList();
    private int enemyTimer = 0;

    HUD myHUD;

    const int level1Distance = 100;
    const int level2Distance = 300;
    const int level3Distance = 700;

    private int level = 0;

    public MyGame() : base((int)screenSize.x, (int)screenSize.y, false, false)     // Create a window that's 1216x832 and NOT fullscreen, VSync = false
    {
        targetFps = 60;       // Framerate
        Init();
    }

    private void DestroyAll()
    {
        List<GameObject> children = GetChildren();
        foreach (GameObject child in children)
        {
            if (!child.Equals(mouse))
            {
                child.Destroy();
            }
        }
    }

    private void Init()
    {
        AddChild(mouse = new Mouse());  // So Mouse.Update() is called automatically

        // Add last so it displays on top
        AddChild(myHUD = new HUD(scene));
    }

    private void InitializeGame()
    {
        DestroyAll();

        gameStarted = true;

        DrawTiles();            // Add the tiles as children

        myPlayer = new MyPlayer("Assets/circle.png", 1, 1, 600, 400);
        AddChild(myPlayer);

        myCannon = new Cannon();
        AddChild(myCannon);

        enemyList.Clear();
        score = 0;

        // Add last so it displays on top
        AddChild(myHUD = new HUD(scene));
    }

    private void InitializeRestart()
    {
        DestroyAll();

        gameStarted = false;
        MyPlayer.playerIsAlive = true;


        // Add last so it displays on top
        AddChild(myHUD = new HUD(scene));
        myHUD.SetScore(score);
    }

    private void switchClick()
    {
        if (Input.GetMouseButtonUp(0)) { scene = 1; }
    }

    private void EnemySpawner()
    {
        int pos = Utils.Random(0, 7);
        Enemy temp;
        switch (pos)
        {
            case 1:
                temp = new Enemy(600, 0);
                break;
            case 2:
                temp = new Enemy(1200, 0);
                break;
            case 3:
                temp = new Enemy(1200, 400);
                break;
            case 4:
                temp = new Enemy(1200, 800);
                break;
            case 5:
                temp = new Enemy(600, 800);
                break;
            case 6:
                temp = new Enemy(0, 800);
                break;
            case 7:
                temp = new Enemy(0, 400);
                break;
            default:
                temp = new Enemy(0, 0);
                break;
        }
        enemyList.Add(temp);
        AddChild(temp);
    }

    private void MoveMyPlayer()
    {
        //// WASD for debugging purposes
        //int speed = 5;

        //if (Input.GetKey(Key.W)) { myPlayer.Move(0, -speed); }
        //if (Input.GetKey(Key.A)) { myPlayer.Move(-speed, 0); }
        //if (Input.GetKey(Key.S)) { myPlayer.Move(0, speed); }
        //if (Input.GetKey(Key.D)) { myPlayer.Move(speed, 0); }


        if (Input.GetMouseButtonUp(0) && level == 1)
        {
            if (!myPlayer.isMoving)     // Player won't be able to change direction while already moving
            {
                myPlayer.CalculateKnockback(Mouse.x, Mouse.y, level1Distance, true);
                new Bullet(MyPlayer.playerPos, Mouse.x, Mouse.y, 150);
                myCannon.CannonSFX(0.5f, 1.75f);
            }
        }
        else if (Input.GetMouseButtonUp(0) && level == 2)
        {
            if (!myPlayer.isMoving)     // Player won't be able to change direction while already moving
            {
                myPlayer.CalculateKnockback(Mouse.x, Mouse.y, level2Distance, true);
                new Bullet(MyPlayer.playerPos, Mouse.x, Mouse.y, 600);
                myCannon.CannonSFX(1.5f, 1.25f);
            }
        }
        else if (Input.GetMouseButtonUp(0) && level == 3)
        {
            if (!myPlayer.isMoving)     // Player won't be able to change direction while already moving
            {
                myPlayer.CalculateKnockback(Mouse.x, Mouse.y, level3Distance, true);
                new Bullet(MyPlayer.playerPos, Mouse.x, Mouse.y, 1000);
                myCannon.CannonSFX(3);
            }
        }

        level = Mouse.MouseTimer();
    }

    private void CollisionChecker()
    {
        // I use distance instead of HitTest because it is more precise
        // Player hit enemy while not moving
        foreach (Enemy i in enemyList)
        {
            if (i.isAlive)
            {
                // Player hits enemy while not moving
                if (myPlayer.DistanceTo(i) <= 62 && !myPlayer.isMoving)
                {
                    myPlayer.CalculateKnockback(i.x, i.y, 125, false);
                    i.CalculateKnockback(myPlayer.x, myPlayer.y, 20);
                    myPlayer.TakeDamage(25);
                }


                // Player hits enemy while moving
                if (myPlayer.DistanceTo(i) <= 62 && myPlayer.isMoving)
                {
                    score++;
                    i.TakeDamage(100);
                }


                // Bullet hits enemy
                foreach (Bullet bullet in Bullet.bulletList)
                {
                    if (bullet.HitTest(i) && !bullet.hasHit)
                    {
                        i.TakeDamage(25);
                        bullet.hasHit = true;
                    }
                }
            }
        }



    }

    // For every game object, Update is called every frame, by the engine:
    public void Update()
    {
        Console.WriteLine(Utils.Random(0, 8));
        switch (scene)
        {
            case 0:     // Main menu
                switchClick();
                return;
            case 1:     // Game
                if (!gameStarted)
                {
                    InitializeGame();
                }
                break;
            case 2:     // Restart screen
                if (gameStarted)
                {
                    myHUD.SetHighScore(score);
                    InitializeRestart();
                }
                switchClick();
                break;
        }

        if (!MyPlayer.playerIsAlive)
        {
            scene = 2;
        }

        // Only continue the function if game has started
        if (!gameStarted) { return; }

        MoveMyPlayer();
        CollisionChecker();

        if (enemyTimer < 2000)
        {
            enemyTimer += Time.deltaTime;
        }
        else
        {
            enemyTimer = 0;
            EnemySpawner();
        }

        myHUD.SetScore(score);

        foreach (Bullet bullet in Bullet.bulletList)
        {
            // Check if the current bullet is already added
            if (!HasChild(bullet))
            {
                AddChild(bullet);
            }
        }
    }

    static void Main()                          // Main() is the first method that's called when the program is run
    {
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