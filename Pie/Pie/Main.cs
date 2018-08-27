using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Windows.Forms;

public class Main : Microsoft.Xna.Framework.Game
{
    GraphicsDeviceManager graphics;
    public static Form gameForm;

    SoundEffectInstance[] sounds;
    int turn;

    public Main()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        base.Initialize();
        gameForm = (Form)Form.FromHandle(Settings.game.Window.Handle);
        gameForm.FormBorderStyle = FormBorderStyle.None;
    }

    protected override void LoadContent()
    {
        Settings.SetUp(graphics, Content, this);

        //LevelManager.LoadContent();

        MenuManager.Initialize();

        Settings.game.IsMouseVisible = true;

        this.sounds = new SoundEffectInstance[6];
        this.sounds[0] = Settings.content.Load<SoundEffect>("Sounds/Pie Volume 1").CreateInstance();
        this.sounds[1] = Settings.content.Load<SoundEffect>("Sounds/Pie Volume 2").CreateInstance();
        this.sounds[2] = Settings.content.Load<SoundEffect>("Sounds/Pie Volume 3").CreateInstance();
        this.sounds[3] = Settings.content.Load<SoundEffect>("Sounds/Pie Volume 4").CreateInstance();
        this.sounds[4] = Settings.content.Load<SoundEffect>("Sounds/Pie Volume 5").CreateInstance();
        this.sounds[5] = Settings.content.Load<SoundEffect>("Sounds/Pie Volume 6").CreateInstance();

        this.sounds[0].Play();
        this.turn = 0;

        Settings.sounds = false;

    }
    
    protected override void Update(GameTime gameTime)
    {
        if (Settings.sounds)
        {
            if (this.turn > this.sounds.Length - 1) this.turn = 0;
            if (this.sounds[turn].State == SoundState.Stopped)
                this.turn++;
            if (this.sounds[this.turn].State != SoundState.Playing) this.sounds[this.turn].Play();
        }
        else
        {
            for (int i = 0; i < this.sounds.Length - 1; i++) this.sounds[i].Stop(); this.turn = 0;
        }
        if (gameForm.Focused)
        {
            Settings.SetUpdate(gameTime);

            MenuManager.Update();

            if (MenuManager.gameState == GameState.Playing)
            {
                //The gamestate is equal to playing
                LevelManager.Update();
                MenuManager.pauseAlpha -= 0.01f;
                MenuManager.loadAlpha -= 0.01f;

            }
            MenuManager.pauseAlpha = MathHelper.Clamp(MenuManager.pauseAlpha, 0, 0.3f);
            if (MenuManager.gameState == GameState.Loading && MenuManager.menuState == MenuState.StartGame)
                LevelManager.SelfUpdate();
            Settings.GetUpdate();
        }
        base.Update(gameTime);
    }
    public static List<string> writing;
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        #region Reset Graphics

        Settings.gDevice.BlendState = BlendState.AlphaBlend;
        Settings.gDevice.DepthStencilState = DepthStencilState.None;
        Settings.gDevice.RasterizerState = RasterizerState.CullCounterClockwise;
        Settings.gDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;

        #endregion

        #region Begin 3D render

        Settings.gDevice.BlendState = BlendState.Opaque;
        Settings.gDevice.DepthStencilState = DepthStencilState.Default;

        #endregion

        #region Level Render

        if (MenuManager.gameState == GameState.Playing || MenuManager.gameState == GameState.Paused)
        {
            LevelManager.Draw3D();
            LevelManager.Draw2D();
        }

        #endregion

        #region Begin 2D render

        #region Game Name

        if (MenuManager.gameState == GameState.MainMenu && MenuManager.menuState == MenuState.MainMenu)
        {
            Settings.StartDraw();
            Settings.spriteBatch.DrawString(Settings.titleFont, "3D Shooter",
                new Vector2(Settings.gManager.PreferredBackBufferWidth / 2, Settings.gManager.PreferredBackBufferHeight / 2 - Settings.gManager.PreferredBackBufferHeight / 4),
                Color.Blue);
            Settings.EndDraw();
        }

        #endregion
        writing = new List<string>();
        if (Settings.debug)
        {
            writing.Add("");
            writing.Add("");
            writing.Add("");
            writing.Add("");
            writing.Add("");
            writing.Add("");
            writing.Add("");
            writing.Add("");
            writing.Add("");
            writing.Add("");
            writing.Add("Debug Menu:");
            writing.Add("");
            if (Loading.loaded)
            {
                writing.Add("X: " + LevelManager.player.pos.X);
                writing.Add("Y: " + LevelManager.player.pos.Y);
                writing.Add("Z: " + LevelManager.player.pos.Z);
                writing.Add("Facing " + Level.camera.direction.ToString());
                writing.Add("Bullets: " + LevelManager.player.gun.bullets.Count.ToString());
                writing.Add(Level.camera.RotationAngle.ToString());
                LevelManager.levels.ForEach(delegate(Level l)
                {
                    if (l.goodLoad) writing.Add("Level " + l.id + " returned a good load");
                    else writing.Add("Level " + l.id + " returned a bad load and cannot update");
                });
            }
            writing.Add("");
            writing.Add("Press T to show/hide Debug Menu");
            Settings.DrawMultiText(writing, Vector2.Zero);

        }

        #endregion

        #region Menu

        MenuManager.Draw();

        #endregion

        base.Draw(gameTime);
    }

}

#region Program

#if WINDOWS
public static class Program
{
    static void Main(string[] args)
    {
        using (Main game = new Main())
        {
            game.Run();
        }
    }
}
#endif

#endregion
