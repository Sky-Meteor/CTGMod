using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using CTGMod.Common.Configs;
using CTGMod.Common.ModPlayers;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CTGMod.Drawing;

public class DrawPlayerCTGPatch : PlayerDrawLayer
{
    public override bool IsLoadingEnabled(Mod mod) => CTGConfig.Instance.DrawExtraGems;// waiting for special gems
    
    public override Position GetDefaultPosition()
    {
        return new AfterParent(PlayerDrawLayers.CaptureTheGem);
    }

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        if (drawInfo.shadow != 0.0 || (byte)drawInfo.drawPlayer.ownedLargeGems <= 0)
            return;

        Player player = drawInfo.drawPlayer;

        BitsByte ownedLargeGems = player.ownedLargeGems;
        float num1 = 0.0f;
        for (int key = 0; key < 7; ++key)
        {
            if (ownedLargeGems[key])
                ++num1;
        }
        float num2 = (float)(1.0 - num1 * 0.059999998658895493);
        float num3 = (float)((num1 /* - 1.0 */) * 4.0);
        if (num1 > 1)
        {
            num3 += 14f - num1 * 2f;
        }

        float num4 = -(float)(player.miscCounter / 300.0 * 6.2831854820251465);
        if (num1 <= 0.0)
            return;
        float num5 = 6.28318548f / num1;
        float num6 = 0.0f;

        List<DrawData> ctgDrawDataBefore = new List<DrawData>();
        List<DrawData> ctgDrawDataAfter = new List<DrawData>();

        Vector2 drawPosition = new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X + drawInfo.drawPlayer.width / 2f),
            (int)(drawInfo.Position.Y - Main.screenPosition.Y + drawInfo.drawPlayer.height - 80.0));
        Vector2 extraYOffset = new Vector2(0f, 20f + num1 * 5f);
        
        for (int key = 0; key < 7; ++key)
        {
            if (!ownedLargeGems[key])
            {
                ++num6;
            }
            else
            {
                Vector2 rotationVector2 = (num4 + num5 * (key - num6)).ToRotationVector2();
                Vector2 offsetVector2 = rotationVector2 * Vector2.One * num3 * new Vector2(2.5f, .25f);
                
                if (num1 == 1f)
                    offsetVector2 *= new Vector2(2.5f, 1f);
                float scaleExtra = 1f + rotationVector2.Y * .1f;// larger when closer
                
                Texture2D texture2D = TextureAssets.Gem[key].Value;
                DrawData drawData = new DrawData(texture2D, drawPosition + offsetVector2, new Rectangle?(),
                    new Color(250, 250, 250, Main.mouseTextColor / 2), 0.0f, texture2D.Size() / 2f,
                    (float)(Main.mouseTextColor / 1000.0 + 0.800000011920929) * num2 * .8f * scaleExtra, SpriteEffects.None);
                ctgDrawDataBefore.Add(drawData);
            }
        }

        int ctgDrawDataCount = ctgDrawDataBefore.Count;
        
        int vanillaGemCount = 0;
        for (int i = 0; i < 7; i++)
        {
            if (drawInfo.drawPlayer.ownedLargeGems[i])
                vanillaGemCount++;
        }
        var vanillaDrawData = drawInfo.DrawDataCache.GetRange(drawInfo.DrawDataCache.Count - vanillaGemCount, vanillaGemCount);
        drawInfo.DrawDataCache.RemoveRange(drawInfo.DrawDataCache.Count - vanillaGemCount, vanillaGemCount);
        vanillaDrawData = vanillaDrawData.Select(data =>
        {
            data.scale *= .8f;
            data.position -= extraYOffset;
            return data;
        }).ToList();

        if (ctgDrawDataCount > 1)
        {
            ctgDrawDataBefore.Sort((x, y) => x.position.Y.CompareTo(y.position.Y));
            ctgDrawDataAfter = ctgDrawDataBefore.GetRange(ctgDrawDataCount / 2, ctgDrawDataCount - ctgDrawDataCount / 2);
            ctgDrawDataBefore.RemoveRange(ctgDrawDataCount / 2, ctgDrawDataCount - ctgDrawDataCount / 2);
        }
        else // 1 ctg gem
        {
            if (ctgDrawDataBefore[0].position.Y > (int)(drawInfo.Position.Y - Main.screenPosition.Y + drawInfo.drawPlayer.height - 80.0))
            {
                ctgDrawDataAfter.Add(ctgDrawDataBefore[0]);
                ctgDrawDataBefore.Clear();
            }
        }

        ctgDrawDataBefore = ctgDrawDataBefore.Select(data =>
        {
            data.position = data.position.RotatedBy(player.GetModPlayer<CTGPlayer>().DrawCounter / 1800f * MathHelper.TwoPi, drawPosition) - extraYOffset;
            return data;
        }).ToList();
        ctgDrawDataAfter = ctgDrawDataAfter.Select(data =>
        {
            data.position = data.position.RotatedBy(player.GetModPlayer<CTGPlayer>().DrawCounter / 1800f * MathHelper.TwoPi, drawPosition) - extraYOffset;
            return data;
        }).ToList();
        

        drawInfo.DrawDataCache.AddRange(ctgDrawDataBefore); // back layer
        drawInfo.DrawDataCache.AddRange(vanillaDrawData);
        drawInfo.DrawDataCache.AddRange(ctgDrawDataAfter); // front layer
    }
}