using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unitt.AvatarMod;

namespace Unitt.AvatarMod.Editor
{
    public sealed class UnittAvatarModWindow : EditorWindow
    {
        private const float AvatarPreviewPanelWidth = 280f;
        private const float AvatarPreviewSize = 220f;
        private const float InventoryMinWidth = 320f;
        private const float InventoryIconSize = 42f;

        private GameObject avatarRoot;
        private UnittModificationProfile modificationProfile;
        private GameObject accessoryPrefab;
        private Material replacementMaterial;
        private string attachBoneName = "Head";
        private bool applyMaterialToAllSkinnedMeshRenderers;
        private Vector2 scrollPosition;
        private Vector2 inventoryScrollPosition;
        private readonly List<GameObject> inventoryPrefabs = new List<GameObject>();
        private List<UnittValidationResult> validationResults = new List<UnittValidationResult>();
        private GUIStyle panelStyle;
        private GUIStyle playfulHeaderStyle;
        private GUIStyle inventoryItemStyle;
        private GUIStyle mutedLabelStyle;

        [MenuItem("Tools/Unitt!/Open")]
        public static void Open()
        {
            var window = GetWindow<UnittAvatarModWindow>();
            window.titleContent = new GUIContent(UnittLocalization.AppName);
            window.minSize = new Vector2(720f, 460f);
            window.Show();
        }

        private void OnEnable()
        {
            titleContent = new GUIContent(UnittLocalization.AppName);
            minSize = new Vector2(720f, 460f);
            UnittModularAvatarDependency.PromptInstallIfMissing();

            if (avatarRoot == null)
            {
                avatarRoot = UnittAvatarDetector.DetectFromSelection();
            }

            RefreshInventoryPrefabs();
        }

        private void OnFocus()
        {
            RefreshInventoryPrefabs();
        }

        private void OnProjectChange()
        {
            RefreshInventoryPrefabs();
            Repaint();
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
            InitializeStyles();
            titleContent = new GUIContent(UnittLocalization.AppName);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            DrawHeader();
            DrawWardrobeLayout();
            DrawObjectFields();
            DrawActionButtons();
            DrawValidationResults();

            EditorGUILayout.EndScrollView();
        }

        private void DrawHeader()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(":: unitt! dress-up deck ::", playfulHeaderStyle);
            EditorGUILayout.HelpBox(
                "Pick an avatar on the left, then choose a loaded prefab from the Wardrobe Inventory on the right.",
                MessageType.Info);
        }

