using CitizenFX.Core;
using metabolism.Server.Application.Interfaces;
using metabolism.Server.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using static CitizenFX.Core.Native.API;

namespace metabolism.Server.Infrastructure.Config
{
    public class JsonBodyPartConfigProvider : IBodyPartConfigProvider
    {
        private readonly string _resourceName;
        private readonly string _filePath;


        public JsonBodyPartConfigProvider(string resourceName, string filePath = "body-part.json")
        {
            _resourceName = resourceName;
            _filePath = filePath;
        }

        public IReadOnlyList<BodyPartDefinition> LoadBodyParts()
        {
            string json = LoadResourceFile(_resourceName, _filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new InvalidOperationException($"[Metabolism] Unable to load {_filePath}");
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());

            List<BodyPartDefinition> bodyParts =
                JsonConvert.DeserializeObject<List<BodyPartDefinition>>(json, settings)
                ?? throw new InvalidOperationException($"[Metabolism] {_filePath} content not valid");

            CitizenFX.Core.Debug.WriteLine($"[Metabolism] {bodyParts.Count} body parts loaded.");
            return bodyParts;
        }
    }
}
