using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Unitt.AvatarMod.Editor
{
    internal static class UnittModificationApplier
    {
        private const string LogPrefix = "[Unitt]";

        public static List<UnittValidationResult> Validate(
            GameObject avatarRoot,
            GameObject accessoryPrefab,
            Material replacementMaterial,
            string attachBoneName,
            bool applyMaterialToAllSkinnedMeshRenderers)
        {
            var results = new List<UnittValidationResult>();

            if (avatarRoot == null)
            {
                results.Add(UnittValidationResult.Error("Avatar Root is required."));
                return results;
            }

            if (EditorUtility.IsPersistent(avatarRoot) || !avatarRoot.scene.IsValid())
            {
                results.Add(UnittValidationResult.Error("Avatar Root must be a GameObject in the open scene."));
            }

            if (accessoryPrefab == null && replacementMaterial == null)
            {
                results.Add(UnittValidationResult.Error("Specify an Accessory Prefab or Replacement Material."));
            }

            if (string.IsNullOrWhiteSpace(attachBoneName))
            {
                results.Add(UnittValidationResult.Warning("Attach Bone Name is empty. Accessories will be attached to Avatar Root."));
            }
            else if (FindChildRecursive(avatarRoot.transform, attachBoneName) == null)
            {
                results.Add(UnittValidationResult.Warning(
                    "Attach Bone Name was not found. Accessories will be attached to Avatar Root."));
            }

            if (accessoryPrefab == null &&
                replacementMaterial != null &&
                !applyMaterialToAllSkinnedMeshRenderers)
            {
                results.Add(UnittValidationResult.Error(
                    "Replacement Material is set, but it will not be applied. Enable material application or add an Accessory Prefab."));
            }
            else if (replacementMaterial != null && !applyMaterialToAllSkinnedMeshRenderers)
            {
                results.Add(UnittValidationResult.Warning(
                    "Replacement Material is set, but material application is disabled."));
            }

            if (replacementMaterial != null && applyMaterialToAllSkinnedMeshRenderers)
            {
                var affectedRenderers = GetAffectedRenderers(avatarRoot);
                if (affectedRenderers.Count == 0)
                {
                    results.Add(UnittValidationResult.Error(
                        "Replacement Material requires at least one SkinnedMeshRenderer under Avatar Root."));
                }
            }

            if (!HasError(results))
            {
                results.Add(UnittValidationResult.Info("Validation completed. Apply is available."));
            }

            return results;
        }

        public static bool Apply(
            GameObject avatarRoot,
            GameObject accessoryPrefab,
            Material replacementMaterial,
            string attachBoneName,
            bool applyMaterialToAllSkinnedMeshRenderers,
            bool dryRun)
        {
            var validationResults = Validate(
                avatarRoot,
                accessoryPrefab,
                replacementMaterial,
                attachBoneName,
                applyMaterialToAllSkinnedMeshRenderers);

            if (HasError(validationResults))
            {
                foreach (var result in validationResults)
                {
                    LogValidationResult(result);
                }

                return false;
            }

            var attachTarget = ResolveAttachTarget(avatarRoot, attachBoneName);
            var affectedRenderers = replacementMaterial != null && applyMaterialToAllSkinnedMeshRenderers
                ? GetAffectedRenderers(avatarRoot)
                : new List<SkinnedMeshRenderer>();

            if (dryRun)
            {
                LogDryRun(avatarRoot, accessoryPrefab, attachTarget, replacementMaterial, affectedRenderers);
                return true;
            }

            Undo.RegisterFullObjectHierarchyUndo(avatarRoot, "Apply Unitt Modification");

            if (accessoryPrefab != null)
            {
                AddAccessory(accessoryPrefab, attachTarget);
            }

            if (replacementMaterial != null && applyMaterialToAllSkinnedMeshRenderers)
            {
                ReplaceMaterials(affectedRenderers, replacementMaterial);
            }

            EditorUtility.SetDirty(avatarRoot);
            if (avatarRoot.scene.IsValid())
            {
                EditorSceneManager.MarkSceneDirty(avatarRoot.scene);
            }

            Debug.Log($"{LogPrefix} Applied avatar modification to {avatarRoot.name}.", avatarRoot);
            return true;
        }

        public static Transform FindChildRecursive(Transform root, string childName)
        {
            if (root == null || string.IsNullOrWhiteSpace(childName))
            {
                return null;
            }

            if (root.name == childName)
            {
                return root;
            }

            for (var index = 0; index < root.childCount; index++)
            {
                var result = FindChildRecursive(root.GetChild(index), childName);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private static Transform ResolveAttachTarget(GameObject avatarRoot, string attachBoneName)
        {
            var target = FindChildRecursive(avatarRoot.transform, attachBoneName);
            return target != null ? target : avatarRoot.transform;
        }

        private static void AddAccessory(GameObject accessoryPrefab, Transform attachTarget)
        {
            var createdObject = PrefabUtility.InstantiatePrefab(accessoryPrefab) as GameObject;
            if (createdObject == null)
            {
                createdObject = Object.Instantiate(accessoryPrefab);
                createdObject.name = accessoryPrefab.name;
            }

            Undo.RegisterCreatedObjectUndo(createdObject, "Add Unitt Accessory");
            Undo.SetTransformParent(createdObject.transform, attachTarget, "Attach Unitt Accessory");
            createdObject.transform.localPosition = Vector3.zero;
            createdObject.transform.localRotation = Quaternion.identity;
            createdObject.transform.localScale = Vector3.one;

            EditorUtility.SetDirty(createdObject);
            EditorUtility.SetDirty(attachTarget);
        }

        private static void ReplaceMaterials(IReadOnlyList<SkinnedMeshRenderer> renderers, Material replacementMaterial)
        {
            foreach (var renderer in renderers)
            {
                Undo.RecordObject(renderer, "Replace Unitt Materials");

                var materials = renderer.sharedMaterials;
                for (var index = 0; index < materials.Length; index++)
                {
                    materials[index] = replacementMaterial;
                }

                renderer.sharedMaterials = materials;
                EditorUtility.SetDirty(renderer);
            }
        }

        private static List<SkinnedMeshRenderer> GetAffectedRenderers(GameObject avatarRoot)
        {
            return new List<SkinnedMeshRenderer>(
                avatarRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true));
        }

        private static bool HasError(IEnumerable<UnittValidationResult> results)
        {
            foreach (var result in results)
            {
                if (result.Severity == UnittValidationSeverity.Error)
                {
                    return true;
                }
            }

            return false;
        }

        private static void LogDryRun(
            GameObject avatarRoot,
            GameObject accessoryPrefab,
            Transform attachTarget,
            Material replacementMaterial,
            IReadOnlyList<SkinnedMeshRenderer> affectedRenderers)
        {
            Debug.Log($"{LogPrefix} Avatar Root: {DescribeObject(avatarRoot)}", avatarRoot);
            Debug.Log($"{LogPrefix} Accessory Prefab: {DescribeObject(accessoryPrefab)}", accessoryPrefab);
            Debug.Log($"{LogPrefix} Attach Target: {DescribeObject(attachTarget.gameObject)}", attachTarget);
            Debug.Log($"{LogPrefix} Replacement Material: {DescribeObject(replacementMaterial)}", replacementMaterial);
            Debug.Log($"{LogPrefix} Affected Renderers: {FormatRendererList(affectedRenderers)}", avatarRoot);
        }

        private static void LogValidationResult(UnittValidationResult result)
        {
            var message = $"{LogPrefix} {result.Severity}: {result.Message}";
            if (result.Severity == UnittValidationSeverity.Error)
            {
                Debug.LogError(message);
                return;
            }

            if (result.Severity == UnittValidationSeverity.Warning)
            {
                Debug.LogWarning(message);
                return;
            }

            Debug.Log(message);
        }

        private static string DescribeObject(Object value)
        {
            return value != null ? value.name : "(none)";
        }

        private static string FormatRendererList(IReadOnlyList<SkinnedMeshRenderer> renderers)
        {
            if (renderers.Count == 0)
            {
                return "0";
            }

            var names = new List<string>();
            foreach (var renderer in renderers)
            {
                names.Add(renderer.name);
            }

            return $"{renderers.Count} ({string.Join(", ", names)})";
        }
    }
}
