using System.IO;
using Newtonsoft.Json;

namespace Sat.Recruitment.Shared
{
    public interface IJsonFileHelper<T>
    {
        T ReadFile(string filePath);
        void SaveFile(T data, string filePath);
    }

    public class JsonFileHelper<T> : IJsonFileHelper<T>
    {
        public T ReadFile(string filePath)
        {
            using StreamReader reader = new StreamReader(filePath);
            var json = reader.ReadToEnd();
            T data = JsonConvert.DeserializeObject<T>(json);
            return data;
        }

        public void SaveFile(T data, string filePath)
        {
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, json);
        }
    }
}
