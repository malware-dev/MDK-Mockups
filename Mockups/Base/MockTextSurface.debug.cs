using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Sandbox.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace IngameScript.Mockups.Base
{
    public class MockTextSurface : IMyTextSurface
    {
        const int MaxCharacterCount = 100000;

        readonly List<string> _fonts = new List<string>
        {
            "Debug",
            "Red",
            "Green",
            "Blue",
            "White",
            "DarkBlue",
            "UrlNormal",
            "UrlHighlight",
            "ErrorMessageboxCaption",
            "ErrorMessageBoxText",
            "InfoMessageBoxCaption",
            "InfoMessageBoxText",
            "ScreenCaption",
            "GameCredits",
            "LoadingScreen",
            "BuildInfo",
            "BuildInfoHighlight",
            "Monospace"
        };

        readonly List<string> _scripts = new List<string>
        {
        };

        List<string> _selectedImages = new List<string>();
        List<string> _sprites = new List<string>();
        MySpriteDrawFrame _spriteFrame = new MySpriteDrawFrame();
        StringBuilder _text = new StringBuilder();

        public string CurrentlyShownImage { get; private set; }
        
        public List<string> LoadedImages { get; } = new List<string>
        {
            "Offline",
            "Online",
            "Arrow",
            "Cross",
            "Danger",
            "No Entry",
            "Construction",
            "White screen"
        };

        public float FontSize { get; set; } = 1;
        public Color FontColor { get; set; } = new Color(255, 255, 255);
        public Color BackgroundColor { get; set; } = new Color(0, 0, 0);
        public byte BackgroundAlpha { get; set; } = new byte();
        public float ChangeInterval { get; set; } = 0;
        public string Font { get; set; } = "Debug";
        public TextAlignment Alignment { get; set; } = TextAlignment.LEFT;
        public string Script { get; set; } = "";
        public ContentType ContentType { get; set; } = ContentType.NONE;

        public Vector2 SurfaceSize { get; private set; } = new Vector2();

        public Vector2 TextureSize { get; private set; } = new Vector2();

        public bool PreserveAspectRatio { get; set; } = true;
        public float TextPadding { get; set; } = 0f;
        public Color ScriptBackgroundColor { get; set; } = new Color(255, 255, 255);
        public Color ScriptForegroundColor { get; set; } = new Color(0, 0, 0);

        public string Name { get; private set; }

        public string DisplayName { get; private set; }
        
        public virtual void AddImagesToSelection(List<string> ids, bool checkExistence = false)
        {
            Debug.Assert(ids != null, $"{nameof(ids)} cannot be null");
            foreach (var id in ids)
                AddImageToSelection(id, checkExistence);
        }

        public virtual void AddImageToSelection(string id, bool checkExistence = false)
        {
            Debug.Assert(id != null, $"{nameof(id)} cannot be null");
            if (checkExistence && _selectedImages.Contains(id))
                return;
            if (LoadedImages.Contains(id))
                _selectedImages.Add(id);
        }

        public virtual void ClearImagesFromSelection()
        {
            _selectedImages.Clear();
        }

        public virtual void GetFonts(List<string> fonts)
        {
            Debug.Assert(fonts != null, $"{nameof(fonts)} cannot be null!");
            fonts.Clear();
            foreach (var font in _fonts)
                fonts.Add(font);
        }

        public MySpriteDrawFrame DrawFrame() => _spriteFrame;
        public void GetScripts(List<string> scripts)
        {
            Debug.Assert(scripts != null, $"{nameof(scripts)} cannot be null!");
            scripts.Clear();
            foreach (var script in _script)
                scripts.Add(script);
        }

        public void GetSelectedImages(List<string> output)
        {
            Debug.Assert(output != null, $"{nameof(output)} cannot be null!");
            output.Clear();
            foreach (var image in _selectedImages)
                output.Add(image);
        }

        public void GetSprites(List<string> sprites)
        {
            Debug.Assert(sprites != null, $"{nameof(sprites)} cannot be null!");
            sprites.Clear();
            foreach (var image in _sprites)
                sprites.Add(image);
        }

        public string GetText() => _text.ToString();

        public Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale)
        {
            throw new NotImplementedException();
        }

        public void ReadText(StringBuilder buffer, bool append = false)
        {
            Debug.Assert(buffer != null, $"{nameof(buffer)} cannot be null");
            if (!append)
                buffer.Clear();
            buffer.AppendStringBuilder(_text);
        }

        public void RemoveImageFromSelection(string id, bool removeDuplicates = false)
        {
            Debug.Assert(id != null, $"{nameof(id)} cannot be null");
            if (removeDuplicates)
                _selectedImages.RemoveAll(img => img == id);
            else
                _selectedImages.Remove(id);
        }

        public void RemoveImagesFromSelection(List<string> ids, bool removeDuplicates = false)
        {
            Debug.Assert(ids != null, $"{nameof(ids)} cannot be null");
            foreach (var id in ids)
                RemoveImageFromSelection(id, removeDuplicates);
        }

        public bool WriteText(string value, bool append = false)
        {
            Debug.Assert(value != null, $"{nameof(value)} cannot be null");
            if (!append)
                _text.Clear();

            if (_text.Length + value.Length > MaxCharacterCount)
                value = value.Remove(MaxCharacterCount - _text.Length);
            _text.Append(value);

            return true;
        }

        public virtual bool WriteText(StringBuilder value, bool append = false)
        {
            Debug.Assert(value != null, $"{nameof(value)} cannot be null");
            if (!append)
                _text.Clear();

            if (_text.Length + value.Length > MaxCharacterCount)
                _text.AppendSubstring(value, 0, MaxCharacterCount);
            else
                _text.AppendStringBuilder(value);

            return true;
        }
    }
}
