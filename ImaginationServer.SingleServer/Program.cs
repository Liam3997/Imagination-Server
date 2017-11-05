using System;
using static System.Console;
using System.Windows.Forms;
using Newtonsoft.Json;
using FluentNHibernate.Cfg;
using ImaginationServer.Auth;
using ImaginationServer.Common;
using ImaginationServer.World;
using ImaginationServer.SQL_DB;
using System.Threading;

namespace ImaginationServer.SingleServer
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting Imagination Server!");
            Console.WriteLine("Loading config...");
            Config.Init();
            Console.WriteLine($"Address: {Config.Current.Address}");
            Console.WriteLine($"Encrypt Packets: {Config.Current.EncryptPackets}");
            Console.WriteLine(" ->OK");
            try
            {
                Console.WriteLine("Setting up database...");
                SessionHelper.Init();
                Console.WriteLine(" ->OK");
                Console.WriteLine("Setting up CDClient Database...");
                CdClientDb.Init();
                Console.WriteLine(" ->OK");
            }
            catch(FluentConfigurationException exception)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.InnerException);
                foreach(var reason in exception.PotentialReasons) Console.WriteLine(" - " + reason);
                Console.ReadKey(true);
                Environment.Exit(-1);
            }
            Console.WriteLine("Starting Auth...");
            AuthServer.Init(Config.Current.Address);
            Console.WriteLine(" ->OK");
            Console.WriteLine("Starting World...");
            WorldServer.Init(Config.Current.Address);
            Console.WriteLine(" ->OK");
            Console.WriteLine("Beginning message receiving...");
            new Thread(Command).Start();
            while (!Environment.HasShutdownStarted)
            {
                WorldServer.Service();
                AuthServer.Service();
                
            }
           
        }


        
        [STAThread]
        public static void Command()
        {
            while (!Environment.HasShutdownStarted)
            {
                var input = ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;
                var split = input.Split(new[] { ' ' }, 2);
                var cmd = split[0].ToLower();
                var cmdArgs = split.Length > 1 ? split[1].Split(' ') : null;
                using (var database = new DbUtils())
                    switch (cmd)
                    {
                        case "help":
                            WriteLine("addaccount <username> <password>");
                            WriteLine("removeaccount <username>");
                            WriteLine("accountexists <username>");
                            WriteLine("ban <username>");
                            WriteLine("unban <username>");
                            WriteLine("printinfo <a for account, anything else for character> <username> [clipboard]");
                            WriteLine("deletecharacter <username>");
                            break;
                        case "deletecharacter":
                            if (cmdArgs?.Length >= 1)
                            {
                                var name = cmdArgs[0];
                                if (!database.CharacterExists(name))
                                {
                                    WriteLine("Character does not exist.");
                                    continue;
                                }

                                database.DeleteCharacter(database.GetCharacter(name));

                                WriteLine("Success!");
                                continue;
                            }

                            WriteLine("Invalid Arguments");
                            break;
                        case "printinfo":
                            if (cmdArgs?.Length >= 2)
                            {
                                var type = cmdArgs[0];
                                var username = cmdArgs[1];
                                if (type == "a" && !database.AccountExists(username))
                                {
                                    WriteLine("Account does not exist.");
                                    continue;
                                }
                                else if (type != "a" && !database.CharacterExists(username))
                                {
                                    WriteLine("Character does not exist!");
                                    continue;
                                }
                                var account =
                                    JsonConvert.SerializeObject(
                                        (type == "a"
                                            ? (dynamic) database.GetAccount(username)
                                            : (dynamic) database.GetCharacter(username)), Formatting.Indented);
                                WriteLine(account);
                                if (cmdArgs.Length >= 3)
                                    Clipboard.SetText(account);
                                continue;
                            }

                            WriteLine("Invalid Arguments!");
                            break;
                        case "addaccount":
                            if (cmdArgs != null && cmdArgs.Length >= 2)
                            {
                                var username = cmdArgs[0];
                                var password = cmdArgs[1];
                                database.CreateAccount(username, password);

                                WriteLine("Success!");
                                continue;
                            }

                            WriteLine("Invalid Arguments.");
                            break;
                        case "ban":
                            if (cmdArgs?.Length >= 1)
                            {
                                var username = cmdArgs[0];
                                if (!database.AccountExists(username))
                                {
                                    WriteLine("User does not exist.");
                                    continue;
                                }
                                var account = database.GetAccount(username);
                                account.Banned = true;
                                database.UpdateAccount(account);
                                WriteLine("Success!");
                                continue;
                            }

                            WriteLine("Invalid Arguments.");
                            break;
                        case "unban":
                            if (cmdArgs?.Length >= 1)
                            {
                                var username = cmdArgs[0];
                                if (!database.AccountExists(username))
                                {
                                    WriteLine("User does not exist.");
                                    continue;
                                }
                                var account = database.GetAccount(username);
                                account.Banned = false;
                                database.UpdateAccount(account);
                                WriteLine("Success!");
                                continue;
                            }

                            WriteLine("Invalid Arguments.");
                            break;
                        case "removeaccount":
                            if (cmdArgs?.Length >= 1)
                            {
                                var username = cmdArgs[0];
                                if (database.AccountExists(username))
                                {
                                    database.DeleteAccount(database.GetAccount(username));
                                    WriteLine("Success!");
                                    continue;
                                }

                                WriteLine("User does not exist.");
                                continue;
                            }

                            WriteLine("Invalid Arguments.");
                            break;
                        case "accountexists":
                            if (cmdArgs != null && cmdArgs.Length >= 1)
                            {
                                var username = cmdArgs[0];
                                WriteLine(database.AccountExists(username));
                                continue;
                            }

                            WriteLine("Invalid Arguments.");
                            break;
                        default:
                            WriteLine("Unknown command.");
                            break;
                    }
            }
        }

    }
}
