using UnityEngine;

namespace Atelier.Core {

    /// <summary>
    /// Just helpers for dealing with layer masks. Nothing special.
    /// 
    /// Use the following import to gain access to the extension methods:
    /// <code>
    /// using static Atelier.Core.LayerMasks;
    /// </code>
    /// </summary>
    public static class LayerMasks {

        public const int Nothing = 0;
        public const int Everything = ~0;

        public static bool ContainsLayer(this LayerMask mask, int layer) {
            return (LayerToMask(layer) & mask.value) != 0;
        }

        public static int LayerToMask(int layer) {
            return 1 << layer;
        }

    }

}

