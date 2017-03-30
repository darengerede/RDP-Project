using System;
using Newtonsoft.Json.Converters;
using System.Management;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace RDP_Info
{
    class Program
    {
        public class IPAPI
        {
            public string AS;
            public string CITY;
            public string COUNTRY;
            public string ISP;
            public string LAT;
            public string LON;
            public string ORG;
            public string QUERY;
            public string REGION;
            public string REGIONNAME;
            public string STATUS;
            public string TIMEZONE;
            public string ZIP;
        }

        static void Main(string[] args)
        {
            GetUri();
            getSystemDetails();
            Console.ReadKey();
        }

        public static void GetUri()
        {
            var webClient = new System.Net.WebClient();
            string HTML = webClient.DownloadString("http://ip-api.com/json/");

            IPAPI Rec = JsonConvert.DeserializeObject<IPAPI>(HTML);
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("----------------- [ GEO INFO ] ------------------");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Provider: " + Rec.AS);
            Console.WriteLine("Country: " + Rec.COUNTRY);
            Console.WriteLine("City: " + Rec.CITY);
            Console.WriteLine("ORG: " + Rec.ORG);
            Console.WriteLine("IP: " + Rec.QUERY);
            Console.WriteLine("Connection: " + Rec.STATUS);
            Console.WriteLine("TimeZone: " + Rec.TIMEZONE);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("-------------------------------------------------");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void getSystemDetails()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("-------------- [ Machine INFO ] -----------------");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Login: " + Environment.UserName); // User name of PC
            Console.WriteLine("MachineName: " + Environment.MachineName);// Machine name
            string OStype = "";
            if (Environment.Is64BitOperatingSystem) { OStype = "64-Bit, "; } else { OStype = "32-Bit, "; }
            Console.WriteLine("OS: " + getOSInfo() + "  (" + Environment.OSVersion + ")");
            Console.WriteLine("OSType: " + OStype);
            ProcInfo();


            string[] drivers = Environment.GetLogicalDrives();
            foreach (string value in drivers)
            {
                Console.WriteLine("Local drivers: " + value);
            }
            
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("-------------------------------------------------");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("--------------- [ Video INFO ] ------------------");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            VideoInfo();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("-------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine();
            Console.WriteLine("Press any key to view Program INFO -> ");
            Console.ReadKey();

            Console.WriteLine("--------------- [ Program INFO ] ----------------");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            ProgramInfo();
            Console.WriteLine();
            Console.WriteLine("-------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static string getOSInfo()
        {
            //Get Operating system information.
            OperatingSystem os = Environment.OSVersion;
            //Get version information about the os.
            Version vs = os.Version;

            //Variable to hold our return value
            string operatingSystem = "";

            if (os.Platform == PlatformID.Win32Windows)
            {
                //This is a pre-NT version of Windows
                switch (vs.Minor)
                {
                    case 0:
                        operatingSystem = "95";
                        break;
                    case 10:
                        if (vs.Revision.ToString() == "2222A")
                            operatingSystem = "98SE";
                        else
                            operatingSystem = "98";
                        break;
                    case 90:
                        operatingSystem = "Me";
                        break;
                    default:
                        break;
                }
            }
            else if (os.Platform == PlatformID.Win32NT)
            {
                switch (vs.Major)
                {
                    case 3:
                        operatingSystem = "NT 3.51";
                        break;
                    case 4:
                        operatingSystem = "NT 4.0";
                        break;
                    case 5:
                        if (vs.Minor == 0)
                            operatingSystem = "Windows 2000";
                        else
                            operatingSystem = "Windows XP";
                        break;
                    case 6:
                        if (vs.Minor == 0)
                            operatingSystem = "Windows Vista";
                        else
                            operatingSystem = "Windows 7 or Above";
                        break;
                    default:
                        break;
                }

            }
            return operatingSystem;
        }

        public static void  ProcInfo()
        {
            ManagementObjectSearcher searcher8 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");

            foreach (ManagementObject queryObj in searcher8.Get())
            {
                Console.WriteLine("Name: {0}", queryObj["Name"]);
                Console.WriteLine("NumberOfCores: {0}", queryObj["NumberOfCores"]);
                Console.WriteLine("ProcessorId: {0}", queryObj["ProcessorId"]);
            }
        }

        public static void VideoInfo()
        {
            ManagementObjectSearcher searcher11 = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");

            foreach (ManagementObject queryObj in searcher11.Get())
            {
                Console.WriteLine("AdapterRAM: {0}", queryObj["AdapterRAM"]);
                Console.WriteLine("Caption: {0}", queryObj["Caption"]);
                Console.WriteLine("Description: {0}", queryObj["Description"]);
                Console.WriteLine("VideoProcessor: {0}", queryObj["VideoProcessor"]);
            }
        }

        public static void ProgramInfo()
        {
            ManagementObjectSearcher searcher_soft = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Product");

            foreach (ManagementObject queryObj in searcher_soft.Get())
            {
                Console.WriteLine("Program: {0}", queryObj["Caption"]);
            }


        }
    }
           
}
