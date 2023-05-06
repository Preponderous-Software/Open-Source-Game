namespace osg {

    class GameConfig {
        private int chunkSize;
        private int locationScale;
        private int statusExpirationTicks;
        private int playerWalkSpeed;
        private int playerRunSpeed;

        public GameConfig() {
            chunkSize = 9;
            locationScale = 9;
            statusExpirationTicks = 500;
            playerWalkSpeed = 20;
            playerRunSpeed = 50;
        }

        public int getChunkSize() {
            return chunkSize;
        }

        public int getLocationScale() {
            return locationScale;
        }

        public int getStatusExpirationTicks() {
            return statusExpirationTicks;
        }

        public int getPlayerWalkSpeed() {
            return playerWalkSpeed;
        }

        public int getPlayerRunSpeed() {
            return playerRunSpeed;
        }
    }
}