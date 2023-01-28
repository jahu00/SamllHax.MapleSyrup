using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Draw
{
    public class InputEvents
    {
        private static Dictionary<InputAction, Keys> inputActionMapping = new Dictionary<InputAction, Keys>()
        {
            { InputAction.Left, Keys.Left },
            { InputAction.Right, Keys.Right },
            { InputAction.Up, Keys.Up },
            { InputAction.Down, Keys.Down },
            { InputAction.Jump, Keys.LeftAlt },
            { InputAction.UsePortal, Keys.Up },
        };
        private KeyboardState _keyboardState;

        public InputEvents()
        {
            // TODO: Read keys from config or settings
        }

        public InputEvents Init(KeyboardState keyboardState)
        {
            _keyboardState = keyboardState;
            return this;
        }

        public bool IsDown(InputAction inputAction) => _keyboardState.IsKeyDown(inputActionMapping[inputAction]);
        public bool IsPressed(InputAction inputAction) => _keyboardState.IsKeyPressed(inputActionMapping[inputAction]);
    }

    public enum InputAction
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3,
        Jump = 4,
        JumpDown = 5,
        UsePortal = 6,
    }
}
