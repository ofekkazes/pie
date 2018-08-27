using System;
using Microsoft.Xna.Framework;

public class LoseMod : Menu
{
    
    public LoseMod()
        : base()
    {
        Initialize();

    }
    public override void Initialize()
    {
        base.Initialize();
        AddEntries();

    }

    public void AddEntries()
    {
        this.AddEntry("Continue Playing", 0);
        this.AddEntry("Go To Main Menu", 1);
    }

    public override void Update()
    {
        base.Update();

        if (this.confirm)
        {
            switch (this.place)
            {
                case 0: ContinueSelected();
                    break;
                case 1: MainMenuSelected();
                    break;

            }
        }
        if (this.done)
            this.Initialize();
    }
    public override void Draw()
    {
        base.Draw();
        Settings.StartDraw();
        Settings.spriteBatch.DrawString(Settings.titleFont, "YOU LOST!", new Vector2(Settings.gManager.PreferredBackBufferWidth / 2, Settings.gManager.PreferredBackBufferHeight / 2 - 100), Color.Red);
        Settings.EndDraw();
    }

    private void ContinueSelected()
    {
        MenuManager.gameState = GameState.Playing;
        MenuManager.menuState = MenuState.StartGame;
        LevelManager.levels[LevelManager.currentLevelID].Initialize();
        LevelManager.player.Initialize();
        this.done = true;
    }
    private void MainMenuSelected()
    {
        MenuManager.gameState = GameState.MainMenu;
        MenuManager.menuState = MenuState.MainMenu;
        this.done = true;
    }
   
}

