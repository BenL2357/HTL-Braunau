using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.DirectoryServices;
using WLANLib;

namespace ARES_Terminal
{
    public static class ARES_Engine
    {

        public static void WriteARES(string I_Text)
        {
            SetARESColor();
            WriteSlow(I_Text);
        }

        public static void WriteUSER(string I_Text)
        {
            SetUserColor();
            WriteSlow(I_Text);
        }

        public static void WriteFast(string I_Text)
        {
            WriteTimeDelayed(I_Text, 20);
        }

        public static void WriteSlow(string I_Text)
        {
            WriteTimeDelayed(I_Text, 50);
        }

        public static void WriteTimeDelayed(string I_Text, int iDelay)
        {
            foreach (char c in I_Text)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(iDelay);
            }
            Console.WriteLine();
        }

        public static void SetARESColor()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public static void SetUserColor()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }

        public static bool HandleInput(string strInp)
        {
            if (strInp.ToLower().Contains("exit"))
            {
                return false;
            }
            else if(strInp.ToLower().Contains("help"))
            {
                HandleHelpCommand();
            }
            else if(strInp.ToLower().Contains("commands"))
            {
                HandleCommandsCommand();
            }
            else if (strInp.ToLower().Contains("bluetooth"))
            {
                HandleCommandBluetooth();
            }
            else if(strInp.ToLower().Contains("user list"))
            {
                HandleUserListCommand();
            }
            else if(strInp.ToLower().Contains("wlan"))
            {
                HandleWLANCommand();
            }
            else if(strInp.ToLower().Contains("deathray"))
            {
                HandleDeathrayCommand();
            }
            else if(strInp.ToLower().Contains("symbol"))
            {
                PaintSymbol();
            }
            else if(strInp.ToLower().Contains("logo"))
            {
                SetUserColor();
                PaintBanner();
                Console.ReadKey();
                SetARESColor();
            }
            return true;

        }

        public static void HandleCommandBluetooth()
        {
            Console.Clear();
            WriteSlow("Ok, I am searching for near Bluetooth devices...");
            List<string> NearDevs;
            if (GetNearBTDevices(out NearDevs))
            {
                WriteSlow("Finished scanning. This is what I found:");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (NearDevs != null && NearDevs.Count > 0)
                {
                    foreach (string dev in NearDevs) Console.WriteLine(dev);
                }
                else Console.WriteLine("---Nothing---");
            }
            else
            {
                WriteSlow("Sorry, I was unable to use Bluetooth. Please check, if the provider is currently running!");
            }

            SetARESColor();

            Console.ReadKey();
        }

        public static void HandleHelpCommand()
        {
            Console.Clear();
            WriteSlow("Hi.");
            WriteSlow("My name is ARES. I am a smart, artificial terminal created to help you");
            WriteSlow("I can access different components of your computer and check the status for you");
            WriteSlow("For Example, if you want to find all near bluetooth devices, just type in \"Bluetooth\"");
            WriteSlow("If you need a complete list of my commands, type in \"Commands\"");
            Console.ReadKey();
        }

        public static void HandleCommandsCommand()
        {
            Console.Clear();
            WriteUSER("Bluetooth");
            WriteARES("Find near bluetooth devices");
            Console.WriteLine();
            Console.WriteLine();
            WriteUSER("User List");
            WriteARES("Shows the list of the users on this computer");
            WriteUSER("Wlan");
            WriteARES("Find near networks");
            Console.WriteLine();
            WriteUSER("Deathray");
            WriteARES("Emits a high pitched noise");
            Console.WriteLine();
            WriteUSER("Help");
            WriteARES("Opens the help");
            Console.WriteLine();
            WriteUSER("Commands");
            WriteARES("Shows the list of avaiable commands");
            Console.ReadKey();
        }

        public static void HandleWLANCommand()
        {
            Console.Clear();
            WriteARES("Scanning for near wifis...");
            List<WLANInformation> Wlans = Netsh.GetVisibleNetworks();

            WriteSlow("Finished scanning. This is what I found:");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (Wlans != null && Wlans.Count > 0)
            {
                foreach (WLANInformation wlan in Wlans) Console.WriteLine(wlan.Name);
            }
            else Console.WriteLine("---Nothing---");
            SetARESColor();
            Console.ReadKey();
        }
        public static void HandleUserListCommand()
        {
            Console.Clear();
            WriteARES("Ok. These are the users I found on this computer:");
            List<string> User = GetComputerUsers();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (User != null && User.Count > 0)
            {
                foreach (string u in User) Console.WriteLine(u);
            }
            else Console.WriteLine("---Nothing---");
            SetARESColor();
            Console.ReadKey();
        }

        public static void HandleDeathrayCommand()
        {
            Console.Clear();
            WriteARES("Ok. Operating Deathray. Cover your ears...");
            Console.WriteLine("3");
            Thread.Sleep(1000);
            Console.WriteLine("2");
            Thread.Sleep(1000);
            Console.WriteLine("1");
            Thread.Sleep(1000);
            Console.Beep(12000, 5000);
            WriteSlow("Deathray executed");
            Console.ReadKey();
        }

        public static bool IsUserAdmin()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public static List<string> GetComputerUsers()
        {
            List<string> users = new List<string>();
            var path = string.Format("WinNT://{0},computer", Environment.MachineName);
            using (var computerEntry = new DirectoryEntry(path))
            {
                foreach (DirectoryEntry childEntry in computerEntry.Children)
                {
                    if (childEntry.SchemaClassName == "User")
                    {
                        users.Add(childEntry.Name);
                    }
                }
            }
            return users;
        }

        public static void PaintSymbol()
        {
            Console.Clear();
            SetUserColor();
            WriteFast("        00      00        ");
            WriteFast("        0000000000        ");
            WriteFast("00    00000000000000    00");
            WriteFast("0000  00000000000000  0000");
            WriteFast("    0000  000000  0000    ");
            WriteFast("      00    00    00      ");
            WriteFast("    000000  00  000000    ");
            WriteFast("000000  0000000000  000000");
            WriteFast("  00    00      00    00  ");
            WriteFast("          00  00          ");
            Console.ReadKey();
            SetARESColor();
        }

        public static void PaintBanner()
        {
            Console.Clear();
            WriteFast(":::'###::::'########::'########::'######::");
            WriteFast("::'## ##::: ##.... ##: ##.....::'##... ##:");
            WriteFast(":'##:. ##:: ##:::: ##: ##::::::: ##:::..::");
            WriteFast("'##:::. ##: ########:: ######:::. ######::");
            WriteFast(" #########: ##.. ##::: ##...:::::..... ##:");
            WriteFast(" ##.... ##: ##::. ##:: ##:::::::'##::: ##:");
            WriteFast(" ##:::: ##: ##:::. ##: ########:. ######::");
            WriteFast(" ..:::::..::..:::::..::........:::......:::");
        }

        public static bool GetNearBTDevices(out List<string> strL)
        {
            strL = null;
            BluetoothDeviceInfo[] bdi = null;
            try
            {
                using (BluetoothClient bc = new BluetoothClient())
                {
                    bdi = bc.DiscoverDevicesInRange();
                }
                
                strL = new List<string>();
                foreach (BluetoothDeviceInfo b in bdi) strL.Add(b.DeviceName);
                return true;
            }
            catch (PlatformNotSupportedException pex)
            {
                return false;
            }

        }

    }
}
