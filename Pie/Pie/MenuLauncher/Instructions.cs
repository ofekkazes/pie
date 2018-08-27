using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Instructions : Menu
{
    public Instructions()
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
        this.AddEntry("Mouse", 0);
        this.AddEntry("Keyboard", 1);
        this.AddEntry("Objective", 2);
        this.AddEntry("Back", 3);
    }
    public override void Update()
    {
        base.Update();
        if (this.confirm)
        {
            switch (place)
            {
                case 0: MouseSelected();
                    break;
                case 1: KeyboardSelected();
                    break;
                case 2: ObjectiveSelected();
                    break;
                case 3: BackScreenSelected();
                    break;
            }
        }
        if (this.done)
            this.Initialize();
    }
    public void MouseSelected()
    {
        MenuManager.menuState = MenuState.MouseInstructions;
        this.done = true;
    }
    public void KeyboardSelected()
    {
        MenuManager.menuState = MenuState.KeyboardInstructions;
        this.done = true;
    }
    public void ObjectiveSelected()
    {
        MenuManager.menuState = MenuState.ObjectiveInstructions;
        this.done = true;
    }
    public void BackScreenSelected()
    {
        MenuManager.menuState = MenuState.MainMenu;
        this.done = true;
    }
    public override void Draw()
    {
        base.Draw();
    }
}

