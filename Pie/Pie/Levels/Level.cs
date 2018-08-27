using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


public abstract class Level
{
    public int id;
    protected bool done;
    public bool goodLoad;

    private Vector3 startPoint;
    private Vector3 endPoint;
    private Matrix endPointMatrix;
    private int openEndMessageTimer;
    private float alphaKill;

    public Ground ground;

    public Hero player;

    public static Camera camera;

    protected List<Enemy> enemies;

    protected List<Coin> coins;

    public List<TurningPoint> aTurns;

    public Rectangle upperArrow, lowerArrow, rightArrow, leftArrow;

    public Level(int id, Vector3 startPoint, Vector3 endPoint, Model ground, Hero player)
    {
        this.goodLoad = false;
        this.id = id;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.endPointMatrix = Matrix.CreateTranslation(this.endPoint);
        this.ground = new Ground(ground, new Vector3(0, -5, 0));
        this.player = player;

        this.upperArrow = new Rectangle(Settings.gManager.PreferredBackBufferWidth / 2 - Settings.upperArrow.Width / 2, Settings.gManager.PreferredBackBufferHeight / 4 - Settings.upperArrow.Height / 2, Settings.upperArrow.Width, Settings.upperArrow.Height);
        this.lowerArrow = new Rectangle(Settings.gManager.PreferredBackBufferWidth / 2 - Settings.lowerArrow.Width / 2, Settings.gManager.PreferredBackBufferHeight / 2 + Settings.gManager.PreferredBackBufferHeight / 4 - Settings.lowerArrow.Height / 2, Settings.lowerArrow.Width, Settings.lowerArrow.Height);
        this.rightArrow = new Rectangle(Settings.gManager.PreferredBackBufferWidth / 2 + Settings.gManager.PreferredBackBufferWidth / 4 - Settings.rightArrow.Width / 2, Settings.gManager.PreferredBackBufferHeight / 2 - Settings.rightArrow.Height / 2, Settings.rightArrow.Width, Settings.rightArrow.Height);
        this.leftArrow = new Rectangle(Settings.gManager.PreferredBackBufferWidth / 4 - Settings.leftArrow.Width / 2, Settings.gManager.PreferredBackBufferHeight / 2 - Settings.leftArrow.Height / 2, Settings.leftArrow.Width, Settings.leftArrow.Height);

        this.Initialize();
    }
    public void Initialize()
    {
        camera = new Camera();
        this.player.pos = startPoint;
        this.player.Initialize(this.startPoint);
        this.player.matrix = Matrix.CreateTranslation(this.player.pos);
        this.openEndMessageTimer = 100;
        this.coins = new List<Coin>();
        this.enemies = new List<Enemy>();
        this.aTurns = new List<TurningPoint>();
        LoadCoins();
        LoadEnemies();
        LoadTurns();
        LoadGroundBorders();
        isCollide = false;
        this.done = false;
        this.alphaKill = 0;

        if (this.aTurns.Count == 0 || this.coins.Count == 0 || this.ground.boxes.Count == 0 || this.enemies.Count == 0) this.goodLoad = false;
        else this.goodLoad = true;
    }

    public List<Coin> GetCoins()
    {
        return this.coins;
    }

    public List<Enemy> GetEnemies()
    {
        return this.enemies;
    }
    public bool IsDone
    {
        get { return this.done; }
    }

    public virtual void UnloadContent()
    {
        this.coins = null;
        this.enemies = null;
        this.aTurns = null;
        this.ground = null;
    }

    public virtual void LoadTurns()
    {

    }

    public virtual void LoadGroundBorders()
    {

    }

    public virtual void LoadEnemies()
    {

    }

    public virtual void LoadCoins()
    {

    }

