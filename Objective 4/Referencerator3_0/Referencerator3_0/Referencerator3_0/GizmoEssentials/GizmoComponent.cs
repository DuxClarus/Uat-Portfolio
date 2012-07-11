using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RefrenceratorV3_0.Framework;
using Referencerator3_0;


namespace RefrenceratorV3_0.GizmoEssentials
{
    public class GizmoComponent
    {

        private GraphicsDevice graphics;
        private BasicEffect lineEffect;

        public bool Enabled;
        private bool precisionMode = false;

        private Matrix view;
        private Matrix projection;

        // -- Screen Scale -- //
        private Matrix screenScaleMatrix;
        private float screenScale;

        // -- Position - Rotation -- //
        private Vector3 position = Vector3.Zero;
        private Matrix rotationMatrix = Matrix.Identity;
        private Quaternion Rotation
        {
            get { return Quaternion.CreateFromRotationMatrix(rotationMatrix); }
        }

        private Vector3 localForward = Vector3.Forward;
        private Vector3 localUp = Vector3.Up;
        private Vector3 localRight;

        // -- Matrices -- //
        private Matrix objectOrientedWorld;
        private Matrix axisAlignedWorld;
        private Matrix[] modelLocalSpace;

        // used for all drawing, assigned by local- or world-space matrices
        private Matrix gizmoWorld = Matrix.Identity;

        // the matrix used to apply to your whole scene, usually matrix.identity (default scale, origin on 0,0,0 etc.)
        public Matrix sceneWorld;

        // -- Models -- //
        private Model translationModel;
        private Model rotationModel;
        private Model scaleModel;

        // -- Lines (Vertices) -- //
        private VertexPositionColor[] translationLineVertices;
        private float lineLength = 3f;
        private float lineOffset = 0.5f;

        // -- Quads -- //
        Quad[] quads;
        BasicEffect quadEffect;

        // -- Colors -- //
        private Color[] axisColors;
        private Color highlightColor;

        // -- UI Text -- //
        private string[] axisText;
        private Vector3 axisTextOffset = new Vector3(0, 0.5f, 0);

        // -- Modes & Selections -- //
        public GizmoAxis ActiveAxis = GizmoAxis.None;
        public GizmoMode ActiveMode = GizmoMode.Translate;
        public TransformSpace ActiveSpace = TransformSpace.World;
        public PivotType ActivePivot = PivotType.SelectionCenter;

        // -- BoundingBoxes -- //
        #region BoundingBoxes

        private float boxThickness = 0.05f;
        private BoundingOrientedBox X_Box
        {
            get
            {
                return new BoundingOrientedBox(Vector3.Transform(new Vector3((lineLength / 2) +
                    (lineOffset * 2), 0, 0), gizmoWorld),
                    Vector3.Transform(new Vector3(lineLength / 2, 0.2f, 0.2f),
                    screenScaleMatrix), Rotation);
            }
        }

        private BoundingOrientedBox Y_Box
        {
            get
            {
                return new BoundingOrientedBox(Vector3.Transform(new Vector3(0, (lineLength / 2) +
                    (lineOffset * 2), 0), gizmoWorld),
                    Vector3.Transform(new Vector3(0.2f, lineLength / 2, 0.2f),
                    screenScaleMatrix), Rotation);
            }
        }

        private BoundingOrientedBox Z_Box
        {
            get
            {
                return new BoundingOrientedBox(Vector3.Transform(new Vector3(0, 0, (lineLength / 2) +
                    (lineOffset * 2)), gizmoWorld),
                    Vector3.Transform(new Vector3(0.2f, 0.2f, lineLength / 2), screenScaleMatrix),
                    Rotation);
            }
        }

        private BoundingOrientedBox XZ_Box
        {
            get
            {
                return new BoundingOrientedBox(Vector3.Transform(new Vector3(lineOffset, 0, lineOffset),
                    gizmoWorld),
                    Vector3.Transform(new Vector3(lineOffset, boxThickness, lineOffset),
                    screenScaleMatrix), Rotation);
            }
        }

        private BoundingOrientedBox XY_Box
        {
            get
            {
                return new BoundingOrientedBox(Vector3.Transform(new Vector3(lineOffset, lineOffset, 0),
                    gizmoWorld),
                    Vector3.Transform(new Vector3(lineOffset, lineOffset, boxThickness),
                    screenScaleMatrix), Rotation);
            }
        }

        private BoundingOrientedBox YZ_Box
        {
            get
            {
                return new BoundingOrientedBox(Vector3.Transform(new Vector3(0, lineOffset, lineOffset),
                    gizmoWorld),
                    Vector3.Transform(new Vector3(boxThickness, lineOffset, lineOffset),
                    screenScaleMatrix), Rotation);
            }
        }

        #endregion

        // -- BoundingSpheres -- //
        #region BoundingSpheres
        private float radius = 1f;
        private BoundingSphere X_Sphere
        {
            get
            {
                return new BoundingSphere(Vector3.Transform(translationLineVertices[1].Position,
                    gizmoWorld), radius * screenScale);
            }
        }

