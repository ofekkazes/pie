using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Threading;

public class Loading : Menu
{
    Texture2D texture;
    int i;
    public static bool loaded;
    public Loading()
        : base()
    {
        this.texture = Settings.blankTexture;
        i = 0;
        loaded = false;
    }
    public override void Initialize()
    {
        base.Initialize();
        i = 0;
    }
    public override void Update()
    {
        base.Update();

        if (!loaded)
        {
            LevelManager.LoadContent();
            loaded = true;
        }
        if (i >= 20)
        {
            MenuManager.menuState = MenuState.StartGame;
            MenuManager.gameState = GameState.Playing;
            this.done = true;
            Initialize();
        }
        i++;
    }
    public override void Draw()
    {
        base.Draw();
        Settings.StartDraw();
        Settings.spriteBatch.Draw(this.texture, new Rectangle(0, 0, Settings.gManager.PreferredBackBufferWidth, Settings.gManager.PreferredBackBufferHeight), Color.Black);
        Settings.spriteBatch.DrawString(Settings.menuFont, "Loading..", new Vector2(Settings.gManager.PreferredBackBufferWidth / 2 - Settings.menuFont.MeasureString("Loading..").X / 2,
            Settings.gManager.PreferredBackBufferHeight / 2 - Settings.menuFont.MeasureString("Loading").Y / 2), Color.White);
        Settings.EndDraw();
    }
}

