// Decompiled with JetBrains decompiler
// Type: Terraria.MessageBuffer
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;
using Terraria.GameContent.Golf;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Net;
using Terraria.Testing;
using Terraria.UI;

namespace Terraria
{
  public class MessageBuffer
  {
    public const int readBufferMax = 131070;
    public const int writeBufferMax = 131070;
    public bool broadcast;
    public byte[] readBuffer = new byte[131070];
    public byte[] writeBuffer = new byte[131070];
    public bool writeLocked;
    public int messageLength;
    public int totalData;
    public int whoAmI;
    public int spamCount;
    public int maxSpam;
    public bool checkBytes;
    public MemoryStream readerStream;
    public MemoryStream writerStream;
    public BinaryReader reader;
    public BinaryWriter writer;
    public PacketHistory History = new PacketHistory();

    public static event TileChangeReceivedEvent OnTileChangeReceived;

    public void Reset()
    {
      Array.Clear((Array) this.readBuffer, 0, this.readBuffer.Length);
      Array.Clear((Array) this.writeBuffer, 0, this.writeBuffer.Length);
      this.writeLocked = false;
      this.messageLength = 0;
      this.totalData = 0;
      this.spamCount = 0;
      this.broadcast = false;
      this.checkBytes = false;
      this.ResetReader();
      this.ResetWriter();
    }

    public void ResetReader()
    {
      if (this.readerStream != null)
        this.readerStream.Close();
      this.readerStream = new MemoryStream(this.readBuffer);
      this.reader = new BinaryReader((Stream) this.readerStream);
    }

    public void ResetWriter()
    {
      if (this.writerStream != null)
        this.writerStream.Close();
      this.writerStream = new MemoryStream(this.writeBuffer);
      this.writer = new BinaryWriter((Stream) this.writerStream);
    }