    public void Done()
    {
        this.done = true;
    }
    public bool isCollide;
    public bool checkRotation = false;
    public virtual void Update()
    {
        if (this.goodLoad)
        {
            if (Settings.keyBoard.IsKeyDown(Keys.I))
                this.player.Initialize(this.startPoint);
            isCollide = false;
            for (int i = 0; i < this.ground.boxes.Count && !isCollide; i++)
            {
                if (this.ground.boxes[i].Intersects(this.player.box.boundingBox))
                    isCollide = true;
            }
            if (!isCollide)
                this.player.velocity.Y -= 0.06f;
            if (this.player.pos.Y <= -50)
                this.player.healthBar.LoseHealth(200f);
            if (this.player.pos.Y <= -5)
                this.alphaKill += 0.01f;

            if (this.coins.Count == 0 && this.enemies.Count == 0)
                if (Functions.InRadius(this.player.pos, this.endPoint, 2.5f))
                    this.Done();
        }
        
        this.checkRotation = false;
        isCollide = true;
        this.player.Update();

        for (int i = 0; i < this.aTurns.Count && !checkRotation; i++)
        {
            if (this.aTurns[i].IsCollide(this.player.box))
            {
                checkRotation = true;
                if (camera.direction == Camera.Facing.WhileChange)
                {
                    this.player.pos = this.aTurns[i].position;
                    this.player.velocity = Vector3.Zero;
                }
            }
        }
        
        camera.Update(this.player.pos, checkRotation);

        if (this.player.healthBar.healthValue < 0)
            MenuManager.gameState = GameState.Lost;

        for (int i = 0; i < this.enemies.Count && this.enemies.Count != 0; i++)
        {
            this.enemies[i].Update();
            for (int j = 0; j < this.player.gun.bullets.Count && this.player.gun.bullets.Count != 0; j++)
            {
                if (Functions.IsCollision(this.enemies[i].model, Gun.model, this.enemies[i].matrix, this.player.gun.bullets[j].matrix))
                {
                    char c = 'N';
                    switch (enemies[i].moveOnAxis)
                    {
                        case 'X': c = Functions.RightLeftX(this.player.gun.bullets[j].position, this.enemies[i].pos, 2.5f);
                            if (c == 'R') this.enemies[i].velocity.X += 2;
                            else this.enemies[i].velocity.X -= 2;
                            break;
                        case 'Z': c = Functions.RightLeftZ(this.player.gun.bullets[j].position, this.enemies[i].pos, 2.5f);
                            if (c == 'R') this.enemies[i].velocity.Z += 2;
                            else this.enemies[i].velocity.Z -= 2;
                            break;
                    }
                    this.player.gun.bullets.Remove(this.player.gun.bullets[j]);
                    this.enemies[i].healthBar.LoseHealth(10f);
                }
            }
            if (enemies[i].healthBar.healthValue <= 0) this.enemies.Remove(this.enemies[i]);
        }

        if (this.coins.Count != 0 || this.coins != null)
        {
            this.coins.ForEach(delegate(Coin c)
            {
                c.Update();
                if (c.boundigSphere.Intersects(this.player.box.boundingBox))
                    c.collected = true;
                if (c.collected == true)
                {
                    this.coins.Remove(c);
                }
            });
        }

    }

