using System.Collections.Generic;
using CTGMod.Common.WorldGeneration;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace CTGMod.Common.Systems;

public class WorldGenSystem : ModSystem
{
    public static Dictionary<string, Rectangle> Shrines = new();

    public override void SaveWorldData(TagCompound tag)
    {
        int count = 0;
        foreach (var item in Shrines)
        {
            tag.Add($"CTGShrines{count}Key", item.Key);
            tag.Add($"CTGShrines{count}Value", item.Value);
            count++;
        }
    }

    public override void LoadWorldData(TagCompound tag)
    {
        Shrines.Clear();
        for (int i = 0; i < GemShrine.VanillaGems.Count; i++)
        {
            var key = tag.Get<string>($"CTGShrines{i}Key");
            var value = tag.Get<Rectangle>($"CTGShrines{i}Value");
            if (!string.IsNullOrEmpty(key))
                Shrines.Add(key, value);
        }
    }

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        int index = tasks.FindIndex(genPass => genPass.Name.Equals("Micro Biomes"));
        tasks.Insert(index + 1, new PassLegacy("正在生成宝石神龛", GemShrine.GenerateGemShrine));
    }

    public override void Load()
    {
        Shrines = new Dictionary<string, Rectangle>();
    }

    public override void Unload()
    {
        Shrines = null;
    }
}