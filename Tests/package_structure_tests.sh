#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT_DIR"

fail() {
  printf 'FAIL: %s\n' "$1" >&2
  exit 1
}

assert_file() {
  local path="$1"
  [[ -f "$path" ]] || fail "Expected file to exist: $path"
}

assert_contains() {
  local path="$1"
  local needle="$2"
  assert_file "$path"
  grep -Fq "$needle" "$path" || fail "Expected $path to contain: $needle"
}

assert_json_file() {
  local path="$1"
  assert_file "$path"
  python3 -m json.tool "$path" >/dev/null || fail "Expected valid JSON: $path"
}

assert_file "README.md"
assert_file ".gitignore"
assert_file "Packages/com.unitt.avatar-mod/README.md"
assert_file "Packages/com.unitt.avatar-mod/CHANGELOG.md"
assert_file "Packages/com.unitt.avatar-mod/LICENSE.md"
assert_json_file "Packages/com.unitt.avatar-mod/package.json"
assert_json_file "Packages/com.unitt.avatar-mod/Editor/UnittAvatarMod.Editor.asmdef"
assert_json_file "Packages/com.unitt.avatar-mod/Runtime/UnittAvatarMod.Runtime.asmdef"
assert_json_file "vpm/index.json"
assert_file ".github/workflows/release.yml"

assert_contains "Packages/com.unitt.avatar-mod/package.json" '"name": "com.unitt.avatar-mod"'
assert_contains "Packages/com.unitt.avatar-mod/package.json" '"displayName": "Unitt!"'
assert_contains "Packages/com.unitt.avatar-mod/package.json" '"version": "0.1.0"'
assert_contains "Packages/com.unitt.avatar-mod/package.json" '"unity": "2022.3"'
assert_contains "Packages/com.unitt.avatar-mod/package.json" '"com.vrchat.avatars": ">=3.7.0"'

assert_contains "Packages/com.unitt.avatar-mod/Runtime/UnittModificationProfile.cs" 'CreateAssetMenu('
assert_contains "Packages/com.unitt.avatar-mod/Runtime/UnittModificationProfile.cs" 'menuName = "Unitt!/Modification Profile"'
assert_contains "Packages/com.unitt.avatar-mod/Runtime/UnittModificationProfile.cs" 'public string attachBoneName = "Head";'
assert_contains "Packages/com.unitt.avatar-mod/Runtime/UnittModificationProfile.cs" 'public bool applyMaterialToAllSkinnedMeshRenderers = false;'

assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittLocalization.cs" 'EditorPrefs.GetString("EditorLanguage")'
assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittLocalization.cs" 'return "ゆにっと！";'
assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittLocalization.cs" 'return "Unitt!";'

assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittAvatarModWindow.cs" '[MenuItem("Tools/Unitt!/Open")]'
assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittAvatarModWindow.cs" 'UnittLocalization.AppName'
assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittAvatarModWindow.cs" 'UnittModularAvatarDependency.PromptInstallIfMissing();'
assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittAvatarModWindow.cs" 'Dry Run'
assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittAvatarModWindow.cs" 'Validate'
assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittAvatarModWindow.cs" 'Apply'

assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittAvatarDetector.cs" 'VRCAvatarDescriptor'
assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittModularAvatarDependency.cs" 'nadena.dev.modular-avatar'
assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittModularAvatarDependency.cs" 'PackageInfo.FindForPackageName'
assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittModularAvatarDependency.cs" 'EditorUtility.DisplayDialog'
assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittModularAvatarDependency.cs" 'Application.OpenURL'
assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittModularAvatarDependency.cs" 'SessionState'
assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittModificationApplier.cs" 'Undo.RegisterCreatedObjectUndo'
assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittModificationApplier.cs" 'Undo.RecordObject'
assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittModificationApplier.cs" 'EditorUtility.SetDirty'
assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittModificationApplier.cs" 'Enable material application or add an Accessory Prefab.'
assert_contains "Packages/com.unitt.avatar-mod/Editor/UnittValidationResult.cs" 'public enum UnittValidationSeverity'

assert_contains "vpm/index.json" '"id": "com.unitt.vpm"'
assert_contains ".github/workflows/release.yml" 'softprops/action-gh-release@v2'
assert_contains ".github/workflows/release.yml" 'sha256sum "${ZIP_NAME}" > "${ZIP_NAME}.sha256"'

printf 'All package structure tests passed.\n'
