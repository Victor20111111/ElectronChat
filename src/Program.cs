using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using ElectronChat.Networking;
using ElectronChat.Views;

namespace ElectronChat
{
    class Program
    {
        
        public static Settings settings;
        public static List<Chat> chats = new List<Chat>();
        public static Nat nat;
        
        static void Main(string[] args)
        {
            settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText("settings.json"));
            Listener listener = new Listener(IPAddress.Parse(settings.IP), settings.Port);
            
            if (settings.EnableUPNP)
                new Thread(() =>
                {
                    nat = new Nat();
                    nat.Search();
                }).Start();
            
            AppDomain.CurrentDomain.ProcessExit += OnClose;

            if (!args.Contains("--nogui"))
                App.Create(args);
        }

        static void OnClose(object sender, EventArgs e)
        {
            if (settings.EnableUPNP)
                nat.RemoveUPNP();
        }

    }
}
