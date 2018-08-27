using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class Level1 : Level
{
    public Level1(int id, Vector3 startPoint, Vector3 endPoint, Model ground, Hero player)
        : base(id, startPoint, endPoint, ground, player)
    {
        this.id = id;
        
    }

    public override void LoadGroundBorders()
    {
        this.ground.AddBox(new BoundingBox(new Vector3(9f, 0, -8.8f), new Vector3(9f, 50f, 0)));
        this.ground.AddBox(new BoundingBox(new Vector3(-10f, 0, 0), new Vector3(9f, 50f, 0)));
        this.ground.AddBox(new BoundingBox(new Vector3(-10f, 0, -27.5f), new Vector3(-10f, 50f, 9f)));
        this.ground.AddBox(new BoundingBox(new Vector3(-10f, 0, -27.5f), new Vector3(18.8f, 50f, -27.5f)));
        this.ground.AddBox(new BoundingBox(new Vector3(18.8f, 0, -27.5f), new Vector3(18.8f, 50f, 13.5f)));
        this.ground.AddBox(new BoundingBox(new Vector3(-1.3f, 0, 13.5f), new Vector3(18.8f, 50f, 13.5f)));
    }

    public override void LoadTurns()
    {
        //Set by the developer
        this.aTurns.Add(new TurningPoint(new Vector3(-10, 0, 0), true, false, true, true));
        this.aTurns.Add(new TurningPoint(new Vector3(9, 0, 0), false, true, true, false));
        this.aTurns.Add(new TurningPoint(new Vector3(-10, 0, -27.5f), true, false, false, true));
        this.aTurns.Add(new TurningPoint(new Vector3(18.8f, 0, -27.5f), false, true, false, true));
        this.aTurns.Add(new TurningPoint(new Vector3(18.8f, 0, 13.5f), true, false, true, false));
    }

    public override void LoadCoins()
    {
        this.coins = new List<Coin>();
        this.coins.Add(new Coin(new Vector3(3.6f, 0, 0)));
        this.coins.Add(new Coin(new Vector3(-10f, 0, -8.8f)));
        this.coins.Add(new Coin(new Vector3(9f, 0, -5f)));
        this.coins.Add(new Coin(new Vector3(18.8f, 0, -18.8f)));
        this.coins.Add(new Coin(new Vector3(18.8f, 0, 8f)));
        this.coins.Add(new Coin(new Vector3(-2f, 0, -28f)));
        base.LoadCoins();
    }

    public override void LoadEnemies()
    {
        this.enemies.Add(new NormalEnemy());
        this.enemies.Add(new DashingEnemy());
        this.enemies.Add(new JumpingEnemy());
        this.enemies.Add(new JumpingEnemy());

        this.enemies[0].LoadPlaceToMove(new Vector3(-10f, 0, -27.5f), new Vector3(18.8f, 0, -27.5f), 'X');
        this.enemies[1].LoadPlaceToMove(new Vector3(-1.5f, 0, 13.5f), new Vector3(18.8f, 0, 13.5f), 'X');
        this.enemies[2].LoadPlaceToMove(new Vector3(-10f, 0, -27.5f), new Vector3(-10f, 0, 8.8f), 'Z');
        this.enemies[3].LoadPlaceToMove(new Vector3(18.8f, 0, -27.5f), new Vector3(18.8f, 0, 13.5f), 'Z');

        for (int i = 0; i < this.enemies.Count; i++)
        {
            this.enemies[i].StartMove();
        }

        base.LoadEnemies();
    }

    public override void Update()
    {
        base.Update();
    }
}

