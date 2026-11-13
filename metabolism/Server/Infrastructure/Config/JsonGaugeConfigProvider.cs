using metabolism.Server.Application.Abstractions;
using metabolism.Server.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static CitizenFX.Core.Native.API;

namespace metabolism.Server.Infrastructure.Config
{
    public class JsonGaugeConfigProvider : IGaugeConfigProvider
    {
        private readonly string _resourceName;
        private readonly string _filePath;


        public JsonGaugeConfigProvider(string resourceName, string filePath = "config.json")
        {
            _resourceName = resourceName;
            _filePath = filePath;
        }

        public IReadOnlyList<GaugeDefinition> LoadGauges()
        {
            string json = LoadResourceFile(_resourceName, _filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                throw new InvalidOperationException($"[Metabolism] Unable to load {_filePath}");
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());

            List<GaugeDefinition> gauges =
                JsonConvert.DeserializeObject<List<GaugeDefinition>>(json, settings)
                ?? throw new InvalidOperationException($"[Metabolism] {_filePath} content not valid");

            string duplicate = gauges
                .GroupBy(g => g.Name)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .FirstOrDefault();

            if (duplicate != null)
            {
                throw new InvalidOperationException($"[Metabolism] Duplicate gauge name : {duplicate}");
            }

            CitizenFX.Core.Debug.WriteLine($"[Metabolism] {gauges.Count} gauges loaded.");
            return gauges;
        }
    }
}
