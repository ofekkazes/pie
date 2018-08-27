using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class ObjectiveInstructions : Menu
{
    public ObjectiveInstructions()
        : base()
    {
        Initialize();
    }
    public override void Initialize()
    {
        base.Initialize();
        AddEntries();
        AddObjectToFloat(Settings.content.Load<Texture2D>("Textures/Objective"), new Rectangle(Settings.gManager.PreferredBackBufferWidth / 2 - 200, Settings.gManager.PreferredBackBufferHeight - 100, 400, 200));
    }
    public void AddEntries()
    {
        this.AddEntry("Back", 0);
    }
    public override void Update()
    {
        base.Update();
        if (this.confirm)
        {
            switch (place)
            {
                case 0: BackScreenSelected();
                    break;
            }
        }
        if (this.done)
            this.Initialize();
    }

    public void BackScreenSelected()
    {
        MenuManager.menuState = MenuState.Instructions;
        this.done = true;
    }
    public override void Draw()
    {
        base.Draw();
    }
}

