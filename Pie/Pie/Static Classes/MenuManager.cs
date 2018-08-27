using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System.Collections.Generic;

public static class MenuManager
{
    public static GameState gameState;
    public static MenuState menuState;

    public static MainMenu mainMenu;
    public static Instructions instructions;
    public static Options options;
    public static Loading loading;
    public static LoseMod loseMod;
    public static WonMod wonMod;
    public static ChooseLevel chooseLevel;
    public static MouseInstructions mouseInst;
    public static KeyboardInstructions keyboardInst;
    public static ObjectiveInstructions objectiveInst;

    public static float pauseAlpha;
    public static float loadAlpha;

    public static void Initialize()
    {
        loadAlpha = 0;

        gameState = GameState.MainMenu;
        menuState = MenuState.MainMenu;

        mainMenu = new MainMenu();
        instructions = new Instructions();
        options = new Options();
        loading = new Loading();
        loseMod = new LoseMod();
        wonMod = new WonMod();
        chooseLevel = new ChooseLevel();
        mouseInst = new MouseInstructions();
        keyboardInst = new KeyboardInstructions();
        objectiveInst = new ObjectiveInstructions();
    }

    public static void Update()
    {

        #region Pause

        if (Settings.KeyPress(Keys.P))
        {
            if (MenuManager.gameState == GameState.Playing && MenuManager.menuState == MenuState.StartGame)
                MenuManager.gameState = GameState.Paused;
            else if (MenuManager.gameState == GameState.Paused)
                MenuManager.gameState = GameState.Playing;
        }
        else if (MenuManager.gameState == GameState.Paused)
        {
            //The gamestate is equal to paused
            pauseAlpha += 0.01f;
        }
        if (Settings.KeyPress(Keys.Escape) && MenuManager.gameState == GameState.Paused)
        {
            MenuManager.gameState = GameState.MainMenu;
            MenuManager.menuState = MenuState.MainMenu;
            mainMenu.Initialize();
        }
        #endregion

        #region Main Menu

        if (MenuManager.gameState == GameState.MainMenu && MenuManager.menuState == MenuState.MainMenu)
            mainMenu.Update();

        #endregion

        #region Instructions

        if (MenuManager.menuState == MenuState.Instructions && MenuManager.gameState == GameState.MainMenu)
            instructions.Update();

        #endregion

        #region Options

        if (MenuManager.menuState == MenuState.Options && MenuManager.gameState == GameState.MainMenu)
            options.Update();

        #endregion

        #region Loading

        if (MenuManager.menuState == MenuState.StartGame && MenuManager.gameState == GameState.Loading)
        {
            loading.Update();
            loadAlpha = 0.3f;
        }

        MenuManager.loadAlpha = MathHelper.Clamp(MenuManager.loadAlpha, 0, 0.3f);

        #endregion

        #region LostMod

        if (gameState == GameState.Lost)
            loseMod.Update();

        #endregion

        #region WinMod

        if (gameState == GameState.Won)
            wonMod.Update();

        #endregion

        if (menuState == MenuState.LevelChooser)
            chooseLevel.Update();

        if (menuState == MenuState.MouseInstructions)
            mouseInst.Update();

        if (menuState == MenuState.KeyboardInstructions)
            keyboardInst.Update();

        if (menuState == MenuState.ObjectiveInstructions)
            objectiveInst.Update();
    }

    public static void Draw()
    {
        Functions.FadeBackBufferToBlack(loadAlpha, Settings.gDevice, Settings.spriteBatch);

        #region Main Menu

        if (MenuManager.gameState == GameState.MainMenu && MenuManager.menuState == MenuState.MainMenu)
            mainMenu.Draw();
        if (MenuManager.menuState == MenuState.Instructions && MenuManager.gameState == GameState.MainMenu)
            instructions.Draw();
        if (MenuManager.menuState == MenuState.Options && MenuManager.gameState == GameState.MainMenu)
            options.Draw();

        #endregion

        #region Pause

        if (gameState == GameState.Paused || gameState == GameState.Playing)
            Functions.FadeBackBufferToBlack(pauseAlpha, Settings.gDevice, Settings.spriteBatch);
        if (gameState == GameState.Paused)
        {
            Settings.StartDraw();
            Functions.ShowPauseScreen();
            Settings.EndDraw();
        }

        #endregion

        #region Loading

        if (MenuManager.menuState == MenuState.StartGame && MenuManager.gameState == GameState.Loading)
            loading.Draw();
        #endregion

        #region LostMod

        if (gameState == GameState.Lost)
            loseMod.Draw();

        #endregion

        #region WinMod

        if (gameState == GameState.Won)
            wonMod.Draw();

        #endregion

        if (menuState == MenuState.LevelChooser)
            chooseLevel.Draw();

        if (menuState == MenuState.MouseInstructions)
            mouseInst.Draw();

        if (menuState == MenuState.KeyboardInstructions)
            keyboardInst.Draw();

        if (menuState == MenuState.ObjectiveInstructions)
            objectiveInst.Draw();
    }

}

public enum GameState
{
    MainMenu = 0,
    Loading = 1,
    Playing = 2,
    Paused = 3,
    Lost = 4,
    Won = 5
}
public enum MenuState
{
    MainMenu = 0,
    StartGame = 1,
    Instructions = 2,
    Options = 3,
    LevelChooser = 4,
    KeyboardInstructions = 5,
    MouseInstructions = 6,
    ObjectiveInstructions = 7
}

