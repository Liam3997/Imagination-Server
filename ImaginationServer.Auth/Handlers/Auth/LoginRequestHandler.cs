using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ImaginationServer.Auth.Packets.Auth;
using ImaginationServer.Common;
using ImaginationServer.Common.Handlers;
using static System.Console;

namespace ImaginationServer.Auth.Handlers.Auth
{
    public class LoginRequestHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, LuClient client)
        {
            using (var database = new DbUtils())
            {
                var loginRequest = new LoginRequest(reader);
                WriteLine($"{loginRequest.Username} sent authentication request.");

                byte valid = 0x01;
                if (!database.AccountExists(loginRequest.Username))
                {
                    valid = 0x06;
                }

                if (valid == 0x01)
                {
                    var account = database.GetAccount(loginRequest.Username);
                    var hash =
                        SHA512.Create()
                            .ComputeHash(Encoding.Unicode.GetBytes(loginRequest.Password).Concat(account.Salt).ToArray());
                    if (!account.Password.SequenceEqual(hash)) valid = 0x06;
                    if (valid == 0x01 && account.Banned) valid = 0x02;
                }

                var message = "screwed up.";
                switch (valid)
                {
                    case 0x01:
                        message = "was successful.";
                        break;
                    case 0x02:
                        message = "failed: Banned.";
                        break;
                    case 0x05:
                        message = "failed: Account permissions not high enough";
                        // If a custom message is included in the packet it will be displayed in frontend, otherwise the message for “General Failed” will be shown
                        break;
                    case 0x06:
                        message = "failed: Invalid Credentials.";
                        break;
                    case 0x07:
                        message = "failed: Account is currently locked (wrong password entered too many times)";
                        break;
                    case 0x08:
                        message = "failed: Invalid Username(?)";
                        // 0x06 appeared in the logged traffic due to an invalid password
                        break;
                    case 0x09:
                        message = "failed: Account Activation Pending";
                        // Shows the same ingame message as 0x06/0x08
                        break;
                    case 0x0a:
                        message = "failed: Account is disabled";
                        // Shows the same ingame message as 0x06/0x08
                        break;
                    case 0x0b:
                        message = "failed: Game Time Expired";
                        break;
                    case 0x0c:
                        message = "failed: Free Trial Has Ended";
                        break;
                    case 0x0d:
                        message = "failed: Play schedule not allowing it";
                        break;
                    case 0x0e:
                        message = "failed: Account not activated";
                        break;
                    default:
                        WriteLine(
                            "FATAL: Magically, the valid variable was not a recognized byte value! (How is that even possible..? I'm only checking because resharper is making me.)");
                        break;
                }

                WriteLine("User login " + message);

                string userKey = RandomString(66);

                if (valid == 0x01)
                {
                    // TODO: Store user key
                }

                // Send our Response
                var response = new LoginResponse(valid, userKey);
                response.Send(client.Address);
            }
        }

        private string RandomString(int length,
            string allowedChars = "0123456789abcdef")
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length), "length cannot be less than zero.");
            if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length)
                throw new ArgumentException(
                    $"allowedChars may contain no more than {byteSize} characters.");

            // Guid.NewGuid and System.Random are not particularly random. By using a
            // cryptographically-secure random number generator, the caller is always
            // protected, regardless of use.
            using (var rng = new RNGCryptoServiceProvider())
            {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        // Divide the byte into allowedCharSet-sized groups. If the
                        // random value falls into the last group and the last group is
                        // too small to choose from the entire allowedCharSet, ignore
                        // the value in order to avoid biasing the result.
                        var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }
                return result.ToString();
            }
        }
    }
}