using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RefrenceratorV3_0.Framework;
using RefrenceratorV3_0.GizmoEssentials;
using RefrenceratorV3_0;

namespace Referencerator3_0
{
    public static class Engine
    {
        public static List<SceneEntity> Entities = new List<SceneEntity>();

        public static Matrix view;
        public static Matrix projection;
        public static Vector3 cameraPosition;
        public static Ray ray;
        public static WorldObject model;
        public static Camera camera;

        public static GraphicsDevice Graphics;

        public static void SetupEngine(GraphicsDevice graphics)
        {
            Graphics = graphics;
        }

        public static void Update()
        {
            foreach (SceneEntity entity in Entities)
            {
                entity.Update();
            }
            model.UpdateObject();
        }

        public static void Draw()
        {
            foreach (SceneEntity entity in Entities)
            {
                entity.Draw();
            }
        }
    }
}
