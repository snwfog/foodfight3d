﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FoodFight3D
{
  public struct VertexPositionNormal : IVertexType
  {
    public Vector3 Position;
    public Vector3 Normal;

    public VertexPositionNormal(Vector3 position, Vector3 normal)
    {
      Position = position;
      Normal = normal;
    }

    public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(
      new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
      new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
    );

    VertexDeclaration IVertexType.VertexDeclaration
    {
      get { return VertexPositionNormal.VertexDeclaration; }
    }
  }
}
