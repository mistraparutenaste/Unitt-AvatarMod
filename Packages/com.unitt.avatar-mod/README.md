# Unitt!

Unitt! is a Unity Editor extension for applying simple VRChat avatar
modifications from one drag-and-drop window.

## Display Name

The Package Manager and VPM display name is `Unitt!`.

The Unity Editor window title and main header use:

- `ゆにっと！` when the Unity Editor language is Japanese.
- `Unitt!` for all other Editor languages.

The menu path is fixed as `Tools > Unitt! > Open`.

## Usage

1. Open a Unity 2022.3 avatar project.
2. Select `Tools > Unitt! > Open`.
3. Drag a scene avatar into `Avatar Root`.
4. Drag an accessory prefab or replacement material into the window.
5. Set `Attach Bone Name`; the default is `Head`.
6. Press `Validate`.
7. Press `Dry Run` to preview Console output.
8. Press `Apply`.

## Notes

- Accessory prefabs are instantiated under the named bone when found.
- Replacement materials can be applied to all `SkinnedMeshRenderer` material
  slots.
- All scene changes are registered with Unity Undo.
- VRChat SDK types are detected by name so this package can compile without a
  direct SDK assembly reference.

## License

MIT. See `LICENSE.md`.
