using Everglow.Commons.CustomTiles;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.NPCs;

public class Bulbling : ModNPC
{
	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 5;
		NPCSpawnManager.RegisterNPC(Type);
	}
	public override void SetDefaults()
	{
		NPC.width = 40;
		NPC.height = 40;
		NPC.aiStyle = -1;
		NPC.damage = 13;
		NPC.defense = 2;
		NPC.lifeMax = 24;
		NPC.HitSound = SoundID.NPCHit3;
		NPC.DeathSound = SoundID.NPCDeath3;
		NPC.noGravity = true;
		NPC.noTileCollide = false;
		NPC.knockBackResist = 1f;
		NPC.value = 90;

	}
	public override void OnSpawn(IEntitySource source)
	{
		Anger = false;
		BloomLightColor = new Vector3(0f, 0.5f, 1f);
		State = (int)NPCState.Sleep;
		NPC.scale = Main.rand.NextFloat(0.75f, 1.18f);
	}
	int State;
	public bool Anger = false;
	public Vector3 BloomLightColor;
	private enum NPCState
	{
		Sleep,
		Dash,
		Rest,
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
	}
	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	{
		YggdrasilTownBiome YggdrasilTownBiome = ModContent.GetInstance<YggdrasilTownBiome>();
		if (!YggdrasilTownBiome.IsBiomeActive(Main.LocalPlayer))
			return 0f;
		return 3f;
	}

	public override void FindFrame(int frameHeight)
	{
		NPC.frame.Width = 48;
		NPC.frame.Height = 58;
		if (Anger)
		{
			NPC.frame.X = 0;
		}
		else
		{
			NPC.frame.X = 48;
		}
		switch (State)
		{
			case (int)NPCState.Dash:
				{
					NPC.frame.Y = (int)(NPC.frameCounter / 5 % 5) * 58;
					break;
				}
			case (int)NPCState.Sleep:
				{
					NPC.frame.Y = (int)(NPC.frameCounter / 5 % 5) * 58;
					break;
				}
			case (int)NPCState.Rest:
				{
					NPC.frame.Y = (int)(NPC.frameCounter / 5 % 5) * 58;
					break;
				}
		}
	}
	public override void AI()
	{
		switch (State)
		{
			case (int)NPCState.Sleep:
				{
					NPC.frameCounter++;
					NPC.TargetClosest();
					Vector2 targetVel = new Vector2(MathF.Sin((float)Main.time * 0.03f + NPC.whoAmI), MathF.Sin((float)Main.time * 0.06f + MathF.PI + NPC.whoAmI)) * NPC.scale;
					NPC.velocity = Vector2.Lerp(NPC.velocity, targetVel, 0.4f);
					NPC.rotation = Math.Clamp(NPC.velocity.X * 0.3f, -1f, 1f);
					if (NPC.HasValidTarget && Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1)
							 && Anger)
					{
						State = (int)NPCState.Dash;
						NPC.ai[0] = 0;
						NPC.frameCounter = 0;
					}
					break;
				}
			case (int)NPCState.Dash:
				{
					NPC.TargetClosest();
					Player target = Main.player[NPC.target];
					Vector2 toAim = target.Center - NPC.Center;
					NPC.velocity = Vector2.Normalize(toAim) * 3 * NPC.scale;
					NPC.rotation = NPC.velocity.ToRotation() + 1.57f;

					NPC.frameCounter++;
					if (NPC.frameCounter >= 5)
					{
						State = (int)NPCState.Rest;
						NPC.frameCounter = 0;
					}

					break;
				} 
			case (int)NPCState.Rest:
				{
					NPC.velocity *= 0.92f;
					NPC.frameCounter++;
					if (NPC.velocity.Length() <= 0.5f)
					{
						NPC.velocity *= 0;
						State = (int)NPCState.Dash;
						NPC.frameCounter = 0;
						NPC.TargetClosest();
						if (NPC.HasValidTarget && Collision.CanHit(NPC.Center, 1, 1, Main.player[NPC.target].Center, 1, 1)
										   && Main.player[NPC.target].Distance(NPC.Center) > 1000)
						{
							Anger = false;
							State = (int)NPCState.Sleep;
							NPC.frameCounter = 0;
						}
					}
					NPC.rotation = NPC.velocity.ToRotation() + 1.57f;
					break;
				}
		}
		float glowStrength = 0.4f;
		if (State == (int)NPCState.Sleep)
		{
			glowStrength = 0.1f;
		}
		if(Anger)
		{
			if (NPC.frame.Y == 58)
			{
				BloomLightColor = Vector3.Lerp(BloomLightColor, new Vector3(0.5f, 2f, 4f) * glowStrength, 0.3f);

			}
			else if (NPC.frame.Y == 116)
			{
				BloomLightColor = Vector3.Lerp(BloomLightColor, new Vector3(0f, 1f, 2f) * glowStrength, 0.3f);
			}
			else
			{
				BloomLightColor = Vector3.Lerp(BloomLightColor, new Vector3(0f, 0.3f, 0.6f) * glowStrength, 0.3f);
			}
		}
		else
		{
			BloomLightColor = Vector3.Lerp(BloomLightColor, new Vector3(0f, 0.5f, 1f) * glowStrength, 0.3f);
		}
		Lighting.AddLight(NPC.Center, BloomLightColor);
	}
	public override void HitEffect(NPC.HitInfo hit)
	{
		if(!Anger)
		{
			Anger = true;
		}
		if(NPC.life <= 0)
		{
			for (int i = 0; i < 20; i++)
			{
				Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<BulblingGel>());
				d.scale *= Main.rand.NextFloat(1f, 2.4f);
				d.velocity = new Vector2(Main.rand.NextFloat(2, 8f), 0).RotatedByRandom(MathHelper.TwoPi);
			}
			for (int i = 0; i < 4; i++)
			{
				Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<BulblingSpark>());
				d.velocity = new Vector2(Main.rand.NextFloat(2, 6f), 0).RotatedByRandom(MathHelper.TwoPi);
			}
		}
		else
		{
			for (int i = 0; i < 4; i++)
			{
				Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<BulblingGel>());
				d.scale *= Main.rand.NextFloat(1f, 1.7f);
				d.velocity = new Vector2(Main.rand.NextFloat(2, 4f), 0).RotatedByRandom(MathHelper.TwoPi);
			}
			for (int i = 0; i < 1; i++)
			{
				Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<BulblingSpark>());
				d.velocity = new Vector2(Main.rand.NextFloat(2, 4f), 0).RotatedByRandom(MathHelper.TwoPi);
			}
		}
		base.HitEffect(hit);
	}
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		float glowStrength = 1f;
		if (State == (int)NPCState.Sleep)
		{
			glowStrength = 0.4f;
		}
		Texture2D texture = ModAsset.Bulbling.Value;
		Texture2D textureG = ModAsset.Bulbling_glow.Value;
		Texture2D textureB = ModAsset.Bulbling_bloom.Value;
		spriteBatch.Draw(textureB, NPC.Center - Main.screenPosition, new Rectangle(NPC.frame.X / 48 * 160, NPC.frame.Y / 58 * 160, 160, 160), new Color(1f, 1f, 1f, 0f) * glowStrength, NPC.rotation, new Vector2(80), NPC.scale, SpriteEffects.None, 0);
		spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, NPC.frame, Color.Lerp(drawColor * 0.7f, new Color(0.6f, 1f, 1f, 1f), 0.4f), NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, SpriteEffects.None, 0);

		spriteBatch.Draw(textureG, NPC.Center - Main.screenPosition, NPC.frame, new Color(1f, 1f, 1f, 0f) * glowStrength, NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, SpriteEffects.None, 0);


		return false;
	}
	public override void OnKill()
	{

	}
	public override void ModifyNPCLoot(NPCLoot npcLoot)
	{
		//TODO 掉落物
	}
}