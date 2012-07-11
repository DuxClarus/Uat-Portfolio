using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RefrenceratorV3_0.Primitives3D
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
        //Vertex Declartation Object, contains information about the vertex elements contained within this struct
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0));
        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexPositionNormal.VertexDeclaration; }
        }

    }
}
