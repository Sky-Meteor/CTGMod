using CTGMod.Content.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTGMod.Content.Items;

public abstract class BaseGemPylon : ModItem
{
    public abstract int PylonTile { get; }

    public override string Texture => $"Terraria/Images/Item_{ItemID.TeleportationPylonVictory}";

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(PylonTile);
        Item.rare = ItemRarityID.Blue;
    }
}

public class AmberPylon : BaseGemPylon
{
    public override int PylonTile => ModContent.TileType<AmberPylonTile>();
}
public class AmethystPylon : BaseGemPylon
{
    public override int PylonTile => ModContent.TileType<AmethystPylonTile>();
}
public class DiamondPylon : BaseGemPylon
{
    public override int PylonTile => ModContent.TileType<DiamondPylonTile>();
}
public class EmeraldPylon : BaseGemPylon
{
    public override int PylonTile => ModContent.TileType<EmeraldPylonTile>();
}
public class RubyPylon : BaseGemPylon
{
    public override int PylonTile => ModContent.TileType<RubyPylonTile>();
}
public class SapphirePylon : BaseGemPylon
{
    public override int PylonTile => ModContent.TileType<SapphirePylonTile>();
}
public class TopazPylon : BaseGemPylon
{
    public override int PylonTile => ModContent.TileType<TopazPylonTile>();
}