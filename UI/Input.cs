using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;

namespace RawENGINE
{
    /// <summary>
    /// GamePad controls expressed as one type, unified with button semantics.
    /// </summary>
    public enum GamePadButtons
    {
        Start,
        Back,
        A,
        B,
        X,
        Y,
        Up,
        Down,
        Left,
        Right,
        LeftShoulder,
        RightShoulder,
        LeftTrigger,
        RightTrigger,
    }

    public static class Input
    {


        /// <summary>
        /// The value of an analog control that reads as a "pressed button".
        /// </summary>
        const float analogLimit = 0.5f;

        private static Point prevMsPos;
        private static Point curMsPos;

        /// <summary>
        /// The state of the keyboard as of the last update.
        /// </summary>
        private static KeyboardState currentKeyboardState;
        /// <summary>
        /// The state of the keyboard as of the previous update.
        /// </summary>
        private static KeyboardState previousKeyboardState;

        private static MouseState prevMsState;
        private static MouseState curMsState;


        private static GamePadState currentGamePadStatePlayerOne;
        private static GamePadState previousGamePadStatePlayerOne;

        private static GamePadState currentGamePadStatePlayerTwo;
        private static GamePadState previousGamePadStatePlayerTwo;

        private static GamePadState currentGamePadStatePlayerThree;
        private static GamePadState previousGamePadStatePlayerThree;

        private static GamePadState currentGamePadStatePlayerFour;
        private static GamePadState previousGamePadStatePlayerFour;

        /// <summary>
        /// The state of the keyboard as of the last update.
        /// </summary>
        public static KeyboardState CurrentKeyboardState
        {
            get { return currentKeyboardState; }
        }
        public static Point PreviousMousePosition
        {
            get { return prevMsPos; }
        }
        public static Point CurrentMousePosition
        {
            get { return curMsPos; }
        }

        /// <summary>
        /// Check if a key is pressed.
        /// </summary>
        public static bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }


        /// <summary>
        /// Check if a key was just pressed in the most recent update.
        /// </summary>
        public static bool IsKeyTriggered(Keys key)
        {
            bool trigger = (currentKeyboardState.IsKeyDown(key)) &&
                (!previousKeyboardState.IsKeyDown(key));

            //currentKeyboardState = previousKeyboardState;

            return trigger;
        }

        /// <summary>
        /// The state of the gamepad as of the last update.
        /// </summary>
        public static GamePadState CurrentGamePadState
        {
            get { return currentGamePadStatePlayerOne; }
        }

