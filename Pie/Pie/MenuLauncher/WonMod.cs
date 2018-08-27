using System;
using Microsoft.Xna.Framework;

public class WonMod : Menu
{

    public WonMod()
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
        this.AddEntry("Start Again?", 0);
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
        Settings.spriteBatch.DrawString(Settings.titleFont, "YOU WON!", new Vector2(Settings.gManager.PreferredBackBufferWidth / 2, Settings.gManager.PreferredBackBufferHeight / 2 - 100), Color.White);
        Settings.EndDraw();
    }

    private void ContinueSelected()
    {
        LevelManager.currentLevelID = 0;
        MenuManager.gameState = GameState.Loading;
        MenuManager.menuState = MenuState.StartGame;
        this.done = true;
    }
    private void MainMenuSelected()
    {
        MenuManager.gameState = GameState.MainMenu;
        MenuManager.menuState = MenuState.MainMenu;
        this.done = true;
    }

}

