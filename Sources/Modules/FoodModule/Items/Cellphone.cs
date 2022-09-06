﻿namespace Everglow.Sources.Modules.FoodModule.Items
{
    public class Cellphone : GlobalItem
    {
        public override void UpdateInventory(Item item, Player player)
        {
            if (item.type == ItemID.CellPhone)
            {
                FoodSatietyInfoDisplayplayer SatietyInfo = player.GetModPlayer<FoodSatietyInfoDisplayplayer>();
                SatietyInfo.AccBloodGlucoseMonitor = true;//显示当前饱食度

                ThirstystateInfoDisplayplayer ThirstystateInfo = player.GetModPlayer<ThirstystateInfoDisplayplayer>();
                ThirstystateInfo.AccOsmoticPressureMonitor = true;//显示渴觉状态
            }
        }
    }
    public class CellphoneRecipe : ModSystem
    {
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];
                if (recipe.TryGetIngredient(ItemID.PDA, out Item ingredient))
                {
                    recipe.AddIngredient(ModContent.ItemType<OsmoticPressureMonitor>());
                    recipe.AddIngredient(ModContent.ItemType<BloodGlucoseMonitor>());
                }
            }
        }
    }
}
