using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using IngameScript.Mockups.Base;
using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace IngameScript.Mockups.Blocks
{
    public class MockTextPanel : MockFunctionalBlock, IMyTextPanel
    {
        readonly MockTextSurface _surface = new MockTextSurface(new Vector2(512, 512), new Vector2(512, 512));
        readonly StringBuilder _publicTitle = new StringBuilder();
        TextAlignmentEnum _alignment = TextAlignmentEnum.Align_Left;

        public virtual string CurrentlyShownImage
        {
            get { return _surface.CurrentlyShownImage; }
            set { _surface.CurrentlyShownImage = value; }
        }

        [Obsolete("This property no has meaning in-game. If you need a secondary storage, use CustomData")]
        public virtual ShowTextOnScreenFlag ShowOnScreen { get; set; } = ShowTextOnScreenFlag.PUBLIC;

        public bool ShowText => _surface.ContentType == ContentType.TEXT_AND_IMAGE;

        public virtual float FontSize
        {
            get { return _surface.FontSize; }
            set { _surface.FontSize = value; }
        }

        public virtual Color FontColor
        {
            get { return _surface.FontColor; }
            set { _surface.FontColor = value; }
        }

        public virtual Color BackgroundColor
        {
            get { return _surface.BackgroundColor; }
            set { _surface.BackgroundColor = value; }
        }

        public virtual float ChangeInterval
        {
            get { return _surface.ChangeInterval; }
            set { _surface.ChangeInterval = value; }
        }

        public virtual string Font
        {
            get { return _surface.Font; }
            set { _surface.Font = value; }
        }

        public byte BackgroundAlpha
        {
            get { return _surface.BackgroundAlpha; }
            set { _surface.BackgroundAlpha = value; }
        }

        public TextAlignment Alignment
        {
            get { return _surface.Alignment; }
            set { _surface.Alignment = value; }
        }

        public string Script
        {
            get { return _surface.Script; }
            set { _surface.Script = value; }
        }

        public ContentType ContentType
        {
            get { return _surface.ContentType; }
            set { _surface.ContentType = value; }
        }

        public bool PreserveAspectRatio
        {
            get { return _surface.PreserveAspectRatio; }
            set { _surface.PreserveAspectRatio = value; }
        }

        public float TextPadding
        {
            get { return _surface.TextPadding; }
            set { _surface.TextPadding = value; }
        }

        public Color ScriptBackgroundColor
        {
            get { return _surface.ScriptBackgroundColor; }
            set { _surface.ScriptBackgroundColor = value; }
        }

        public Color ScriptForegroundColor
        {
            get { return _surface.ScriptForegroundColor; }
            set { _surface.ScriptForegroundColor = value; }
        }

        public Vector2 SurfaceSize => _surface.SurfaceSize;

        public Vector2 TextureSize => _surface.TextureSize;

        protected override IEnumerable<ITerminalProperty> CreateTerminalProperties()
        {
            return base.CreateTerminalProperties().Concat(new ITerminalProperty[]
            {
                new MockTerminalProperty<IMyTextPanel, int>("alignment", b => (int)_alignment, (b, v) => _alignment = (TextAlignmentEnum)v, 1),
                new MockTerminalProperty<IMyTextPanel, float>("FontSize", b => b.FontSize, (b, v) => b.FontSize = v, 1),
                new MockTerminalProperty<IMyTextPanel, Color>("FontColor", b => b.FontColor, (b, v) => b.FontColor = v, Color.White),
                new MockTerminalProperty<IMyTextPanel, Color>("BackgroundColor", b => b.BackgroundColor, (b, v) => b.BackgroundColor = v, Color.Black),
                new MockTerminalProperty<IMyTextPanel, float>("ChangeIntervalSlider", b => b.ChangeInterval, (b, v) => b.ChangeInterval = v),
                
                new MockTerminalProperty<IMyTextPanel, long>("Font", b =>
                {
                    throw new NotImplementedException("Sorry, don't know how the ID is generated");
                }, (b, v) =>
                {
                    throw new NotImplementedException("Sorry, don't know how the ID is generated");
                }),

                // Ugh... >_<
                new MockTerminalProperty<IMyTerminalBlock, StringBuilder>("Title", b => _publicTitle, (b, v) =>
                {
                    _publicTitle.Clear();
                    _publicTitle.Append(v);
                }),
            });
        }

        public virtual void AddImagesToSelection(List<string> ids, bool checkExistence = false)
            => _surface.AddImagesToSelection(ids, checkExistence);

        public virtual void AddImageToSelection(string id, bool checkExistence = false) 
            => _surface.AddImageToSelection(id, checkExistence);

        public virtual void ClearImagesFromSelection() 
            => _surface.ClearImagesFromSelection();

        public virtual void GetFonts(List<string> fonts) 
            => _surface.GetFonts(fonts);
        

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

        public virtual string GetPublicText() => _surface.GetText();

        public virtual string GetPublicTitle() => _publicTitle.ToString();

        public virtual void GetSelectedImages(List<string> output) 
            => _surface.GetSelectedImages(output);

        public virtual void ReadPublicText(StringBuilder buffer, bool append = false)
            => _surface.ReadText(buffer, append);

        public virtual void RemoveImageFromSelection(string id, bool removeDuplicates = false)
            => _surface.RemoveImageFromSelection(id, removeDuplicates);

        public virtual void RemoveImagesFromSelection(List<string> ids, bool removeDuplicates = false)
            => _surface.RemoveImagesFromSelection(ids, removeDuplicates);

        [Obsolete("This method no longer have meaning in-game. If you need a secondary storage, use CustomData")]
        public virtual void SetShowOnScreen(ShowTextOnScreenFlag set)
        {
            throw new NotSupportedException();
        }

        [Obsolete("This method no longer have meaning in-game. If you need a secondary storage, use CustomData")]
        public virtual void ShowPrivateTextOnScreen()
        {
            throw new NotSupportedException();
        }

        [Obsolete]
        public virtual void ShowPublicTextOnScreen()
        {
            throw new NotSupportedException();
        }

        [Obsolete]
        public virtual void ShowTextureOnScreen()
        {
            throw new NotSupportedException();
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
            => _surface.WriteText(value, append);

        public virtual bool WritePublicText(StringBuilder value, bool append = false)
            => _surface.WriteText(value, append);

        public virtual bool WritePublicTitle(string value, bool append = false)
        {
            Debug.Assert(value != null, $"{nameof(value)} cannot be null");
            if (!append)
                _publicTitle.Clear();

            _publicTitle.Append(value);

            return true;
        }

        public bool WriteText(string value, bool append = false) => _surface.WriteText(value, append);
        public string GetText() => _surface.GetText();
        public bool WriteText(StringBuilder value, bool append = false) => _surface.WriteText(value, append);
        public void ReadText(StringBuilder buffer, bool append = false) => _surface.ReadText(buffer, append);
        public void GetSprites(List<string> sprites) => _surface.GetSprites(sprites);
        public void GetScripts(List<string> scripts) => _surface.GetScripts(scripts);
        public MySpriteDrawFrame DrawFrame() => _surface.DrawFrame();
        public Vector2 MeasureStringInPixels(StringBuilder text, string font, float scale) => _surface.MeasureStringInPixels(text, font, scale);
    }
}