        private BoundingSphere Y_Sphere
        {
            get
            {
                return new BoundingSphere(Vector3.Transform(translationLineVertices[7].Position,
                    gizmoWorld), radius * screenScale);
            }
        }

        private BoundingSphere Z_Sphere
        {
            get
            {
                return new BoundingSphere(Vector3.Transform(translationLineVertices[13].Position,
                    gizmoWorld), radius * screenScale);
            }
        }
        #endregion

        // -- Input & Hotkeys -- //
        Keys addToSelection = Keys.LeftControl;
        Keys removeFromSelection = Keys.LeftAlt;

        // other hotkeys:
        // Space = change of transformationSpace
        // Space + Shift = change of pivotType
        // 1, 2, 3, 4 = change of transformation-mode

        private float inputScale;

        /// <summary>
        /// The value to adjust all transformation when precisionMode is active.
        /// </summary>
        private float precisionModeScale = 0.1f;

        // -- Selection -- //
        public List<Framework.SceneEntity> Selection = new List<Framework.SceneEntity>();

        // -- Translation Variables -- //
        Vector3 translationDelta;
        Vector3 lastIntersectionPosition;
        Vector3 intersectPosition;
        private const float translationClampDistance = 1000;


        public bool SnapEnabled = false;

        public float TranslationSnapValue = 5;
        public float RotationSnapValue = 30;
        public float ScaleSnapValue = 0.5f;

        private Vector3 translationScaleSnapDelta;
        private float rotationSnapDelta;



        public GizmoComponent(ContentManager content, GraphicsDevice graphics)
            : this(content, graphics, Matrix.Identity) { }

        public GizmoComponent(ContentManager content, GraphicsDevice graphics, Matrix world)
        {
            this.sceneWorld = world;
            this.graphics = graphics;
            lineEffect = new BasicEffect(graphics);
            lineEffect.VertexColorEnabled = true;
            lineEffect.AmbientLightColor = Vector3.One;
            lineEffect.EmissiveColor = Vector3.One;

            quadEffect = new BasicEffect(graphics);
            quadEffect.EnableDefaultLighting();
            quadEffect.World = Matrix.Identity;
            quadEffect.DiffuseColor = highlightColor.ToVector3();
            quadEffect.Alpha = 0.5f;


            translationModel = content.Load<Model>("Models/gizmo_translate");
            rotationModel = content.Load<Model>("Models/gizmo_rotate");
            scaleModel = content.Load<Model>("Models/gizmo_scale");
            //font = content.Load<SpriteFont>("gizmoFont");
        }

        public void Initialize()
        {
            // -- Set local-space offset -- //
            modelLocalSpace = new Matrix[3];
            modelLocalSpace[0] = Matrix.CreateWorld(new Vector3(lineLength, 0, 0), Vector3.Left,
                Vector3.Up);
            modelLocalSpace[1] = Matrix.CreateWorld(new Vector3(0, lineLength, 0), Vector3.Down,
                Vector3.Left);
            modelLocalSpace[2] = Matrix.CreateWorld(new Vector3(0, 0, lineLength),
                Vector3.Forward, Vector3.Up);

            // -- Colors: X,Y,Z,Highlight -- //
            axisColors = new Color[3];
            axisColors[0] = Color.Red;
            axisColors[1] = Color.Green;
            axisColors[2] = Color.Blue;
            highlightColor = Color.Gold;

            // text projected in 3D
            axisText = new string[3];
            axisText[0] = "X";
            axisText[1] = "Y";
            axisText[2] = "Z";

            // translucent quads
            #region Translucent Quads
            float doubleOffset = lineOffset * 2;
            quads = new Quad[3];
            quads[0] = new Quad(new Vector3(lineOffset, lineOffset, 0), Vector3.Backward,
                Vector3.Up, doubleOffset, doubleOffset); //XY
            quads[1] = new Quad(new Vector3(lineOffset, 0, lineOffset), Vector3.Up,
                Vector3.Right, doubleOffset, doubleOffset); //XZ
            quads[2] = new Quad(new Vector3(0, lineOffset, lineOffset), Vector3.Right,
                Vector3.Up, doubleOffset, doubleOffset); //ZY 
            #endregion

            // fill array with vertex-data
            #region Fill Axis-Line array
            List<VertexPositionColor> vertexList = new List<VertexPositionColor>(18);

            // helper to apply colors
            Color xColor = axisColors[0];
            Color yColor = axisColors[1];
            Color zColor = axisColors[2];

            float doubleLineOffset = lineOffset * 2;

            // -- X Axis -- // index 0 - 5
            vertexList.Add(new VertexPositionColor(new Vector3(lineOffset, 0, 0), xColor));
            vertexList.Add(new VertexPositionColor(new Vector3(lineLength, 0, 0), xColor));

            vertexList.Add(new VertexPositionColor(new Vector3(doubleLineOffset, 0, 0), xColor));
            vertexList.Add(new VertexPositionColor(new Vector3(doubleLineOffset, doubleLineOffset,
                0), xColor));

            vertexList.Add(new VertexPositionColor(new Vector3(doubleLineOffset, 0, 0), xColor));
            vertexList.Add(new VertexPositionColor(new Vector3(doubleLineOffset, 0,
                doubleLineOffset), xColor));

            // -- Y Axis -- // index 6 - 11
            vertexList.Add(new VertexPositionColor(new Vector3(0, lineOffset, 0), yColor));
            vertexList.Add(new VertexPositionColor(new Vector3(0, lineLength, 0), yColor));

            vertexList.Add(new VertexPositionColor(new Vector3(0, doubleLineOffset, 0), yColor));
            vertexList.Add(new VertexPositionColor(new Vector3(doubleLineOffset, doubleLineOffset,
                0), yColor));

            vertexList.Add(new VertexPositionColor(new Vector3(0, doubleLineOffset, 0), yColor));
            vertexList.Add(new VertexPositionColor(new Vector3(0, doubleLineOffset,
                doubleLineOffset), yColor));

            // -- Z Axis -- // index 12 - 17
            vertexList.Add(new VertexPositionColor(new Vector3(0, 0, lineOffset), zColor));
            vertexList.Add(new VertexPositionColor(new Vector3(0, 0, lineLength), zColor));

            vertexList.Add(new VertexPositionColor(new Vector3(0, 0, doubleLineOffset), zColor));
            vertexList.Add(new VertexPositionColor(new Vector3(doubleLineOffset, 0,
                doubleLineOffset), zColor));

            vertexList.Add(new VertexPositionColor(new Vector3(0, 0, doubleLineOffset), zColor));
            vertexList.Add(new VertexPositionColor(new Vector3(0, doubleLineOffset,
                doubleLineOffset), zColor));

            // -- Convert to array -- //
            translationLineVertices = vertexList.ToArray();
            #endregion
        }

