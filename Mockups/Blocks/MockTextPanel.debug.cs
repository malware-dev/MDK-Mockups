using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace IngameScript.Mockups.Blocks
{
    public class MockTextPanel : MockFunctionalBlock, IMyTextPanel
    {
        const int MaxCharacterCount = 100000;

        List<string> _fonts = new List<string>
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

        StringBuilder _publicText = new StringBuilder();
        StringBuilder _publicTitle = new StringBuilder();
        List<string> _selectedImages = new List<string>();

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

        public virtual string CurrentlyShownImage { get; set; }

        [Obsolete("This property no has meaning in-game. If you need a secondary storage, use CustomData")]
        public virtual ShowTextOnScreenFlag ShowOnScreen { get; set; } = ShowTextOnScreenFlag.PUBLIC;

        public virtual bool ShowText { get; set; }

        public virtual float FontSize { get; set; }

        public virtual Color FontColor { get; set; }

        public virtual Color BackgroundColor { get; set; }

        public virtual float ChangeInterval
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual string Font { get; set; } = "Debug";

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

        [Obsolete("This method no longer have meaning in-game. If you need a secondary storage, use CustomData")]
        public virtual string GetPrivateText()
        {
            throw new NotSupportedException();
        }

        [Obsolete("This methods no longer have meaning in-game. If you need a secondary storage, use CustomData")]
        public virtual string GetPrivateTitle()
        {
            throw new NotSupportedException();
        }

        public virtual string GetPublicText() => _publicText.ToString();

        public virtual string GetPublicTitle() => _publicTitle.ToString();

        public virtual void GetSelectedImages(List<string> output)
        {
            Debug.Assert(output != null, $"{nameof(output)} cannot be null");
            output.Clear();
            foreach(var img in _selectedImages)
                output.Add(img);
        }

        public virtual void ReadPublicText(StringBuilder buffer, bool append = false)
        {
            Debug.Assert(buffer != null, $"{nameof(buffer)} cannot be null");
            if (!append)
                buffer.Clear();
            buffer.AppendStringBuilder(_publicText);
        }

        public virtual void RemoveImageFromSelection(string id, bool removeDuplicates = false)
        {
            Debug.Assert(id != null, $"{nameof(id)} cannot be null");
            if (removeDuplicates)
                _selectedImages.RemoveAll(img => img == id);
            else
                _selectedImages.Remove(id);
        }

        public virtual void RemoveImagesFromSelection(List<string> ids, bool removeDuplicates = false)
        {
            Debug.Assert(ids != null, $"{nameof(ids)} cannot be null");
            foreach (var id in ids)
                RemoveImageFromSelection(id, removeDuplicates);
        }

        [Obsolete("This method no longer have meaning in-game. If you need a secondary storage, use CustomData")]
        public virtual void SetShowOnScreen(ShowTextOnScreenFlag set)
        {
            ShowOnScreen = set;
        }

        [Obsolete("This method no longer have meaning in-game. If you need a secondary storage, use CustomData")]
        public virtual void ShowPrivateTextOnScreen()
        {
            throw new NotSupportedException();
        }

        public virtual void ShowPublicTextOnScreen()
        {
            ShowText = true;
        }

        public virtual void ShowTextureOnScreen()
        {
            ShowText = false;
        }

        [Obsolete("This method no longer have meaning in-game. If you need a secondary storage, use CustomData")]
        public virtual bool WritePrivateText(string value, bool append = false)
        {
            throw new NotSupportedException();
        }

        [Obsolete("This method no longer have meaning in-game. If you need a secondary storage, use CustomData")]
        public virtual bool WritePrivateTitle(string value, bool append = false)
        {
            throw new NotSupportedException();
        }

        public virtual bool WritePublicText(string value, bool append = false)
        {
            Debug.Assert(value != null, $"{nameof(value)} cannot be null");
            if (!append)
                _publicText.Clear();

            if (_publicText.Length + value.Length > MaxCharacterCount)
                value = value.Remove(MaxCharacterCount - _publicText.Length);
            _publicText.Append(value);

            return true;
        }

        public virtual bool WritePublicText(StringBuilder value, bool append = false)
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

        public virtual bool WritePublicTitle(string value, bool append = false)
        {
            Debug.Assert(value != null, $"{nameof(value)} cannot be null");
            if (!append)
                _publicTitle.Clear();
            _publicTitle.Append(value);

            return true;
        }
    }
}
