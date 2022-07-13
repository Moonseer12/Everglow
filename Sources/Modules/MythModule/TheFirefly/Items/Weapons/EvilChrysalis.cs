﻿using Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles;
using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Weapons
{
    public class EvilChrysalis : ModItem
    {
        public override void SetStaticDefaults()
        {
            GetGlowMask = MythContent.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            Item.damage = 12;
            Item.mana = 6;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 2.25f;
            Item.value = 2100;
            Item.rare = 2;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<Projectiles.EvilChrysalis>();
            Item.autoReuse = true;
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
            Item.noUseGraphic = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, Main.rand.NextFloat(0, 200f), Main.rand.NextFloat(0, 200f));
            return false;
        }
    }
}
