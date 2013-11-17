using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FoodFight3D
{
  public class GeometricPrimitive : IDisposable
  {
    protected List<VertexPositionNormal> vertices = new List<VertexPositionNormal>();
    protected List<ushort> indices = new List<ushort>();

    protected VertexBuffer vertexBuffer;
    protected IndexBuffer indexBuffer;
    protected BasicEffect basicEffect;

    protected void AddVertex(Vector3 position, Vector3 normal)
    {
      vertices.Add(new VertexPositionNormal(position, normal));
    }

    protected void AddIndex(int index)
    {
      if (index > ushort.MaxValue)
        throw new ArgumentOutOfRangeException("index");

      indices.Add((ushort)index);
    }

    protected int CurrentVertex
    {
      get { return vertices.Count;  }
    }

    protected void InitializePrimitive(GraphicsDevice graphicsDevice)
    {
      vertexBuffer = new VertexBuffer(graphicsDevice,
        typeof(VertexPositionNormal), vertices.Count, BufferUsage.None);
      vertexBuffer.SetData(vertices.ToArray());

      indexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort),
        indices.Count, BufferUsage.None);
      indexBuffer.SetData(indices.ToArray());

      basicEffect = new BasicEffect(graphicsDevice);
      basicEffect.EnableDefaultLighting();
    }

    ~GeometricPrimitive()
    {
      Dispose();
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (vertexBuffer != null)
          vertexBuffer.Dispose();

        if (indexBuffer != null)
          indexBuffer.Dispose();

        if (basicEffect != null)
          basicEffect.Dispose();
      }
    }

    protected void _Draw(Effect effect)
    {
      GraphicsDevice graphicsDevice = effect.GraphicsDevice;
      graphicsDevice.SetVertexBuffer(vertexBuffer);
      graphicsDevice.Indices = indexBuffer;

      foreach (EffectPass effectPass in effect.CurrentTechnique.Passes)
      {
        effectPass.Apply();
        int primitiveCount = indices.Count / 3;
        graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
          vertices.Count, 0, primitiveCount);
      }
    }

    public void Draw(Matrix world, Matrix view, Matrix projection, Color color)
    {
      basicEffect.World = world;
      basicEffect.View = view;
      basicEffect.Projection = projection;
      basicEffect.DiffuseColor = color.ToVector3();
      basicEffect.Alpha = color.A / 255.0f;

      GraphicsDevice device = basicEffect.GraphicsDevice;
      device.DepthStencilState = DepthStencilState.Default;

      if (color.A < 255)
      {
        device.BlendState = BlendState.AlphaBlend;
      }
      else
      {
        device.BlendState = BlendState.Opaque;
      }

      _Draw(basicEffect);
    }
  }
}
