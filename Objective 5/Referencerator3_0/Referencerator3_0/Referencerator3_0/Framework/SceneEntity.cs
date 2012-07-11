using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;
using Referencerator3_0;

namespace RefrenceratorV3_0.Framework
{
    [Serializable]
    public class SceneEntity : ISerializable
    {
        public Vector3 position = Vector3.Zero;
        public Vector3 scale = Vector3.One;

        public Vector3 forward = Vector3.Forward;
        public Vector3 up = Vector3.Up;

        public Model model;
        public string name;

        private Matrix world;

        public Texture2D texture;

        public BoundingBox BoundingBox
        {
            get { return new BoundingBox(position - (Vector3.One * 5), position + (Vector3.One * 5)); }
        }

        //regular constructor
        public SceneEntity(Model model, string name)
        {
            this.model = model;
            this.name = name;
        }

        //deserialization constructor
        public SceneEntity(SerializationInfo loadInformation, StreamingContext context)
        {
            this.name = (string)loadInformation.GetValue("modelName", typeof(string));
            this.position = (Vector3)loadInformation.GetValue("modelPosition", typeof(Vector3));
            this.scale = (Vector3)loadInformation.GetValue("modelScale", typeof(Vector3));
        }

        //serialization method
        public void GetObjectData(SerializationInfo saveInformation, StreamingContext context)
        {
            saveInformation.AddValue("modelName", this.name);
            saveInformation.AddValue("modelPosition", this.position);
            saveInformation.AddValue("modelScale", this.scale);
        }

        public void Update()
        {
            Vector3 Up = up;
            up.Normalize();

            Vector3 Forward = forward;
            forward.Normalize();

            world = Matrix.CreateScale(scale) * Matrix.CreateWorld(position, forward, up);
        }

        public void Draw()
        {

            foreach (ModelMesh modelmesh in model.Meshes)
            {
                foreach (ModelMeshPart meshpart in modelmesh.MeshParts)
                {
                    BasicEffect effect = (BasicEffect)meshpart.Effect;
                    effect.World = world;
                    effect.View = Engine.view;
                    effect.Projection = Engine.projection;
                    effect.Texture = this.texture;
                    effect.EnableDefaultLighting();

                }
                modelmesh.Draw();
            }
        }
    }
}
