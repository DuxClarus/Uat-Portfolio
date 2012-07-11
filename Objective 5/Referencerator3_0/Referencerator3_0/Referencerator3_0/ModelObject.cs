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
using SkinnedModel;
using Referencerator3_0;
using RefrenceratorV3_0.Framework;
using RefrenceratorV3_0.Primitives3D;

namespace Referencerator3_0
{
    public class WorldObject
    {
        private Vector3 position;
        private Model model;
        private Texture2D texture;
        private AnimatiedPlayer updatedModel;
        private Matrix[] originalBoneMatrix;
        private SkinnedSphere[] skinnedSpheres;
        private BoundingSphere[] boundingSpheres;
        private BoundingSphere drawSpheres;
        SkinningDataStorage skinningData;
        RefrenceratorV3_0.Primitives3D.SpherePrimitive mySpherePrimitive;
        float bendY;
        private bool drawBoundingSphere = false;

        private InputState input;


        BoundingSphereAndNames selectedSkinnedSphere;
        public bool isASkinnedSphereSelected = false;

        public struct BoundingSphereAndNames
        {
            public string boundingSphereName;
            public BoundingSphere boundingSphere;
        }
        private BoundingSphereAndNames[] boundingSphereAndNamesArray;



        public WorldObject(Model myModel, Texture2D myTexture, SkinnedSphere[] mySkinnedSphere, InputState input)
        {
            this.model = myModel;
            this.texture = myTexture;
            this.position = Vector3.Zero;
            this.skinnedSpheres = mySkinnedSphere;
            this.input = input;
             
        }
        public void LoadObject(GraphicsDevice graphicsDevice)
        {
            this.skinningData = this.model.Tag as SkinningDataStorage;
            this.originalBoneMatrix = new Matrix[skinningData.BindPose.Count];

            int currentBone = 0;
            while (currentBone < skinningData.BindPose.Count)
            {
                this.originalBoneMatrix[currentBone] = skinningData.BindPose[currentBone];
                currentBone++; 
            }

            if (skinningData == null)
            {
                throw new InvalidOperationException
                ("This model does not contain a skinning data tag.");
            }

            boundingSpheres = new BoundingSphere[skinnedSpheres.Length];
            updatedModel = new AnimatiedPlayer(skinningData);
            skinningData.BindPose.CopyTo(updatedModel.boneTransforms, 0);
            mySpherePrimitive = new SpherePrimitive(graphicsDevice, 1, 12);
            UpdateBoundingSpheres();
            boundingSphereAndNamesArray = new BoundingSphereAndNames[skinnedSpheres.Length];
            UpdateStructArray();
        }

        private void UpdateStructArray()
        {
            for (int index = 0; index < skinnedSpheres.Length; index++)
            {
                boundingSphereAndNamesArray[index].boundingSphere = boundingSpheres[index];
                boundingSphereAndNamesArray[index].boundingSphereName = skinnedSpheres[index].BoneName;
            }
        }

        public void UpdateObject()
        {
            updatedModel.UpdateWorldTransforms(Matrix.Identity);
            updatedModel.UpdateSkinTransforms();
            UpdateBoundingSpheres();
            UpdateStructArray();

            //Updates for selecting bone and moving bone
            updateBoundingSphereAndNamesArray();
            updateBoundingSphereAndNamesArray();
            modelBonePicking(input);

            if (isASkinnedSphereSelected)
                MoveBonesWithMouse(input);
        }

        public void UpdateBoundingSpheres()
        {
            Matrix[] worldTransforms = updatedModel.getWorldTransforms();
            for (int index = 0; index < skinnedSpheres.Length; index++)
            {
                SkinnedSphere source = skinnedSpheres[index];
                Vector3 center = new Vector3(source.Offset, 0, 0);
                BoundingSphere sphere = new BoundingSphere(center, source.Radius);
                int boneIndex = skinningData.BoneIndices[source.BoneName];
                boundingSpheres[index] = sphere.Transform(worldTransforms[boneIndex]);
            }
        }

