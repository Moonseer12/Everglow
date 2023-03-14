﻿namespace Everglow.Sources.Modules.MythModule.LanternMoon.Projectiles
{
	class LBloodEffectGra : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 90;
			Projectile.tileCollide = false;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
		}
		int TrueL = 1;
		public override ModProjectile Clone(Projectile projectile)
		{
			var clone = base.Clone(projectile) as LBloodEffectGra;
			//TrueL = 1;
			return clone;
		}
		public override void AI()
		{
			Projectile.velocity.Y += 0.25f;
			Projectile.velocity.X += Main.windSpeedCurrent * 0.02f / Projectile.scale;
			Projectile.velocity *= 0.96f;
			Projectile.scale -= 0.005f;
			if (Projectile.scale <= 0.005f)
			{
				Projectile.Kill();
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
		public override void PostDraw(Color lightColor)
		{
			List<Vertex2D> bars = new List<Vertex2D>();
			float width = 6 * Projectile.scale;
			if (Projectile.timeLeft < 60)
			{
				width = Projectile.timeLeft / 10f * Projectile.scale;
			}
			TrueL = 0;
			for (int i = 1; i < Projectile.oldPos.Length; ++i)
			{
				TrueL++;
				if (Projectile.oldPos[i] == Vector2.Zero)
					break;
			}
			for (int i = 1; i < Projectile.oldPos.Length; ++i)
			{
				if (Projectile.oldPos[i] == Vector2.Zero)
					break;
				var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
				normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

				var factor = i / (float)TrueL;
				var w = MathHelper.Lerp(1f, 0.05f, factor);

				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * width + new Vector2(10, 10) - Main.screenPosition, new Color(255, 0, 0, 0), new Vector3(factor, 1, w)));
				bars.Add(new Vertex2D(Projectile.oldPos[i] + normalDir * -width + new Vector2(10, 10) - Main.screenPosition, new Color(255, 0, 0, 0), new Vector3(factor, 0, w)));
			}
			List<Vertex2D> Vx = new List<Vertex2D>();
			if (bars.Count > 2)
			{
				Vx.Add(bars[0]);
				var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + Vector2.Normalize(Projectile.velocity) * 30, new Color(255, 0, 0, 0), new Vector3(0, 0.5f, 1));
				Vx.Add(bars[1]);
				Vx.Add(vertex);
				for (int i = 0; i < bars.Count - 2; i += 2)
				{
					Vx.Add(bars[i]);
					Vx.Add(bars[i + 2]);
					Vx.Add(bars[i + 1]);

					Vx.Add(bars[i + 1]);
					Vx.Add(bars[i + 2]);
					Vx.Add(bars[i + 3]);
				}

			}
			if (Vx.Count > 2)
			{
				Texture2D t = Common.MythContent.QuickTexture("LanternMoon/Projectiles/LBloodEffect");
				Main.graphics.GraphicsDevice.Textures[0] = t;
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
			}
			//Texture2D LightE = Common.MythContent.QuickTexture("VisualTextures/LightEffect");
			//Main.spriteBatch.Draw(LightE, Projectile.Center - Main.screenPosition, null, new Color(55, 46, 0, 0), -(float)(Math.Sin(Main.time / 26d)) + 0.6f, new Vector2(128f, 128f), 0.15f + (float)(0.075 * Math.Sin(Main.time / 26d)), SpriteEffects.None, 0);
			//Main.spriteBatch.Draw(LightE, Projectile.Center - Main.screenPosition, null, new Color(55, 46, 0, 0), (float)(Math.Sin(Main.time / 12d + 2)) + 1.6f, new Vector2(128f, 128f), 0.15f + (float)(0.075 * Math.Sin(Main.time / 26d)), SpriteEffects.None, 0);
			//Main.spriteBatch.Draw(LightE, Projectile.Center - Main.screenPosition, null, new Color(55, 46, 0, 0), (float)Math.PI / 2f + (float)(Main.time / 9d), new Vector2(128f, 128f), 0.15f + (float)(0.075 * Math.Sin(Main.time / 26d + 1.57)), SpriteEffects.None, 0);
			//Main.spriteBatch.Draw(LightE, Projectile.Center - Main.screenPosition, null, new Color(55, 46, 0, 0), (float)(Main.time / 26d), new Vector2(128f, 128f), 0.15f + (float)(0.075 * Math.Sin(Main.time / 26d + 3.14)), SpriteEffects.None, 0);
			//Main.spriteBatch.Draw(LightE, Projectile.Center - Main.screenPosition, null, new Color(55, 46, 0, 0), -(float)(Main.time / 26d), new Vector2(128f, 128f), 0.15f + (float)(0.075 * Math.Sin(Main.time / 26d + 4.71)), SpriteEffects.None, 0);
		}
	}
}
