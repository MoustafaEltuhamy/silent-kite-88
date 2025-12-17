using System;
using Newtonsoft.Json;
using UnityEngine;

namespace AGAPI.Foundation
{

    // A JSON-based implementation of <see cref="ISerializer{TResult}"/> that uses Newtonsoft.Json.
    public class NewtonsoftJsonStringSerializer : ISerializer<string>
    {

        private readonly JsonSerializerSettings _settings;

        public NewtonsoftJsonStringSerializer(JsonSerializerSettings settings = null)
            => _settings = settings ?? new JsonSerializerSettings { Formatting = Formatting.Indented };


        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }


        public T Deserialize<T>(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(data, _settings);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[JsonDeserialize Error] Type: {typeof(T).FullName} | Exception: {ex}");
                throw;
            }
        }

    }
}