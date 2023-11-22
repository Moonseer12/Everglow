namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class CyanVineThrowingSpear : ModItem
{
	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Swing;
		Item.width = 54;
		Item.height = 108;
		Item.useAnimation = 16;
		Item.useTime = 16;
		Item.knockBack = 4f;
		Item.damage = 13;
		Item.rare = ItemRarityID.White;
		Item.UseSound = SoundID.Item1;
		Item.value = 3600;
		Item.autoReuse = false;
		Item.DamageType = DamageClass.Melee;
		Item.channel = true;

		Item.noMelee = true;
		Item.noUseGraphic = true;


		Item.shoot = ModContent.ProjectileType<Projectiles.CyanVineThrowingSpear>();
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ModContent.ItemType<CyanVineBar>(), 14)
			.AddIngredient(ModContent.ItemType<StoneDragonScaleWood>(), 26)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
