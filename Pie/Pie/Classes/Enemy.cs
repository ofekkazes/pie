using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

public abstract class Enemy
{
    #region Variables

    public string EnemyName = "Enemy";

    public Model model;
    public Matrix matrix;
    public BoundingBoxO box;
    public Vector3 pos;
    public Vector3 velocity;
    public Vector3 dumping;
    public HealthBar healthBar;

    public float attackRadius = 2.0f;

    public int stopInterval;
    public bool attack;

    protected bool move;
    public char moveOnAxis;
    public char moveTo;
    protected bool normalMove;

    public Vector3 startPoint;
    public Vector3 endPoint;

    #endregion

    #region Initialization

    public Enemy()
    {
        this.model = Settings.content.Load<Model>("Models/cube");
        this.box = new BoundingBoxO(this.model, Color.Red);
        this.Initialize();
    }

    public void Initialize()
    {
        this.matrix = Matrix.CreateTranslation(this.pos);
        this.healthBar = new HealthBar(new Vector2(Settings.gManager.PreferredBackBufferWidth / 2 + Settings.gManager.PreferredBackBufferWidth / 4 - 75, 20), 150f);
        this.dumping = new Vector3(0.2f);
        this.stopInterval = 0;
        this.attack = false;
    }

    #endregion

    #region T/F Questions

    public void StartMove()
    {
        move = true;
    }
    public void EndMove()
    {
        move = false;
    }

    #endregion
    int j = 0;
    public virtual void Update()
    {
        if (j > 20)
        {
            if(normalMove) Move();
            if(LevelManager.player.canBeAttacked)
                Attack();
        }
        j++;

        velocity.Y = MathHelper.Clamp(velocity.Y, 1, -1);

        pos.X += velocity.X;
        pos.Z += velocity.Z;
        
        matrix = Matrix.CreateTranslation(pos);

        Vector3 rel = (this.box.boundingBox.Max - this.box.boundingBox.Min) / 2;
        this.box.boundingBox.Min = this.matrix.Translation - rel;
        this.box.boundingBox.Max = this.matrix.Translation + rel;

        velocity *= dumping;
    }

    #region Actions

    private void Move()
    {
        if (move)
        {
            switch (moveOnAxis)
            {
                case 'X':
                case 'x':
                    if (this.pos.X <= endPoint.X && this.pos.X >= startPoint.X)
                    {
                        if (moveTo == 'R')
                        {
                            this.velocity.X+= 0.01f;
                            if (this.pos.X >= endPoint.X - 1)
                                moveTo = 'L';
                        }
                        else if (moveTo == 'L')
                        {
                            this.velocity.X -= 0.01f;
                            if (this.pos.X <= startPoint.X + 1)
                                moveTo = 'R';
                        }
                    }
                    else this.healthBar.healthValue = 0;
                    break;
                case 'Z':
                case 'z':
                    if (this.pos.Z <= endPoint.Z && this.pos.Z >= startPoint.Z)
                    {
                        if (moveTo == 'R')
                        {
                            this.velocity.Z += 0.01f;
                            if (this.pos.Z >= endPoint.Z - 1)
                                moveTo = 'L';
                        }
                        else if (moveTo == 'L')
                        {
                            this.velocity.Z -= 0.01f;
                            if (this.pos.Z <= startPoint.Z + 1)
                                moveTo = 'R';
                        }
                    }
                    else this.healthBar.healthValue = 0;
                    break;
            }
        }
    }

    private void Attack()
    {
        char rightLeft = 'N';
        if (this.box.boundingBox.Intersects(LevelManager.player.box.boundingBox))
            rightLeft = Functions.RightLeft(this.pos, LevelManager.player.pos, moveOnAxis);
        
        if (rightLeft != 'N')
        {
            this.EndMove();
            LevelManager.player.healthBar.LoseHealth(10);

            if (rightLeft == 'R')
                LevelManager.player.Move(moveOnAxis, 2.5f);
            else if (rightLeft == 'L')
                LevelManager.player.Move(moveOnAxis, -2.5f);

            this.attack = true;
            Random randNum = new Random();
            if (randNum.Next(0, 2) == 1)
            {
                if (moveTo == 'R')
                    moveTo = 'L';
                else moveTo = 'R';
            }
        }
        if (attack)
        {
            stopInterval++;
            if (stopInterval >= 150)
            {
                this.StartMove();
                stopInterval = 0;
                attack = false;
            }
        }
    }

    #endregion

    public void Draw3D(Camera camera)
    {
        if (Settings.debug)
            this.box.Draw(this.matrix);
        else Functions.DrawModel(this.model, this.matrix, camera, new Vector3(100f, 0.1f, 0.1f));
    }

    public void Draw2D()
    {
        Settings.StartDraw();
        Settings.spriteBatch.DrawString(Settings.menuFont, EnemyName, new Vector2(this.healthBar.healthBarBorders.X + this.healthBar.healthBarBorders.Width, 20), Color.Red);
        Settings.EndDraw();
        this.healthBar.Draw();
    }

