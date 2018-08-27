using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Bullet
{
    public Matrix matrix;
    public Vector3 position;
    public float acceleration;
    public bool right;
    public int timer;
    public bool kill;
    public Bullet(Vector3 position, bool right)
    {
        this.position = position;
        this.matrix = Matrix.CreateTranslation(this.position);
        this.acceleration = 0;
        this.right = right;
        this.timer = 0;
        this.kill = false;
    }
}
public class Gun
{
    public List<Bullet> bullets;
    public static Model model = Settings.content.Load<Model>("Models/bullet");
    public int timer;
    public bool avalible;
    public bool adder;

    public Gun()
    {
        this.bullets = new List<Bullet>();
        this.timer = 15;
        this.avalible = true;
        this.adder = false;
    }

    public void Update(Vector3 position, Camera.Facing facing)
    {
        if (Settings.MouseClick(false))
        {
            this.avalible = false;
            if (facing == Camera.Facing.North || facing == Camera.Facing.South)
            {
                if (Settings.mouseRec.X >= Settings.gManager.PreferredBackBufferWidth / 2)
                    this.bullets.Add(new Bullet(position, true));
                else this.bullets.Add(new Bullet(position, false));
            }
            else if (facing == Camera.Facing.West || facing == Camera.Facing.East)
            {
                if (Settings.mouseRec.X >= Settings.gManager.PreferredBackBufferWidth / 2)
                    this.bullets.Add(new Bullet(position, false));
                else this.bullets.Add(new Bullet(position, true));
            }
        }
        else if(Settings.KeyPress(Keys.LeftControl))
        {
            if (this.bullets.Count == 0)
                this.bullets.Add(new Bullet(position, true));
            else this.bullets.Add(new Bullet(position, this.bullets[this.bullets.Count - 1].right));
        }
        
        foreach (Bullet b in this.bullets)
        {
            b.acceleration += 0.6f;
            b.acceleration = MathHelper.Clamp(b.acceleration, 0f, 4f);
            MoveForward(b, facing);
            b.matrix = Matrix.CreateTranslation(b.position);
            b.timer++;
            if (b.timer > 100)
                b.kill = true;
        }
        for (int i = 0; i < this.bullets.Count - 1; i++)
            if (this.bullets[i].kill)
                this.bullets.Remove(this.bullets[i]);

    }
    private void MoveForward(Bullet b, Camera.Facing facing)
    {
        //b.position -= b.acceleration;

        if (facing == Camera.Facing.North)
        {
            if (b.right)
                b.position.X += b.acceleration;
            else b.position.X -= b.acceleration;
        }
        else if (facing == Camera.Facing.East)
        {
            if (b.right)
                b.position.Z -= b.acceleration;
            else b.position.Z += b.acceleration;
        }
        else if (facing == Camera.Facing.West)
        {
            if (b.right)
                b.position.Z += b.acceleration;
            else b.position.Z -= b.acceleration;
        }
        else if (facing == Camera.Facing.South)
        {
            if (b.right)
                b.position.X -= b.acceleration;
            else b.position.X += b.acceleration;
        }
    }

    public void Draw(Camera camera)
    {
        foreach (Bullet b in this.bullets)
            Functions.DrawModel(Gun.model, b.matrix, camera, Color.Silver.ToVector3());
    }
}

