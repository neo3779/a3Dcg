using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace AWGP
{
    public class MouseMonitor : iMouseMonitor
    {

        #region Enums

        /// <summary>
        /// Enum for all the button on a mouse
        /// </summary>
        public enum MouseButtons
        {
            LEFTBUTTON,
            MIDDLEBUTTON,
            RIGHTBUTTON,
            EXTRABUTTON1,
            EXTRABUTTON2
        }

        #endregion

        #region Variables

        // Mouse current and previous states
        private MouseState preMouseState, curMouseState;
        //Buttons that have been pressed and released
        private List<MouseButtons> buttonPressed, buttonReleased, buttonsToWatch;
        //Events for new button and old button that are pressed 
        public event EventHandler NewMousePosition, NewMouseWheelPosition, NewButtonPressed, OldButtonReleased;
        //
        private InputDirection mouseAxisDirection, mouseWheelDirection;
        private bool mouseWheel = false;
        private List<InputDirection> mouseAxisToWatch;

        #endregion

        #region Constuctors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="buttonsToWatch">Buttons to be watched by monitor</param>
        public MouseMonitor(List<MouseButtons> buttonsToWatch, List<InputDirection> mouseAxisToWatch, bool mouseWheel)
        {
            //Create buttons to watch list
            this.buttonsToWatch = buttonsToWatch;
            this.mouseWheel = mouseWheel;
            this.mouseAxisToWatch = mouseAxisToWatch;
            this.buttonPressed = new List<MouseButtons>();
            this.buttonReleased = new List<MouseButtons>();
            //Initailize the states
            Update();
        }

        /// <summary>
        /// Deconstructor
        /// </summary>
        ~MouseMonitor()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Update the states
        /// </summary>
        public void Update()
        {
            //Get current mouse states
            this.curMouseState = Mouse.GetState();
            //Check to see if state has changed
            if (!this.curMouseState.Equals(this.preMouseState))
            {
                if (buttonsToWatch.Count != 0)
                {
                    //Loop thorugh all the new pressed buttons
                    foreach (MouseButtons w in buttonsToWatch)
                    {

                        //Determine if new
                        if (this.NewButtonPressed != null && IsNewPress(w))
                        {
                            //Add to pressed button list
                            this.buttonPressed.Add(w);
                            //Create event for new button pressed
                            this.NewButtonPressed(this, new EventArgs());

                            this.buttonPressed.Remove(w);
                        }
                        //Determine if old
                        if (this.OldButtonReleased != null && IsOldPress(w))
                        {
                            //Remove from pressed buttons
                            this.buttonPressed.Remove(w);
                            //Add to released buttons
                            this.buttonReleased.Add(w);
                            //Create event for old button relesaed
                            this.OldButtonReleased(this, new EventArgs());
                            //Remove from released buttons
                            this.buttonReleased.Remove(w);
                        }
                    }

                }
                if (mouseAxisToWatch.Count != 0)
                {
                    //Check if mouse has moved
                    if (this.curMouseState.X != this.preMouseState.X ||
                        this.curMouseState.Y != this.preMouseState.Y)
                    {
                        if (this.curMouseState.X != this.preMouseState.X &&
                            this.curMouseState.Y != this.preMouseState.Y)
                        {
                            if (this.curMouseState.X > this.preMouseState.X &&
                                this.curMouseState.Y > this.preMouseState.Y)
                            {
                                if (this.mouseAxisToWatch.Contains(InputDirection.DOWN_RIGHT))
                                {
                                    this.mouseAxisDirection = InputDirection.DOWN_RIGHT;
                                }
                            }

                            if (this.curMouseState.X < this.preMouseState.X &&
                                this.curMouseState.Y > this.preMouseState.Y)
                            {
                                if (this.mouseAxisToWatch.Contains(InputDirection.DOWN_LEFT))
                                {
                                    this.mouseAxisDirection = InputDirection.DOWN_LEFT;
                                }
                            }

                            if (this.curMouseState.X < this.preMouseState.X &&
                                this.curMouseState.Y < this.preMouseState.Y)
                            {
                                if (this.mouseAxisToWatch.Contains(InputDirection.UP_LEFT))
                                {
                                    this.mouseAxisDirection = InputDirection.UP_LEFT;
                                }
                            }

                            if (this.curMouseState.X > this.preMouseState.X &&
                                this.curMouseState.Y < this.preMouseState.Y)
                            {
                                if (this.mouseAxisToWatch.Contains(InputDirection.UP_RIGHT))
                                {
                                    this.mouseAxisDirection = InputDirection.UP_RIGHT;
                                }
                            }

                        }
                        else
                        {
                            if (this.curMouseState.X != this.preMouseState.X)
                            {
                                if (this.curMouseState.X > this.preMouseState.X)
                                {
                                    if (this.mouseAxisToWatch.Contains(InputDirection.RIGHT))
                                    {
                                        this.mouseAxisDirection = InputDirection.RIGHT;
                                    }
                                }
                                else
                                {
                                    if (this.mouseAxisToWatch.Contains(InputDirection.LEFT))
                                    {
                                        this.mouseAxisDirection = InputDirection.LEFT;
                                    }
                                }
                            }
                            if (this.curMouseState.Y != this.preMouseState.Y)
                            {
                                if (this.curMouseState.Y > this.preMouseState.Y)
                                {
                                    if (this.mouseAxisToWatch.Contains(InputDirection.DOWN))
                                    {
                                        this.mouseAxisDirection = InputDirection.DOWN;
                                    }
                                }
                                else
                                {
                                    if (this.mouseAxisToWatch.Contains(InputDirection.UP))
                                    {
                                        this.mouseAxisDirection = InputDirection.UP;
                                    }
                                }
                            }
                        }
                    }

                    if (this.mouseAxisDirection != InputDirection.NONE)
                    {
                        //Check event exists 
                        if (this.NewMousePosition != null)
                        {
                            //Create event for new position
                            this.NewMousePosition(this, new EventArgs());
                        }
                    }

                }

                if (mouseWheel)
                {
                    //Check if the mouse wheel has moved
                    if (this.curMouseState.ScrollWheelValue != this.preMouseState.ScrollWheelValue)
                    {
                        if (this.curMouseState.ScrollWheelValue > this.preMouseState.ScrollWheelValue)
                        {
                            this.mouseWheelDirection = InputDirection.UP;
                        }
                        else if (this.curMouseState.ScrollWheelValue < this.preMouseState.ScrollWheelValue)
                        {
                            this.mouseWheelDirection = InputDirection.DOWN;
                        }

                        if (this.mouseWheelDirection != InputDirection.NONE)
                        {
                            //Check event exists
                            if (this.NewMouseWheelPosition != null)
                            {
                                //Create event for new wheel position
                                this.NewMouseWheelPosition(this, new EventArgs());
                            }
                        }
                    }
                }
                this.preMouseState = curMouseState;
            }
            else
            {
                this.mouseAxisDirection = InputDirection.NONE;
                this.mouseWheelDirection = InputDirection.NONE;
            }
            //Update previous mouse state

        }

        /// <summary>
        /// To string for mouse state
        /// </summary>
        /// <returns>String with mouse details</returns>
        public new string ToString()
        {
            //Create string
            string s = "";
            //Update string with mouse information
            s += "Mouse" + Environment.NewLine;
            //Get current mouse state
            s += Mouse.GetState().ToString();
            //Insert new line
            s += Environment.NewLine;
            //Input direction
            s += "Input Direction: " + mouseAxisDirection.ToString();
            //Insert new line
            s += Environment.NewLine;
            //Mouse wheel direction
            s += "Mouse Wheel Direction: " + mouseWheelDirection.ToString();
            //Insert new line
            s += Environment.NewLine;
            //Mouse wheel velocity
            s += "Mouse wheel velocity: " + this.MouseWheelVelocity.ToString();
            //Insert new line
            s += Environment.NewLine;
            //Mouse velocity
            s += "Mouse velocity: " + this.MouseVelocity.ToString();
            //Insert new line
            s += Environment.NewLine;
            //Return string
            return s;
        }

        /// <summary>
        /// Get mouse position
        /// </summary>
        public Vector2 MousePosition
        {
            //Vector with mouse position
            get { return new Vector2(this.curMouseState.X, this.curMouseState.Y); }
        }

        /// <summary>
        /// Get mouse velocity 
        /// </summary>
        public Vector2 MouseVelocity
        {
            get
            {
                //Return the velocity of the mouse movement
                return (new Vector2(this.curMouseState.X, this.curMouseState.Y) - new Vector2(this.preMouseState.X, this.curMouseState.Y));
            }
        }

        /// <summary>
        /// Get new wheel postion
        /// </summary>
        public float MouseWheelPostion
        {
            //New wheel postion
            get { return this.curMouseState.ScrollWheelValue; }
        }

        /// <summary>
        /// Get mouse wheel velocity
        /// </summary>
        public float MouseWheelVelocity
        {
            //Return mouse wheel velocity
            get { return this.curMouseState.ScrollWheelValue - this.preMouseState.ScrollWheelValue; }
        }

        /// <summary>
        /// Determine if button is newly pressed
        /// </summary>
        /// <param name="button">Button to search for</param>
        /// <returns></returns>
        private bool IsNewPress(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LEFTBUTTON:
                    //Check left button
                    return (
                        this.preMouseState.LeftButton == ButtonState.Released &&
                        this.curMouseState.LeftButton == ButtonState.Pressed);
                case MouseButtons.MIDDLEBUTTON:
                    //Check middle button
                    return (
                        this.preMouseState.MiddleButton == ButtonState.Released &&
                        this.curMouseState.MiddleButton == ButtonState.Pressed);
                case MouseButtons.RIGHTBUTTON:
                    //Check right button
                    return (
                        this.preMouseState.RightButton == ButtonState.Released &&
                        this.curMouseState.RightButton == ButtonState.Pressed);
                case MouseButtons.EXTRABUTTON1:
                    //Check exrta button one
                    return (
                        this.preMouseState.XButton1 == ButtonState.Released &&
                        this.curMouseState.XButton1 == ButtonState.Pressed);
                case MouseButtons.EXTRABUTTON2:
                    //Check exrta button one
                    return (
                        this.preMouseState.XButton2 == ButtonState.Released &&
                        this.curMouseState.XButton2 == ButtonState.Pressed);
                default:
                    //Button does not exist therefore false 
                    return false;
            }
        }

        /// <summary>
        /// Determine if button is newly released
        /// </summary>
        /// <param name="button">Button to search for</param>
        /// <returns></returns>
        private bool IsOldPress(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.LEFTBUTTON:
                    //Check left button
                    return (
                        this.preMouseState.LeftButton == ButtonState.Pressed &&
                        this.curMouseState.LeftButton == ButtonState.Released);
                case MouseButtons.MIDDLEBUTTON:
                    //Check middle button
                    return (
                        this.preMouseState.MiddleButton == ButtonState.Pressed &&
                        this.curMouseState.MiddleButton == ButtonState.Released);
                case MouseButtons.RIGHTBUTTON:
                    //Check right button
                    return (
                        this.preMouseState.RightButton == ButtonState.Pressed &&
                        this.curMouseState.RightButton == ButtonState.Released);
                case MouseButtons.EXTRABUTTON1:
                    //Check exrta button one
                    return (
                        this.preMouseState.XButton1 == ButtonState.Pressed &&
                        this.curMouseState.XButton1 == ButtonState.Released);
                case MouseButtons.EXTRABUTTON2:
                    //Check exrta button two
                    return (
                        this.preMouseState.XButton2 == ButtonState.Pressed &&
                        this.curMouseState.XButton2 == ButtonState.Released);
                default:
                    //Button does not exist therefore false 
                    return false;
            }
        }

        /// <summary>
        /// Get button that are pressed
        /// </summary>
        /// <returns>Mouse button that are pressed</returns>
        public MouseButtons[] ButtonsPressed()
        {
            //Array of buttons
            return this.buttonPressed.ToArray(); ;
        }

        /// <summary>
        /// Get button that are released
        /// </summary>
        /// <returns>Mouse button that are pressed</returns>
        public MouseButtons[] ButtonsReleased()
        {
            //Array of buttons
            return this.buttonReleased.ToArray(); ;
        }

        /// <summary>
        /// Get mouse direction
        /// </summary>
        public InputDirection MouseAxisDirection
        {
            get { return this.mouseAxisDirection; }
        }

        /// <summary>
        /// Get mouse wheel direction
        /// </summary>
        public InputDirection MouseWheelDirection
        {
            get { return this.mouseWheelDirection; }
        }

        #endregion

    }
}
