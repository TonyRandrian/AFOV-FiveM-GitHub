using CitizenFX.Core;
using metabolism.Server.Application.Interfaces;
using metabolism.Server.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static CitizenFX.Core.Native.API;

namespace metabolism.Server.Infrastructure.Config
{
    public class JsonAnimationConfigProvider : IAnimationConfigProvider
    {
        private readonly string _resourceName;
        private readonly string _filePath;


        public JsonAnimationConfigProvider(string resourceName, string filePath = "animations.json")
        {
            _resourceName = resourceName;
            _filePath = filePath;
        }

        public IReadOnlyList<AnimationRule> LoadAnimationRules()
        {
            string json = LoadResourceFile(_resourceName, _filePath);

            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidOperationException($"[Metabolism] Unable to load {_filePath}");

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());

            List<AnimationRule> rules =
                JsonConvert.DeserializeObject<List<AnimationRule>>(json, settings)
                ?? throw new InvalidOperationException($"[Metabolism] {_filePath} content not valid");

            CitizenFX.Core.Debug.WriteLine($"[Metabolism] {rules.Count} animation rules loaded");
            return rules;
        }
    }
}
