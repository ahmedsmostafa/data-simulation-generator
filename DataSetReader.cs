using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Etisalat_Mobile_Http_Data_Gen
{
    public static class DataSetReader
    {
        private static string userBaseFilePath;
        private static string urlsFilePath;
        private static List<string> mobileAccessDevices = new List<string>();
        private static List<string> protocolsList = new List<string>();
        private static List<string> urlsList = new List<string>();
        private static List<string> usebase = new List<string>();
        public static List<string> DeviceAccess
        {
            get
            {
                if (mobileAccessDevices.Capacity == 0)
                {
                    using (var reader = new StreamReader(Path.Combine("./datasets", "deviceaccess.csv")))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            mobileAccessDevices.Add(line);
                        }
                    }
                    return mobileAccessDevices;
                }
                else
                {
                    return mobileAccessDevices;
                }
            }
        }
        public static List<string> Urls
        {
            get
            {
                if (urlsList.Count == 0)
                {
                    using (var reader = new StreamReader(urlsFilePath))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            urlsList.Add(line);
                        }
                    }
                    return urlsList;
                }
                else
                {
                    return urlsList;
                }
            }
        }
        public static List<string> Protocols
        {
            get
            {
                if (protocolsList.Count == 0)
                {
                    using (var reader = new StreamReader(Path.Combine("./datasets", "protocols.csv")))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            protocolsList.Add(line);
                        }
                    }
                    return protocolsList;
                }
                else
                {
                    return protocolsList;
                }
            }
        }
        private static readonly Random random = new Random();
        //Get Random Device from the list of 55 sample devices
        public static string GetRandomDevice()
        {
            lock (random)
            {
                return DeviceAccess[random.Next(DeviceAccess.Count)];
            }
        }
        public static List<string> UserBase
        {
            get
            {
                if (usebase.Count == 0)
                {
                    using (var reader = new StreamReader(userBaseFilePath))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            usebase.Add(line);
                        }
                    }
                    return usebase;
                }
                else
                {
                    return usebase;
                }
            }
        }
        //Get Random protocol from the list 
        public static string GetRandomProtocol()
        {
            lock (random)
            {
                return Protocols[random.Next(Protocols.Count)];
            }
        }
         //Get Random Url from the list 
        public static string GetRandomUrl(string urlsFileName)
        {
            urlsFilePath = Path.Combine("./app/urlbase/",urlsFileName);
            lock (random)
            {
                return Urls[random.Next(Urls.Count)];
            }
        }

        //
        public static string GetRandomUser(string userBaseFileName)
        {
            userBaseFilePath = Path.Combine("./app/userbase/",userBaseFileName);
            Console.WriteLine("usebase file name {0} and full file path is {1}",userBaseFileName, userBaseFilePath);
            int skip = random.Next(0, UserBase.Count);
            return UserBase.Skip(skip).First();
        }
    }
}
