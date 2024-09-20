#if UNITY_EDITOR
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editors.Extensions;
using YNL.Editors.UIElements.Styled;

namespace YNL.Editors.Windows.TextureImageResizer
{
    public class EImageDisplayer : Image
    {
        private const string _styleSheet = "Style Sheets/Windows/General Toolbox/Texture - Image Resizer/EImageDisplayer";

        public StyledInteractableAssetsField<Object> Background;
        public Image Grid;
        public ScrollView Scroll;

        public EImageDisplayer() : base()
        {

            this.AddStyle(_styleSheet).AddClass("Main");

            Grid = new Image().AddClass("Grid").SetName("Grid");
            Scroll = new ScrollView().AddClass("Scroll").AddElements(Grid);
            Background = new StyledInteractableAssetsField<Object>().AddClass("Background").AddElements(Scroll).SetName("Background");
            Background.OnDragPerform += TryGetAsset;

            this.AddElements(Background);
        }

        public void GenerateImages(Texture2D[] textures, float width, Vector2 newSize, bool keepAspectRatio)
        {
            Grid.RemoveAllElements();

            foreach (var texture in textures)
            {
                //MDebug.Custom("Image", texture.IsNull() ? "is null" : texture.name);
                EImageBox imageBox = new EImageBox(texture, newSize).SetSize(width, width * 1.5f);
                Grid.AddElements(imageBox);
                if (keepAspectRatio) imageBox.SetNewAspectedSize((int)newSize.x, true);
            }
        }
        public void ClearItems() => Grid.RemoveAllElements();
        private void TryGetAsset(Object[] assets)
        {
            Main.OnAddImage?.Invoke(assets);
        }

        public void SetAllNewSize(int value, bool isWidth)
        {
            if (isWidth) foreach (EImageBox box in Grid.Children().ToArray()) box.SetNewSize(new(value, box.NewAssignedSize.y));
            else foreach (EImageBox box in Grid.Children().ToArray()) box.SetNewSize(new(box.NewAssignedSize.x, value));
        }
        public void SetAllNewAspectedSize(int value, bool isWidth)
        {
            foreach (EImageBox box in Grid.Children().ToArray()) box.SetNewAspectedSize(value, isWidth);
        }
    }
}
#endif