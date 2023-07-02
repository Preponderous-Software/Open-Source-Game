using UnityEngine;

using osg;

namespace osgtests {

    public static class TestChunkId {
        
        public static void runTests() {
            testInitialization();
            testEquality();
        }

        public static void testInitialization() {
            // run
            ChunkId chunkId = new ChunkId();

            // verify
            UnityEngine.Debug.Assert(chunkId != null);
        }

        public static void testEquality() {
            // prepare
            ChunkId chunkId1 = new ChunkId();
            ChunkId chunkId2 = new ChunkId();

            // verify
            UnityEngine.Debug.Assert(chunkId1 != chunkId2);
            UnityEngine.Debug.Assert(chunkId1 == chunkId1);
            UnityEngine.Debug.Assert(chunkId2 == chunkId2);
        }
    }
}