﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Social.WeGame.NetServerSocialModule
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using rail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Terraria.Localization;
using Terraria.Net;
using Terraria.Net.Sockets;

namespace Terraria.Social.WeGame
{
  public class NetServerSocialModule : NetSocialModule
  {
    private SocketConnectionAccepted _connectionAcceptedCallback;
    private bool _acceptingClients;
    private ServerMode _mode;
    private RailCallBackHelper _callbackHelper = new RailCallBackHelper();
    private MessageDispatcherClient _client = new MessageDispatcherClient();
    private bool _serverConnected;
    private RailID _serverID = new RailID();
    private Action _ipcConnetedAction;
    private List<RailFriendInfo> _wegameFriendList;

    public NetServerSocialModule() => this._lobby._lobbyCreatedExternalCallback = new Action<RailID>(this.OnLobbyCreated);

    private void BroadcastConnectedUsers()
    {
    }

    private bool AcceptAnUserSession(RailID local_peer, RailID remote_peer)
    {
      bool flag = false;
      WeGameHelper.WriteDebugString("AcceptAnUserSession server:" + ((RailComparableID) local_peer).id_.ToString() + " remote:" + ((RailComparableID) remote_peer).id_.ToString());
      IRailNetwork irailNetwork = rail_api.RailFactory().RailNetworkHelper();
      if (irailNetwork != null)
        flag = irailNetwork.AcceptSessionRequest(local_peer, remote_peer) == 0;
      return flag;
    }

    private void TerminateRemotePlayerSession(RailID remote_id) => rail_api.RailFactory().RailPlayer()?.TerminateSessionOfPlayer(remote_id);

    private bool CloseNetWorkSession(RailID remote_peer)
    {
      bool flag = false;
      IRailNetwork irailNetwork = rail_api.RailFactory().RailNetworkHelper();
      if (irailNetwork != null)
        flag = irailNetwork.CloseSession(this._serverID, remote_peer) == 0;
      return flag;
    }

    private RailID GetServerID()
    {
      RailID railId = (RailID) null;
      IRailGameServer server = this._lobby.GetServer();
      if (server != null)
        railId = server.GetGameServerRailID();
      return railId ?? new RailID();
    }

    private void CloseAndUpdateUserState(RailID remote_peer)
    {
      if (!this._connectionStateMap.ContainsKey(remote_peer))
        return;
      WeGameHelper.WriteDebugString("CloseAndUpdateUserState, remote:{0}", (object) ((RailComparableID) remote_peer).id_);
      this.TerminateRemotePlayerSession(remote_peer);
      this.CloseNetWorkSession(remote_peer);
      this._connectionStateMap[remote_peer] = NetSocialModule.ConnectionState.Inactive;
      this._reader.ClearUser(remote_peer);
      this._writer.ClearUser(remote_peer);
    }

    public void OnConnected()
    {
      this._serverConnected = true;
      if (this._ipcConnetedAction != null)
        this._ipcConnetedAction();
      this._ipcConnetedAction = (Action) null;
      WeGameHelper.WriteDebugString("IPC connected");
    }

    private void OnCreateSessionRequest(CreateSessionRequest data)
    {
      if (!this._acceptingClients)
        WeGameHelper.WriteDebugString(" - Ignoring connection from " + ((RailComparableID) data.remote_peer).id_.ToString() + " while _acceptionClients is false.");
      else if (!this._mode.HasFlag((Enum) ServerMode.FriendsOfFriends) && !this.IsWeGameFriend(data.remote_peer))
      {
        WeGameHelper.WriteDebugString("Ignoring connection from " + ((RailComparableID) data.remote_peer).id_.ToString() + ". Friends of friends is disabled.");
      }
      else
      {
        WeGameHelper.WriteDebugString("pass wegame friend check");
        this.AcceptAnUserSession(data.local_peer, data.remote_peer);
        this._connectionStateMap[data.remote_peer] = NetSocialModule.ConnectionState.Authenticating;
        if (this._connectionAcceptedCallback == null)
          return;
        this._connectionAcceptedCallback((ISocket) new SocialSocket((RemoteAddress) new WeGameAddress(data.remote_peer, "")));
      }
    }

    private void OnCreateSessionFailed(CreateSessionFailed data)
    {
      WeGameHelper.WriteDebugString("CreateSessionFailed, local:{0}, remote:{1}", (object) ((RailComparableID) data.local_peer).id_, (object) ((RailComparableID) data.remote_peer).id_);
      this.CloseAndUpdateUserState(data.remote_peer);
    }

