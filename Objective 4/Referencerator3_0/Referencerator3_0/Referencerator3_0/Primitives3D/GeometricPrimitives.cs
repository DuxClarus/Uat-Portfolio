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
    public abstract class GeometricPrimitive : IDisposable
    {
        List<VertexPositionNormal> vertices = new List<VertexPositionNormal>();
        List<ushort> indices = new List<ushort>();

        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        BasicEffect basicEffect;

        //Add a new vertex to the primitive model. This should only call during the initialization process
        protected void AddVertex(Vector3 position, Vector3 normal)
        {
            vertices.Add(new VertexPositionNormal(position, normal));
        }

        //Adds new index to the primitive model this should only be called during the initialization process
        protected void AddIndex(int index)
        {
            if (index > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("index");
            indices.Add((ushort)index);
        }

        //Queries the index of the current Vertex. 
        //Starts at zero and increments every time AddVertex is called. 
        protected int CurrentVertex
        {
            get { return vertices.Count; }
        }

        //Once the geometry has been specified by calling AddVertex and AddIndex, this method copies ther vertex and index into GPU format buffers, ready for efficient rendering. 
        protected void InitializePrimitive(GraphicsDevice graphicsDevice)
        {
            //Create a vertex buffer, and copy our vertex data into it.
            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionNormal), vertices.Count, BufferUsage.None);
            vertexBuffer.SetData(vertices.ToArray());
            //Create a index buffer, and copy our index data to it. 
            indexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort), indices.Count, BufferUsage.None);
            indexBuffer.SetData(indices.ToArray());

            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.EnableDefaultLighting();

        }
        //Finalize
        ~GeometricPrimitive()
        {
            Dispose(false);
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

        //Draws primitive model, using specified effect. Unlike other draw overload this method does not set any renderstates, so you must make sure all states are set to sensible values before you call it. 
        public void Draw(Effect effect)
        {
            GraphicsDevice graphicsDevice = effect.GraphicsDevice;

            // Set vertex declaration, vertex buffer, and index buffer
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            graphicsDevice.Indices = indexBuffer;

            foreach (EffectPass effectPass in effect.CurrentTechnique.Passes)
            {
                effectPass.Apply();
                int primitiveCount = indices.Count / 3;

                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertices.Count, 0, primitiveCount);
            }
        }

        //Draws primitive model, using a Basic Effect Shader with default lighting. this method sets important renderstates to sensible values for 3D model rendering, so you do not need to set these states before you call it. 

        public void Draw(Matrix world, Matrix view, Matrix projection, Color color)
        {
            //Sets parameters 
            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.DiffuseColor = color.ToVector3();
            basicEffect.Alpha = color.A / 255.0f;
            GraphicsDevice device = basicEffect.GraphicsDevice;

            device.DepthStencilState = DepthStencilState.Default;
            //Sets renderstats for Alpha blended rendering. 
            if (color.A < 255)
            {
                device.BlendState = BlendState.AlphaBlend;
            }
            else
            {
                device.BlendState = BlendState.Opaque;
            }
            Draw(basicEffect);
        }

    }
}