    public virtual void Draw3D()
    {
        if (this.goodLoad)
        {
            this.ground.Draw();
            player.Draw3D();
            if (this.coins.Count == 0 && this.enemies.Count == 0) Functions.DrawModel(this.player.model, this.endPointMatrix, camera, Color.White.ToVector3());
            for (int i = 0; i < this.enemies.Count && this.enemies.Count != 0; i++) this.enemies[i].Draw3D(camera);
            this.coins.ForEach(delegate(Coin c) { c.Draw(); });
            this.aTurns.ForEach(delegate(TurningPoint t) { if (Settings.debug) t.BoundingBox.Draw(Matrix.CreateTranslation(t.position)); });
            Functions.FadeBackBufferToBlack(this.alphaKill, Settings.gDevice, Settings.spriteBatch, Color.Red);
        }
    }
    public virtual void Draw2D()
    {
        if (this.goodLoad)
        {

            #region UpperUi

            Functions.FadeBackBufferToBlackOn(0.3f, Settings.spriteBatch, new Rectangle(0, 0, Settings.gManager.PreferredBackBufferWidth, 65));
            Settings.StartDraw();
            Settings.spriteBatch.DrawString(Settings.bigFont, "Level " + this.id, new Vector2(Settings.gManager.PreferredBackBufferWidth / 2 - Settings.bigFont.MeasureString("Level " + this.id).X / 2, 10), Color.White);
            Settings.EndDraw();
            if (this.enemies.Count != 0) this.TheClosest().Draw2D();

            this.player.Draw2D();

            #endregion

            #region LeftUi

            Functions.FadeBackBufferToBlackOn(0.3f, Settings.spriteBatch, new Rectangle(0, 120, 70, 80));
            Rectangle coinR = new Rectangle(5, 130, Settings.coin.Width - 25, Settings.coin.Height - 25);
            Rectangle enemyR = new Rectangle(coinR.X, coinR.Y + 35, coinR.Width, coinR.Height);

            Settings.StartDraw();

            Settings.spriteBatch.Draw(Settings.coin, coinR, Color.White);
            Settings.spriteBatch.DrawString(Settings.menuFont, this.coins.Count.ToString(), new Vector2(Settings.coin.Width - 5, coinR.Y), Color.White);

            Settings.spriteBatch.Draw(Settings.enemy, enemyR, Color.White);
            Settings.spriteBatch.DrawString(Settings.menuFont, this.enemies.Count.ToString(), new Vector2(Settings.enemy.Width - 5, enemyR.Y), Color.White);

            Settings.EndDraw();

            #endregion

            #region PlayerUi

            if (MenuManager.gameState != GameState.Paused && Settings.game.IsMouseVisible)
            {
                Settings.StartDraw();
                if (this.checkRotation)
                {
                    if (this.upperArrow.Intersects(Settings.mouseRec)) Settings.spriteBatch.Draw(Settings.upperArrow, this.upperArrow, Color.White);
                    else Settings.spriteBatch.Draw(Settings.upperArrow, this.upperArrow, Color.Gray);
                    if (this.lowerArrow.Intersects(Settings.mouseRec)) Settings.spriteBatch.Draw(Settings.lowerArrow, this.lowerArrow, Color.White);
                    else Settings.spriteBatch.Draw(Settings.lowerArrow, this.lowerArrow, Color.Gray);
                }
                if (this.leftArrow.Intersects(Settings.mouseRec)) Settings.spriteBatch.Draw(Settings.leftArrow, this.leftArrow, Color.White);
                else Settings.spriteBatch.Draw(Settings.leftArrow, this.leftArrow, Color.Gray);
                if (this.rightArrow.Intersects(Settings.mouseRec)) Settings.spriteBatch.Draw(Settings.rightArrow, this.rightArrow, Color.White);
                else Settings.spriteBatch.Draw(Settings.rightArrow, this.rightArrow, Color.Gray);

                Settings.EndDraw();
            }

            #endregion

            if ((this.openEndMessageTimer >= 0) && (this.coins.Count <= 0) && (this.enemies.Count <= 0))
            {
                Functions.DisplayMessage("End portal is now open!");
                this.openEndMessageTimer--;
            }
        }
        else { Functions.DisplayMessage("Bad Load: Try restart the game"); }
        checkRotation = false;
    }

    private Enemy TheClosest()
    {
        if (this.enemies.Count != 0)
        {
            Enemy best = enemies[0];

            for (int i = 1; i < this.enemies.Count; i++)
            {

                float xBestPlayer = Math.Abs(best.pos.X - this.player.pos.X);
                float zBestPlayer = Math.Abs(best.pos.Z - this.player.pos.Z);
                float xEnemyPlayer = Math.Abs(this.enemies[i].pos.X - this.player.pos.X);
                float zEnemyPlayer = Math.Abs(this.enemies[i].pos.Z - this.player.pos.Z);



                float distanceBest = MathHelper.Distance(xBestPlayer, xEnemyPlayer);
                float distanceEnemy = MathHelper.Distance(zEnemyPlayer, zBestPlayer);

                if (distanceEnemy > distanceBest)
                    best = enemies[i];

            }
            return best;
        }
        return null;
    }
}
