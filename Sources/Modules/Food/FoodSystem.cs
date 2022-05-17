﻿using Everglow.Sources.Modules.Food.Buffs;

namespace Everglow.Sources.Modules.Food
{
    internal class FoodSystem : ModSystem
    {
        // 对于原版的食物进行类型Id到 FoodInfo 的映射，直接获取FoodInfo实例
        private Dictionary<Item_id, FoodInfo> m_vanillaFoodInfos;

        public Dictionary<Item_id, FoodInfo> VanillaFoodInfos
        {
            get { return m_vanillaFoodInfos; }
        }

        public override void PostSetupContent()
        {
            m_vanillaFoodInfos = new Dictionary<Item_id, FoodInfo>
            {
                { ItemID.Ale, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<AleBuff>() } },
                { ItemID.Apple, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<AppleBuff>() } },
                { ItemID.AppleJuice, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<AppleJuiceBuff>() } },
                { ItemID.ApplePie, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<ApplePieBuff>() } },
                { ItemID.Apricot, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<ApricotBuff>() } },
                { ItemID.Bacon, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<MilkCartonBuff>() } },
                { ItemID.Banana, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<BaconBuff>() } },
                { ItemID.BananaDaiquiri, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<BananaDaiquiriBuff>() } },
                { ItemID.BananaSplit, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<BananaSplitBuff>() } },
                { ItemID.BBQRibs, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<BBQRibsBuff>() } },
                { ItemID.BlackCurrant, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<BlackCurrantBuff>() } },
                { ItemID.BloodOrange, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<BloodOrangeBuff>() } },
                { ItemID.BloodyMoscato, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<BloodOrangeBuff>() } },
                { ItemID.BowlofSoup, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<BloodyMoscatoBuff>() } },
                { ItemID.BunnyStew, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<BunnyStewBuff>() } },
                { ItemID.Burger, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<BurgerBuff>() } },
                { ItemID.Cherry, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<CherryBuff>() } },
                { ItemID.ChickenNugget, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<ChickenNuggetBuff>() } },
                { ItemID.ChocolateChipCookie, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<ChocolateChipCookieBuff>() } },
                { ItemID.ChristmasPudding, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<ChristmasPuddingBuff>() } },
                { ItemID.Coconut, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<CoconutBuff>() } },
                { ItemID.CoffeeCup, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<CoffeeCupBuff>() } },
                { ItemID.CookedFish, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<CookedFishBuff>() } },
                { ItemID.CookedMarshmallow, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<CookedMarshmallowBuff>() } },
                { ItemID.CookedShrimp, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<CookedShrimpBuff>() } },
                { ItemID.CreamSoda, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<CreamSodaBuff>() } },
                { ItemID.Dragonfruit, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<DragonfruitBuff>() } },
                { ItemID.Elderberry, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<ElderberryBuff>() } },
                { ItemID.Escargot, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<EscargotBuff>() } },
                { ItemID.FriedEgg, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<FriedEggBuff>() } },
                { ItemID.Fries, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<FriesBuff>() } },
                { ItemID.FroggleBunwich, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<FroggleBunwichBuff>() } },
                { ItemID.FruitJuice, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<FruitJuiceBuff>() } },
                { ItemID.FruitSalad, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<FruitSaladBuff>() } },
                { ItemID.GingerbreadCookie, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<GingerbreadCookieBuff>() } },
                { ItemID.GoldenDelight, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<GoldenDelightBuff>() } },
                { ItemID.Grapefruit, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<GrapefruitBuff>() } },
                { ItemID.GrapeJuice, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<GrapeJuiceBuff>() } },
                { ItemID.Grapes, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<GrapesBuff>() } },
                { ItemID.GrilledSquirrel, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<GrilledSquirrelBuff>() } },
                { ItemID.GrubSoup, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<GrubSoupBuff>() } },
                { ItemID.Hotdog, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<HotdogBuff>() } },
                { ItemID.IceCream, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<IceCreamBuff>() } },
                { ItemID.Lemonade, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<LemonadeBuff>() } },
                { ItemID.Lemon, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<LemonBuff>() } },
                { ItemID.LobsterTail, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<LobsterTailBuff>() } },
                { ItemID.Mango, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<MangoBuff>() } },
                { ItemID.Marshmallow, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<MarshmallowBuff>() } },
                { ItemID.MilkCarton, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<MilkCartonBuff>() } },
                { ItemID.Milkshake, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<MilkshakeBuff>() } },
                { ItemID.MonsterLasagna, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<MonsterLasagnaBuff>() } },
                { ItemID.Nachos, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<NachosBuff>() } },
                { ItemID.PadThai, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PadThaiBuff>() } },
                { ItemID.Peach, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PeachBuff>() } },
                { ItemID.PeachSangria, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PeachSangriaBuff>() } },
                { ItemID.Pho, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PhoBuff>() } },
                { ItemID.PinaColada, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PinaColadaBuff>() } },
                { ItemID.Pineapple, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PineappleBuff>() } },
                { ItemID.Pizza, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PizzaBuff>() } },
                { ItemID.Plum, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PlumBuff>() } },
                { ItemID.PotatoChips, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PotatoChipsBuff>() } },
                { ItemID.PrismaticPunch, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PrismaticPunchBuff>() } },
                { ItemID.PumpkinPie, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<PumpkinPieBuff>() } },
                { ItemID.Rambutan, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<RambutanBuff>() } },
                { ItemID.RoastedBird, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<RoastedBirdBuff>() } },
                { ItemID.RoastedDuck, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<RoastedDuckBuff>() } },
                { ItemID.Sake, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SakeBuff>() } },
                { ItemID.Sashimi, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SashimiBuff>() } },
                { ItemID.SauteedFrogLegs, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SauteedFrogLegsBuff>() } },
                { ItemID.SeafoodDinner, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SeafoodDinnerBuff>() } },
                { ItemID.ShrimpPoBoy, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<ShrimpPoBoyBuff>() } },
                { ItemID.ShuckedOyster, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<ShuckedOysterBuff>() } },
                { ItemID.SmoothieofDarkness, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SmoothieofDarknessBuff>() } },
                { ItemID.Spaghetti, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SpaghettiBuff>() } },
                { ItemID.Starfruit, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<StarfruitBuff>() } },
                { ItemID.Steak, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SteakBuff>() } },
                { ItemID.SugarCookie, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<SugarCookieBuff>() } },
                { ItemID.Teacup, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<TeacupBuff>() } },
                { ItemID.TropicalSmoothie, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<TropicalSmoothieBuff>() } }
            };
            base.PostSetupContent();
        }
    }
}
