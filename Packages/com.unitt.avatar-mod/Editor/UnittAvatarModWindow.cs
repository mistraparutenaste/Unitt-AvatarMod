using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unitt.AvatarMod;

namespace Unitt.AvatarMod.Editor
{
    public sealed class UnittAvatarModWindow : EditorWindow
    {
        private GameObject avatarRoot;
        private UnittModificationProfile modificationProfile;
        private GameObject accessoryPrefab;
        private Material replacementMaterial;
        private string attachBoneName = "Head";
        private bool applyMaterialToAllSkinnedMeshRenderers;
        private Vector2 scrollPosition;
        private List<UnittValidationResult> validationResults = new List<UnittValidationResult>();

        [MenuItem("Tools/Unitt!/Open")]
        public static void Open()
        {
            var window = GetWindow<UnittAvatarModWindow>();
            window.titleContent = new GUIContent(UnittLocalization.AppName);
            window.minSize = new Vector2(420f, 360f);
            window.Show();
        }

        private void OnEnable()
        {
            titleContent = new GUIContent(UnittLocalization.AppName);
            minSize = new Vector2(420f, 360f);

            if (avatarRoot == null)
            {
                avatarRoot = UnittAvatarDetector.DetectFromSelection();
            }
        }

        private void OnSelectionChange()
        {
            if (avatarRoot == null)
            {
                avatarRoot = UnittAvatarDetector.DetectFromSelection();
                Repaint();
            }
        }

        private void OnGUI()
        {
            titleContent = new GUIContent(UnittLocalization.AppName);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(UnittLocalization.AppName, EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "Drag an avatar, modification profile, accessory prefab, or replacement material here to apply an avatar modification.",
                MessageType.Info);

            DrawObjectFields();
            DrawActionButtons();
            DrawValidationResults();

            EditorGUILayout.EndScrollView();
        }

        private void DrawObjectFields()
        {
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            avatarRoot = (GameObject)EditorGUILayout.ObjectField(
                "Avatar Root",
                avatarRoot,
                typeof(GameObject),
                true);
            if (EditorGUI.EndChangeCheck())
            {
                validationResults.Clear();
            }

            EditorGUI.BeginChangeCheck();
            modificationProfile = (UnittModificationProfile)EditorGUILayout.ObjectField(
                "Modification Profile",
                modificationProfile,
                typeof(UnittModificationProfile),
                false);
            if (EditorGUI.EndChangeCheck())
            {
                ApplyProfile(modificationProfile);
                validationResults.Clear();
            }

            EditorGUI.BeginChangeCheck();
            accessoryPrefab = (GameObject)EditorGUILayout.ObjectField(
                "Accessory Prefab",
                accessoryPrefab,
                typeof(GameObject),
                false);
            if (EditorGUI.EndChangeCheck())
            {
                validationResults.Clear();
            }

            EditorGUI.BeginChangeCheck();
            replacementMaterial = (Material)EditorGUILayout.ObjectField(
                "Replacement Material",
                replacementMaterial,
                typeof(Material),
                false);
            if (EditorGUI.EndChangeCheck())
            {
                if (replacementMaterial != null && modificationProfile == null)
                {
                    applyMaterialToAllSkinnedMeshRenderers = true;
                }

                validationResults.Clear();
            }

            EditorGUI.BeginChangeCheck();
            attachBoneName = EditorGUILayout.TextField("Attach Bone Name", attachBoneName);
            applyMaterialToAllSkinnedMeshRenderers = EditorGUILayout.ToggleLeft(
                "Apply Material To All Skinned Mesh Renderers",
                applyMaterialToAllSkinnedMeshRenderers);
            if (EditorGUI.EndChangeCheck())
            {
                validationResults.Clear();
            }
        }

        private void DrawActionButtons()
        {
            EditorGUILayout.Space();

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Validate"))
                {
                    Validate();
                }

                if (GUILayout.Button("Dry Run"))
                {
                    RunDryRun();
                }

                if (GUILayout.Button("Apply"))
                {
                    Apply();
                }

                if (GUILayout.Button("Clear"))
                {
                    Clear();
                }
            }
        }

        private void DrawValidationResults()
        {
            EditorGUILayout.Space();

            if (validationResults.Count == 0)
            {
                EditorGUILayout.HelpBox("No validation results yet.", MessageType.None);
                return;
            }

            foreach (var result in validationResults)
            {
                EditorGUILayout.HelpBox(result.Message, ToMessageType(result.Severity));
            }
        }

        private void ApplyProfile(UnittModificationProfile profile)
        {
            if (profile == null)
            {
                return;
            }

            accessoryPrefab = profile.accessoryPrefab;
            replacementMaterial = profile.replacementMaterial;
            attachBoneName = string.IsNullOrWhiteSpace(profile.attachBoneName) ? "Head" : profile.attachBoneName;
            applyMaterialToAllSkinnedMeshRenderers = profile.applyMaterialToAllSkinnedMeshRenderers;
        }

        private void Validate()
        {
            validationResults = UnittModificationApplier.Validate(
                avatarRoot,
                accessoryPrefab,
                replacementMaterial,
                attachBoneName,
                applyMaterialToAllSkinnedMeshRenderers);
        }

        private void RunDryRun()
        {
            Validate();
            UnittModificationApplier.Apply(
                avatarRoot,
                accessoryPrefab,
                replacementMaterial,
                attachBoneName,
                applyMaterialToAllSkinnedMeshRenderers,
                true);
        }

        private void Apply()
        {
            Validate();
            if (UnittModificationApplier.Apply(
                    avatarRoot,
                    accessoryPrefab,
                    replacementMaterial,
                    attachBoneName,
                    applyMaterialToAllSkinnedMeshRenderers,
                    false))
            {
                Validate();
            }
        }

        private void Clear()
        {
            avatarRoot = null;
            modificationProfile = null;
            accessoryPrefab = null;
            replacementMaterial = null;
            attachBoneName = "Head";
            applyMaterialToAllSkinnedMeshRenderers = false;
            validationResults.Clear();
        }

        private static MessageType ToMessageType(UnittValidationSeverity severity)
        {
            switch (severity)
            {
                case UnittValidationSeverity.Error:
                    return MessageType.Error;
                case UnittValidationSeverity.Warning:
                    return MessageType.Warning;
                default:
                    return MessageType.Info;
            }
        }
    }
}
