using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editor.Utilities;

public class ELine : Image
{
    private const string USS_StyleSheet = EAddress.FolderPath + "Elements/E Line/ELine.uss";

    public ELine(ELineMode mode) : base()
    {
        this.AddStyle(USS_StyleSheet);
        if (mode == ELineMode.Horizontal) this.SetName("Horizontal");
        if (mode == ELineMode.Vertical) this.SetName("Vertical");
    }
}

public enum ELineMode
{
    Vertical, Horizontal
}