#if UNITY_EDITOR
using System;
using UnityEngine.UIElements;
using YNL.Editor.Utilities;

public class ESwitchToggle : Button
{
    public const string _styleSheet = "Assets/Plugins/Yunasawa の Library/YのL - Editor/Elements/E Switch Toggle/ESwitchToggle.uss";

    public bool Enable = false;

    public Image Background;
    public Image Toggle;

    public Action<bool> OnSwitch;

    public ESwitchToggle(bool enable = false) : base()
    {
        Enable = enable;

        this.AddStyle(_styleSheet, EAddress.USSFont).AddClass("Main");

        Toggle = new Image().AddClass("Toggle").EnableClass(Enable, "Toggle_Enable");
        Background = new Image().AddClass("Background").EnableClass(Enable, "Background_Enable").AddElements(Toggle);

        this.clicked += Switch;
        this.AddElements(Background);
    }

    public void Switch()
    {
        Enable = !Enable;

        Toggle.EnableClass(Enable, "Toggle_Enable");
        Background.EnableClass(Enable, "Background_Enable");

        OnSwitch?.Invoke(Enable);
    }

    public void SetEnable(bool enable)
    {
        Enable = enable;

        Toggle.EnableClass(Enable, "Toggle_Enable");
        Background.EnableClass(Enable, "Background_Enable");
    }
}
#endif