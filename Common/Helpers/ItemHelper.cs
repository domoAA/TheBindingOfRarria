using Terraria;

namespace TheBindingOfRarria.Common.Helpers;

public static partial class Helper
{
    public static bool HasTag(this Item item, int[] tag)
    {
        foreach (int member in tag)
            if (member == item.type)
                return true;

        return false;
    }
}
