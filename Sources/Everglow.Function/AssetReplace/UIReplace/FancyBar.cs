using System.Reflection;
using ReLogic.Content;
using Terraria.GameContent.UI.ResourceSets;

namespace Everglow.Commons.AssetReplace.UIReplace;

public class FancyBar
{
	public Asset<Texture2D> HeartFillRed;
	public Asset<Texture2D> HeartFillGold;
	public Asset<Texture2D> HeartLeft;
	public Asset<Texture2D> HeartMiddle;
	public Asset<Texture2D> HeartRight;
	public Asset<Texture2D> HeartSingleFancy;
	public Asset<Texture2D> HeartRightFancy;
	public Asset<Texture2D> StarA;
	public Asset<Texture2D> StarB;
	public Asset<Texture2D> StarC;
	public Asset<Texture2D> StarFill;
	public Asset<Texture2D> StarSingle;

	/// <summary>
	/// 根据传入的路径读取Texture2D
	/// </summary>
	/// <param name="path">贴图组在Resources文件夹内的名字，比如UISkinMyth</param>
	public void LoadTextures(string path)
	{
		HeartFillRed = UIReplaceModule.GetTexture($"{path}/Bars/Fancy/Heart_Fill");
		HeartFillGold = UIReplaceModule.GetTexture($"{path}/Bars/Fancy/Heart_Fill_B");
		HeartLeft = UIReplaceModule.GetTexture($"{path}/Bars/Fancy/Heart_Left");
		HeartMiddle = UIReplaceModule.GetTexture($"{path}/Bars/Fancy/Heart_Middle");
		HeartRight = UIReplaceModule.GetTexture($"{path}/Bars/Fancy/Heart_Right");
		HeartSingleFancy = UIReplaceModule.GetTexture($"{path}/Bars/Fancy/Heart_Single_Fancy");
		HeartRightFancy = UIReplaceModule.GetTexture($"{path}/Bars/Fancy/Heart_Right_Fancy");
		StarA = UIReplaceModule.GetTexture($"{path}/Bars/Fancy/Star_A");
		StarB = UIReplaceModule.GetTexture($"{path}/Bars/Fancy/Star_B");
		StarC = UIReplaceModule.GetTexture($"{path}/Bars/Fancy/Star_C");
		StarFill = UIReplaceModule.GetTexture($"{path}/Bars/Fancy/Star_Fill");
		StarSingle = UIReplaceModule.GetTexture($"{path}/Bars/Fancy/Star_Single");
	}

	public void ReplaceTextures()
	{
		ReplaceForInstance(UIReplaceModule.PlayerResourceSets["New"]);
		ReplaceForInstance(UIReplaceModule.PlayerResourceSets["NewWithText"]);
	}

	private void ReplaceForInstance(IPlayerResourcesDisplaySet resourceBar)
	{
		if (resourceBar is not FancyClassicPlayerResourcesDisplaySet instance)
		{
			Ins.Logger.Warn("FancyBar sprites replacement loading failed, sprite replacement would not work.");
			return;
		}

		instance._heartLeft = HeartLeft;
		instance._heartMiddle = HeartMiddle;
		instance._heartRight = HeartRight;
		instance._heartRightFancy = HeartRightFancy;
		instance._heartFill = HeartFillRed;
		instance._heartFillHoney = HeartFillGold;
		instance._starTop = StarA;
		instance._starMiddle = StarB;
		instance._starBottom = StarC;
		instance._starSingle = StarSingle;
		instance._starFill = StarFill;
	}
}
