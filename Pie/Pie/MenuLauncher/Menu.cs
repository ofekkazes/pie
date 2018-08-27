using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

public abstract class Menu
{
    public static int number;

    public Texture2D objectToFloat;
    public Rectangle objectPos;

    public List<Entry> entries;
    public int place;

    protected bool justShowed;
    public bool done;
    protected bool pressed;
    protected bool confirm;

    protected bool isScrolling;

    public Menu()
    {
        this.Initialization();
    }

    private void Initialization()
    {
        this.entries = new List<Entry>();
        this.place = 0;
        this.pressed = false;
        this.confirm = false;
        this.done = false;
        this.isScrolling = true;
        this.justShowed = true;
    }
    public virtual void Initialize()
    {
        this.entries = new List<Entry>();
        this.place = 0;
        this.pressed = false;
        this.confirm = false;
        this.done = false;
        this.isScrolling = true;
        this.justShowed = true;
    }

    public void AddEntry(string entryName)
    {
        entries.Add(new Entry(entryName));
    }
    public void AddEntry(string entryName, int newID)
    {
        entries.Add(new Entry(entryName, newID));
    }

    public void AddObjectToFloat(Texture2D newObject, Rectangle position)
    {
        objectToFloat = newObject;
        objectPos = position;
    }

    /// <summary>
    /// Deletes the first entry with the name in all the entries.
    /// Returns true if the entry deleted, and false if not.
    /// </summary>
    /// <param name="name">The name of the entry you want to delete</param>
    /// <returns></returns>
    public bool DeleteEntry(string name)
    {
        foreach (Entry entry in entries)
        {
            if (entry.name == name)
            {
                entries.RemoveAt(entry.id);
                return true;
            }
        }
        return false;
    }
    public void DeleteAllEntries()
    {
        this.entries = null;
    }

    public virtual void Update()
    {
        if (entries.Count != 0)
        {
            #region Input

            if (!this.isScrolling)
            {

                if (Settings.KeyPress(Keys.Up) || Settings.KeyPress(Keys.W))
                {
                    place--;
                    if (0 > place)
                        place = entries.Count - 1;
                }
                if (Settings.KeyPress(Keys.Down) || Settings.KeyPress(Keys.S))
                {
                    place++;
                    if (place > entries.Count - 1)
                        place = 0;
                }
                int chooseCount = 0;
                for (int i = 0; i < this.entries.Count; i++)
                {
                    if (Settings.mouseRec.Intersects(this.entries[i].rectangle) && Settings.game.IsMouseVisible)
                    {
                        this.place = i;
                        chooseCount++;
                    }
                    if (Settings.mouseState.LeftButton == ButtonState.Pressed && Settings.prevMouseState.LeftButton == ButtonState.Released && Settings.game.IsMouseVisible)
                    {
                        this.pressed = true;
                        this.isScrolling = true;
                    }
                    if (this.entries[i].id == place && (chooseCount == 0 || chooseCount == 1))
                    {
                        this.entries[i].color = Color.White;
                        this.entries[i].chosen = true;
                    }
                    else
                    {
                        this.entries[i].color = Color.Gray;
                        this.entries[i].chosen = false;
                    }
                }

                if (Settings.KeyPress(Keys.Enter))
                {
                    this.pressed = true;
                    this.isScrolling = true;
                }
            }

            #endregion

            #region Scroll

            if (isScrolling)
            {
                if (justShowed)
                {
                    foreach (Entry e in entries)
                        e.yVelocity -= 5.25f;
                    if (entries[0].position.Y < Settings.gManager.PreferredBackBufferHeight / 2 + Settings.gManager.PreferredBackBufferHeight / 3)
                    {
                        isScrolling = false;
                        justShowed = false;
                    }
                }
                if (pressed)
                {
                    foreach (Entry e in entries)
                        e.yVelocity += 5.25f;
                    if (entries[0].position.Y > Settings.gManager.PreferredBackBufferHeight)
                    {
                        if (objectToFloat != null)
                        {
                            if (objectPos.Y > Settings.gManager.PreferredBackBufferHeight)
                            {
                                isScrolling = false;
                                confirm = true;
                            }
                        }
                        else
                        {
                            isScrolling = false;
                            confirm = true;
                        }
                    }
                }
            }

            #endregion

            #region End Update

            foreach (Entry e in entries)
            {
                e.yVelocity *= 0.8f;
                e.position.Y += e.yVelocity;
                e.rectangle = new Rectangle((int)e.position.X, (int)e.position.Y, (int)Settings.menuFont.MeasureString(e.name).X, (int)Settings.menuFont.MeasureString(e.name).Y);
            }
            objectPos.Y = this.entries[0].rectangle.Y - 200;

            #endregion

        }
    }

    public virtual void Draw()
    {
        Settings.StartDraw();
        if (entries.Count != 0)
        {
            foreach (Entry entry in entries)
            {
                Settings.spriteBatch.DrawString(Settings.menuFont, entry.name, entry.position, entry.color);
            }
        }
        if (objectToFloat != null)
            Settings.spriteBatch.Draw(objectToFloat, objectPos, Color.White);

        Settings.EndDraw();
    }
}
