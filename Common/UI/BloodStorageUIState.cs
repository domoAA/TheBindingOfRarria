using Terraria;
using Terraria.UI;

namespace TheBindingOfRarria.Common.UI;

public class BloodStorageUIState : UIState
{
    public override void OnInitialize()
    {
        DraggableUIPanel panel = new();
        panel.SetPadding(0);

            // I'm not even gonna fucking try to understand the crack you were smoking.
        panel.Left.Set(Main.screenWidth * 0.78125f, 0f);
        panel.Top.Set(Main.screenHeight * 0.13888f, 0f);

        panel.Width.Set(50f, 0f);
        panel.Height.Set(50f, 0f);

        panel.BackgroundColor = Color.Transparent;
        panel.BorderColor = Color.Transparent;
        Append(panel);

        BloodStorageUIElement element = new();
        panel.Append(element);
    }
}
