using UnityEngine;

namespace Unitt.AvatarMod
{
    [CreateAssetMenu(
        fileName = "UnittModificationProfile",
        menuName = "Unitt!/Modification Profile"
    )]
    public sealed class UnittModificationProfile : ScriptableObject
    {
        public GameObject accessoryPrefab;
        public Material replacementMaterial;
        public string attachBoneName = "Head";
        public bool applyMaterialToAllSkinnedMeshRenderers = false;
    }
}
