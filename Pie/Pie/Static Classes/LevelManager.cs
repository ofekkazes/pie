using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

public static class LevelManager
{
    public static Hero player;
    
    private static Level levelLoader;

    public static List<Level> levels;
    public static int currentLevelID = 0;

    public static int highestUnlocked = 0;

    public static void LoadContent()
    {
        player = new Hero(Settings.content.Load<Model>("Models/cube"));

        levels = new List<Level>();

        levelLoader = new Level1(1, Vector3.Zero, new Vector3(9, 0, 0), Settings.content.Load<Model>("Models/Ground-Beta"), player);
        
        levels.Add(levelLoader);

        levelLoader = new Level2(2, Vector3.Zero, new Vector3(-80, 0, -0.8f), Settings.content.Load<Model>("Models/level2"), player);

        levelLoader.Initialize();

        levels.Add(levelLoader);

        levelLoader = new Level3(3, Vector3.Zero, new Vector3(75.6f, 0, 0), Settings.content.Load<Model>("Models/level3"), player);

        levelLoader.Initialize();

        levels.Add(levelLoader);

        levelLoader = null;
    }
    public static void Update()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            if (i == currentLevelID)
            {
                levels[i].Update();
                if (levels[i].IsDone)
                {
                    levels[i].UnloadContent();
                    currentLevelID++;
                    if (currentLevelID > highestUnlocked)
                        highestUnlocked = currentLevelID;
                    if (currentLevelID > levels.Count - 1) MenuManager.gameState = GameState.Won;
                }
            }
        }
    }
    public static void SelfUpdate()
    {
        if (levels != null)
        {
            for (int i = 0; i < levels.Count; i++)
            {
                if (i == currentLevelID)
                    levels[i].Initialize();
            }
        }
    }
    public static void Draw3D()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            if (i == currentLevelID)
                levels[i].Draw3D();
        }
    }
    public static void Draw2D()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            if (i == currentLevelID)
                levels[i].Draw2D();
        }
    }
}

