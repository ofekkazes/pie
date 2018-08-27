#region Usings

using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

#endregion

public class HealthBar
{
        #region Variables

    public static float MaxHealth = 150;

    public Texture2D texture;
    public Rectangle rectangle;
    public Vector2 position;

    public Rectangle healthBarBorders;
    public Texture2D healthBarBordersTexture;

    public float healthValue;
    public Vector2 healthValuePosition;

    #endregion

    #region Initialization

    public HealthBar(Vector2 position)
    {
        this.position = position;
        this.healthValue = MaxHealth;

        this.Initialize();
    }
    public HealthBar(Vector2 position, float startHealth)
    {
        this.position = position;

        if (MaxHealth > startHealth)
            this.healthValue = startHealth;
        else this.healthValue = MaxHealth;

        this.Initialize();
    }
    public void Initialize()
    {
        this.texture = Settings.content.Load<Texture2D>("Textures/HealthBar");
        this.rectangle = new Rectangle(0, 0, this.texture.Width, this.texture.Height);

        this.healthValuePosition = new Vector2(this.position.X + 3 + this.rectangle.Width / 2 - Settings.menuFont.MeasureString(this.healthValue.ToString()).X / 2,
            this.position.Y + 3 + this.rectangle.Height / 2 - Settings.menuFont.MeasureString(this.healthValue.ToString()).Y / 2);

        this.healthBarBordersTexture = Settings.content.Load<Texture2D>("Textures/HealthBarBorders");
        this.healthBarBorders = new Rectangle((int)position.X, (int)position.Y, this.healthBarBordersTexture.Width, this.healthBarBordersTexture.Height);
    }

    #endregion

    #region Draw

    public void Draw()
    {
        this.rectangle.Width = (int)this.healthValue;
        Settings.StartDraw();
        Settings.spriteBatch.Draw(this.texture, this.position, this.rectangle, Color.White);
        Settings.spriteBatch.DrawString(Settings.menuFont, this.healthValue.ToString(), this.healthValuePosition, Color.White);
        Settings.spriteBatch.Draw(this.healthBarBordersTexture, this.healthBarBorders, Color.White);
        Settings.EndDraw();
    }

    #endregion

    #region Updates

    public void LoseHealth(float loss)
    {
        this.healthValue -= loss;
        if (this.healthValue < 0)
            this.healthValue = 0;
    }
    public void AddHealth(float add)
    {
        this.healthValue += add;
        if (this.healthValue > MaxHealth)
            this.healthValue = MaxHealth;
    }

    #endregion

    #region Overrides

    public override string ToString()
    {
        return (healthValue.ToString());
    }

    #endregion
}

