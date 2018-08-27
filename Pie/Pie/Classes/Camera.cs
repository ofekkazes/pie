#region Using Statemants

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#endregion

public class Camera
{
    #region Variables

    private Matrix viewMatrix;
    private Matrix projectionMatrix;
    private float distance;

    public Vector3 position;
    private Vector3 targetPosition;

    private float arc;

    private bool rightRotate;
    private bool leftRotate;
    private float rotationAngle;
    private int rotationAngleCounter;

    public Facing direction;

    #endregion

    #region Properties

    public Matrix ViewMatrix
    {
        get { return this.viewMatrix; }
        set { this.viewMatrix = value; }
    }

    public Matrix ProjectionMatrix
    {
        get { return this.projectionMatrix; }
        set { this.projectionMatrix = value; }
    }

    public float DistanceFromObject
    {
        get { return this.distance; }
        set { this.distance = MathHelper.Clamp(value, 0.01f, 500f); }
    }

    public Vector3 TargetPosition
    {
        get { return this.targetPosition; }
        set { this.targetPosition = value; }
    }

    public float RotationAngle
    {
        get { return this.rotationAngle; }
        set { this.rotationAngle = value; }
    }

    public float Arc
    {
        get { return this.arc; }
        set { this.arc = value; }
    }


    #endregion

    #region Constructor

    public Camera()
    {
        this.distance = 20f;
        this.rotationAngleCounter = 0;
        this.arc = 10;
        this.rotationAngle = 0;

        this.direction = Facing.North;
        this.position = Vector3.Zero;

    }

    #endregion

    #region Public Mothods

    public void Update(Vector3 targetPosition, bool isRotation)
    {
        if (isRotation)
        {
            if (LevelManager.levels[LevelManager.currentLevelID].upperArrow.Intersects(Settings.mouseRec) || Settings.keyBoard.IsKeyDown(Keys.W) || Settings.keyBoard.IsKeyDown(Keys.Up) || rightRotate)
                rotationAngleUpdateRight();
            else if (LevelManager.levels[LevelManager.currentLevelID].lowerArrow.Intersects(Settings.mouseRec) || Settings.keyBoard.IsKeyDown(Keys.S) || Settings.keyBoard.IsKeyDown(Keys.Down) || leftRotate)
                rotationAngleUpdateLeft();
        }
        CheckDirection();

        Vector3 thirdPersonReference = new Vector3(0, this.arc, this.distance);
        Vector3 transformedReference = Vector3.Transform(thirdPersonReference, Matrix.CreateRotationY(MathHelper.ToRadians(this.rotationAngle)));
        this.position = transformedReference + targetPosition;

        this.viewMatrix = Matrix.CreateLookAt(this.position, targetPosition, Vector3.Up);

        this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                    Settings.gDevice.Viewport.AspectRatio,
                                                                    1,
                                                                    10000);
    }

    #endregion

    #region Private Methods

    private void rotationAngleUpdateRight()
    {
        if (!leftRotate)
        {
            if (this.rotationAngleCounter >= 45)
            {
                this.rightRotate = false;
                this.rotationAngleCounter = 0;
            }
            if (Settings.KeyPress(Keys.Up) || Settings.KeyPress(Keys.W) || Settings.MouseClick(true)) this.rightRotate = true;

            if (this.rightRotate && this.rotationAngleCounter < 45)
            {
                this.rotationAngle += 2;
                this.rotationAngleCounter++;
                LevelManager.levels[LevelManager.currentLevelID].checkRotation = true;
            }
        }
    }
    private void rotationAngleUpdateLeft()
    {
        if (!rightRotate)
        {
            if (this.rotationAngleCounter >= 45)
            {
                this.leftRotate = false;
                this.rotationAngleCounter = 0;
            }
            if (Settings.KeyPress(Keys.Down) || Settings.KeyPress(Keys.S) || Settings.MouseClick(true)) this.leftRotate = true;

            if (this.leftRotate && this.rotationAngleCounter < 45)
            {
                this.rotationAngle -= 2;
                this.rotationAngleCounter++;
                LevelManager.levels[LevelManager.currentLevelID].checkRotation = true;
            }
        }
    }

    private void CheckDirection()
    {
        if (this.rotationAngle >= 360 || this.rotationAngle <= -360)
            this.rotationAngle = 0;

        if (this.rotationAngle == 0)
            this.direction = Facing.North;
        else if (this.rotationAngle == 270 || this.rotationAngle == -90)
            this.direction = Facing.East;
        else if (this.rotationAngle == -270 || this.rotationAngle == 90)
            this.direction = Facing.West;
        else if (this.rotationAngle == 180 || this.rotationAngle == -180)
            this.direction = Facing.South;
        else this.direction = Facing.WhileChange;
    }

    #endregion

    #region Enums

    public enum Facing
    {
        North = 0,
        East = 1,
        West = 2,
        South = 4,
        WhileChange = 5
    }

    #endregion

}
