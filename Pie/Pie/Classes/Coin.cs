using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

public class Coin
{
    public static int ID = 0;
    public int id = 0;
    public Model model;
    public Vector3 position;
    public Matrix matrix;
    public float rotation;

    public bool collected;
    public BoundingSphere boundigSphere;

    public Coin(Vector3 position)
    {
        this.model = Settings.content.Load<Model>("Models/Coin");
        this.position = position;
        this.matrix = Matrix.Identity;
        this.matrix = Matrix.CreateTranslation(this.position);
        ID++;
        this.id = ID;
        this.rotation = 0;

        this.collected = false;
        this.boundigSphere = Functions.CreateBoundingSphere(this.model, this.position);
    }

    public void Update()
    {
        this.rotation += 0.5f;
        if (this.rotation >= 360f)
            this.rotation = 0;
    }
    public void Draw()
    {
        Functions.DrawModel(this.model, this.matrix, Matrix.CreateRotationY(MathHelper.ToRadians(this.rotation)), Level.camera, Color.Gold.ToVector3());
    }
}