        public void Update(GameTime gameTime)
        {
            if (!Enabled)
                return;

            this.view = Engine.view;
            this.projection = Engine.projection;

            // scale for mouse.delta
            inputScale = (float)gameTime.ElapsedGameTime.TotalSeconds;

            SetPosition();

            // -- Scale Gizmo to fit on-screen -- //
            Vector3 vLength = Engine.cameraPosition - position;
            float scaleFactor = 25;

            screenScale = vLength.Length() / scaleFactor;
            screenScaleMatrix = Matrix.CreateScale(new Vector3(screenScale));

            localForward = Selection[0].forward;
            localUp = Selection[0].up;
            // -- Vector Rotation (Local/World) -- //
            localForward.Normalize();
            localRight = Vector3.Cross(localForward, localUp);
            localUp = Vector3.Cross(localRight, localForward);
            localRight.Normalize();
            localUp.Normalize();

            // -- Create Both World Matrices -- //
            objectOrientedWorld = screenScaleMatrix * Matrix.CreateWorld(position, localForward,
                localUp);
            axisAlignedWorld = screenScaleMatrix * Matrix.CreateWorld(position,
                sceneWorld.Forward, sceneWorld.Up);

            // Assign World
            if (ActiveSpace == TransformSpace.World ||
                (ActiveMode == GizmoMode.Rotate && Selection.Count > 1) ||
                (ActiveMode == GizmoMode.NonUniformScale && Selection.Count > 1) ||
                (ActiveMode == GizmoMode.UniformScale && Selection.Count > 1))
            {
                gizmoWorld = axisAlignedWorld;

                // align lines, boxes etc. with the grid-lines
                rotationMatrix.Forward = sceneWorld.Forward;
                rotationMatrix.Up = sceneWorld.Up;
                rotationMatrix.Right = sceneWorld.Right;
            }
            else
            {
                gizmoWorld = objectOrientedWorld;

                // align lines, boxes etc. with the selected object
                rotationMatrix.Forward = localForward;
                rotationMatrix.Up = localUp;
                rotationMatrix.Right = localRight;
            }

            // -- Reset Colors to default -- //
            ApplyColor(GizmoAxis.X, axisColors[0]);
            ApplyColor(GizmoAxis.Y, axisColors[1]);
            ApplyColor(GizmoAxis.Z, axisColors[2]);

            // -- Apply Highlight -- //
            ApplyColor(ActiveAxis, highlightColor);

        }

