using Eco.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonHelper.Domain
{
    internal class SteamKey : IKey
    {
        public static string KeysFilePath 
        { 
            get
            {
                if (!File.Exists(keysFilePath))
                    throw new FileNotFoundException($"There is no file wuth steam keys in {keysFilePath}");
                return keysFilePath;
            }
        }
        public string Key { get; set; }
        private const string keysFilePath = @".\keys\SteamKeys.json";
    }
}
