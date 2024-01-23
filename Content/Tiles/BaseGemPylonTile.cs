using CTGMod.Content.Items;
using CTGMod.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.ObjectData;
using Terraria.GameContent;
using Terraria.Map;

namespace CTGMod.Content.Tiles;

public abstract class BaseGemPylonTile<TItem, TPylon> : ModPylon
    where TItem : BaseGemPylon
    where TPylon : TEModdedPylon
{
    protected BaseGemPylonTile(int gemID)
    {
        ThisGemID = gemID;
    }

    public readonly int ThisGemID;

    public const int CrystalVerticalFrameCount = 8;

    public Asset<Texture2D> CrystalTexture;
    public Asset<Texture2D> CrystalHighlightTexture;
    public Asset<Texture2D> MapIcon;

    public override string Texture => $"Terraria/Images/Tiles_{TileID.MasterTrophyBase}";

    public override void Load()
    {
        CrystalTexture = ModContent.Request<Texture2D>($"CTGMod/Content/Tiles/{Name}_Crystal");
        CrystalHighlightTexture = ModContent.Request<Texture2D>("CTGMod/Content/Tiles/PylonTile_CrystalHighlight");
        MapIcon = ModContent.Request<Texture2D>($"CTGMod/Assets/PylonMapIcons/Crystal_{ThisGemID}");
    }

    public override void SetStaticDefaults()
    {
        Main.tileLighted[Type] = true;
        Main.tileFrameImportant[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4);
        TileObjectData.newTile.LavaDeath = false;
        TileObjectData.newTile.DrawYOffset = 2;
        TileObjectData.newTile.StyleHorizontal = true;
        // These definitions allow for vanilla's pylon TileEntities to be placed.
        // tModLoader has a built in Tile Entity specifically for modded pylons, which we must extend (see SimplePylonTileEntity)
        TEModdedPylon moddedPylon = ModContent.GetInstance<TPylon>();
        TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(moddedPylon.PlacementPreviewHook_CheckIfCanPlace, 1, 0, true);
        TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(moddedPylon.Hook_AfterPlacement, -1, 0, false);

        TileObjectData.addTile(Type);

        TileID.Sets.InteractibleByNPCs[Type] = true;
        TileID.Sets.PreventsSandfall[Type] = true;
        TileID.Sets.AvoidedByMeteorLanding[Type] = true;

        // Adds functionality for proximity of pylons; if this is true, then being near this tile will count as being near a pylon for the teleportation process.
        AddToArray(ref TileID.Sets.CountsAsPylon);

        LocalizedText pylonName = CreateMapEntryName(); //Name is in the localization file
        AddMapEntry(Color.White, pylonName);
    }
    public override NPCShop.Entry GetNPCShopEntry() => null;

    public override void MouseOver(int i, int j)
    {
        // Show a little pylon icon on the mouse indicating we are hovering over it.
        Main.LocalPlayer.cursorItemIconEnabled = true;
        Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<TItem>();
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        // We need to clean up after ourselves, since this is still a "unique" tile, separate from Vanilla Pylons, so we must kill the TileEntity.
        ModContent.GetInstance<TPylon>().Kill(i, j);
    }

    public override bool ValidTeleportCheck_NPCCount(TeleportPylonInfo pylonInfo, int defaultNecessaryNPCCount) => true; // always available

    public override bool ValidTeleportCheck_BiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData) => true; // always available

    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        // Pylons in vanilla light up, which is just a simple functionality we add using ModTile's ModifyLight.
        // Let's just add a simple white light for our pylon:
        r = g = b = 0.75f;
    }

    public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
    {
        // We want to draw the pylon crystal the exact same way vanilla does, so we can use this built in method in ModPylon for default crystal drawing:
        // For the sake of example, lets make our pylon create a bit more dust by decreasing the dustConsequent value down to 1. If you want your dust spawning to be identical to vanilla, set dustConsequent to 4.
        // We also multiply the pylonShadowColor in order to decrease its opacity, so it actually looks like a "shadow"
        DefaultDrawPylonCrystal(spriteBatch, i, j, CrystalTexture, CrystalHighlightTexture, new Vector2(-1f, -12f), Color.White * 0.05f, Color.White, 4, CrystalVerticalFrameCount);
    }

    public override void DrawMapIcon(ref MapOverlayDrawContext context, ref string mouseOverText, TeleportPylonInfo pylonInfo, bool isNearPylon, Color drawColor, float deselectedScale, float selectedScale)
    {
        bool mouseOver = DefaultDrawMapIcon(ref context, MapIcon, pylonInfo.PositionInTiles.ToVector2() + new Vector2(1.5f, 2f), drawColor, deselectedScale, selectedScale);
        DefaultMapClickHandle(mouseOver, pylonInfo, ModContent.GetInstance<TItem>().DisplayName.Key, ref mouseOverText);
    }
}