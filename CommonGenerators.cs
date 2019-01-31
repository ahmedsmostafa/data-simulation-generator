using System;
using System.Linq;
using System.Collections;
using System.Text;

namespace Etisalat_Mobile_Http_Data_Gen
{
    public static class CommonMobileGenerators
    {
        private static readonly Random random = new Random();
        private static readonly Random randomMobileNumber = new Random();

        //Generate Random String with certain lenght
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            lock (random)
            {
                return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
        //Generate a random number string with certain length
        public static string RandomNumberString(int length)
        {
            const string chars = "0123456789";
            lock (random)
            {
                return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
        /// <summary>
        /// Get current date and time in yyyyMMddHHmmss
        /// </summary>
        /// <returns>string datetime in formate yyyyMMddHHmmss</returns>
        public static string CurrentDateTime(DateTime startTime, DateTime endTime)
        {
            if (startTime != null && endTime != null)
            {
                var range = endTime - startTime;
                var rnd = new Random();
                var randTimeSpan = new TimeSpan((long)(rnd.NextDouble() * range.Ticks));
                return (startTime + randTimeSpan).ToString("yyyyMMddHHmmss");
            }
            else
                return DateTime.Now.ToString("yyyyMMddHHmmss");
        }
        /// <summary>
        /// Generate random number within a ranged
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static string RandomNumber(int from, int to)
        {
            lock (random)
            {
                return new Random().Next(from, to).ToString(); //returns integer of 0-100
            }
        }
        /// <summary>
        /// Generate random boolean
        /// </summary>
        /// <returns></returns>
        public static string RandomBoolean()
        {
            lock (random)
            {
                return (new Random().Next(100) <= 20 ? true : false).ToString().ToLower();
            }
        }

        public static string GenerateRandomMobileNumber()
        {
            StringBuilder mobileNumber = new StringBuilder(7);
            for (int i = 0; i < 7; i++)
            {
                lock (randomMobileNumber)
                {
                    mobileNumber.Append(randomMobileNumber.Next(10).ToString());
                }
            }
            return mobileNumber.ToString();
        }

        ///Generate IMEI
        public static string RandomImeiNumber()
        {
            //Formate 1236587852545470
            return RandomNumberString(16);
        }
        //Generate IMSI
        public static string RandomImsiNumber()
        {
            //Formate 424054869854257
            return RandomNumberString(15);
        }
        /// <summary>
        /// Generate Random Mobile number 
        /// Formate 050XXXXXXX 054XXXXXXX 056XXXXXXX
        /// UAE: Telephone numbers are fixed at seven digits, with area codes fixed at two or three digits.
        /// </summary>
        /// <returns></returns>
        public static string RandomMobileNumber()
        {
            string[] mobilePrefix = new string[] { "050", "054", "056" };
            int randomIndex = int.Parse(RandomNumber(1, 3));
            return string.Format("{0}{1}", mobilePrefix[randomIndex], GenerateRandomMobileNumber());
        }

        public static String PtsHostName { get { return "DXB-Offline-3.etisalat"; } }
        public static String AcctSessionId { get { return "d9a45e5615f4e85d"; } }
        public static String CellId { get { return "0124F420A08C1131"; } }
        public static String ApnName { get { return "etisalat.ae"; } }
        public static String FrameIp { get { return "10.21.172.22"; } }
        public static String NetworkType { get { return "Mobile"; } }

        public static string SgsnIp { get { return "217.164.94.115"; } }
    }
    public static class CommonFixedLineGenerators
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// Generate Random Mobile number 
        /// Formate 050XXXXXXX 054XXXXXXX 056XXXXXXX
        /// UAE: Telephone numbers are fixed at seven digits, with area codes fixed at two or three digits.
        /// </summary>
        /// <returns></returns>
        public static string RandomFixedNumber()
        {
            string[] mobilePrefix = new string[] { "02", "03", "04", "06", "07", "08", "09" };
            int randomIndex = int.Parse(CommonMobileGenerators.RandomNumber(1, 7));
            return string.Format("{0}{1}", mobilePrefix[randomIndex], CommonMobileGenerators.GenerateRandomMobileNumber());
        }
    }
}