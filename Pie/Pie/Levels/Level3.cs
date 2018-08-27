using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class Level3 : Level
{
    public Level3(int id, Vector3 startPoint, Vector3 endPoint, Model ground, Hero player)
        : base(id, startPoint, endPoint, ground, player)
    {
        this.id = id;
    }

    public override void LoadGroundBorders()
    {
        this.ground.boxes.Add(new BoundingBox(new Vector3(-23.6f, 0, 0), new Vector3(23.6f, 50, 1.2f)));
        this.ground.boxes.Add(new BoundingBox(new Vector3(23.6f, 0, -35f), new Vector3(24f, 50, 35f)));
        this.ground.boxes.Add(new BoundingBox(new Vector3(23.6f, 0, 34f), new Vector3(58f, 50, 35f)));
        this.ground.boxes.Add(new BoundingBox(new Vector3(23.6f, 0, -35f), new Vector3(58f, 50, -34f)));
        this.ground.boxes.Add(new BoundingBox(new Vector3(23.6f, 0, -35f), new Vector3(58f, 50, 35f)));
        this.ground.boxes.Add(new BoundingBox(new Vector3(58f, 0, -1f), new Vector3(83f, 50, 1f)));
        this.ground.boxes.Add(new BoundingBox(new Vector3(-23.4f, 0, -35f), new Vector3(-22.7f, 50, 35f)));
        this.ground.boxes.Add(new BoundingBox(new Vector3(-65f, 0, -35.4f), new Vector3(-23.4f, 50, -34.6f)));
        this.ground.boxes.Add(new BoundingBox(new Vector3(-65f, 0, 35.4f), new Vector3(-23.4f, 50, 34.6f)));
        this.ground.boxes.Add(new BoundingBox(new Vector3(-65f, 0, -35f), new Vector3(-64f, 50, 35f)));
        this.ground.boxes.Add(new BoundingBox(new Vector3(-116f, 0, 22.4f), new Vector3(-63.6f, 50, 22.7f)));


    }

    public override void LoadTurns()
    {
        this.aTurns.Add(new TurningPoint(new Vector3(24f, 0, 0), true, false, true, false));
        this.aTurns.Add(new TurningPoint(new Vector3(24f, 0, -35), true, false, true, false));
        this.aTurns.Add(new TurningPoint(new Vector3(24f, 0, 34.4f), true, false, true, false));
        this.aTurns.Add(new TurningPoint(new Vector3(57.5f, 0, 34.4f), true, false, true, false));
        this.aTurns.Add(new TurningPoint(new Vector3(57.5f, 0, -35f), true, false, true, false));
        this.aTurns.Add(new TurningPoint(new Vector3(58f, 0, 0f), true, false, true, false));
        this.aTurns.Add(new TurningPoint(new Vector3(-23.2f, 0, 0f), true, false, true, false));
        this.aTurns.Add(new TurningPoint(new Vector3(-23.2f, 0, -35f), true, false, true, false));
        this.aTurns.Add(new TurningPoint(new Vector3(-23.2f, 0, 35f), true, false, true, false));
        this.aTurns.Add(new TurningPoint(new Vector3(-63.6f, 0, -35f), true, false, true, false));
        this.aTurns.Add(new TurningPoint(new Vector3(-63.6f, 0, 35f), true, false, true, false));
        this.aTurns.Add(new TurningPoint(new Vector3(-63.6f, 0, 22.5f), true, false, true, false));

    }

    public override void LoadCoins()
    {
        this.coins.Add(new Coin(new Vector3(-70, 0, 22.5f)));
        this.coins.Add(new Coin(new Vector3(-100, 4, 22.5f)));
        this.coins.Add(new Coin(new Vector3(-63.6f, 2, -12.9f)));
        this.coins.Add(new Coin(new Vector3(-63.6f, 4, -15.9f)));
        this.coins.Add(new Coin(new Vector3(-63.6f, 4, -34.3f)));
        this.coins.Add(new Coin(new Vector3(-63.6f, 0, -34.3f)));
        this.coins.Add(new Coin(new Vector3(-44f, 5, -35f)));
        this.coins.Add(new Coin(new Vector3(-23.2f, 0, -20.6f)));
        this.coins.Add(new Coin(new Vector3(-23.2f, 4, 12.6f)));
        this.coins.Add(new Coin(new Vector3(-23.2f, 4, 37.5f)));
        this.coins.Add(new Coin(new Vector3(-8.4f, 5, 0f)));
        this.coins.Add(new Coin(new Vector3(24f, 4, -21f)));
        this.coins.Add(new Coin(new Vector3(42f, 4, -35f)));
        this.coins.Add(new Coin(new Vector3(57.5f, 0, -22.2f)));
        this.coins.Add(new Coin(new Vector3(57.5f, 0, 24f)));

        base.LoadCoins();
    }

    public override void LoadEnemies()
    {
        this.enemies.Add(new NormalEnemy());
        this.enemies.Add(new DashingEnemy());
        this.enemies.Add(new NormalEnemy());
        this.enemies.Add(new JumpingEnemy());
        this.enemies.Add(new NormalEnemy());
        this.enemies.Add(new DashingEnemy());
        this.enemies.Add(new JumpingEnemy());
        this.enemies.Add(new DashingEnemy());
        this.enemies.Add(new JumpingEnemy());
        this.enemies.Add(new NormalEnemy());

        this.enemies[0].LoadPlaceToMove(new Vector3(23.6f, 0, -35f), new Vector3(23.6f, 0, 35f), 'Z');
        this.enemies[1].LoadPlaceToMove(new Vector3(23.6f, 0, -35f), new Vector3(23.6f, 0, 35f), 'Z');
        this.enemies[2].LoadPlaceToMove(new Vector3(23.6f, 0, 35f), new Vector3(58f, 0, 35f), 'X');
        this.enemies[3].LoadPlaceToMove(new Vector3(23.6f, 0, -35f), new Vector3(58f, 0, -35f), 'Z');
        this.enemies[4].LoadPlaceToMove(new Vector3(58f, 0, 0f), new Vector3(83f, 0, 0), 'X');
        this.enemies[5].LoadPlaceToMove(new Vector3(-23.2f, 0, -35f), new Vector3(-23.2f, 0, 35f), 'Z');
        this.enemies[6].LoadPlaceToMove(new Vector3(-65f, 0, -35f), new Vector3(-65f, 0, 35f), 'Z');
        this.enemies[7].LoadPlaceToMove(new Vector3(-116f, 0, 22.4f), new Vector3(-63.6f, 0, 22.4f), 'X');
        this.enemies[8].LoadPlaceToMove(new Vector3(-116f, 0, 22.4f), new Vector3(-63.6f, 0, 22.4f), 'X');
        this.enemies[9].LoadPlaceToMove(new Vector3(-116f, 0, 22.4f), new Vector3(-63.6f, 0, 22.4f), 'X');

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