        private void DrawWardrobeLayout()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                DrawAvatarPreviewPanel();
                DrawInventoryPanel();
            }
        }

        private void DrawAvatarPreviewPanel()
        {
            using (new EditorGUILayout.VerticalScope(panelStyle, GUILayout.Width(AvatarPreviewPanelWidth)))
            {
                EditorGUILayout.LabelField("Avatar Preview", EditorStyles.boldLabel);
                avatarRoot = (GameObject)EditorGUILayout.ObjectField(
                    "Avatar Root",
                    avatarRoot,
                    typeof(GameObject),
                    true);

                var previewTexture = GetPreviewTexture(avatarRoot);
                var previewRect = GUILayoutUtility.GetRect(
                    AvatarPreviewSize,
                    AvatarPreviewSize,
                    GUILayout.Width(AvatarPreviewSize),
                    GUILayout.Height(AvatarPreviewSize));

                GUI.Box(previewRect, GUIContent.none);
                if (previewTexture != null)
                {
                    EditorGUI.DrawPreviewTexture(previewRect, previewTexture, null, ScaleMode.ScaleToFit);
                }
                else
                {
                    GUI.Label(previewRect, "Drop an avatar here", EditorStyles.centeredGreyMiniLabel);
                }

                var selectedName = accessoryPrefab != null ? accessoryPrefab.name : "nothing yet";
                EditorGUILayout.LabelField("Now wearing: " + selectedName, mutedLabelStyle);
            }
        }

        private void DrawInventoryPanel()
        {
            using (new EditorGUILayout.VerticalScope(panelStyle, GUILayout.MinWidth(InventoryMinWidth)))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Wardrobe Inventory", EditorStyles.boldLabel);
                    if (GUILayout.Button("Refresh", GUILayout.Width(72f)))
                    {
                        RefreshInventoryPrefabs();
                    }
                }

                EditorGUILayout.LabelField(
                    "Loaded project prefabs become playful outfit cards here.",
                    mutedLabelStyle);

                if (inventoryPrefabs.Count == 0)
                {
                    EditorGUILayout.HelpBox(
                        "No prefabs found in the project yet. Add outfit prefabs to Assets, then press Refresh.",
                        MessageType.Info);
                    return;
                }

                inventoryScrollPosition = EditorGUILayout.BeginScrollView(
                    inventoryScrollPosition,
                    GUILayout.MinHeight(220f));

                foreach (var prefab in inventoryPrefabs)
                {
                    DrawInventoryItem(prefab);
                }

                EditorGUILayout.EndScrollView();
            }
        }

        private void DrawInventoryItem(GameObject prefab)
        {
            if (prefab == null)
            {
                return;
            }

            using (new EditorGUILayout.HorizontalScope(inventoryItemStyle))
            {
                var icon = AssetPreview.GetMiniThumbnail(prefab) as Texture2D;
                GUILayout.Label(icon, GUILayout.Width(InventoryIconSize), GUILayout.Height(InventoryIconSize));

                using (new EditorGUILayout.VerticalScope())
                {
                    EditorGUILayout.LabelField(prefab.name, EditorStyles.boldLabel);
                    EditorGUILayout.LabelField(AssetDatabase.GetAssetPath(prefab), mutedLabelStyle);
                }

                using (new EditorGUILayout.VerticalScope(GUILayout.Width(92f)))
                {
                    if (GUILayout.Button("Set"))
                    {
                        accessoryPrefab = prefab;
                        validationResults.Clear();
                    }

                    if (GUILayout.Button("Wear"))
                    {
                        WearInventoryPrefab(prefab);
                    }
                }
            }
        }

        private void DrawObjectFields()
        {
            EditorGUILayout.Space();
            using (new EditorGUILayout.VerticalScope(panelStyle))
            {
                EditorGUILayout.LabelField("Fitting Room Controls", EditorStyles.boldLabel);

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

        private void RefreshInventoryPrefabs()
        {
            inventoryPrefabs.Clear();

            var prefabGuids = AssetDatabase.FindAssets("t:Prefab");
            foreach (var guid in prefabGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab != null)
                {
                    inventoryPrefabs.Add(prefab);
                }
            }

            inventoryPrefabs.Sort((left, right) =>
                string.Compare(left.name, right.name, StringComparison.OrdinalIgnoreCase));
        }

        private void WearInventoryPrefab(GameObject prefab)
        {
            accessoryPrefab = prefab;
            Apply();
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

        private void InitializeStyles()
        {
            if (panelStyle != null)
            {
                return;
            }

            panelStyle = new GUIStyle(EditorStyles.helpBox)
            {
                padding = new RectOffset(10, 10, 8, 10),
                margin = new RectOffset(4, 4, 4, 4)
            };

            playfulHeaderStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 15,
                alignment = TextAnchor.MiddleCenter
            };

            inventoryItemStyle = new GUIStyle(EditorStyles.helpBox)
            {
                padding = new RectOffset(6, 6, 6, 6),
                margin = new RectOffset(2, 2, 3, 3)
            };

            mutedLabelStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                wordWrap = true
            };
        }

        private static Texture2D GetPreviewTexture(UnityEngine.Object target)
        {
            if (target == null)
            {
                return null;
            }

            return AssetPreview.GetAssetPreview(target) ?? (AssetPreview.GetMiniThumbnail(target) as Texture2D);
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
