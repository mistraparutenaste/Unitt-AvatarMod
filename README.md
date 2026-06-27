# Unitt-AvatarMod

> unitt! unitt! unitt! ちいさく、軽く、アバター改変をひとつに。

## 日本語

Unitt-AvatarMod は、Unity 上で VRChat 向けアバター改変をドラッグ&ドロップで行うための Unity Editor 拡張です。アバター、改変用 Prefab、Material、設定 Profile をひとつの IMGUI ウィンドウに入れて、Validate、Dry Run、Apply の流れで安全に適用できます。

Unity Editor 内の表示名は、Editor の言語設定に応じて切り替わります。

- 日本語環境: `ゆにっと！`
- その他の環境: `Unitt!`

メニューは Unity の属性制約により固定で `Tools > Unitt! > Open` です。

### インストール

VCC または ALCOM に以下の VPM Repository URL を追加してください。

```text
https://unitt.github.io/Unitt-AvatarMod/vpm/index.json
```

その後、Unity 2022.3 のアバタープロジェクトに `com.unitt.avatar-mod` を追加します。

### 使い方

1. Unity でアバタープロジェクトを開きます。
2. `Tools > Unitt! > Open` を選びます。
3. `Avatar Root` に Scene 上のアバターをドラッグ&ドロップします。
4. `Accessory Prefab` または `Replacement Material` をドラッグ&ドロップします。
5. 必要に応じて `Attach Bone Name` を設定します。初期値は `Head` です。
6. `Validate` でエラーや警告を確認します。
7. `Dry Run` で変更予定を Console に出力します。
8. `Apply` で実際に適用します。

### 補足

- Accessory Prefab は指定した Bone が見つかればその配下に、見つからなければ Avatar Root 直下に追加されます。
- Material 差し替えは、設定が有効な場合に `SkinnedMeshRenderer` の全 slot を対象にします。
- Scene への変更は Unity Undo に登録されます。
- VRChat SDK の型は直接参照せず、名前で検出するため、SDK が無い環境でもコンパイルしやすい構成です。
- Modular Avatar がプロジェクトに無い場合、起動時にインストール案内を表示します。

## English

Unitt-AvatarMod is a Unity Editor extension for applying simple VRChat avatar modifications through a drag-and-drop GUI. Drop an avatar, modification prefab, material, or saved profile into one IMGUI window, then validate, dry run, and apply the change with Undo support.

The in-Editor display name follows the Unity Editor language setting.

- Japanese Editor: `ゆにっと！`
- Other Editor languages: `Unitt!`

The menu path is fixed as `Tools > Unitt! > Open` because Unity menu attributes require compile-time constants.

### Installation

Add this VPM repository URL to VCC or ALCOM.

```text
https://unitt.github.io/Unitt-AvatarMod/vpm/index.json
```

Then add `com.unitt.avatar-mod` to a Unity 2022.3 avatar project.

### Usage

1. Open an avatar project in Unity.
2. Select `Tools > Unitt! > Open`.
3. Drag a scene avatar into `Avatar Root`.
4. Drag an `Accessory Prefab` or `Replacement Material` into the window.
5. Set `Attach Bone Name` if needed. The default is `Head`.
6. Press `Validate` to review errors and warnings.
7. Press `Dry Run` to log the planned changes without modifying the scene.
8. Press `Apply` to apply the modification.

### Notes

- Accessory prefabs are parented under the named bone when found, or under the avatar root as a fallback.
- Material replacement targets all slots of every `SkinnedMeshRenderer` when material application is enabled.
- Scene changes are registered with Unity Undo.
- VRChat SDK types are detected by name instead of direct assembly references.
- If Modular Avatar is missing, the window shows an installation prompt on startup.

## License

MIT. See `LICENSE` and `Packages/com.unitt.avatar-mod/LICENSE.md`.
