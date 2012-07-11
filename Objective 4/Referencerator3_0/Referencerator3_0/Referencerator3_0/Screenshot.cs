using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using System.IO;

namespace RefrenceratorV3_0
{
    class Screenshot
    {
        GraphicsDevice graphicsDevice;

        public Screenshot()
        {
        }

        public void takeScreenshot(GraphicsDevice inGraphcisDevice)
        {
            graphicsDevice = inGraphcisDevice;
            captureScreenshot();
        }

        private void captureScreenshot()
        {
            try
            {
                byte[] screenData;

                screenData = new byte[graphicsDevice.PresentationParameters.BackBufferWidth * graphicsDevice.PresentationParameters.BackBufferHeight * 4];
                //take whats on the graphics device and put it to the back buffer
                graphicsDevice.GetBackBufferData<byte>(screenData);
                Texture2D texture2D = new Texture2D(graphicsDevice,
                                                    graphicsDevice.PresentationParameters.BackBufferWidth,
                                                    graphicsDevice.PresentationParameters.BackBufferHeight, false,
                                                    graphicsDevice.PresentationParameters.BackBufferFormat);
                texture2D.SetData<byte>(screenData);
                saveScreenshot(texture2D);
                texture2D.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show("You broke it....");
            }
        }
        /// <summary>
        /// Opens dialog for user to save screenshot
        /// </summary>
        /// <param name="texture2D">Takes in texture2D for screen information</param>
        private static void saveScreenshot(Texture2D texture2D)
        {
            SaveFileDialog saveScreenshot = new SaveFileDialog();
            saveScreenshot.DefaultExt = ".png";
            saveScreenshot.ShowDialog();
            saveScreenshot.CreatePrompt = true;
            if (saveScreenshot.FileName != string.Empty)
            {
                Stream stream = new FileStream(saveScreenshot.FileName, FileMode.Create);
                texture2D.SaveAsPng(stream, texture2D.Width, texture2D.Height);
                stream.Close();
            }
        }
    }
}
