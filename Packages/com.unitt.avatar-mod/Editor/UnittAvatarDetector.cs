using UnityEditor;
using UnityEngine;

namespace Unitt.AvatarMod.Editor
{
    internal static class UnittAvatarDetector
    {
        private const string AvatarDescriptorTypeName = "VRCAvatarDescriptor";
        private const string AvatarDescriptorFullTypeName = "VRC.SDK3.Avatars.Components.VRCAvatarDescriptor";

        public static GameObject DetectFromSelection()
        {
            return DetectAvatarRoot(Selection.activeGameObject);
        }

        public static GameObject DetectAvatarRoot(GameObject selectedObject)
        {
            if (selectedObject == null)
            {
                return null;
            }

            var descriptorInParents = FindDescriptorInParents(selectedObject.transform);
            if (descriptorInParents != null)
            {
                return descriptorInParents.gameObject;
            }

            var descriptorInChildren = FindDescriptorInChildren(selectedObject.transform);
            if (descriptorInChildren != null)
            {
                return descriptorInChildren.gameObject;
            }

            return selectedObject;
        }

        private static Transform FindDescriptorInParents(Transform start)
        {
            var current = start;
            while (current != null)
            {
                if (HasAvatarDescriptor(current.gameObject))
                {
                    return current;
                }

                current = current.parent;
            }

            return null;
        }

        private static Transform FindDescriptorInChildren(Transform root)
        {
            var transforms = root.GetComponentsInChildren<Transform>(true);
            foreach (var transform in transforms)
            {
                if (HasAvatarDescriptor(transform.gameObject))
                {
                    return transform;
                }
            }

            return null;
        }

        private static bool HasAvatarDescriptor(GameObject gameObject)
        {
            var components = gameObject.GetComponents<Component>();
            foreach (var component in components)
            {
                if (component == null)
                {
                    continue;
                }

                var componentType = component.GetType();
                if (componentType.Name == AvatarDescriptorTypeName ||
                    componentType.FullName == AvatarDescriptorFullTypeName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
