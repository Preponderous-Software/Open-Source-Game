using UnityEngine;

namespace osg {

    public class PlayerDeathEvent : Event {

        private Vector3 position;
        private Player player;

        public PlayerDeathEvent(Player player) : base(EventType.PlayerDeath, "Player " + player.getId() + " has died.") {
            this.player = player;
        }

        public Player getPlayer() {
            return player;
        }
    }
}