        public void HandleInput(InputState input)
        {
            // -- Select Gizmo Mode -- //
            if (input.WasKeyPressed(Keys.D1))
            {
                ActiveMode = GizmoMode.Translate;
            }
            else if (input.WasKeyPressed(Keys.D2))
            {
                ActiveMode = GizmoMode.Rotate;
            }
            else if (input.WasKeyPressed(Keys.D3))
            {
                ActiveMode = GizmoMode.NonUniformScale;
            }
            else if (input.WasKeyPressed(Keys.D4))
            {
                ActiveMode = GizmoMode.UniformScale;
            }

            // -- Cycle TransformationSpaces -- //
            if (input.WasKeyPressed(Keys.Space))
            {
                if (ActiveSpace == TransformSpace.Local)
                    ActiveSpace = TransformSpace.World;
                else
                    ActiveSpace = TransformSpace.Local;
            }

            // -- Cycle PivotTypes -- //
            if (input.WasKeyPressed(Keys.P))
            {
                if (ActivePivot == PivotType.WorldOrigin)
                    ActivePivot = PivotType.ObjectCenter;
                else
                    ActivePivot++;
            }

            // -- Toggle PrecisionMode -- //
            if (input.IsKeyDown(Keys.LeftShift))
            {
                precisionMode = true;
            }
            else
                precisionMode = false;

            // -- Toggle Snapping -- //
            if (input.WasKeyPressed(Keys.C))
            {
                SnapEnabled = !SnapEnabled;
            }
            if (input.Mouse.WasButtonPressed(Framework.MouseButtons.Left) && ActiveAxis == GizmoAxis.None)
            {
                // add to selection or clear current selection
                if (input.IsKeyUp(addToSelection) && input.IsKeyUp(removeFromSelection) || input.IsKeyDown(Keys.LeftAlt))
                {
                    Selection.Clear();
                }

                PickObject(input.Mouse.Position, input.IsKeyDown(removeFromSelection));
            }
            else if (input.WasKeyPressed(Keys.R))
            {
                Selection.Clear();
            }

            if (Enabled)
            {
                if (input.Mouse.WasButtonPressed(Framework.MouseButtons.Left))
                {
                    // reset for intersection (plane vs. ray)
                    intersectPosition = Vector3.Zero;
                    // reset for snapping
                    translationScaleSnapDelta = Vector3.Zero;
                    rotationSnapDelta = 0;
                }

                lastIntersectionPosition = intersectPosition;

                if (input.Mouse.WasButtonHeld(Framework.MouseButtons.Left) && ActiveAxis != GizmoAxis.None)
                {
                    if (ActiveMode == GizmoMode.Translate || ActiveMode == GizmoMode.NonUniformScale || ActiveMode == GizmoMode.UniformScale)
                    {
                        #region Translate & Scale
                        Vector3 delta = Vector3.Zero;
                        Ray ray = Engine.ray;

                        Matrix transform = Matrix.Invert(rotationMatrix);
                        ray.Position = Vector3.Transform(ray.Position, transform);
                        ray.Direction = Vector3.TransformNormal(ray.Direction, transform);


                        if (ActiveAxis == GizmoAxis.X || ActiveAxis == GizmoAxis.XY)
                        {
                            Plane plane = new Plane(Vector3.Forward, Vector3.Transform(position,
                                Matrix.Invert(rotationMatrix)).Z);

                            float? intersection = ray.Intersects(plane);
                            if (intersection.HasValue)
                            {
                                intersectPosition = (ray.Position + (ray.Direction *
                                    intersection.Value));
                                if (lastIntersectionPosition != Vector3.Zero)
                                {
                                    translationDelta = intersectPosition - 
                                        lastIntersectionPosition;
                                }
                                if (ActiveAxis == GizmoAxis.X)
                                    delta = new Vector3(translationDelta.X, 0, 0);
                                else
                                    delta = new Vector3(translationDelta.X, translationDelta.Y, 0);
                            }
                        }
                        else if (ActiveAxis == GizmoAxis.Y || ActiveAxis == GizmoAxis.YZ || ActiveAxis == GizmoAxis.Z)
                        {
                            Plane plane = new Plane(Vector3.Left, Vector3.Transform(position,
                                Matrix.Invert(rotationMatrix)).X);

                            float? intersection = ray.Intersects(plane);
                            if (intersection.HasValue)
                            {
                                intersectPosition = (ray.Position + (ray.Direction *
                                    intersection.Value));
                                if (lastIntersectionPosition != Vector3.Zero)
                                {
                                    translationDelta = intersectPosition - 
                                        lastIntersectionPosition;
                                }
                                if (ActiveAxis == GizmoAxis.Y)
                                    delta = new Vector3(0, translationDelta.Y, 0);
                                else if (ActiveAxis == GizmoAxis.Z)
                                    delta = new Vector3(0, 0, translationDelta.Z);
                                else
                                    delta = new Vector3(0, translationDelta.Y, translationDelta.Z);
                            }
                        }
                        else if (ActiveAxis == GizmoAxis.ZX)
                        {
                            Plane plane = new Plane(Vector3.Down, Vector3.Transform(position, 
                                Matrix.Invert(rotationMatrix)).Y);

                            float? intersection = ray.Intersects(plane);
                            if (intersection.HasValue)
                            {
                                intersectPosition = (ray.Position + (ray.Direction * 
                                    intersection.Value));
                                if (lastIntersectionPosition != Vector3.Zero)
                                {
                                    translationDelta = intersectPosition - 
                                        lastIntersectionPosition;
                                }
                            }

                            delta = new Vector3(translationDelta.X, 0, translationDelta.Z);
                        }


                        if (SnapEnabled)
                        {
                            float snapValue = TranslationSnapValue;
                            if (ActiveMode == GizmoMode.UniformScale || ActiveMode == GizmoMode.NonUniformScale)
                                snapValue = ScaleSnapValue;
                            if (precisionMode)
                            {
                                delta *= precisionModeScale;
                                snapValue *= precisionModeScale;
                            }

                            translationScaleSnapDelta += delta;

                            delta = new Vector3(
                                (int)(translationScaleSnapDelta.X / snapValue) * snapValue,
                                (int)(translationScaleSnapDelta.Y / snapValue) * snapValue,
                                (int)(translationScaleSnapDelta.Z / snapValue) * snapValue);

                            translationScaleSnapDelta -= delta;
                        }
                        else if (precisionMode)
                            delta *= precisionModeScale;



                        if (ActiveMode == GizmoMode.Translate)
                        {
                            // transform (local or world)
                            delta = Vector3.Transform(delta, rotationMatrix);

                            // apply
                            foreach (Framework.SceneEntity entity in Selection)
                            {
                                entity.position += delta;
                            }
                            
                        }
                        else if (ActiveMode == GizmoMode.NonUniformScale)
                        {
                            // -- Apply Scale -- //
                            foreach (Framework.SceneEntity entity in Selection)
                            {
                                entity.scale += delta;
                                entity.scale = Vector3.Clamp(entity.scale, Vector3.Zero,
                                    entity.scale);
                            }
                        }
                        else if (ActiveMode == GizmoMode.UniformScale)
                        {

                            float diff = 1 + ((delta.X + delta.Y + delta.Z) / 3);
                            foreach (Framework.SceneEntity entity in Selection)
                            {
                                entity.scale *= diff;

                                entity.scale = Vector3.Clamp(entity.scale, Vector3.Zero, 
                                    entity.scale);
                            }
                        }
                        #endregion
                    }
                    else if (ActiveMode == GizmoMode.Rotate)
                    {
                        #region Rotate
                        float delta = input.Mouse.Delta.X;
                        delta *= inputScale;


                        if (SnapEnabled)
                        {
                            float snapValue = MathHelper.ToRadians(RotationSnapValue);
                            if (precisionMode)
                            {
                                delta *= precisionModeScale;
                                snapValue *= precisionModeScale;
                            }

                            rotationSnapDelta += delta;

                            float snapped = (int)(rotationSnapDelta / snapValue) * snapValue;
                            rotationSnapDelta -= snapped;

                            delta = snapped;
                        }
                        else if (precisionMode)
                        {
                            delta *= precisionModeScale;
                        }

                        // rotation matrix to transform - if more than one objects selected, always use world-space.
                        Matrix rot = Matrix.Identity;
                        rot.Forward = sceneWorld.Forward;
                        rot.Up = sceneWorld.Up;
                        rot.Right = sceneWorld.Right;

                        if (ActiveAxis == GizmoAxis.X)
                        {
                            rot *= Matrix.CreateFromAxisAngle(rotationMatrix.Right, delta);
                        }
                        else if (ActiveAxis == GizmoAxis.Y)
                        {
                            rot *= Matrix.CreateFromAxisAngle(rotationMatrix.Up, delta);
                        }
                        else if (ActiveAxis == GizmoAxis.Z)
                        {
                            rot *= Matrix.CreateFromAxisAngle(rotationMatrix.Forward, delta);
                        }

                        // -- Apply rotation -- //
                        foreach (Framework.SceneEntity entity in Selection)
                        {
                            // use gizmo position for all PivotTypes except for object-center, it should use the entity.position instead.
                            Vector3 pos = position;
                            if (ActivePivot == PivotType.ObjectCenter)
                            {
                                pos = entity.position;
                            }

                            Matrix localRot = Matrix.Identity;
                            localRot.Forward = entity.forward;
                            localRot.Up = entity.up;
                            localRot.Right = Vector3.Cross(entity.forward, entity.up);
                            localRight.Normalize();
                            localRot.Translation = entity.position - pos;

                            Matrix newRot = localRot * rot;

                            entity.forward = newRot.Forward;
                            entity.up = newRot.Up;
                            entity.position = newRot.Translation + pos;

                        }
                        #endregion
                    }
                }
                else
                {
                    UpdateAxisSelection(input.Mouse.Position);
                }
            }

            // Enable only if something is selected.
            if (Selection.Count < 1)
                Enabled = false;
            else
                Enabled = true;
        }

