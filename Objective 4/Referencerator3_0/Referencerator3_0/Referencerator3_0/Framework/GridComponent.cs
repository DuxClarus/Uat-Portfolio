using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Referencerator3_0;

namespace RefrenceratorV3_0.Framework
{
    public sealed class GridComponent
    {
        public bool enabled = true;

        private int spacing;
        public int GridSpacing
        {
            get { return spacing; }
            set
            {
                spacing = value;
                ResetLines();
            }
        }

        private int gridSize = 360;

        /// <summary>
        /// Number of lines in total.
        /// </summary>
        private int numberOfLines;
        private BasicEffect effect;
        private GraphicsDevice graphics;
        private VertexPositionColor[] vertexData;

        private Color lineColor = Color.Black;
        private Color highlightColor = Color.Red;


        public GridComponent(GraphicsDevice device, int gridspacing)
        {
            effect = new BasicEffect(device);
            effect.VertexColorEnabled = true;
            effect.World = Matrix.Identity;

            graphics = device;

            spacing = gridspacing;

            ResetLines();
        }

        public void ResetLines()
        {
            // calculate nr of lines, +2 for the highlights, +12 for boundingbox
            numberOfLines = ((gridSize / spacing) * 4) + 2;

            List<VertexPositionColor> vertexList = new List<VertexPositionColor>(numberOfLines);

            // fill array
            for (int i = 1; i < (gridSize / spacing) + 1; i++)
            {
                vertexList.Add(new VertexPositionColor(new Vector3((i * spacing), 0, gridSize), lineColor));
                vertexList.Add(new VertexPositionColor(new Vector3((i * spacing), 0, -gridSize), lineColor));

                vertexList.Add(new VertexPositionColor(new Vector3((-i * spacing), 0, gridSize), lineColor));
                vertexList.Add(new VertexPositionColor(new Vector3((-i * spacing), 0, -gridSize), lineColor));

                vertexList.Add(new VertexPositionColor(new Vector3(gridSize, 0, (i * spacing)), lineColor));
                vertexList.Add(new VertexPositionColor(new Vector3(-gridSize, 0, (i * spacing)), lineColor));

                vertexList.Add(new VertexPositionColor(new Vector3(gridSize, 0, (-i * spacing)), lineColor));
                vertexList.Add(new VertexPositionColor(new Vector3(-gridSize, 0, (-i * spacing)), lineColor));
            }

            // add highlights
            vertexList.Add(new VertexPositionColor(Vector3.Forward * gridSize, highlightColor));
            vertexList.Add(new VertexPositionColor(Vector3.Backward * gridSize, highlightColor));

            vertexList.Add(new VertexPositionColor(Vector3.Right * gridSize, highlightColor));
            vertexList.Add(new VertexPositionColor(Vector3.Left * gridSize, highlightColor));

            // convert to array for drawing
            vertexData = vertexList.ToArray();
        }

        public void Draw3D()
        {
            if (enabled)
            {
                graphics.DepthStencilState = DepthStencilState.Default;

                effect.View = Engine.view;
                effect.Projection = Engine.projection;

                effect.CurrentTechnique.Passes[0].Apply();
                {
                    graphics.DrawUserPrimitives(PrimitiveType.LineList, vertexData, 0, numberOfLines);
                }
            }
        }
    }
}
