﻿using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Modules.MythModule.Common;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.MagnetSphere
{
    internal class MagnetSphereArray : ModProjectile, IWarpProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10000;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.Center = Projectile.Center * 0.7f + (player.Center + new Vector2(player.direction * 22, 12 * player.gravDir * (float)(0.2 + Math.Sin(Main.timeForVisualEffects / 18d) / 2d))) * 0.3f;
            Projectile.spriteDirection = player.direction;
            Projectile.velocity *= 0;
            if (player.itemTime > 0 && player.HeldItem.type == ItemID.MagnetSphere)
            {
                Projectile.timeLeft = player.itemTime + 60;
                if (Timer < 30)
                {
                    Timer++;
                }
            }
            else
            {
                Timer--;
                if (Timer < 0)
                {
                    Projectile.Kill();
                }
            }
            Player.CompositeArmStretchAmount PCAS = Player.CompositeArmStretchAmount.Full;

            player.SetCompositeArmFront(true, PCAS, (float)(-Math.Sin(Main.timeForVisualEffects / 18d) * 0.6 + 1.2) * -player.direction);
            Vector2 vTOMouse = Main.MouseWorld - player.Center;
            player.SetCompositeArmBack(true, PCAS, (float)(Math.Atan2(vTOMouse.Y, vTOMouse.X) - Math.PI / 2d));
            Projectile.rotation = player.fullRotation;

            RingPos = RingPos * 0.9f + new Vector2(-12 * player.direction, -24 * player.gravDir) * 0.1f;
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCs.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.hide = false;
            DrawMagicArray(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLineBlackShade"), new Color(1f, 1f, 1f, 1f));
            DrawMagicArray(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Vague"), new Color(0, 255, 174, 0));
            return false;
        }

        internal int Timer = 0;
        internal Vector2 RingPos = Vector2.Zero;

        public void DrawMagicArray(Texture2D tex, Color c0)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D Water = tex;
            Color c1 = new Color(c0.R * 0.39f / 255f, c0.G * 0.39f / 255f, c0.B * 0.39f / 255f, c0.A * 0.39f / 255f);
            DrawTexCircle(Timer * 1.6f, 22, c0, player.Center + RingPos - Main.screenPosition, Water, Main.timeForVisualEffects / 17);
            DrawTexCircle(Timer * 1.3f, 32, c1, player.Center + RingPos - Main.screenPosition, Water, -Main.timeForVisualEffects / 17);

            float timeRot = (float)(Main.timeForVisualEffects / 57d);
            Vector2 Point0 = player.Center + RingPos - Main.screenPosition;
            Vector2 Point1 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 0 + timeRot);
            Vector2 Point2 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 2 / 3d + timeRot);
            Vector2 Point3 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 4 / 3d - timeRot);

            Vector2 Point4 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 1 / 3d + timeRot * 2.4f);
            Vector2 Point5 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 3 / 3d - timeRot * 0.8f);
            Vector2 Point6 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 5 / 3d - timeRot);

            Vector2 Point7 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 1 / 3d + timeRot * 2);
            Vector2 Point8 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 3 / 3d - timeRot * 1.6f);
            Vector2 Point9 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 5 / 3d - timeRot * 1.1f);


            float Light1 = (float)(Math.Sin(Main.timeForVisualEffects / 3f + Math.PI / 3d * 1) + 0.2) / 1.4f;
            float Light2 = (float)(Math.Sin(Main.timeForVisualEffects / 3f + Math.PI / 3d * 2) + 0.2) / 1.4f;
            float Light3 = (float)(Math.Sin(Main.timeForVisualEffects / 4f + Math.PI / 3d * 3) + 0.2) / 1.4f;
            float Light4 = (float)(Math.Sin(Main.timeForVisualEffects / 3f + Math.PI / 3d * 4) + 0.2) / 1.4f;
            float Light5 = (float)(Math.Sin(Main.timeForVisualEffects / 2.3f + Math.PI / 3d * 4) + 0.2) / 1.4f;
            float Light6 = (float)(Math.Sin(Main.timeForVisualEffects / 3f + Math.PI / 3d * 6) + 0.2) / 1.4f;
            float Light7 = (float)(Math.Sin(Main.timeForVisualEffects / 5f + Math.PI / 3d * 4) + 0.4) / 2.4f;
            float Light8 = (float)(Math.Sin(Main.timeForVisualEffects / 3.3f + Math.PI / 3d * 4) + 0.3) / 1.8f;
            float Light9 = (float)(Math.Sin(Main.timeForVisualEffects / 3f + Math.PI / 3d * 6) + 0.2) / 1.4f;


            DrawTexLine(Point0, Point2, c1 * Light1, c1 * Light2, Water);
            DrawTexLine(Point0, Point3, c1 * Light2, c1 * Light3, Water);
            DrawTexLine(Point0, Point1, c1 * Light3, c1 * Light4, Water);

            DrawTexLine(Point0, Point5, c1 * Light4, c1 * Light5, Water);
            DrawTexLine(Point0, Point6, c1 * Light5, c1 * Light6, Water);
            DrawTexLine(Point0, Point4, c1 * Light6, c1 * Light1, Water);

            DrawTexLine(Point0, Point7, c1 * Light4, c1 * Light7, Water);
            DrawTexLine(Point0, Point8, c1 * Light5, c1 * Light9, Water);
            DrawTexLine(Point0, Point9, c1 * Light6, c1 * Light8, Water);
        }

        private static void DrawTexCircle(float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
        {
            List<Vertex2D> circle = new List<Vertex2D>();
            for (int h = 0; h < radious / 2; h++)
            {
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(1, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(addRot), color, new Vector3(1, 0, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(addRot), color, new Vector3(0, 0, 0)));
            if (circle.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
            }
        }

        public static void DrawTexLine(Vector2 StartPos, Vector2 EndPos, Color color1, Color color2, Texture2D tex)
        {
            float Wid = 12f;
            float MinR = Math.Max(color1.G / 255f, color2.G / 255f);
            Wid *= MinR;
            Vector2 Width = Vector2.Normalize(StartPos - EndPos).RotatedBy(Math.PI / 2d) * Wid;

            List<Vertex2D> vertex2Ds = new List<Vertex2D>();

            for (int x = 0; x < 3; x++)
            {
                float Value0 = (float)(Main.timeForVisualEffects / 291d + 20) % 1f;
                float Value1 = (float)(Main.timeForVisualEffects / 291d + 20.09) % 1f;
                if (Value1 < Value0)
                {
                    float D0 = 1 - Value0;
                    Vector2 Delta = EndPos - StartPos;
                    vertex2Ds.Add(new Vertex2D(StartPos + Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 0, 0)));
                    vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(1, 0, 0)));
                    vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 1, 0)));

                    vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(1, 0, 0)));
                    vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 - Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(1, 1, 0)));
                    vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 1, 0)));

                    vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 + Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 0, 0)));
                    vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 0, 0)));
                    vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 1, 0)));

                    vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 0, 0)));
                    vertex2Ds.Add(new Vertex2D(EndPos - Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 1, 0)));
                    vertex2Ds.Add(new Vertex2D(StartPos + Delta * D0 - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(0, 1, 0)));

                    continue;
                }
                vertex2Ds.Add(new Vertex2D(StartPos + Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 0, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 1, 0)));

                vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos - Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 1, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 1, 0)));
            }

            Main.graphics.GraphicsDevice.Textures[0] = tex;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
        }

        public void DrawWarp()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            KEx.CurrentTechnique.Passes[0].Apply();
            Player player = Main.player[Projectile.owner];
            DrawTexCircle(Timer * 1.2f, 52, new Color(64, 70, 255, 0), player.Center + RingPos - Main.screenPosition, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLine"), Main.timeForVisualEffects / 17);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}