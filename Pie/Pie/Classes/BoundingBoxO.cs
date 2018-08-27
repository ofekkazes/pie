using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

class BoundingBoxBuffers
{
    public VertexBuffer Vertices;
    public int VertexCount;
    public IndexBuffer Indices;
    public int PrimitiveCount;
}

public class BoundingBoxO
{
    public Model model;
    private BoundingBoxBuffers _buffers;
    private BasicEffect _effect;
    public BoundingBox boundingBox;

    public BoundingBoxO(Model m, Color c)
    {
        this.model = m;
        this.boundingBox = CreateBoundingBox(model);
        _buffers = CreateBoundingBoxBuffers(boundingBox, Settings.gDevice);

        _effect = new BasicEffect(Settings.gDevice);
        _effect.LightingEnabled = false;
        _effect.TextureEnabled = false;
        _effect.VertexColorEnabled = true;
        _effect.DiffuseColor = c.ToVector3();

    }
    private static BoundingBox? GetBoundingBox(ModelMeshPart meshPart, Matrix transform)
    {
        if (meshPart.VertexBuffer == null)
            return null;

        Vector3[] positions = GetVertexElement(meshPart, VertexElementUsage.Position);
        if (positions == null)
            return null;

        Vector3[] transformedPositions = new Vector3[positions.Length];
        Vector3.Transform(positions, ref transform, transformedPositions);

        return BoundingBox.CreateFromPoints(transformedPositions);
    }
    private static BoundingBox CreateBoundingBox(Model model)
    {
        Matrix[] boneTransforms = new Matrix[model.Bones.Count];
        model.CopyAbsoluteBoneTransformsTo(boneTransforms);

        BoundingBox result = new BoundingBox();
        foreach (ModelMesh mesh in model.Meshes)
            foreach (ModelMeshPart meshPart in mesh.MeshParts)
            {
                BoundingBox? meshPartBoundingBox = GetBoundingBox(meshPart, boneTransforms[mesh.ParentBone.Index]);
                if (meshPartBoundingBox != null)
                    result = BoundingBox.CreateMerged(result, meshPartBoundingBox.Value);
            }
        return result;
    }
    public static Vector3[] GetVertexElement(ModelMeshPart meshPart, VertexElementUsage usage)
    {
        VertexDeclaration vd = meshPart.VertexBuffer.VertexDeclaration;
        VertexElement[] elements = vd.GetVertexElements();

        Func<VertexElement, bool> elementPredicate = ve => ve.VertexElementUsage == usage && ve.VertexElementFormat == VertexElementFormat.Vector3;
        if (!elements.Any(elementPredicate))
            return null;

        VertexElement element = elements.First(elementPredicate);

        Vector3[] vertexData = new Vector3[meshPart.NumVertices];
        meshPart.VertexBuffer.GetData((meshPart.VertexOffset * vd.VertexStride) + element.Offset,
        vertexData, 0, vertexData.Length, vd.VertexStride);

        return vertexData;
    }
    private BoundingBoxBuffers CreateBoundingBoxBuffers(BoundingBox boundingBox, GraphicsDevice graphicsDevice)
    {
        BoundingBoxBuffers boundingBoxBuffers = new BoundingBoxBuffers();

        boundingBoxBuffers.PrimitiveCount = 24;
        boundingBoxBuffers.VertexCount = 48;

        VertexBuffer vertexBuffer = new VertexBuffer(graphicsDevice,
        typeof(VertexPositionColor), boundingBoxBuffers.VertexCount,
        BufferUsage.WriteOnly);
        List<VertexPositionColor> vertices = new List<VertexPositionColor>();

        const float ratio = 5.0f;

        Vector3 xOffset = new Vector3((boundingBox.Max.X - boundingBox.Min.X) / ratio, 0, 0);
        Vector3 yOffset = new Vector3(0, (boundingBox.Max.Y - boundingBox.Min.Y) / ratio, 0);
        Vector3 zOffset = new Vector3(0, 0, (boundingBox.Max.Z - boundingBox.Min.Z) / ratio);
        Vector3[] corners = boundingBox.GetCorners();

        // Corner 1.
        vertices.Add(new VertexPositionColor(corners[0], Color.White));
        vertices.Add(new VertexPositionColor(corners[0] + xOffset, Color.White));
        vertices.Add(new VertexPositionColor(corners[0], Color.White));
        vertices.Add(new VertexPositionColor(corners[0] - yOffset, Color.White));
        vertices.Add(new VertexPositionColor(corners[0], Color.White));
        vertices.Add(new VertexPositionColor(corners[0] - zOffset, Color.White));

        // Corner 2.
        vertices.Add(new VertexPositionColor(corners[1], Color.White));
        vertices.Add(new VertexPositionColor(corners[1] - xOffset, Color.White));
        vertices.Add(new VertexPositionColor(corners[1], Color.White));
        vertices.Add(new VertexPositionColor(corners[1] - yOffset, Color.White));
        vertices.Add(new VertexPositionColor(corners[1], Color.White));
        vertices.Add(new VertexPositionColor(corners[1] - zOffset, Color.White));

        // Corner 3.
        vertices.Add(new VertexPositionColor(corners[2], Color.White));
        vertices.Add(new VertexPositionColor(corners[2] - xOffset, Color.White));
        vertices.Add(new VertexPositionColor(corners[2], Color.White));
        vertices.Add(new VertexPositionColor(corners[2] + yOffset, Color.White));
        vertices.Add(new VertexPositionColor(corners[2], Color.White));
        vertices.Add(new VertexPositionColor(corners[2] - zOffset, Color.White));

        // Corner 4.
        vertices.Add(new VertexPositionColor(corners[3], Color.White));
        vertices.Add(new VertexPositionColor(corners[3] + xOffset, Color.White));
        vertices.Add(new VertexPositionColor(corners[3], Color.White));
        vertices.Add(new VertexPositionColor(corners[3] + yOffset, Color.White));
        vertices.Add(new VertexPositionColor(corners[3], Color.White));
        vertices.Add(new VertexPositionColor(corners[3] - zOffset, Color.White));


        // Corner 5.
        vertices.Add(new VertexPositionColor(corners[4], Color.White));
        vertices.Add(new VertexPositionColor(corners[4] + xOffset, Color.White));
        vertices.Add(new VertexPositionColor(corners[4], Color.White));
        vertices.Add(new VertexPositionColor(corners[4] - yOffset, Color.White));
        vertices.Add(new VertexPositionColor(corners[4], Color.White));
        vertices.Add(new VertexPositionColor(corners[4] + zOffset, Color.White));
        
        // Corner 6.
        vertices.Add(new VertexPositionColor(corners[5], Color.White));
        vertices.Add(new VertexPositionColor(corners[5] - xOffset, Color.White));
        vertices.Add(new VertexPositionColor(corners[5], Color.White));
        vertices.Add(new VertexPositionColor(corners[5] - yOffset, Color.White));
        vertices.Add(new VertexPositionColor(corners[5], Color.White));
        vertices.Add(new VertexPositionColor(corners[5] + zOffset, Color.White));

        // Corner 7.
        vertices.Add(new VertexPositionColor(corners[6], Color.White));
        vertices.Add(new VertexPositionColor(corners[6] - xOffset, Color.White));
        vertices.Add(new VertexPositionColor(corners[6], Color.White));
        vertices.Add(new VertexPositionColor(corners[6] + yOffset, Color.White));
        vertices.Add(new VertexPositionColor(corners[6], Color.White));
        vertices.Add(new VertexPositionColor(corners[6] + zOffset, Color.White));

        // Corner 8.
        vertices.Add(new VertexPositionColor(corners[7], Color.White));
        vertices.Add(new VertexPositionColor(corners[7] + xOffset, Color.White));
        vertices.Add(new VertexPositionColor(corners[7], Color.White));
        vertices.Add(new VertexPositionColor(corners[7] + yOffset, Color.White));
        vertices.Add(new VertexPositionColor(corners[7], Color.White));
        vertices.Add(new VertexPositionColor(corners[7] + zOffset, Color.White));

        vertexBuffer.SetData(vertices.ToArray());
        boundingBoxBuffers.Vertices = vertexBuffer;

        IndexBuffer indexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, boundingBoxBuffers.VertexCount,
        BufferUsage.WriteOnly);
        indexBuffer.SetData(Enumerable.Range(0, boundingBoxBuffers.VertexCount).Select(i => (short)i).ToArray());
        boundingBoxBuffers.Indices = indexBuffer;

        return boundingBoxBuffers;
    }
    

    public void Draw(Matrix matrix)
    {
        Settings.gDevice.SetVertexBuffer(_buffers.Vertices);
        Settings.gDevice.Indices = _buffers.Indices;

        //ICameraService cameraService = (ICameraService)Game.Services.GetService(typeof(ICameraService));
        _effect.World = matrix;
        _effect.View = Level.camera.ViewMatrix;
        _effect.Projection = Level.camera.ProjectionMatrix;

        foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            Settings.gDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0,
            _buffers.VertexCount, 0, _buffers.PrimitiveCount);
        }
    }

}


