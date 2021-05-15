using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Xamarin.Essentials.Interfaces;

namespace P0st.WPF
{
    public class PreferencesImplementation : IPreferences
    {
        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public string Get(string key, string defaultValue)
        {
            var c = GetConfig();
            return c.ContainsKey(key) ? c[key] : string.Empty;
        }

        public bool Get(string key, bool defaultValue)
        {
            throw new NotImplementedException();
        }

        public int Get(string key, int defaultValue)
        {
            throw new NotImplementedException();
        }

        public double Get(string key, double defaultValue)
        {
            throw new NotImplementedException();
        }

        public float Get(string key, float defaultValue)
        {
            throw new NotImplementedException();
        }

        public long Get(string key, long defaultValue)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, string value)
        {
            var c = GetConfig();
            c[key] = value;
            SetConfig(c);
        }

        public void Set(string key, bool value)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, int value)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, double value)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, float value)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, long value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key, string sharedName)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key, string sharedName)
        {
            throw new NotImplementedException();
        }

        public void Clear(string sharedName)
        {
            throw new NotImplementedException();
        }

        public string Get(string key, string defaultValue, string sharedName)
        {
            var c = GetConfig();
            return c.ContainsKey(key) ? c[key] : string.Empty;
        }

        public bool Get(string key, bool defaultValue, string sharedName)
        {
            throw new NotImplementedException();
        }

        public int Get(string key, int defaultValue, string sharedName)
        {
            throw new NotImplementedException();
        }

        public double Get(string key, double defaultValue, string sharedName)
        {
            throw new NotImplementedException();
        }

        public float Get(string key, float defaultValue, string sharedName)
        {
            throw new NotImplementedException();
        }

        public long Get(string key, long defaultValue, string sharedName)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, string value, string sharedName)
        {
            var c = GetConfig();
            c[key] = value;
            SetConfig(c);
        }

        public void Set(string key, bool value, string sharedName)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, int value, string sharedName)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, double value, string sharedName)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, float value, string sharedName)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, long value, string sharedName)
        {
            throw new NotImplementedException();
        }

        public DateTime Get(string key, DateTime defaultValue)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, DateTime value)
        {
            throw new NotImplementedException();
        }

        public DateTime Get(string key, DateTime defaultValue, string sharedName)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, DateTime value, string sharedName)
        {
            throw new NotImplementedException();
        }
        
        private static readonly string ConfigFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "P0st.json");

        private static IDictionary<string, string> GetConfig()
        {
            if (!File.Exists(ConfigFile))
                return new Dictionary<string, string>();
            return JsonConvert.DeserializeObject<IDictionary<string, string>>(File.ReadAllText(ConfigFile));
        }

        private static void SetConfig(IDictionary<string, string> dict)
        {
            File.WriteAllText(ConfigFile, JsonConvert.SerializeObject(dict));
        }
    }
}