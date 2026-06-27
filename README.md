# Unitt-AvatarMod

> unitt! ちいさく、軽く、アバター改変をひとつに。

## 日本語

Unitt-AvatarMod は、Unity 上で VRChat 向けアバター改変をドラッグ&ドロップで行うための Unity Editor 拡張です。現在の UI は、左側にアバターのプレビュー、右側に読み込まれている Prefab のインベントリを置いた、ちょっと遊び心のある着せ替え画面になっています。

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
3. 左側の `Avatar Preview` に Scene 上のアバターを指定します。
4. 右側の `Wardrobe Inventory` で、Project 内に読み込まれている Prefab を選びます。
5. `Set` で `Accessory Prefab` にセットするか、`Wear` でそのまま装備として適用します。
6. 必要に応じて `Attach Bone Name` を設定します。初期値は `Head` です。
7. `Validate` でエラーや警告を確認します。
8. `Dry Run` で変更予定を Console に出力します。
9. `Apply` で実際に適用します。

### UI の見方

- 左側: アバタープレビューと現在選択中の装備名を表示します。
- 右側: `AssetDatabase.FindAssets("t:Prefab")` で Project 内の Prefab を読み込み、Wardrobe Inventory として表示します。
- `Refresh`: Project に追加した Prefab を再読み込みします。
- `Set`: Prefab を選択状態にします。
- `Wear`: 選択した Prefab を既存の Apply 処理へ渡して装備します。

### 補足

- このプロジェクトは Codex を使用して作成されました。
- Accessory Prefab は指定 Bone 配下、または Avatar Root 直下に追加されます。
- Material 差し替えは `SkinnedMeshRenderer` の全 slot を対象にできます。
- 変更は Unity Undo に登録されます。
- Modular Avatar が無い場合は、起動時にインストール案内を表示します。

## English

Unitt-AvatarMod is a Unity Editor extension for applying simple VRChat avatar modifications through a drag-and-drop GUI. The current interface is a playful wardrobe view: an avatar preview on the left and a right-side inventory of loaded prefabs on the right.

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
3. Assign a scene avatar in the left-side `Avatar Preview`.
4. Pick a loaded project prefab from the right-side inventory, `Wardrobe Inventory`.
5. Press `Set` to choose it as the `Accessory Prefab`, or press `Wear` to apply it immediately.
6. Set `Attach Bone Name` if needed. The default is `Head`.
7. Press `Validate` to review errors and warnings.
8. Press `Dry Run` to log the planned changes without modifying the scene.
9. Press `Apply` to apply the modification.

### UI Notes

- Left side: shows the avatar preview and currently selected outfit.
- Right side: loads project prefabs with `AssetDatabase.FindAssets("t:Prefab")` and shows them as Wardrobe Inventory cards.
- `Refresh`: reloads newly added project prefabs.
- `Set`: selects a prefab.
- `Wear`: sends the prefab through the existing Apply flow.

### Notes

- Accessory prefabs are parented under the named bone when found, or under Avatar Root as a fallback.
- Replacement materials can target every `SkinnedMeshRenderer` material slot.
- Scene changes are registered with Unity Undo.
- If Modular Avatar is missing, the window shows an installation prompt on startup.

## License

MIT. See `LICENSE` and `Packages/com.unitt.avatar-mod/LICENSE.md`.
