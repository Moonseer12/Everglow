using Terraria.DataStructures;
using Terraria.IO;
using Terraria.WorldBuilding;
using Everglow.Minortopography.GiantPinetree.TilesAndWalls;

namespace Everglow.Minortopography.GiantPinetree;

public class GiantPinetree : ModSystem
{
	private class GiantPinetreeGenPass : GenPass
	{
		public GiantPinetreeGenPass() : base("GiantPinetree", 500)
		{
		}

		public override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			//TODO:翻译：建造巨大的雪松
			Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.Everlow.Common.WorldSystem.BuildMothCave");

			BuildGiantPinetree();
		}
	}

	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) => tasks.Add(new GiantPinetreeGenPass());
	/// <summary>
	/// 在雪地表面随机获取一点
	/// </summary>
	/// <returns></returns>
	public static Point16 RandomPointInSurfaceSnow()
	{
		var aimPoint = new List<Point16>();
		int minYCoord = Main.maxTilesY - 100;
		for (int i = 33; i < Main.maxTilesX - 34; i += 33)
		{
			for (int buildCoordY = 12; buildCoordY < Main.maxTilesY - 100; buildCoordY += 6)
			{
				Tile tile = Main.tile[i, buildCoordY];
				if (tile.TileType == TileID.SnowBlock && tile.HasTile)
				{
					aimPoint.Add(new Point16(i, buildCoordY));
					if (buildCoordY < minYCoord)
						minYCoord = buildCoordY;
					break;
				}
			}
		}
		var newAimPoint = new List<Point16>();
		foreach (Point16 point in aimPoint)
		{
			if (point.Y <= minYCoord + 30)
				newAimPoint.Add(point);
		}
		return newAimPoint[Main.rand.Next(newAimPoint.Count)];
	}
	/// <summary>
	/// 建造巨大的雪松
	/// </summary>
	public static void BuildGiantPinetree()
	{

		Point16 centerPoint = RandomPointInSurfaceSnow();

		//TODO 尝试修复小世界雪松过高的问题

		//避免小世界生成位置突兀
		int positonX = Main.maxTilesX == 4200 ? centerPoint.X + 2 : centerPoint.X;

		//降低高度
		int positonY = centerPoint.Y - 8;

		float treeSize = Main.rand.NextFloat(16f, 20f);
		PlacePineLeaves(positonX, positonY, 0, treeSize * 7.5f, new Vector2(0, -1));
		if (Main.snowBG[2] == 260)//在这种条件下，背景符合这段代码生成的松树
		{

		}
		float trunkWidth = treeSize;//随机摇宽度
		int buildCoordY = 0;
		for (int a = -3; a <= 3; a++)
		{
			GenerateRoots(new Point16(positonX, positonY), 0, a / 2f);//随机发射树根
		}
		while (trunkWidth > 0)
		{
			buildCoordY--;
			if (buildCoordY + positonY >= Main.maxTilesY - 10 || buildCoordY + positonY <= 10 || positonX <= 20 || positonX >= Main.maxTilesX - 20)//防止超界
				break;
			for (int i = (int)-trunkWidth; i <= (int)trunkWidth; i++)
			{
				Tile tile = Main.tile[i + positonX, buildCoordY + positonY];
				if (i > -trunkWidth + 4 || i < trunkWidth - 4 || trunkWidth < 4)
					tile.HasTile = false;
			}
			trunkWidth -= (float)(Math.Sin(buildCoordY * 0.8) * 0.5 + 0.2);//制造松树一层一层的效果
		}
		GenerateTrunkWall(new Point16(positonX, positonY), 7, 50);
		GenerateBranch(new Point16(positonX, positonY - 20), -1.4f, 26);
		GenerateBranch(new Point16(positonX, positonY - 30), 1.4f, 20);
		GenerateBranch(new Point16(positonX, positonY - 40), -1.4f, 12);
		GenerateBranch(new Point16(positonX, positonY - 50), 1.4f, 6);

		//平滑木头部分
		SmoothTile(positonX - 60, positonY - 10, 120, 250, ModContent.TileType<PineWood>());
	}

	/// <summary>
	/// 建造根系,起始角度=0时向下,根系会在起始位置以起始角度发射,并逐渐转向目标角度,如果天然卷曲,根系会在末尾处再发生一次拐弯
	/// </summary>
	/// <param name="startPoint"></param>
	/// <param name="startRotation"></param>
	/// <param name="trendRotation"></param>
	/// <param name="naturalCurve"></param>
	public static void GenerateRoots(Point16 startPoint, float startRotation = 0, float trendRotation = 0, bool naturalCurve = true)
	{
		int positonX = startPoint.X;
		int positonY = startPoint.Y;
		float trunkWidth = Main.rand.NextFloat(8f, 10f);//随机摇宽度
		var rootPosition = new Vector2(0, 0);
		Vector2 rootVelocity = new Vector2(0, 1).RotatedBy(startRotation);//根系当前速度
		Vector2 rootTrendVelocity = new Vector2(0, 1).RotatedBy(trendRotation);//根系稳定趋势速度
		float omega = Main.rand.NextFloat(-0.2f, 0.2f);//末端旋转的角速度
		if (!naturalCurve)//如果禁止了自然旋转,角速度=0
			omega = 0;
		float startToRotatedByOmega = Main.rand.NextFloat(1.81f, 3.62f);//算作末端的起始位置，这里用剩余宽度统计
		while (trunkWidth > 0)
		{
			for (int a = (int)-trunkWidth; a <= (int)trunkWidth; a++)
			{
				Vector2 tilePosition = rootPosition + a * rootVelocity.RotatedBy(MathHelper.PiOver2) * 0.6f;
				int i = (int)tilePosition.X;
				int buildCoordY = (int)tilePosition.Y;
				if (buildCoordY + positonY >= Main.maxTilesY - 10 || buildCoordY + positonY <= 10 || -10 + positonX <= 10 || 10 + positonX >= Main.maxTilesX + 10)//防止超界
					break;
				Tile tile = Main.tile[i + positonX, buildCoordY + positonY];
				//if(a == (int)trunkWidth && trunkWidth <= 2)//孤立像素一般的物块自动剪除，然后破
				//{
				//	Tile tileTop = Main.tile[i + positonX, buildCoordY + positonY - 1];
				//	Tile tileBottom = Main.tile[i + positonX, buildCoordY + positonY + 1];
				//	Tile tileLeft = Main.tile[i + positonX - 1, buildCoordY + positonY];
				//	Tile tileRight = Main.tile[i + positonX + 1, buildCoordY + positonY];
				//	if(tileTop.TileType != (ushort)ModContent.TileType<PineWood>() && tileBottom.TileType != (ushort)ModContent.TileType<PineWood>() && tileLeft.TileType != (ushort)ModContent.TileType<PineWood>() && tileRight.TileType != (ushort)ModContent.TileType<PineWood>())
				//	{
				//		trunkWidth = 0;
				//		break;
				//	}
				//}
				if (a <= -trunkWidth + 4 || a >= trunkWidth - 4)//在靠边的部位为实木块
				{
					if (tile.WallType != ModContent.WallType<PineWoodWall>())//防止松树块互相重合
					{
						tile.TileType = (ushort)ModContent.TileType<PineWood>();
						tile.HasTile = true;
					}
				}
				else//空心根管
				{
					tile.HasTile = false;
					tile.LiquidAmount = 0;
				}
				if (a > -trunkWidth + 2 && a < trunkWidth - 2)//铺上墙壁
					tile.WallType = (ushort)ModContent.WallType<PineWoodWall>();
			}
			rootPosition += rootVelocity;
			if (trunkWidth > startToRotatedByOmega)//没有收束到末端
				rootVelocity = rootVelocity * 0.95f + rootTrendVelocity * 0.05f;
			else//已经收束到末端
			{
				rootVelocity = rootVelocity.RotatedBy(omega * (startToRotatedByOmega - trunkWidth) / startToRotatedByOmega);
			}
			if (naturalCurve)//只有自然卷曲才会导致以下现象
			{
				//重力因素也会影响根系,下面判定根系悬空程度
				int surroundTileCount = 0;//我们判定周围存在方块的数量来推断悬空程度，存在方块越少越悬空
				for (int b = 0; b < 12; b++)
				{
					Vector2 tilePosition = rootPosition + 3 * rootVelocity.RotatedBy(b / 6d * Math.PI);
					int i = (int)tilePosition.X;
					int buildCoordY = (int)tilePosition.Y;
					Tile tile = Main.tile[i + positonX, buildCoordY + positonY];
					if (tile.HasTile || tile.WallType == (ushort)ModContent.WallType<PineWoodWall>()/*这一项是为了防止自己干扰自己*/)
						surroundTileCount++;
				}
				if (surroundTileCount < 6)
				{
					rootVelocity += new Vector2(0, (6 - surroundTileCount) / 16f);//重力自然下垂
					rootVelocity = Vector2.Normalize(rootVelocity);//化作单位向量
					trunkWidth += (6 - surroundTileCount) / 50f;//防止下降过程根系过分收束
				}
				else if (surroundTileCount > 9)
				{
					trunkWidth -= (surroundTileCount - 9) / 60f;//周围物块太多，产生阻力，加快收束
				}
			}
			trunkWidth -= 0.1f;
			if (trunkWidth < 1.8f)//太细了，准备破
			{
				if (trunkWidth < 1.8f)//破掉吧
				{
					break;
				}
			}
			

		}
	}
	/// <summary>
	/// 根据分型生成树叶
	/// </summary>
	/// <param name="i"></param>
	/// <param name="buildCoordY"></param>
	/// <param name="iteration"></param>
	/// <param name="strength"></param>
	/// <param name="direction"></param>
	public static void PlacePineLeaves(int i, int buildCoordY, int iteration, float strength, Vector2 direction)
	{
		if (iteration > 50)//万一发散就完了
			return;
		for (int x = 0; x < strength; x++)
		{
			int aBSXStr = Math.Min((int)((strength - x) * 0.16f), 8);

			for (int y = -aBSXStr; y < aBSXStr + 1; y++)
			{
				Vector2 normalizedDirection = Utils.SafeNormalize(direction, new Vector2(0, -1));
				Vector2 VnormalizedDirection = normalizedDirection.RotatedBy(Math.PI / 2d);
				int a = (int)(i + normalizedDirection.X * x + VnormalizedDirection.X * y);
				int b = (int)(buildCoordY + normalizedDirection.Y * x + VnormalizedDirection.Y * y);
				if (b >= Main.maxTilesY - 10 || b <= 10 || a <= 20 || a >= Main.maxTilesX - 20)//防止超界
					break;
				var tile = Main.tile[a, b];
				tile.TileType = (ushort)ModContent.TileType<PineLeaves>();
				tile.HasTile = true;
				if (strength - x > 1)
					tile.WallType = (ushort)ModContent.WallType<PineLeavesWall>();
				if (y == 0)
				{
					if (x % 6 == 1)
						PlacePineLeaves(a, b, iteration + 1, (strength - x) * 0.34f, normalizedDirection.RotatedBy(Math.PI * 0.3));
					if (x % 6 == 4)
						PlacePineLeaves(a, b, iteration + 1, (strength - x) * 0.34f, normalizedDirection.RotatedBy(-Math.PI * 0.3));
				}
			}
		}
	}
	/// <summary>
	/// 生成枝干
	/// </summary>
	/// <param name="startPoint"></param>
	/// <param name="startRotation"></param>
	/// <param name="trendRotation"></param>
	/// <param name="naturalCurve"></param>
	public static void GenerateBranch(Point16 startPoint, float startRotation = 0, float expectLength = 40)
	{
		int positonX = startPoint.X;
		int positonY = startPoint.Y;
		float trunkWidth = Main.rand.NextFloat(2f, 3f);//随机摇宽度
		var branchPosition = new Vector2(0, 0);
		Vector2 branchVelocity = new Vector2(0, -1).RotatedBy(startRotation);//根系当前速度
		Vector2 branchTrendVelocity = branchVelocity;//根系稳定趋势速度
		int iteration = 0;
		while (trunkWidth > 0)
		{
			iteration++;
			for (int a = (int)-trunkWidth; a <= (int)trunkWidth; a++)
			{
				Vector2 tilePosition = branchPosition + a * branchVelocity.RotatedBy(MathHelper.PiOver2) * 0.6f;
				int i = (int)tilePosition.X;
				int buildCoordY = (int)tilePosition.Y;
				if (buildCoordY + positonY >= Main.maxTilesY - 10 || buildCoordY + positonY <= 10 || -10 + positonX <= 10 || 10 + positonX >= Main.maxTilesX + 10)//防止超界
					break;
				Tile tile = Main.tile[i + positonX, buildCoordY + positonY];
				if (a <= -trunkWidth + 4 || a >= trunkWidth - 4)//在靠边的部位为实木块
				{
					tile.TileType = (ushort)ModContent.TileType<PineWood>();
					tile.HasTile = true;
				}
				else//空心根管
				{
					tile.HasTile = false;
					tile.LiquidAmount = 0;
				}
				if (a > -trunkWidth + 2 && a < trunkWidth - 2)//铺上墙壁
					tile.WallType = (ushort)ModContent.WallType<PineWoodWall>();
			}
			branchPosition += branchVelocity;
			if (iteration > expectLength)//已经收束到末端
			{
				branchVelocity = branchVelocity * 0.75f + branchTrendVelocity * 0.25f;
				trunkWidth -= 0.4f;
			}
			else//没有收束到末端
			{
				branchVelocity = branchVelocity * 0.8f + new Vector2(Math.Sign(branchVelocity.X), 0) * 0.2f;
			}
			
			if (trunkWidth < 0.8f)//太细了，准备破
			{
				if (trunkWidth < 0.8f)//破掉吧
				{
					break;
				}
			}
		}
	}
	/// <summary>
	/// 生成树干的墙壁部分
	/// </summary>
	/// <param name="startPoint"></param>
	/// <param name="width"></param>
	/// <param name="expectLength"></param>
	public static void GenerateTrunkWall(Point16 startPoint, float width = 5, float expectLength = 40)
	{
		int positonX = startPoint.X;
		int positonY = startPoint.Y;
		Vector2 trunkVelocity = new Vector2(0, -1);
		Vector2 trunkPosition = Vector2.zeroVector;
		int iteration = 0;
		while (width > 0)
		{
			iteration++;
			for (int a = (int)-width; a <= (int)width; a++)
			{
				Vector2 tilePosition = trunkPosition + a * trunkVelocity.RotatedBy(MathHelper.PiOver2) * 0.6f;
				int i = (int)tilePosition.X;
				int buildCoordY = (int)tilePosition.Y;
				if (buildCoordY + positonY >= Main.maxTilesY - 10 || buildCoordY + positonY <= 10 || -10 + positonX <= 10 || 10 + positonX >= Main.maxTilesX + 10)//防止超界
					break;
				Tile tile = Main.tile[i + positonX, buildCoordY + positonY];
				if (a > -width && a < width)//铺上墙壁
					tile.WallType = (ushort)ModContent.WallType<PineWoodWall>();
			}
			trunkPosition += trunkVelocity;
			if (iteration > expectLength)//已经收束到末端
			{
				width -= 0.4f;
			}
			if (width < 0.8f)//太细了，准备破
			{
				if (width < 0.8f)//破掉吧
				{
					break;
				}
			}
		}
	}
	/// <summary>
	/// 平滑：参数分别是左上点世界坐标，宽，高，限定物块种类
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="width"></param>
	/// <param name="height"></param>
	/// <param name="tileType"></param>
	private static void SmoothTile(int x = 0, int y = 0, int width = 0, int height = 0, int tileType = -1)
	{
		for (int i = 0; i < width; i += 1)
		{
			if (i + x > Main.maxTilesX - 20)
			{
				break;
			}
			if (i + x < 20)
			{
				break;
			}
			for (int j = 0; j < height; j += 1)
			{
				if(j + y > Main.maxTilesY - 20)
				{
					break;
				}
				if (j + y < 20)
				{
					break;
				}
				if (tileType == -1)
				{
					Tile.SmoothSlope(x + i, y + j, false);
					WorldGen.TileFrame(x + i, y + j, true, false);
					WorldGen.SquareWallFrame(x + i, y + j, true);
				}
				else
				{
					if (Main.tile[x + i, y + j].TileType == tileType)
					{
						Tile.SmoothSlope(x + i, y + j, false);
						WorldGen.TileFrame(x + i, y + j, true, false);
						WorldGen.SquareWallFrame(x + i, y + j, true);
					}
				}
			}
		}
	}
}