        /// <summary>
        /// Helper method for applying color to the gizmo lines.
        /// </summary>
        private void ApplyColor(GizmoAxis axis, Color color)
        {
            if (ActiveMode == GizmoMode.Translate || ActiveMode == GizmoMode.NonUniformScale)
            {
                switch (axis)
                {
                    case GizmoAxis.X:
                        ApplyLineColor(0, 6, color);
                        break;
                    case GizmoAxis.Y:
                        ApplyLineColor(6, 6, color);
                        break;
                    case GizmoAxis.Z:
                        ApplyLineColor(12, 6, color);
                        break;
                    case GizmoAxis.XY:
                        ApplyLineColor(0, 4, color);
                        ApplyLineColor(6, 4, color);
                        break;
                    case GizmoAxis.YZ:
                        ApplyLineColor(6, 2, color);
                        ApplyLineColor(12, 2, color);
                        ApplyLineColor(10, 2, color);
                        ApplyLineColor(16, 2, color);
                        break;
                    case GizmoAxis.ZX:
                        ApplyLineColor(0, 2, color);
                        ApplyLineColor(4, 2, color);
                        ApplyLineColor(12, 4, color);
                        break;
                }
            }
            else if (ActiveMode == GizmoMode.Rotate)
            {
                switch (axis)
                {
                    case GizmoAxis.X:
                        ApplyLineColor(0, 6, color);
                        break;
                    case GizmoAxis.Y:
                        ApplyLineColor(6, 6, color);
                        break;
                    case GizmoAxis.Z:
                        ApplyLineColor(12, 6, color);
                        break;
                }
            }
            else if (ActiveMode == GizmoMode.UniformScale)
            {
                // all three axis red.
                if (ActiveAxis == GizmoAxis.None)
                    ApplyLineColor(0, translationLineVertices.Length, axisColors[0]);
                else
                    ApplyLineColor(0, translationLineVertices.Length, highlightColor);
            }
        }

