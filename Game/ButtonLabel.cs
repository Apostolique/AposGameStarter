using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace Apos.Gui {
    public class ButtonLabel : Component {
        public ButtonLabel(int id, string text, int fontSize, Color c) : base(id) {
            Text = text;
            FontSize = fontSize;
            Color = c;
        }

        public string Text {
            get => _text;
            set {
                if (value != _text) {
                    _text = value;
                    _isDirty = true;
                }
            }
        }
        public int Padding { get; set; } = 20;
        public Color Color { get; set; }
        public int FontSize {
            get => _fontSize;
            set {
                if (value != _fontSize) {
                    _fontSize = value;
                    _isDirty = true;
                }
            }
        }

        public bool Clicked { get; set; } = false;
        public override bool IsFocusable { get; set; } = true;
        public override bool IsFocused {
            get => base.IsFocused;
            set {
                base.IsFocused = value;
                if (!value) {
                    _mousePressed = false;
                    _hovered = false;
                    _buttonPressed = false;
                }
            }
        }

        public override void UpdateSetup(GameTime gameTime) {
            if (Clicked) {
                Clicked = false;
            }
        }
        public override void UpdateInput(GameTime gameTime) {
            if (Clip.Contains(GuiHelper.Mouse)) {
                _hovered = true;
                if (Default.MouseInteraction.Pressed()) {
                    _mousePressed = true;
                    Root.GrabFocus(this);
                }
            } else {
                _hovered = false;
            }

            if (IsFocused) {
                if (_mousePressed) {
                    if (Default.MouseInteraction.Released()) {
                        if (Clip.Contains(GuiHelper.Mouse))
                            Clicked = true;
                        _mousePressed = false;
                    } else {
                        Default.MouseInteraction.Consume();
                    }
                }

                if (Default.ButtonInteraction.Pressed()) {
                    _buttonPressed = true;
                } else if (_buttonPressed) {
                    if (Default.ButtonInteraction.Released()) {
                        Clicked = true;
                        _buttonPressed = false;
                    } else {
                        Default.ButtonInteraction.Consume();
                    }
                }
            }
        }

        public override void UpdatePrefSize(GameTime gameTime) {
            Cache();
            PrefWidth = _cachedSize.X + Padding * 2;
            PrefHeight = _cachedSize.Y + Padding * 2;
        }

        public override void Draw(GameTime gameTime) {
            GuiHelper.PushScissor(Clip);

            var font = GuiHelper.GetFont(_fontSize);
            var c = _hovered || IsFocused ? Color.White : Color;
            GuiHelper.ShapeBatch.DrawString(font, _text, XY + new Vector2(Padding), c, scale: GuiHelper.FontScale);

            GuiHelper.PopScissor();
        }

        protected void Cache() {
            if (_isDirty) {
                _cachedSize = GuiHelper.MeasureString(_text, _fontSize);
                _isDirty = false;
            }
        }

        protected string _text = null!;
        protected int _fontSize;
        protected Vector2 _cachedSize;
        protected bool _isDirty = false;

        protected bool _mousePressed = false;
        protected bool _buttonPressed = false;
        protected bool _hovered = false;

        public static ButtonLabel Put(string text, int fontSize = 30, Color? color = null, [CallerLineNumber] int id = 0, bool isAbsoluteId = false) {
            id = IMGUI.TryCreateId(id, isAbsoluteId, out IComponent? c);

            color ??= new Color(200, 200, 200);

            ButtonLabel a;
            if (c is ButtonLabel d) {
                a = d;
                a.Text = text;
                a.FontSize = fontSize;
                a.Color = color.Value;
            } else {
                a = new ButtonLabel(id, text, fontSize, color.Value);
            }

            GuiHelper.CurrentIMGUI.GrabParent(a);

            return a;
        }
    }
}
