// Decompiled with JetBrains decompiler
// Type: Terraria.NetMessage
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Ionic.Zlib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.IO;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Net.Sockets;
using Terraria.Social;

namespace Terraria
{
  public class NetMessage
  {
    public static MessageBuffer[] buffer = new MessageBuffer[257];
    private static PlayerDeathReason _currentPlayerDeathReason;
    private static NetMessage.NetSoundInfo _currentNetSoundInfo;
    private static CoinLossRevengeSystem.RevengeMarker _currentRevengeMarker;

    public static bool TrySendData(
      int msgType,
      int remoteClient = -1,
      int ignoreClient = -1,
      NetworkText text = null,
      int number = 0,
      float number2 = 0.0f,
      float number3 = 0.0f,
      float number4 = 0.0f,
      int number5 = 0,
      int number6 = 0,
      int number7 = 0)
    {
      try
      {
        NetMessage.SendData(msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7);
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    public static void SendData(
      int msgType,
      int remoteClient = -1,
      int ignoreClient = -1,
      NetworkText text = null,
      int number = 0,
      float number2 = 0.0f,
      float number3 = 0.0f,
      float number4 = 0.0f,
      int number5 = 0,
      int number6 = 0,
      int number7 = 0)
    {
      if (Main.netMode == 0)
        return;
      int index1 = 256;
      if (text == null)
        text = NetworkText.Empty;
      if (Main.netMode == 2 && remoteClient >= 0)
        index1 = remoteClient;
      lock (NetMessage.buffer[index1])
      {
        BinaryWriter writer = NetMessage.buffer[index1].writer;
        if (writer == null)
        {
          NetMessage.buffer[index1].ResetWriter();
          writer = NetMessage.buffer[index1].writer;
        }
        writer.BaseStream.Position = 0L;
        long position1 = writer.BaseStream.Position;
        writer.BaseStream.Position += 2L;
        writer.Write((byte) msgType);
        switch (msgType)
        {
          case 1:
            writer.Write("Terraria" + 248.ToString());
            break;
          case 2:
            text.Serialize(writer);
            if (Main.dedServ)
            {
              Console.WriteLine(Language.GetTextValue("CLI.ClientWasBooted", (object) Netplay.Clients[index1].Socket.GetRemoteAddress().ToString(), (object) text));
              break;
            }
            break;
          case 3:
            writer.Write((byte) remoteClient);
            writer.Write(false);
            break;
          case 4:
            Player player1 = Main.player[number];
            writer.Write((byte) number);
            writer.Write((byte) player1.skinVariant);
            writer.Write((byte) player1.hair);
            writer.Write(player1.name);
            writer.Write(player1.hairDye);
            BitsByte bitsByte1 = (BitsByte) (byte) 0;
            for (int key = 0; key < 8; ++key)
              bitsByte1[key] = player1.hideVisibleAccessory[key];
            writer.Write((byte) bitsByte1);
            BitsByte bitsByte2 = (BitsByte) (byte) 0;
            for (int key = 0; key < 2; ++key)
              bitsByte2[key] = player1.hideVisibleAccessory[key + 8];
            writer.Write((byte) bitsByte2);
            writer.Write((byte) player1.hideMisc);
            writer.WriteRGB(player1.hairColor);
            writer.WriteRGB(player1.skinColor);
            writer.WriteRGB(player1.eyeColor);
            writer.WriteRGB(player1.shirtColor);
            writer.WriteRGB(player1.underShirtColor);
            writer.WriteRGB(player1.pantsColor);
            writer.WriteRGB(player1.shoeColor);
            BitsByte bitsByte3 = (BitsByte) (byte) 0;
            if (player1.difficulty == (byte) 1)
              bitsByte3[0] = true;
            else if (player1.difficulty == (byte) 2)
              bitsByte3[1] = true;
            else if (player1.difficulty == (byte) 3)
              bitsByte3[3] = true;
            bitsByte3[2] = player1.extraAccessory;
            writer.Write((byte) bitsByte3);
            BitsByte bitsByte4 = (BitsByte) (byte) 0;
            bitsByte4[0] = player1.UsingBiomeTorches;
            bitsByte4[1] = player1.happyFunTorchTime;
            bitsByte4[2] = player1.unlockedBiomeTorches;
            writer.Write((byte) bitsByte4);
            break;
          case 5:
            writer.Write((byte) number);
            writer.Write((short) number2);
            Player player2 = Main.player[number];
            Item obj1 = (double) number2 < 220.0 ? ((double) number2 < 180.0 ? ((double) number2 < 179.0 ? ((double) number2 < 139.0 ? ((double) number2 < 99.0 ? ((double) number2 < 94.0 ? ((double) number2 < 89.0 ? ((double) number2 < 79.0 ? ((double) number2 < 59.0 ? player2.inventory[(int) number2] : player2.armor[(int) number2 - 59]) : player2.dye[(int) number2 - 79]) : player2.miscEquips[(int) number2 - 89]) : player2.miscDyes[(int) number2 - 94]) : player2.bank.item[(int) number2 - 99]) : player2.bank2.item[(int) number2 - 139]) : player2.trashItem) : player2.bank3.item[(int) number2 - 180]) : player2.bank4.item[(int) number2 - 220];
            if (obj1.Name == "" || obj1.stack == 0 || obj1.type == 0)
              obj1.SetDefaults(0, true);
            int num1 = obj1.stack;
            int netId1 = obj1.netID;
            if (num1 < 0)
              num1 = 0;
            writer.Write((short) num1);
            writer.Write((byte) number3);
            writer.Write((short) netId1);
            break;
          case 7:
            writer.Write((int) Main.time);
            BitsByte bitsByte5 = (BitsByte) (byte) 0;
            bitsByte5[0] = Main.dayTime;
            bitsByte5[1] = Main.bloodMoon;
            bitsByte5[2] = Main.eclipse;
            writer.Write((byte) bitsByte5);
            writer.Write((byte) Main.moonPhase);
            writer.Write((short) Main.maxTilesX);
            writer.Write((short) Main.maxTilesY);
            writer.Write((short) Main.spawnTileX);
            writer.Write((short) Main.spawnTileY);
            writer.Write((short) Main.worldSurface);
            writer.Write((short) Main.rockLayer);
            writer.Write(Main.worldID);
            writer.Write(Main.worldName);
            writer.Write((byte) Main.GameMode);
            writer.Write(Main.ActiveWorldFileData.UniqueId.ToByteArray());
            writer.Write(Main.ActiveWorldFileData.WorldGeneratorVersion);
            writer.Write((byte) Main.moonType);
            writer.Write((byte) WorldGen.treeBG1);
            writer.Write((byte) WorldGen.treeBG2);
            writer.Write((byte) WorldGen.treeBG3);
            writer.Write((byte) WorldGen.treeBG4);
            writer.Write((byte) WorldGen.corruptBG);
            writer.Write((byte) WorldGen.jungleBG);
            writer.Write((byte) WorldGen.snowBG);
            writer.Write((byte) WorldGen.hallowBG);
            writer.Write((byte) WorldGen.crimsonBG);
            writer.Write((byte) WorldGen.desertBG);
            writer.Write((byte) WorldGen.oceanBG);
            writer.Write((byte) WorldGen.mushroomBG);
            writer.Write((byte) WorldGen.underworldBG);
            writer.Write((byte) Main.iceBackStyle);
            writer.Write((byte) Main.jungleBackStyle);
            writer.Write((byte) Main.hellBackStyle);
            writer.Write(Main.windSpeedTarget);
            writer.Write((byte) Main.numClouds);
            for (int index2 = 0; index2 < 3; ++index2)
              writer.Write(Main.treeX[index2]);
            for (int index3 = 0; index3 < 4; ++index3)
              writer.Write((byte) Main.treeStyle[index3]);
            for (int index4 = 0; index4 < 3; ++index4)
              writer.Write(Main.caveBackX[index4]);
            for (int index5 = 0; index5 < 4; ++index5)
              writer.Write((byte) Main.caveBackStyle[index5]);
            WorldGen.TreeTops.SyncSend(writer);
            if (!Main.raining)
              Main.maxRaining = 0.0f;
            writer.Write(Main.maxRaining);
            BitsByte bitsByte6 = (BitsByte) (byte) 0;
            bitsByte6[0] = WorldGen.shadowOrbSmashed;
            bitsByte6[1] = NPC.downedBoss1;
            bitsByte6[2] = NPC.downedBoss2;
            bitsByte6[3] = NPC.downedBoss3;
            bitsByte6[4] = Main.hardMode;
            bitsByte6[5] = NPC.downedClown;
            bitsByte6[7] = NPC.downedPlantBoss;
            writer.Write((byte) bitsByte6);
            BitsByte bitsByte7 = (BitsByte) (byte) 0;
            bitsByte7[0] = NPC.downedMechBoss1;
            bitsByte7[1] = NPC.downedMechBoss2;
            bitsByte7[2] = NPC.downedMechBoss3;
            bitsByte7[3] = NPC.downedMechBossAny;
            bitsByte7[4] = (double) Main.cloudBGActive >= 1.0;
            bitsByte7[5] = WorldGen.crimson;
            bitsByte7[6] = Main.pumpkinMoon;
            bitsByte7[7] = Main.snowMoon;
            writer.Write((byte) bitsByte7);
            BitsByte bitsByte8 = (BitsByte) (byte) 0;
            bitsByte8[1] = Main.fastForwardTime;
            bitsByte8[2] = Main.slimeRain;
            bitsByte8[3] = NPC.downedSlimeKing;
            bitsByte8[4] = NPC.downedQueenBee;
            bitsByte8[5] = NPC.downedFishron;
            bitsByte8[6] = NPC.downedMartians;
            bitsByte8[7] = NPC.downedAncientCultist;
            writer.Write((byte) bitsByte8);
            BitsByte bitsByte9 = (BitsByte) (byte) 0;
            bitsByte9[0] = NPC.downedMoonlord;
            bitsByte9[1] = NPC.downedHalloweenKing;
            bitsByte9[2] = NPC.downedHalloweenTree;
            bitsByte9[3] = NPC.downedChristmasIceQueen;
            bitsByte9[4] = NPC.downedChristmasSantank;
            bitsByte9[5] = NPC.downedChristmasTree;
            bitsByte9[6] = NPC.downedGolemBoss;
            bitsByte9[7] = BirthdayParty.PartyIsUp;
            writer.Write((byte) bitsByte9);
            BitsByte bitsByte10 = (BitsByte) (byte) 0;
            bitsByte10[0] = NPC.downedPirates;
            bitsByte10[1] = NPC.downedFrost;
            bitsByte10[2] = NPC.downedGoblins;
            bitsByte10[3] = Sandstorm.Happening;
            bitsByte10[4] = DD2Event.Ongoing;
            bitsByte10[5] = DD2Event.DownedInvasionT1;
            bitsByte10[6] = DD2Event.DownedInvasionT2;
            bitsByte10[7] = DD2Event.DownedInvasionT3;
            writer.Write((byte) bitsByte10);
            BitsByte bitsByte11 = (BitsByte) (byte) 0;
            bitsByte11[0] = NPC.combatBookWasUsed;
            bitsByte11[1] = LanternNight.LanternsUp;
            bitsByte11[2] = NPC.downedTowerSolar;
            bitsByte11[3] = NPC.downedTowerVortex;
            bitsByte11[4] = NPC.downedTowerNebula;
            bitsByte11[5] = NPC.downedTowerStardust;
            bitsByte11[6] = Main.forceHalloweenForToday;
            bitsByte11[7] = Main.forceXMasForToday;
            writer.Write((byte) bitsByte11);
            BitsByte bitsByte12 = (BitsByte) (byte) 0;
            bitsByte12[0] = NPC.boughtCat;
            bitsByte12[1] = NPC.boughtDog;
            bitsByte12[2] = NPC.boughtBunny;
            bitsByte12[3] = NPC.freeCake;
            bitsByte12[4] = Main.drunkWorld;
            bitsByte12[5] = NPC.downedEmpressOfLight;
            bitsByte12[6] = NPC.downedQueenSlime;
            bitsByte12[7] = Main.getGoodWorld;
            writer.Write((byte) bitsByte12);
            BitsByte bitsByte13 = (BitsByte) (byte) 0;
            bitsByte13[0] = Main.tenthAnniversaryWorld;
            bitsByte13[1] = Main.dontStarveWorld;
            bitsByte13[2] = NPC.downedDeerclops;
            bitsByte13[3] = Main.notTheBeesWorld;
            writer.Write((byte) bitsByte13);
            writer.Write((short) WorldGen.SavedOreTiers.Copper);
            writer.Write((short) WorldGen.SavedOreTiers.Iron);
            writer.Write((short) WorldGen.SavedOreTiers.Silver);
            writer.Write((short) WorldGen.SavedOreTiers.Gold);
            writer.Write((short) WorldGen.SavedOreTiers.Cobalt);
            writer.Write((short) WorldGen.SavedOreTiers.Mythril);
            writer.Write((short) WorldGen.SavedOreTiers.Adamantite);
            writer.Write((sbyte) Main.invasionType);
            if (SocialAPI.Network != null)
              writer.Write(SocialAPI.Network.GetLobbyId());
            else
              writer.Write(0UL);
            writer.Write(Sandstorm.IntendedSeverity);
            break;
          case 8:
            writer.Write(number);
            writer.Write((int) number2);
            break;
          case 9:
            writer.Write(number);
            text.Serialize(writer);
            BitsByte bitsByte14 = (BitsByte) (byte) number2;
            writer.Write((byte) bitsByte14);
            break;
          case 10:
            int num2 = NetMessage.CompressTileBlock(number, (int) number2, (short) number3, (short) number4, NetMessage.buffer[index1].writeBuffer, (int) writer.BaseStream.Position);
            writer.BaseStream.Position += (long) num2;
            break;
          case 11:
            writer.Write((short) number);
            writer.Write((short) number2);
            writer.Write((short) number3);
            writer.Write((short) number4);
            break;
          case 12:
            Player player3 = Main.player[number];
            writer.Write((byte) number);
            writer.Write((short) player3.SpawnX);
            writer.Write((short) player3.SpawnY);
            writer.Write(player3.respawnTimer);
            writer.Write((byte) number2);
            break;
          case 13:
            Player player4 = Main.player[number];
            writer.Write((byte) number);
            BitsByte bitsByte15 = (BitsByte) (byte) 0;
            bitsByte15[0] = player4.controlUp;
            bitsByte15[1] = player4.controlDown;
            bitsByte15[2] = player4.controlLeft;
            bitsByte15[3] = player4.controlRight;
            bitsByte15[4] = player4.controlJump;
            bitsByte15[5] = player4.controlUseItem;
            bitsByte15[6] = player4.direction == 1;
            writer.Write((byte) bitsByte15);
            BitsByte bitsByte16 = (BitsByte) (byte) 0;
            bitsByte16[0] = player4.pulley;
            bitsByte16[1] = player4.pulley && player4.pulleyDir == (byte) 2;
            bitsByte16[2] = player4.velocity != Vector2.Zero;
            bitsByte16[3] = player4.vortexStealthActive;
            bitsByte16[4] = (double) player4.gravDir == 1.0;
            bitsByte16[5] = player4.shieldRaised;
            bitsByte16[6] = player4.ghost;
            writer.Write((byte) bitsByte16);
            BitsByte bitsByte17 = (BitsByte) (byte) 0;
            bitsByte17[0] = player4.tryKeepingHoveringUp;
            bitsByte17[1] = player4.IsVoidVaultEnabled;
            bitsByte17[2] = player4.sitting.isSitting;
            bitsByte17[3] = player4.downedDD2EventAnyDifficulty;
            bitsByte17[4] = player4.isPettingAnimal;
            bitsByte17[5] = player4.isTheAnimalBeingPetSmall;
            bitsByte17[6] = player4.PotionOfReturnOriginalUsePosition.HasValue;
            bitsByte17[7] = player4.tryKeepingHoveringDown;
            writer.Write((byte) bitsByte17);
            BitsByte bitsByte18 = (BitsByte) (byte) 0;
            bitsByte18[0] = player4.sleeping.isSleeping;
            writer.Write((byte) bitsByte18);
            writer.Write((byte) player4.selectedItem);
            writer.WriteVector2(player4.position);
            if (bitsByte16[2])
              writer.WriteVector2(player4.velocity);
            if (bitsByte17[6])
            {
              writer.WriteVector2(player4.PotionOfReturnOriginalUsePosition.Value);
              writer.WriteVector2(player4.PotionOfReturnHomePosition.Value);
              break;
            }
            break;
          case 14:
            writer.Write((byte) number);
            writer.Write((byte) number2);
            break;
          case 16:
            writer.Write((byte) number);
            writer.Write((short) Main.player[number].statLife);
            writer.Write((short) Main.player[number].statLifeMax);
            break;
          case 17:
            writer.Write((byte) number);
            writer.Write((short) number2);
            writer.Write((short) number3);
            writer.Write((short) number4);
            writer.Write((byte) number5);
            break;
          case 18:
            writer.Write(Main.dayTime ? (byte) 1 : (byte) 0);
            writer.Write((int) Main.time);
            writer.Write(Main.sunModY);
            writer.Write(Main.moonModY);
            break;
          case 19:
            writer.Write((byte) number);
            writer.Write((short) number2);
            writer.Write((short) number3);
            writer.Write((double) number4 == 1.0 ? (byte) 1 : (byte) 0);
            break;
          case 20:
            int num3 = number;
            int num4 = (int) number2;
            int num5 = (int) number3;
            if (num5 < 0)
              num5 = 0;
            int num6 = (int) number4;
            if (num6 < 0)
              num6 = 0;
            if (num3 < num5)
              num3 = num5;
            if (num3 >= Main.maxTilesX + num5)
              num3 = Main.maxTilesX - num5 - 1;
            if (num4 < num6)
              num4 = num6;
            if (num4 >= Main.maxTilesY + num6)
              num4 = Main.maxTilesY - num6 - 1;
            writer.Write((short) num3);
            writer.Write((short) num4);
            writer.Write((byte) num5);
            writer.Write((byte) num6);
            writer.Write((byte) number5);
            for (int index6 = num3; index6 < num3 + num5; ++index6)
            {
              for (int index7 = num4; index7 < num4 + num6; ++index7)
              {
                BitsByte bitsByte19 = (BitsByte) (byte) 0;
                BitsByte bitsByte20 = (BitsByte) (byte) 0;
                byte num7 = 0;
                byte num8 = 0;
                Tile tile = Main.tile[index6, index7];
                bitsByte19[0] = tile.active();
                bitsByte19[2] = tile.wall > (ushort) 0;
                bitsByte19[3] = tile.liquid > (byte) 0 && Main.netMode == 2;
                bitsByte19[4] = tile.wire();
                bitsByte19[5] = tile.halfBrick();
                bitsByte19[6] = tile.actuator();
                bitsByte19[7] = tile.inActive();
                bitsByte20[0] = tile.wire2();
                bitsByte20[1] = tile.wire3();
                if (tile.active() && tile.color() > (byte) 0)
                {
                  bitsByte20[2] = true;
                  num7 = tile.color();
                }
                if (tile.wall > (ushort) 0 && tile.wallColor() > (byte) 0)
                {
                  bitsByte20[3] = true;
                  num8 = tile.wallColor();
                }
                bitsByte20 = (BitsByte) (byte) ((uint) (byte) bitsByte20 + (uint) (byte) ((uint) tile.slope() << 4));
                bitsByte20[7] = tile.wire4();
                writer.Write((byte) bitsByte19);
                writer.Write((byte) bitsByte20);
                if (num7 > (byte) 0)
                  writer.Write(num7);
                if (num8 > (byte) 0)
                  writer.Write(num8);
                if (tile.active())
                {
                  writer.Write(tile.type);
                  if (Main.tileFrameImportant[(int) tile.type])
                  {
                    writer.Write(tile.frameX);
                    writer.Write(tile.frameY);
                  }
                }
                if (tile.wall > (ushort) 0)
                  writer.Write(tile.wall);
                if (tile.liquid > (byte) 0 && Main.netMode == 2)
                {
                  writer.Write(tile.liquid);
                  writer.Write(tile.liquidType());
                }
              }
            }
            break;
          case 21:
          case 90:
            Item obj2 = Main.item[number];
            writer.Write((short) number);
            writer.WriteVector2(obj2.position);
            writer.WriteVector2(obj2.velocity);
            writer.Write((short) obj2.stack);
            writer.Write(obj2.prefix);
            writer.Write((byte) number2);
            short num9 = 0;
            if (obj2.active && obj2.stack > 0)
              num9 = (short) obj2.netID;
            writer.Write(num9);
            break;
          case 22:
            writer.Write((short) number);
            writer.Write((byte) Main.item[number].playerIndexTheItemIsReservedFor);
            break;
          case 23:
            NPC npc1 = Main.npc[number];
            writer.Write((short) number);
            writer.WriteVector2(npc1.position);
            writer.WriteVector2(npc1.velocity);
            writer.Write((ushort) npc1.target);
            int num10 = npc1.life;
            if (!npc1.active)
              num10 = 0;
            if (!npc1.active || npc1.life <= 0)
              npc1.netSkip = 0;
            short netId2 = (short) npc1.netID;
            bool[] flagArray = new bool[4];
            BitsByte bitsByte21 = (BitsByte) (byte) 0;
            bitsByte21[0] = npc1.direction > 0;
            bitsByte21[1] = npc1.directionY > 0;
            bitsByte21[2] = flagArray[0] = (double) npc1.ai[0] != 0.0;
            bitsByte21[3] = flagArray[1] = (double) npc1.ai[1] != 0.0;
            bitsByte21[4] = flagArray[2] = (double) npc1.ai[2] != 0.0;
            bitsByte21[5] = flagArray[3] = (double) npc1.ai[3] != 0.0;
            bitsByte21[6] = npc1.spriteDirection > 0;
            bitsByte21[7] = num10 == npc1.lifeMax;
            writer.Write((byte) bitsByte21);
            BitsByte bitsByte22 = (BitsByte) (byte) 0;
            bitsByte22[0] = npc1.statsAreScaledForThisManyPlayers > 1;
            bitsByte22[1] = npc1.SpawnedFromStatue;
            bitsByte22[2] = (double) npc1.strengthMultiplier != 1.0;
            writer.Write((byte) bitsByte22);
            for (int index8 = 0; index8 < NPC.maxAI; ++index8)
            {
              if (flagArray[index8])
                writer.Write(npc1.ai[index8]);
            }
            writer.Write(netId2);
            if (bitsByte22[0])
              writer.Write((byte) npc1.statsAreScaledForThisManyPlayers);
            if (bitsByte22[2])
              writer.Write(npc1.strengthMultiplier);
            if (!bitsByte21[7])
            {
              byte num11 = 1;
              if (npc1.lifeMax > (int) short.MaxValue)
                num11 = (byte) 4;
              else if (npc1.lifeMax > (int) sbyte.MaxValue)
                num11 = (byte) 2;
              writer.Write(num11);
              switch (num11)
              {
                case 2:
                  writer.Write((short) num10);
                  break;
                case 4:
                  writer.Write(num10);
                  break;
                default:
                  writer.Write((sbyte) num10);
                  break;
              }
            }
            if (npc1.type >= 0 && npc1.type < 670 && Main.npcCatchable[npc1.type])
            {
              writer.Write((byte) npc1.releaseOwner);
              break;
            }
            break;
          case 24:
            writer.Write((short) number);
            writer.Write((byte) number2);
            break;
          case 27:
            Projectile projectile1 = Main.projectile[number];
            writer.Write((short) projectile1.identity);
            writer.WriteVector2(projectile1.position);
            writer.WriteVector2(projectile1.velocity);
            writer.Write((byte) projectile1.owner);
            writer.Write((short) projectile1.type);
            BitsByte bitsByte23 = (BitsByte) (byte) 0;
            for (int key = 0; key < Projectile.maxAI; ++key)
            {
              if ((double) projectile1.ai[key] != 0.0)
                bitsByte23[key] = true;
            }
            if (projectile1.bannerIdToRespondTo != 0)
              bitsByte23[3] = true;
            if (projectile1.damage != 0)
              bitsByte23[4] = true;
            if ((double) projectile1.knockBack != 0.0)
              bitsByte23[5] = true;
            if (projectile1.type > 0 && projectile1.type < 972 && ProjectileID.Sets.NeedsUUID[projectile1.type])
              bitsByte23[7] = true;
            if (projectile1.originalDamage != 0)
              bitsByte23[6] = true;
            writer.Write((byte) bitsByte23);
            for (int key = 0; key < Projectile.maxAI; ++key)
            {
              if (bitsByte23[key])
                writer.Write(projectile1.ai[key]);
            }
            if (bitsByte23[3])
              writer.Write((ushort) projectile1.bannerIdToRespondTo);
            if (bitsByte23[4])
              writer.Write((short) projectile1.damage);
            if (bitsByte23[5])
              writer.Write(projectile1.knockBack);
            if (bitsByte23[6])
              writer.Write((short) projectile1.originalDamage);
            if (bitsByte23[7])
            {
              writer.Write((short) projectile1.projUUID);
              break;
            }
            break;
          case 28:
            writer.Write((short) number);
            writer.Write((short) number2);
            writer.Write(number3);
            writer.Write((byte) ((double) number4 + 1.0));
            writer.Write((byte) number5);
            break;
          case 29:
            writer.Write((short) number);
            writer.Write((byte) number2);
            break;
          case 30:
            writer.Write((byte) number);
            writer.Write(Main.player[number].hostile);
            break;
          case 31:
            writer.Write((short) number);
            writer.Write((short) number2);
            break;
          case 32:
            Item obj3 = Main.chest[number].item[(int) (byte) number2];
            writer.Write((short) number);
            writer.Write((byte) number2);
            short num12 = (short) obj3.netID;
            if (obj3.Name == null)
              num12 = (short) 0;
            writer.Write((short) obj3.stack);
            writer.Write(obj3.prefix);
            writer.Write(num12);
            break;
          case 33:
            int num13 = 0;
            int num14 = 0;
            int num15 = 0;
            string str1 = (string) null;
            if (number > -1)
            {
              num13 = Main.chest[number].x;
              num14 = Main.chest[number].y;
            }
            if ((double) number2 == 1.0)
            {
              string str2 = text.ToString();
              num15 = (int) (byte) str2.Length;
              if (num15 == 0 || num15 > 20)
                num15 = (int) byte.MaxValue;
              else
                str1 = str2;
            }
            writer.Write((short) number);
            writer.Write((short) num13);
            writer.Write((short) num14);
            writer.Write((byte) num15);
            if (str1 != null)
            {
              writer.Write(str1);
              break;
            }
            break;
          case 34:
            writer.Write((byte) number);
            writer.Write((short) number2);
            writer.Write((short) number3);
            writer.Write((short) number4);
            if (Main.netMode == 2)
            {
              Netplay.GetSectionX((int) number2);
              Netplay.GetSectionY((int) number3);
              writer.Write((short) number5);
              break;
            }
            writer.Write((short) 0);
            break;
          case 35:
            writer.Write((byte) number);
            writer.Write((short) number2);
            break;
          case 36:
            Player player5 = Main.player[number];
            writer.Write((byte) number);
            writer.Write((byte) player5.zone1);
            writer.Write((byte) player5.zone2);
            writer.Write((byte) player5.zone3);
            writer.Write((byte) player5.zone4);
            break;
          case 38:
            writer.Write(Netplay.ServerPassword);
            break;
          case 39:
            writer.Write((short) number);
            break;
          case 40:
            writer.Write((byte) number);
            writer.Write((short) Main.player[number].talkNPC);
            break;
          case 41:
            writer.Write((byte) number);
            writer.Write(Main.player[number].itemRotation);
            writer.Write((short) Main.player[number].itemAnimation);
            break;
          case 42:
            writer.Write((byte) number);
            writer.Write((short) Main.player[number].statMana);
            writer.Write((short) Main.player[number].statManaMax);
            break;
          case 43:
            writer.Write((byte) number);
            writer.Write((short) number2);
            break;
          case 45:
            writer.Write((byte) number);
            writer.Write((byte) Main.player[number].team);
            break;
          case 46:
            writer.Write((short) number);
            writer.Write((short) number2);
            break;
          case 47:
            writer.Write((short) number);
            writer.Write((short) Main.sign[number].x);
            writer.Write((short) Main.sign[number].y);
            writer.Write(Main.sign[number].text);
            writer.Write((byte) number2);
            writer.Write((byte) number3);
            break;
          case 48:
            Tile tile1 = Main.tile[number, (int) number2];
            writer.Write((short) number);
            writer.Write((short) number2);
            writer.Write(tile1.liquid);
            writer.Write(tile1.liquidType());
            break;
          case 50:
            writer.Write((byte) number);
            for (int index9 = 0; index9 < 22; ++index9)
              writer.Write((ushort) Main.player[number].buffType[index9]);
            break;
          case 51:
            writer.Write((byte) number);
            writer.Write((byte) number2);
            break;
          case 52:
            writer.Write((byte) number2);
            writer.Write((short) number3);
            writer.Write((short) number4);
            break;
          case 53:
            writer.Write((short) number);
            writer.Write((ushort) number2);
            writer.Write((short) number3);
            break;
          case 54:
            writer.Write((short) number);
            for (int index10 = 0; index10 < 5; ++index10)
            {
              writer.Write((ushort) Main.npc[number].buffType[index10]);
              writer.Write((short) Main.npc[number].buffTime[index10]);
            }
            break;
          case 55:
            writer.Write((byte) number);
            writer.Write((ushort) number2);
            writer.Write((int) number3);
            break;
          case 56:
            writer.Write((short) number);
            if (Main.netMode == 2)
            {
              string givenName = Main.npc[number].GivenName;
              writer.Write(givenName);
              writer.Write(Main.npc[number].townNpcVariationIndex);
              break;
            }
            break;
          case 57:
            writer.Write(WorldGen.tGood);
            writer.Write(WorldGen.tEvil);
            writer.Write(WorldGen.tBlood);
            break;
          case 58:
            writer.Write((byte) number);
            writer.Write(number2);
            break;
          case 59:
            writer.Write((short) number);
            writer.Write((short) number2);
            break;
          case 60:
            writer.Write((short) number);
            writer.Write((short) number2);
            writer.Write((short) number3);
            writer.Write((byte) number4);
            break;
          case 61:
            writer.Write((short) number);
            writer.Write((short) number2);
            break;
          case 62:
            writer.Write((byte) number);
            writer.Write((byte) number2);
            break;
          case 63:
          case 64:
            writer.Write((short) number);
            writer.Write((short) number2);
            writer.Write((byte) number3);
            break;
          case 65:
            BitsByte bitsByte24 = (BitsByte) (byte) 0;
            bitsByte24[0] = (number & 1) == 1;
            bitsByte24[1] = (number & 2) == 2;
            bitsByte24[2] = number6 == 1;
            bitsByte24[3] = (uint) number7 > 0U;
            writer.Write((byte) bitsByte24);
            writer.Write((short) number2);
            writer.Write(number3);
            writer.Write(number4);
            writer.Write((byte) number5);
            if (bitsByte24[3])
            {
              writer.Write(number7);
              break;
            }
            break;
          case 66:
            writer.Write((byte) number);
            writer.Write((short) number2);
            break;
          case 68:
            writer.Write(Main.clientUUID);
            break;
          case 69:
            Netplay.GetSectionX((int) number2);
            Netplay.GetSectionY((int) number3);
            writer.Write((short) number);
            writer.Write((short) number2);
            writer.Write((short) number3);
            writer.Write(Main.chest[(int) (short) number].name);
            break;
          case 70:
            writer.Write((short) number);
            writer.Write((byte) number2);
            break;
          case 71:
            writer.Write(number);
            writer.Write((int) number2);
            writer.Write((short) number3);
            writer.Write((byte) number4);
            break;
          case 72:
            for (int index11 = 0; index11 < 40; ++index11)
              writer.Write((short) Main.travelShop[index11]);
            break;
          case 73:
            writer.Write((byte) number);
            break;
          case 74:
            writer.Write((byte) Main.anglerQuest);
            bool flag1 = Main.anglerWhoFinishedToday.Contains(text.ToString());
            writer.Write(flag1);
            break;
          case 76:
            writer.Write((byte) number);
            writer.Write(Main.player[number].anglerQuestsFinished);
            writer.Write(Main.player[number].golferScoreAccumulated);
            break;
          case 77:
            writer.Write((short) number);
            writer.Write((ushort) number2);
            writer.Write((short) number3);
            writer.Write((short) number4);
            break;
          case 78:
            writer.Write(number);
            writer.Write((int) number2);
            writer.Write((sbyte) number3);
            writer.Write((sbyte) number4);
            break;
          case 79:
            writer.Write((short) number);
            writer.Write((short) number2);
            writer.Write((short) number3);
            writer.Write((short) number4);
            writer.Write((byte) number5);
            writer.Write((sbyte) number6);
            writer.Write(number7 == 1);
            break;
          case 80:
            writer.Write((byte) number);
            writer.Write((short) number2);
            break;
          case 81:
            writer.Write(number2);
            writer.Write(number3);
            writer.WriteRGB(new Color()
            {
              PackedValue = (uint) number
            });
            writer.Write((int) number4);
            break;
          case 83:
            int index12 = number;
            if (index12 < 0 && index12 >= 289)
              index12 = 1;
            int num16 = NPC.killCount[index12];
            writer.Write((short) index12);
            writer.Write(num16);
            break;
          case 84:
            byte index13 = (byte) number;
            float stealth = Main.player[(int) index13].stealth;
            writer.Write(index13);
            writer.Write(stealth);
            break;
          case 85:
            byte num17 = (byte) number;
            writer.Write(num17);
            break;
          case 86:
            writer.Write(number);
            bool flag2 = TileEntity.ByID.ContainsKey(number);
            writer.Write(flag2);
            if (flag2)
            {
              TileEntity.Write(writer, TileEntity.ByID[number], true);
              break;
            }
            break;
          case 87:
            writer.Write((short) number);
            writer.Write((short) number2);
            writer.Write((byte) number3);
            break;
          case 88:
            BitsByte bitsByte25 = (BitsByte) (byte) number2;
            BitsByte bitsByte26 = (BitsByte) (byte) number3;
            writer.Write((short) number);
            writer.Write((byte) bitsByte25);
            Item obj4 = Main.item[number];
            if (bitsByte25[0])
              writer.Write(obj4.color.PackedValue);
            if (bitsByte25[1])
              writer.Write((ushort) obj4.damage);
            if (bitsByte25[2])
              writer.Write(obj4.knockBack);
            if (bitsByte25[3])
              writer.Write((ushort) obj4.useAnimation);
            if (bitsByte25[4])
              writer.Write((ushort) obj4.useTime);
            if (bitsByte25[5])
              writer.Write((short) obj4.shoot);
            if (bitsByte25[6])
              writer.Write(obj4.shootSpeed);
            if (bitsByte25[7])
            {
              writer.Write((byte) bitsByte26);
              if (bitsByte26[0])
                writer.Write((ushort) obj4.width);
              if (bitsByte26[1])
                writer.Write((ushort) obj4.height);
              if (bitsByte26[2])
                writer.Write(obj4.scale);
              if (bitsByte26[3])
                writer.Write((short) obj4.ammo);
              if (bitsByte26[4])
                writer.Write((short) obj4.useAmmo);
              if (bitsByte26[5])
              {
                writer.Write(obj4.notAmmo);
                break;
              }
              break;
            }
            break;
          case 89:
            writer.Write((short) number);
            writer.Write((short) number2);
            Item obj5 = Main.player[(int) number4].inventory[(int) number3];
            writer.Write((short) obj5.netID);
            writer.Write(obj5.prefix);
            writer.Write((short) number5);
            break;
          case 91:
            writer.Write(number);
            writer.Write((byte) number2);
            if ((double) number2 != (double) byte.MaxValue)
            {
              writer.Write((ushort) number3);
              writer.Write((ushort) number4);
              writer.Write((byte) number5);
              if (number5 < 0)
              {
                writer.Write((short) number6);
                break;
              }
              break;
            }
            break;
          case 92:
            writer.Write((short) number);
            writer.Write((int) number2);
            writer.Write(number3);
            writer.Write(number4);
            break;
          case 95:
            writer.Write((ushort) number);
            writer.Write((byte) number2);
            break;
          case 96:
            writer.Write((byte) number);
            Player player6 = Main.player[number];
            writer.Write((short) number4);
            writer.Write(number2);
            writer.Write(number3);
            writer.WriteVector2(player6.velocity);
            break;
          case 97:
            writer.Write((short) number);
            break;
          case 98:
            writer.Write((short) number);
            break;
          case 99:
            writer.Write((byte) number);
            writer.WriteVector2(Main.player[number].MinionRestTargetPoint);
            break;
          case 100:
            writer.Write((ushort) number);
            NPC npc2 = Main.npc[number];
            writer.Write((short) number4);
            writer.Write(number2);
            writer.Write(number3);
            writer.WriteVector2(npc2.velocity);
            break;
          case 101:
            writer.Write((ushort) NPC.ShieldStrengthTowerSolar);
            writer.Write((ushort) NPC.ShieldStrengthTowerVortex);
            writer.Write((ushort) NPC.ShieldStrengthTowerNebula);
            writer.Write((ushort) NPC.ShieldStrengthTowerStardust);
            break;
          case 102:
            writer.Write((byte) number);
            writer.Write((ushort) number2);
            writer.Write(number3);
            writer.Write(number4);
            break;
          case 103:
            writer.Write(NPC.MoonLordCountdown);
            break;
          case 104:
            writer.Write((byte) number);
            writer.Write((short) number2);
            writer.Write((short) number3 < (short) 0 ? 0.0f : number3);
            writer.Write((byte) number4);
            writer.Write(number5);
            writer.Write((byte) number6);
            break;
          case 105:
            writer.Write((short) number);
            writer.Write((short) number2);
            writer.Write((double) number3 == 1.0);
            break;
          case 106:
            HalfVector2 halfVector2 = new HalfVector2((float) number, number2);
            writer.Write(halfVector2.PackedValue);
            break;
          case 107:
            writer.Write((byte) number2);
            writer.Write((byte) number3);
            writer.Write((byte) number4);
            text.Serialize(writer);
            writer.Write((short) number5);
            break;
          case 108:
            writer.Write((short) number);
            writer.Write(number2);
            writer.Write((short) number3);
            writer.Write((short) number4);
            writer.Write((short) number5);
            writer.Write((short) number6);
            writer.Write((byte) number7);
            break;
          case 109:
            writer.Write((short) number);
            writer.Write((short) number2);
            writer.Write((short) number3);
            writer.Write((short) number4);
            writer.Write((byte) number5);
            break;
          case 110:
            writer.Write((short) number);
            writer.Write((short) number2);
            writer.Write((byte) number3);
            break;
          case 112:
            writer.Write((byte) number);
            writer.Write((int) number2);
            writer.Write((int) number3);
            writer.Write((byte) number4);
            writer.Write((short) number5);
            break;
          case 113:
            writer.Write((short) number);
            writer.Write((short) number2);
            break;
          case 115:
            writer.Write((byte) number);
            writer.Write((short) Main.player[number].MinionAttackTargetNPC);
            break;
          case 116:
            writer.Write(number);
            break;
          case 117:
            writer.Write((byte) number);
            NetMessage._currentPlayerDeathReason.WriteSelfTo(writer);
            writer.Write((short) number2);
            writer.Write((byte) ((double) number3 + 1.0));
            writer.Write((byte) number4);
            writer.Write((sbyte) number5);
            break;
          case 118:
            writer.Write((byte) number);
            NetMessage._currentPlayerDeathReason.WriteSelfTo(writer);
            writer.Write((short) number2);
            writer.Write((byte) ((double) number3 + 1.0));
            writer.Write((byte) number4);
            break;
          case 119:
            writer.Write(number2);
            writer.Write(number3);
            writer.WriteRGB(new Color()
            {
              PackedValue = (uint) number
            });
            text.Serialize(writer);
            break;
          case 120:
            writer.Write((byte) number);
            writer.Write((byte) number2);
            break;
          case 121:
            int num18 = (int) number3;
            bool dye1 = (double) number4 == 1.0;
            if (dye1)
              num18 += 8;
            writer.Write((byte) number);
            writer.Write((int) number2);
            writer.Write((byte) num18);
            if (TileEntity.ByID[(int) number2] is TEDisplayDoll teDisplayDoll)
            {
              teDisplayDoll.WriteItem((int) number3, writer, dye1);
              break;
            }
            writer.Write(0);
            writer.Write((byte) 0);
            break;
          case 122:
            writer.Write(number);
            writer.Write((byte) number2);
            break;
          case 123:
            writer.Write((short) number);
            writer.Write((short) number2);
            Item obj6 = Main.player[(int) number4].inventory[(int) number3];
            writer.Write((short) obj6.netID);
            writer.Write(obj6.prefix);
            writer.Write((short) number5);
            break;
          case 124:
            int num19 = (int) number3;
            bool dye2 = (double) number4 == 1.0;
            if (dye2)
              num19 += 2;
            writer.Write((byte) number);
            writer.Write((int) number2);
            writer.Write((byte) num19);
            if (TileEntity.ByID[(int) number2] is TEHatRack teHatRack)
            {
              teHatRack.WriteItem((int) number3, writer, dye2);
              break;
            }
            writer.Write(0);
            writer.Write((byte) 0);
            break;
          case 125:
            writer.Write((byte) number);
            writer.Write((short) number2);
            writer.Write((short) number3);
            writer.Write((byte) number4);
            break;
          case 126:
            NetMessage._currentRevengeMarker.WriteSelfTo(writer);
            break;
          case (int) sbyte.MaxValue:
            writer.Write(number);
            break;
          case 128:
            writer.Write((byte) number);
            writer.Write((ushort) number5);
            writer.Write((ushort) number6);
            writer.Write((ushort) number2);
            writer.Write((ushort) number3);
            break;
          case 130:
            writer.Write((ushort) number);
            writer.Write((ushort) number2);
            writer.Write((short) number3);
            break;
          case 131:
            writer.Write((ushort) number);
            writer.Write((byte) number2);
            if ((byte) number2 == (byte) 1)
            {
              writer.Write((int) number3);
              writer.Write((short) number4);
              break;
            }
            break;
          case 132:
            NetMessage._currentNetSoundInfo.WriteSelfTo(writer);
            break;
          case 133:
            writer.Write((short) number);
            writer.Write((short) number2);
            Item obj7 = Main.player[(int) number4].inventory[(int) number3];
            writer.Write((short) obj7.netID);
            writer.Write(obj7.prefix);
            writer.Write((short) number5);
            break;
          case 134:
            writer.Write((byte) number);
            Player player7 = Main.player[number];
            writer.Write(player7.ladyBugLuckTimeLeft);
            writer.Write(player7.torchLuck);
            writer.Write(player7.luckPotion);
            writer.Write(player7.HasGardenGnomeNearby);
            break;
          case 135:
            writer.Write((byte) number);
            break;
          case 136:
            for (int index14 = 0; index14 < 2; ++index14)
            {
              for (int index15 = 0; index15 < 3; ++index15)
                writer.Write((ushort) NPC.cavernMonsterType[index14, index15]);
            }
            break;
          case 137:
            writer.Write((short) number);
            writer.Write((ushort) number2);
            break;
          case 139:
            writer.Write((byte) number);
            bool flag3 = (double) number2 == 1.0;
            writer.Write(flag3);
            break;
          case 140:
            writer.Write((byte) number);
            writer.Write((int) number2);
            break;
          case 141:
            writer.Write((byte) number);
            writer.Write((byte) number2);
            writer.Write(number3);
            writer.Write(number4);
            writer.Write(number5);
            writer.Write(number6);
            break;
          case 142:
            writer.Write((byte) number);
            Player player8 = Main.player[number];
            player8.piggyBankProjTracker.Write(writer);
            player8.voidLensChest.Write(writer);
            break;
        }
        int position2 = (int) writer.BaseStream.Position;
        writer.BaseStream.Position = position1;
        writer.Write((short) position2);
        writer.BaseStream.Position = (long) position2;
        if (Main.netMode == 1)
        {
          if (Netplay.Connection.Socket.IsConnected())
          {
            try
            {
              ++NetMessage.buffer[index1].spamCount;
              Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, position2);
              Netplay.Connection.Socket.AsyncSend(NetMessage.buffer[index1].writeBuffer, 0, position2, new SocketSendCallback(Netplay.Connection.ClientWriteCallBack));
            }
            catch
            {
            }
          }
        }
        else if (remoteClient == -1)
        {
          switch (msgType)
          {
            case 13:
              for (int index16 = 0; index16 < 256; ++index16)
              {
                if (index16 != ignoreClient && NetMessage.buffer[index16].broadcast)
                {
                  if (Netplay.Clients[index16].IsConnected())
                  {
                    try
                    {
                      ++NetMessage.buffer[index16].spamCount;
                      Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, position2);
                      Netplay.Clients[index16].Socket.AsyncSend(NetMessage.buffer[index1].writeBuffer, 0, position2, new SocketSendCallback(Netplay.Clients[index16].ServerWriteCallBack));
                    }
                    catch
                    {
                    }
                  }
                }
              }
              ++Main.player[number].netSkip;
              if (Main.player[number].netSkip > 2)
              {
                Main.player[number].netSkip = 0;
                break;
              }
              break;
            case 20:
              for (int index17 = 0; index17 < 256; ++index17)
              {
                if (index17 != ignoreClient && NetMessage.buffer[index17].broadcast && Netplay.Clients[index17].IsConnected())
                {
                  if (Netplay.Clients[index17].SectionRange((int) Math.Max(number3, number4), number, (int) number2))
                  {
                    try
                    {
                      ++NetMessage.buffer[index17].spamCount;
                      Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, position2);
                      Netplay.Clients[index17].Socket.AsyncSend(NetMessage.buffer[index1].writeBuffer, 0, position2, new SocketSendCallback(Netplay.Clients[index17].ServerWriteCallBack));
                    }
                    catch
                    {
                    }
                  }
                }
              }
              break;
            case 23:
              NPC npc3 = Main.npc[number];
              for (int index18 = 0; index18 < 256; ++index18)
              {
                if (index18 != ignoreClient && NetMessage.buffer[index18].broadcast && Netplay.Clients[index18].IsConnected())
                {
                  bool flag4 = false;
                  if (npc3.boss || npc3.netAlways || npc3.townNPC || !npc3.active)
                    flag4 = true;
                  else if (npc3.netSkip <= 0)
                  {
                    Rectangle rect1 = Main.player[index18].getRect();
                    Rectangle rect2 = npc3.getRect();
                    rect2.X -= 2500;
                    rect2.Y -= 2500;
                    rect2.Width += 5000;
                    rect2.Height += 5000;
                    if (rect1.Intersects(rect2))
                      flag4 = true;
                  }
                  else
                    flag4 = true;
                  if (flag4)
                  {
                    try
                    {
                      ++NetMessage.buffer[index18].spamCount;
                      Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, position2);
                      Netplay.Clients[index18].Socket.AsyncSend(NetMessage.buffer[index1].writeBuffer, 0, position2, new SocketSendCallback(Netplay.Clients[index18].ServerWriteCallBack));
                    }
                    catch
                    {
                    }
                  }
                }
              }
              ++npc3.netSkip;
              if (npc3.netSkip > 4)
              {
                npc3.netSkip = 0;
                break;
              }
              break;
            case 27:
              Projectile projectile2 = Main.projectile[number];
              for (int index19 = 0; index19 < 256; ++index19)
              {
                if (index19 != ignoreClient && NetMessage.buffer[index19].broadcast && Netplay.Clients[index19].IsConnected())
                {
                  bool flag5 = false;
                  if (projectile2.type == 12 || Main.projPet[projectile2.type] || projectile2.aiStyle == 11 || projectile2.netImportant)
                  {
                    flag5 = true;
                  }
                  else
                  {
                    Rectangle rect3 = Main.player[index19].getRect();
                    Rectangle rect4 = projectile2.getRect();
                    rect4.X -= 5000;
                    rect4.Y -= 5000;
                    rect4.Width += 10000;
                    rect4.Height += 10000;
                    if (rect3.Intersects(rect4))
                      flag5 = true;
                  }
                  if (flag5)
                  {
                    try
                    {
                      ++NetMessage.buffer[index19].spamCount;
                      Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, position2);
                      Netplay.Clients[index19].Socket.AsyncSend(NetMessage.buffer[index1].writeBuffer, 0, position2, new SocketSendCallback(Netplay.Clients[index19].ServerWriteCallBack));
                    }
                    catch
                    {
                    }
                  }
                }
              }
              break;
            case 28:
              NPC npc4 = Main.npc[number];
              for (int index20 = 0; index20 < 256; ++index20)
              {
                if (index20 != ignoreClient && NetMessage.buffer[index20].broadcast && Netplay.Clients[index20].IsConnected())
                {
                  bool flag6 = false;
                  if (npc4.life <= 0)
                  {
                    flag6 = true;
                  }
                  else
                  {
                    Rectangle rect5 = Main.player[index20].getRect();
                    Rectangle rect6 = npc4.getRect();
                    rect6.X -= 3000;
                    rect6.Y -= 3000;
                    rect6.Width += 6000;
                    rect6.Height += 6000;
                    if (rect5.Intersects(rect6))
                      flag6 = true;
                  }
                  if (flag6)
                  {
                    try
                    {
                      ++NetMessage.buffer[index20].spamCount;
                      Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, position2);
                      Netplay.Clients[index20].Socket.AsyncSend(NetMessage.buffer[index1].writeBuffer, 0, position2, new SocketSendCallback(Netplay.Clients[index20].ServerWriteCallBack));
                    }
                    catch
                    {
                    }
                  }
                }
              }
              break;
            case 34:
            case 69:
              for (int index21 = 0; index21 < 256; ++index21)
              {
                if (index21 != ignoreClient && NetMessage.buffer[index21].broadcast)
                {
                  if (Netplay.Clients[index21].IsConnected())
                  {
                    try
                    {
                      ++NetMessage.buffer[index21].spamCount;
                      Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, position2);
                      Netplay.Clients[index21].Socket.AsyncSend(NetMessage.buffer[index1].writeBuffer, 0, position2, new SocketSendCallback(Netplay.Clients[index21].ServerWriteCallBack));
                    }
                    catch
                    {
                    }
                  }
                }
              }
              break;
            default:
              for (int index22 = 0; index22 < 256; ++index22)
              {
                if (index22 != ignoreClient && (NetMessage.buffer[index22].broadcast || Netplay.Clients[index22].State >= 3 && msgType == 10))
                {
                  if (Netplay.Clients[index22].IsConnected())
                  {
                    try
                    {
                      ++NetMessage.buffer[index22].spamCount;
                      Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, position2);
                      Netplay.Clients[index22].Socket.AsyncSend(NetMessage.buffer[index1].writeBuffer, 0, position2, new SocketSendCallback(Netplay.Clients[index22].ServerWriteCallBack));
                    }
                    catch
                    {
                    }
                  }
                }
              }
              break;
          }
        }
        else if (Netplay.Clients[remoteClient].IsConnected())
        {
          try
          {
            ++NetMessage.buffer[remoteClient].spamCount;
            Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, position2);
            Netplay.Clients[remoteClient].Socket.AsyncSend(NetMessage.buffer[index1].writeBuffer, 0, position2, new SocketSendCallback(Netplay.Clients[remoteClient].ServerWriteCallBack));
          }
          catch
          {
          }
        }
        if (Main.verboseNetplay)
        {
          int num20 = 0;
          while (num20 < position2)
            ++num20;
          for (int index23 = 0; index23 < position2; ++index23)
          {
            int num21 = (int) NetMessage.buffer[index1].writeBuffer[index23];
          }
        }
        NetMessage.buffer[index1].writeLocked = false;
        if (msgType != 2 || Main.netMode != 2)
          return;
        Netplay.Clients[index1].PendingTermination = true;
        Netplay.Clients[index1].PendingTerminationApproved = true;
      }
    }

    public static int CompressTileBlock(
      int xStart,
      int yStart,
      short width,
      short height,
      byte[] buffer,
      int bufferStart)
    {
      using (MemoryStream output = new MemoryStream())
      {
        using (BinaryWriter writer = new BinaryWriter((Stream) output))
        {
          writer.Write(xStart);
          writer.Write(yStart);
          writer.Write(width);
          writer.Write(height);
          NetMessage.CompressTileBlock_Inner(writer, xStart, yStart, (int) width, (int) height);
          int length = buffer.Length;
          if ((long) bufferStart + output.Length > (long) length)
            return (int) ((long) (length - bufferStart) + output.Length);
          output.Position = 0L;
          MemoryStream memoryStream = new MemoryStream();
          using (DeflateStream destination = new DeflateStream((Stream) memoryStream, (CompressionMode) 0, true))
          {
            output.CopyTo((Stream) destination);
            ((Stream) destination).Flush();
            ((Stream) destination).Close();
            ((Stream) destination).Dispose();
          }
          if (output.Length <= memoryStream.Length)
          {
            output.Position = 0L;
            buffer[bufferStart] = (byte) 0;
            ++bufferStart;
            output.Read(buffer, bufferStart, (int) output.Length);
            return (int) output.Length + 1;
          }
          memoryStream.Position = 0L;
          buffer[bufferStart] = (byte) 1;
          ++bufferStart;
          memoryStream.Read(buffer, bufferStart, (int) memoryStream.Length);
          return (int) memoryStream.Length + 1;
        }
      }
    }

    public static void CompressTileBlock_Inner(
      BinaryWriter writer,
      int xStart,
      int yStart,
      int width,
      int height)
    {
      short[] numArray1 = new short[8000];
      short[] numArray2 = new short[1000];
      short[] numArray3 = new short[1000];
      short index1 = 0;
      short num1 = 0;
      short num2 = 0;
      short num3 = 0;
      int index2 = 0;
      int index3 = 0;
      byte num4 = 0;
      byte[] buffer = new byte[15];
      Tile compTile = (Tile) null;
      for (int index4 = yStart; index4 < yStart + height; ++index4)
      {
        for (int index5 = xStart; index5 < xStart + width; ++index5)
        {
          Tile tile = Main.tile[index5, index4];
          if (tile.isTheSameAs(compTile) && TileID.Sets.AllowsSaveCompressionBatching[(int) tile.type])
          {
            ++num3;
          }
          else
          {
            if (compTile != null)
            {
              if (num3 > (short) 0)
              {
                buffer[index2] = (byte) ((uint) num3 & (uint) byte.MaxValue);
                ++index2;
                if (num3 > (short) byte.MaxValue)
                {
                  num4 |= (byte) 128;
                  buffer[index2] = (byte) (((int) num3 & 65280) >> 8);
                  ++index2;
                }
                else
                  num4 |= (byte) 64;
              }
              buffer[index3] = num4;
              writer.Write(buffer, index3, index2 - index3);
              num3 = (short) 0;
            }
            index2 = 3;
            int num5;
            byte num6 = (byte) (num5 = 0);
            byte num7 = (byte) num5;
            num4 = (byte) num5;
            if (tile.active())
            {
              num4 |= (byte) 2;
              buffer[index2] = (byte) tile.type;
              ++index2;
              if (tile.type > (ushort) byte.MaxValue)
              {
                buffer[index2] = (byte) ((uint) tile.type >> 8);
                ++index2;
                num4 |= (byte) 32;
              }
              if (TileID.Sets.BasicChest[(int) tile.type] && (int) tile.frameX % 36 == 0 && (int) tile.frameY % 36 == 0)
              {
                short chest = (short) Chest.FindChest(index5, index4);
                if (chest != (short) -1)
                {
                  numArray1[(int) index1] = chest;
                  ++index1;
                }
              }
              if (tile.type == (ushort) 88 && (int) tile.frameX % 54 == 0 && (int) tile.frameY % 36 == 0)
              {
                short chest = (short) Chest.FindChest(index5, index4);
                if (chest != (short) -1)
                {
                  numArray1[(int) index1] = chest;
                  ++index1;
                }
              }
              if (tile.type == (ushort) 85 && (int) tile.frameX % 36 == 0 && (int) tile.frameY % 36 == 0)
              {
                short num8 = (short) Sign.ReadSign(index5, index4);
                if (num8 != (short) -1)
                  numArray2[(int) num1++] = num8;
              }
              if (tile.type == (ushort) 55 && (int) tile.frameX % 36 == 0 && (int) tile.frameY % 36 == 0)
              {
                short num9 = (short) Sign.ReadSign(index5, index4);
                if (num9 != (short) -1)
                  numArray2[(int) num1++] = num9;
              }
              if (tile.type == (ushort) 425 && (int) tile.frameX % 36 == 0 && (int) tile.frameY % 36 == 0)
              {
                short num10 = (short) Sign.ReadSign(index5, index4);
                if (num10 != (short) -1)
                  numArray2[(int) num1++] = num10;
              }
              if (tile.type == (ushort) 573 && (int) tile.frameX % 36 == 0 && (int) tile.frameY % 36 == 0)
              {
                short num11 = (short) Sign.ReadSign(index5, index4);
                if (num11 != (short) -1)
                  numArray2[(int) num1++] = num11;
              }
              if (tile.type == (ushort) 378 && (int) tile.frameX % 36 == 0 && tile.frameY == (short) 0)
              {
                int num12 = TETrainingDummy.Find(index5, index4);
                if (num12 != -1)
                  numArray3[(int) num2++] = (short) num12;
              }
              if (tile.type == (ushort) 395 && (int) tile.frameX % 36 == 0 && tile.frameY == (short) 0)
              {
                int num13 = TEItemFrame.Find(index5, index4);
                if (num13 != -1)
                  numArray3[(int) num2++] = (short) num13;
              }
              if (tile.type == (ushort) 520 && (int) tile.frameX % 18 == 0 && tile.frameY == (short) 0)
              {
                int num14 = TEFoodPlatter.Find(index5, index4);
                if (num14 != -1)
                  numArray3[(int) num2++] = (short) num14;
              }
              if (tile.type == (ushort) 471 && (int) tile.frameX % 54 == 0 && tile.frameY == (short) 0)
              {
                int num15 = TEWeaponsRack.Find(index5, index4);
                if (num15 != -1)
                  numArray3[(int) num2++] = (short) num15;
              }
              if (tile.type == (ushort) 470 && (int) tile.frameX % 36 == 0 && tile.frameY == (short) 0)
              {
                int num16 = TEDisplayDoll.Find(index5, index4);
                if (num16 != -1)
                  numArray3[(int) num2++] = (short) num16;
              }
              if (tile.type == (ushort) 475 && (int) tile.frameX % 54 == 0 && tile.frameY == (short) 0)
              {
                int num17 = TEHatRack.Find(index5, index4);
                if (num17 != -1)
                  numArray3[(int) num2++] = (short) num17;
              }
              if (tile.type == (ushort) 597 && (int) tile.frameX % 54 == 0 && (int) tile.frameY % 72 == 0)
              {
                int num18 = TETeleportationPylon.Find(index5, index4);
                if (num18 != -1)
                  numArray3[(int) num2++] = (short) num18;
              }
              if (Main.tileFrameImportant[(int) tile.type])
              {
                buffer[index2] = (byte) ((uint) tile.frameX & (uint) byte.MaxValue);
                int index6 = index2 + 1;
                buffer[index6] = (byte) (((int) tile.frameX & 65280) >> 8);
                int index7 = index6 + 1;
                buffer[index7] = (byte) ((uint) tile.frameY & (uint) byte.MaxValue);
                int index8 = index7 + 1;
                buffer[index8] = (byte) (((int) tile.frameY & 65280) >> 8);
                index2 = index8 + 1;
              }
              if (tile.color() != (byte) 0)
              {
                num6 |= (byte) 8;
                buffer[index2] = tile.color();
                ++index2;
              }
            }
            if (tile.wall != (ushort) 0)
            {
              num4 |= (byte) 4;
              buffer[index2] = (byte) tile.wall;
              ++index2;
              if (tile.wallColor() != (byte) 0)
              {
                num6 |= (byte) 16;
                buffer[index2] = tile.wallColor();
                ++index2;
              }
            }
            if (tile.liquid != (byte) 0)
            {
              if (tile.lava())
                num4 |= (byte) 16;
              else if (tile.honey())
                num4 |= (byte) 24;
              else
                num4 |= (byte) 8;
              buffer[index2] = tile.liquid;
              ++index2;
            }
            if (tile.wire())
              num7 |= (byte) 2;
            if (tile.wire2())
              num7 |= (byte) 4;
            if (tile.wire3())
              num7 |= (byte) 8;
            int num19 = !tile.halfBrick() ? (tile.slope() == (byte) 0 ? 0 : (int) tile.slope() + 1 << 4) : 16;
            byte num20 = (byte) ((uint) num7 | (uint) (byte) num19);
            if (tile.actuator())
              num6 |= (byte) 2;
            if (tile.inActive())
              num6 |= (byte) 4;
            if (tile.wire4())
              num6 |= (byte) 32;
            if (tile.wall > (ushort) byte.MaxValue)
            {
              buffer[index2] = (byte) ((uint) tile.wall >> 8);
              ++index2;
              num6 |= (byte) 64;
            }
            index3 = 2;
            if (num6 != (byte) 0)
            {
              num20 |= (byte) 1;
              buffer[index3] = num6;
              --index3;
            }
            if (num20 != (byte) 0)
            {
              num4 |= (byte) 1;
              buffer[index3] = num20;
              --index3;
            }
            compTile = tile;
          }
        }
      }
      if (num3 > (short) 0)
      {
        buffer[index2] = (byte) ((uint) num3 & (uint) byte.MaxValue);
        ++index2;
        if (num3 > (short) byte.MaxValue)
        {
          num4 |= (byte) 128;
          buffer[index2] = (byte) (((int) num3 & 65280) >> 8);
          ++index2;
        }
        else
          num4 |= (byte) 64;
      }
      buffer[index3] = num4;
      writer.Write(buffer, index3, index2 - index3);
      writer.Write(index1);
      for (int index9 = 0; index9 < (int) index1; ++index9)
      {
        Chest chest = Main.chest[(int) numArray1[index9]];
        writer.Write(numArray1[index9]);
        writer.Write((short) chest.x);
        writer.Write((short) chest.y);
        writer.Write(chest.name);
      }
      writer.Write(num1);
      for (int index10 = 0; index10 < (int) num1; ++index10)
      {
        Sign sign = Main.sign[(int) numArray2[index10]];
        writer.Write(numArray2[index10]);
        writer.Write((short) sign.x);
        writer.Write((short) sign.y);
        writer.Write(sign.text);
      }
      writer.Write(num2);
      for (int index11 = 0; index11 < (int) num2; ++index11)
        TileEntity.Write(writer, TileEntity.ByID[(int) numArray3[index11]]);
    }

    public static void DecompressTileBlock(byte[] buffer, int bufferStart, int bufferLength)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        memoryStream.Write(buffer, bufferStart, bufferLength);
        memoryStream.Position = 0L;
        MemoryStream input;
        if ((uint) memoryStream.ReadByte() > 0U)
        {
          MemoryStream destination = new MemoryStream();
          using (DeflateStream deflateStream = new DeflateStream((Stream) memoryStream, (CompressionMode) 1, true))
          {
            ((Stream) deflateStream).CopyTo((Stream) destination);
            ((Stream) deflateStream).Close();
          }
          input = destination;
          input.Position = 0L;
        }
        else
        {
          input = memoryStream;
          input.Position = 1L;
        }
        using (BinaryReader reader = new BinaryReader((Stream) input))
        {
          int xStart = reader.ReadInt32();
          int yStart = reader.ReadInt32();
          short width = reader.ReadInt16();
          short height = reader.ReadInt16();
          NetMessage.DecompressTileBlock_Inner(reader, xStart, yStart, (int) width, (int) height);
        }
      }
    }

    public static void DecompressTileBlock_Inner(
      BinaryReader reader,
      int xStart,
      int yStart,
      int width,
      int height)
    {
      Tile tile = (Tile) null;
      int num1 = 0;
      for (int index1 = yStart; index1 < yStart + height; ++index1)
      {
        for (int index2 = xStart; index2 < xStart + width; ++index2)
        {
          if (num1 != 0)
          {
            --num1;
            if (Main.tile[index2, index1] == null)
              Main.tile[index2, index1] = new Tile(tile);
            else
              Main.tile[index2, index1].CopyFrom(tile);
          }
          else
          {
            byte num2;
            byte num3 = num2 = (byte) 0;
            tile = Main.tile[index2, index1];
            if (tile == null)
            {
              tile = new Tile();
              Main.tile[index2, index1] = tile;
            }
            else
              tile.ClearEverything();
            byte num4 = reader.ReadByte();
            if (((int) num4 & 1) == 1)
            {
              num3 = reader.ReadByte();
              if (((int) num3 & 1) == 1)
                num2 = reader.ReadByte();
            }
            bool flag = tile.active();
            if (((int) num4 & 2) == 2)
            {
              tile.active(true);
              ushort type = tile.type;
              int index3;
              if (((int) num4 & 32) == 32)
              {
                byte num5 = reader.ReadByte();
                index3 = (int) reader.ReadByte() << 8 | (int) num5;
              }
              else
                index3 = (int) reader.ReadByte();
              tile.type = (ushort) index3;
              if (Main.tileFrameImportant[index3])
              {
                tile.frameX = reader.ReadInt16();
                tile.frameY = reader.ReadInt16();
              }
              else if (!flag || (int) tile.type != (int) type)
              {
                tile.frameX = (short) -1;
                tile.frameY = (short) -1;
              }
              if (((int) num2 & 8) == 8)
                tile.color(reader.ReadByte());
            }
            if (((int) num4 & 4) == 4)
            {
              tile.wall = (ushort) reader.ReadByte();
              if (((int) num2 & 16) == 16)
                tile.wallColor(reader.ReadByte());
            }
            byte num6 = (byte) (((int) num4 & 24) >> 3);
            if (num6 != (byte) 0)
            {
              tile.liquid = reader.ReadByte();
              if (num6 > (byte) 1)
              {
                if (num6 == (byte) 2)
                  tile.lava(true);
                else
                  tile.honey(true);
              }
            }
            if (num3 > (byte) 1)
            {
              if (((int) num3 & 2) == 2)
                tile.wire(true);
              if (((int) num3 & 4) == 4)
                tile.wire2(true);
              if (((int) num3 & 8) == 8)
                tile.wire3(true);
              byte num7 = (byte) (((int) num3 & 112) >> 4);
              if (num7 != (byte) 0 && Main.tileSolid[(int) tile.type])
              {
                if (num7 == (byte) 1)
                  tile.halfBrick(true);
                else
                  tile.slope((byte) ((uint) num7 - 1U));
              }
            }
            if (num2 > (byte) 0)
            {
              if (((int) num2 & 2) == 2)
                tile.actuator(true);
              if (((int) num2 & 4) == 4)
                tile.inActive(true);
              if (((int) num2 & 32) == 32)
                tile.wire4(true);
              if (((int) num2 & 64) == 64)
              {
                byte num8 = reader.ReadByte();
                tile.wall = (ushort) ((uint) num8 << 8 | (uint) tile.wall);
              }
            }
            switch ((byte) (((int) num4 & 192) >> 6))
            {
              case 0:
                num1 = 0;
                continue;
              case 1:
                num1 = (int) reader.ReadByte();
                continue;
              default:
                num1 = (int) reader.ReadInt16();
                continue;
            }
          }
        }
      }
      short num9 = reader.ReadInt16();
      for (int index4 = 0; index4 < (int) num9; ++index4)
      {
        short index5 = reader.ReadInt16();
        short num10 = reader.ReadInt16();
        short num11 = reader.ReadInt16();
        string str = reader.ReadString();
        if (index5 >= (short) 0 && index5 < (short) 8000)
        {
          if (Main.chest[(int) index5] == null)
            Main.chest[(int) index5] = new Chest();
          Main.chest[(int) index5].name = str;
          Main.chest[(int) index5].x = (int) num10;
          Main.chest[(int) index5].y = (int) num11;
        }
      }
      short num12 = reader.ReadInt16();
      for (int index6 = 0; index6 < (int) num12; ++index6)
      {
        short index7 = reader.ReadInt16();
        short num13 = reader.ReadInt16();
        short num14 = reader.ReadInt16();
        string str = reader.ReadString();
        if (index7 >= (short) 0 && index7 < (short) 1000)
        {
          if (Main.sign[(int) index7] == null)
            Main.sign[(int) index7] = new Sign();
          Main.sign[(int) index7].text = str;
          Main.sign[(int) index7].x = (int) num13;
          Main.sign[(int) index7].y = (int) num14;
        }
      }
      short num15 = reader.ReadInt16();
      for (int index = 0; index < (int) num15; ++index)
      {
        TileEntity tileEntity = TileEntity.Read(reader);
        TileEntity.ByID[tileEntity.ID] = tileEntity;
        TileEntity.ByPosition[tileEntity.Position] = tileEntity;
      }
    }

    public static void ReceiveBytes(byte[] bytes, int streamLength, int i = 256)
    {
      lock (NetMessage.buffer[i])
      {
        try
        {
          Buffer.BlockCopy((Array) bytes, 0, (Array) NetMessage.buffer[i].readBuffer, NetMessage.buffer[i].totalData, streamLength);
          NetMessage.buffer[i].totalData += streamLength;
          NetMessage.buffer[i].checkBytes = true;
        }
        catch
        {
          if (Main.netMode == 1)
          {
            Main.menuMode = 15;
            Main.statusText = Language.GetTextValue("Error.BadHeaderBufferOverflow");
            Netplay.Disconnect = true;
          }
          else
            Netplay.Clients[i].PendingTermination = true;
        }
      }
    }

    public static void CheckBytes(int bufferIndex = 256)
    {
      lock (NetMessage.buffer[bufferIndex])
      {
        int startIndex = 0;
        int num = NetMessage.buffer[bufferIndex].totalData;
        try
        {
          while (num >= 2)
          {
            int uint16 = (int) BitConverter.ToUInt16(NetMessage.buffer[bufferIndex].readBuffer, startIndex);
            if (num >= uint16)
            {
              long position = NetMessage.buffer[bufferIndex].reader.BaseStream.Position;
              NetMessage.buffer[bufferIndex].GetData(startIndex + 2, uint16 - 2, out int _);
              NetMessage.buffer[bufferIndex].reader.BaseStream.Position = position + (long) uint16;
              num -= uint16;
              startIndex += uint16;
            }
            else
              break;
          }
        }
        catch (Exception ex)
        {
          num = 0;
          startIndex = 0;
        }
        if (num != NetMessage.buffer[bufferIndex].totalData)
        {
          for (int index = 0; index < num; ++index)
            NetMessage.buffer[bufferIndex].readBuffer[index] = NetMessage.buffer[bufferIndex].readBuffer[index + startIndex];
          NetMessage.buffer[bufferIndex].totalData = num;
        }
        NetMessage.buffer[bufferIndex].checkBytes = false;
      }
    }

    public static void BootPlayer(int plr, NetworkText msg) => NetMessage.SendData(2, plr, text: msg);

    public static void SendObjectPlacment(
      int whoAmi,
      int x,
      int y,
      int type,
      int style,
      int alternative,
      int random,
      int direction)
    {
      int remoteClient;
      int ignoreClient;
      if (Main.netMode == 2)
      {
        remoteClient = -1;
        ignoreClient = whoAmi;
      }
      else
      {
        remoteClient = whoAmi;
        ignoreClient = -1;
      }
      NetMessage.SendData(79, remoteClient, ignoreClient, number: x, number2: ((float) y), number3: ((float) type), number4: ((float) style), number5: alternative, number6: random, number7: direction);
    }

    public static void SendTemporaryAnimation(
      int whoAmi,
      int animationType,
      int tileType,
      int xCoord,
      int yCoord)
    {
      if (Main.netMode != 2)
        return;
      NetMessage.SendData(77, whoAmi, number: animationType, number2: ((float) tileType), number3: ((float) xCoord), number4: ((float) yCoord));
    }

    public static void SendPlayerHurt(
      int playerTargetIndex,
      PlayerDeathReason reason,
      int damage,
      int direction,
      bool critical,
      bool pvp,
      int hitContext,
      int remoteClient = -1,
      int ignoreClient = -1)
    {
      NetMessage._currentPlayerDeathReason = reason;
      BitsByte number4 = (BitsByte) (byte) 0;
      number4[0] = critical;
      number4[1] = pvp;
      NetMessage.SendData(117, remoteClient, ignoreClient, number: playerTargetIndex, number2: ((float) damage), number3: ((float) direction), number4: ((float) (byte) number4), number5: hitContext);
    }

    public static void SendPlayerDeath(
      int playerTargetIndex,
      PlayerDeathReason reason,
      int damage,
      int direction,
      bool pvp,
      int remoteClient = -1,
      int ignoreClient = -1)
    {
      NetMessage._currentPlayerDeathReason = reason;
      BitsByte number4 = (BitsByte) (byte) 0;
      number4[0] = pvp;
      NetMessage.SendData(118, remoteClient, ignoreClient, number: playerTargetIndex, number2: ((float) damage), number3: ((float) direction), number4: ((float) (byte) number4));
    }

    public static void PlayNetSound(
      NetMessage.NetSoundInfo info,
      int remoteClient = -1,
      int ignoreClient = -1)
    {
      NetMessage._currentNetSoundInfo = info;
      NetMessage.SendData(132, remoteClient, ignoreClient);
    }

    public static void SendCoinLossRevengeMarker(
      CoinLossRevengeSystem.RevengeMarker marker,
      int remoteClient = -1,
      int ignoreClient = -1)
    {
      NetMessage._currentRevengeMarker = marker;
      NetMessage.SendData(126, remoteClient, ignoreClient);
    }

    public static void SendTileSquare(
      int whoAmi,
      int tileX,
      int tileY,
      int xSize,
      int ySize,
      TileChangeType changeType = TileChangeType.None)
    {
      NetMessage.SendData(20, whoAmi, number: tileX, number2: ((float) tileY), number3: ((float) xSize), number4: ((float) ySize), number5: ((int) changeType));
      WorldGen.RangeFrame(tileX, tileY, xSize, ySize);
    }

    public static void SendTileSquare(
      int whoAmi,
      int tileX,
      int tileY,
      int centeredSquareSize,
      TileChangeType changeType = TileChangeType.None)
    {
      int num = (centeredSquareSize - 1) / 2;
      NetMessage.SendTileSquare(whoAmi, tileX - num, tileY - num, centeredSquareSize, centeredSquareSize, changeType);
    }

    public static void SendTileSquare(int whoAmi, int tileX, int tileY, TileChangeType changeType = TileChangeType.None)
    {
      int num1 = 1;
      int num2 = (num1 - 1) / 2;
      NetMessage.SendTileSquare(whoAmi, tileX - num2, tileY - num2, num1, num1, changeType);
    }

    public static void SendTravelShop(int remoteClient)
    {
      if (Main.netMode != 2)
        return;
      NetMessage.SendData(72, remoteClient);
    }

    public static void SendAnglerQuest(int remoteClient)
    {
      if (Main.netMode != 2)
        return;
      if (remoteClient == -1)
      {
        for (int remoteClient1 = 0; remoteClient1 < (int) byte.MaxValue; ++remoteClient1)
        {
          if (Netplay.Clients[remoteClient1].State == 10)
            NetMessage.SendData(74, remoteClient1, text: NetworkText.FromLiteral(Main.player[remoteClient1].name), number: Main.anglerQuest);
        }
      }
      else
      {
        if (Netplay.Clients[remoteClient].State != 10)
          return;
        NetMessage.SendData(74, remoteClient, text: NetworkText.FromLiteral(Main.player[remoteClient].name), number: Main.anglerQuest);
      }
    }

    public static void SendSection(int whoAmi, int sectionX, int sectionY, bool skipSent = false)
    {
      if (Main.netMode != 2)
        return;
      try
      {
        if (sectionX < 0 || sectionY < 0 || sectionX >= Main.maxSectionsX || sectionY >= Main.maxSectionsY || skipSent && Netplay.Clients[whoAmi].TileSections[sectionX, sectionY])
          return;
        Netplay.Clients[whoAmi].TileSections[sectionX, sectionY] = true;
        int number1 = sectionX * 200;
        int num1 = sectionY * 150;
        int number4 = 150;
        for (int number2 = num1; number2 < num1 + 150; number2 += number4)
          NetMessage.SendData(10, whoAmi, number: number1, number2: ((float) number2), number3: 200f, number4: ((float) number4));
        for (int number2 = 0; number2 < 200; ++number2)
        {
          if (Main.npc[number2].active && Main.npc[number2].townNPC)
          {
            int sectionX1 = Netplay.GetSectionX((int) ((double) Main.npc[number2].position.X / 16.0));
            int sectionY1 = Netplay.GetSectionY((int) ((double) Main.npc[number2].position.Y / 16.0));
            int num2 = sectionX;
            if (sectionX1 == num2 && sectionY1 == sectionY)
              NetMessage.SendData(23, whoAmi, number: number2);
          }
        }
      }
      catch
      {
      }
    }

    public static void greetPlayer(int plr)
    {
      if (Main.motd == "")
        ChatHelper.SendChatMessageToClient(NetworkText.FromFormattable("{0} {1}!", (object) Lang.mp[18].ToNetworkText(), (object) Main.worldName), new Color((int) byte.MaxValue, 240, 20), plr);
      else
        ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral(Main.motd), new Color((int) byte.MaxValue, 240, 20), plr);
      string str = "";
      for (int index = 0; index < (int) byte.MaxValue; ++index)
      {
        if (Main.player[index].active)
          str = !(str == "") ? str + ", " + Main.player[index].name : str + Main.player[index].name;
      }
      ChatHelper.SendChatMessageToClient(NetworkText.FromKey("Game.JoinGreeting", (object) str), new Color((int) byte.MaxValue, 240, 20), plr);
    }

    public static void sendWater(int x, int y)
    {
      if (Main.netMode == 1)
      {
        NetMessage.SendData(48, number: x, number2: ((float) y));
      }
      else
      {
        for (int remoteClient = 0; remoteClient < 256; ++remoteClient)
        {
          if ((NetMessage.buffer[remoteClient].broadcast || Netplay.Clients[remoteClient].State >= 3) && Netplay.Clients[remoteClient].IsConnected())
          {
            int index1 = x / 200;
            int index2 = y / 150;
            if (Netplay.Clients[remoteClient].TileSections[index1, index2])
              NetMessage.SendData(48, remoteClient, number: x, number2: ((float) y));
          }
        }
      }
    }

    public static void SyncDisconnectedPlayer(int plr)
    {
      NetMessage.SyncOnePlayer(plr, -1, plr);
      NetMessage.EnsureLocalPlayerIsPresent();
    }

    public static void SyncConnectedPlayer(int plr)
    {
      NetMessage.SyncOnePlayer(plr, -1, plr);
      for (int plr1 = 0; plr1 < (int) byte.MaxValue; ++plr1)
      {
        if (plr != plr1 && Main.player[plr1].active)
          NetMessage.SyncOnePlayer(plr1, plr, -1);
      }
      NetMessage.SendNPCHousesAndTravelShop(plr);
      NetMessage.SendAnglerQuest(plr);
      CreditsRollEvent.SendCreditsRollRemainingTimeToPlayer(plr);
      NPC.RevengeManager.SendAllMarkersToPlayer(plr);
      NetMessage.EnsureLocalPlayerIsPresent();
    }

    private static void SendNPCHousesAndTravelShop(int plr)
    {
      bool flag = false;
      for (int number = 0; number < 200; ++number)
      {
        if (Main.npc[number].active && Main.npc[number].townNPC && NPC.TypeToDefaultHeadIndex(Main.npc[number].type) > 0)
        {
          if (!flag && Main.npc[number].type == 368)
            flag = true;
          byte householdStatus = WorldGen.TownManager.GetHouseholdStatus(Main.npc[number]);
          NetMessage.SendData(60, plr, number: number, number2: ((float) Main.npc[number].homeTileX), number3: ((float) Main.npc[number].homeTileY), number4: ((float) householdStatus));
        }
      }
      if (!flag)
        return;
      NetMessage.SendTravelShop(plr);
    }

    private static void EnsureLocalPlayerIsPresent()
    {
      if (!Main.autoShutdown)
        return;
      bool flag = false;
      for (int plr = 0; plr < (int) byte.MaxValue; ++plr)
      {
        if (NetMessage.DoesPlayerSlotCountAsAHost(plr))
        {
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      Console.WriteLine(Language.GetTextValue("Net.ServerAutoShutdown"));
      WorldFile.SaveWorld();
      Netplay.Disconnect = true;
    }

    public static bool DoesPlayerSlotCountAsAHost(int plr) => Netplay.Clients[plr].State == 10 && Netplay.Clients[plr].Socket.GetRemoteAddress().IsLocalHost();

    private static void SyncOnePlayer(int plr, int toWho, int fromWho)
    {
      int number2_1 = 0;
      if (Main.player[plr].active)
        number2_1 = 1;
      if (Netplay.Clients[plr].State == 10)
      {
        NetMessage.SendData(14, toWho, fromWho, number: plr, number2: ((float) number2_1));
        NetMessage.SendData(4, toWho, fromWho, number: plr);
        NetMessage.SendData(13, toWho, fromWho, number: plr);
        if (Main.player[plr].statLife <= 0)
          NetMessage.SendData(135, toWho, fromWho, number: plr);
        NetMessage.SendData(16, toWho, fromWho, number: plr);
        NetMessage.SendData(30, toWho, fromWho, number: plr);
        NetMessage.SendData(45, toWho, fromWho, number: plr);
        NetMessage.SendData(42, toWho, fromWho, number: plr);
        NetMessage.SendData(50, toWho, fromWho, number: plr);
        NetMessage.SendData(80, toWho, fromWho, number: plr, number2: ((float) Main.player[plr].chest));
        NetMessage.SendData(142, toWho, fromWho, number: plr);
        for (int number2_2 = 0; number2_2 < 59; ++number2_2)
          NetMessage.SendData(5, toWho, fromWho, number: plr, number2: ((float) number2_2), number3: ((float) Main.player[plr].inventory[number2_2].prefix));
        for (int index = 0; index < Main.player[plr].armor.Length; ++index)
          NetMessage.SendData(5, toWho, fromWho, number: plr, number2: ((float) (59 + index)), number3: ((float) Main.player[plr].armor[index].prefix));
        for (int index = 0; index < Main.player[plr].dye.Length; ++index)
          NetMessage.SendData(5, toWho, fromWho, number: plr, number2: ((float) (79 + index)), number3: ((float) Main.player[plr].dye[index].prefix));
        for (int index = 0; index < Main.player[plr].miscEquips.Length; ++index)
          NetMessage.SendData(5, toWho, fromWho, number: plr, number2: ((float) (89 + index)), number3: ((float) Main.player[plr].miscEquips[index].prefix));
        for (int index = 0; index < Main.player[plr].miscDyes.Length; ++index)
          NetMessage.SendData(5, toWho, fromWho, number: plr, number2: ((float) (94 + index)), number3: ((float) Main.player[plr].miscDyes[index].prefix));
        if (Netplay.Clients[plr].IsAnnouncementCompleted)
          return;
        Netplay.Clients[plr].IsAnnouncementCompleted = true;
        ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.mp[19].Key, (object) Main.player[plr].name), new Color((int) byte.MaxValue, 240, 20), plr);
        if (!Main.dedServ)
          return;
        Console.WriteLine(Lang.mp[19].Format((object) Main.player[plr].name));
      }
      else
      {
        int number2_3 = 0;
        NetMessage.SendData(14, ignoreClient: plr, number: plr, number2: ((float) number2_3));
        if (Netplay.Clients[plr].IsAnnouncementCompleted)
        {
          Netplay.Clients[plr].IsAnnouncementCompleted = false;
          ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.mp[20].Key, (object) Netplay.Clients[plr].Name), new Color((int) byte.MaxValue, 240, 20), plr);
          if (Main.dedServ)
            Console.WriteLine(Lang.mp[20].Format((object) Netplay.Clients[plr].Name));
          Netplay.Clients[plr].Name = "Anonymous";
        }
        Player.Hooks.PlayerDisconnect(plr);
      }
    }

    public struct NetSoundInfo
    {
      public Vector2 position;
      public ushort soundIndex;
      public int style;
      public float volume;
      public float pitchOffset;

      public NetSoundInfo(
        Vector2 position,
        ushort soundIndex,
        int style = -1,
        float volume = -1f,
        float pitchOffset = -1f)
      {
        this.position = position;
        this.soundIndex = soundIndex;
        this.style = style;
        this.volume = volume;
        this.pitchOffset = pitchOffset;
      }

      public void WriteSelfTo(BinaryWriter writer)
      {
        writer.WriteVector2(this.position);
        writer.Write(this.soundIndex);
        BitsByte bitsByte = new BitsByte(this.style != -1, (double) this.volume != -1.0, (double) this.pitchOffset != -1.0);
        writer.Write((byte) bitsByte);
        if (bitsByte[0])
          writer.Write(this.style);
        if (bitsByte[1])
          writer.Write(this.volume);
        if (!bitsByte[2])
          return;
        writer.Write(this.pitchOffset);
      }
    }
  }
}