        /// <summary>
        /// Apply color on the lines associated with translation mode (re-used in Scale)
        /// </summary>
        private void ApplyLineColor(int startindex, int count, Color color)
        {
            for (int i = startindex; i < (startindex + count); i++)
            {
                translationLineVertices[i].Color = color;
            }
        }

        /// <summary>
        /// Per-frame check to see if mouse is hovering over any axis.
        /// </summary>
        private void UpdateAxisSelection(Vector2 mousePosition)
        {
            float closestintersection = float.MaxValue;
            Ray ray = Engine.ray;

            closestintersection = float.MaxValue;
            float? intersection;

            if (ActiveMode == GizmoMode.Translate || ActiveMode == GizmoMode.NonUniformScale || ActiveMode == GizmoMode.UniformScale)
            {
                #region BoundingBoxes

                intersection = XY_Box.Intersects(ref ray);
                if (intersection.HasValue)
                {
                    if (intersection.Value < closestintersection)
                    {
                        ActiveAxis = GizmoAxis.XY;
                        closestintersection = intersection.Value;
                    }
                }
                intersection = XZ_Box.Intersects(ref ray);
                if (intersection.HasValue)
                {
                    if (intersection.Value < closestintersection)
                    {
                        ActiveAxis = GizmoAxis.ZX;
                        closestintersection = intersection.Value;
                    }
                }
                intersection = YZ_Box.Intersects(ref ray);
                if (intersection.HasValue)
                {
                    if (intersection.Value < closestintersection)
                    {
                        ActiveAxis = GizmoAxis.YZ;
                        closestintersection = intersection.Value;
                    }
                }

                intersection = X_Box.Intersects(ref ray);
                if (intersection.HasValue)
                {
                    if (intersection.Value < closestintersection)
                    {
                        ActiveAxis = GizmoAxis.X;
                        closestintersection = intersection.Value;
                    }
                }
                intersection = Y_Box.Intersects(ref ray);
                if (intersection.HasValue)
                {
                    if (intersection.Value < closestintersection)
                    {
                        ActiveAxis = GizmoAxis.Y;
                        closestintersection = intersection.Value;
                    }
                }
                intersection = Z_Box.Intersects(ref ray);
                if (intersection.HasValue)
                {
                    if (intersection.Value < closestintersection)
                    {
                        ActiveAxis = GizmoAxis.Z;
                        closestintersection = intersection.Value;
                    }
                }

                #endregion
            }

            if (ActiveMode == GizmoMode.Rotate || ActiveMode == GizmoMode.UniformScale || ActiveMode == GizmoMode.NonUniformScale)
            {
                #region BoundingSpheres

                intersection = X_Sphere.Intersects(ray);
                if (intersection.HasValue)
                {
                    if (intersection.Value < closestintersection)
                    {
                        ActiveAxis = GizmoAxis.X;
                        closestintersection = intersection.Value;
                    }
                }
                intersection = Y_Sphere.Intersects(ray);
                if (intersection.HasValue)
                {
                    if (intersection.Value < closestintersection)
                    {
                        ActiveAxis = GizmoAxis.Y;
                        closestintersection = intersection.Value;
                    }
                }
                intersection = Z_Sphere.Intersects(ray);
                if (intersection.HasValue)
                {
                    if (intersection.Value < closestintersection)
                    {
                        ActiveAxis = GizmoAxis.Z;
                        closestintersection = intersection.Value;
                    }
                }


                #endregion
            }

            if (ActiveMode == GizmoMode.Rotate)
            {
                #region X,Y,Z Boxes
                intersection = X_Box.Intersects(ref ray);
                if (intersection.HasValue)
                {
                    if (intersection.Value < closestintersection)
                    {
                        ActiveAxis = GizmoAxis.X;
                        closestintersection = intersection.Value;
                    }
                }
                intersection = Y_Box.Intersects(ref ray);
                if (intersection.HasValue)
                {
                    if (intersection.Value < closestintersection)
                    {
                        ActiveAxis = GizmoAxis.Y;
                        closestintersection = intersection.Value;
                    }
                }
                intersection = Z_Box.Intersects(ref ray);
                if (intersection.HasValue)
                {
                    if (intersection.Value < closestintersection)
                    {
                        ActiveAxis = GizmoAxis.Z;
                        closestintersection = intersection.Value;
                    }
                }
                #endregion
            }

            if (closestintersection == float.MaxValue)
            {
                ActiveAxis = GizmoAxis.None;
            }
        }

