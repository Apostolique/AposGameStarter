using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AposGameCheatSheet
{
    public class Input
    {
        public Input() {
            _newMouse = Mouse.GetState();
            _newKeyboard = Keyboard.GetState();
            _newGamepad = GamePad.GetState(PlayerIndex.One);
        }
        MouseState _oldMouse;
        MouseState _newMouse;
        KeyboardState _oldKeyboard;
        KeyboardState _newKeyboard;
        GamePadState _oldGamePad;
        GamePadState _newGamepad;
        GamePadCapabilities _capabilities;

        public MouseState OldMouse => _oldMouse;
        public MouseState NewMouse => _newMouse;
        public KeyboardState OldKeyboard => _oldKeyboard;
        public KeyboardState NewKeyboard => _newKeyboard;
        public GamePadState OldGamePad => _oldGamePad;
        public GamePadState NewGamePad => _newGamepad;
        public GamePadCapabilities Capabilities => _capabilities;

        public void Update() {
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