    /// <summary>
    /// Load a random point between the end and start points,
    /// and save the two other point to perform a move between them
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public void LoadPlaceToMove(Vector3 start, Vector3 end, char moveOnAxis)
    {
        if (moveOnAxis == 'X')
        {
            this.startPoint = new Vector3(start.X + 1, start.Y, start.Z);
            this.endPoint = new Vector3(end.X - 1, end.Y, end.Z);
        }
        else if (moveOnAxis == 'Z')
        {
            this.startPoint = new Vector3(start.X, start.Y, start.Z + 1);
            this.endPoint = new Vector3(end.X, end.Y, end.Z - 1);
        }
        else
        {
            this.startPoint = start;
            this.endPoint = end;
        }

        Random rnd = new Random();
        float xAxis;
        float yAxis;
        float zAxis;

        Vector3 big; Vector3 small;

        if (start.X >= end.X)
        {
            big.X = start.X;
            small.X = end.X;
        }
        else
        {
            big.X = end.X;
            small.X = start.X;
        }

        if (start.Y >= end.Y)
        {
            big.Y = start.Y;
            small.Y = end.Y;
        }
        else
        {
            big.Y = end.Y;
            small.Y = start.Y;
        }

        if (start.Z >= end.Z)
        {
            big.Z = start.Z;
            small.Z = end.Z;
        }
        else
        {
            big.Z = end.Z;
            small.Z = start.Z;
        }

        xAxis = MathHelper.Clamp(rnd.Next((int)small.X, (int)big.X), small.X, big.X);
        yAxis = MathHelper.Clamp(rnd.Next((int)small.Y, (int)big.Y), small.Y, big.Y);
        zAxis = MathHelper.Clamp(rnd.Next((int)small.Z, (int)big.Z), small.Z, big.Z);

        pos = new Vector3(xAxis, yAxis, zAxis);

        if (moveOnAxis == 'X' || moveOnAxis == 'Z')
            this.moveOnAxis = moveOnAxis;
        else this.moveOnAxis = 'X';

        int rightLeft = rnd.Next(0, 1);
        if (rightLeft == 0)
            moveTo = 'L';
        else if (rightLeft == 1)
            moveTo = 'R';
    }
}

public class JumpingEnemy : Enemy
{
    private bool hasJumped;
    private Vector3 lastSavedPos;
    private float bleedOff;

    public JumpingEnemy()
        : base()
    {
        this.EnemyName = "Jumping Enemy";
        this.hasJumped = false;
        this.lastSavedPos = Vector3.Zero;
        this.bleedOff = 0.5f;
        this.normalMove = true;
    }
    public void Jump()
    {
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
                if(move) bleedOff = 0.5f;
                lastSavedPos = this.pos;
            }
    }
    public override void Update()
    {
        this.Jump();
        base.Update();
    }

}

public class NormalEnemy : Enemy
{
    public NormalEnemy()
        : base()
    {
        this.EnemyName = "Normal Enemy";
        this.normalMove = true;
    }
}

public class DashingEnemy : Enemy
{
    int timer;
    bool timedMove;
    public DashingEnemy()
        : base()
    {
        this.EnemyName = "Dashing Enemy";
        this.normalMove = false;
        timer = 150;
        timedMove = false;
    }
    public override void Update()
    {
        base.Update();
        Dash();
    }
    private void Dash()
    {
        if (move)
        {
            switch (moveOnAxis)
            {
                case 'X':
                case 'x':
                    if (this.pos.X <= endPoint.X && this.pos.X >= startPoint.X)
                    {
                        if (moveTo == 'R')
                        {
                            if (timer <= 0)
                            {
                                this.timer = 50;
                                if (timedMove) this.velocity.X += 2f;
                                this.timedMove = false;
                                
                            }
                            else
                            {
                                this.timer--;
                                this.timedMove = true;
                            }
                            if (this.pos.X >= endPoint.X - 1)
                                moveTo = 'L';
                        }
                        else if (moveTo == 'L')
                        {
                            if (timer <= 0)
                            {
                                this.timer = 50;
                                if (timedMove) this.velocity.X -= 2f;
                                this.timedMove = false;
                                
                            }
                            else
                            {
                                this.timer--;
                                this.timedMove = true;
                            }
                            if (this.pos.X <= startPoint.X + 1)
                                moveTo = 'R';
                        }
                    }
                    else this.healthBar.healthValue = 0;
                    break;
                case 'Z':
                case 'z':
                    if (this.pos.Z <= endPoint.Z && this.pos.Z >= startPoint.Z)
                    {
                        if (moveTo == 'R')
                        {
                            if (timer <= 0)
                            {
                                this.timer = 50;
                                if (timedMove) this.velocity.Z += 2f;
                                this.timedMove = false;
                                
                            }
                            else
                            {
                                this.timer--;
                                this.timedMove  =true;
                            }
                            if (this.pos.Z >= endPoint.Z - 1)
                                moveTo = 'L';
                        }
                        else if (moveTo == 'L')
                        {
                            if (timer <= 0)
                            {
                                this.timer = 50;
                                if(timedMove) this.velocity.Z -= 2f;
                                this.timedMove = false;
                                
                            }
                            else
                            {
                                this.timer--;
                                this.timedMove = true;
                            }
                            if (this.pos.Z <= startPoint.Z + 1)
                                moveTo = 'R';
                        }
                    }
                    else this.healthBar.healthValue = 0;
                    break;
            }
        }
    }
}
