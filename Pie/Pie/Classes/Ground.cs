using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class Ground
{
    private Model model;
    private Vector3 position;
    private Matrix matrix;

    public List<BoundingBox> boxes;

    public Ground(Model model, Vector3 position)
    {
        this.model = model;
        this.position = position;
        this.matrix = Matrix.CreateTranslation(this.position);
        this.boxes = new List<BoundingBox>();
    }
    public void AddBox(BoundingBox b)
    {
        this.boxes.Add(b);
    }

    public void Draw()
    {
        if (Settings.debug) { this.boxes.ForEach(delegate(BoundingBox b) {  }); }
        else Functions.DrawModel(this.model, this.matrix, Matrix.CreateRotationZ(MathHelper.ToRadians(0f)), Level.camera, Vector3.Zero);
    }
}