        public void DrawObject(Matrix View, Matrix Projection)
        {
            Matrix[] bones = updatedModel.getSkinTransforms();
            //used for moving model (broken)
            //Matrix translation = Matrix.CreateTranslation(position);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.SetBoneTransforms(bones);
                    effect.View = View;
                    effect.Projection = Projection;
                    effect.SpecularColor = new Vector3(1f);
                    effect.SpecularPower = 12;
                    effect.Texture = this.texture; 
                }
                mesh.Draw();
            }
            DrawBoundingSpheres(View, Projection);
        }

        public void DrawBoundingSpheres(Matrix view, Matrix projection)
        {
            Matrix world = Matrix.CreateScale(drawSpheres.Radius) * Matrix.CreateTranslation(drawSpheres.Center);
            mySpherePrimitive.Draw(world, view, projection, Color.AntiqueWhite); 
        }

        public bool CheckRayIntersection(Ray ray, BoundingSphere sphere)
        {
            if (ray.Intersects(sphere) != null)
            {
                drawSpheres = sphere;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void setDrawBoundingSphere(BoundingSphere sphere)
        {
            drawSpheres = sphere;
        }

        public AnimatiedPlayer UpdatedPlayer
        {
            get{ return this.updatedModel; }
            set{ this.updatedModel = value; }
        }

        #region Bone Picking and Moving

        private void MoveBonesWithMouse(InputState input)
        {
            float rotationFB = input.Mouse.Delta.Y / 120;
            float rotationLR = input.Mouse.Delta.X / 120;

            switch (selectedSkinnedSphere.boundingSphereName)
            {
                case "Character1_LeftUpLeg":
                    {
                        #region Mouse
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                        {
                            Matrix transfrom = Matrix.CreateRotationX(rotationFB);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
                        {
                            Matrix transfrom = Matrix.CreateRotationY(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            Matrix transfrom = Matrix.CreateRotationZ(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        #endregion

                        break;
                    }
                case "Character1_LeftLeg":
                    {
                        #region Mouse
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                        {
                            Matrix transfrom = Matrix.CreateRotationX(rotationFB);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
                        {
                            Matrix transfrom = Matrix.CreateRotationY(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            Matrix transfrom = Matrix.CreateRotationZ(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        #endregion

                        break;
                    }
                case "Character1_LeftFoot":
                    {
                        #region Mouse
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                        {
                            Matrix transfrom = Matrix.CreateRotationX(rotationFB);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
                        {
                            Matrix transfrom = Matrix.CreateRotationY(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            Matrix transfrom = Matrix.CreateRotationZ(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        #endregion

                        break;
                    }
                case "Character1_RightUpLeg":
                    {
                        #region Mouse
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                        {
                            Matrix transfrom = Matrix.CreateRotationX(rotationFB);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
                        {
                            Matrix transfrom = Matrix.CreateRotationY(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            Matrix transfrom = Matrix.CreateRotationZ(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        #endregion

                        break;
                    }
                case "Character1_RightLeg":
                    {
                        #region Mouse
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                        {
                            Matrix transfrom = Matrix.CreateRotationX(rotationFB);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
                        {
                            Matrix transfrom = Matrix.CreateRotationY(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            Matrix transfrom = Matrix.CreateRotationZ(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        #endregion

                        break;
                    }
                case "Character1_RightFoot":
                    {
                        #region Mouse
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                        {
                            Matrix transfrom = Matrix.CreateRotationX(rotationFB);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
                        {
                            Matrix transfrom = Matrix.CreateRotationY(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            Matrix transfrom = Matrix.CreateRotationZ(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        #endregion

                        break;
                    }
                case "Character1_Spine":
                    {
                        #region Mouse
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                        {
                            Matrix transfrom = Matrix.CreateRotationX(rotationFB);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
                        {
                            Matrix transfrom = Matrix.CreateRotationY(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            Matrix transfrom = Matrix.CreateRotationZ(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        #endregion

                        break;
                    }
                case "Character1_LeftShoulder":
                    {
                        #region Mouse
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                        {
                            Matrix transfrom = Matrix.CreateRotationX(rotationFB);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
                        {
                            Matrix transfrom = Matrix.CreateRotationY(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            Matrix transfrom = Matrix.CreateRotationZ(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        #endregion

                        break;
                    }
                case "Character1_LeftArm":
                    {
                        #region Mouse
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                        {
                            Matrix transfrom = Matrix.CreateRotationX(rotationFB);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
                        {
                            Matrix transfrom = Matrix.CreateRotationY(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            Matrix transfrom = Matrix.CreateRotationZ(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        #endregion

                        break;
                    }
                case "Character1_LeftHand":
                    {
                        #region Mouse
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                        {
                            Matrix transfrom = Matrix.CreateRotationX(rotationFB);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
                        {
                            Matrix transfrom = Matrix.CreateRotationY(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            Matrix transfrom = Matrix.CreateRotationZ(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        #endregion

                        break;
                    }
                case "Character1_LeftForeArm":
                    {
                        #region Mouse
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                        {
                            Matrix transfrom = Matrix.CreateRotationX(rotationFB);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
                        {
                            Matrix transfrom = Matrix.CreateRotationY(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            Matrix transfrom = Matrix.CreateRotationZ(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        #endregion

                        break;
                    }
                case "Character1_RightShoulder":
                    {
                        #region Mouse
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                        {
                            Matrix transfrom = Matrix.CreateRotationX(rotationFB);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
                        {
                            Matrix transfrom = Matrix.CreateRotationY(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            Matrix transfrom = Matrix.CreateRotationZ(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        #endregion

                        break;
                    }
                case "Character1_RightArm":
                    {
                        #region Mouse
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                        {
                            Matrix transfrom = Matrix.CreateRotationX(rotationFB);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
                        {
                            Matrix transfrom = Matrix.CreateRotationY(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            Matrix transfrom = Matrix.CreateRotationZ(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        #endregion

                        break;
                    }

                case "Character1_RightForeArm":
                    {
                        #region Mouse
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                        {
                            Matrix transfrom = Matrix.CreateRotationX(rotationFB);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
                        {
                            Matrix transfrom = Matrix.CreateRotationY(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            Matrix transfrom = Matrix.CreateRotationZ(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        #endregion

                        break;
                    }
                case "Character1_RightHand":
                    {
                        #region Mouse
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                        {
                            Matrix transfrom = Matrix.CreateRotationX(rotationFB);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
                        {
                            Matrix transfrom = Matrix.CreateRotationY(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            Matrix transfrom = Matrix.CreateRotationZ(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        #endregion

                        break;
                    }
                case "Character1_Neck":
                    {
                        #region Mouse
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt))
                        {
                            Matrix transfrom = Matrix.CreateRotationX(rotationFB);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];
                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl))
                        {
                            Matrix transfrom = Matrix.CreateRotationY(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightShift))
                        {
                            Matrix transfrom = Matrix.CreateRotationZ(rotationLR);
                            UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)] =
                                transfrom * UpdatedPlayer.boneTransforms[getSkinningBoneIndex(selectedSkinnedSphere.boundingSphereName)];

                        }
                        #endregion

                        break;
                    }
                default:
                    {
                        throw new Exception("Ummm... defaulted in the switch statement of MoveBonesWithMouse method in Game.cs, good luck! " + selectedSkinnedSphere.boundingSphereName);
                    }
            }
        }

        private void modelBonePicking(InputState input)
        {
            // Only perform picking when the user releases the left mouse button
            if (input.Mouse.WasButtonPressed(MouseButtons.Left))
            {
                if (!isASkinnedSphereSelected)
                {
                    for (int index = 0; index < boundingSphereAndNamesArray.Length; index++)
                    {
                        if (CheckRayIntersection(Engine.ray, boundingSphereAndNamesArray[index].boundingSphere))
                        {
                            setDrawBoundingSphere(boundingSphereAndNamesArray[index].boundingSphere);
                            selectedSkinnedSphere.boundingSphere = boundingSphereAndNamesArray[index].boundingSphere;
                            selectedSkinnedSphere.boundingSphereName = boundingSphereAndNamesArray[index].boundingSphereName;
                            isASkinnedSphereSelected = true;
                        }
                    }
                }
                else
                {
                    setDrawBoundingSphere(new BoundingSphere());
                    selectedSkinnedSphere.boundingSphere = new BoundingSphere();
                    selectedSkinnedSphere.boundingSphereName = "";
                    isASkinnedSphereSelected = false;
                }
            }
        }

        private void updateBoundingSphereAndNamesArray()
        {
            for (int index = 0; index < boundingSphereAndNamesArray.Length; index++)
            {
                boundingSphereAndNamesArray[index].boundingSphere = getBoundingSphere(index);
                boundingSphereAndNamesArray[index].boundingSphereName = getBoundingSphereName(index);
            }
        }

        #endregion

        internal string getBoundingSphereName(int index)
        {
            return boundingSphereAndNamesArray[index].boundingSphereName;
        }

        internal BoundingSphere getBoundingSphere(int index)
        {
            return boundingSphereAndNamesArray[index].boundingSphere;
        }

        internal int getBoundingSphereAndNamesArrayLength()
        {
            return boundingSphereAndNamesArray.Length;
        }

        internal int getSkinningData(string boneName)
        {
            for (int index = 0; index < skinnedSpheres.Length; index++)
            {
                if (skinnedSpheres[index].BoneName == boneName)
                {
                    return index;
                }
            }
            throw new Exception("Bone name: " + boneName + " was not found!");
        }

        internal int getSkinningBoneIndex(string boneName)
        {
            for (int index = 0; index < skinnedSpheres.Length; index++)
            {
                if (skinnedSpheres[index].BoneName == boneName)
                {
                    return skinningData.BoneIndices[boneName];
                }
            }
            throw new Exception("Bone name: " + boneName + " was not found!");
        }

        internal void increasePositionZ()
        {
            this.position.Z++;
        }

        internal void decreasePositionZ()
        {
            this.position.Z--;
        }

        internal void increasePositionX()
        {
            this.position.X++;
        }

        internal void decreasePositionX()
        {
            this.position.X--;
        }

        internal void increasePositionY()
        {
            this.position.Y++;
        }

        internal void decreasePositionY()
        {
            this.position.Y--;
        }
    }  
}
