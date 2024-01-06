using System.Collections.Generic;
using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace CTGMod.Common.Systems;

public class GrapplingHookAIPatch : ModSystem
{
    public override void Load()
    {
        On_Projectile.AI_007_GrapplingHooks += Projectile_AI_007_GrapplingHooks;
        new Hook(typeof(ProjectileLoader).GetMethod("GrappleOutOfRange", BindingFlags.Static | BindingFlags.Public), GrappleOutOfRange).Apply();
    }

    private void Projectile_AI_007_GrapplingHooks(On_Projectile.orig_AI_007_GrapplingHooks orig, Projectile self)
    {
        if (self.aiStyle != ProjAIStyleID.Hook || Main.player[self.owner] == null || !Main.player[self.owner].GetModPlayer<CTGEffectPlayer>().StrongHook)
        {
            orig.Invoke(self);
            return;
        }
        #region ModifiedMethod
        if (Main.player[self.owner].dead || Main.player[self.owner].stoned || Main.player[self.owner].webbed || Main.player[self.owner].frozen)
        {
            self.Kill();
            return;
        }
        Vector2 mountedCenter = Main.player[self.owner].MountedCenter;
        Vector2 vector = new Vector2(self.position.X + self.width * 0.5f, self.position.Y + self.height * 0.5f);
        float num = mountedCenter.X - vector.X;
        float num12 = mountedCenter.Y - vector.Y;
        float num13 = (float)Math.Sqrt(num * num + num12 * num12);
        self.rotation = (float)Math.Atan2(num12, num) - 1.57f;
        if (self.ai[0] == 2f && self.type == 865)
        {
            float num14 = (float)Math.PI / 2f;
            int num15 = (int)Math.Round(self.rotation / num14);
            self.rotation = num15 * num14;
        }
        if (Main.myPlayer == self.owner)
        {
            int num16 = (int)(self.Center.X / 16f);
            int num17 = (int)(self.Center.Y / 16f);
            if (num16 > 0 && num17 > 0 && num16 < Main.maxTilesX && num17 < Main.maxTilesY && Main.tile[num16, num17].HasUnactuatedTile && TileID.Sets.CrackedBricks[Main.tile[num16, num17].TileType] && Main.rand.NextBool(16))
            {
                WorldGen.KillTile(num16, num17);
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 20, num16, num17);
                }
            }
        }
        if (num13 > 2500f)
        {
            self.Kill();
        }
        if (self.type == 256)
        {
            self.rotation = (float)Math.Atan2(num12, num) + 3.9250002f;
        }
        if (self.type == 446)
        {
            Lighting.AddLight(mountedCenter, 0f, 0.4f, 0.3f);
            self.localAI[0] += 1f;
            if (self.localAI[0] >= 28f)
            {
                self.localAI[0] = 0f;
            }
            DelegateMethods.v3_1 = new Vector3(0f, 0.4f, 0.3f);
            Terraria.Utils.PlotTileLine(self.Center, mountedCenter, 8f, DelegateMethods.CastLightOpen);
        }
        if (self.type == 652 && ++self.frameCounter >= 7)
        {
            self.frameCounter = 0;
            if (++self.frame >= Main.projFrames[self.type])
            {
                self.frame = 0;
            }
        }
        if (self.type >= 646 && self.type <= 649)
        {
            Vector3 vector2 = Vector3.Zero;
            switch (self.type)
            {
                case 646:
                    vector2 = new Vector3(0.7f, 0.5f, 0.1f);
                    break;
                case 647:
                    vector2 = new Vector3(0f, 0.6f, 0.7f);
                    break;
                case 648:
                    vector2 = new Vector3(0.6f, 0.2f, 0.6f);
                    break;
                case 649:
                    vector2 = new Vector3(0.6f, 0.6f, 0.9f);
                    break;
            }
            Lighting.AddLight(mountedCenter, vector2);
            Lighting.AddLight(self.Center, vector2);
            DelegateMethods.v3_1 = vector2;
            Terraria.Utils.PlotTileLine(self.Center, mountedCenter, 8f, DelegateMethods.CastLightOpen);
        }
        if (self.ai[0] == 0f)
        {
            // hook length
            // AntiGravityHook, StaticHook and SquirrelHook change is not actually applied due to special AI
            if ((num13 > 300f * 1.25f && self.type == 13) || (num13 > 400f * 1.25f && self.type == 32) ||
                (num13 > 440f * 1.25f && self.type == 73) || (num13 > 440f * 1.25f && self.type == 74) ||
                (num13 > 375f * 1.25f && self.type == 165) || (num13 > 350f * 1.25f && self.type == 256) ||
                (num13 > 500f * 1.25f && self.type == 315) || (num13 > 550f * 1.25f && self.type == 322) ||
                (num13 > 400f * 1.25f && self.type == 331) || (num13 > 550f * 1.25f && self.type == 332) ||
                (num13 > 400f * 1.25f && self.type == 372) || (num13 > 300f * 1.25f && self.type == 396) ||
                (num13 > 550f * 1.25f && self.type >= 646 && self.type <= 649) || (num13 > 600f * 1.25f && self.type == ProjectileID.StaticHook) ||
                (num13 > 300f * 1.25f && self.type == ProjectileID.SquirrelHook) || (num13 > 500f * 1.25f && self.type == 935) ||
                (num13 > 480f * 1.25f && self.type >= 486 && self.type <= 489) || (num13 > 500f * 1.25f && self.type == ProjectileID.AntiGravityHook))
            {
                self.ai[0] = 1f;
            }
            else if (self.type >= 230 && self.type <= 235)
            {
                int num18 = 300 + (self.type - 230) * 30;
                if (num13 > num18 * 1.25f)
                {
                    self.ai[0] = 1f;
                }
            }
            else if (self.type == 753)
            {
                int num19 = 420;
                if (num13 > num19 * 1.25f)
                {
                    self.ai[0] = 1f;
                }
            }
            else if (ProjectileLoader.GrappleOutOfRange(num13, self)) // modified below
            {
                self.ai[0] = 1f;
            }

            Vector2 vector3 = self.Center - new Vector2(5f);
            Vector2 vector5 = self.Center + new Vector2(5f);
            Point point = (vector3 - new Vector2(16f)).ToTileCoordinates();
            Point point4 = (vector5 + new Vector2(32f)).ToTileCoordinates();
            int num2 = point.X;
            int num3 = point4.X;
            int num4 = point.Y;
            int num5 = point4.Y;
            if (num2 < 0)
            {
                num2 = 0;
            }
            if (num3 > Main.maxTilesX)
            {
                num3 = Main.maxTilesX;
            }
            if (num4 < 0)
            {
                num4 = 0;
            }
            if (num5 > Main.maxTilesY)
            {
                num5 = Main.maxTilesY;
            }
            Player player = Main.player[self.owner];
            List<Point> list = new List<Point>();
            for (int i = 0; i < player.grapCount; i++)
            {
                Projectile projectile = Main.projectile[player.grappling[i]];
                if (projectile.aiStyle != 7 || projectile.ai[0] != 2f)
                {
                    continue;
                }
                Point pt = projectile.Center.ToTileCoordinates();
                Tile tileSafely = Framing.GetTileSafely(pt);
                if (tileSafely.TileType != 314 && !TileID.Sets.Platforms[tileSafely.TileType])
                {
                    continue;
                }
                for (int j = -2; j <= 2; j++)
                {
                    for (int k = -2; k <= 2; k++)
                    {
                        Point point2 = new Point(pt.X + j, pt.Y + k);
                        Tile tileSafely2 = Framing.GetTileSafely(point2);
                        if (tileSafely2.TileType == 314 || TileID.Sets.Platforms[tileSafely2.TileType])
                        {
                            list.Add(point2);
                        }
                    }
                }
            }
            Vector2 vector4 = default;
            for (int l = num2; l < num3; l++)
            {
                for (int m = num4; m < num5; m++)
                {
                    /*if (Main.tile[l, m] == null)
                    {
                        Main.tile[l, m] = default;
                    }*/
                    vector4.X = l * 16;
                    vector4.Y = m * 16;
                    if (!(vector3.X + 10f > vector4.X) || !(vector3.X < vector4.X + 16f) || !(vector3.Y + 10f > vector4.Y) || !(vector3.Y < vector4.Y + 16f))
                    {
                        continue;
                    }
                    Tile tile = Main.tile[l, m];
                    if (!(bool)typeof(Projectile).GetMethod("AI_007_GrapplingHooks_CanTileBeLatchedOnTo", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(self, new object[] { l, m }) 
                        || list.Contains(new Point(l, m)) || (self.type == 403 && tile.TileType != 314) || Main.player[self.owner].IsBlacklistedForGrappling(new Point(l, m)))
                    {
                        continue;
                    }
                    if (Main.player[self.owner].grapCount < 10)
                    {
                        Main.player[self.owner].grappling[Main.player[self.owner].grapCount] = self.whoAmI;
                        Main.player[self.owner].grapCount++;
                    }
                    if (Main.myPlayer != self.owner)
                    {
                        continue;
                    }
                    int num6 = 0;
                    int num7 = -1;
                    int num8 = 100000;
                    if (self.type == 73 || self.type == 74)
                    {
                        for (int n = 0; n < 1000; n++)
                        {
                            if (n != self.whoAmI && Main.projectile[n].active && Main.projectile[n].owner == self.owner && Main.projectile[n].aiStyle == 7 && Main.projectile[n].ai[0] == 2f)
                            {
                                Main.projectile[n].Kill();
                            }
                        }
                    }
                    else
                    {
                        int num9 = 3;
                        if (self.type == 165)
                        {
                            num9 = 8;
                        }
                        if (self.type == 256)
                        {
                            num9 = 2;
                        }
                        if (self.type == 372)
                        {
                            num9 = 2;
                        }
                        if (self.type == 652)
                        {
                            num9 = 1;
                        }
                        if (self.type >= 646 && self.type <= 649)
                        {
                            num9 = 4;
                        }
                        ProjectileLoader.NumGrappleHooks(self, Main.player[self.owner], ref num9);
                        for (int num10 = 0; num10 < 1000; num10++)
                        {
                            if (Main.projectile[num10].active && Main.projectile[num10].owner == self.owner && Main.projectile[num10].aiStyle == 7)
                            {
                                if (Main.projectile[num10].timeLeft < num8)
                                {
                                    num7 = num10;
                                    num8 = Main.projectile[num10].timeLeft;
                                }
                                num6++;
                            }
                        }
                        if (num6 > num9)
                        {
                            Main.projectile[num7].Kill();
                        }
                    }
                    WorldGen.KillTile(l, m, fail: true, effectOnly: true);
                    SoundEngine.PlaySound(SoundID.Dig, new Vector2(l * 16, m * 16));
                    self.velocity.X = 0f;
                    self.velocity.Y = 0f;
                    self.ai[0] = 2f;
                    self.position.X = l * 16 + 8 - self.width / 2;
                    self.position.Y = m * 16 + 8 - self.height / 2;
                    Rectangle? tileVisualHitbox = WorldGen.GetTileVisualHitbox(l, m);
                    if (tileVisualHitbox.HasValue)
                    {
                        self.Center = tileVisualHitbox.Value.Center.ToVector2();
                    }
                    self.damage = 0;
                    self.netUpdate = true;
                    if (Main.myPlayer == self.owner)
                    {
                        if (self.type == 935)
                        {
                            Main.player[self.owner].DoQueenSlimeHookTeleport(self.Center);
                        }
                        NetMessage.SendData(13, -1, -1, null, self.owner);
                    }
                    break;
                }
                if (self.ai[0] == 2f)
                {
                    break;
                }
            }
        }
        else if (self.ai[0] == 1f)
        {
            float num11 = 11f;
            if (self.type == 32)
            {
                num11 = 15f;
            }
            if (self.type == 73 || self.type == 74)
            {
                num11 = 17f;
            }
            if (self.type == 315)
            {
                num11 = 20f;
            }
            if (self.type == 322)
            {
                num11 = 22f;
            }
            if (self.type >= 230 && self.type <= 235)
            {
                num11 = 11f + (self.type - 230) * 0.75f;
            }
            if (self.type == 753)
            {
                num11 = 15f;
            }
            if (self.type == 446)
            {
                num11 = 20f;
            }
            if (self.type >= 486 && self.type <= 489)
            {
                num11 = 18f;
            }
            if (self.type >= 646 && self.type <= 649)
            {
                num11 = 24f;
            }
            if (self.type == 652)
            {
                num11 = 24f;
            }
            if (self.type == 332)
            {
                num11 = 17f;
            }
            ProjectileLoader.GrappleRetreatSpeed(self, Main.player[self.owner], ref num11);
            if (num13 < 24f)
            {
                self.Kill();
            }
            num13 = num11 / num13;
            num *= num13;
            num12 *= num13;
            self.velocity.X = num;
            self.velocity.Y = num12;
        }
        else if (self.ai[0] == 2f)
        {
            Point point3 = self.Center.ToTileCoordinates();
            /*if (Main.tile[point3.X, point3.Y] == null)
            {
                Main.tile[point3.X, point3.Y] = default;
            }*/
            bool flag = !(bool)typeof(Projectile).GetMethod("AI_007_GrapplingHooks_CanTileBeLatchedOnTo", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(self, new object[] { point3.X, point3.Y });
            if (flag)
            {
                self.ai[0] = 1f;
            }
            else if (Main.player[self.owner].grapCount < 10)
            {
                Main.player[self.owner].grappling[Main.player[self.owner].grapCount] = self.whoAmI;
                Main.player[self.owner].grapCount++;
            }
        }
        #endregion
    }

    private delegate bool GrappleOutOfRangeDelegate(float distance, Projectile projectile);
    private static bool GrappleOutOfRange(GrappleOutOfRangeDelegate orig, float distance, Projectile projectile)
    {
        if (projectile.aiStyle != ProjAIStyleID.Hook || Main.player[projectile.owner] == null || !Main.player[projectile.owner].GetModPlayer<CTGEffectPlayer>().StrongHook)
        {
            return orig.Invoke(distance, projectile);
        }
        return distance > projectile.ModProjectile?.GrappleRange() * 1.25f;
    }
}