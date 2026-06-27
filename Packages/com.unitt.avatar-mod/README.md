# Unitt!

unitt! unitt! unitt! tiny tools, tidy avatars.

## 日本語

Unitt! は、Unity 上で VRChat 向けアバター改変をドラッグ&ドロップで扱うための Editor 拡張です。左側にアバターのプレビュー、右側に Project 内 Prefab のインベントリを表示し、インベントリから装備を選んで着せるような感覚で操作できます。

Unity Editor 内の表示名は Editor の言語設定に応じて切り替わります。

- 日本語環境: `ゆにっと！`
- その他の環境: `Unitt!`

メニューは `Tools > Unitt! > Open` です。

### 使い方

1. Unity 2022.3 のアバタープロジェクトを開きます。
2. `Tools > Unitt! > Open` を選びます。
3. 左側の `Avatar Preview` に Scene 上のアバターを指定します。
4. 右側の `Wardrobe Inventory` から読み込まれている Prefab を選びます。
5. `Set` で選択するか、`Wear` でそのまま装備として適用します。
6. 必要に応じて `Attach Bone Name` を設定します。初期値は `Head` です。
7. `Validate`、`Dry Run`、`Apply` の順に確認しながら作業できます。

### 補足

- 右側のインベントリは `AssetDatabase.FindAssets("t:Prefab")` で Project 内 Prefab を読み込みます。
- Accessory Prefab は指定 Bone 配下、または Avatar Root 直下に追加されます。
- Material 差し替えは `SkinnedMeshRenderer` の全 slot を対象にできます。
- 変更は Unity Undo に登録されます。
- Modular Avatar が無い場合は、起動時にインストール案内を表示します。

## English

Unitt! is a Unity Editor extension for applying simple VRChat avatar modifications with a drag-and-drop workflow. It uses a playful wardrobe layout with an avatar preview on the left and a right-side inventory of loaded project prefabs.

The in-Editor display name follows the Unity Editor language setting.

- Japanese Editor: `ゆにっと！`
- Other Editor languages: `Unitt!`

The menu path is `Tools > Unitt! > Open`.

### Usage

1. Open a Unity 2022.3 avatar project.
2. Select `Tools > Unitt! > Open`.
3. Assign a scene avatar in the left-side `Avatar Preview`.
4. Pick a prefab from the right-side inventory, `Wardrobe Inventory`.
5. Press `Set` to select it, or `Wear` to apply it immediately.
6. Set `Attach Bone Name` if needed. The default is `Head`.
7. Use `Validate`, `Dry Run`, and `Apply` to review and apply the change.

### Notes

- The inventory loads project prefabs with `AssetDatabase.FindAssets("t:Prefab")`.
- Accessory prefabs are added under the named bone, or under Avatar Root as a fallback.
- Replacement materials can target every `SkinnedMeshRenderer` material slot.
- Scene changes are registered with Unity Undo.
- If Modular Avatar is missing, the window shows an installation prompt on startup.

## License

MIT. See `LICENSE.md`.
