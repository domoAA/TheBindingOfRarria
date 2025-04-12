using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace TheBindingOfRarria.Common.UI;

[Autoload(Side = ModSide.Client)]
public class BloodStorageUISystem : ModSystem
{
    internal BloodStorageUIState state;
    private UserInterface Interface;

    public void Show() => Interface?.SetState(state);

    public void Hide() => Interface?.SetState(null);

    public override void Load()
    {
        state = new BloodStorageUIState();
        Interface = new UserInterface();
        state.Activate();
    }

    public override void UpdateUI(GameTime gameTime) => Interface?.Update(gameTime);

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
            // You might wanna use another layer.
        int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Entity Health Bars"));
        if (index == -1)
            return;

        layers.Insert(index, new LegacyGameInterfaceLayer("TheBindingOfRarria: Blood Storage", delegate
        {
            Interface.Draw(Main.spriteBatch, new GameTime());

            return true;

        }, InterfaceScaleType.UI));
    }
}
