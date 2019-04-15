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

        List<string> _selectedImages = new List<string>();
        MySpriteDrawFrame _spriteFrame = new MySpriteDrawFrame();
        StringBuilder _publicText = new StringBuilder();

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
            throw new NotImplementedException();
        }

        public void GetSelectedImages(List<string> output)
        {
            throw new NotImplementedException();
        }

        public void GetSprites(List<string> sprites)
        {
            throw new NotImplementedException();
        }

        public string GetText()
        {
            throw new NotImplementedException();
        }

        public Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale)
        {
            throw new NotImplementedException();
        }

        public void ReadText(StringBuilder buffer, bool append = false)
        {
            throw new NotImplementedException();
        }

        public void RemoveImageFromSelection(string id, bool removeDuplicates = false)
        {
            throw new NotImplementedException();
        }

        public void RemoveImagesFromSelection(List<string> ids, bool removeDuplicates = false)
        {
            throw new NotImplementedException();
        }

        public bool WriteText(string value, bool append = false)
        {
            throw new NotImplementedException();
        }

        public virtual bool WriteText(StringBuilder value, bool append = false)
        {
            Debug.Assert(value != null, $"{nameof(value)} cannot be null");
            if (!append)
                _publicText.Clear();

            if (_publicText.Length + value.Length > MaxCharacterCount)
                _publicText.AppendSubstring(value, 0, MaxCharacterCount);
            else
                _publicText.AppendStringBuilder(value);

            return true;
        }
    }
}
