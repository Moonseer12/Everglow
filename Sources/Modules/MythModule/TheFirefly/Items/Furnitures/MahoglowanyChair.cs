using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Furnitures
{
	public class MahoglowanyChair : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furnitures.MahoglowanyChair>());
			Item.value = 150;
			Item.maxStack = 99;
			Item.width = 12;
			Item.height = 30;
		}

	}
}
