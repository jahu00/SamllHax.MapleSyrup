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

        private static List<InputAction> resetActions = new List<InputAction>();
        private KeyboardState _keyboardState;

        public InputEvents()
        {
            // TODO: Read keys from config or settings
        }

        public InputEvents Init(KeyboardState keyboardState)
        {
            _keyboardState = keyboardState;
            foreach(var resetAction in resetActions.ToArray())
            {
                if (_keyboardState.IsKeyPressed(inputActionMapping[resetAction]))
                {
                    resetActions.Remove(resetAction);
                }
            }
            return this;
        }

        public bool IsDown(InputAction inputAction) => !resetActions.Contains(inputAction) && _keyboardState.IsKeyDown(inputActionMapping[inputAction]);
        public bool IsPressed(InputAction inputAction) => _keyboardState.IsKeyPressed(inputActionMapping[inputAction]);
        public bool IsUp(InputAction inputAction) => _keyboardState.IsKeyReleased(inputActionMapping[inputAction]);
        public void Reset(InputAction inputAction)
        {
            if (resetActions.Contains(inputAction))
            {
                return;
            }
            resetActions.Add(inputAction);
        }
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
