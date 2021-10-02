using Futebox.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Futebox.Services
{
    public class CacheHandler : ICacheHandler
    {
        private string cacheFolderPath = "/CacheFiles";

        public CacheHandler()
        {
            if (!Directory.Exists(cacheFolderPath)) Directory.CreateDirectory(cacheFolderPath);
        }

        public string ObterPastaCache()
        {
            return cacheFolderPath;
        }

        public T ObterConteudo<T>(string cacheName) where T : class
        {
            var cacheFilePath = $"{cacheFolderPath}/{cacheName}.txt";

            if (File.Exists(cacheFilePath))
            {
                var fileContent = File.ReadAllText(cacheFilePath);
                var fi = new FileInfo(cacheFilePath);
                if (string.IsNullOrEmpty(fileContent)) return null;

                var storage = JsonConvert.DeserializeObject<CacheStorageType<T>>(fileContent);
                if (storage.Expirado()) return null;

                return storage.Dados();
            }

            return null;
        }

        public bool DefinirConteudo<T>(string cacheName, T dados, int validadeHoras = 8) where T : class
        {
            var cacheFilePath = $"{cacheFolderPath}/{cacheName}.txt";
            var storage = new CacheStorageType<T>(dados, validadeHoras);
            File.WriteAllText(cacheFilePath, JsonConvert.SerializeObject(storage));
            var fi = new FileInfo(cacheFilePath);
            return true;
        }

        private class CacheStorageType<T> where T : class
        {
            public DateTime expires { get; set; }
            public T data { get; set; }

            public CacheStorageType(T data, int hoursToExpires)
            {
                this.expires = DateTime.Now.AddHours(hoursToExpires);
                this.data = data;
            }

            public bool Expirado()
            {
                return this.expires < DateTime.Now;
            }

            public T Dados()
            {
                return this.data;
            }
        }
    }
}
