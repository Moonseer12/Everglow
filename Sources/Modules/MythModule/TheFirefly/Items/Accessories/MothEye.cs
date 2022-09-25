﻿using Everglow.Sources.Modules.MythModule.Common;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class MothEye : ModItem
    {
        FireflyBiome fireflyBiome = ModContent.GetInstance<FireflyBiome>();
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ommateum of the Moth");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "魔蛾之眼");
            //Tooltip.SetDefault("Increases minion slots by 1\nIncreases summon damage by 6%\n'Sophisticated structures of the ommatuem have amazed you'");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "召唤栏位增加1\n召唤伤害增加6%\n'复眼精妙的结构使你着魔'");
        }
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 46;
            Item.value = 2000;
            Item.accessory = true;
            Item.rare = 2;
            //Item.vanity = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 1;
            player.GetDamage(DamageClass.Summon) *= 1.06f;

            if (fireflyBiome.IsBiomeActive(Main.LocalPlayer))
            {
                player.maxTurrets += 1;
                player.wingTime *= 1.10f;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            
            if (fireflyBiome.IsBiomeActive(Main.LocalPlayer))
            {
                tooltips.Add(new TooltipLine(ModLoader.GetMod("Everglow"), "MothEyeText1", "[c/2888FE:While in the Firefly biome:\n- Increases sentry slots by 1\n- Increases flight time by 10%\n- All Firefly weapons deal 5% more damage\n- Some Firefly-related items gain bonuses]"));
                tooltips.Add(new TooltipLine(ModLoader.GetMod("Everglow"), "MothEyeTextUnfinished", "[c/BA0022:This item is unfinished]"));
            }
            base.ModifyTooltips(tooltips);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
    /*        Vector2 slotSize = new Vector2(1f, 1f);
            position -= slotSize * Main.inventoryScale / 1f - frame.Size() * scale / 1f;
            Vector2 drawPos = position + slotSize * Main.inventoryScale / 1f/* - texture.Size() * Main.inventoryScale / 2f*/;
    /*        Texture2D RArr = MythContent.QuickTexture("TheFirefly/Projectiles/FixCoin3AltLight");
            spriteBatch.Draw(RArr, drawPos, null, new Color(255, 255, 255, 50), 0f, new Vector2(8), scale * 1f, SpriteEffects.None, 0f);
            //ModContent.Request<Texture2D>("MythMod/UIImages/RightDFan").Value;
     */       return true;
        } // This currently has no effect. I want it to work so there is this glow in the back of the item in the inventory while in the firefly biome ~Setnour6
    }
}
