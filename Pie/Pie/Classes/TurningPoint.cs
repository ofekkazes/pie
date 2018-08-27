using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

public class TurningPoint
{
    public Vector3 position;
    private BoundingBoxO box;
    public BoundingBoxO BoundingBox
    {
        get { return this.box; }
    }

    public bool eastTurn;
    public bool westTurn;
    public bool northTurn;
    public bool southTurn;

    public TurningPoint(Vector3 position, bool eastTurn, bool westTurn, bool northTurn, bool southTurn)
    {
        this.position = position;
        this.eastTurn = eastTurn;
        this.westTurn = westTurn;
        this.southTurn = southTurn;
        this.northTurn = northTurn;
        this.box = new BoundingBoxO(Settings.content.Load<Model>("Models/cube"), Color.Green);
        Vector3 rel = (this.box.boundingBox.Max - this.box.boundingBox.Min) / 2;
        this.box.boundingBox.Min = Matrix.CreateTranslation(this.position).Translation - rel;
        this.box.boundingBox.Max = Matrix.CreateTranslation(this.position).Translation + rel;
    }
    public List<string> GetAvalibleTurns()
    {

        List<string> avalible = new List<string>();
        if (this.eastTurn)
            avalible.Add("East");
        if (this.westTurn)
            avalible.Add("West");
        if (this.northTurn)
            avalible.Add("North");
        if (this.southTurn)
            avalible.Add("South");
        return avalible;
    }

    public bool IsCollide(BoundingBoxO box)
    {
        return this.box.boundingBox.Intersects(box.boundingBox);
    }

    public override string ToString()
    {
        return position.ToString();
    }
}