        /// <summary>
        /// Select objects inside level. Very simple example. Should be replaced by your own code, more fit for your custom editors.
        /// </summary>
        private void PickObject(Vector2 mousePosition, bool removeFromSelection)
        {
            foreach (Framework.SceneEntity entity in Engine.Entities)
            {
                if (entity.BoundingBox.Intersects(Engine.ray).HasValue)
                {
                    if (!Selection.Contains(entity))
                    {
                        Selection.Add(entity);
                        break;
                    }
                    else
                    {
                        if (removeFromSelection)
                            Selection.Remove(entity);
                    }
                }
            }
        }

        /// <summary>
        /// Set position of the gizmo, position will be center of all selected entities.
        /// </summary>
        private void SetPosition()
        {

            switch (ActivePivot)
            {
                case PivotType.ObjectCenter:
                    {
                        if (Selection.Count > 0)
                            position = Selection[0].position;
                    }
                    break;
                case PivotType.SelectionCenter:
                    {
                        Vector3 center = Vector3.Zero;
                        foreach (Framework.SceneEntity entity in Selection)
                        {
                            center += entity.position;
                        }
                        center /= Selection.Count;

                        position = center;
                    }
                    break;
                case PivotType.WorldOrigin:
                    {
                        position = sceneWorld.Translation;
                    }
                    break;
            }
        }

        /// <summary>
        /// Resets transformation (except position) on the current selection.
        /// </summary>
        public void ResetTransform()
        {
            foreach (Framework.SceneEntity entity in Selection)
            {
                entity.forward = Vector3.Forward;
                entity.up = Vector3.Up;
                entity.scale = Vector3.One;
                // optional
                // entity.Position = Vector3.Zero;
            }
        }

