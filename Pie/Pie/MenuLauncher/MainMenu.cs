using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Windows.Forms;

    public class MainMenu : Menu
    {
        public MainMenu() : base()
        {
            this.Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            AddEntries();
        }

        public void AddEntries()
        {
            this.AddEntry("Play Game", 0);
            this.AddEntry("Instructions", 1);
            this.AddEntry("Options", 2);
            this.AddEntry("Exit Game", 3);
        }

        public override void Update()
        {
            base.Update();

            if (this.confirm)
            {
                switch (place)
                {
                    case 0: PlayScreenSelected();
                        break;
                    case 1: InstructionsScreenSelected();
                        break;
                    case 2: OptionsScreenSelected();
                        break;
                    case 3: ExitScreenSelected();
                        break;
                }
            }
            if (this.done)
                this.Initialize();
        }
        

        private void PlayScreenSelected()
        {
            /*for (int i = 0; i < LevelManager.levels.Count; i++)
                LevelManager.levels[i].Initialize();
            LevelManager.currentLevelID = 0;
            LevelManager.player.Initialize();*/
            MenuManager.gameState = GameState.Loading;
            MenuManager.menuState = MenuState.LevelChooser;
            this.done = true;
        }
        private void InstructionsScreenSelected()
        {
            MenuManager.menuState = MenuState.Instructions;
            this.done = true;
        }
        private void OptionsScreenSelected()
        {
            MenuManager.menuState = MenuState.Options;
            this.done = true;
        }
        private void ExitScreenSelected()
        {
            this.done = true;
            if (Settings.gManager.IsFullScreen == true)
                Settings.gManager.ToggleFullScreen();
            if (MessageBox.Show("Do you want to go out to Windows?", "Quit The Game?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Settings.game.Exit();
        }

        public override void Draw()
        {
            base.Draw();
        }
    }

