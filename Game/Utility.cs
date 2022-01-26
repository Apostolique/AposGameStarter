using System;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject {
    static class Utility {
        public static Settings Settings;
        public static Game Game;
        public static GameWindow Window => Game.Window;
        public static GraphicsDeviceManager Graphics { get; set; }

        public static bool ShowLine = false;

        public static void ToggleFullscreen() {
            bool oldIsFullscreen = Settings.IsFullscreen;

            if (Settings.IsBorderless) {
                Settings.IsBorderless = false;
            } else {
                Settings.IsFullscreen = !Settings.IsFullscreen;
            }

            ApplyFullscreenChange(oldIsFullscreen);
        }
        public static void ToggleBorderless() {
            bool oldIsFullscreen = Settings.IsFullscreen;

            Settings.IsBorderless = !Settings.IsBorderless;
            Settings.IsFullscreen = Settings.IsBorderless;

            ApplyFullscreenChange(oldIsFullscreen);
        }

        public static string GetPath(string name) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name);
        public static T LoadJson<T>(string name) where T : new() {
            T json;
            string jsonPath = GetPath(name);

            if (File.Exists(jsonPath)) {
                json = JsonSerializer.Deserialize<T>(File.ReadAllText(jsonPath), _options);
            } else {
                json = new T();
            }

            return json;
        }
        public static void SaveJson<T>(string name, T json) {
            string jsonPath = GetPath(name);
            string jsonString = JsonSerializer.Serialize(json, _options);
            File.WriteAllText(jsonPath, jsonString);
        }
        public static T EnsureJson<T>(string name) where T : new() {
            T json;
            string jsonPath = GetPath(name);

            if (File.Exists(jsonPath)) {
                json = JsonSerializer.Deserialize<T>(File.ReadAllText(jsonPath), _options);
            } else {
                json = new T();
                string jsonString = JsonSerializer.Serialize(json, _options);
                File.WriteAllText(jsonPath, jsonString);
            }

            return json;
        }

        public static void SaveWindow() {
            Settings.X = Window.ClientBounds.X;
            Settings.Y = Window.ClientBounds.Y;
            Settings.Width = Window.ClientBounds.Width;
            Settings.Height = Window.ClientBounds.Height;
        }
        public static void RestoreWindow() {
            Window.Position = new Point(Settings.X, Settings.Y);
            Graphics.PreferredBackBufferWidth = Settings.Width;
            Graphics.PreferredBackBufferHeight = Settings.Height;
            Graphics.ApplyChanges();
        }

        public static void ApplyFullscreenChange(bool oldIsFullscreen) {
            if (Settings.IsFullscreen) {
                if (oldIsFullscreen) {
                    ApplyHardwareMode();
                } else {
                    SetFullscreen();
                }
            } else {
                UnsetFullscreen();
            }
        }
        private static void ApplyHardwareMode() {
            Graphics.HardwareModeSwitch = !Settings.IsBorderless;
            Graphics.ApplyChanges();
        }
        private static void SetFullscreen() {
            SaveWindow();

            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Graphics.HardwareModeSwitch = !Settings.IsBorderless;

            Graphics.IsFullScreen = true;
            Graphics.ApplyChanges();
        }
        private static void UnsetFullscreen() {
            Graphics.IsFullScreen = false;
            RestoreWindow();
        }

        private static JsonSerializerOptions _options = new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };
    }
}
