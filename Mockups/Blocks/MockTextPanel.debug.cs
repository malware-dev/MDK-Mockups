using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using IngameScript.Mockups.Base;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage.Game.GUI.TextPanel;
using VRageMath;
using IMyTextPanel = Sandbox.ModAPI.Ingame.IMyTextPanel;

namespace IngameScript.Mockups.Blocks
{
  public class MockTextPanel : MockFunctionalBlock, IMyTextPanel
  {

    List<string> _fonts = new List<string>() {
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

    public virtual string CurrentlyShownImage
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public virtual ShowTextOnScreenFlag ShowOnScreen { get; set; }

    public virtual bool ShowText => ShowOnScreen != ShowTextOnScreenFlag.NONE;

    public virtual float FontSize { get; set; }

    public virtual Color FontColor { get; set; }

    public virtual Color BackgroundColor { get; set; }

    public virtual float ChangeInterval { get; set; }

    public virtual string Font { get; set; }

    public virtual void AddImagesToSelection(List<string> ids, bool checkExistence = false)
    {
      throw new NotImplementedException();
    }

    public virtual void AddImageToSelection(string id, bool checkExistence = false)
    {
      throw new NotImplementedException();
    }

    public virtual void ClearImagesFromSelection()
    {
      throw new NotImplementedException();
    }

    public virtual void GetFonts(List<string> fonts)
    {
      Debug.Assert(fonts != null, "List<string> fonts cannot be null!");
      fonts.Clear();

      for (int i = 0; i < _fonts.Count; i++)
        fonts.Add(_fonts[i]);
    }

    public virtual string GetPrivateText()
    {
      throw new NotImplementedException();
    }

    public virtual string GetPrivateTitle()
    {
      throw new NotImplementedException();
    }

    public virtual string GetPublicText() => _publicText.ToString();

    public virtual string GetPublicTitle() => _publicTitle.ToString();

    public virtual void GetSelectedImages(List<string> output)
    {
      throw new NotImplementedException();
    }

    public virtual void ReadPublicText(StringBuilder buffer, bool append = false)
    {
      if (append)
        buffer.AppendStringBuilder(_publicText);

      else {
        buffer.Clear()
          .AppendStringBuilder(_publicText);
      }
    }

    public virtual void RemoveImageFromSelection(string id, bool removeDuplicates = false)
    {
      throw new NotImplementedException();
    }

    public virtual void RemoveImagesFromSelection(List<string> ids, bool removeDuplicates = false)
    {
      throw new NotImplementedException();
    }

    public virtual void SetShowOnScreen(ShowTextOnScreenFlag set)
    {
      if (ShowOnScreen != set)
        ShowOnScreen = set;
    }

    public virtual void ShowPrivateTextOnScreen()
    {
      throw new NotImplementedException();
    }

    public virtual void ShowPublicTextOnScreen()
    {
      SetShowOnScreen(ShowTextOnScreenFlag.PUBLIC);
    }

    public virtual void ShowTextureOnScreen()
    {
      throw new NotImplementedException();
    }

    public virtual bool WritePrivateText(string value, bool append = false)
    {
      throw new NotImplementedException();
    }

    public virtual bool WritePrivateTitle(string value, bool append = false)
    {
      throw new NotImplementedException();
    }

    public virtual bool WritePublicText(string value, bool append = false)
    {
      if (append) {

        if ((_publicText.Length + value.Length) > _publicText.MaxCapacity)
          return false;

        _publicText.Append(value);
      }
      else
        _publicText.Clear()
          .Append(value);

      return true;
    }

    public virtual bool WritePublicText(StringBuilder value, bool append = false)
    {
      if (append) {

        if ((_publicText.Length + value.Length) > _publicText.MaxCapacity)
          return false;

        _publicText.AppendStringBuilder(value);
      }
      else
        _publicText.Clear()
          .AppendStringBuilder(value);

      return true;
    }

    public virtual bool WritePublicTitle(string value, bool append = false)
    {
      if (append) {

        if ((_publicTitle.Length + value.Length) > _publicTitle.MaxCapacity)
          return false;

        _publicTitle.Append(value);
      }
      else
        _publicTitle.Clear()
          .Append(value);

      return true;
    }
  }
}