    private bool GetRailFriendList(List<RailFriendInfo> list)
    {
      bool railFriendList = false;
      IRailFriends irailFriends = rail_api.RailFactory().RailFriends();
      if (irailFriends != null)
        railFriendList = irailFriends.GetFriendsList(list) == 0;
      return railFriendList;
    }

    private void OnWegameMessage(IPCMessage message)
    {
      if (message.GetCmd() != IPCMessageType.IPCMessageTypeNotifyFriendList)
        return;
      WeGameFriendListInfo friendListInfo;
      message.Parse<WeGameFriendListInfo>(out friendListInfo);
      this.UpdateFriendList(friendListInfo);
    }

    private void UpdateFriendList(WeGameFriendListInfo friendListInfo)
    {
      this._wegameFriendList = friendListInfo._friendList;
      WeGameHelper.WriteDebugString("On update friend list - " + this.DumpFriendListString(friendListInfo._friendList));
    }

    private bool IsWeGameFriend(RailID id)
    {
      bool flag = false;
      if (this._wegameFriendList != null)
      {
        foreach (RailFriendInfo wegameFriend in this._wegameFriendList)
        {
          if (RailComparableID.op_Equality((RailComparableID) wegameFriend.friend_rail_id, (RailComparableID) id))
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    private string DumpFriendListString(List<RailFriendInfo> list)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (RailFriendInfo railFriendInfo in list)
        stringBuilder.AppendLine(string.Format("friend_id: {0}, type: {1}, online: {2}, playing: {3}", (object) ((RailComparableID) railFriendInfo.friend_rail_id).id_, (object) railFriendInfo.friend_type, (object) railFriendInfo.online_state.friend_online_state.ToString(), (object) railFriendInfo.online_state.game_define_game_playing_state));
      return stringBuilder.ToString();
    }

    private bool IsActiveUser(RailID user) => this._connectionStateMap.ContainsKey(user) && (uint) this._connectionStateMap[user] > 0U;

    private void UpdateUserStateBySessionAuthResult(GameServerStartSessionWithPlayerResponse data)
    {
      RailID remoteRailId = data.remote_rail_id;
      RailResult result = ((EventBase) data).result;
      if (!this._connectionStateMap.ContainsKey(remoteRailId))
        return;
      if (result == null)
      {
        WeGameHelper.WriteDebugString("UpdateUserStateBySessionAuthResult Auth Success");
        this.BroadcastConnectedUsers();
      }
      else
      {
        WeGameHelper.WriteDebugString("UpdateUserStateBySessionAuthResult Auth Failed");
        this.CloseAndUpdateUserState(remoteRailId);
      }
    }

    private bool TryAuthUserByRecvData(RailID user, byte[] data, int length)
    {
      WeGameHelper.WriteDebugString("TryAuthUserByRecvData user:{0}", (object) ((RailComparableID) user).id_);
      if (length < 3)
      {
        WeGameHelper.WriteDebugString("Failed to validate authentication packet: Too short. (Length: " + length.ToString() + ")");
        return false;
      }
      int num = (int) data[1] << 8 | (int) data[0];
      if (num != length)
      {
        WeGameHelper.WriteDebugString("Failed to validate authentication packet: Packet size mismatch. (" + num.ToString() + "!=" + length.ToString() + ")");
        return false;
      }
      if (data[2] == (byte) 93)
        return true;
      WeGameHelper.WriteDebugString("Failed to validate authentication packet: Packet type is not correct. (Type: " + data[2].ToString() + ")");
      return false;
    }

    private bool OnPacketRead(byte[] data, int size, RailID user)
    {
      if (!this.IsActiveUser(user))
      {
        WeGameHelper.WriteDebugString("OnPacketRead IsActiveUser false");
        return false;
      }
      NetSocialModule.ConnectionState connectionState = this._connectionStateMap[user];
      if (connectionState != NetSocialModule.ConnectionState.Authenticating)
        return connectionState == NetSocialModule.ConnectionState.Connected;
      if (!this.TryAuthUserByRecvData(user, data, size))
        this.CloseAndUpdateUserState(user);
      else
        this.OnAuthSuccess(user);
      return false;
    }

    private void OnAuthSuccess(RailID remote_peer)
    {
      if (!this._connectionStateMap.ContainsKey(remote_peer))
        return;
      this._connectionStateMap[remote_peer] = NetSocialModule.ConnectionState.Connected;
      int length = 3;
      byte[] numArray = new byte[length];
      numArray[0] = (byte) (length & (int) byte.MaxValue);
      numArray[1] = (byte) (length >> 8 & (int) byte.MaxValue);
      numArray[2] = (byte) 93;
      rail_api.RailFactory().RailNetworkHelper().SendReliableData(this._serverID, remote_peer, numArray, (uint) length);
    }

    public void OnRailEvent(RAILEventID event_id, EventBase data)
    {
      WeGameHelper.WriteDebugString("OnRailEvent,id=" + event_id.ToString() + " ,result=" + data.result.ToString());
      if (event_id != 3006)
      {
        if (event_id != 16001)
        {
          if (event_id != 16002)
            return;
          this.OnCreateSessionFailed((CreateSessionFailed) data);
        }
        else
          this.OnCreateSessionRequest((CreateSessionRequest) data);
      }
      else
        this.UpdateUserStateBySessionAuthResult((GameServerStartSessionWithPlayerResponse) data);
    }

    private void OnLobbyCreated(RailID lobbyID)
    {
      WeGameHelper.WriteDebugString("SetLocalPeer: {0}", (object) ((RailComparableID) lobbyID).id_);
      this._reader.SetLocalPeer(lobbyID);
      this._writer.SetLocalPeer(lobbyID);
      this._serverID = lobbyID;
      Action action = (Action) (() =>
      {
        ReportServerID t = new ReportServerID()
        {
          _serverID = ((RailComparableID) lobbyID).id_.ToString()
        };
        IPCMessage msg = new IPCMessage();
        msg.Build<ReportServerID>(IPCMessageType.IPCMessageTypeReportServerID, t);
        WeGameHelper.WriteDebugString("Send serverID to game client - " + this._client.SendMessage(msg).ToString());
      });
      if (this._serverConnected)
      {
        action();
      }
      else
      {
        this._ipcConnetedAction += action;
        WeGameHelper.WriteDebugString("report server id fail, no connection");
      }
    }

    private void RegisterRailEvent()
    {
      RAILEventID[] railEventIdArray = new RAILEventID[4]
      {
        (RAILEventID) 16001,
        (RAILEventID) 16002,
        (RAILEventID) 3006,
        (RAILEventID) 3005
      };
      foreach (int num in railEventIdArray)
      {
        // ISSUE: method pointer
        this._callbackHelper.RegisterCallback((RAILEventID) num, new RailEventCallBackHandler((object) this, __methodptr(OnRailEvent)));
      }
    }

    public override void Initialize()
    {
      base.Initialize();
      this._mode |= ServerMode.Lobby;
      this.RegisterRailEvent();
      this._reader.SetReadEvent(new WeGameP2PReader.OnReadEvent(this.OnPacketRead));
      if (Program.LaunchParameters.ContainsKey("-lobby"))
      {
        this._mode |= ServerMode.Lobby;
        string launchParameter = Program.LaunchParameters["-lobby"];
        if (!(launchParameter == "private"))
        {
          if (launchParameter == "friends")
          {
            this._mode |= ServerMode.FriendsCanJoin;
            this._lobby.Create(false);
          }
          else
            Console.WriteLine(Language.GetTextValue("Error.InvalidLobbyFlag", (object) "private", (object) "friends"));
        }
        else
          this._lobby.Create(true);
      }
      if (Program.LaunchParameters.ContainsKey("-friendsoffriends"))
        this._mode |= ServerMode.FriendsOfFriends;
      this._client.Init("WeGame.Terraria.Message.Client", "WeGame.Terraria.Message.Server");
      this._client.OnConnected += new Action(this.OnConnected);
      this._client.OnMessage += new Action<IPCMessage>(this.OnWegameMessage);
      CoreSocialModule.OnTick += new Action(this._client.Tick);
      this._client.Start();
    }

    public override ulong GetLobbyId() => ((RailComparableID) this._serverID).id_;

    public override void OpenInviteInterface()
    {
    }

    public override void CancelJoin()
    {
    }

    public override bool CanInvite() => false;

    public override void LaunchLocalServer(Process process, ServerMode mode)
    {
    }

    public override bool StartListening(SocketConnectionAccepted callback)
    {
      this._acceptingClients = true;
      this._connectionAcceptedCallback = callback;
      return false;
    }

    public override void StopListening() => this._acceptingClients = false;

    public override void Connect(RemoteAddress address)
    {
    }

    public override void Close(RemoteAddress address) => this.CloseAndUpdateUserState(this.RemoteAddressToRailId(address));
  }
}
