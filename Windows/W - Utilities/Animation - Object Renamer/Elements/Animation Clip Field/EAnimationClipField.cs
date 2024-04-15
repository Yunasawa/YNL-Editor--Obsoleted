#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editor.UIElement;
using YNL.Editor.Utility;

namespace YNL.Editor.Window.Animation.ObjectRenamer
{
    public class EAnimationClipField : Button
    {
        private const string USS_StyleSheet = "Style Sheets/Windows/W - Utilities/Animation - Object Renamer/EAnimationClipField";

        private const string _uss_field = "e-animation-clip-field__field";
        private const string _uss_color = "e-animation-clip-field__color";
        private const string _uss_clip = "e-animation-clip-field__clip";

        public Image Color;
        public EAssetField<AnimationClip> Clip;

        public EAnimationClipField(KeyValuePair<AnimationClip, Color> pair) : base()
        {
            this.AddStyle(USS_StyleSheet);

            Color = new Image().AddClass(_uss_color).SetBackgroundColor(pair.Value);
            Clip = new EAssetField<AnimationClip>(pair.Key).AddClass(_uss_clip);

            this.AddClass(_uss_field).AddElements(Clip, Color);
        }
    }
}
#endif