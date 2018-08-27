using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Windows.Forms;

public class ChooseLevel : Menu
{
    bool overControl;
    public ChooseLevel()
        : base()
    {
        this.Initialize();
        overControl = false;
    }

    public override void Initialize()
    {
        base.Initialize();
        AddEntries();
    }

    public void AddEntries()
    {
        this.AddEntry("Level 1", 0);
        this.AddEntry("Level 2", 1);
        this.AddEntry("Level 3", 2);
        this.AddEntry("Back", 3);
    }

    public override void Update()
    {
        base.Update();

        if (this.pressed)
        {
            if (place > LevelManager.highestUnlocked && place != this.entries.Count - 1)
            {
                overControl = true;
                MessageBox.Show("כדי להיכנס לשלב הזה, צריך לעבור את השלב הקודם");
                this.done = true;
            }
        }
        
        if (this.confirm && !overControl)
        {
            switch (place)
            {
                case 0: Level1Selected();
                    break;
                case 1: Level2Selected();
                    break;
                case 2: Level3Selected();
                    break;
                case 3: ExitScreenSelected();
                    break;
            }
        }
        overControl = false;
        if (this.done)
            this.Initialize();
    }


    private void Level1Selected()
    {
        LevelManager.currentLevelID = 0;
        MenuManager.gameState = GameState.Loading;
        MenuManager.menuState = MenuState.StartGame;
        this.done = true;
    }
    private void Level2Selected()
    {
        LevelManager.currentLevelID = 1;
        MenuManager.gameState = GameState.Loading;
        MenuManager.menuState = MenuState.StartGame;
        this.done = true;
    }
    private void Level3Selected()
    {
        LevelManager.currentLevelID = 2;
        MenuManager.gameState = GameState.Loading;
        MenuManager.menuState = MenuState.StartGame;
        this.done = true;
    }
    private void ExitScreenSelected()
    {
        MenuManager.menuState = MenuState.MainMenu;
        MenuManager.gameState = GameState.MainMenu;
        this.done = true;
    }

    public override void Draw()
    {
        base.Draw();
    }
}

