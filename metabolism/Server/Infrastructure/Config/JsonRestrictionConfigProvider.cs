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
    public class JsonRestrictionConfigProvider : IRestrictionConfigProvider
    {
        private readonly string _resourceName;
        private readonly string _filePath;


        public JsonRestrictionConfigProvider(string resourceName, string filePath = "restrictions.json")
        {
            _resourceName = resourceName;
            _filePath = filePath;
        }

        public IReadOnlyList<RestrictionRule> LoadRestrictionRules()
        {
            string json = LoadResourceFile(_resourceName, _filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new InvalidOperationException($"[Metabolism] unable to load {_filePath}");
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());

            List<RestrictionRule> restrictions =
                JsonConvert.DeserializeObject<List<RestrictionRule>>(json, settings)
                ?? throw new InvalidOperationException($"[Metabolism] {_filePath} content not valid");

            Debug.WriteLine($"[Metabolism] {restrictions.Count} action restrictions loaded");
            return restrictions;
        }
    }
}
