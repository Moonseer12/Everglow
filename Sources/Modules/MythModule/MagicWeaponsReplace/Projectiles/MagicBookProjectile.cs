﻿using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles
{
    /// <summary>
    /// 魔法书类
    /// </summary>
    public abstract class MagicBookProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10000;
            Projectile.DamageType = DamageClass.MagicSummonHybrid;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            SetDef();
        }
        /// <summary>
        /// 最好不要动计时器，计算书本的翻开程度，甚至决定了书本是否kill
        /// </summary>
        internal int Timer = 0;
        /// <summary>
        /// 产生粒子的类型
        /// </summary>
        internal int DustType = -1;
        /// <summary>
        /// 发射的弹幕
        /// </summary>
        internal int ProjType = -1;
        /// <summary>
        /// 物品种类
        /// </summary>
        internal int ItemType = -1;
        /// <summary>
        /// 绘制出来的特效书尺寸,和物品贴图大小无关,默认12
        /// </summary>
        internal float BookScale = 12f;
        /// <summary>
        /// 是否使用荧光效果
        /// </summary>
        internal bool UseGlow = true;
        /// <summary>
        /// 荧光的颜色
        /// </summary>
        internal Color glowColor = new Color(255, 255, 255, 0);

        public virtual void SetDef()
        {
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.Center = Projectile.Center * 0.7f + (player.Center + new Vector2(player.direction * 22, 12 * player.gravDir * (float)(0.2 + Math.Sin(Main.timeForVisualEffects / 18d) / 2d))) * 0.3f;//书跟着玩家飞
            Projectile.spriteDirection = player.direction;
            Projectile.velocity *= 0;
            if (player.itemTime > 0 && player.HeldItem.type == ItemType)//检测手持物品
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
            Player.CompositeArmStretchAmount PCAS = Player.CompositeArmStretchAmount.Full;//玩家动作

            player.SetCompositeArmFront(true, PCAS, (float)(-Math.Sin(Main.timeForVisualEffects / 18d) * 0.6 + 1.2) * -player.direction);
            Vector2 vTOMouse = Main.MouseWorld - player.Center;
            player.SetCompositeArmBack(true, PCAS, (float)(Math.Atan2(vTOMouse.Y, vTOMouse.X) - Math.PI / 2d));
            Projectile.rotation = player.fullRotation;

            if(ProjType == -1)
            {
                return;
            }
            if (player.itemTime == 2)
            {
                Vector2 velocity = Utils.SafeNormalize(vTOMouse, Vector2.Zero) * player.HeldItem.shootSpeed;
                Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + velocity * 6, velocity, ProjType, player.HeldItem.damage, player.HeldItem.knockBack, player.whoAmI);
                p.CritChance = (int)player.GetCritChance(DamageClass.Generic);
            }
        }
        public override void PostDraw(Color lightColor)
        {
            if(ItemType == -1)
            {
                return;
            }
            Texture2D Book = TextureAssets.Item[ItemType].Value;
            Texture2D BookGlow = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Item_" + ItemType + "_Glow");
            Texture2D Paper = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/MagicBookPaper");

            DrawBack(Book);
            DrawBack(BookGlow, UseGlow);
            DrawPaper(Paper);
            DrawFront(Book);
            DrawFront(BookGlow, UseGlow);
        }
        /// <summary>
        /// 对于书页的绘制，包括正在被翻起的以及堆叠在前后两侧的。关于纸张的绘制，因为较小，都没有经过严格的投影，随手捏了一个近似函数，只保证视觉效果上大致正确
        /// </summary>
        /// <param name="tex"></param>
        public void DrawPaper(Texture2D tex, bool Glowing = false)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 X0 = new Vector2(BookScale * player.direction, BookScale * player.gravDir) * 0.45f;//把书本贴图（有内容部分）算作一个矩形，这里表示这个矩形的半宽。玩家朝右，重力方向朝下时指向右下
            Vector2 Y0 = new Vector2(BookScale * player.direction, -BookScale * player.gravDir) * 0.64f;//把书本贴图（有内容部分）算作一个矩形，这里表示这个矩形的半长，方向与X0垂直，玩家朝右，重力方向朝下时指向右上
            Color c0 = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f));
            if (Glowing)//如果开荧光，用不同的颜色，否则取光照色
            {
                c0 = glowColor;
            }
            //后部书页
            for (int x = 0; x < 8/*一共8页*/; x++)
            {
                List<Vertex2D> bars = new List<Vertex2D>();
                for (int i = 0; i < 10/*一页相当于10个【矩形长条】组成的曲面*/; ++i)
                {
                    double rot = Timer / 270d + i * Timer / 400d * (1 + Math.Sin(Main.timeForVisualEffects / 7d) * 0.4)/*一个和时间，i都有关的函数，制造了页的曲面效果，timer到30就停了，Main.timeForVisualEffects一直变化*/;
                    rot -= x / 540d * (Timer);//每一页之间的角度差
                    rot += Projectile.rotation;//当然也收到弹幕本身的旋转角度影响
                    Vector2 BasePos = Projectile.Center + X0 - X0.RotatedBy(rot) * i / 4.5f;//【矩形长条】长轴的中点，借X0遍历经过弯曲的宽轴【-X0,X0】，如果你意识到了X0是半宽轴，这里就不会有什么疑问

                    float UpX = MathHelper.Lerp(16f / tex.Width, 27f / tex.Width, i / 9f);//纹理坐标的横向插值
                    float UpY = MathHelper.Lerp(0, 11f / tex.Height, i / 9f);//纹理坐标的纵向插值
                    Vector2 Up = new Vector2(UpX, UpY);//合并
                    Vector2 Down = Up + new Vector2(-15f / tex.Width, 15f / tex.Height);
                    //上面这几行如果看不懂，就看这个目录下的BookDrawPrinciple.png

                    if (Math.Abs(rot) > Math.PI / 2d)//TR不支持禁用背景剔除，只能这样取巧
                    {
                        if (player.direction * player.gravDir == 1)//分向讨论
                        {
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        }
                        else
                        {
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        }
                    }
                    else
                    {
                        if (player.direction * player.gravDir == 1)
                        {
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        }
                        else
                        {
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        }
                    }
                }
                if (bars.Count > 0)
                {
                    Main.graphics.GraphicsDevice.Textures[0] = tex;
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
                }
            }

            //正在翻起的书页
            List<Vertex2D> barsII = new List<Vertex2D>();
            for (int i = 0; i < 10; ++i)
            {
                double rotII = -Timer / 270d - i * Timer / 400d * (1 + Math.Sin(Main.timeForVisualEffects / 7d + 1) * 0.4);//翻页起点角度
                rotII += 8 / 18d / 30d * (Timer);

                double rotIII = Timer / 270d + i * Timer / 400d * (1 + Math.Sin(Main.timeForVisualEffects / 7d) * 0.4);//翻页终点角度
                rotIII -= 8 / 18d / 30d * (Timer);

                double rotIV = MathHelper.Lerp((float)rotII, (float)rotIII, (float)(Main.timeForVisualEffects / 15d + Math.Sin(Main.timeForVisualEffects / 62d) * 9) % 1f);//翻页过程角度插值
                rotIV += Projectile.rotation;
                Vector2 BasePos = Projectile.Center + X0 - X0.RotatedBy(rotIV) * i / 4.5f - Y0 * 0.05f - X0 * 0.02f;//前半部分已经讲过了，至于为什么多出来【- Y0 * 0.05f - X0 * 0.02f】，是因为要凸显出正在被翻起的那一页

                float UpX = MathHelper.Lerp(16f / tex.Width, 27f / tex.Width, i / 9f);
                float UpY = MathHelper.Lerp(0, 11f / tex.Height, i / 9f);
                Vector2 Up = new Vector2(UpX, UpY);
                Vector2 Down = Up + new Vector2(-15f / tex.Width, 15f / tex.Height);

                if (Math.Abs(rotIV) > Math.PI / 2d)
                {
                    if (player.direction == 1)
                    {
                        barsII.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        barsII.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                    }
                    else
                    {
                        barsII.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        barsII.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                    }
                }
                else
                {
                    if (player.direction * player.gravDir == 1)
                    {
                        barsII.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        barsII.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                    }
                    else
                    {
                        barsII.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        barsII.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                    }
                }
            }
            if (barsII.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, barsII.ToArray(), 0, barsII.Count - 2);
            }
            //前部书页
            for (int x = 0; x < 8; x++)
            {
                List<Vertex2D> bars = new List<Vertex2D>();
                for (int i = 0; i < 10; ++i)
                {
                    double rot = -Timer / 270d - i * Timer / 400d * (1 + Math.Sin(Main.timeForVisualEffects / 7d + 1) * 0.4);
                    rot += x / 18d / 30d * (Timer);
                    rot += Projectile.rotation;
                    Vector2 BasePos = Projectile.Center + X0 - X0.RotatedBy(rot) * i / 4.5f - Y0 * 0.05f - X0 * 0.02f;//【- Y0 * 0.05f - X0 * 0.02f】再现，为了不让翻起的那一页到前面时凸出来

                    float UpX = MathHelper.Lerp(16f / tex.Width, 27f / tex.Width, i / 9f);
                    float UpY = MathHelper.Lerp(0 / tex.Height, 11f / tex.Height, i / 9f);
                    Vector2 Up = new Vector2(UpX, UpY);
                    Vector2 Down = Up + new Vector2(-15f / tex.Width, 15f / tex.Height);

                    if (Math.Abs(rot) > Math.PI / 2d)
                    {
                        if (player.direction * player.gravDir == 1)
                        {
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        }
                        else
                        {
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        }
                    }
                    else
                    {
                        if (player.direction * player.gravDir == 1)
                        {
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        }
                        else
                        {
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        }
                    }
                }
                if (bars.Count > 0)
                {
                    Main.graphics.GraphicsDevice.Textures[0] = tex;
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
                }
            }
        }

        /// <summary>
        /// 对于书本后部的绘制，只有一页,This function is drawing the back side of book, no "drawback".
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="Glowing"></param>
        public void DrawBack(Texture2D tex, bool Glowing = false)
        {
            //这里应该不用注解了（
            Player player = Main.player[Projectile.owner];
            Vector2 X0 = new Vector2(BookScale * player.direction, BookScale * player.gravDir) * 0.5f;
            Vector2 Y0 = new Vector2(BookScale * player.direction, -BookScale * player.gravDir) * 0.707f;
            Color c0 = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f));
            if (Glowing)
            {
                c0 = glowColor;
            }
            List<Vertex2D> bars = new List<Vertex2D>();
            for (int i = 0; i < 10; ++i)
            {
                double rot = Timer / 270d + i * Timer / 400d * (1 + Math.Sin(Main.timeForVisualEffects / 7d) * 0.4);
                Vector2 BasePos = Projectile.Center + X0 - X0.RotatedBy(rot) * i / 4.5f;

                float UpX = MathHelper.Lerp(16f / tex.Width, 27f / tex.Width, i / 9f);
                float UpY = MathHelper.Lerp(0 / tex.Height, 11f / tex.Height, i / 9f);
                Vector2 Up = new Vector2(UpX, UpY);
                Vector2 Down = Up + new Vector2(-15f / tex.Width, 15f / tex.Height);

                rot += Projectile.rotation;
                if (Math.Abs(rot) > Math.PI / 2d)
                {
                    if (player.direction * player.gravDir == 1)
                    {
                        bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                    }
                    else
                    {
                        bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                    }
                }
                else
                {
                    if (player.direction * player.gravDir == 1)
                    {
                        bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                    }
                    else
                    {
                        bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                    }
                }
            }
            if (bars.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
        }
        /// <summary>
        /// 对于书本前部的绘制
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="Glowing"></param>
        public void DrawFront(Texture2D tex, bool Glowing = false)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 X0 = new Vector2(BookScale * player.direction, BookScale * player.gravDir) * 0.5f;
            Vector2 Y0 = new Vector2(BookScale * player.direction, -BookScale * player.gravDir) * 0.707f;
            Color c0 = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f));
            if (Glowing)
            {
                c0 = glowColor;
            }
            List<Vertex2D> bars = new List<Vertex2D>();
            for (int i = 0; i < 10; ++i)
            {
                double rot = -Timer / 270d - i * Timer / 400d * (1 + Math.Sin(Main.timeForVisualEffects / 7d + 1) * 0.4);
                rot += Projectile.rotation;
                Vector2 BasePos = Projectile.Center + X0 - X0.RotatedBy(rot) * i / 4.5f - Y0 * 0.05f - X0 * 0.02f;

                float UpX = MathHelper.Lerp(16f / tex.Width, 27f / tex.Width, i / 9f);
                float UpY = MathHelper.Lerp(0 / tex.Height, 11f / tex.Height, i / 9f);
                Vector2 Up = new Vector2(UpX, UpY);
                Vector2 Down = Up + new Vector2(-15f / tex.Width, 15f / tex.Height);

                if (Math.Abs(rot) > Math.PI / 2d)
                {
                    if (player.direction * player.gravDir == 1)
                    {
                        bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                    }
                    else
                    {
                        bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                    }
                }
                else
                {
                    if (player.direction * player.gravDir == 1)
                    {
                        bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                    }
                    else
                    {
                        bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                    }
                }
            }
            if (bars.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
        }

        public override void Kill(int timeLeft)
        {
            if(DustType == -1)
            {
                return;
            }
            Player player = Main.player[Projectile.owner];
            Vector2 X0 = new Vector2(BookScale * player.direction, BookScale * player.gravDir) * 0.5f;
            Vector2 Y0 = new Vector2(BookScale * player.direction, -BookScale * player.gravDir) * 0.707f;
            for (int i = 0; i < 10; ++i)
            {
                double rot = 0;
                rot += Projectile.rotation;
                Vector2 BasePos = Projectile.Center + X0 - X0.RotatedBy(rot) * i / 4.5f;
                Dust d0 = Dust.NewDustDirect(BasePos - Y0, 0, 0, DustType);
                d0.noGravity = true;
                Dust d1 = Dust.NewDustDirect(BasePos + Y0, 0, 0, DustType);
                d1.noGravity = true;
            }
            for (int i = 0; i < 14; ++i)
            {
                double rot = 0;
                rot += Projectile.rotation;
                Vector2 BasePos = Projectile.Center + Y0 - Y0.RotatedBy(rot) * i / 4.5f;
                Dust d0 = Dust.NewDustDirect(BasePos - X0, 0, 0, DustType);
                d0.noGravity = true;
                Dust d1 = Dust.NewDustDirect(BasePos + X0, 0, 0, DustType);
                d1.noGravity = true;
            }
        }
    }
}