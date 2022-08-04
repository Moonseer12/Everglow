﻿using Terraria.Localization;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
namespace Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.NPCs
{
    public class EvilPack : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Evil Cocoon");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "魔茧");
            Main.npcFrameCount[NPC.type] = 2;
        }
        public override void SetDefaults()
        {
            NPC.damage = 0;
            NPC.width = 80;
            NPC.height = 150;
            NPC.defense = 0;
            NPC.lifeMax = 1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.color = new Color(0, 0, 0, 0);
            NPC.alpha = 0;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.behindTiles = true;
            NPC.HitSound = SoundID.NPCHit18; //Or use NPCHit11. Whichever one sounds more realistic to the cocoon. ~Setnour6
            NPC.DeathSound = SoundID.NPCDeath11;
            NPC.aiStyle = -1;
            NPC.boss = false;
        }
        private float omega = 0;
        public override void AI()
        {
            NPC.rotation += omega;
            omega -= NPC.rotation * 0.03f;
            omega *= 0.97f;
            if(NPC.ai[0] < 10)
            {
                NPC.frame = new Rectangle(0, 150, 80, 150);
            }
            else
            {
                if (NPC.ai[1] > 90)
                {
                    NPC.frame = new Rectangle(0, 0, 80, 150);
                    if(NPC.ai[2] == 0)
                    {
                        NPC.NewNPC(NPC.GetSource_FromAI(),(int)NPC.position.X + 26, (int)NPC.position.Y + 106,ModContent.NPCType<CorruptMoth>());
                        NPC.ai[2] += 1;
                    }
                }
                else
                {
                    omega *= 0.9f;
                    float step = 0.05f;
                    NPC.ai[1] += step;
                    if(NPC.ai[1] >= 20f && NPC.ai[1] - step < 20f)
                    {
                        omega += 0.02f;
                    }
                    if (NPC.ai[1] >= 40f && NPC.ai[1] - step < 40f)
                    {
                        omega -= 0.03f;
                    }
                    if (NPC.ai[1] >= 60f && NPC.ai[1] - step < 60f)
                    {
                        omega += 0.04f;
                    }
                    if (NPC.ai[1] >= 70f && NPC.ai[1] - step < 70f)
                    {
                        omega -= 0.05f;
                    }
                    if (NPC.ai[1] >= 76f && NPC.ai[1] - step < 76f)
                    {
                        omega += 0.02f;
                    }
                    if (NPC.ai[1] >= 80f && NPC.ai[1] - step < 80f)
                    {
                        omega += 0.05f;
                    }
                    if (NPC.ai[1] >= 82f && NPC.ai[1] - step < 82f)
                    {
                        omega -= 0.06f;
                    }
                    if (NPC.ai[1] >= 86f && NPC.ai[1] - step < 86f)
                    {
                        omega -= 0.03f;
                    }
                    if (NPC.ai[1] >= 89f && NPC.ai[1] - step < 89f)
                    {
                        omega += 0.1f;
                    }
                }
            }
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.ai[0] < 10)
            {
                NPC.ai[0] += 1;
            }
            else
            {
                if(NPC.ai[1] < 90f)
                {
                    NPC.ai[1] += 1f;
                }
                else
                {
                    NPC.ai[1] = 91f;
                }
            }
            NPC.life = NPC.lifeMax;
            if (Math.Abs(omega) < 0.2f)
            {
                omega -= hitDirection * (float)damage / 1000f;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D tg = MythContent.QuickTexture("Bosses/CorruptMoth/NPCs/EvilPack");
            Color color = drawColor;
            Main.spriteBatch.Draw(tg, NPC.position + new Vector2(tg.Width / 2f, 0) - Main.screenPosition, new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(tg.Width / 2f, 0), 1f, effects, 0f);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D tg = MythContent.QuickTexture("Bosses/CorruptMoth/NPCs/EvilPackGlow");
            float C = (float)Math.Sqrt(Math.Max((90 - NPC.ai[1]) / 90f, 0));
            Color color = new Color(C, C, C, 0);
            Main.spriteBatch.Draw(tg, NPC.position + new Vector2(tg.Width / 2f, 0) - Main.screenPosition, new Rectangle?(NPC.frame), color, NPC.rotation, new Vector2(tg.Width / 2f, 0), 1f, effects, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.AnisotropicWrap,DepthStencilState.None,RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix) ;
            Vector2 CrackCenter = new Vector2(-14, 106).RotatedBy(NPC.rotation) + new Vector2(40, 0);
            if(NPC.ai[1] <= 90)
            {
                DrawCrack(CrackCenter + NPC.position - Main.screenPosition, Math.Clamp(NPC.ai[1], 0, 15), 0);
                DrawCrack(CrackCenter + NPC.position - Main.screenPosition, Math.Clamp(NPC.ai[1] - 8, 0, 15), 1);
                DrawCrack(CrackCenter + NPC.position - Main.screenPosition, Math.Clamp(NPC.ai[1] - 16, 0, 15), 2);
                DrawCrack(CrackCenter + NPC.position - Main.screenPosition, Math.Clamp(NPC.ai[1] - 24, 0, 15), 3);
                DrawCrack(CrackCenter + NPC.position - Main.screenPosition, Math.Clamp(NPC.ai[1] - 30, 0, 15), 4);
                DrawCrack(CrackCenter + NPC.position - Main.screenPosition, Math.Clamp(NPC.ai[1] - 36, 0, 15), 5);
                DrawCrack(CrackCenter + NPC.position - Main.screenPosition, Math.Clamp(NPC.ai[1] - 24, 0, 15), 6, (int)Math.Clamp((NPC.ai[1] - 36) / 2f, 1, 50));
            }
        }
        public void DrawCrack(Vector2 DrawCenter, float Radius, int type, int Power = 1)
        {
            Texture2D t0 = MythContent.QuickTexture("Bosses/CorruptMoth/NPCs/Crack" + type.ToString());
            
            List<Vertex2D> vertex2Ds = new List<Vertex2D>();
            for(int a = 1;a < Power + 1;a++)
            {
                Color color = new Color(1f / (float)a, 1f / (float)a, 1f / (float)a, 0);
                float scale = 2 + (a - 1) / 8f;
                Vector2 Move = new Vector2(-1, 1) * a;
                for (int x = 0; x < 10; x++)
                {
                    Vector2 DrawPoint1 = new Vector2(0, -Radius).RotatedBy(x / 5d * Math.PI);
                    Vector2 DrawPoint2 = new Vector2(0, -Radius).RotatedBy((x + 1) / 5d * Math.PI);
                    Vector2 dp1 = DrawPoint1.RotatedBy(NPC.rotation) * scale + Move;
                    Vector2 dp2 = DrawPoint2.RotatedBy(NPC.rotation) * scale + Move;
                    vertex2Ds.Add(new Vertex2D(DrawCenter + dp1, color, new Vector3(0.5f + DrawPoint1.X / t0.Width, 0.5f + DrawPoint1.Y / t0.Height, 0)));
                    vertex2Ds.Add(new Vertex2D(DrawCenter + dp2, color, new Vector3(0.5f + DrawPoint2.X / t0.Width, 0.5f + DrawPoint2.Y / t0.Height, 0)));
                    vertex2Ds.Add(new Vertex2D(DrawCenter, color, new Vector3(0.5f, 0.5f, 0)));
                }
            }

            Main.graphics.GraphicsDevice.Textures[0] =t0;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
        }
    }
}
