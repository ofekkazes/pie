#region Usings

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

public class Hero
{
    #region Variables

    public static string PlayerName = "Player";

    public Model model;
    public Matrix matrix;

    public BoundingBoxO box;
    public bool canBeAttacked;

    public Vector3 pos;
    public Vector3 velocity;
    public float speed;
    public Vector3 dumping;
    public bool hasJumped;
    public Vector3 lastSavedPos;
    public float bleedOff;
    public HealthBar healthBar;
    public Gun gun;

    #endregion

    #region Initialize

    public Hero(Model model)
    {
        this.model = model;
        this.box = new BoundingBoxO(this.model, Color.Blue);
        
        Initialize();
    }

    public void Initialize()
    {
        this.matrix = Matrix.Identity;
        this.pos = Vector3.Zero;
        this.velocity = new Vector3(0, 0, 0);
        this.dumping = new Vector3(0.8f);
        this.hasJumped = false;
        this.bleedOff = 0.5f;
        this.healthBar = new HealthBar(new Vector2(Settings.gManager.PreferredBackBufferWidth / 2 - Settings.gManager.PreferredBackBufferWidth / 4 - 75, 20), 150f);
        this.speed = 0.1f;
        this.matrix = Matrix.CreateRotationZ(82.0f);
        this.gun = new Gun();
        this.canBeAttacked = true;
    }
    public void Initialize(Vector3 pos)
    {
        this.matrix = Matrix.Identity;
        this.pos = pos;
        this.velocity = new Vector3(0, 0, 0);
        this.dumping = new Vector3(0.8f);
        this.hasJumped = false;
        this.bleedOff = 0.5f;
        this.healthBar = new HealthBar(new Vector2(Settings.gManager.PreferredBackBufferWidth / 2 - Settings.gManager.PreferredBackBufferWidth / 4 - 75, 20), 150f);
        this.speed = 0.1f;
        this.matrix = Matrix.CreateRotationZ(82.0f);
        this.gun = new Gun();
        this.canBeAttacked = true;
    }

    #endregion

    #region Updates

    public void Update()
    {
        if (healthBar.healthValue == 0)
            MenuManager.gameState = GameState.Lost;
        if (Level.camera.direction == Camera.Facing.WhileChange)
            this.canBeAttacked = false;
        else this.canBeAttacked = true;

        pos += velocity;
        Input();

        matrix = Matrix.CreateTranslation(pos);

        Vector3 rel = (this.box.boundingBox.Max - this.box.boundingBox.Min) / 2;
        this.box.boundingBox.Min = this.matrix.Translation - rel;
        this.box.boundingBox.Max = this.matrix.Translation + rel;
        

        velocity *= dumping;

        this.gun.Update(this.pos, Level.camera.direction);

        Limitations();

    }

    public void Draw3D()
    {
        this.gun.Draw(Level.camera);

        if (Settings.debug)
            this.box.Draw(this.matrix);
        else Functions.DrawModel(this.model, this.matrix, Matrix.CreateRotationZ(MathHelper.ToRadians(Level.camera.RotationAngle)), Level.camera, new Vector3(0.1f, 0.1f, 100f));
    }

    public void Draw2D()
    {
        
        Settings.StartDraw();
        Settings.spriteBatch.DrawString(Settings.menuFont, PlayerName, new Vector2(this.healthBar.position.X - Settings.menuFont.MeasureString(PlayerName).X - 5, this.healthBar.position.Y), Color.Blue);
        Settings.EndDraw();
        this.healthBar.Draw();
    }

    #endregion

    #region Input

    public void Input()
    {
        bool leftIntersection = false, rightIntersection = false;
        if (Settings.mouseState.LeftButton == ButtonState.Pressed)
        {
            if (LevelManager.levels[LevelManager.currentLevelID].leftArrow.Intersects(Settings.mouseRec)) leftIntersection = true;
            if (LevelManager.levels[LevelManager.currentLevelID].rightArrow.Intersects(Settings.mouseRec)) rightIntersection = true;
        }

        #region Moving

        if (Level.camera.direction == Camera.Facing.North)
        {
            if (Settings.keyBoard.IsKeyDown(Keys.A) || Settings.keyBoard.IsKeyDown(Keys.Left) || leftIntersection)
                this.Move('X', -this.speed);
            else if (Settings.keyBoard.IsKeyDown(Keys.D) || Settings.keyBoard.IsKeyDown(Keys.Right) || rightIntersection)
                this.Move('X', this.speed);
        }
        else if (Level.camera.direction == Camera.Facing.South)
        {
            if (Settings.keyBoard.IsKeyDown(Keys.A) || Settings.keyBoard.IsKeyDown(Keys.Left) || leftIntersection)
                this.Move('X', this.speed);
            else if (Settings.keyBoard.IsKeyDown(Keys.D) || Settings.keyBoard.IsKeyDown(Keys.Right) || rightIntersection)
                this.Move('X', -this.speed);
        }
        else if (Level.camera.direction == Camera.Facing.East)
        {
            if (Settings.keyBoard.IsKeyDown(Keys.A) || Settings.keyBoard.IsKeyDown(Keys.Left) || leftIntersection)
                this.Move('Z', -this.speed);
            else if (Settings.keyBoard.IsKeyDown(Keys.D) || Settings.keyBoard.IsKeyDown(Keys.Right) || rightIntersection)
                this.Move('Z', this.speed);
        }
        else if (Level.camera.direction == Camera.Facing.West)
        {
            if (Settings.keyBoard.IsKeyDown(Keys.A) || Settings.keyBoard.IsKeyDown(Keys.Left) || leftIntersection)
                this.Move('Z', this.speed);
            else if (Settings.keyBoard.IsKeyDown(Keys.D) || Settings.keyBoard.IsKeyDown(Keys.Right) || rightIntersection)
                this.Move('Z', -this.speed);
        }

        #endregion

        this.CheckJump();
    }

    public void Move(char axis, float add)
    {
        switch (axis)
        {
            case 'x':
            case 'X': this.velocity.X += add;
                break;
            case 'y':
            case 'Y': this.velocity.Y += add;
                break;
            case 'z':
            case 'Z': this.velocity.Z += add;
                break;
        }
    }

    #endregion

    #region Checkers

    public void CheckJump()
    {
        if ((Settings.KeyPress(Keys.Space) || Settings.mouseRec.Intersects(new Rectangle(Settings.gManager.PreferredBackBufferWidth / 2 - 30, Settings.gManager.PreferredBackBufferHeight/ 2 - 30, 60, 60)) ) && hasJumped == false)
            hasJumped = true;
        if (hasJumped)
        {
            pos.Y += bleedOff;
            bleedOff -= 0.03f;
            if (pos.Y < lastSavedPos.Y)
            {
                hasJumped = false;
                pos.Y = lastSavedPos.Y;
            }
        }
        bleedOff = MathHelper.Clamp(bleedOff, -1f, 1f);
        if (hasJumped == false)
        {
            bleedOff = 0.5f;
            lastSavedPos = pos;
        }
    }

    #endregion

    #region Limits

    public void Limitations()
    {
        this.velocity.X = MathHelper.Clamp(this.velocity.X, -1f, 1f);
        this.velocity.Y = MathHelper.Clamp(this.velocity.Y, -3f, 3f);
    }

    #endregion

}