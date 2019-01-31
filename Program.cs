using System;
using System.IO;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Etisalat_Mobile_Http_Data_Gen
{
    class Program
    {
        static string generatorType;
        static ulong numbderOfRecords;
        static DateTime startTime;
        static DateTime endTime;
        static string userbaseFileName;
        static string urlsFileName;
        static string env_GeneratorType_Name = "GENTYPE";
        static string env_RecordsCount_Name = "COUNT";
        static string env_StartTime_Name = "TIME_START";
        static string env_EndTime_Name = "TIME_END";
        static string env_UrlFileName="URL_FILE_NAME";
        static string env_UserbaseFileName="UserBase_FILE_NAME";

        static void Main(string[] args)
        {
            //Build Configurations
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
            //Check variables 
            if (CheckEnvVariables())
            {
                generatorType = Environment.GetEnvironmentVariable(env_GeneratorType_Name);
                numbderOfRecords = ulong.Parse(Environment.GetEnvironmentVariable(env_RecordsCount_Name));
                if (Environment.GetEnvironmentVariable(env_StartTime_Name) != null && Environment.GetEnvironmentVariable(env_EndTime_Name) != null)
                {
                    startTime =  DateTime.ParseExact(Environment.GetEnvironmentVariable(env_StartTime_Name), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    endTime =  DateTime.ParseExact(Environment.GetEnvironmentVariable(env_EndTime_Name), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    urlsFileName = Environment.GetEnvironmentVariable(env_UrlFileName);
                    userbaseFileName = Environment.GetEnvironmentVariable(env_UserbaseFileName);
                }
            }
            else if (CheckPassedArgs(args))
            {
                generatorType = args[0];
                numbderOfRecords = ulong.Parse(args[1]);
            }
            else
            {
                Console.WriteLine("Please specify the generator type and the record count to be generated");
            }
            switch (generatorType)
            {
                //Generate random user base
                case "0":
                    GenerateRandomUserWithImeisData(numbderOfRecords);
                    break;
                //Generate mobile http daily transaction
                case "1":
                    GenerateDailyMobileHttpTransactions(numbderOfRecords,startTime,endTime,urlsFileName,userbaseFileName);
                    break;
                //Generate mobile protocol daily transaction
                case "2":
                    GenerateDailyMobileProtocolsTransactions(numbderOfRecords,startTime,endTime,urlsFileName);
                    break;
                default:
                    Console.WriteLine("No generator found with option {0}", generatorType);
                    break;
            }
        }

        private static bool CheckEnvVariables()
        {
            Console.WriteLine("Env variable value {0}", Environment.GetEnvironmentVariable(env_GeneratorType_Name));
             Console.WriteLine("Env variable value {0}", Environment.GetEnvironmentVariable(env_UrlFileName));
              Console.WriteLine("Env variable value {0}", Environment.GetEnvironmentVariable(env_UserbaseFileName));
            if (Environment.GetEnvironmentVariable(env_GeneratorType_Name) == null || Environment.GetEnvironmentVariable(env_RecordsCount_Name) == null)
                return false;
            else return true;
        }
        private static bool CheckPassedArgs(string[] args)
        {
            Console.WriteLine("Number of command line args passed is {0}", args.Length);
            if (args.Length > 0)
            {
                foreach (var item in args)
                {
                    Console.WriteLine("CommandLine agrs: {0}", item);
                }
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Generates the daily mobile http transactions.
        /// Sample formate: 
        /// 343092911-2|0|2016-06-11 23:56:28 |DXB-Offline-3.etisalat|d9a45e5615f4e85d|971501233331|424021623325386|10.21.172.22|Mobile|etisalat.ae|1|42402|217.164.94.115
        /// |0124F420A08C1131|3599312528232002|Galaxy S6 G920F,Samsung,GSM;35212707,Android|false|http|GET|pa.namshicdn.com|/product/46/9651/1-mobile-android-catalog.webp||Dalvik/2.1.0 (Linux; U; Android 6.0.1; SM-G920F Build/MMB29K)|1|0||||||||||
        /// </summary>
        private static void GenerateDailyMobileHttpTransactions(ulong limit,DateTime startTime, DateTime endTime, string urlsFile, string userbaseFile)
        {
            string outputFormate = "{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}|{26}|{27}|{28}|{29}|{30}|{31}|{32}|{33}|{34}"; Console.WriteLine("Generating daily mobile http transactions started at:{0} to generate {1} transactions", DateTime.Now.ToShortDateString(), limit.ToString());
            string fileName = string.Format("data_mobile_broadband_http_{0}_{1}.txt", DateTime.Now.ToString("yyyyMMddHHmm"), CommonMobileGenerators.RandomNumberString(4));
            ulong counter = 0;
            Directory.CreateDirectory("./app/data");
            using (StreamWriter file = new StreamWriter(Path.Combine("./app/data", fileName)))
            {
                while (counter < limit)
                {
                    string[] userbase = DataSetReader.GetRandomUser(userbaseFile).Split('|');
                    string lineItem = string.Format(outputFormate,
                    //Transaction
                    CommonMobileGenerators.RandomNumberString(9) + "-2",
                    //Transaction State
                    CommonMobileGenerators.RandomNumber(0, 9),
                    //Request Time
                    CommonMobileGenerators.CurrentDateTime(startTime,endTime),
                    //PTS Hostname
                    CommonMobileGenerators.PtsHostName,
                    //Acct-Session-Id
                   CommonMobileGenerators.AcctSessionId,
                    //Subscriber ID
                     userbase[0],
                    //IMSI
                     userbase[2],
                    //Framed IP
                   CommonMobileGenerators.FrameIp,
                    //Network Type
                   CommonMobileGenerators.NetworkType,
                    //APN
                   CommonMobileGenerators.ApnName,
                    //RAT Type
                    "1",
                    //SGSN MCC-MNC
                    "42402",
                    //SGSN IP
                    CommonMobileGenerators.SgsnIp,
                    //Cell-Id
                    CommonMobileGenerators.CellId,
                    //IMEI
                    userbase[1],
                    //Access Device
                    userbase[3],
                    //Tethered
                    CommonMobileGenerators.RandomBoolean(),
                    //Protocol
                    "http",
                    //Method
                    "GET",
                    //Host
                    DataSetReader.GetRandomUrl(urlsFile),
                    //Resource
                    "/product/46/9651/1-mobile-android-catalog.webp",
                    //Refere
                    "",
                    //User Agent
                    "Dalvik/2.1.0 (Linux; U; Android 6.0.1; SM-G920F Build/MMB29K)",
                    //Subscriber RTT
                    "1",
                    //Internet RTT
                    "0",
                    //Custom Attr1
                    "",
                    //Custom Attr2
                    "",
                    //Custom Attr3
                    "",
                    //Custom Attr4
                    "",
                    //Custom Attr5
                    "",
                    //Custom Attr6
                    "",
                    //Custom Attr7
                    "",
                    //Custom Attr8
                    "",
                    //Custom Attr9
                    "",
                    //Custom Attr10
                    ""
                    );
                    Console.WriteLine(string.Format("{0}: {1}", counter.ToString(), lineItem));
                    file.WriteLine(lineItem);
                    counter++;
                }
            }
            Console.WriteLine("Generating Random Data stopped at:{0}", DateTime.Now.ToShortTimeString());
        }
        /// <summary>
        /// Generates the daily mobile protocols transactions.
        /// Sample formate: 
        /// usage_stop|0|1|2016-06-11 23:52:35 +0400|2016-06-11 23:57:47 +0400|DXB-Offline-1.etisalat|d9a45e5615f4e823|0501111220|421113844453418|10.23.176.22|Mobile|etisalat.ae|1|42402|217.164.94.11|0124F4207D98CA09|3525512347968201|
        /// Galaxy S5 G900F,Samsung,GSM;35179806,Android|false|ntp|Miscellaneous|98|98|196||||||||||||
        /// </summary>
        /// <param name="limit"></param>
        private static void GenerateDailyMobileProtocolsTransactions(ulong limit,DateTime startTime, DateTime endTime, string urlsFile)
        {
            string outputFormate = "{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}|{26}|{27}|{28}|{29}|{30}|{31}|{32}|{33}|{34}|";
            Console.WriteLine("Generating daily mobile protocols transactions started at:{0} to generate {1} transactions", DateTime.Now.ToShortDateString(), limit.ToString());
            string fileName = string.Format("data_mobile_protocol_{0}_{1}.txt", DateTime.Now.ToString("yyyyMMddHHmm"), CommonMobileGenerators.RandomNumberString(4));
            ulong counter = 0;
            Directory.CreateDirectory("./app/data");
            using (StreamWriter file = new StreamWriter(Path.Combine("./app/data", fileName)))
            {
                while (counter < limit)
                {
                    string[] userbase = DataSetReader.GetRandomUser(urlsFile).Split('|');
                    string lineItem = string.Format(outputFormate,
                    //Record Type
                    "usage_stop",
                    //Record Status
                    "0",
                    //Record Numeber
                    counter.ToString(),
                    //Start Time
                      CommonMobileGenerators.CurrentDateTime(startTime,endTime),
                    //Stop Time
                      CommonMobileGenerators.CurrentDateTime(startTime,endTime),
                    //PTS Hostname
                    CommonMobileGenerators.PtsHostName,
                    //Acct-Session-Id
                    CommonMobileGenerators.AcctSessionId,
                    //Subscriber ID
                    userbase[0],
                    //CommonMobileGenerators.RandomMobileNumber(),
                    //IMSI
                    //CommonMobileGenerators.RandomImsiNumber(),
                    userbase[2],
                    //Framed IP
                    "10.21.172.22",
                    //Network Type
                    CommonMobileGenerators.NetworkType,
                    //APN
                    CommonMobileGenerators.ApnName,
                    //RAT Type
                    "1",
                    //SGSN MCC-MNC
                    "42402",
                    //SGSN IP
                    "217.164.94.115",
                    //Cell-Id
                    CommonMobileGenerators.CellId,
                    //IMEI
                    userbase[1],
                    //CommonMobileGenerators.RandomImeiNumber(),
                    //Access Device
                    userbase[3],
                    //DataSetReader.GetRandomDevice(),
                    //Tethered
                    CommonMobileGenerators.RandomBoolean(),
                    //Protocol/ Service
                    DataSetReader.GetRandomProtocol(),
                    //Application Type
                    "Miscellaneous",
                    //Tx Bytes
                    "98",
                    //Rx Bytes
                    "98",
                    //Total Bytes
                    "196",
                    //Subscriber RTT
                    "1",
                    //Internet RTT
                    "0",
                    //Custom Attr1
                    "",
                    //Custom Attr2
                    "",
                    //Custom Attr3
                    "",
                    //Custom Attr4
                    "",
                    //Custom Attr5
                    "",
                    //Custom Attr6
                    "",
                    //Custom Attr7
                    "",
                    //Custom Attr8
                    "",
                    //Custom Attr9
                    "",
                    //Custom Attr10
                    ""
                    );

                    Console.WriteLine(string.Format("{0}: {1}", counter.ToString(), lineItem));
                    file.WriteLine(lineItem);
                    counter++;
                }
            }
            Console.WriteLine("Generating Random Data stopped at:{0}", DateTime.Now.ToShortTimeString());
        }
        /// <summary>
        /// Generate Randmon user base: Mobile Number, IMEI and IMSI
        /// </summary>
        private static void GenerateRandomUserWithImeisData(ulong limit)
        {
            string outputFormate = "{0}|{1}|{2}|{3}";
            Console.WriteLine("Generating random user base started at:{0} to generate {1} transactions", DateTime.Now.ToShortDateString(), limit.ToString());
            string fileName = string.Format("data_users_base_{0}_{1}.txt", DateTime.Now.ToString("yyyyMMddHHmm"), CommonMobileGenerators.RandomNumberString(4));
            ulong counter = 0;
            Directory.CreateDirectory("./app/data");
            using (StreamWriter file = new StreamWriter(Path.Combine("./app/data", fileName)))
            {
                while (counter < limit)
                {
                    var lineItem = string.Format(outputFormate, CommonMobileGenerators.RandomMobileNumber(), CommonMobileGenerators.RandomImeiNumber(), CommonMobileGenerators.RandomImsiNumber(),DataSetReader.GetRandomDevice());
                    Console.WriteLine(string.Format("{0}: {1}", counter.ToString(), lineItem));
                    file.WriteLine(lineItem);
                    counter++;
                }
            }
            Console.WriteLine("Generating User Data stopped at:{0}", DateTime.Now.ToShortTimeString());
        }

    }
}
