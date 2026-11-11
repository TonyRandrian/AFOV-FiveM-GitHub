using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace metabolism.Server.Application
{
    public class PlayerRegistry
    {
        private readonly Dictionary<string, PlayerState> _players =
            new Dictionary<string, PlayerState>();

        public void Add(PlayerState state)
        {
            _players[state.Handle] = state;
        }

        public bool TryGet(string handle, out PlayerState state)
        {
            return _players.TryGetValue(handle, out state);
        }

        public bool Remove(string handle)
        {
            return _players.Remove(handle);
        }

        // copy : so the list can still be iterated even if a player join or leave while looping
        public IReadOnlyList<PlayerState> Snapshot()
        {
            return _players.Values.ToList();
        }
    }
}
