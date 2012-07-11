using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RefrenceratorV3_0.Framework
{
    // An enum of buttons on the mouse, since XNA doesn't provide one
    public enum MouseButtons { Left, Right, Middle, X1, X2 };

    // An input device that keeps track of the Mouse and MouseState
    public class MouseDevice
    {
        // The last and current MouseStates
        MouseState last;
        MouseState current;

        // The MouseButtons that are currently down
        MouseButtons[] currentButtons;

        // Public properties for the above members
        public MouseState State { get { return current; } }
        public MouseButtons[] PressedButtons { get { return currentButtons; } }

        // The position in (X,Y) coordinates of the mouse. Setting the mouse
        // position will tell XNA to move the mouse
        Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                Mouse.SetPosition((int)value.X, (int)value.Y);
            }
        }
        // The change in position of the mouse between the last two frames
        public Vector2 Delta = Vector2.Zero;

        // Whether to reset the mouse to the middle of the screen on each
        // update to simulate a screen with no borders
        public bool ResetMouseAfterUpdate = false;

        // The current position of the scroll wheel
        public float ScrollPosition { get { return current.ScrollWheelValue; } }

        // The change in position of the scroll wheel between the last two
        // frames
        public float ScrollDelta = 0;

        // Events for when a button is pressed, released, or held
        public event MouseEventHandler<MouseButtons, MouseState> ButtonPressed;
        public event MouseEventHandler<MouseButtons, MouseState> ButtonReleased;
        public event MouseEventHandler<MouseButtons, MouseState> ButtonHeld;

        GraphicsDevice graphics;

        // Constructor gets the initial MouseState, moves the mouse to the
        // center of the screen, and does the first update
        public MouseDevice(GraphicsDevice graphics)
        {
            if (ResetMouseAfterUpdate)
                Position = new Vector2(
                    graphics.Viewport.Width / 2,
                    graphics.Viewport.Height / 2);

            current = Mouse.GetState();

            this.graphics = graphics;
        }

        // List of pressed buttons used to setup currentButtons
        List<MouseButtons> pressed = new List<MouseButtons>();

        public void Update(GameTime gameTime)
        {
            // Update the last state
            last = current;

            // Extract the scroll wheel position from the last state

            float lastScrollWheel = last.ScrollWheelValue;

            // Update the current state and clear the list of pressed
            // buttons
            current = Mouse.GetState();
            pressed.Clear();

            // Calculcate the scroll delta with the current position
            // and the copy of the last position we made earlier
            ScrollDelta = ScrollPosition - lastScrollWheel;

            // If we are supposed to reset the mouse after each update...
            if (ResetMouseAfterUpdate)
            {
                // Find the center of the screen
                Vector2 center = new Vector2(
                    graphics.Viewport.Width / 2,
                    graphics.Viewport.Height / 2);

                // Find the mouse's distance from the center of the screen to
                // find the change in position
                Delta = new Vector2(current.X - center.X, current.Y - center.Y);

                // Move the mouse back to the center so we never run out of
                // screen space
                Position = center;
            }
            else
            {
                // Otherwise just find the difference in position between the
                // last two frames
                Delta = new Vector2(current.X - last.X, current.Y - last.Y);
                position = new Vector2(current.X, current.Y);
            }
            // For each mouse button...
            foreach (MouseButtons button in InputState.GetEnumValues<MouseButtons>())
            {
                // If it is down, add it to the list
                if (IsButtonDown(button))
                    pressed.Add(button);

                // If it was just pressed, fire the event
                if (WasButtonPressed(button))
                    if (ButtonPressed != null)
                        ButtonPressed(this, new MouseDeviceEventArgs
                            <MouseButtons, MouseState>(button, this));

                // If it was just released, fire the event
                if (WasButtonReleased(button))
                    if (ButtonReleased != null)
                        ButtonReleased(this, new MouseDeviceEventArgs
                            <MouseButtons, MouseState>(button, this));

                // If it was held, fire the event
                if (WasButtonHeld(button))
                    if (ButtonHeld != null)
                        ButtonHeld(this, new MouseDeviceEventArgs
                            <MouseButtons, MouseState>(button, this));
            }

            // Update the currentButtons array from the list of buttons
            // that are down
            currentButtons = pressed.ToArray();
        }

        // Whether the specified button is currently down
        public bool IsButtonDown(MouseButtons Button)
        {
            return IsButtonDown(Button, current);
        }

        // An internal version of IsButtonDown that also allows us
        // to specify which state the check against
        bool IsButtonDown(MouseButtons Button, MouseState State)
        {
            return GetButtonState(Button, State) ==
                ButtonState.Pressed ? true : false;
        }

        // Whether the specified button is currently up
        public bool IsButtonUp(MouseButtons Button)
        {
            return IsButtonUp(Button, current);
        }

        // An internal version of IsButtonUp that also allows us
        // to specify which state the check against
        bool IsButtonUp(MouseButtons Button, MouseState State)
        {
            return GetButtonState(Button, State) ==
                ButtonState.Released ? true : false;
        }

        // Whether the specified button is down for the time this frame
        public bool WasButtonPressed(MouseButtons Button)
        {
            if (IsButtonUp(Button, last) && IsButtonDown(Button, current))
                return true;

            return false;
        }

        // Whether the specified button is up for the first this frame
        public bool WasButtonReleased(MouseButtons Button)
        {
            if (IsButtonDown(Button, last) && IsButtonUp(Button, current))
                return true;

            return false;
        }

        // Whether the specified button has been down for more than one frame
        public bool WasButtonHeld(MouseButtons Button)
        {
            if (IsButtonDown(Button, last) && IsButtonDown(Button, current))
                return true;

            return false;
        }

        // Retrieves the ButtonState of the specified button from the 
        // specified state.
        ButtonState GetButtonState(MouseButtons Button, MouseState State)
        {
            if (Button == MouseButtons.Left)
                return State.LeftButton;
            else if (Button == MouseButtons.Middle)
                return State.MiddleButton;
            else if (Button == MouseButtons.Right)
                return State.RightButton;
            else if (Button == MouseButtons.X1)
                return State.XButton1;
            else if (Button == MouseButtons.X2)
                return State.XButton2;

            return ButtonState.Released;
        }

    }
    public class MouseDeviceEventArgs<O, S> : EventArgs
    {
        public O Object;

        public MouseDevice Device;

        public MouseState State;

        public MouseDeviceEventArgs(O Object, MouseDevice Device)
        {
            this.Object = Object;
            this.Device = Device;
            this.State = Device.State;
        }
    }
    public delegate void MouseEventHandler<O, S>(object sender, 
    MouseDeviceEventArgs<O, S> e);
}
