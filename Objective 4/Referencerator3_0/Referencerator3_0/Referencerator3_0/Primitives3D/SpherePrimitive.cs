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
    public class SpherePrimitive : GeometricPrimitive
    {
        //Constructs new sphere, using default settings
        public SpherePrimitive(GraphicsDevice graphicsDevice)
            : this(graphicsDevice, 1, 16)
        {
        }
        //Constructs new sphere, with specified size and tessellation level. 
        public SpherePrimitive(GraphicsDevice graphicsDevice, float diameter, int tessellation)
        {
            if (tessellation < 3)
                throw new ArgumentOutOfRangeException("tessellation");

            int verticalSegments = tessellation;
            int horizontalSegments = tessellation * 2;
            float radius = diameter / 2;
            //Starts with a single vertex at the bottom of the sphere. 
            AddVertex(Vector3.Down * radius, Vector3.Down);
            //Crates rings of vertices at progressively higher latitudes 
            for (int index = 0; index < verticalSegments - 1; index++)
            {
                float latitude = ((index + 1) * MathHelper.Pi / verticalSegments) - MathHelper.PiOver2;
                float dy = (float)Math.Sin(latitude);
                float dxz = (float)Math.Cos(latitude);
                //Create single ring vertices at latitude. 
                for (int indexTwo = 0; indexTwo < horizontalSegments; indexTwo++)
                {
                    float longitude = indexTwo * MathHelper.TwoPi / horizontalSegments;
                    float dx = (float)Math.Cos(longitude) * dxz;
                    float dz = (float)Math.Sin(longitude) * dxz;
                    Vector3 normal = new Vector3(dx, dy, dz);

                    AddVertex(normal * radius, normal);
                }
            }
            //Finish with a single vertex at top of sphere. 
            AddVertex(Vector3.Up * radius, Vector3.Up);

            //Creates a fan connecting the bottom vertex to the bottom latitude ring. 
            for (int index = 0; index < horizontalSegments; index++)
            {
                AddIndex(0);
                AddIndex(1 + (index + 1) % horizontalSegments);
                AddIndex(1 + index);
            }

            //Fill the sphere body with triangles joing each pair of latitude rings. 
            for (int index = 0; index < verticalSegments - 2; index++)
            {
                for (int indexTwo = 0; indexTwo < horizontalSegments; indexTwo++)
                {
                    int nextIndex = index + 1;
                    int nextIndexTwo = (indexTwo + 1) % horizontalSegments;

                    AddIndex(1 + index * horizontalSegments + indexTwo);
                    AddIndex(1 + index * horizontalSegments + nextIndexTwo);
                    AddIndex(1 + nextIndex * horizontalSegments + indexTwo);

                    AddIndex(1 + index * horizontalSegments + nextIndexTwo);
                    AddIndex(1 + nextIndex * horizontalSegments + nextIndexTwo);
                    AddIndex(1 + nextIndex * horizontalSegments + indexTwo);
                }
            }

            for (int index = 0; index < horizontalSegments; index++)
            {
                AddIndex(CurrentVertex - 1);
                AddIndex(CurrentVertex - 2 - (index + 1) % horizontalSegments);
                AddIndex(CurrentVertex - 2 - index);

            }
            InitializePrimitive(graphicsDevice);
        }
    }
}
