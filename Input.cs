using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AposGameCheatSheet
{
    public static class Input
    {
        private static MouseState _oldMouse;
        private static MouseState _newMouse;
        private static KeyboardState _oldKeyboard;
        private static KeyboardState _newKeyboard;
        private static GamePadState _oldGamePad;
        private static GamePadState _newGamepad;
        private static GamePadCapabilities _capabilities;

        public static MouseState OldMouse => _oldMouse;
        public static MouseState NewMouse => _newMouse;
        public static KeyboardState OldKeyboard => _oldKeyboard;
        public static KeyboardState NewKeyboard => _newKeyboard;
        public static GamePadState OldGamePad => _oldGamePad;
        public static GamePadState NewGamePad => _newGamepad;
        public static GamePadCapabilities Capabilities => _capabilities;

        public static void Setup() {
            _newMouse = Mouse.GetState();
            _newKeyboard = Keyboard.GetState();
            _newGamepad = GamePad.GetState(PlayerIndex.One);
        }

        public static void Update() {
            _oldMouse = _newMouse;
            _oldKeyboard = _newKeyboard;
            _oldGamePad = _newGamepad;

            _newMouse = Mouse.GetState();
            _newKeyboard = Keyboard.GetState();
            _newGamepad = GamePad.GetState(PlayerIndex.One);
            _capabilities = GamePad.GetCapabilities(PlayerIndex.One);
        }
    }
}