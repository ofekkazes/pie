using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public static class Settings
{
    public static Game game;
    public static bool debug;
    public static bool sounds;

    public static GraphicsDevice gDevice;
    public static GraphicsDeviceManager gManager;
    public static SpriteBatch spriteBatch;
    public static ContentManager content;
    public static GameTime gameTime;

    public static KeyboardState keyBoard;
    public static KeyboardState prevKeyBoard;

    public static MouseState mouseState;
    public static MouseState prevMouseState;
    public static Rectangle mouseRec;

    public static SpriteFont menuFont;
    public static SpriteFont titleFont;
    public static SpriteFont bigFont;
    public static SpriteFont smallFont;

    public static Texture2D enemy;
    public static Texture2D coin;
    public static Texture2D blankTexture;
    public static Texture2D rightArrow, leftArrow, upperArrow, lowerArrow;

    public static void SetUp(GraphicsDeviceManager Manager, ContentManager Content, Game gameHelper)
    {
        game = gameHelper;
        debug = false;
        sounds = true;

        gManager = Manager;
        gDevice = gManager.GraphicsDevice;
        content = Content;
        spriteBatch = new SpriteBatch(gDevice);

        menuFont = content.Load<SpriteFont>("Fonts/menuFont");
        titleFont = content.Load<SpriteFont>("Fonts/title");
        bigFont = content.Load<SpriteFont>("Fonts/BigFont");
        smallFont = content.Load<SpriteFont>("Fonts/SmallFont");

        blankTexture = content.Load<Texture2D>("Textures/blank");
        coin = Settings.content.Load<Texture2D>("Textures/Coin");
        enemy = Settings.content.Load<Texture2D>("Textures/Enemy");
        rightArrow = Settings.content.Load<Texture2D>("Textures/rightArrow");
        leftArrow = Settings.content.Load<Texture2D>("Textures/leftArrow");
        upperArrow = Settings.content.Load<Texture2D>("Textures/upperArrow");
        lowerArrow = Settings.content.Load<Texture2D>("Textures/lowerArrow");
    }

    public static void SetUpdate(GameTime gametime)
    {
        gameTime = gametime;
        keyBoard = Keyboard.GetState();
        mouseState = Mouse.GetState();

        if (Settings.game.IsMouseVisible) mouseRec = new Rectangle((int)Settings.GetMousePos().X, (int)Settings.GetMousePos().Y, 1, 1);
        else mouseRec = new Rectangle(0, 0, 1, 1);
        if (Settings.KeyPress(Keys.T))
        {
            if (debug)
                debug = false;
            else debug = true;
        }
    }

    public static void GetUpdate()
    {
        prevKeyBoard = keyBoard;
        prevMouseState = mouseState;
    }

    public static void StartDraw()
    {
        Settings.spriteBatch.Begin();
    }

    public static void EndDraw()
    {
        Settings.spriteBatch.End();
    }

    public static bool KeyPress(Keys k)
    {
        return (keyBoard.IsKeyDown(k) && prevKeyBoard.IsKeyUp(k));
    }

    public static bool MouseClick(bool leftMouse)
    {
        if (leftMouse) return (Settings.mouseState.LeftButton == ButtonState.Pressed && Settings.prevMouseState.LeftButton == ButtonState.Released);
        return (Settings.mouseState.RightButton == ButtonState.Pressed && Settings.prevMouseState.RightButton == ButtonState.Released);
    }

    public static Vector2 GetMousePos()
    {
        return new Vector2(mouseState.X, mouseState.Y);
    }

    public static void DrawMultiText(List<string> writes, Vector2 startingPos)
    {
        float yPos = startingPos.Y;
        StartDraw();
        foreach (string s in writes)
        {
            Settings.spriteBatch.DrawString(Settings.smallFont, s, new Vector2(startingPos.X, yPos), Color.White);
            yPos += 20;
        }
        EndDraw();
    }
}