        /// <summary>
        /// Check if the gamepad's Start button is pressed.
        /// </summary>
        public static bool IsGamePadStartPressed()
        {
            return (currentGamePadStatePlayerOne.Buttons.Start == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's Back button is pressed.
        /// </summary>
        public static bool IsGamePadBackPressed()
        {
            return (currentGamePadStatePlayerOne.Buttons.Back == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's A button is pressed.
        /// </summary>
        public static bool IsGamePadAPressed()
        {
            return (currentGamePadStatePlayerOne.Buttons.A == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's B button is pressed.
        /// </summary>
        public static bool IsGamePadBPressed()
        {
            return (currentGamePadStatePlayerOne.Buttons.B == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's X button is pressed.
        /// </summary>
        public static bool IsGamePadXPressed()
        {
            return (currentGamePadStatePlayerOne.Buttons.X == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's Y button is pressed.
        /// </summary>
        public static bool IsGamePadYPressed()
        {
            return (currentGamePadStatePlayerOne.Buttons.Y == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's LeftShoulder button is pressed.
        /// </summary>
        public static bool IsGamePadLeftShoulderPressed()
        {
            return (currentGamePadStatePlayerOne.Buttons.LeftShoulder == ButtonState.Pressed);
        }


        /// <summary>
        /// <summary>
        /// Check if the gamepad's RightShoulder button is pressed.
        /// </summary>
        public static bool IsGamePadRightShoulderPressed()
        {
            return (currentGamePadStatePlayerOne.Buttons.RightShoulder == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if Up on the gamepad's directional pad is pressed.
        /// </summary>
        public static bool IsGamePadDPadUpPressed()
        {
            return (currentGamePadStatePlayerOne.DPad.Up == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if Down on the gamepad's directional pad is pressed.
        /// </summary>
        public static bool IsGamePadDPadDownPressed()
        {
            return (currentGamePadStatePlayerOne.DPad.Down == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if Left on the gamepad's directional pad is pressed.
        /// </summary>
        public static bool IsGamePadDPadLeftPressed()
        {
            return (currentGamePadStatePlayerOne.DPad.Left == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if Right on the gamepad's directional pad is pressed.
        /// </summary>
        public static bool IsGamePadDPadRightPressed()
        {
            return (currentGamePadStatePlayerOne.DPad.Right == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's left trigger is pressed.
        /// </summary>
        public static bool IsGamePadLeftTriggerPressed()
        {
            return (currentGamePadStatePlayerOne.Triggers.Left > analogLimit);
        }


        /// <summary>
        /// Check if the gamepad's right trigger is pressed.
        /// </summary>
        public static bool IsGamePadRightTriggerPressed()
        {
            return (currentGamePadStatePlayerOne.Triggers.Right > analogLimit);
        }


        /// <summary>
        /// Check if Up on the gamepad's left analog stick is pressed.
        /// </summary>
        public static bool IsGamePadLeftStickUpPressed()
        {
            return (currentGamePadStatePlayerOne.ThumbSticks.Left.Y > analogLimit);
        }


        /// <summary>
        /// Check if Down on the gamepad's left analog stick is pressed.
        /// </summary>
        public static bool IsGamePadLeftStickDownPressed()
        {
            return (-1f * currentGamePadStatePlayerOne.ThumbSticks.Left.Y > analogLimit);
        }


        /// <summary>
        /// Check if Left on the gamepad's left analog stick is pressed.
        /// </summary>
        public static bool IsGamePadLeftStickLeftPressed()
        {
            return (-1f * currentGamePadStatePlayerOne.ThumbSticks.Left.X > analogLimit);
        }


        /// <summary>
        /// Check if Right on the gamepad's left analog stick is pressed.
        /// </summary>
        public static bool IsGamePadLeftStickRightPressed()
        {
            return (currentGamePadStatePlayerOne.ThumbSticks.Left.X > analogLimit);
        }

        public static bool GetFirstOrSecondOrThirdOrDefaultOfMouseStateXYWhereRectangleIntersects(Rectangle rect)
        {
            Rectangle cur = new Rectangle(curMsState.X, curMsState.Y, 1, 1);
            return (cur.Intersects(rect) && ((curMsState.LeftButton == ButtonState.Pressed) || (curMsState.RightButton == ButtonState.Pressed) || (curMsState.MiddleButton == ButtonState.Pressed)));
        }

        /// <summary>
        /// Check if the GamePadKey value specified is pressed.
        /// </summary>
        public static bool IsGamePadButtonPressed(GamePadButtons gamePadKey)
        {
            switch (gamePadKey)
            {
                case GamePadButtons.Start:
                    return IsGamePadStartPressed();

                case GamePadButtons.Back:
                    return IsGamePadBackPressed();

                case GamePadButtons.A:
                    return IsGamePadAPressed();

                case GamePadButtons.B:
                    return IsGamePadBPressed();

                case GamePadButtons.X:
                    return IsGamePadXPressed();

                case GamePadButtons.Y:
                    return IsGamePadYPressed();

                case GamePadButtons.LeftShoulder:
                    return IsGamePadLeftShoulderPressed();

                case GamePadButtons.RightShoulder:
                    return IsGamePadRightShoulderPressed();

                case GamePadButtons.LeftTrigger:
                    return IsGamePadLeftTriggerPressed();

                case GamePadButtons.RightTrigger:
                    return IsGamePadRightTriggerPressed();

                case GamePadButtons.Up:
                    return IsGamePadDPadUpPressed() ||
                        IsGamePadLeftStickUpPressed();

                case GamePadButtons.Down:
                    return IsGamePadDPadDownPressed() ||
                        IsGamePadLeftStickDownPressed();

                case GamePadButtons.Left:
                    return IsGamePadDPadLeftPressed() ||
                        IsGamePadLeftStickLeftPressed();

                case GamePadButtons.Right:
                    return IsGamePadDPadRightPressed() ||
                        IsGamePadLeftStickRightPressed();
            }

            return false;
        }


        /// <summary>
        /// Check if the gamepad's Start button was just pressed.
        /// </summary>
        public static bool IsGamePadStartTriggered()
        {
            return ((currentGamePadStatePlayerOne.Buttons.Start == ButtonState.Pressed) &&
              (previousGamePadStatePlayerOne.Buttons.Start == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's Back button was just pressed.
        /// </summary>
        public static bool IsGamePadBackTriggered()
        {
            return ((currentGamePadStatePlayerOne.Buttons.Back == ButtonState.Pressed) &&
              (previousGamePadStatePlayerOne.Buttons.Back == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's A button was just pressed.
        /// </summary>
        public static bool IsGamePadATriggered()
        {
            return ((currentGamePadStatePlayerOne.Buttons.A == ButtonState.Pressed) &&
              (previousGamePadStatePlayerOne.Buttons.A == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's B button was just pressed.
        /// </summary>
        public static bool IsGamePadBTriggered()
        {
            return ((currentGamePadStatePlayerOne.Buttons.B == ButtonState.Pressed) &&
              (previousGamePadStatePlayerOne.Buttons.B == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's X button was just pressed.
        /// </summary>
        public static bool IsGamePadXTriggered()
        {
            return ((currentGamePadStatePlayerOne.Buttons.X == ButtonState.Pressed) &&
              (previousGamePadStatePlayerOne.Buttons.X == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's Y button was just pressed.
        /// </summary>
        public static bool IsGamePadYTriggered()
        {
            return ((currentGamePadStatePlayerOne.Buttons.Y == ButtonState.Pressed) &&
              (previousGamePadStatePlayerOne.Buttons.Y == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's LeftShoulder button was just pressed.
        /// </summary>
        public static bool IsGamePadLeftShoulderTriggered()
        {
            return (
                (currentGamePadStatePlayerOne.Buttons.LeftShoulder == ButtonState.Pressed) &&
                (previousGamePadStatePlayerOne.Buttons.LeftShoulder == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's RightShoulder button was just pressed.
        /// </summary>
        public static bool IsGamePadRightShoulderTriggered()
        {
            return (
                (currentGamePadStatePlayerOne.Buttons.RightShoulder == ButtonState.Pressed) &&
                (previousGamePadStatePlayerOne.Buttons.RightShoulder == ButtonState.Released));
        }


        /// <summary>
        /// Check if Up on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool IsGamePadDPadUpTriggered()
        {
            return ((currentGamePadStatePlayerOne.DPad.Up == ButtonState.Pressed) &&
              (previousGamePadStatePlayerOne.DPad.Up == ButtonState.Released));
        }


        /// <summary>
        /// Check if Down on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool IsGamePadDPadDownTriggered()
        {
            return ((currentGamePadStatePlayerOne.DPad.Down == ButtonState.Pressed) &&
              (previousGamePadStatePlayerOne.DPad.Down == ButtonState.Released));
        }


        /// <summary>
        /// Check if Left on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool IsGamePadDPadLeftTriggered()
        {
            return ((currentGamePadStatePlayerOne.DPad.Left == ButtonState.Pressed) &&
              (previousGamePadStatePlayerOne.DPad.Left == ButtonState.Released));
        }

        public static bool IsLeftMouseButtonClicked()
        {
            bool result = ((prevMsState.LeftButton == ButtonState.Released && curMsState.LeftButton == ButtonState.Pressed));
            return result;
        }
        public static bool IsRightMouseButtonClicked()
        {
            bool result = ((prevMsState.RightButton == ButtonState.Released && curMsState.RightButton == ButtonState.Pressed));
            return result;
        }
        /// <summary>
        /// Check if Right on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool IsGamePadDPadRightTriggered()
        {
            return ((currentGamePadStatePlayerOne.DPad.Right == ButtonState.Pressed) &&
              (previousGamePadStatePlayerOne.DPad.Right == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's left trigger was just pressed.
        /// </summary>
        public static bool IsGamePadLeftTriggerTriggered()
        {
            return ((currentGamePadStatePlayerOne.Triggers.Left > analogLimit) &&
                (previousGamePadStatePlayerOne.Triggers.Left < analogLimit));
        }


        /// <summary>
        /// Check if the gamepad's right trigger was just pressed.
        /// </summary>
        public static bool IsGamePadRightTriggerTriggered()
        {
            return ((currentGamePadStatePlayerOne.Triggers.Right > analogLimit) &&
                (previousGamePadStatePlayerOne.Triggers.Right < analogLimit));
        }


        /// <summary>
        /// Check if Up on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool IsGamePadLeftStickUpTriggered()
        {
            return ((currentGamePadStatePlayerOne.ThumbSticks.Left.Y > analogLimit) &&
                (previousGamePadStatePlayerOne.ThumbSticks.Left.Y < analogLimit));
        }


        /// <summary>
        /// Check if Down on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool IsGamePadLeftStickDownTriggered()
        {
            return ((-1f * currentGamePadStatePlayerOne.ThumbSticks.Left.Y > analogLimit) &&
                (-1f * previousGamePadStatePlayerOne.ThumbSticks.Left.Y < analogLimit));
        }


        /// <summary>
        /// Check if Left on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool IsGamePadLeftStickLeftTriggered()
        {
            return ((-1f * currentGamePadStatePlayerOne.ThumbSticks.Left.X > analogLimit) &&
                (-1f * previousGamePadStatePlayerOne.ThumbSticks.Left.X < analogLimit));
        }


        /// <summary>
        /// Check if Right on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool IsGamePadLeftStickRightTriggered()
        {
            return ((currentGamePadStatePlayerOne.ThumbSticks.Left.X > analogLimit) &&
                (previousGamePadStatePlayerOne.ThumbSticks.Left.X < analogLimit));
        }

        public static bool IsGamePadButtonTriggered(GamePadButtons gamePadKey)
        {
            switch (gamePadKey)
            {
                case GamePadButtons.Start:
                    return IsGamePadStartTriggered();

                case GamePadButtons.Back:
                    return IsGamePadBackTriggered();

                case GamePadButtons.A:
                    return IsGamePadATriggered();

                case GamePadButtons.B:
                    return IsGamePadBTriggered();

                case GamePadButtons.X:
                    return IsGamePadXTriggered();

                case GamePadButtons.Y:
                    return IsGamePadYTriggered();

                case GamePadButtons.LeftShoulder:
                    return IsGamePadLeftShoulderTriggered();

                case GamePadButtons.RightShoulder:
                    return IsGamePadRightShoulderTriggered();

                case GamePadButtons.LeftTrigger:
                    return IsGamePadLeftTriggerTriggered();

                case GamePadButtons.RightTrigger:
                    return IsGamePadRightTriggerTriggered();

                case GamePadButtons.Up:
                    return IsGamePadDPadUpTriggered() ||
                        IsGamePadLeftStickUpTriggered();

                case GamePadButtons.Down:
                    return IsGamePadDPadDownTriggered() ||
                        IsGamePadLeftStickDownTriggered();

                case GamePadButtons.Left:
                    return IsGamePadDPadLeftTriggered() ||
                        IsGamePadLeftStickLeftTriggered();

                case GamePadButtons.Right:
                    return IsGamePadDPadRightTriggered() ||
                        IsGamePadLeftStickRightTriggered();
            }

            return false;
        }



        /// <summary>
        /// Updates the keyboard and gamepad control states.
        /// </summary>
        public static void Update()
        {
            // update the keyboard state
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            curMsPos = new Point(curMsState.X, curMsState.Y);
            prevMsPos = new Point(prevMsState.X, prevMsState.Y);

            prevMsState = curMsState;
            curMsState = Mouse.GetState();

            // update the gamepad state
            previousGamePadStatePlayerOne = currentGamePadStatePlayerOne;
            currentGamePadStatePlayerOne = GamePad.GetState(PlayerIndex.One);

            // update the gamepad state
            previousGamePadStatePlayerTwo = currentGamePadStatePlayerTwo;
            currentGamePadStatePlayerTwo = GamePad.GetState(PlayerIndex.Two);

            // update the gamepad state
            previousGamePadStatePlayerThree = currentGamePadStatePlayerThree;
            currentGamePadStatePlayerThree = GamePad.GetState(PlayerIndex.Three);

            // update the gamepad state
            previousGamePadStatePlayerFour = currentGamePadStatePlayerFour;
            currentGamePadStatePlayerFour = GamePad.GetState(PlayerIndex.Four);


        }
    }
}
