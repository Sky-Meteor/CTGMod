using CTGMod.ID;
using Terraria;
using Terraria.ModLoader;

namespace CTGMod.Common.Systems;

public class RecipeSystem : ModSystem
{
    public override void PostAddRecipes()
    {
        int count = 0;
        int modRecipeCount = 0;
        foreach (Recipe recipe in Main.recipe)
        {
            if (GemID.Gems.Contains(recipe.createItem.type) || recipe.requiredItem.Exists(item => GemID.Gems.Contains(item.type)))
            {
                count++;
                recipe.DisableRecipe();
                recipe.DisableDecraft();
                if (recipe.Mod != null)
                {
                    modRecipeCount++;
                }
            }
        }
        Mod.Logger.Info($"CTG RecipeSystem: Removed {count} recipes, including {modRecipeCount} mod recipe(s).");
    }
}