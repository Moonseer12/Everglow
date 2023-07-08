using Everglow.Commons.Vertex;
using Everglow.Commons.MEAC;
using Terraria.GameContent;
using Everglow.Commons.Utilities;

namespace Everglow.IIID.Projectiles.NonIIIDProj.PlanetBefallArray
{
	public class PlanetBefallArray : ModProjectile //,IBloomProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 28;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 300;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = false;
		}
		internal float Timer = 0;
		internal float alpha = 1;
		public float BloomIntensity = 1;
		public bool BloomIntensityChange = false;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.velocity *= 0;
			float f = 0.025f;
			if (Timer < 40)
			{
				Timer = f.Lerp(Timer, 20);
			}
			if (Projectile.timeLeft < 60)
			{
				alpha *= 0.95f;
			}
			
			if (BloomIntensityChange)
			{
				Projectile.ai[0]++;
				BloomIntensity = MathF.Sin((float)(Projectile.ai[0] / (18 * Math.PI)))+1;
			}
		}
		public override void Kill(int timeLeft)
		{
			BloomIntensity = 0;
			base.Kill(timeLeft);
		}
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			overPlayers.Add(index);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.hide = false;
			
			DrawMagicArray();
			
			return false;
		}
		public void DrawBloom()
		{
			Color c = Color.White;
			PreDraw(ref c);
		}

		public void DrawMagicArray( )
		{
			Player player = Main.player[Projectile.owner];
			Texture2D PlantBeFallIn = ModContent.Request<Texture2D>("Everglow/IIID/Projectiles/NonIIIDProj/PlanetBefallArray/PlantBeFallIn").Value;
			Texture2D PlantBeFallOut = ModContent.Request<Texture2D>("Everglow/IIID/Projectiles/NonIIIDProj/PlanetBefallArray/PlantBeFallOut").Value;
			Texture2D GeoElement = ModContent.Request<Texture2D>("Everglow/IIID/Projectiles/NonIIIDProj/PlanetBefallArray/GeoElement").Value;
			Vector2 p = Projectile.Center - Main.screenPosition - new Vector2(GeoElement.Width, GeoElement.Height) / 8;
			Main.spriteBatch.Draw(GeoElement, new Rectangle((int)p.X, (int)p.Y, GeoElement.Width / 4, GeoElement.Height / 4), Color.White * alpha);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			DrawTexCircle(Timer * 30f, 100, Color.Gold * alpha, Projectile.Center - Main.screenPosition, PlantBeFallOut, Main.timeForVisualEffects / 1500 + MathHelper.PiOver4);
			List<Vertex2D> In = new List<Vertex2D>();

			Vector2 Point1 = Projectile.Center - Main.screenPosition + new Vector2(Timer * 25, Timer * 25).RotatedBy(Math.PI * 0 - Main.timeForVisualEffects / 500);
			Vector2 Point2 = Projectile.Center - Main.screenPosition + new Vector2(Timer * 25, Timer * 25).RotatedBy(Math.PI * 1 / 2d - Main.timeForVisualEffects / 500);
			Vector2 Point3 = Projectile.Center - Main.screenPosition + new Vector2(Timer * 25, Timer * 25).RotatedBy(Math.PI * 2 / 2d - Main.timeForVisualEffects / 500);
			Vector2 Point4 = Projectile.Center - Main.screenPosition + new Vector2(Timer * 25, Timer * 25).RotatedBy(Math.PI * 3 / 2d - Main.timeForVisualEffects / 500);
			In.Add(new Vertex2D(Point1, Color.Gold * alpha, new Vector3(0, 0, 0)));
			In.Add(new Vertex2D(Point2, Color.Gold * alpha, new Vector3(1, 0, 0)));
			In.Add(new Vertex2D(Point4, Color.Gold * alpha, new Vector3(0, 1, 0)));
			In.Add(new Vertex2D(Point3, Color.Gold * alpha, new Vector3(1, 1, 0)));
			if (In.Count > 0)
			{
				Main.graphics.GraphicsDevice.Textures[0] = PlantBeFallIn;
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, In.ToArray(), 0, 2);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}

		private static void DrawTexCircle(float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
		{
			List<Vertex2D> circle = new List<Vertex2D>();
			for (int h = 0; h < radious / 2; h++)
			{
				circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
				circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
			}
			circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(1, 1, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(1, 0, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(0, 1, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0, 0, 0)));
			if (circle.Count > 0)
			{
				Main.graphics.GraphicsDevice.Textures[0] = tex;
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 4);
			}
		}


	}
}
