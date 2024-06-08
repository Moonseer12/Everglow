using Everglow.Commons.VFX.CommonVFXDusts;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Myth.TheTusk.NPCs.BloodTusk;
[NoGameModeScale]
public class Living_Jawbone : ModNPC
{
	public bool OpenMouth = false;

	public override void SetDefaults()
	{
		NPC.width = 30;
		NPC.height = 30;
		NPC.friendly = false;
		NPC.noTileCollide = false;
		NPC.lifeMax = 120;
		NPC.behindTiles = true;
	}

	public override void OnSpawn(IEntitySource source)
	{
		for (int g = 0; g < 4; g++)
		{
			var blood = new BloodDrop
			{
				velocity = NPC.velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * Main.rand.NextFloat(0.4f, 1.1f),
				Active = true,
				Visible = true,
				position = NPC.Center,
				maxTime = Main.rand.Next(54, 74),
				scale = Main.rand.NextFloat(6f, 25f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) },
			};
			Ins.VFXManager.Add(blood);
		}
		for (int g = 0; g < 2; g++)
		{
			var blood = new BloodSplash
			{
				velocity = NPC.velocity.RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * Main.rand.NextFloat(0.4f, 1.1f),
				Active = true,
				Visible = true,
				position = NPC.Center,
				maxTime = Main.rand.Next(54, 74),
				scale = Main.rand.NextFloat(6f, 18f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0, Main.rand.NextFloat(20.0f, 40.0f) },
			};
			Ins.VFXManager.Add(blood);
		}
	}

	public override void AI()
	{
		NPC.rotation = NPC.velocity.ToRotation();
		NPC.velocity.Y += 0.2f;
		NPC.velocity *= 0.99f;
		NPC.TargetClosest(false);
		Player player = Main.player[NPC.target];
		NPC.velocity.X += 0.1f * Math.Sign(player.Center.X - NPC.Center.X);
		if (!OpenMouth)
		{
			NPC.ai[0] = 0;
			if (Main.rand.NextBool(24))
			{
				OpenMouth = true;
			}
		}
		else
		{
			NPC.ai[0] = (float)Utils.Lerp(NPC.ai[0], 2, 0.06f);
			if (NPC.ai[0] > 1.8f)
			{
				SoundEngine.PlaySound(SoundID.DD2_SkeletonDeath.WithVolume(0.4f * NPC.scale), NPC.Center);
				NPC.ai[0] = 0;
				OpenMouth = false;
			}
		}
		if (Collision.SolidCollision(NPC.Center + new Vector2(NPC.velocity.X, 0), 0, 0))
		{
			NPC.velocity.X *= -0.6f;
		}
		if (NPC.collideY)
		{
			if(Main.rand.NextBool(15))
			{
				NPC.velocity.Y -= Main.rand.NextFloat(4f, 8f);
			}
		}
	}

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		Texture2D upJaw = ModAsset.Living_Jawbone_up.Value;
		Texture2D downJaw = ModAsset.Living_Jawbone_down.Value;
		SpriteEffects spriteEffects = SpriteEffects.None;
		if (NPC.spriteDirection == -1)
		{
			spriteEffects = SpriteEffects.FlipVertically;
		}
		spriteBatch.Draw(upJaw, NPC.Center - Main.screenPosition, null, Lighting.GetColor(NPC.Center.ToTileCoordinates()), NPC.rotation + NPC.ai[0], new Vector2(8f, 18f), NPC.scale, spriteEffects, 0);
		spriteBatch.Draw(downJaw, NPC.Center - Main.screenPosition, null, Lighting.GetColor(NPC.Center.ToTileCoordinates()), NPC.rotation - NPC.ai[0], new Vector2(8f, 18f), NPC.scale, spriteEffects, 0);
		return false;
	}
}