    public void GetData(int start, int length, out int messageType)
    {
      if (this.whoAmI < 256)
        Netplay.Clients[this.whoAmI].TimeOutTimer = 0;
      else
        Netplay.Connection.TimeOutTimer = 0;
      int bufferStart = start + 1;
      byte num1 = this.readBuffer[start];
      messageType = (int) num1;
      if (num1 >= (byte) 143)
        return;
      Main.ActiveNetDiagnosticsUI.CountReadMessage((int) num1, length);
      if (Main.netMode == 1 && Netplay.Connection.StatusMax > 0)
        ++Netplay.Connection.StatusCount;
      if (Main.verboseNetplay)
      {
        int num2 = start;
        while (num2 < start + length)
          ++num2;
        for (int index = start; index < start + length; ++index)
        {
          int num3 = (int) this.readBuffer[index];
        }
      }
      if (Main.netMode == 2 && num1 != (byte) 38 && Netplay.Clients[this.whoAmI].State == -1)
      {
        NetMessage.TrySendData(2, this.whoAmI, text: Lang.mp[1].ToNetworkText());
      }
      else
      {
        if (Main.netMode == 2)
        {
          if (Netplay.Clients[this.whoAmI].State < 10 && num1 > (byte) 12 && num1 != (byte) 93 && num1 != (byte) 16 && num1 != (byte) 42 && num1 != (byte) 50 && num1 != (byte) 38 && num1 != (byte) 68)
            NetMessage.BootPlayer(this.whoAmI, Lang.mp[2].ToNetworkText());
          if (Netplay.Clients[this.whoAmI].State == 0 && num1 != (byte) 1)
            NetMessage.BootPlayer(this.whoAmI, Lang.mp[2].ToNetworkText());
        }
        if (this.reader == null)
          this.ResetReader();
        this.reader.BaseStream.Position = (long) bufferStart;
        switch (num1)
        {
          case 1:
            if (Main.netMode != 2)
              break;
            if (Main.dedServ && Netplay.IsBanned(Netplay.Clients[this.whoAmI].Socket.GetRemoteAddress()))
            {
              NetMessage.TrySendData(2, this.whoAmI, text: Lang.mp[3].ToNetworkText());
              break;
            }
            if (Netplay.Clients[this.whoAmI].State != 0)
              break;
            if (this.reader.ReadString() == "Terraria" + 248.ToString())
            {
              if (string.IsNullOrEmpty(Netplay.ServerPassword))
              {
                Netplay.Clients[this.whoAmI].State = 1;
                NetMessage.TrySendData(3, this.whoAmI);
                break;
              }
              Netplay.Clients[this.whoAmI].State = -1;
              NetMessage.TrySendData(37, this.whoAmI);
              break;
            }
            NetMessage.TrySendData(2, this.whoAmI, text: Lang.mp[4].ToNetworkText());
            break;
          case 2:
            if (Main.netMode != 1)
              break;
            Netplay.Disconnect = true;
            Main.statusText = NetworkText.Deserialize(this.reader).ToString();
            break;
          case 3:
            if (Main.netMode != 1)
              break;
            if (Netplay.Connection.State == 1)
              Netplay.Connection.State = 2;
            int number1 = (int) this.reader.ReadByte();
            bool flag1 = this.reader.ReadBoolean();
            Netplay.Connection.ServerSpecialFlags[2] = flag1;
            if (number1 != Main.myPlayer)
            {
              Main.player[number1] = Main.ActivePlayerFileData.Player;
              Main.player[Main.myPlayer] = new Player();
            }
            Main.player[number1].whoAmI = number1;
            Main.myPlayer = number1;
            Player player1 = Main.player[number1];
            NetMessage.TrySendData(4, number: number1);
            NetMessage.TrySendData(68, number: number1);
            NetMessage.TrySendData(16, number: number1);
            NetMessage.TrySendData(42, number: number1);
            NetMessage.TrySendData(50, number: number1);
            for (int number2 = 0; number2 < 59; ++number2)
              NetMessage.TrySendData(5, number: number1, number2: ((float) number2), number3: ((float) player1.inventory[number2].prefix));
            for (int index = 0; index < player1.armor.Length; ++index)
              NetMessage.TrySendData(5, number: number1, number2: ((float) (59 + index)), number3: ((float) player1.armor[index].prefix));
            for (int index = 0; index < player1.dye.Length; ++index)
              NetMessage.TrySendData(5, number: number1, number2: ((float) (79 + index)), number3: ((float) player1.dye[index].prefix));
            for (int index = 0; index < player1.miscEquips.Length; ++index)
              NetMessage.TrySendData(5, number: number1, number2: ((float) (89 + index)), number3: ((float) player1.miscEquips[index].prefix));
            for (int index = 0; index < player1.miscDyes.Length; ++index)
              NetMessage.TrySendData(5, number: number1, number2: ((float) (94 + index)), number3: ((float) player1.miscDyes[index].prefix));
            for (int index = 0; index < player1.bank.item.Length; ++index)
              NetMessage.TrySendData(5, number: number1, number2: ((float) (99 + index)), number3: ((float) player1.bank.item[index].prefix));
            for (int index = 0; index < player1.bank2.item.Length; ++index)
              NetMessage.TrySendData(5, number: number1, number2: ((float) (139 + index)), number3: ((float) player1.bank2.item[index].prefix));
            NetMessage.TrySendData(5, number: number1, number2: 179f, number3: ((float) player1.trashItem.prefix));
            for (int index = 0; index < player1.bank3.item.Length; ++index)
              NetMessage.TrySendData(5, number: number1, number2: ((float) (180 + index)), number3: ((float) player1.bank3.item[index].prefix));
            for (int index = 0; index < player1.bank4.item.Length; ++index)
              NetMessage.TrySendData(5, number: number1, number2: ((float) (220 + index)), number3: ((float) player1.bank4.item[index].prefix));
            NetMessage.TrySendData(6);
            if (Netplay.Connection.State != 2)
              break;
            Netplay.Connection.State = 3;
            break;
          case 4:
            int number3 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number3 = this.whoAmI;
            if (number3 == Main.myPlayer && !Main.ServerSideCharacter)
              break;
            Player player2 = Main.player[number3];
            player2.whoAmI = number3;
            player2.skinVariant = (int) this.reader.ReadByte();
            player2.skinVariant = (int) MathHelper.Clamp((float) player2.skinVariant, 0.0f, 11f);
            player2.hair = (int) this.reader.ReadByte();
            if (player2.hair >= 165)
              player2.hair = 0;
            player2.name = this.reader.ReadString().Trim().Trim();
            player2.hairDye = this.reader.ReadByte();
            BitsByte bitsByte1 = (BitsByte) this.reader.ReadByte();
            for (int key = 0; key < 8; ++key)
              player2.hideVisibleAccessory[key] = bitsByte1[key];
            bitsByte1 = (BitsByte) this.reader.ReadByte();
            for (int key = 0; key < 2; ++key)
              player2.hideVisibleAccessory[key + 8] = bitsByte1[key];
            player2.hideMisc = (BitsByte) this.reader.ReadByte();
            player2.hairColor = this.reader.ReadRGB();
            player2.skinColor = this.reader.ReadRGB();
            player2.eyeColor = this.reader.ReadRGB();
            player2.shirtColor = this.reader.ReadRGB();
            player2.underShirtColor = this.reader.ReadRGB();
            player2.pantsColor = this.reader.ReadRGB();
            player2.shoeColor = this.reader.ReadRGB();
            BitsByte bitsByte2 = (BitsByte) this.reader.ReadByte();
            player2.difficulty = (byte) 0;
            if (bitsByte2[0])
              player2.difficulty = (byte) 1;
            if (bitsByte2[1])
              player2.difficulty = (byte) 2;
            if (bitsByte2[3])
              player2.difficulty = (byte) 3;
            if (player2.difficulty > (byte) 3)
              player2.difficulty = (byte) 3;
            player2.extraAccessory = bitsByte2[2];
            BitsByte bitsByte3 = (BitsByte) this.reader.ReadByte();
            player2.UsingBiomeTorches = bitsByte3[0];
            player2.happyFunTorchTime = bitsByte3[1];
            player2.unlockedBiomeTorches = bitsByte3[2];
            if (Main.netMode != 2)
              break;
            bool flag2 = false;
            if (Netplay.Clients[this.whoAmI].State < 10)
            {
              for (int index = 0; index < (int) byte.MaxValue; ++index)
              {
                if (index != number3 && player2.name == Main.player[index].name && Netplay.Clients[index].IsActive)
                  flag2 = true;
              }
            }
            if (flag2)
            {
              NetMessage.TrySendData(2, this.whoAmI, text: NetworkText.FromKey(Lang.mp[5].Key, (object) player2.name));
              break;
            }
            if (player2.name.Length > Player.nameLen)
            {
              NetMessage.TrySendData(2, this.whoAmI, text: NetworkText.FromKey("Net.NameTooLong"));
              break;
            }
            if (player2.name == "")
            {
              NetMessage.TrySendData(2, this.whoAmI, text: NetworkText.FromKey("Net.EmptyName"));
              break;
            }
            if (player2.difficulty == (byte) 3 && !Main.GameModeInfo.IsJourneyMode)
            {
              NetMessage.TrySendData(2, this.whoAmI, text: NetworkText.FromKey("Net.PlayerIsCreativeAndWorldIsNotCreative"));
              break;
            }
            if (player2.difficulty != (byte) 3 && Main.GameModeInfo.IsJourneyMode)
            {
              NetMessage.TrySendData(2, this.whoAmI, text: NetworkText.FromKey("Net.PlayerIsNotCreativeAndWorldIsCreative"));
              break;
            }
            Netplay.Clients[this.whoAmI].Name = player2.name;
            Netplay.Clients[this.whoAmI].Name = player2.name;
            NetMessage.TrySendData(4, ignoreClient: this.whoAmI, number: number3);
            break;
          case 5:
            int number4 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number4 = this.whoAmI;
            if (number4 == Main.myPlayer && !Main.ServerSideCharacter && !Main.player[number4].HasLockedInventory())
              break;
            Player player3 = Main.player[number4];
            lock (player3)
            {
              int number2 = (int) this.reader.ReadInt16();
              int num4 = (int) this.reader.ReadInt16();
              int num5 = (int) this.reader.ReadByte();
              int type1 = (int) this.reader.ReadInt16();
              Item[] objArray1 = (Item[]) null;
              Item[] objArray2 = (Item[]) null;
              int index = 0;
              bool flag3 = false;
              if (number2 >= 220)
              {
                index = number2 - 220;
                objArray1 = player3.bank4.item;
                objArray2 = Main.clientPlayer.bank4.item;
              }
              else if (number2 >= 180)
              {
                index = number2 - 180;
                objArray1 = player3.bank3.item;
                objArray2 = Main.clientPlayer.bank3.item;
              }
              else if (number2 >= 179)
                flag3 = true;
              else if (number2 >= 139)
              {
                index = number2 - 139;
                objArray1 = player3.bank2.item;
                objArray2 = Main.clientPlayer.bank2.item;
              }
              else if (number2 >= 99)
              {
                index = number2 - 99;
                objArray1 = player3.bank.item;
                objArray2 = Main.clientPlayer.bank.item;
              }
              else if (number2 >= 94)
              {
                index = number2 - 94;
                objArray1 = player3.miscDyes;
                objArray2 = Main.clientPlayer.miscDyes;
              }
              else if (number2 >= 89)
              {
                index = number2 - 89;
                objArray1 = player3.miscEquips;
                objArray2 = Main.clientPlayer.miscEquips;
              }
              else if (number2 >= 79)
              {
                index = number2 - 79;
                objArray1 = player3.dye;
                objArray2 = Main.clientPlayer.dye;
              }
              else if (number2 >= 59)
              {
                index = number2 - 59;
                objArray1 = player3.armor;
                objArray2 = Main.clientPlayer.armor;
              }
              else
              {
                index = number2;
                objArray1 = player3.inventory;
                objArray2 = Main.clientPlayer.inventory;
              }
              if (flag3)
              {
                player3.trashItem = new Item();
                player3.trashItem.netDefaults(type1);
                player3.trashItem.stack = num4;
                player3.trashItem.Prefix(num5);
                if (number4 == Main.myPlayer && !Main.ServerSideCharacter)
                  Main.clientPlayer.trashItem = player3.trashItem.Clone();
              }
              else if (number2 <= 58)
              {
                int type2 = objArray1[index].type;
                int stack = objArray1[index].stack;
                objArray1[index] = new Item();
                objArray1[index].netDefaults(type1);
                objArray1[index].stack = num4;
                objArray1[index].Prefix(num5);
                if (number4 == Main.myPlayer && !Main.ServerSideCharacter)
                  objArray2[index] = objArray1[index].Clone();
                if (number4 == Main.myPlayer && index == 58)
                  Main.mouseItem = objArray1[index].Clone();
                if (number4 == Main.myPlayer && Main.netMode == 1)
                {
                  Main.player[number4].inventoryChestStack[number2] = false;
                  if (objArray1[index].stack != stack || objArray1[index].type != type2)
                  {
                    Recipe.FindRecipes(true);
                    SoundEngine.PlaySound(7);
                  }
                }
              }
              else
              {
                objArray1[index] = new Item();
                objArray1[index].netDefaults(type1);
                objArray1[index].stack = num4;
                objArray1[index].Prefix(num5);
                if (number4 == Main.myPlayer && !Main.ServerSideCharacter)
                  objArray2[index] = objArray1[index].Clone();
              }
              if (Main.netMode != 2 || number4 != this.whoAmI || number2 > 58 + player3.armor.Length + player3.dye.Length + player3.miscEquips.Length + player3.miscDyes.Length)
                break;
              NetMessage.TrySendData(5, ignoreClient: this.whoAmI, number: number4, number2: ((float) number2), number3: ((float) num5));
              break;
            }
          case 6:
            if (Main.netMode != 2)
              break;
            if (Netplay.Clients[this.whoAmI].State == 1)
              Netplay.Clients[this.whoAmI].State = 2;
            NetMessage.TrySendData(7, this.whoAmI);
            Main.SyncAnInvasion(this.whoAmI);
            break;
          case 7:
            if (Main.netMode != 1)
              break;
            Main.time = (double) this.reader.ReadInt32();
            BitsByte bitsByte4 = (BitsByte) this.reader.ReadByte();
            Main.dayTime = bitsByte4[0];
            Main.bloodMoon = bitsByte4[1];
            Main.eclipse = bitsByte4[2];
            Main.moonPhase = (int) this.reader.ReadByte();
            Main.maxTilesX = (int) this.reader.ReadInt16();
            Main.maxTilesY = (int) this.reader.ReadInt16();
            Main.spawnTileX = (int) this.reader.ReadInt16();
            Main.spawnTileY = (int) this.reader.ReadInt16();
            Main.worldSurface = (double) this.reader.ReadInt16();
            Main.rockLayer = (double) this.reader.ReadInt16();
            Main.worldID = this.reader.ReadInt32();
            Main.worldName = this.reader.ReadString();
            Main.GameMode = (int) this.reader.ReadByte();
            Main.ActiveWorldFileData.UniqueId = new Guid(this.reader.ReadBytes(16));
            Main.ActiveWorldFileData.WorldGeneratorVersion = this.reader.ReadUInt64();
            Main.moonType = (int) this.reader.ReadByte();
            WorldGen.setBG(0, (int) this.reader.ReadByte());
            WorldGen.setBG(10, (int) this.reader.ReadByte());
            WorldGen.setBG(11, (int) this.reader.ReadByte());
            WorldGen.setBG(12, (int) this.reader.ReadByte());
            WorldGen.setBG(1, (int) this.reader.ReadByte());
            WorldGen.setBG(2, (int) this.reader.ReadByte());
            WorldGen.setBG(3, (int) this.reader.ReadByte());
            WorldGen.setBG(4, (int) this.reader.ReadByte());
            WorldGen.setBG(5, (int) this.reader.ReadByte());
            WorldGen.setBG(6, (int) this.reader.ReadByte());
            WorldGen.setBG(7, (int) this.reader.ReadByte());
            WorldGen.setBG(8, (int) this.reader.ReadByte());
            WorldGen.setBG(9, (int) this.reader.ReadByte());
            Main.iceBackStyle = (int) this.reader.ReadByte();
            Main.jungleBackStyle = (int) this.reader.ReadByte();
            Main.hellBackStyle = (int) this.reader.ReadByte();
            Main.windSpeedTarget = this.reader.ReadSingle();
            Main.numClouds = (int) this.reader.ReadByte();
            for (int index = 0; index < 3; ++index)
              Main.treeX[index] = this.reader.ReadInt32();
            for (int index = 0; index < 4; ++index)
              Main.treeStyle[index] = (int) this.reader.ReadByte();
            for (int index = 0; index < 3; ++index)
              Main.caveBackX[index] = this.reader.ReadInt32();
            for (int index = 0; index < 4; ++index)
              Main.caveBackStyle[index] = (int) this.reader.ReadByte();
            WorldGen.TreeTops.SyncReceive(this.reader);
            WorldGen.BackgroundsCache.UpdateCache();
            Main.maxRaining = this.reader.ReadSingle();
            Main.raining = (double) Main.maxRaining > 0.0;
            BitsByte bitsByte5 = (BitsByte) this.reader.ReadByte();
            WorldGen.shadowOrbSmashed = bitsByte5[0];
            NPC.downedBoss1 = bitsByte5[1];
            NPC.downedBoss2 = bitsByte5[2];
            NPC.downedBoss3 = bitsByte5[3];
            Main.hardMode = bitsByte5[4];
            NPC.downedClown = bitsByte5[5];
            Main.ServerSideCharacter = bitsByte5[6];
            NPC.downedPlantBoss = bitsByte5[7];
            if (Main.ServerSideCharacter)
              Main.ActivePlayerFileData.MarkAsServerSide();
            BitsByte bitsByte6 = (BitsByte) this.reader.ReadByte();
            NPC.downedMechBoss1 = bitsByte6[0];
            NPC.downedMechBoss2 = bitsByte6[1];
            NPC.downedMechBoss3 = bitsByte6[2];
            NPC.downedMechBossAny = bitsByte6[3];
            Main.cloudBGActive = bitsByte6[4] ? 1f : 0.0f;
            WorldGen.crimson = bitsByte6[5];
            Main.pumpkinMoon = bitsByte6[6];
            Main.snowMoon = bitsByte6[7];
            BitsByte bitsByte7 = (BitsByte) this.reader.ReadByte();
            Main.fastForwardTime = bitsByte7[1];
            Main.UpdateTimeRate();
            int num6 = bitsByte7[2] ? 1 : 0;
            NPC.downedSlimeKing = bitsByte7[3];
            NPC.downedQueenBee = bitsByte7[4];
            NPC.downedFishron = bitsByte7[5];
            NPC.downedMartians = bitsByte7[6];
            NPC.downedAncientCultist = bitsByte7[7];
            BitsByte bitsByte8 = (BitsByte) this.reader.ReadByte();
            NPC.downedMoonlord = bitsByte8[0];
            NPC.downedHalloweenKing = bitsByte8[1];
            NPC.downedHalloweenTree = bitsByte8[2];
            NPC.downedChristmasIceQueen = bitsByte8[3];
            NPC.downedChristmasSantank = bitsByte8[4];
            NPC.downedChristmasTree = bitsByte8[5];
            NPC.downedGolemBoss = bitsByte8[6];
            BirthdayParty.ManualParty = bitsByte8[7];
            BitsByte bitsByte9 = (BitsByte) this.reader.ReadByte();
            NPC.downedPirates = bitsByte9[0];
            NPC.downedFrost = bitsByte9[1];
            NPC.downedGoblins = bitsByte9[2];
            Sandstorm.Happening = bitsByte9[3];
            DD2Event.Ongoing = bitsByte9[4];
            DD2Event.DownedInvasionT1 = bitsByte9[5];
            DD2Event.DownedInvasionT2 = bitsByte9[6];
            DD2Event.DownedInvasionT3 = bitsByte9[7];
            BitsByte bitsByte10 = (BitsByte) this.reader.ReadByte();
            NPC.combatBookWasUsed = bitsByte10[0];
            LanternNight.ManualLanterns = bitsByte10[1];
            NPC.downedTowerSolar = bitsByte10[2];
            NPC.downedTowerVortex = bitsByte10[3];
            NPC.downedTowerNebula = bitsByte10[4];
            NPC.downedTowerStardust = bitsByte10[5];
            Main.forceHalloweenForToday = bitsByte10[6];
            Main.forceXMasForToday = bitsByte10[7];
            BitsByte bitsByte11 = (BitsByte) this.reader.ReadByte();
            NPC.boughtCat = bitsByte11[0];
            NPC.boughtDog = bitsByte11[1];
            NPC.boughtBunny = bitsByte11[2];
            NPC.freeCake = bitsByte11[3];
            Main.drunkWorld = bitsByte11[4];
            NPC.downedEmpressOfLight = bitsByte11[5];
            NPC.downedQueenSlime = bitsByte11[6];
            Main.getGoodWorld = bitsByte11[7];
            BitsByte bitsByte12 = (BitsByte) this.reader.ReadByte();
            Main.tenthAnniversaryWorld = bitsByte12[0];
            Main.dontStarveWorld = bitsByte12[1];
            NPC.downedDeerclops = bitsByte12[2];
            Main.notTheBeesWorld = bitsByte12[3];
            WorldGen.SavedOreTiers.Copper = (int) this.reader.ReadInt16();
            WorldGen.SavedOreTiers.Iron = (int) this.reader.ReadInt16();
            WorldGen.SavedOreTiers.Silver = (int) this.reader.ReadInt16();
            WorldGen.SavedOreTiers.Gold = (int) this.reader.ReadInt16();
            WorldGen.SavedOreTiers.Cobalt = (int) this.reader.ReadInt16();
            WorldGen.SavedOreTiers.Mythril = (int) this.reader.ReadInt16();
            WorldGen.SavedOreTiers.Adamantite = (int) this.reader.ReadInt16();
            if (num6 != 0)
              Main.StartSlimeRain();
            else
              Main.StopSlimeRain();
            Main.invasionType = (int) this.reader.ReadSByte();
            Main.LobbyId = this.reader.ReadUInt64();
            Sandstorm.IntendedSeverity = this.reader.ReadSingle();
            if (Netplay.Connection.State == 3)
            {
              Main.windSpeedCurrent = Main.windSpeedTarget;
              Netplay.Connection.State = 4;
            }
            Main.checkHalloween();
            Main.checkXMas();
            break;
          case 8:
            if (Main.netMode != 2)
              break;
            int num7 = this.reader.ReadInt32();
            int num8 = this.reader.ReadInt32();
            bool flag4 = true;
            if (num7 == -1 || num8 == -1)
              flag4 = false;
            else if (num7 < 10 || num7 > Main.maxTilesX - 10)
              flag4 = false;
            else if (num8 < 10 || num8 > Main.maxTilesY - 10)
              flag4 = false;
            int number5 = Netplay.GetSectionX(Main.spawnTileX) - 2;
            int number2_1 = Netplay.GetSectionY(Main.spawnTileY) - 1;
            int num9 = number5 + 5;
            int num10 = number2_1 + 3;
            if (number5 < 0)
              number5 = 0;
            if (num9 >= Main.maxSectionsX)
              num9 = Main.maxSectionsX - 1;
            if (number2_1 < 0)
              number2_1 = 0;
            if (num10 >= Main.maxSectionsY)
              num10 = Main.maxSectionsY - 1;
            int num11 = (num9 - number5) * (num10 - number2_1);
            List<Point> dontInclude = new List<Point>();
            for (int x = number5; x < num9; ++x)
            {
              for (int y = number2_1; y < num10; ++y)
                dontInclude.Add(new Point(x, y));
            }
            int number3_1 = -1;
            int number4_1 = -1;
            if (flag4)
            {
              num7 = Netplay.GetSectionX(num7) - 2;
              num8 = Netplay.GetSectionY(num8) - 1;
              number3_1 = num7 + 5;
              number4_1 = num8 + 3;
              if (num7 < 0)
                num7 = 0;
              if (number3_1 >= Main.maxSectionsX)
                number3_1 = Main.maxSectionsX - 1;
              if (num8 < 0)
                num8 = 0;
              if (number4_1 >= Main.maxSectionsY)
                number4_1 = Main.maxSectionsY - 1;
              for (int x = num7; x <= number3_1; ++x)
              {
                for (int y = num8; y <= number4_1; ++y)
                {
                  if (x < number5 || x >= num9 || y < number2_1 || y >= num10)
                  {
                    dontInclude.Add(new Point(x, y));
                    ++num11;
                  }
                }
              }
            }
            int num12 = 1;
            List<Point> portals;
            List<Point> portalCenters;
            PortalHelper.SyncPortalsOnPlayerJoin(this.whoAmI, 1, dontInclude, out portals, out portalCenters);
            int number6 = num11 + portals.Count;
            if (Netplay.Clients[this.whoAmI].State == 2)
              Netplay.Clients[this.whoAmI].State = 3;
            NetMessage.TrySendData(9, this.whoAmI, text: Lang.inter[44].ToNetworkText(), number: number6);
            Netplay.Clients[this.whoAmI].StatusText2 = Language.GetTextValue("Net.IsReceivingTileData");
            Netplay.Clients[this.whoAmI].StatusMax += number6;
            for (int sectionX = number5; sectionX < num9; ++sectionX)
            {
              for (int sectionY = number2_1; sectionY < num10; ++sectionY)
                NetMessage.SendSection(this.whoAmI, sectionX, sectionY);
            }
            NetMessage.TrySendData(11, this.whoAmI, number: number5, number2: ((float) number2_1), number3: ((float) (num9 - 1)), number4: ((float) (num10 - 1)));
            if (flag4)
            {
              for (int sectionX = num7; sectionX <= number3_1; ++sectionX)
              {
                for (int sectionY = num8; sectionY <= number4_1; ++sectionY)
                  NetMessage.SendSection(this.whoAmI, sectionX, sectionY, true);
              }
              NetMessage.TrySendData(11, this.whoAmI, number: num7, number2: ((float) num8), number3: ((float) number3_1), number4: ((float) number4_1));
            }
            for (int index = 0; index < portals.Count; ++index)
              NetMessage.SendSection(this.whoAmI, portals[index].X, portals[index].Y, true);
            for (int index = 0; index < portalCenters.Count; ++index)
              NetMessage.TrySendData(11, this.whoAmI, number: (portalCenters[index].X - num12), number2: ((float) (portalCenters[index].Y - num12)), number3: ((float) (portalCenters[index].X + num12 + 1)), number4: ((float) (portalCenters[index].Y + num12 + 1)));
            for (int number7 = 0; number7 < 400; ++number7)
            {
              if (Main.item[number7].active)
              {
                NetMessage.TrySendData(21, this.whoAmI, number: number7);
                NetMessage.TrySendData(22, this.whoAmI, number: number7);
              }
            }
            for (int number8 = 0; number8 < 200; ++number8)
            {
              if (Main.npc[number8].active)
                NetMessage.TrySendData(23, this.whoAmI, number: number8);
            }
            for (int number9 = 0; number9 < 1000; ++number9)
            {
              if (Main.projectile[number9].active && (Main.projPet[Main.projectile[number9].type] || Main.projectile[number9].netImportant))
                NetMessage.TrySendData(27, this.whoAmI, number: number9);
            }
            for (int number10 = 0; number10 < 289; ++number10)
              NetMessage.TrySendData(83, this.whoAmI, number: number10);
            NetMessage.TrySendData(49, this.whoAmI);
            NetMessage.TrySendData(57, this.whoAmI);
            NetMessage.TrySendData(7, this.whoAmI);
            NetMessage.TrySendData(103, number: NPC.MoonLordCountdown);
            NetMessage.TrySendData(101, this.whoAmI);
            NetMessage.TrySendData(136, this.whoAmI);
            Main.BestiaryTracker.OnPlayerJoining(this.whoAmI);
            CreativePowerManager.Instance.SyncThingsToJoiningPlayer(this.whoAmI);
            Main.PylonSystem.OnPlayerJoining(this.whoAmI);
            break;
          case 9:
            if (Main.netMode != 1)
              break;
            Netplay.Connection.StatusMax += this.reader.ReadInt32();
            Netplay.Connection.StatusText = NetworkText.Deserialize(this.reader).ToString();
            BitsByte bitsByte13 = (BitsByte) this.reader.ReadByte();
            BitsByte serverSpecialFlags = Netplay.Connection.ServerSpecialFlags;
            serverSpecialFlags[0] = bitsByte13[0];
            serverSpecialFlags[1] = bitsByte13[1];
            Netplay.Connection.ServerSpecialFlags = serverSpecialFlags;
            break;
          case 10:
            if (Main.netMode != 1)
              break;
            NetMessage.DecompressTileBlock(this.readBuffer, bufferStart, length);
            break;
          case 11:
            if (Main.netMode != 1)
              break;
            WorldGen.SectionTileFrame((int) this.reader.ReadInt16(), (int) this.reader.ReadInt16(), (int) this.reader.ReadInt16(), (int) this.reader.ReadInt16());
            break;
          case 12:
            int index1 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              index1 = this.whoAmI;
            Player player4 = Main.player[index1];
            player4.SpawnX = (int) this.reader.ReadInt16();
            player4.SpawnY = (int) this.reader.ReadInt16();
            player4.respawnTimer = this.reader.ReadInt32();
            if (player4.respawnTimer > 0)
              player4.dead = true;
            PlayerSpawnContext playerSpawnContext = (PlayerSpawnContext) this.reader.ReadByte();
            player4.Spawn(playerSpawnContext);
            if (index1 == Main.myPlayer && Main.netMode != 2)
            {
              Main.ActivePlayerFileData.StartPlayTimer();
              Player.Hooks.EnterWorld(Main.myPlayer);
            }
            if (Main.netMode != 2 || Netplay.Clients[this.whoAmI].State < 3)
              break;
            if (Netplay.Clients[this.whoAmI].State == 3)
            {
              Netplay.Clients[this.whoAmI].State = 10;
              NetMessage.buffer[this.whoAmI].broadcast = true;
              NetMessage.SyncConnectedPlayer(this.whoAmI);
              bool flag5 = NetMessage.DoesPlayerSlotCountAsAHost(this.whoAmI);
              Main.countsAsHostForGameplay[this.whoAmI] = flag5;
              if (NetMessage.DoesPlayerSlotCountAsAHost(this.whoAmI))
                NetMessage.TrySendData(139, this.whoAmI, number: this.whoAmI, number2: ((float) flag5.ToInt()));
              NetMessage.TrySendData(12, ignoreClient: this.whoAmI, number: this.whoAmI, number2: ((float) (byte) playerSpawnContext));
              NetMessage.TrySendData(74, this.whoAmI, text: NetworkText.FromLiteral(Main.player[this.whoAmI].name), number: Main.anglerQuest);
              NetMessage.TrySendData(129, this.whoAmI);
              NetMessage.greetPlayer(this.whoAmI);
              if (!Main.player[index1].unlockedBiomeTorches)
                break;
              NPC npc = new NPC();
              npc.SetDefaults(664);
              Main.BestiaryTracker.Kills.RegisterKill(npc);
              break;
            }
            NetMessage.TrySendData(12, ignoreClient: this.whoAmI, number: this.whoAmI, number2: ((float) (byte) playerSpawnContext));
            break;
          case 13:
            int number11 = (int) this.reader.ReadByte();
            if (number11 == Main.myPlayer && !Main.ServerSideCharacter)
              break;
            if (Main.netMode == 2)
              number11 = this.whoAmI;
            Player player5 = Main.player[number11];
            BitsByte bitsByte14 = (BitsByte) this.reader.ReadByte();
            BitsByte bitsByte15 = (BitsByte) this.reader.ReadByte();
            BitsByte bitsByte16 = (BitsByte) this.reader.ReadByte();
            BitsByte bitsByte17 = (BitsByte) this.reader.ReadByte();
            player5.controlUp = bitsByte14[0];
            player5.controlDown = bitsByte14[1];
            player5.controlLeft = bitsByte14[2];
            player5.controlRight = bitsByte14[3];
            player5.controlJump = bitsByte14[4];
            player5.controlUseItem = bitsByte14[5];
            player5.direction = bitsByte14[6] ? 1 : -1;
            if (bitsByte15[0])
            {
              player5.pulley = true;
              player5.pulleyDir = bitsByte15[1] ? (byte) 2 : (byte) 1;
            }
            else
              player5.pulley = false;
            player5.vortexStealthActive = bitsByte15[3];
            player5.gravDir = bitsByte15[4] ? 1f : -1f;
            player5.TryTogglingShield(bitsByte15[5]);
            player5.ghost = bitsByte15[6];
            player5.selectedItem = (int) this.reader.ReadByte();
            player5.position = this.reader.ReadVector2();
            if (bitsByte15[2])
              player5.velocity = this.reader.ReadVector2();
            else
              player5.velocity = Vector2.Zero;
            if (bitsByte16[6])
            {
              player5.PotionOfReturnOriginalUsePosition = new Vector2?(this.reader.ReadVector2());
              player5.PotionOfReturnHomePosition = new Vector2?(this.reader.ReadVector2());
            }
            else
            {
              player5.PotionOfReturnOriginalUsePosition = new Vector2?();
              player5.PotionOfReturnHomePosition = new Vector2?();
            }
            player5.tryKeepingHoveringUp = bitsByte16[0];
            player5.IsVoidVaultEnabled = bitsByte16[1];
            player5.sitting.isSitting = bitsByte16[2];
            player5.downedDD2EventAnyDifficulty = bitsByte16[3];
            player5.isPettingAnimal = bitsByte16[4];
            player5.isTheAnimalBeingPetSmall = bitsByte16[5];
            player5.tryKeepingHoveringDown = bitsByte16[7];
            player5.sleeping.SetIsSleepingAndAdjustPlayerRotation(player5, bitsByte17[0]);
            if (Main.netMode != 2 || Netplay.Clients[this.whoAmI].State != 10)
              break;
            NetMessage.TrySendData(13, ignoreClient: this.whoAmI, number: number11);
            break;
          case 14:
            int playerIndex = (int) this.reader.ReadByte();
            int num13 = (int) this.reader.ReadByte();
            if (Main.netMode != 1)
              break;
            int num14 = Main.player[playerIndex].active ? 1 : 0;
            if (num13 == 1)
            {
              if (!Main.player[playerIndex].active)
                Main.player[playerIndex] = new Player();
              Main.player[playerIndex].active = true;
            }
            else
              Main.player[playerIndex].active = false;
            int num15 = Main.player[playerIndex].active ? 1 : 0;
            if (num14 == num15)
              break;
            if (Main.player[playerIndex].active)
            {
              Player.Hooks.PlayerConnect(playerIndex);
              break;
            }
            Player.Hooks.PlayerDisconnect(playerIndex);
            break;
          case 15:
            break;
          case 16:
            int number12 = (int) this.reader.ReadByte();
            if (number12 == Main.myPlayer && !Main.ServerSideCharacter)
              break;
            if (Main.netMode == 2)
              number12 = this.whoAmI;
            Player player6 = Main.player[number12];
            player6.statLife = (int) this.reader.ReadInt16();
            player6.statLifeMax = (int) this.reader.ReadInt16();
            if (player6.statLifeMax < 100)
              player6.statLifeMax = 100;
            player6.dead = player6.statLife <= 0;
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(16, ignoreClient: this.whoAmI, number: number12);
            break;
          case 17:
            byte number13 = this.reader.ReadByte();
            int index2 = (int) this.reader.ReadInt16();
            int index3 = (int) this.reader.ReadInt16();
            short index4 = this.reader.ReadInt16();
            int num16 = (int) this.reader.ReadByte();
            bool fail = index4 == (short) 1;
            if (!WorldGen.InWorld(index2, index3, 3))
              break;
            if (Main.tile[index2, index3] == null)
              Main.tile[index2, index3] = new Tile();
            if (Main.netMode == 2)
            {
              if (!fail)
              {
                if (number13 == (byte) 0 || number13 == (byte) 2 || number13 == (byte) 4)
                  ++Netplay.Clients[this.whoAmI].SpamDeleteBlock;
                if (number13 == (byte) 1 || number13 == (byte) 3)
                  ++Netplay.Clients[this.whoAmI].SpamAddBlock;
              }
              if (!Netplay.Clients[this.whoAmI].TileSections[Netplay.GetSectionX(index2), Netplay.GetSectionY(index3)])
                fail = true;
            }
            if (number13 == (byte) 0)
            {
              WorldGen.KillTile(index2, index3, fail);
              if (Main.netMode == 1 && !fail)
                HitTile.ClearAllTilesAtThisLocation(index2, index3);
            }
            if (number13 == (byte) 1)
              WorldGen.PlaceTile(index2, index3, (int) index4, forced: true, style: num16);
            if (number13 == (byte) 2)
              WorldGen.KillWall(index2, index3, fail);
            if (number13 == (byte) 3)
              WorldGen.PlaceWall(index2, index3, (int) index4);
            if (number13 == (byte) 4)
              WorldGen.KillTile(index2, index3, fail, noItem: true);
            if (number13 == (byte) 5)
              WorldGen.PlaceWire(index2, index3);
            if (number13 == (byte) 6)
              WorldGen.KillWire(index2, index3);
            if (number13 == (byte) 7)
              WorldGen.PoundTile(index2, index3);
            if (number13 == (byte) 8)
              WorldGen.PlaceActuator(index2, index3);
            if (number13 == (byte) 9)
              WorldGen.KillActuator(index2, index3);
            if (number13 == (byte) 10)
              WorldGen.PlaceWire2(index2, index3);
            if (number13 == (byte) 11)
              WorldGen.KillWire2(index2, index3);
            if (number13 == (byte) 12)
              WorldGen.PlaceWire3(index2, index3);
            if (number13 == (byte) 13)
              WorldGen.KillWire3(index2, index3);
            if (number13 == (byte) 14)
              WorldGen.SlopeTile(index2, index3, (int) index4);
            if (number13 == (byte) 15)
              Minecart.FrameTrack(index2, index3, true);
            if (number13 == (byte) 16)
              WorldGen.PlaceWire4(index2, index3);
            if (number13 == (byte) 17)
              WorldGen.KillWire4(index2, index3);
            if (number13 == (byte) 18)
            {
              Wiring.SetCurrentUser(this.whoAmI);
              Wiring.PokeLogicGate(index2, index3);
              Wiring.SetCurrentUser();
              break;
            }
            if (number13 == (byte) 19)
            {
              Wiring.SetCurrentUser(this.whoAmI);
              Wiring.Actuate(index2, index3);
              Wiring.SetCurrentUser();
              break;
            }
            if (number13 == (byte) 20)
            {
              if (!WorldGen.InWorld(index2, index3, 2))
                break;
              int type = (int) Main.tile[index2, index3].type;
              WorldGen.KillTile(index2, index3, fail);
              short number4_2 = !Main.tile[index2, index3].active() || (int) Main.tile[index2, index3].type != type ? (short) 0 : (short) 1;
              if (Main.netMode != 2)
                break;
              NetMessage.TrySendData(17, number: ((int) number13), number2: ((float) index2), number3: ((float) index3), number4: ((float) number4_2), number5: num16);
              break;
            }
            if (number13 == (byte) 21)
              WorldGen.ReplaceTile(index2, index3, (ushort) index4, num16);
            if (number13 == (byte) 22)
              WorldGen.ReplaceWall(index2, index3, (ushort) index4);
            if (number13 == (byte) 23)
            {
              WorldGen.SlopeTile(index2, index3, (int) index4);
              WorldGen.PoundTile(index2, index3);
            }
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(17, ignoreClient: this.whoAmI, number: ((int) number13), number2: ((float) index2), number3: ((float) index3), number4: ((float) index4), number5: num16);
            if (number13 != (byte) 1 && number13 != (byte) 21 || !TileID.Sets.Falling[(int) index4])
              break;
            NetMessage.SendTileSquare(-1, index2, index3);
            break;
          case 18:
            if (Main.netMode != 1)
              break;
            Main.dayTime = this.reader.ReadByte() == (byte) 1;
            Main.time = (double) this.reader.ReadInt32();
            Main.sunModY = this.reader.ReadInt16();
            Main.moonModY = this.reader.ReadInt16();
            break;
          case 19:
            byte number14 = this.reader.ReadByte();
            int num17 = (int) this.reader.ReadInt16();
            int num18 = (int) this.reader.ReadInt16();
            if (!WorldGen.InWorld(num17, num18, 3))
              break;
            int direction1 = this.reader.ReadByte() == (byte) 0 ? -1 : 1;
            switch (number14)
            {
              case 0:
                WorldGen.OpenDoor(num17, num18, direction1);
                break;
              case 1:
                WorldGen.CloseDoor(num17, num18, true);
                break;
              case 2:
                WorldGen.ShiftTrapdoor(num17, num18, direction1 == 1, 1);
                break;
              case 3:
                WorldGen.ShiftTrapdoor(num17, num18, direction1 == 1, 0);
                break;
              case 4:
                WorldGen.ShiftTallGate(num17, num18, false, true);
                break;
              case 5:
                WorldGen.ShiftTallGate(num17, num18, true, true);
                break;
            }
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(19, ignoreClient: this.whoAmI, number: ((int) number14), number2: ((float) num17), number3: ((float) num18), number4: (direction1 == 1 ? 1f : 0.0f));
            break;
          case 20:
            int num19 = (int) this.reader.ReadInt16();
            int num20 = (int) this.reader.ReadInt16();
            ushort num21 = (ushort) this.reader.ReadByte();
            ushort num22 = (ushort) this.reader.ReadByte();
            byte number5_1 = this.reader.ReadByte();
            if (!WorldGen.InWorld(num19, num20, 3))
              break;
            TileChangeType type3 = TileChangeType.None;
            if (Enum.IsDefined(typeof (TileChangeType), (object) number5_1))
              type3 = (TileChangeType) number5_1;
            if (MessageBuffer.OnTileChangeReceived != null)
              MessageBuffer.OnTileChangeReceived(num19, num20, (int) Math.Max(num21, num22), type3);
            BitsByte bitsByte18 = (BitsByte) (byte) 0;
            BitsByte bitsByte19 = (BitsByte) (byte) 0;
            for (int index5 = num19; index5 < num19 + (int) num21; ++index5)
            {
              for (int index6 = num20; index6 < num20 + (int) num22; ++index6)
              {
                if (Main.tile[index5, index6] == null)
                  Main.tile[index5, index6] = new Tile();
                Tile tile = Main.tile[index5, index6];
                bool flag6 = tile.active();
                BitsByte bitsByte20 = (BitsByte) this.reader.ReadByte();
                BitsByte bitsByte21 = (BitsByte) this.reader.ReadByte();
                tile.active(bitsByte20[0]);
                tile.wall = bitsByte20[2] ? (ushort) 1 : (ushort) 0;
                bool flag7 = bitsByte20[3];
                if (Main.netMode != 2)
                  tile.liquid = flag7 ? (byte) 1 : (byte) 0;
                tile.wire(bitsByte20[4]);
                tile.halfBrick(bitsByte20[5]);
                tile.actuator(bitsByte20[6]);
                tile.inActive(bitsByte20[7]);
                tile.wire2(bitsByte21[0]);
                tile.wire3(bitsByte21[1]);
                if (bitsByte21[2])
                  tile.color(this.reader.ReadByte());
                if (bitsByte21[3])
                  tile.wallColor(this.reader.ReadByte());
                if (tile.active())
                {
                  int type4 = (int) tile.type;
                  tile.type = this.reader.ReadUInt16();
                  if (Main.tileFrameImportant[(int) tile.type])
                  {
                    tile.frameX = this.reader.ReadInt16();
                    tile.frameY = this.reader.ReadInt16();
                  }
                  else if (!flag6 || (int) tile.type != type4)
                  {
                    tile.frameX = (short) -1;
                    tile.frameY = (short) -1;
                  }
                  byte slope = 0;
                  if (bitsByte21[4])
                    ++slope;
                  if (bitsByte21[5])
                    slope += (byte) 2;
                  if (bitsByte21[6])
                    slope += (byte) 4;
                  tile.slope(slope);
                }
                tile.wire4(bitsByte21[7]);
                if (tile.wall > (ushort) 0)
                  tile.wall = this.reader.ReadUInt16();
                if (flag7)
                {
                  tile.liquid = this.reader.ReadByte();
                  tile.liquidType((int) this.reader.ReadByte());
                }
              }
            }
            WorldGen.RangeFrame(num19, num20, num19 + (int) num21, num20 + (int) num22);
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData((int) num1, ignoreClient: this.whoAmI, number: num19, number2: ((float) num20), number3: ((float) num21), number4: ((float) num22), number5: ((int) number5_1));
            break;
          case 21:
          case 90:
            int index7 = (int) this.reader.ReadInt16();
            Vector2 vector2_1 = this.reader.ReadVector2();
            Vector2 vector2_2 = this.reader.ReadVector2();
            int Stack = (int) this.reader.ReadInt16();
            int pre1 = (int) this.reader.ReadByte();
            int num23 = (int) this.reader.ReadByte();
            int type5 = (int) this.reader.ReadInt16();
            if (Main.netMode == 1)
            {
              if (type5 == 0)
              {
                Main.item[index7].active = false;
                break;
              }
              int index8 = index7;
              Item obj = Main.item[index8];
              ItemSyncPersistentStats syncPersistentStats = new ItemSyncPersistentStats();
              syncPersistentStats.CopyFrom(obj);
              bool flag8 = (obj.newAndShiny || obj.netID != type5) && ItemSlot.Options.HighlightNewItems && (type5 < 0 || type5 >= 5125 || !ItemID.Sets.NeverAppearsAsNewInInventory[type5]);
              obj.netDefaults(type5);
              obj.newAndShiny = flag8;
              obj.Prefix(pre1);
              obj.stack = Stack;
              obj.position = vector2_1;
              obj.velocity = vector2_2;
              obj.active = true;
              if (num1 == (byte) 90)
              {
                obj.instanced = true;
                obj.playerIndexTheItemIsReservedFor = Main.myPlayer;
                obj.keepTime = 600;
              }
              obj.wet = Collision.WetCollision(obj.position, obj.width, obj.height);
              syncPersistentStats.PasteInto(obj);
              break;
            }
            if (Main.timeItemSlotCannotBeReusedFor[index7] > 0)
              break;
            if (type5 == 0)
            {
              if (index7 >= 400)
                break;
              Main.item[index7].active = false;
              NetMessage.TrySendData(21, number: index7);
              break;
            }
            bool flag9 = false;
            if (index7 == 400)
              flag9 = true;
            if (flag9)
            {
              Item obj = new Item();
              obj.netDefaults(type5);
              index7 = Item.NewItem((IEntitySource) new EntitySource_Sync(), (int) vector2_1.X, (int) vector2_1.Y, obj.width, obj.height, obj.type, Stack, true);
            }
            Item obj1 = Main.item[index7];
            obj1.netDefaults(type5);
            obj1.Prefix(pre1);
            obj1.stack = Stack;
            obj1.position = vector2_1;
            obj1.velocity = vector2_2;
            obj1.active = true;
            obj1.playerIndexTheItemIsReservedFor = Main.myPlayer;
            if (flag9)
            {
              NetMessage.TrySendData(21, number: index7);
              if (num23 == 0)
              {
                Main.item[index7].ownIgnore = this.whoAmI;
                Main.item[index7].ownTime = 100;
              }
              Main.item[index7].FindOwner(index7);
              break;
            }
            NetMessage.TrySendData(21, ignoreClient: this.whoAmI, number: index7);
            break;
          case 22:
            int number15 = (int) this.reader.ReadInt16();
            int num24 = (int) this.reader.ReadByte();
            if (Main.netMode == 2 && Main.item[number15].playerIndexTheItemIsReservedFor != this.whoAmI)
              break;
            Main.item[number15].playerIndexTheItemIsReservedFor = num24;
            Main.item[number15].keepTime = num24 != Main.myPlayer ? 0 : 15;
            if (Main.netMode != 2)
              break;
            Main.item[number15].playerIndexTheItemIsReservedFor = (int) byte.MaxValue;
            Main.item[number15].keepTime = 15;
            NetMessage.TrySendData(22, number: number15);
            break;
          case 23:
            if (Main.netMode != 1)
              break;
            int index9 = (int) this.reader.ReadInt16();
            Vector2 vector2_3 = this.reader.ReadVector2();
            Vector2 vector2_4 = this.reader.ReadVector2();
            int num25 = (int) this.reader.ReadUInt16();
            if (num25 == (int) ushort.MaxValue)
              num25 = 0;
            BitsByte bitsByte22 = (BitsByte) this.reader.ReadByte();
            BitsByte bitsByte23 = (BitsByte) this.reader.ReadByte();
            float[] numArray1 = new float[NPC.maxAI];
            for (int index10 = 0; index10 < NPC.maxAI; ++index10)
              numArray1[index10] = !bitsByte22[index10 + 2] ? 0.0f : this.reader.ReadSingle();
            int Type1 = (int) this.reader.ReadInt16();
            int? nullable = new int?(1);
            if (bitsByte23[0])
              nullable = new int?((int) this.reader.ReadByte());
            float num26 = 1f;
            if (bitsByte23[2])
              num26 = this.reader.ReadSingle();
            int num27 = 0;
            if (!bitsByte22[7])
            {
              switch (this.reader.ReadByte())
              {
                case 2:
                  num27 = (int) this.reader.ReadInt16();
                  break;
                case 4:
                  num27 = this.reader.ReadInt32();
                  break;
                default:
                  num27 = (int) this.reader.ReadSByte();
                  break;
              }
            }
            int oldType = -1;
            NPC npc1 = Main.npc[index9];
            if (npc1.active && Main.multiplayerNPCSmoothingRange > 0 && (double) Vector2.DistanceSquared(npc1.position, vector2_3) < 640000.0)
              npc1.netOffset += npc1.position - vector2_3;
            if (!npc1.active || npc1.netID != Type1)
            {
              npc1.netOffset *= 0.0f;
              if (npc1.active)
                oldType = npc1.type;
              npc1.active = true;
              npc1.SetDefaults(Type1);
            }
            npc1.position = vector2_3;
            npc1.velocity = vector2_4;
            npc1.target = num25;
            npc1.direction = bitsByte22[0] ? 1 : -1;
            npc1.directionY = bitsByte22[1] ? 1 : -1;
            npc1.spriteDirection = bitsByte22[6] ? 1 : -1;
            if (bitsByte22[7])
              num27 = npc1.life = npc1.lifeMax;
            else
              npc1.life = num27;
            if (num27 <= 0)
              npc1.active = false;
            npc1.SpawnedFromStatue = bitsByte23[0];
            if (npc1.SpawnedFromStatue)
              npc1.value = 0.0f;
            for (int index11 = 0; index11 < NPC.maxAI; ++index11)
              npc1.ai[index11] = numArray1[index11];
            if (oldType > -1 && oldType != npc1.type)
              npc1.TransformVisuals(oldType, npc1.type);
            if (Type1 == 262)
              NPC.plantBoss = index9;
            if (Type1 == 245)
              NPC.golemBoss = index9;
            if (npc1.type < 0 || npc1.type >= 670 || !Main.npcCatchable[npc1.type])
              break;
            npc1.releaseOwner = (short) this.reader.ReadByte();
            break;
          case 24:
            int number16 = (int) this.reader.ReadInt16();
            int number2_2 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number2_2 = this.whoAmI;
            Player player7 = Main.player[number2_2];
            Main.npc[number16].StrikeNPC(player7.inventory[player7.selectedItem].damage, player7.inventory[player7.selectedItem].knockBack, player7.direction);
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(24, ignoreClient: this.whoAmI, number: number16, number2: ((float) number2_2));
            NetMessage.TrySendData(23, number: number16);
            break;
          case 25:
            break;
          case 26:
            break;
          case 27:
            int num28 = (int) this.reader.ReadInt16();
            Vector2 vector2_5 = this.reader.ReadVector2();
            Vector2 vector2_6 = this.reader.ReadVector2();
            int index12 = (int) this.reader.ReadByte();
            int Type2 = (int) this.reader.ReadInt16();
            BitsByte bitsByte24 = (BitsByte) this.reader.ReadByte();
            float[] numArray2 = new float[Projectile.maxAI];
            for (int key = 0; key < Projectile.maxAI; ++key)
              numArray2[key] = !bitsByte24[key] ? 0.0f : this.reader.ReadSingle();
            int num29 = bitsByte24[3] ? (int) this.reader.ReadUInt16() : 0;
            int num30 = bitsByte24[4] ? (int) this.reader.ReadInt16() : 0;
            float num31 = bitsByte24[5] ? this.reader.ReadSingle() : 0.0f;
            int num32 = bitsByte24[6] ? (int) this.reader.ReadInt16() : 0;
            int index13 = bitsByte24[7] ? (int) this.reader.ReadInt16() : -1;
            if (index13 >= 1000)
              index13 = -1;
            if (Main.netMode == 2)
            {
              if (Type2 == 949)
              {
                index12 = (int) byte.MaxValue;
              }
              else
              {
                index12 = this.whoAmI;
                if (Main.projHostile[Type2])
                  break;
              }
            }
            int number17 = 1000;
            for (int index14 = 0; index14 < 1000; ++index14)
            {
              if (Main.projectile[index14].owner == index12 && Main.projectile[index14].identity == num28 && Main.projectile[index14].active)
              {
                number17 = index14;
                break;
              }
            }
            if (number17 == 1000)
            {
              for (int index15 = 0; index15 < 1000; ++index15)
              {
                if (!Main.projectile[index15].active)
                {
                  number17 = index15;
                  break;
                }
              }
            }
            if (number17 == 1000)
              number17 = Projectile.FindOldestProjectile();
            Projectile projectile = Main.projectile[number17];
            if (!projectile.active || projectile.type != Type2)
            {
              projectile.SetDefaults(Type2);
              if (Main.netMode == 2)
                ++Netplay.Clients[this.whoAmI].SpamProjectile;
            }
            projectile.identity = num28;
            projectile.position = vector2_5;
            projectile.velocity = vector2_6;
            projectile.type = Type2;
            projectile.damage = num30;
            projectile.bannerIdToRespondTo = num29;
            projectile.originalDamage = num32;
            projectile.knockBack = num31;
            projectile.owner = index12;
            for (int index16 = 0; index16 < Projectile.maxAI; ++index16)
              projectile.ai[index16] = numArray2[index16];
            if (index13 >= 0)
            {
              projectile.projUUID = index13;
              Main.projectileIdentity[index12, index13] = number17;
            }
            projectile.ProjectileFixDesperation();
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(27, ignoreClient: this.whoAmI, number: number17);
            break;
          case 28:
            int number18 = (int) this.reader.ReadInt16();
            int num33 = (int) this.reader.ReadInt16();
            float num34 = this.reader.ReadSingle();
            int num35 = (int) this.reader.ReadByte() - 1;
            byte number5_2 = this.reader.ReadByte();
            if (Main.netMode == 2)
            {
              if (num33 < 0)
                num33 = 0;
              Main.npc[number18].PlayerInteraction(this.whoAmI);
            }
            if (num33 >= 0)
            {
              Main.npc[number18].StrikeNPC(num33, num34, num35, number5_2 == (byte) 1, fromNet: true);
            }
            else
            {
              Main.npc[number18].life = 0;
              Main.npc[number18].HitEffect();
              Main.npc[number18].active = false;
            }
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(28, ignoreClient: this.whoAmI, number: number18, number2: ((float) num33), number3: num34, number4: ((float) num35), number5: ((int) number5_2));
            if (Main.npc[number18].life <= 0)
              NetMessage.TrySendData(23, number: number18);
            else
              Main.npc[number18].netUpdate = true;
            if (Main.npc[number18].realLife < 0)
              break;
            if (Main.npc[Main.npc[number18].realLife].life <= 0)
            {
              NetMessage.TrySendData(23, number: Main.npc[number18].realLife);
              break;
            }
            Main.npc[Main.npc[number18].realLife].netUpdate = true;
            break;
          case 29:
            int number19 = (int) this.reader.ReadInt16();
            int number2_3 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number2_3 = this.whoAmI;
            for (int index17 = 0; index17 < 1000; ++index17)
            {
              if (Main.projectile[index17].owner == number2_3 && Main.projectile[index17].identity == number19 && Main.projectile[index17].active)
              {
                Main.projectile[index17].Kill();
                break;
              }
            }
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(29, ignoreClient: this.whoAmI, number: number19, number2: ((float) number2_3));
            break;
          case 30:
            int number20 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number20 = this.whoAmI;
            bool flag10 = this.reader.ReadBoolean();
            Main.player[number20].hostile = flag10;
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(30, ignoreClient: this.whoAmI, number: number20);
            LocalizedText localizedText1 = flag10 ? Lang.mp[11] : Lang.mp[12];
            Color color1 = Main.teamColor[Main.player[number20].team];
            ChatHelper.BroadcastChatMessage(NetworkText.FromKey(localizedText1.Key, (object) Main.player[number20].name), color1);
            break;
          case 31:
            if (Main.netMode != 2)
              break;
            int num36 = (int) this.reader.ReadInt16();
            int num37 = (int) this.reader.ReadInt16();
            int chest1 = Chest.FindChest(num36, num37);
            if (chest1 <= -1 || Chest.UsingChest(chest1) != -1)
              break;
            for (int number2_4 = 0; number2_4 < 40; ++number2_4)
              NetMessage.TrySendData(32, this.whoAmI, number: chest1, number2: ((float) number2_4));
            NetMessage.TrySendData(33, this.whoAmI, number: chest1);
            Main.player[this.whoAmI].chest = chest1;
            if (Main.myPlayer == this.whoAmI)
              Main.recBigList = false;
            NetMessage.TrySendData(80, ignoreClient: this.whoAmI, number: this.whoAmI, number2: ((float) chest1));
            if (Main.netMode != 2 || !WorldGen.IsChestRigged(num36, num37))
              break;
            Wiring.SetCurrentUser(this.whoAmI);
            Wiring.HitSwitch(num36, num37);
            Wiring.SetCurrentUser();
            NetMessage.TrySendData(59, ignoreClient: this.whoAmI, number: num36, number2: ((float) num37));
            break;
          case 32:
            int index18 = (int) this.reader.ReadInt16();
            int index19 = (int) this.reader.ReadByte();
            int num38 = (int) this.reader.ReadInt16();
            int pre2 = (int) this.reader.ReadByte();
            int type6 = (int) this.reader.ReadInt16();
            if (index18 < 0 || index18 >= 8000)
              break;
            if (Main.chest[index18] == null)
              Main.chest[index18] = new Chest();
            if (Main.chest[index18].item[index19] == null)
              Main.chest[index18].item[index19] = new Item();
            Main.chest[index18].item[index19].netDefaults(type6);
            Main.chest[index18].item[index19].Prefix(pre2);
            Main.chest[index18].item[index19].stack = num38;
            Recipe.FindRecipes(true);
            break;
          case 33:
            int number2_5 = (int) this.reader.ReadInt16();
            int index20 = (int) this.reader.ReadInt16();
            int index21 = (int) this.reader.ReadInt16();
            int num39 = (int) this.reader.ReadByte();
            string str1 = string.Empty;
            if (num39 != 0)
            {
              if (num39 <= 20)
                str1 = this.reader.ReadString();
              else if (num39 != (int) byte.MaxValue)
                num39 = 0;
            }
            if (Main.netMode == 1)
            {
              Player player8 = Main.player[Main.myPlayer];
              if (player8.chest == -1)
              {
                Main.playerInventory = true;
                SoundEngine.PlaySound(10);
              }
              else if (player8.chest != number2_5 && number2_5 != -1)
              {
                Main.playerInventory = true;
                SoundEngine.PlaySound(12);
                Main.recBigList = false;
              }
              else if (player8.chest != -1 && number2_5 == -1)
              {
                SoundEngine.PlaySound(11);
                Main.recBigList = false;
              }
              player8.chest = number2_5;
              player8.chestX = index20;
              player8.chestY = index21;
              Recipe.FindRecipes(true);
              if (Main.tile[index20, index21].frameX < (short) 36 || Main.tile[index20, index21].frameX >= (short) 72)
                break;
              AchievementsHelper.HandleSpecialEvent(Main.player[Main.myPlayer], 16);
              break;
            }
            if (num39 != 0)
            {
              int chest2 = Main.player[this.whoAmI].chest;
              Chest chest3 = Main.chest[chest2];
              chest3.name = str1;
              NetMessage.TrySendData(69, ignoreClient: this.whoAmI, number: chest2, number2: ((float) chest3.x), number3: ((float) chest3.y));
            }
            Main.player[this.whoAmI].chest = number2_5;
            Recipe.FindRecipes(true);
            NetMessage.TrySendData(80, ignoreClient: this.whoAmI, number: this.whoAmI, number2: ((float) number2_5));
            break;
          case 34:
            byte number21 = this.reader.ReadByte();
            int index22 = (int) this.reader.ReadInt16();
            int index23 = (int) this.reader.ReadInt16();
            int index24 = (int) this.reader.ReadInt16();
            int id = (int) this.reader.ReadInt16();
            if (Main.netMode == 2)
              id = 0;
            if (Main.netMode == 2)
            {
              if (number21 == (byte) 0)
              {
                int number5_3 = WorldGen.PlaceChest(index22, index23, style: index24);
                if (number5_3 == -1)
                {
                  NetMessage.TrySendData(34, this.whoAmI, number: ((int) number21), number2: ((float) index22), number3: ((float) index23), number4: ((float) index24), number5: number5_3);
                  Item.NewItem((IEntitySource) new EntitySource_TileBreak(index22, index23), index22 * 16, index23 * 16, 32, 32, Chest.chestItemSpawn[index24], noBroadcast: true);
                  break;
                }
                NetMessage.TrySendData(34, number: ((int) number21), number2: ((float) index22), number3: ((float) index23), number4: ((float) index24), number5: number5_3);
                break;
              }
              if (number21 == (byte) 1 && Main.tile[index22, index23].type == (ushort) 21)
              {
                Tile tile = Main.tile[index22, index23];
                if ((int) tile.frameX % 36 != 0)
                  --index22;
                if ((int) tile.frameY % 36 != 0)
                  --index23;
                int chest4 = Chest.FindChest(index22, index23);
                WorldGen.KillTile(index22, index23);
                if (tile.active())
                  break;
                NetMessage.TrySendData(34, number: ((int) number21), number2: ((float) index22), number3: ((float) index23), number5: chest4);
                break;
              }
              if (number21 == (byte) 2)
              {
                int number5_4 = WorldGen.PlaceChest(index22, index23, (ushort) 88, style: index24);
                if (number5_4 == -1)
                {
                  NetMessage.TrySendData(34, this.whoAmI, number: ((int) number21), number2: ((float) index22), number3: ((float) index23), number4: ((float) index24), number5: number5_4);
                  Item.NewItem((IEntitySource) new EntitySource_TileBreak(index22, index23), index22 * 16, index23 * 16, 32, 32, Chest.dresserItemSpawn[index24], noBroadcast: true);
                  break;
                }
                NetMessage.TrySendData(34, number: ((int) number21), number2: ((float) index22), number3: ((float) index23), number4: ((float) index24), number5: number5_4);
                break;
              }
              if (number21 == (byte) 3 && Main.tile[index22, index23].type == (ushort) 88)
              {
                Tile tile = Main.tile[index22, index23];
                int num40 = index22 - (int) tile.frameX % 54 / 18;
                if ((int) tile.frameY % 36 != 0)
                  --index23;
                int chest5 = Chest.FindChest(num40, index23);
                WorldGen.KillTile(num40, index23);
                if (tile.active())
                  break;
                NetMessage.TrySendData(34, number: ((int) number21), number2: ((float) num40), number3: ((float) index23), number5: chest5);
                break;
              }
              if (number21 == (byte) 4)
              {
                int number5_5 = WorldGen.PlaceChest(index22, index23, (ushort) 467, style: index24);
                if (number5_5 == -1)
                {
                  NetMessage.TrySendData(34, this.whoAmI, number: ((int) number21), number2: ((float) index22), number3: ((float) index23), number4: ((float) index24), number5: number5_5);
                  Item.NewItem((IEntitySource) new EntitySource_TileBreak(index22, index23), index22 * 16, index23 * 16, 32, 32, Chest.chestItemSpawn2[index24], noBroadcast: true);
                  break;
                }
                NetMessage.TrySendData(34, number: ((int) number21), number2: ((float) index22), number3: ((float) index23), number4: ((float) index24), number5: number5_5);
                break;
              }
              if (number21 != (byte) 5 || Main.tile[index22, index23].type != (ushort) 467)
                break;
              Tile tile1 = Main.tile[index22, index23];
              if ((int) tile1.frameX % 36 != 0)
                --index22;
              if ((int) tile1.frameY % 36 != 0)
                --index23;
              int chest6 = Chest.FindChest(index22, index23);
              WorldGen.KillTile(index22, index23);
              if (tile1.active())
                break;
              NetMessage.TrySendData(34, number: ((int) number21), number2: ((float) index22), number3: ((float) index23), number5: chest6);
              break;
            }
            switch (number21)
            {
              case 0:
                if (id == -1)
                {
                  WorldGen.KillTile(index22, index23);
                  return;
                }
                SoundEngine.PlaySound(0, index22 * 16, index23 * 16);
                WorldGen.PlaceChestDirect(index22, index23, (ushort) 21, index24, id);
                return;
              case 2:
                if (id == -1)
                {
                  WorldGen.KillTile(index22, index23);
                  return;
                }
                SoundEngine.PlaySound(0, index22 * 16, index23 * 16);
                WorldGen.PlaceDresserDirect(index22, index23, (ushort) 88, index24, id);
                return;
              case 4:
                if (id == -1)
                {
                  WorldGen.KillTile(index22, index23);
                  return;
                }
                SoundEngine.PlaySound(0, index22 * 16, index23 * 16);
                WorldGen.PlaceChestDirect(index22, index23, (ushort) 467, index24, id);
                return;
              default:
                Chest.DestroyChestDirect(index22, index23, id);
                WorldGen.KillTile(index22, index23);
                return;
            }
          case 35:
            int number22 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number22 = this.whoAmI;
            int num41 = (int) this.reader.ReadInt16();
            if (number22 != Main.myPlayer || Main.ServerSideCharacter)
              Main.player[number22].HealEffect(num41);
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(35, ignoreClient: this.whoAmI, number: number22, number2: ((float) num41));
            break;
          case 36:
            int number23 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number23 = this.whoAmI;
            Player player9 = Main.player[number23];
            player9.zone1 = (BitsByte) this.reader.ReadByte();
            player9.zone2 = (BitsByte) this.reader.ReadByte();
            player9.zone3 = (BitsByte) this.reader.ReadByte();
            player9.zone4 = (BitsByte) this.reader.ReadByte();
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(36, ignoreClient: this.whoAmI, number: number23);
            break;
          case 37:
            if (Main.netMode != 1)
              break;
            if (Main.autoPass)
            {
              NetMessage.TrySendData(38);
              Main.autoPass = false;
              break;
            }
            Netplay.ServerPassword = "";
            Main.menuMode = 31;
            break;
          case 38:
            if (Main.netMode != 2)
              break;
            if (this.reader.ReadString() == Netplay.ServerPassword)
            {
              Netplay.Clients[this.whoAmI].State = 1;
              NetMessage.TrySendData(3, this.whoAmI);
              break;
            }
            NetMessage.TrySendData(2, this.whoAmI, text: Lang.mp[1].ToNetworkText());
            break;
          case 39:
            if (Main.netMode != 1)
              break;
            int number24 = (int) this.reader.ReadInt16();
            Main.item[number24].playerIndexTheItemIsReservedFor = (int) byte.MaxValue;
            NetMessage.TrySendData(22, number: number24);
            break;
          case 40:
            int number25 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number25 = this.whoAmI;
            int npcIndex = (int) this.reader.ReadInt16();
            Main.player[number25].SetTalkNPC(npcIndex, true);
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(40, ignoreClient: this.whoAmI, number: number25);
            break;
          case 41:
            int number26 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number26 = this.whoAmI;
            Player player10 = Main.player[number26];
            float num42 = this.reader.ReadSingle();
            int num43 = (int) this.reader.ReadInt16();
            player10.itemRotation = num42;
            player10.itemAnimation = num43;
            player10.channel = player10.inventory[player10.selectedItem].channel;
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(41, ignoreClient: this.whoAmI, number: number26);
            break;
          case 42:
            int index25 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              index25 = this.whoAmI;
            else if (Main.myPlayer == index25 && !Main.ServerSideCharacter)
              break;
            int num44 = (int) this.reader.ReadInt16();
            int num45 = (int) this.reader.ReadInt16();
            Main.player[index25].statMana = num44;
            Main.player[index25].statManaMax = num45;
            break;
          case 43:
            int number27 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number27 = this.whoAmI;
            int num46 = (int) this.reader.ReadInt16();
            if (number27 != Main.myPlayer)
              Main.player[number27].ManaEffect(num46);
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(43, ignoreClient: this.whoAmI, number: number27, number2: ((float) num46));
            break;
          case 44:
            break;
          case 45:
            int number28 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number28 = this.whoAmI;
            int index26 = (int) this.reader.ReadByte();
            Player player11 = Main.player[number28];
            int team = player11.team;
            player11.team = index26;
            Color color2 = Main.teamColor[index26];
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(45, ignoreClient: this.whoAmI, number: number28);
            LocalizedText localizedText2 = Lang.mp[13 + index26];
            if (index26 == 5)
              localizedText2 = Lang.mp[22];
            for (int playerId = 0; playerId < (int) byte.MaxValue; ++playerId)
            {
              if (playerId == this.whoAmI || team > 0 && Main.player[playerId].team == team || index26 > 0 && Main.player[playerId].team == index26)
                ChatHelper.SendChatMessageToClient(NetworkText.FromKey(localizedText2.Key, (object) player11.name), color2, playerId);
            }
            break;
          case 46:
            if (Main.netMode != 2)
              break;
            int number29 = Sign.ReadSign((int) this.reader.ReadInt16(), (int) this.reader.ReadInt16());
            if (number29 < 0)
              break;
            NetMessage.TrySendData(47, this.whoAmI, number: number29, number2: ((float) this.whoAmI));
            break;
          case 47:
            int index27 = (int) this.reader.ReadInt16();
            int num47 = (int) this.reader.ReadInt16();
            int num48 = (int) this.reader.ReadInt16();
            string text1 = this.reader.ReadString();
            int number2_6 = (int) this.reader.ReadByte();
            BitsByte bitsByte25 = (BitsByte) this.reader.ReadByte();
            if (index27 < 0 || index27 >= 1000)
              break;
            string str2 = (string) null;
            if (Main.sign[index27] != null)
              str2 = Main.sign[index27].text;
            Main.sign[index27] = new Sign();
            Main.sign[index27].x = num47;
            Main.sign[index27].y = num48;
            Sign.TextSign(index27, text1);
            if (Main.netMode == 2 && str2 != text1)
            {
              number2_6 = this.whoAmI;
              NetMessage.TrySendData(47, ignoreClient: this.whoAmI, number: index27, number2: ((float) number2_6));
            }
            if (Main.netMode != 1 || number2_6 != Main.myPlayer || Main.sign[index27] == null || bitsByte25[0])
              break;
            Main.playerInventory = false;
            Main.player[Main.myPlayer].SetTalkNPC(-1, true);
            Main.npcChatCornerItem = 0;
            Main.editSign = false;
            SoundEngine.PlaySound(10);
            Main.player[Main.myPlayer].sign = index27;
            Main.npcChatText = Main.sign[index27].text;
            break;
          case 48:
            int i1 = (int) this.reader.ReadInt16();
            int j1 = (int) this.reader.ReadInt16();
            byte num49 = this.reader.ReadByte();
            byte liquidType = this.reader.ReadByte();
            if (Main.netMode == 2 && Netplay.SpamCheck)
            {
              int whoAmI = this.whoAmI;
              int num50 = (int) ((double) Main.player[whoAmI].position.X + (double) (Main.player[whoAmI].width / 2));
              int num51 = (int) ((double) Main.player[whoAmI].position.Y + (double) (Main.player[whoAmI].height / 2));
              int num52 = 10;
              int num53 = num50 - num52;
              int num54 = num50 + num52;
              int num55 = num51 - num52;
              int num56 = num51 + num52;
              if (i1 < num53 || i1 > num54 || j1 < num55 || j1 > num56)
                ++Netplay.Clients[this.whoAmI].SpamWater;
            }
            if (Main.tile[i1, j1] == null)
              Main.tile[i1, j1] = new Tile();
            lock (Main.tile[i1, j1])
            {
              Main.tile[i1, j1].liquid = num49;
              Main.tile[i1, j1].liquidType((int) liquidType);
              if (Main.netMode != 2)
                break;
              WorldGen.SquareTileFrame(i1, j1);
              break;
            }
          case 49:
            if (Netplay.Connection.State != 6)
              break;
            Netplay.Connection.State = 10;
            Main.ActivePlayerFileData.StartPlayTimer();
            Player.Hooks.EnterWorld(Main.myPlayer);
            Main.player[Main.myPlayer].Spawn(PlayerSpawnContext.SpawningIntoWorld);
            break;
          case 50:
            int number30 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number30 = this.whoAmI;
            else if (number30 == Main.myPlayer && !Main.ServerSideCharacter)
              break;
            Player player12 = Main.player[number30];
            for (int index28 = 0; index28 < 22; ++index28)
            {
              player12.buffType[index28] = (int) this.reader.ReadUInt16();
              player12.buffTime[index28] = player12.buffType[index28] <= 0 ? 0 : 60;
            }
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(50, ignoreClient: this.whoAmI, number: number30);
            break;
          case 51:
            byte index29 = this.reader.ReadByte();
            byte number2_7 = this.reader.ReadByte();
            switch (number2_7)
            {
              case 1:
                NPC.SpawnSkeletron((int) index29);
                return;
              case 2:
                if (Main.netMode == 2)
                {
                  NetMessage.TrySendData(51, ignoreClient: this.whoAmI, number: ((int) index29), number2: ((float) number2_7));
                  return;
                }
                SoundEngine.PlaySound(SoundID.Item1, (int) Main.player[(int) index29].position.X, (int) Main.player[(int) index29].position.Y);
                return;
              case 3:
                if (Main.netMode != 2)
                  return;
                Main.Sundialing();
                return;
              case 4:
                Main.npc[(int) index29].BigMimicSpawnSmoke();
                return;
              case 5:
                if (Main.netMode != 2)
                  return;
                NPC npc2 = new NPC();
                npc2.SetDefaults(664);
                Main.BestiaryTracker.Kills.RegisterKill(npc2);
                return;
              default:
                return;
            }
          case 52:
            int number2_8 = (int) this.reader.ReadByte();
            int num57 = (int) this.reader.ReadInt16();
            int num58 = (int) this.reader.ReadInt16();
            if (number2_8 == 1)
            {
              Chest.Unlock(num57, num58);
              if (Main.netMode == 2)
              {
                NetMessage.TrySendData(52, ignoreClient: this.whoAmI, number2: ((float) number2_8), number3: ((float) num57), number4: ((float) num58));
                NetMessage.SendTileSquare(-1, num57, num58, 2);
              }
            }
            if (number2_8 != 2)
              break;
            WorldGen.UnlockDoor(num57, num58);
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(52, ignoreClient: this.whoAmI, number2: ((float) number2_8), number3: ((float) num57), number4: ((float) num58));
            NetMessage.SendTileSquare(-1, num57, num58, 2);
            break;
          case 53:
            int number31 = (int) this.reader.ReadInt16();
            int type7 = (int) this.reader.ReadUInt16();
            int time1 = (int) this.reader.ReadInt16();
            Main.npc[number31].AddBuff(type7, time1, true);
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(54, number: number31);
            break;
          case 54:
            if (Main.netMode != 1)
              break;
            int index30 = (int) this.reader.ReadInt16();
            NPC npc3 = Main.npc[index30];
            for (int index31 = 0; index31 < 5; ++index31)
            {
              npc3.buffType[index31] = (int) this.reader.ReadUInt16();
              npc3.buffTime[index31] = (int) this.reader.ReadInt16();
            }
            break;
          case 55:
            int index32 = (int) this.reader.ReadByte();
            int index33 = (int) this.reader.ReadUInt16();
            int num59 = this.reader.ReadInt32();
            if (Main.netMode == 2 && index32 != this.whoAmI && !Main.pvpBuff[index33])
              break;
            if (Main.netMode == 1 && index32 == Main.myPlayer)
            {
              Main.player[index32].AddBuff(index33, num59);
              break;
            }
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(55, index32, number: index32, number2: ((float) index33), number3: ((float) num59));
            break;
          case 56:
            int number32 = (int) this.reader.ReadInt16();
            if (number32 < 0 || number32 >= 200)
              break;
            if (Main.netMode == 1)
            {
              string str3 = this.reader.ReadString();
              Main.npc[number32].GivenName = str3;
              int num60 = this.reader.ReadInt32();
              Main.npc[number32].townNpcVariationIndex = num60;
              break;
            }
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(56, this.whoAmI, number: number32);
            break;
          case 57:
            if (Main.netMode != 1)
              break;
            WorldGen.tGood = this.reader.ReadByte();
            WorldGen.tEvil = this.reader.ReadByte();
            WorldGen.tBlood = this.reader.ReadByte();
            break;
          case 58:
            int index34 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              index34 = this.whoAmI;
            float num61 = this.reader.ReadSingle();
            if (Main.netMode == 2)
            {
              NetMessage.TrySendData(58, ignoreClient: this.whoAmI, number: this.whoAmI, number2: num61);
              break;
            }
            Player player13 = Main.player[index34];
            int type8 = player13.inventory[player13.selectedItem].type;
            switch (type8)
            {
              case 4057:
              case 4372:
              case 4715:
                player13.PlayGuitarChord(num61);
                return;
              case 4673:
                player13.PlayDrums(num61);
                return;
              default:
                Main.musicPitch = num61;
                LegacySoundStyle type9 = SoundID.Item26;
                if (type8 == 507)
                  type9 = SoundID.Item35;
                if (type8 == 1305)
                  type9 = SoundID.Item47;
                SoundEngine.PlaySound(type9, player13.position);
                return;
            }
          case 59:
            int num62 = (int) this.reader.ReadInt16();
            int num63 = (int) this.reader.ReadInt16();
            Wiring.SetCurrentUser(this.whoAmI);
            Wiring.HitSwitch(num62, num63);
            Wiring.SetCurrentUser();
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(59, ignoreClient: this.whoAmI, number: num62, number2: ((float) num63));
            break;
          case 60:
            int n = (int) this.reader.ReadInt16();
            int x1 = (int) this.reader.ReadInt16();
            int y1 = (int) this.reader.ReadInt16();
            byte num64 = this.reader.ReadByte();
            if (n >= 200)
            {
              NetMessage.BootPlayer(this.whoAmI, NetworkText.FromKey("Net.CheatingInvalid"));
              break;
            }
            if (Main.netMode == 1)
            {
              Main.npc[n].homeless = num64 == (byte) 1;
              Main.npc[n].homeTileX = x1;
              Main.npc[n].homeTileY = y1;
              if (num64 == (byte) 1)
              {
                WorldGen.TownManager.KickOut(Main.npc[n].type);
                break;
              }
              if (num64 != (byte) 2)
                break;
              WorldGen.TownManager.SetRoom(Main.npc[n].type, x1, y1);
              break;
            }
            if (num64 == (byte) 1)
            {
              WorldGen.kickOut(n);
              break;
            }
            WorldGen.moveRoom(x1, y1, n);
            break;
          case 61:
            int plr = (int) this.reader.ReadInt16();
            int Type3 = (int) this.reader.ReadInt16();
            if (Main.netMode != 2)
              break;
            if (Type3 >= 0 && Type3 < 670 && NPCID.Sets.MPAllowedEnemies[Type3])
            {
              if (NPC.AnyNPCs(Type3))
                break;
              NPC.SpawnOnPlayer(plr, Type3);
              break;
            }
            switch (Type3)
            {
              case -14:
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.LicenseBunnyUsed"), new Color(50, (int) byte.MaxValue, 130));
                NPC.boughtBunny = true;
                NetMessage.TrySendData(7);
                return;
              case -13:
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.LicenseDogUsed"), new Color(50, (int) byte.MaxValue, 130));
                NPC.boughtDog = true;
                NetMessage.TrySendData(7);
                return;
              case -12:
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.LicenseCatUsed"), new Color(50, (int) byte.MaxValue, 130));
                NPC.boughtCat = true;
                NetMessage.TrySendData(7);
                return;
              case -11:
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.CombatBookUsed"), new Color(50, (int) byte.MaxValue, 130));
                NPC.combatBookWasUsed = true;
                NetMessage.TrySendData(7);
                return;
              case -10:
                if (Main.dayTime || Main.bloodMoon)
                  return;
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[8].Key), new Color(50, (int) byte.MaxValue, 130));
                Main.bloodMoon = true;
                if (Main.GetMoonPhase() == MoonPhase.Empty)
                  Main.moonPhase = 5;
                AchievementsHelper.NotifyProgressionEvent(4);
                NetMessage.TrySendData(7);
                return;
              case -8:
                if (!NPC.downedGolemBoss || !Main.hardMode || NPC.AnyDanger() || NPC.AnyoneNearCultists())
                  return;
                WorldGen.StartImpendingDoom();
                NetMessage.TrySendData(7);
                return;
              case -7:
                Main.invasionDelay = 0;
                Main.StartInvasion(4);
                NetMessage.TrySendData(7);
                NetMessage.TrySendData(78, number2: 1f, number3: ((float) (Main.invasionType + 3)));
                return;
              case -6:
                if (!Main.dayTime || Main.eclipse)
                  return;
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[20].Key), new Color(50, (int) byte.MaxValue, 130));
                Main.eclipse = true;
                NetMessage.TrySendData(7);
                return;
              case -5:
                if (Main.dayTime || DD2Event.Ongoing)
                  return;
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[34].Key), new Color(50, (int) byte.MaxValue, 130));
                Main.startSnowMoon();
                NetMessage.TrySendData(7);
                NetMessage.TrySendData(78, number2: 1f, number3: 1f, number4: 1f);
                return;
              case -4:
                if (Main.dayTime || DD2Event.Ongoing)
                  return;
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[31].Key), new Color(50, (int) byte.MaxValue, 130));
                Main.startPumpkinMoon();
                NetMessage.TrySendData(7);
                NetMessage.TrySendData(78, number2: 1f, number3: 2f, number4: 1f);
                return;
              default:
                if (Type3 >= 0)
                  return;
                int type10 = 1;
                if (Type3 > -5)
                  type10 = -Type3;
                if (type10 > 0 && Main.invasionType == 0)
                {
                  Main.invasionDelay = 0;
                  Main.StartInvasion(type10);
                }
                NetMessage.TrySendData(78, number2: 1f, number3: ((float) (Main.invasionType + 3)));
                return;
            }
          case 62:
            int number33 = (int) this.reader.ReadByte();
            int number2_9 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number33 = this.whoAmI;
            if (number2_9 == 1)
              Main.player[number33].NinjaDodge();
            if (number2_9 == 2)
              Main.player[number33].ShadowDodge();
            if (number2_9 == 4)
              Main.player[number33].BrainOfConfusionDodge();
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(62, ignoreClient: this.whoAmI, number: number33, number2: ((float) number2_9));
            break;
          case 63:
            int num65 = (int) this.reader.ReadInt16();
            int num66 = (int) this.reader.ReadInt16();
            byte num67 = this.reader.ReadByte();
            WorldGen.paintTile(num65, num66, num67);
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(63, ignoreClient: this.whoAmI, number: num65, number2: ((float) num66), number3: ((float) num67));
            break;
          case 64:
            int num68 = (int) this.reader.ReadInt16();
            int num69 = (int) this.reader.ReadInt16();
            byte num70 = this.reader.ReadByte();
            WorldGen.paintWall(num68, num69, num70);
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(64, ignoreClient: this.whoAmI, number: num68, number2: ((float) num69), number3: ((float) num70));
            break;
          case 65:
            BitsByte bitsByte26 = (BitsByte) this.reader.ReadByte();
            int number2_10 = (int) this.reader.ReadInt16();
            if (Main.netMode == 2)
              number2_10 = this.whoAmI;
            Vector2 vector2_7 = this.reader.ReadVector2();
            int num71 = (int) this.reader.ReadByte();
            int number34 = 0;
            if (bitsByte26[0])
              ++number34;
            if (bitsByte26[1])
              number34 += 2;
            bool flag11 = false;
            if (bitsByte26[2])
              flag11 = true;
            int num72 = 0;
            if (bitsByte26[3])
              num72 = this.reader.ReadInt32();
            if (flag11)
              vector2_7 = Main.player[number2_10].position;
            switch (number34)
            {
              case 0:
                Main.player[number2_10].Teleport(vector2_7, num71, num72);
                break;
              case 1:
                Main.npc[number2_10].Teleport(vector2_7, num71, num72);
                break;
              case 2:
                Main.player[number2_10].Teleport(vector2_7, num71, num72);
                if (Main.netMode == 2)
                {
                  RemoteClient.CheckSection(this.whoAmI, vector2_7);
                  NetMessage.TrySendData(65, number2: ((float) number2_10), number3: vector2_7.X, number4: vector2_7.Y, number5: num71, number6: flag11.ToInt(), number7: num72);
                  int index35 = -1;
                  float num73 = 9999f;
                  for (int index36 = 0; index36 < (int) byte.MaxValue; ++index36)
                  {
                    if (Main.player[index36].active && index36 != this.whoAmI)
                    {
                      Vector2 vector2_8 = Main.player[index36].position - Main.player[this.whoAmI].position;
                      if ((double) vector2_8.Length() < (double) num73)
                      {
                        num73 = vector2_8.Length();
                        index35 = index36;
                      }
                    }
                  }
                  if (index35 >= 0)
                  {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Game.HasTeleportedTo", (object) Main.player[this.whoAmI].name, (object) Main.player[index35].name), new Color(250, 250, 0));
                    break;
                  }
                  break;
                }
                break;
            }
            if (Main.netMode != 2 || number34 != 0)
              break;
            NetMessage.TrySendData(65, ignoreClient: this.whoAmI, number: number34, number2: ((float) number2_10), number3: vector2_7.X, number4: vector2_7.Y, number5: num71, number6: flag11.ToInt(), number7: num72);
            break;
          case 66:
            int number35 = (int) this.reader.ReadByte();
            int num74 = (int) this.reader.ReadInt16();
            if (num74 <= 0)
              break;
            Player player14 = Main.player[number35];
            player14.statLife += num74;
            if (player14.statLife > player14.statLifeMax2)
              player14.statLife = player14.statLifeMax2;
            player14.HealEffect(num74, false);
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(66, ignoreClient: this.whoAmI, number: number35, number2: ((float) num74));
            break;
          case 67:
            break;
          case 68:
            this.reader.ReadString();
            break;
          case 69:
            int number36 = (int) this.reader.ReadInt16();
            int num75 = (int) this.reader.ReadInt16();
            int num76 = (int) this.reader.ReadInt16();
            if (Main.netMode == 1)
            {
              if (number36 < 0 || number36 >= 8000)
                break;
              Chest chest7 = Main.chest[number36];
              if (chest7 == null)
              {
                chest7 = new Chest();
                chest7.x = num75;
                chest7.y = num76;
                Main.chest[number36] = chest7;
              }
              else if (chest7.x != num75 || chest7.y != num76)
                break;
              chest7.name = this.reader.ReadString();
              break;
            }
            if (number36 < -1 || number36 >= 8000)
              break;
            if (number36 == -1)
            {
              number36 = Chest.FindChest(num75, num76);
              if (number36 == -1)
                break;
            }
            Chest chest8 = Main.chest[number36];
            if (chest8.x != num75 || chest8.y != num76)
              break;
            NetMessage.TrySendData(69, this.whoAmI, number: number36, number2: ((float) num75), number3: ((float) num76));
            break;
          case 70:
            if (Main.netMode != 2)
              break;
            int i2 = (int) this.reader.ReadInt16();
            int who = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              who = this.whoAmI;
            if (i2 >= 200 || i2 < 0)
              break;
            NPC.CatchNPC(i2, who);
            break;
          case 71:
            if (Main.netMode != 2)
              break;
            int x2 = this.reader.ReadInt32();
            int num77 = this.reader.ReadInt32();
            int num78 = (int) this.reader.ReadInt16();
            byte num79 = this.reader.ReadByte();
            int y2 = num77;
            int Type4 = num78;
            int Style1 = (int) num79;
            int whoAmI1 = this.whoAmI;
            NPC.ReleaseNPC(x2, y2, Type4, Style1, whoAmI1);
            break;
          case 72:
            if (Main.netMode != 1)
              break;
            for (int index37 = 0; index37 < 40; ++index37)
              Main.travelShop[index37] = (int) this.reader.ReadInt16();
            break;
          case 73:
            switch (this.reader.ReadByte())
            {
              case 0:
                Main.player[this.whoAmI].TeleportationPotion();
                return;
              case 1:
                Main.player[this.whoAmI].MagicConch();
                return;
              case 2:
                Main.player[this.whoAmI].DemonConch();
                return;
              default:
                return;
            }
          case 74:
            if (Main.netMode != 1)
              break;
            Main.anglerQuest = (int) this.reader.ReadByte();
            Main.anglerQuestFinished = this.reader.ReadBoolean();
            break;
          case 75:
            if (Main.netMode != 2)
              break;
            string name = Main.player[this.whoAmI].name;
            if (Main.anglerWhoFinishedToday.Contains(name))
              break;
            Main.anglerWhoFinishedToday.Add(name);
            break;
          case 76:
            int number37 = (int) this.reader.ReadByte();
            if (number37 == Main.myPlayer && !Main.ServerSideCharacter)
              break;
            if (Main.netMode == 2)
              number37 = this.whoAmI;
            Player player15 = Main.player[number37];
            player15.anglerQuestsFinished = this.reader.ReadInt32();
            player15.golferScoreAccumulated = this.reader.ReadInt32();
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(76, ignoreClient: this.whoAmI, number: number37);
            break;
          case 77:
            int type11 = (int) this.reader.ReadInt16();
            ushort num80 = this.reader.ReadUInt16();
            short num81 = this.reader.ReadInt16();
            short num82 = this.reader.ReadInt16();
            int tileType = (int) num80;
            int x3 = (int) num81;
            int y3 = (int) num82;
            Animation.NewTemporaryAnimation(type11, (ushort) tileType, x3, y3);
            break;
          case 78:
            if (Main.netMode != 1)
              break;
            Main.ReportInvasionProgress(this.reader.ReadInt32(), this.reader.ReadInt32(), (int) this.reader.ReadSByte(), (int) this.reader.ReadSByte());
            break;
          case 79:
            int x4 = (int) this.reader.ReadInt16();
            int y4 = (int) this.reader.ReadInt16();
            short type12 = this.reader.ReadInt16();
            int style = (int) this.reader.ReadInt16();
            int num83 = (int) this.reader.ReadByte();
            int random = (int) this.reader.ReadSByte();
            int direction2 = !this.reader.ReadBoolean() ? -1 : 1;
            if (Main.netMode == 2)
            {
              ++Netplay.Clients[this.whoAmI].SpamAddBlock;
              if (!WorldGen.InWorld(x4, y4, 10) || !Netplay.Clients[this.whoAmI].TileSections[Netplay.GetSectionX(x4), Netplay.GetSectionY(y4)])
                break;
            }
            WorldGen.PlaceObject(x4, y4, (int) type12, style: style, alternate: num83, random: random, direction: direction2);
            if (Main.netMode != 2)
              break;
            NetMessage.SendObjectPlacment(this.whoAmI, x4, y4, (int) type12, style, num83, random, direction2);
            break;
          case 80:
            if (Main.netMode != 1)
              break;
            int index38 = (int) this.reader.ReadByte();
            int num84 = (int) this.reader.ReadInt16();
            if (num84 < -3 || num84 >= 8000)
              break;
            Main.player[index38].chest = num84;
            Recipe.FindRecipes(true);
            break;
          case 81:
            if (Main.netMode != 1)
              break;
            int x5 = (int) this.reader.ReadSingle();
            int num85 = (int) this.reader.ReadSingle();
            Color color3 = this.reader.ReadRGB();
            int amount = this.reader.ReadInt32();
            int y5 = num85;
            CombatText.NewText(new Rectangle(x5, y5, 0, 0), color3, amount);
            break;
          case 82:
            NetManager.Instance.Read(this.reader, this.whoAmI, length);
            break;
          case 83:
            if (Main.netMode != 1)
              break;
            int index39 = (int) this.reader.ReadInt16();
            int num86 = this.reader.ReadInt32();
            if (index39 < 0 || index39 >= 289)
              break;
            NPC.killCount[index39] = num86;
            break;
          case 84:
            int number38 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number38 = this.whoAmI;
            float num87 = this.reader.ReadSingle();
            Main.player[number38].stealth = num87;
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(84, ignoreClient: this.whoAmI, number: number38);
            break;
          case 85:
            int whoAmI2 = this.whoAmI;
            byte slot = this.reader.ReadByte();
            if (Main.netMode != 2 || whoAmI2 >= (int) byte.MaxValue || slot >= (byte) 58)
              break;
            Chest.ServerPlaceItem(this.whoAmI, (int) slot);
            break;
          case 86:
            if (Main.netMode != 1)
              break;
            int key1 = this.reader.ReadInt32();
            if (!this.reader.ReadBoolean())
            {
              TileEntity tileEntity;
              if (!TileEntity.ByID.TryGetValue(key1, out tileEntity))
                break;
              TileEntity.ByID.Remove(key1);
              TileEntity.ByPosition.Remove(tileEntity.Position);
              break;
            }
            TileEntity tileEntity1 = TileEntity.Read(this.reader, true);
            tileEntity1.ID = key1;
            TileEntity.ByID[tileEntity1.ID] = tileEntity1;
            TileEntity.ByPosition[tileEntity1.Position] = tileEntity1;
            break;
          case 87:
            if (Main.netMode != 2)
              break;
            int num88 = (int) this.reader.ReadInt16();
            int num89 = (int) this.reader.ReadInt16();
            int type13 = (int) this.reader.ReadByte();
            if (!WorldGen.InWorld(num88, num89) || TileEntity.ByPosition.ContainsKey(new Point16(num88, num89)))
              break;
            TileEntity.PlaceEntityNet(num88, num89, type13);
            break;
          case 88:
            if (Main.netMode != 1)
              break;
            int index40 = (int) this.reader.ReadInt16();
            if (index40 < 0 || index40 > 400)
              break;
            Item obj2 = Main.item[index40];
            BitsByte bitsByte27 = (BitsByte) this.reader.ReadByte();
            if (bitsByte27[0])
              obj2.color.PackedValue = this.reader.ReadUInt32();
            if (bitsByte27[1])
              obj2.damage = (int) this.reader.ReadUInt16();
            if (bitsByte27[2])
              obj2.knockBack = this.reader.ReadSingle();
            if (bitsByte27[3])
              obj2.useAnimation = (int) this.reader.ReadUInt16();
            if (bitsByte27[4])
              obj2.useTime = (int) this.reader.ReadUInt16();
            if (bitsByte27[5])
              obj2.shoot = (int) this.reader.ReadInt16();
            if (bitsByte27[6])
              obj2.shootSpeed = this.reader.ReadSingle();
            if (!bitsByte27[7])
              break;
            bitsByte27 = (BitsByte) this.reader.ReadByte();
            if (bitsByte27[0])
              obj2.width = (int) this.reader.ReadInt16();
            if (bitsByte27[1])
              obj2.height = (int) this.reader.ReadInt16();
            if (bitsByte27[2])
              obj2.scale = this.reader.ReadSingle();
            if (bitsByte27[3])
              obj2.ammo = (int) this.reader.ReadInt16();
            if (bitsByte27[4])
              obj2.useAmmo = (int) this.reader.ReadInt16();
            if (!bitsByte27[5])
              break;
            obj2.notAmmo = this.reader.ReadBoolean();
            break;
          case 89:
            if (Main.netMode != 2)
              break;
            int x6 = (int) this.reader.ReadInt16();
            int num90 = (int) this.reader.ReadInt16();
            int num91 = (int) this.reader.ReadInt16();
            int num92 = (int) this.reader.ReadByte();
            int num93 = (int) this.reader.ReadInt16();
            int y6 = num90;
            int netid1 = num91;
            int prefix1 = num92;
            int stack1 = num93;
            TEItemFrame.TryPlacing(x6, y6, netid1, prefix1, stack1);
            break;
          case 91:
            if (Main.netMode != 1)
              break;
            int num94 = this.reader.ReadInt32();
            int type14 = (int) this.reader.ReadByte();
            if (type14 == (int) byte.MaxValue)
            {
              if (!EmoteBubble.byID.ContainsKey(num94))
                break;
              EmoteBubble.byID.Remove(num94);
              break;
            }
            int meta = (int) this.reader.ReadUInt16();
            int time2 = (int) this.reader.ReadUInt16();
            int emotion = (int) this.reader.ReadByte();
            int num95 = 0;
            if (emotion < 0)
              num95 = (int) this.reader.ReadInt16();
            WorldUIAnchor bubbleAnchor = EmoteBubble.DeserializeNetAnchor(type14, meta);
            if (type14 == 1)
              Main.player[meta].emoteTime = 360;
            lock (EmoteBubble.byID)
            {
              if (!EmoteBubble.byID.ContainsKey(num94))
              {
                EmoteBubble.byID[num94] = new EmoteBubble(emotion, bubbleAnchor, time2);
              }
              else
              {
                EmoteBubble.byID[num94].lifeTime = time2;
                EmoteBubble.byID[num94].lifeTimeStart = time2;
                EmoteBubble.byID[num94].emote = emotion;
                EmoteBubble.byID[num94].anchor = bubbleAnchor;
              }
              EmoteBubble.byID[num94].ID = num94;
              EmoteBubble.byID[num94].metadata = num95;
              EmoteBubble.OnBubbleChange(num94);
              break;
            }
          case 92:
            int number39 = (int) this.reader.ReadInt16();
            int num96 = this.reader.ReadInt32();
            float num97 = this.reader.ReadSingle();
            float num98 = this.reader.ReadSingle();
            if (number39 < 0 || number39 > 200)
              break;
            if (Main.netMode == 1)
            {
              Main.npc[number39].moneyPing(new Vector2(num97, num98));
              Main.npc[number39].extraValue = num96;
              break;
            }
            Main.npc[number39].extraValue += num96;
            NetMessage.TrySendData(92, number: number39, number2: ((float) Main.npc[number39].extraValue), number3: num97, number4: num98);
            break;
          case 93:
            break;
          case 95:
            ushort number2_11 = this.reader.ReadUInt16();
            int num99 = (int) this.reader.ReadByte();
            if (Main.netMode != 2)
              break;
            for (int index41 = 0; index41 < 1000; ++index41)
            {
              if (Main.projectile[index41].owner == (int) number2_11 && Main.projectile[index41].active && Main.projectile[index41].type == 602 && (double) Main.projectile[index41].ai[1] == (double) num99)
              {
                Main.projectile[index41].Kill();
                NetMessage.TrySendData(29, number: Main.projectile[index41].identity, number2: ((float) number2_11));
                break;
              }
            }
            break;
          case 96:
            int number40 = (int) this.reader.ReadByte();
            Player player16 = Main.player[number40];
            int num100 = (int) this.reader.ReadInt16();
            Vector2 newPos1 = this.reader.ReadVector2();
            Vector2 vector2_9 = this.reader.ReadVector2();
            player16.lastPortalColorIndex = num100 + (num100 % 2 == 0 ? 1 : -1);
            player16.Teleport(newPos1, 4, num100);
            player16.velocity = vector2_9;
            if (Main.netMode != 2)
              break;
            NetMessage.SendData(96, number: number40, number2: newPos1.X, number3: newPos1.Y, number4: ((float) num100));
            break;
          case 97:
            if (Main.netMode != 1)
              break;
            AchievementsHelper.NotifyNPCKilledDirect(Main.player[Main.myPlayer], (int) this.reader.ReadInt16());
            break;
          case 98:
            if (Main.netMode != 1)
              break;
            AchievementsHelper.NotifyProgressionEvent((int) this.reader.ReadInt16());
            break;
          case 99:
            int number41 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number41 = this.whoAmI;
            Main.player[number41].MinionRestTargetPoint = this.reader.ReadVector2();
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(99, ignoreClient: this.whoAmI, number: number41);
            break;
          case 100:
            int index42 = (int) this.reader.ReadUInt16();
            NPC npc4 = Main.npc[index42];
            int extraInfo = (int) this.reader.ReadInt16();
            Vector2 newPos2 = this.reader.ReadVector2();
            Vector2 vector2_10 = this.reader.ReadVector2();
            npc4.lastPortalColorIndex = extraInfo + (extraInfo % 2 == 0 ? 1 : -1);
            npc4.Teleport(newPos2, 4, extraInfo);
            npc4.velocity = vector2_10;
            npc4.netOffset *= 0.0f;
            break;
          case 101:
            if (Main.netMode == 2)
              break;
            NPC.ShieldStrengthTowerSolar = (int) this.reader.ReadUInt16();
            NPC.ShieldStrengthTowerVortex = (int) this.reader.ReadUInt16();
            NPC.ShieldStrengthTowerNebula = (int) this.reader.ReadUInt16();
            NPC.ShieldStrengthTowerStardust = (int) this.reader.ReadUInt16();
            if (NPC.ShieldStrengthTowerSolar < 0)
              NPC.ShieldStrengthTowerSolar = 0;
            if (NPC.ShieldStrengthTowerVortex < 0)
              NPC.ShieldStrengthTowerVortex = 0;
            if (NPC.ShieldStrengthTowerNebula < 0)
              NPC.ShieldStrengthTowerNebula = 0;
            if (NPC.ShieldStrengthTowerStardust < 0)
              NPC.ShieldStrengthTowerStardust = 0;
            if (NPC.ShieldStrengthTowerSolar > NPC.LunarShieldPowerExpert)
              NPC.ShieldStrengthTowerSolar = NPC.LunarShieldPowerExpert;
            if (NPC.ShieldStrengthTowerVortex > NPC.LunarShieldPowerExpert)
              NPC.ShieldStrengthTowerVortex = NPC.LunarShieldPowerExpert;
            if (NPC.ShieldStrengthTowerNebula > NPC.LunarShieldPowerExpert)
              NPC.ShieldStrengthTowerNebula = NPC.LunarShieldPowerExpert;
            if (NPC.ShieldStrengthTowerStardust <= NPC.LunarShieldPowerExpert)
              break;
            NPC.ShieldStrengthTowerStardust = NPC.LunarShieldPowerExpert;
            break;
          case 102:
            int index43 = (int) this.reader.ReadByte();
            ushort num101 = this.reader.ReadUInt16();
            Vector2 Other = this.reader.ReadVector2();
            if (Main.netMode == 2)
            {
              NetMessage.TrySendData(102, number: this.whoAmI, number2: ((float) num101), number3: Other.X, number4: Other.Y);
              break;
            }
            Player player17 = Main.player[index43];
            for (int index44 = 0; index44 < (int) byte.MaxValue; ++index44)
            {
              Player player18 = Main.player[index44];
              if (player18.active && !player18.dead && (player17.team == 0 || player17.team == player18.team) && (double) player18.Distance(Other) < 700.0)
              {
                Vector2 vector2_11 = player17.Center - player18.Center;
                Vector2 vec = Vector2.Normalize(vector2_11);
                if (!vec.HasNaNs())
                {
                  int num102 = 90;
                  float radians = 0.0f;
                  float num103 = 0.2094395f;
                  Vector2 spinningpoint = new Vector2(0.0f, -8f);
                  Vector2 vector2_12 = new Vector2(-3f);
                  float num104 = 0.0f;
                  float num105 = 0.005f;
                  switch (num101)
                  {
                    case 173:
                      num102 = 90;
                      break;
                    case 176:
                      num102 = 88;
                      break;
                    case 179:
                      num102 = 86;
                      break;
                  }
                  for (int index45 = 0; (double) index45 < (double) vector2_11.Length() / 6.0; ++index45)
                  {
                    Vector2 Position = player18.Center + 6f * (float) index45 * vec + spinningpoint.RotatedBy((double) radians) + vector2_12;
                    radians += num103;
                    int Type5 = num102;
                    Color newColor = new Color();
                    int index46 = Dust.NewDust(Position, 6, 6, Type5, Alpha: 100, newColor: newColor, Scale: 1.5f);
                    Main.dust[index46].noGravity = true;
                    Main.dust[index46].velocity = Vector2.Zero;
                    Main.dust[index46].fadeIn = (num104 += num105);
                    Main.dust[index46].velocity += vec * 1.5f;
                  }
                }
                player18.NebulaLevelup((int) num101);
              }
            }
            break;
          case 103:
            if (Main.netMode != 1)
              break;
            NPC.MoonLordCountdown = this.reader.ReadInt32();
            break;
          case 104:
            if (Main.netMode != 1 || Main.npcShop <= 0)
              break;
            Item[] objArray = Main.instance.shop[Main.npcShop].item;
            int index47 = (int) this.reader.ReadByte();
            int type15 = (int) this.reader.ReadInt16();
            int num106 = (int) this.reader.ReadInt16();
            int pre3 = (int) this.reader.ReadByte();
            int num107 = this.reader.ReadInt32();
            BitsByte bitsByte28 = (BitsByte) this.reader.ReadByte();
            if (index47 >= objArray.Length)
              break;
            objArray[index47] = new Item();
            objArray[index47].netDefaults(type15);
            objArray[index47].stack = num106;
            objArray[index47].Prefix(pre3);
            objArray[index47].value = num107;
            objArray[index47].buyOnce = bitsByte28[0];
            break;
          case 105:
            if (Main.netMode == 1)
              break;
            int i3 = (int) this.reader.ReadInt16();
            int num108 = (int) this.reader.ReadInt16();
            bool flag12 = this.reader.ReadBoolean();
            int j2 = num108;
            int num109 = flag12 ? 1 : 0;
            WorldGen.ToggleGemLock(i3, j2, num109 != 0);
            break;
          case 106:
            if (Main.netMode != 1)
              break;
            Utils.PoofOfSmoke(new HalfVector2()
            {
              PackedValue = this.reader.ReadUInt32()
            }.ToVector2());
            break;
          case 107:
            if (Main.netMode != 1)
              break;
            Color color4 = this.reader.ReadRGB();
            string text2 = NetworkText.Deserialize(this.reader).ToString();
            int num110 = (int) this.reader.ReadInt16();
            Color c = color4;
            int WidthLimit = num110;
            Main.NewTextMultiline(text2, c: c, WidthLimit: WidthLimit);
            break;
          case 108:
            if (Main.netMode != 1)
              break;
            int Damage = (int) this.reader.ReadInt16();
            float KnockBack = this.reader.ReadSingle();
            int x7 = (int) this.reader.ReadInt16();
            int y7 = (int) this.reader.ReadInt16();
            int angle = (int) this.reader.ReadInt16();
            int ammo = (int) this.reader.ReadInt16();
            int owner = (int) this.reader.ReadByte();
            if (owner != Main.myPlayer)
              break;
            WorldGen.ShootFromCannon(x7, y7, angle, ammo, Damage, KnockBack, owner, true);
            break;
          case 109:
            if (Main.netMode != 2)
              break;
            int x8 = (int) this.reader.ReadInt16();
            int num111 = (int) this.reader.ReadInt16();
            int x9 = (int) this.reader.ReadInt16();
            int y8 = (int) this.reader.ReadInt16();
            int num112 = (int) this.reader.ReadByte();
            int whoAmI3 = this.whoAmI;
            WiresUI.Settings.MultiToolMode toolMode = WiresUI.Settings.ToolMode;
            WiresUI.Settings.ToolMode = (WiresUI.Settings.MultiToolMode) num112;
            int y9 = num111;
            Wiring.MassWireOperation(new Point(x8, y9), new Point(x9, y8), Main.player[whoAmI3]);
            WiresUI.Settings.ToolMode = toolMode;
            break;
          case 110:
            if (Main.netMode != 1)
              break;
            int type16 = (int) this.reader.ReadInt16();
            int num113 = (int) this.reader.ReadInt16();
            int index48 = (int) this.reader.ReadByte();
            if (index48 != Main.myPlayer)
              break;
            Player player19 = Main.player[index48];
            for (int index49 = 0; index49 < num113; ++index49)
              player19.ConsumeItem(type16);
            player19.wireOperationsCooldown = 0;
            break;
          case 111:
            if (Main.netMode != 2)
              break;
            BirthdayParty.ToggleManualParty();
            break;
          case 112:
            int number42 = (int) this.reader.ReadByte();
            int num114 = this.reader.ReadInt32();
            int num115 = this.reader.ReadInt32();
            int num116 = (int) this.reader.ReadByte();
            int num117 = (int) this.reader.ReadInt16();
            switch (number42)
            {
              case 1:
                if (Main.netMode == 1)
                  WorldGen.TreeGrowFX(num114, num115, num116, num117);
                if (Main.netMode != 2)
                  return;
                NetMessage.TrySendData((int) num1, number: number42, number2: ((float) num114), number3: ((float) num115), number4: ((float) num116), number5: num117);
                return;
              case 2:
                NPC.FairyEffects(new Vector2((float) num114, (float) num115), num117);
                return;
              default:
                return;
            }
          case 113:
            int x10 = (int) this.reader.ReadInt16();
            int y10 = (int) this.reader.ReadInt16();
            if (Main.netMode != 2 || Main.snowMoon || Main.pumpkinMoon)
              break;
            if (DD2Event.WouldFailSpawningHere(x10, y10))
              DD2Event.FailureMessage(this.whoAmI);
            DD2Event.SummonCrystal(x10, y10, this.whoAmI);
            break;
          case 114:
            if (Main.netMode != 1)
              break;
            DD2Event.WipeEntities();
            break;
          case 115:
            int number43 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number43 = this.whoAmI;
            Main.player[number43].MinionAttackTargetNPC = (int) this.reader.ReadInt16();
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(115, ignoreClient: this.whoAmI, number: number43);
            break;
          case 116:
            if (Main.netMode != 1)
              break;
            DD2Event.TimeLeftBetweenWaves = this.reader.ReadInt32();
            break;
          case 117:
            int playerTargetIndex1 = (int) this.reader.ReadByte();
            if (Main.netMode == 2 && this.whoAmI != playerTargetIndex1 && (!Main.player[playerTargetIndex1].hostile || !Main.player[this.whoAmI].hostile))
              break;
            PlayerDeathReason playerDeathReason1 = PlayerDeathReason.FromReader(this.reader);
            int num118 = (int) this.reader.ReadInt16();
            int num119 = (int) this.reader.ReadByte() - 1;
            BitsByte bitsByte29 = (BitsByte) this.reader.ReadByte();
            bool flag13 = bitsByte29[0];
            bool pvp1 = bitsByte29[1];
            int num120 = (int) this.reader.ReadSByte();
            Main.player[playerTargetIndex1].Hurt(playerDeathReason1, num118, num119, pvp1, true, flag13, num120);
            if (Main.netMode != 2)
              break;
            NetMessage.SendPlayerHurt(playerTargetIndex1, playerDeathReason1, num118, num119, flag13, pvp1, num120, ignoreClient: this.whoAmI);
            break;
          case 118:
            int playerTargetIndex2 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              playerTargetIndex2 = this.whoAmI;
            PlayerDeathReason playerDeathReason2 = PlayerDeathReason.FromReader(this.reader);
            int num121 = (int) this.reader.ReadInt16();
            int num122 = (int) this.reader.ReadByte() - 1;
            bool pvp2 = ((BitsByte) this.reader.ReadByte())[0];
            Main.player[playerTargetIndex2].KillMe(playerDeathReason2, (double) num121, num122, pvp2);
            if (Main.netMode != 2)
              break;
            NetMessage.SendPlayerDeath(playerTargetIndex2, playerDeathReason2, num121, num122, pvp2, ignoreClient: this.whoAmI);
            break;
          case 119:
            if (Main.netMode != 1)
              break;
            int x11 = (int) this.reader.ReadSingle();
            int num123 = (int) this.reader.ReadSingle();
            Color color5 = this.reader.ReadRGB();
            NetworkText networkText = NetworkText.Deserialize(this.reader);
            int y11 = num123;
            CombatText.NewText(new Rectangle(x11, y11, 0, 0), color5, networkText.ToString());
            break;
          case 120:
            int index50 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              index50 = this.whoAmI;
            int num124 = (int) this.reader.ReadByte();
            if (num124 < 0 || num124 >= 151 || Main.netMode != 2)
              break;
            EmoteBubble.NewBubble(num124, new WorldUIAnchor((Entity) Main.player[index50]), 360);
            EmoteBubble.CheckForNPCsToReactToEmoteBubble(num124, Main.player[index50]);
            break;
          case 121:
            int num125 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              num125 = this.whoAmI;
            int num126 = this.reader.ReadInt32();
            int num127 = (int) this.reader.ReadByte();
            bool dye1 = false;
            if (num127 >= 8)
            {
              dye1 = true;
              num127 -= 8;
            }
            TileEntity tileEntity2;
            if (!TileEntity.ByID.TryGetValue(num126, out tileEntity2))
            {
              this.reader.ReadInt32();
              int num128 = (int) this.reader.ReadByte();
              break;
            }
            if (num127 >= 8)
              tileEntity2 = (TileEntity) null;
            if (tileEntity2 is TEDisplayDoll teDisplayDoll)
            {
              teDisplayDoll.ReadItem(num127, this.reader, dye1);
            }
            else
            {
              this.reader.ReadInt32();
              int num129 = (int) this.reader.ReadByte();
            }
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData((int) num1, ignoreClient: num125, number: num125, number2: ((float) num126), number3: ((float) num127), number4: ((float) dye1.ToInt()));
            break;
          case 122:
            int num130 = this.reader.ReadInt32();
            int number2_12 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number2_12 = this.whoAmI;
            if (Main.netMode == 2)
            {
              if (num130 == -1)
              {
                Main.player[number2_12].tileEntityAnchor.Clear();
                NetMessage.TrySendData((int) num1, number: num130, number2: ((float) number2_12));
                break;
              }
              TileEntity tileEntity3;
              if (!TileEntity.IsOccupied(num130, out int _) && TileEntity.ByID.TryGetValue(num130, out tileEntity3))
              {
                Main.player[number2_12].tileEntityAnchor.Set(num130, (int) tileEntity3.Position.X, (int) tileEntity3.Position.Y);
                NetMessage.TrySendData((int) num1, number: num130, number2: ((float) number2_12));
              }
            }
            if (Main.netMode != 1)
              break;
            if (num130 == -1)
            {
              Main.player[number2_12].tileEntityAnchor.Clear();
              break;
            }
            TileEntity tileEntity4;
            if (!TileEntity.ByID.TryGetValue(num130, out tileEntity4))
              break;
            TileEntity.SetInteractionAnchor(Main.player[number2_12], (int) tileEntity4.Position.X, (int) tileEntity4.Position.Y, num130);
            break;
          case 123:
            if (Main.netMode != 2)
              break;
            int x12 = (int) this.reader.ReadInt16();
            int num131 = (int) this.reader.ReadInt16();
            int num132 = (int) this.reader.ReadInt16();
            int num133 = (int) this.reader.ReadByte();
            int num134 = (int) this.reader.ReadInt16();
            int y12 = num131;
            int netid2 = num132;
            int prefix2 = num133;
            int stack2 = num134;
            TEWeaponsRack.TryPlacing(x12, y12, netid2, prefix2, stack2);
            break;
          case 124:
            int num135 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              num135 = this.whoAmI;
            int num136 = this.reader.ReadInt32();
            int num137 = (int) this.reader.ReadByte();
            bool dye2 = false;
            if (num137 >= 2)
            {
              dye2 = true;
              num137 -= 2;
            }
            TileEntity tileEntity5;
            if (!TileEntity.ByID.TryGetValue(num136, out tileEntity5))
            {
              this.reader.ReadInt32();
              int num138 = (int) this.reader.ReadByte();
              break;
            }
            if (num137 >= 2)
              tileEntity5 = (TileEntity) null;
            if (tileEntity5 is TEHatRack teHatRack)
            {
              teHatRack.ReadItem(num137, this.reader, dye2);
            }
            else
            {
              this.reader.ReadInt32();
              int num139 = (int) this.reader.ReadByte();
            }
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData((int) num1, ignoreClient: num135, number: num135, number2: ((float) num136), number3: ((float) num137), number4: ((float) dye2.ToInt()));
            break;
          case 125:
            int num140 = (int) this.reader.ReadByte();
            int num141 = (int) this.reader.ReadInt16();
            int num142 = (int) this.reader.ReadInt16();
            int num143 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              num140 = this.whoAmI;
            if (Main.netMode == 1)
              Main.player[Main.myPlayer].GetOtherPlayersPickTile(num141, num142, num143);
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(125, ignoreClient: num140, number: num140, number2: ((float) num141), number3: ((float) num142), number4: ((float) num143));
            break;
          case 126:
            if (Main.netMode != 1)
              break;
            NPC.RevengeManager.AddMarkerFromReader(this.reader);
            break;
          case 127:
            int markerUniqueID = this.reader.ReadInt32();
            if (Main.netMode != 1)
              break;
            NPC.RevengeManager.DestroyMarker(markerUniqueID);
            break;
          case 128:
            int num144 = (int) this.reader.ReadByte();
            int num145 = (int) this.reader.ReadUInt16();
            int num146 = (int) this.reader.ReadUInt16();
            int num147 = (int) this.reader.ReadUInt16();
            int num148 = (int) this.reader.ReadUInt16();
            if (Main.netMode == 2)
            {
              NetMessage.SendData(128, ignoreClient: num144, number: num144, number2: ((float) num147), number3: ((float) num148), number5: num145, number6: num146);
              break;
            }
            GolfHelper.ContactListener.PutBallInCup_TextAndEffects(new Point(num145, num146), num144, num147, num148);
            break;
          case 129:
            if (Main.netMode != 1)
              break;
            Main.FixUIScale();
            Main.TrySetPreparationState(Main.WorldPreparationState.ProcessingData);
            break;
          case 130:
            if (Main.netMode != 2)
              break;
            int num149 = (int) this.reader.ReadUInt16();
            int num150 = (int) this.reader.ReadUInt16();
            int Type6 = (int) this.reader.ReadInt16();
            int X = num149 * 16;
            int Y = num150 * 16;
            NPC npc5 = new NPC();
            npc5.SetDefaults(Type6);
            int type17 = npc5.type;
            int netId = npc5.netID;
            int number44 = NPC.NewNPC((IEntitySource) new EntitySource_FishedOut((Entity) Main.player[this.whoAmI]), X, Y, Type6);
            if (netId == type17)
              break;
            Main.npc[number44].SetDefaults(netId);
            NetMessage.TrySendData(23, number: number44);
            break;
          case 131:
            if (Main.netMode != 1)
              break;
            int index51 = (int) this.reader.ReadUInt16();
            NPC npc6 = index51 >= 200 ? new NPC() : Main.npc[index51];
            if (this.reader.ReadByte() != (byte) 1)
              break;
            int time3 = this.reader.ReadInt32();
            int fromWho = (int) this.reader.ReadInt16();
            npc6.GetImmuneTime(fromWho, time3);
            break;
          case 132:
            if (Main.netMode != 1)
              break;
            Point point = this.reader.ReadVector2().ToPoint();
            ushort key2 = this.reader.ReadUInt16();
            LegacySoundStyle legacySoundStyle = SoundID.SoundByIndex[key2];
            BitsByte bitsByte30 = (BitsByte) this.reader.ReadByte();
            int Style2 = !bitsByte30[0] ? legacySoundStyle.Style : this.reader.ReadInt32();
            float volumeScale = !bitsByte30[1] ? legacySoundStyle.Volume : MathHelper.Clamp(this.reader.ReadSingle(), 0.0f, 1f);
            float pitchOffset = !bitsByte30[2] ? legacySoundStyle.GetRandomPitch() : MathHelper.Clamp(this.reader.ReadSingle(), -1f, 1f);
            SoundEngine.PlaySound(legacySoundStyle.SoundId, point.X, point.Y, Style2, volumeScale, pitchOffset);
            break;
          case 133:
            if (Main.netMode != 2)
              break;
            int x13 = (int) this.reader.ReadInt16();
            int num151 = (int) this.reader.ReadInt16();
            int num152 = (int) this.reader.ReadInt16();
            int num153 = (int) this.reader.ReadByte();
            int num154 = (int) this.reader.ReadInt16();
            int y13 = num151;
            int netid3 = num152;
            int prefix3 = num153;
            int stack3 = num154;
            TEFoodPlatter.TryPlacing(x13, y13, netid3, prefix3, stack3);
            break;
          case 134:
            int index52 = (int) this.reader.ReadByte();
            int num155 = this.reader.ReadInt32();
            float num156 = this.reader.ReadSingle();
            byte num157 = this.reader.ReadByte();
            bool flag14 = this.reader.ReadBoolean();
            if (Main.netMode == 2)
              index52 = this.whoAmI;
            Player player20 = Main.player[index52];
            player20.ladyBugLuckTimeLeft = num155;
            player20.torchLuck = num156;
            player20.luckPotion = num157;
            player20.HasGardenGnomeNearby = flag14;
            player20.RecalculateLuck();
            if (Main.netMode != 2)
              break;
            NetMessage.SendData(134, ignoreClient: index52, number: index52);
            break;
          case 135:
            int index53 = (int) this.reader.ReadByte();
            if (Main.netMode != 1)
              break;
            Main.player[index53].immuneAlpha = (int) byte.MaxValue;
            break;
          case 136:
            for (int index54 = 0; index54 < 2; ++index54)
            {
              for (int index55 = 0; index55 < 3; ++index55)
                NPC.cavernMonsterType[index54, index55] = (int) this.reader.ReadUInt16();
            }
            break;
          case 137:
            if (Main.netMode != 2)
              break;
            int index56 = (int) this.reader.ReadInt16();
            int buffTypeToRemove = (int) this.reader.ReadUInt16();
            if (index56 < 0 || index56 >= 200)
              break;
            Main.npc[index56].RequestBuffRemoval(buffTypeToRemove);
            break;
          case 139:
            if (Main.netMode == 2)
              break;
            int index57 = (int) this.reader.ReadByte();
            bool flag15 = this.reader.ReadBoolean();
            Main.countsAsHostForGameplay[index57] = flag15;
            break;
          case 140:
            if (Main.netMode != 1)
              break;
            int num158 = (int) this.reader.ReadByte();
            CreditsRollEvent.SetRemainingTimeDirect(this.reader.ReadInt32());
            break;
          case 141:
            LucyAxeMessage.MessageSource messageSource = (LucyAxeMessage.MessageSource) this.reader.ReadByte();
            byte num159 = this.reader.ReadByte();
            Vector2 velocity = this.reader.ReadVector2();
            int num160 = this.reader.ReadInt32();
            int num161 = this.reader.ReadInt32();
            if (Main.netMode == 2)
            {
              NetMessage.SendData(141, ignoreClient: this.whoAmI, number: ((int) messageSource), number2: ((float) num159), number3: velocity.X, number4: velocity.Y, number5: num160, number6: num161);
              break;
            }
            LucyAxeMessage.CreateFromNet(messageSource, num159, new Vector2((float) num160, (float) num161), velocity);
            break;
          case 142:
            int number45 = (int) this.reader.ReadByte();
            if (Main.netMode == 2)
              number45 = this.whoAmI;
            Player player21 = Main.player[number45];
            player21.piggyBankProjTracker.TryReading(this.reader);
            player21.voidLensChest.TryReading(this.reader);
            if (Main.netMode != 2)
              break;
            NetMessage.TrySendData(142, ignoreClient: this.whoAmI, number: number45);
            break;
          default:
            if (Netplay.Clients[this.whoAmI].State != 0)
              break;
            NetMessage.BootPlayer(this.whoAmI, Lang.mp[2].ToNetworkText());
            break;
        }
      }
    }
  }
}
