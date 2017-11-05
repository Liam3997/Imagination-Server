using System;


using ImaginationServer.Auth.Handlers.Auth;
using ImaginationServer.Common;


using static ImaginationServer.Enums.PacketEnums;
using static ImaginationServer.Enums.PacketEnums.ClientAuthPacketId;


namespace ImaginationServer.Auth
{
    public class AuthServer
    {
        public static LuServer Server;

        public static void Init(string address)
        {
            Server = new LuServer(1001, 1000, address);
            Server.AddHandler((byte) RemoteConnection.Auth, (byte) MsgAuthLoginRequest, new LoginRequestHandler());
            Server. Start(Config.Current.EncryptPackets);

        }


        public static void Service()
        {
            Server.Service();
        }
    }
}