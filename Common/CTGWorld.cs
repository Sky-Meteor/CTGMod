using System.Collections.Generic;
using CTGMod.Common.WorldGeneration;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace CTGMod.Common;

public class CTGWorld : ModSystem
{
    public override void SaveWorldData(TagCompound tag)
    {
        
    }

    public override void LoadWorldData(TagCompound tag)
    {
        
    }

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        int index = tasks.FindIndex(genPass => genPass.Name.Equals("Micro Biomes"));
        //tasks.Insert(index + 1, new PassLegacy("正在生成宝石神龛", GemShrine.GenerateGemShrine));
    }
}