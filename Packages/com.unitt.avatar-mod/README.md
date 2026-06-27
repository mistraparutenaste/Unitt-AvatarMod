# Unitt!

> unitt! unitt! unitt! tiny tools, tidy avatars.

## 日本語

Unitt! は、Unity 上で VRChat 向けアバター改変をドラッグ&ドロップで扱うための Editor 拡張です。Avatar Root、Accessory Prefab、Replacement Material、Modification Profile をひとつのウィンドウに入れて、Validate、Dry Run、Apply の順に確認しながら作業できます。

Unity Editor 内の表示名は Editor の言語設定に応じて切り替わります。

- 日本語環境: `ゆにっと！`
- その他の環境: `Unitt!`

メニューは `Tools > Unitt! > Open` です。

### 使い方

1. Unity 2022.3 のアバタープロジェクトを開きます。
2. `Tools > Unitt! > Open` を選びます。
3. `Avatar Root` に Scene 上のアバターをドラッグ&ドロップします。
4. Accessory Prefab または Replacement Material を指定します。
5. 必要に応じて `Attach Bone Name` を設定します。初期値は `Head` です。
6. `Validate`、`Dry Run`、`Apply` の順に実行します。

### 補足

- Accessory Prefab は指定 Bone 配下、または Avatar Root 直下に追加されます。
- Material 差し替えは `SkinnedMeshRenderer` の全 slot を対象にできます。
- 変更は Unity Undo に登録されます。
- Modular Avatar が無い場合は、起動時にインストール案内を表示します。

## English

Unitt! is a Unity Editor extension for applying simple VRChat avatar modifications with a drag-and-drop workflow. Add an avatar root, accessory prefab, replacement material, or modification profile to one window, then validate, dry run, and apply the change.

The in-Editor display name follows the Unity Editor language setting.

- Japanese Editor: `ゆにっと！`
- Other Editor languages: `Unitt!`

The menu path is `Tools > Unitt! > Open`.

### Usage

1. Open a Unity 2022.3 avatar project.
2. Select `Tools > Unitt! > Open`.
3. Drag a scene avatar into `Avatar Root`.
4. Set an Accessory Prefab or Replacement Material.
5. Set `Attach Bone Name` if needed. The default is `Head`.
6. Run `Validate`, `Dry Run`, and `Apply`.

### Notes

- Accessory prefabs are added under the named bone, or under Avatar Root as a fallback.
- Replacement materials can target every `SkinnedMeshRenderer` material slot.
- Scene changes are registered with Unity Undo.
- If Modular Avatar is missing, the window shows an installation prompt on startup.

## License

MIT. See `LICENSE.md`.
