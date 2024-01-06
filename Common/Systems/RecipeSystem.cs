using System.Linq;
using CTGMod.ID;
using Terraria;
using Terraria.ModLoader;

namespace CTGMod.Common.Systems;

public class RecipeSystem : ModSystem
{
    public override void PostAddRecipes()
    {
        foreach (Recipe recipe in Main.recipe.Where(recipe => GemID.Gems.Contains(recipe.createItem.type)))
        {
            recipe.DisableRecipe();
            recipe.DisableDecraft();
        }
    }
}