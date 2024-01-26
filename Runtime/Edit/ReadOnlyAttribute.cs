using UnityEngine;

namespace Atelier.Edit {

    /// <summary>
    /// When attached to a field marked [SerializeField], the field will show up in a component, but
    /// can't be modified. Useful for showing debug information. Pairs with the ReadOnlyDrawer.
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute {
    }

}

