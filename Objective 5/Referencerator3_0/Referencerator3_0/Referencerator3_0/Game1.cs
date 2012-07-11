using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RefrenceratorV3_0;
using RefrenceratorV3_0.Framework;
using RefrenceratorV3_0.GizmoEssentials;
using SkinnedModel;

namespace Referencerator3_0
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public partial class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Form controls
        Form window;
        Screenshot takeScreenShot;
        private bool isScreenshotButtonClicked;

        Camera camera;

        //Framework
        InputState input;
        GizmoComponent gizmo;
        GridComponent grid;
        List<SceneEntity> entitiesToBeDeleted;
        Model rigModel;
        WorldObject mainModel;
        Texture2D skinTextureUnselected;
        SkinnedSphere[] skinnedSpheres;

        Texture2D objectTexture;

        public struct BoundingSphereAndNames
        {
            public string boundingSphereName;
            public BoundingSphere boundingSphere;
        }
        BoundingSphereAndNames[] boundingSphereAndNamesArray;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Engine.SetupEngine(GraphicsDevice);
            window = Control.FromHandle(this.Window.Handle) as Form;
            InitializeComponent(window);
            LoadAllButtons();

            input = new InputState(GraphicsDevice);
            gizmo = new GizmoComponent(Content, GraphicsDevice);
            gizmo.Initialize();
            grid = new GridComponent(GraphicsDevice, 5);

            takeScreenShot = new Screenshot();
            isScreenshotButtonClicked = false;

            this.IsMouseVisible = true;
            this.Window.AllowUserResizing = true;

            entitiesToBeDeleted = new List<SceneEntity>();

            //keep this last.
            window.WindowState = FormWindowState.Maximized;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            rigModel = Content.Load<Model>("Models/Character Models/New Rig");
            skinTextureUnselected = Content.Load<Texture2D>("modelTexture");

            skinnedSpheres = Content.Load<SkinnedSphere[]>("CollisionSpheres");

            mainModel = new WorldObject(rigModel, skinTextureUnselected, skinnedSpheres, input);
            mainModel.LoadObject(GraphicsDevice);

            camera = new Camera(input,this);

            Engine.model = mainModel;

            boundingSphereAndNamesArray = new BoundingSphereAndNames[skinnedSpheres.Length];

            objectTexture = Content.Load<Texture2D>("texture");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Engine.ray = camera.GetMouseRay();
            Engine.Update();
         
            camera.Update();

            gizmo.HandleInput(input);
            gizmo.Update(gameTime);
            
            input.Update(gameTime);


            if (input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Delete) ||
                input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Back))
            {
                entitiesToBeDeleted = gizmo.getSelectedEntities();
                deleteSceneEntities(entitiesToBeDeleted);
            }

            IsGridEnabled();
            IsSnappingEnabled();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);
            Engine.cameraPosition = camera.Position;
            Engine.view = camera.View;
            Engine.projection = camera.Projection;

            Engine.Draw();
            grid.Draw3D();
            gizmo.Draw3D();
            mainModel.DrawObject(Engine.view, Engine.projection);


            if (isScreenshotButtonClicked == true)
            {
                isScreenshotButtonClicked = false;
                takeScreenShot.takeScreenshot(GraphicsDevice);
            }
            base.Draw(gameTime);
        }

        private void deleteSceneEntities(List<SceneEntity> entitiesToBeDeleted)
        {
            foreach (SceneEntity SE in entitiesToBeDeleted)
            {
                if (Engine.Entities.Contains(SE))
                {
                    Engine.Entities.Remove(SE);
                    gizmo = new GizmoComponent(Content, graphics.GraphicsDevice);
                    gizmo.Initialize();
                }
            }
        }

        private void IsGridEnabled()
        {
            if (input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.G))
            {
                grid.enabled = !grid.enabled;
                this.gridToolStripMenuItem.Checked = !this.gridToolStripMenuItem.Checked;
            }
        }

        private void IsSnappingEnabled()
        {
            if (input.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.C))
                this.tsGridSnapping.Checked = !this.tsGridSnapping.Checked;
        }

        #region ButtonClicks

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to save your current progress?", "Warning! Loss of progress may occur.", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                save();
                //remove current elements and reset main model
                Engine.Entities = new List<SceneEntity>();
                defaultModel();//do this
                load();
            }
            else if (result == DialogResult.No)
            {
                //remove current elements and reset main model
                Engine.Entities = new List<SceneEntity>();
                defaultModel();//do this
                load();
            }
            else if (result == DialogResult.Cancel)
            { } //do nothing
            
        }

        private void defaultModel()
        {
            //throw new NotImplementedException();
        }

        private void load()
        {
            Stream stream;
            BinaryFormatter bFormatter = new BinaryFormatter();
            OpenFileDialog loadWindow = new OpenFileDialog();
            loadWindow.Filter = "rrr files (*.rrr)|*.rrr";
            loadWindow.FilterIndex = 2;
            if (loadWindow.ShowDialog() == DialogResult.OK)
            {
                if ((stream = loadWindow.OpenFile()) != null)
                {
                    while (stream.Position < stream.Length)
                    {
                        SceneEntity SE = (SceneEntity)bFormatter.Deserialize(stream);
                        SE.model = Content.Load<Model>(SE.name);
                        Engine.Entities.Add(SE);
                    }
                    stream.Close();
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        private static void save()
        {
            Stream stream;
            BinaryFormatter bFormatter = new BinaryFormatter();
            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.Filter = "rrr files (*.rrr)|*.rrr";
            saveWindow.FilterIndex = 2;
            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                if ((stream = saveWindow.OpenFile()) != null)
                {
                    foreach (SceneEntity SE in Engine.Entities)
                        bFormatter.Serialize(stream, SE);
                    stream.Close();
                }
            }
        }

        private void loadObject_Click(object sender, EventArgs e)
        {
            string objectPath = ((Button)sender).Tag.ToString();

            Model model = Content.Load<Model>(objectPath);
            SceneEntity entity = new SceneEntity(model, objectPath);
            entity.position = new Vector3(20, 0, 0);
            Engine.Entities.Add(entity);
        }

        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid.enabled = this.gridToolStripMenuItem.Checked;
        }

        private void tsGridSnapping_Click(object sender, EventArgs e)
        {
            gizmo.SnapEnabled = this.tsGridSnapping.Checked;
        }

        #endregion

    }
}
