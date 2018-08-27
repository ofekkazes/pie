using System;

    public class Options : Menu
    {
        public Options()
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
            this.AddEntry("Enable Sounds?: " + Settings.sounds, 0);
            this.AddEntry("Mouse Control? " + Settings.game.IsMouseVisible, 1);
            this.AddEntry("Save Game", 2);
            this.AddEntry("LoadGame", 3);
            this.AddEntry("Back", 4);
        }

        public override void Update()
        {
            base.Update();

            if (this.confirm)
            {
                switch (this.place)
                {
                    case 0: SoundsSelected();
                        break;
                    case 1: MouseSelected();
                        break;
                    case 2: SaveSelected();
                        break;
                    case 3: LoadSelected();
                        break;
                    case 4: BackSelected();
                        break;
                }
            }
            if(this.done)
                this.Initialize();
        }
        public override void Draw()
        {
            base.Draw();
        }

        private void SoundsSelected()
        {
            Settings.sounds = !Settings.sounds;
            this.done = true;
        }
        private void MouseSelected()
        {
            Settings.game.IsMouseVisible = !Settings.game.IsMouseVisible;
            this.done = true;
        }
        private void SaveSelected()
        {
            SaveGame.doSave = true;
            SaveGame.Save();
            this.done = true;
        }
        private void LoadSelected()
        {
            SaveGame.Load();
            this.done = true;
        }
        private void BackSelected()
        {
            MenuManager.menuState = MenuState.MainMenu;
            this.done = true;
        }
    }

