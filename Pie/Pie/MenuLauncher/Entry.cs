using System;
using Microsoft.Xna.Framework;

    public class Entry
    {
        private static int ID = 0;
        public int id = 0;
        private static Vector2 startPos = new Vector2(Settings.gDevice.Viewport.Width / 2, Settings.gDevice.Viewport.Height);
        private static int yAdder = 40;
        public Vector2 position;
        public float yVelocity;
        public string name;
        public bool chosen;
        public Color color;
        public Rectangle rectangle;
        
        public Entry(string name)
        {
            this.name = name;
            chosen = false;
            color = Color.Gray;
            this.id = ID;
            ID++;
            position = new Vector2(startPos.X, startPos.Y + yAdder);
            yVelocity = 0;
            yAdder += 40;
            this.rectangle = new Rectangle((int)this.position.X, (int)this.position.Y, (int)Settings.menuFont.MeasureString(this.name).X, (int)Settings.menuFont.MeasureString(this.name).Y);
        }
        public Entry(string name, int newID)
        {
            this.name = name;
            chosen = false;
            color = Color.Gray;
            ID = newID;
            this.id = ID;
            position = new Vector2(startPos.X, startPos.Y + yAdder);
            yVelocity = 0;
            yAdder += 40;
            this.rectangle = new Rectangle((int)this.position.X, (int)this.position.Y, (int)Settings.menuFont.MeasureString(this.name).X, (int)Settings.menuFont.MeasureString(this.name).Y);
        }
        
    }

