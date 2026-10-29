using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace metabolism.Server
{
    public class MetabolismService : BaseScript
    {
        private const float maxHunger = 100.0f;
        private const float hungerDrain = 1f;
        private readonly Dictionary<string, float> _hunger = new Dictionary<string, float>();

        public MetabolismService()
        {
            _ = Run();
        }

        private async Task Run()
        {
            while (true)
            {
                await Delay(1000);

                foreach (Player player in Players)
                {
                    string playerId = player.Handle;

                    if (!_hunger.ContainsKey(playerId))
                    {
                        _hunger[playerId] = maxHunger;
                    }

                    _hunger[playerId] = System.Math.Max(0.0f, _hunger[playerId] - hungerDrain);

                    TriggerClientEvent(
                        player,
                        "metabolism:updateHunger",
                        _hunger[playerId]
                        );
                }
            }
        }
    }
}
