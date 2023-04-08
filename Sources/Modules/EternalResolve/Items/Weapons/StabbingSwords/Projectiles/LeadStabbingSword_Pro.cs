namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class LeadStabbingSword_Pro : StabbingProjectile
    {
        public override void SetDefaults()
        {
            Color = new Color(85, 94, 123);
			base.SetDefaults();
			TradeLength = 4;
			TradeShade = 0.4f;
			Shade = 0.2f;
			FadeTradeShade = 0.44f;
			FadeScale = 1;
			TradeLightColorValue = 0.6f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.72f;
        }
    }
}