using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

public static class Functions
{
    public static bool CheckForCollisions(Model c1Model, Vector3 c1Pos, Model c2Model, Vector3 c2Pos)
    {
        for (int i = 0; i < c1Model.Meshes.Count; i++)
        {
            // Check whether the bounding boxes of the two cubes intersect.
            BoundingSphere c1BoundingSphere = c1Model.Meshes[i].BoundingSphere;

            c1BoundingSphere.Center += c1Pos * 3;

            for (int j = 0; j < c2Model.Meshes.Count; j++)
            {
                BoundingSphere c2BoundingSphere = c2Model.Meshes[j].BoundingSphere;
                c2BoundingSphere.Center += c2Pos * 3;

                if (c1BoundingSphere.Intersects(c2BoundingSphere))
                {
                    return true;
                }
            }
        }
        return false;
    }


    public static void DrawModel(Model model, Matrix modelMatrix, Camera camera, Vector3 ambientColor)
    {
        Matrix[] modelTransforms = new Matrix[model.Bones.Count];
        model.CopyAbsoluteBoneTransformsTo(modelTransforms);

        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.EnableDefaultLighting();
                effect.World = modelTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(modelMatrix.Translation);
                effect.View = camera.ViewMatrix;
                effect.Projection = camera.ProjectionMatrix;
                effect.LightingEnabled = true;
                effect.AmbientLightColor = ambientColor;
                effect.DirectionalLight0.Direction = Vector3.Zero;
                effect.DirectionalLight0.DiffuseColor = new Vector3(0, 0, 0.5f);
                effect.DirectionalLight0.Enabled = true;
                effect.PreferPerPixelLighting = true;
            }
            mesh.Draw();
        }
    }
    public static void DrawModel(Model model, Matrix modelMatrix, Matrix rotation, Camera camera, Vector3 ambientColor)
    {
        Matrix[] modelTransforms = new Matrix[model.Bones.Count];
        model.CopyAbsoluteBoneTransformsTo(modelTransforms);

        foreach (ModelMesh mesh in model.Meshes)
        {
            Matrix world = rotation * modelTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(modelMatrix.Translation);

            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.EnableDefaultLighting();
                effect.World = world;
                effect.View = camera.ViewMatrix;
                effect.Projection = camera.ProjectionMatrix;
                effect.LightingEnabled = true;
                effect.AmbientLightColor = ambientColor;
                effect.DirectionalLight0.Direction = Vector3.Zero;
                effect.DirectionalLight0.DiffuseColor = new Vector3(0, 0, 0.5f);
                effect.DirectionalLight0.Enabled = true;
                effect.PreferPerPixelLighting = true;
            }
            mesh.Draw();
        }
    }



    public static void FadeBackBufferToBlack(float alpha, GraphicsDevice g, SpriteBatch spriteBatch)
    {
        Viewport viewport = g.Viewport;

        spriteBatch.Begin();

        spriteBatch.Draw(Settings.blankTexture,
                         new Rectangle(0, 0, viewport.Width, viewport.Height),
                         Color.Black * alpha);

        spriteBatch.End();
    }
    public static void FadeBackBufferToBlack(float alpha, GraphicsDevice g, SpriteBatch spriteBatch, Color c)
    {
        Viewport viewport = g.Viewport;

        spriteBatch.Begin();

        spriteBatch.Draw(Settings.blankTexture,
                         new Rectangle(0, 0, viewport.Width, viewport.Height),
                         c * alpha);

        spriteBatch.End();
    }
    public static void FadeBackBufferToBlackOn(float alpha, SpriteBatch spriteBatch, Rectangle size)
    {

        spriteBatch.Begin();

        spriteBatch.Draw(Settings.blankTexture,
                         size,
                         Color.Black * alpha);

        spriteBatch.End();
    }

    public static bool IsCollision(Model model1, Model model2, Matrix matrix1, Matrix matrix2)
    {
        for (int i = 0; i < model1.Meshes.Count; i++)
        {
            // Check whether the bounding boxes of the two cubes intersect.
            BoundingSphere c1BoundingSphere = model1.Meshes[i].BoundingSphere;
            c1BoundingSphere.Center += matrix1.Translation;
            BoundingBox box1 = BoundingBox.CreateFromSphere(c1BoundingSphere);

            for (int j = 0; j < model2.Meshes.Count; j++)
            {
                BoundingSphere c2BoundingSphere = model2.Meshes[j].BoundingSphere;
                c2BoundingSphere.Center += matrix2.Translation;
                BoundingBox box2 = BoundingBox.CreateFromSphere(c2BoundingSphere);

                if (box1.Intersects(box2))
                    return true;
            }
        }
        return false;
    }

    public static void ShowPauseScreen()
    {
        string pauseText = "You are in pause mode.";
        string pauseReturn = "To return to the game- Press P";
        string pauseExit = "If you want to leave the game- Press Escape";
        int counter = 0;
        Settings.spriteBatch.DrawString(Settings.menuFont, pauseText, new Vector2(Settings.gManager.PreferredBackBufferWidth / 2 - Settings.menuFont.MeasureString(pauseText).X / 2,
            Settings.gManager.PreferredBackBufferHeight / 2 + counter), Color.White);
        counter += 20;
        Settings.spriteBatch.DrawString(Settings.menuFont, pauseReturn, new Vector2(Settings.gManager.PreferredBackBufferWidth / 2 - Settings.menuFont.MeasureString(pauseReturn).X / 2,
            Settings.gManager.PreferredBackBufferHeight / 2 + counter), Color.White);
        counter += 20;
        Settings.spriteBatch.DrawString(Settings.menuFont, pauseExit, new Vector2(Settings.gManager.PreferredBackBufferWidth / 2 - Settings.menuFont.MeasureString(pauseExit).X / 2,
            Settings.gManager.PreferredBackBufferHeight / 2 + counter), Color.White);
        counter += 20;
    }

    /// <summary>
    /// returns 'R' if the victim is from the right, 'L' if the victim is from the left.
    /// or 'N' if not in radius.
    /// Only Works in X & Z axis.
    /// </summary>
    /// <param name="head"></param>
    /// <param name="victim"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static bool InRadius(Vector3 head, Vector3 victim, float radius)
    {
        //Checks if the victim X axis and Z axis is inside the radius of the head
        if (head.X + radius >= victim.X && victim.X >= head.X)
        {
            if (head.Z + radius >= victim.Z && victim.Z >= head.Z)
                return true;
            else if (head.Z - radius <= victim.Z && head.Z >= victim.Z)
                return true;
        }
        else if (head.X - radius <= victim.X && head.X > victim.X)
        {
            if (head.Z + radius >= victim.Z && victim.Z >= head.Z)
                return true;
            else if (head.Z - radius <= victim.Z && head.Z >= victim.Z)
                return true;
        }

        return false;
    }

    /// <summary>
    /// returns 'R' if the victim is from the right, 'L' if the victim is from the left.
    /// or 'N' if not in radius.
    /// Only Works in Y Upper axis.
    /// </summary>
    /// <param name="head"></param>
    /// <param name="victim"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static char InRadiusCharY_UpX(Vector3 head, Vector3 victim, float radius)
    {
        //Checks if the victim X axis and Z axis is inside the radius of the head
        if ((head.Y + radius >= victim.Y && victim.Y <= head.Y))
        {
            if (head.X + radius >= victim.X && victim.X >= head.X)
            {
                if (head.Z + radius >= victim.Z && victim.Z >= head.Z)
                    return 'R';
                else if (head.Z - radius <= victim.Z && head.Z >= victim.Z)
                    return 'R';
            }
            else if (head.X - radius <= victim.X && head.X > victim.X)
            {
                if (head.Z + radius >= victim.Z && victim.Z >= head.Z)
                    return 'L';
                else if (head.Z - radius <= victim.Z && head.Z >= victim.Z)
                    return 'L';
            }
        }
        return 'N';
    }
    public static char InRadiusCharY_UpZ(Vector3 head, Vector3 victim, float radius)
    {
        //Checks if the victim X axis and Z axis is inside the radius of the head
        if ((head.Y + radius >= victim.Y && victim.Y <= head.Y))
        {
            if (head.Z - radius <= victim.Z && head.Z >= victim.Z)
            {
                if (head.X + radius >= victim.X && victim.X >= head.X)
                    return 'L';
                else if (head.X - radius <= victim.X && head.X >= victim.X)
                    return 'L';
            }
            else if (head.Z + radius >= victim.Z && head.Z <= victim.Z)
            {
                if (head.X + radius >= victim.X && victim.X >= head.X)
                    return 'R';
                else if (head.X - radius <= victim.X && head.X >= victim.X)
                    return 'R';
            }
        }
        return 'N';
    }

    public static char RightLeft(Vector3 head, Vector3 victim, char moveOnAxis)
    {
        if (moveOnAxis == 'X')
        {
            if (head.X <= victim.X)
                return 'R';
            else return 'L';
        }
        else if (moveOnAxis == 'Z')
        {
            if (head.Z <= victim.Z)
                return 'R';
            else return 'L';
        }
        return 'N';
    }
    public static char RightLeftZ(Vector3 head, Vector3 victim, float radius)
    {
        if (head.Z - radius <= victim.Z && head.Z >= victim.Z)
        {
            return 'L';
        }
        else if (head.Z + radius >= victim.Z && head.Z <= victim.Z)
        {
            return 'R';
        }
        return 'N';
    }
    public static char RightLeftX(Vector3 head, Vector3 victim, float radius)
    {
        if (head.X + radius >= victim.X && victim.X >= head.X)
        {
            return 'R';
        }
        else if (head.X - radius <= victim.X && head.X > victim.X)
        {
            return 'L';
        }
        return 'N';
    }

    /// <summary>
    /// returns 'R' if the victim is from the right, 'L' if the victim is from the left.
    /// or 'N' if not in radius.
    /// Only Works in Y Lower axis.
    /// </summary>
    /// <param name="head"></param>
    /// <param name="victim"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static char InRadiusCharY_Down(Vector3 head, Vector3 victim, float radius)
    {
        //Checks if the victim X axis and Z axis is inside the radius of the head
        if ((head.Y - radius <= victim.Y && victim.Y >= head.Y))
        {
            if (head.X + radius >= victim.X && victim.X >= head.X)
            {
                if (head.Z + radius >= victim.Z && victim.Z >= head.Z)
                    return 'R';
                else if (head.Z - radius <= victim.Z && head.Z >= victim.Z)
                    return 'R';
            }
            else if (head.X - radius <= victim.X && head.X > victim.X)
            {
                if (head.Z + radius >= victim.Z && victim.Z >= head.Z)
                    return 'L';
                else if (head.Z - radius <= victim.Z && head.Z >= victim.Z)
                    return 'L';
            }
        }
        return 'N';
    }

    public static void DisplayMessage(string message)
    {
        FadeBackBufferToBlack(0.3f, Settings.gDevice, Settings.spriteBatch);
        Settings.StartDraw();
        Settings.spriteBatch.DrawString(Settings.menuFont, message,
            new Vector2(Settings.gManager.PreferredBackBufferWidth / 2 - Settings.menuFont.MeasureString(message).X / 2,
                Settings.gManager.PreferredBackBufferHeight / 2 - Settings.menuFont.MeasureString(message).Y / 2),
                Color.White);
        Settings.EndDraw();
    }

    public static Model LoadModel(string modelName, Effect effect)
    {
        Model newModel = Settings.content.Load<Model>(modelName); foreach (ModelMesh mesh in newModel.Meshes)
            foreach (ModelMeshPart meshPart in mesh.MeshParts)
                meshPart.Effect = effect.Clone();
        return newModel;
    }
    public static BoundingSphere CreateBoundingSphere(Model model, Vector3 position)
    {
        BoundingSphere sphere = new BoundingSphere();

        foreach (ModelMesh mesh in model.Meshes)
        {
            if (sphere.Radius == 0)
                sphere = mesh.BoundingSphere;
            else
                sphere = BoundingSphere.CreateMerged(sphere, mesh.BoundingSphere);
        }

        sphere.Center = position;

        sphere.Radius *= 1;

        return sphere;
    }

}