        public void Draw3D()
        {

            if (Enabled)
            {
                graphics.DepthStencilState = DepthStencilState.None;

                #region Draw: Axis-Lines
                // -- Draw Lines -- //
                lineEffect.World = gizmoWorld;
                lineEffect.View = view;
                lineEffect.Projection = projection;

                lineEffect.CurrentTechnique.Passes[0].Apply();
                {
                    graphics.DrawUserPrimitives(PrimitiveType.LineList, translationLineVertices,
                        0, translationLineVertices.Length / 2);
                }

                #endregion

                if (ActiveMode == GizmoMode.Translate || ActiveMode == GizmoMode.NonUniformScale)
                {
                    #region Translate & NonUniformScale
                    // these two modes share a lot of the same draw-code

                    // -- Draw Quads -- //
                    if (ActiveAxis == GizmoAxis.XY || ActiveAxis == GizmoAxis.YZ || ActiveAxis == GizmoAxis.ZX)
                    {
                        graphics.BlendState = BlendState.AlphaBlend;
                        graphics.RasterizerState = RasterizerState.CullNone;

                        quadEffect.World = gizmoWorld;
                        quadEffect.View = view;
                        quadEffect.Projection = projection;

                        quadEffect.CurrentTechnique.Passes[0].Apply();

                        Quad activeQuad = new Quad();
                        switch (ActiveAxis)
                        {
                            case GizmoAxis.XY:
                                activeQuad = quads[0];
                                break;
                            case GizmoAxis.ZX:
                                activeQuad = quads[1];
                                break;
                            case GizmoAxis.YZ:
                                activeQuad = quads[2];
                                break;
                        }

                        graphics.DrawUserIndexedPrimitives<VertexPositionNormalTexture>
                            (PrimitiveType.TriangleList, activeQuad.Vertices, 0, 4,
                            activeQuad.Indexes, 0, 2);

                        graphics.BlendState = BlendState.Opaque;
                        graphics.RasterizerState = RasterizerState.CullCounterClockwise;
                    }

                    if (ActiveMode == GizmoMode.Translate)
                    {
                        // -- Draw Cones -- //
                        for (int i = 0; i < 3; i++) // 3 = nr. of axis (order: x, y, z)
                        {
                            foreach (ModelMesh mesh in translationModel.Meshes)
                            {
                                foreach (ModelMeshPart meshpart in mesh.MeshParts)
                                {
                                    BasicEffect effect = (BasicEffect)meshpart.Effect;
                                    Vector3 color = axisColors[i].ToVector3();

                                    effect.World = modelLocalSpace[i] * gizmoWorld;
                                    effect.DiffuseColor = color;
                                    effect.EmissiveColor = color;

                                    effect.EnableDefaultLighting();

                                    effect.View = view;
                                    effect.Projection = projection;
                                }
                                mesh.Draw();
                            }
                        }
                    }
                    else
                    {
                        // -- Draw Boxes -- //
                        for (int i = 0; i < 3; i++) // 3 = nr. of axis (order: x, y, z)
                        {
                            foreach (ModelMesh mesh in scaleModel.Meshes)
                            {
                                foreach (ModelMeshPart meshpart in mesh.MeshParts)
                                {
                                    BasicEffect effect = (BasicEffect)meshpart.Effect;
                                    Vector3 color = axisColors[i].ToVector3();

                                    effect.World = modelLocalSpace[i] * gizmoWorld;
                                    effect.DiffuseColor = color;
                                    effect.EmissiveColor = color;

                                    effect.EnableDefaultLighting();

                                    effect.View = view;
                                    effect.Projection = projection;
                                }
                                mesh.Draw();
                            }
                        }
                    }
                    #endregion
                }
                else if (ActiveMode == GizmoMode.Rotate)
                {
                    #region Rotate
                    // -- Draw Circle-Arrows -- //
                    for (int i = 0; i < 3; i++) // 3 = nr. of axis (order: x, y, z)
                    {
                        foreach (ModelMesh mesh in rotationModel.Meshes)
                        {
                            foreach (ModelMeshPart meshpart in mesh.MeshParts)
                            {
                                BasicEffect effect = (BasicEffect)meshpart.Effect;
                                Vector3 color = axisColors[i].ToVector3();

                                effect.World = modelLocalSpace[i] * gizmoWorld;
                                effect.DiffuseColor = color;
                                effect.EmissiveColor = color;


                                effect.View = view;
                                effect.Projection = projection;
                            }
                            mesh.Draw();
                        }
                    }
                    #endregion
                }
                else if (ActiveMode == GizmoMode.UniformScale)
                {
                    #region UniformScale
                    // -- Draw Boxes -- //
                    for (int i = 0; i < 3; i++) // 3 = nr. of axis (order: x, y, z)
                    {
                        foreach (ModelMesh mesh in scaleModel.Meshes)
                        {
                            foreach (ModelMeshPart meshpart in mesh.MeshParts)
                            {
                                BasicEffect effect = (BasicEffect)meshpart.Effect;
                                Vector3 color = axisColors[0].ToVector3(); //- all using the same color (red)

                                effect.World = modelLocalSpace[i] * gizmoWorld;
                                effect.DiffuseColor = color;
                                effect.EmissiveColor = color;

                                effect.EnableDefaultLighting();

                                effect.View = view;
                                effect.Projection = projection;
                            }
                            mesh.Draw();
                        }
                    }

                    if (ActiveAxis != GizmoAxis.None)
                    {
                        graphics.BlendState = BlendState.AlphaBlend;
                        graphics.RasterizerState = RasterizerState.CullNone;

                        quadEffect.World = gizmoWorld;
                        quadEffect.View = view;
                        quadEffect.Projection = projection;

                        quadEffect.CurrentTechnique.Passes[0].Apply();

                        for (int i = 0; i < quads.Length; i++)
                        {
                            graphics.DrawUserIndexedPrimitives<VertexPositionNormalTexture>
                                (PrimitiveType.TriangleList,
                                quads[i].Vertices, 0, 4,
                                quads[i].Indexes, 0, 2);
                        }

                        graphics.BlendState = BlendState.Opaque;
                        graphics.RasterizerState = RasterizerState.CullCounterClockwise;
                    }
                    #endregion
                }

                graphics.DepthStencilState = DepthStencilState.Default;
            }
        }

        public List<Framework.SceneEntity> getSelectedEntities()
        {
            return Selection;
        }

        internal void setActiveMode(GizmoMode inGizmoMode)
        {
            ActiveMode = inGizmoMode;
        }
    }

    public enum GizmoAxis
    {
        X,
        Y,
        Z,
        XY,
        ZX,
        YZ,
        None
    }

    public enum GizmoMode
    {
        Translate,
        Rotate,
        NonUniformScale,
        UniformScale
    }

    public enum TransformSpace
    {
        Local,
        World
    }

    public enum PivotType
    {
        ObjectCenter,
        SelectionCenter,
        WorldOrigin
    }
}
