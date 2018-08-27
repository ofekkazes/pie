using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class Level2 : Level
{
    public Level2(int id, Vector3 startPoint, Vector3 endPoint, Model ground, Hero player)
        : base(id, startPoint, endPoint, ground, player)
    {
        this.id = id;
        
    }

    public override void LoadGroundBorders()
    {
        this.ground.boxes.Add(new BoundingBox(new Vector3(-40f, 0, 0), new Vector3(21f, 50, 1.2f)));
        this.ground.boxes.Add(new BoundingBox(new Vector3(17.6f, 0, -60), new Vector3(20, 50, 0)));
        this.ground.boxes.Add(new BoundingBox(new Vector3(-40f, 0, -54.4f), new Vector3(-37.5f, 50, 0)));
        this.ground.boxes.Add(new BoundingBox(new Vector3(-90f, 0, -28), new Vector3(-40f, 50, -27)));
        this.ground.boxes.Add(new BoundingBox(new Vector3(-90f, 0, -54), new Vector3(-40f, 50, -52.5f)));
        this.ground.boxes.Add(new BoundingBox(new Vector3(-91f, 0, -54), new Vector3(-89f, 50, 0.7f)));
        this.ground.boxes.Add(new BoundingBox(new Vector3(-88.8f, 0, -0.8f), new Vector3(-63f, 50, 2f)));
    }

    public override void LoadTurns()
    {
        this.aTurns.Add(new TurningPoint(new Vector3(18.4f, 0, 0), true, false, true, false)); 
        this.aTurns.Add(new TurningPoint(new Vector3(-38.8f, 0, 0), true, false, true, false)); 
        this.aTurns.Add(new TurningPoint(new Vector3(-38.8f, 0, -53.2f), true, false, true, false));
        this.aTurns.Add(new TurningPoint(new Vector3(-38.8f, 0, -27.6f), true, false, true, false));
        this.aTurns.Add(new TurningPoint(new Vector3(-88.8f, 0, -53.2f), true, false, true, false));
        this.aTurns.Add(new TurningPoint(new Vector3(-88.8f, 0, -27.6f), true, false, true, false));
        this.aTurns.Add(new TurningPoint(new Vector3(-88.8f, 0, -0.8f), true, false, true, false));
    }

    public override void LoadCoins()
    {
        this.coins.Add(new Coin(new Vector3(-78f, 0, 0.8f)));
        this.coins.Add(new Coin(new Vector3(23, 0, 0.6f)));
        this.coins.Add(new Coin(new Vector3(18.6f, 3, -25)));
        this.coins.Add(new Coin(new Vector3(18.6f, 3, -45)));
        this.coins.Add(new Coin(new Vector3(-38.8f, 5, -38)));
        this.coins.Add(new Coin(new Vector3(-50, 0, -27.6f)));
        this.coins.Add(new Coin(new Vector3(-70, 2.76f, -27.6f)));
        this.coins.Add(new Coin(new Vector3(-67, 5f, -53.2f)));
        this.coins.Add(new Coin(new Vector3(-42, 4f, -53.2f)));
        this.coins.Add(new Coin(new Vector3(-88.8f, 0, -34f)));
        this.coins.Add(new Coin(new Vector3(-88.8f, 0, -55f)));
        this.coins.Add(new Coin(new Vector3(-67.8f, 0, -0.8f)));
        this.coins.Add(new Coin(new Vector3(-22f, 0, -0.8f)));
        base.LoadCoins();
    }

    public override void LoadEnemies()
    {
        this.enemies.Add(new NormalEnemy());
        this.enemies.Add(new JumpingEnemy());
        this.enemies.Add(new NormalEnemy());
        this.enemies.Add(new NormalEnemy());
        this.enemies.Add(new DashingEnemy());
        this.enemies.Add(new NormalEnemy());

        this.enemies[0].LoadPlaceToMove(new Vector3(-90, 0, -0.8f), new Vector3(-63, 0, -0.8f), 'X'); // 
        this.enemies[1].LoadPlaceToMove(new Vector3(-90, 0, -0.8f), new Vector3(-63, 0, -0.8f), 'X'); // 
        this.enemies[2].LoadPlaceToMove(new Vector3(-90.4f, 0, -53.2f), new Vector3(-38f, 0, -53.2f), 'X'); // 
        this.enemies[3].LoadPlaceToMove(new Vector3(-90.4f, 0, -53.2f), new Vector3(-38f, 0, -53.2f), 'X'); // 
        this.enemies[4].LoadPlaceToMove(new Vector3(-90.4f, 0, -27.6f), new Vector3(-38f, 0, -27.6f), 'X'); //
        this.enemies[5].LoadPlaceToMove(new Vector3(-90.4f, 0, -27.6f), new Vector3(-38f, 0, -27.6f), 'X'); //


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