using Apos.Input;
using Microsoft.Xna.Framework.Input;

namespace GameProject {
    public static class Triggers {
        public static ICondition ToggleFullscreen =
            new AllCondition(
                new KeyboardCondition(Keys.LeftAlt),
                new KeyboardCondition(Keys.Enter)
            );
        public static ICondition ToggleBorderless = new KeyboardCondition(Keys.F11);

        public static ICondition Back = new AnyCondition(
            new KeyboardCondition(Keys.Escape),
            new AnyGamePadCondition(GamePadButton.Back)
        );
    }
}
