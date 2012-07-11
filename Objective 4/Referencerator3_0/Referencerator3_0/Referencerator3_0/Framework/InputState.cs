using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;

namespace RefrenceratorV3_0.Framework
{
    public class InputState
    {
        public KeyboardState previuosKBState;
        public KeyboardState currentKBState;
        // The keys that were pressed in the current state
        Keys[] currentPressedKeys;
        public KeyboardState State { get { return currentKBState; } }
        public Keys[] PressedKeys { get { return currentPressedKeys; } }
        // Events for when a key is pressed, released and held.
        public event KeyboardEventHandler<Keys, KeyboardState> KeyPressed;
        public event KeyboardEventHandler<Keys, KeyboardState> KeyReleased;
        public event KeyboardEventHandler<Keys, KeyboardState> KeyHeld;
        public MouseDevice Mouse;

        public InputState(GraphicsDevice graphics)
        {
            Mouse = new MouseDevice(graphics);
        }
       
        public void Update(GameTime gameTime)
        {
            Mouse.Update(gameTime);

            previuosKBState = currentKBState;
            currentKBState = Keyboard.GetState();

            currentPressedKeys = currentKBState.GetPressedKeys();

            // For every key on the keyboard
            foreach (Keys key in GetEnumValues<Keys>())
            {
                // If it is a new key press
                if (WasKeyPressed(key))
                    if (KeyPressed != null)
                        KeyPressed(this, new KeyboardEventArgs<Keys, KeyboardState>(key, this));

                // If it was just released
                if (WasKeyReleased(key))
                    if (KeyReleased != null)
                        KeyReleased(this, new KeyboardEventArgs<Keys, KeyboardState>(key, this));

                // If it has been held
                if (WasKeyHeld(key))
                    if (KeyHeld != null)
                        KeyReleased(this, new KeyboardEventArgs<Keys, KeyboardState>(key, this));

            }

            
        }

        // Check is it's down
        public bool IsKeyDown(Keys Key)
        {
            return currentKBState.IsKeyDown(Key);
        }

        // Check if it's up
        public bool IsKeyUp(Keys Key)
        {
            return currentKBState.IsKeyUp(Key);
        }

        public bool WasKeyPressed(Keys Key)
        {
            if (previuosKBState.IsKeyUp(Key) && currentKBState.IsKeyDown(Key))
                return true;

            return false;
        }

        public bool WasKeyReleased(Keys Key)
        {
            if (previuosKBState.IsKeyDown(Key) && currentKBState.IsKeyUp(Key))
                return true;

            return false;
        }

        public bool WasKeyHeld(Keys Key)
        {
            if (previuosKBState.IsKeyDown(Key) && currentKBState.IsKeyDown(Key))
                return true;

            return false;
        }

        public static List<T> GetEnumValues<T>()
        {
            Type currentEnum = typeof(T);
            List<T> resultSet = new List<T>();

            if (currentEnum.IsEnum)
            {
                FieldInfo[] fields = currentEnum.GetFields(BindingFlags.Static | BindingFlags.Public);
                foreach (FieldInfo field in fields)
                    resultSet.Add((T)field.GetValue(null));
            }
            else
                throw new ArgumentException("The argument must be of type Enum or of a type derived from Enum", "T");

            return resultSet;
        }
    }

    public class KeyboardEventArgs<O, S> : EventArgs
    {
        public O Object;

        public InputState Device;

        public KeyboardState State;

        public KeyboardEventArgs(O Object, InputState Device)
        {
            this.Object = Object;
            this.Device = Device;
            this.State = ((InputState)Device).currentKBState;
        }
    }
    public delegate void KeyboardEventHandler<O, S>(object sender, KeyboardEventArgs<O, S> e);
}
