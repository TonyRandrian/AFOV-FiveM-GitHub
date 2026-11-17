using metabolism.Server.Application.Interfaces;
using metabolism.Server.Domain;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static CitizenFX.Core.Native.API;
using System;
using CitizenFX.Core;

namespace metabolism.Server.Infrastructure.Config
{
    public class JsonDamageSourceConfigProvider : IDamageSourceConfigProvider
    {
        private readonly string _resourceName;
        private readonly string _filePath;


        public JsonDamageSourceConfigProvider(string resourceName, string filePath = "damage-sources.json")
        {
            _resourceName = resourceName;
            _filePath = filePath;
        }

        public IReadOnlyList<DamageSourceProfile> LoadDamageSourceProfiles()
        {
            string json = LoadResourceFile(_resourceName, _filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new InvalidOperationException($"[Metabolism] unable to load {_filePath}");
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());

            List<DamageSourceProfile> profiles =
                JsonConvert.DeserializeObject<List<DamageSourceProfile>>(json, settings)
                ?? throw new InvalidOperationException($"[Metabolism] {_filePath} content not valid");

            Debug.WriteLine($"[Metabolism] {profiles.Count} damage source profiles loaded");
            return profiles;
        }
    }
}
