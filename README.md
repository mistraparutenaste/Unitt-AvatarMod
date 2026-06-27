# Unitt-AvatarMod

Unitt-AvatarMod is a VPM-ready Unity Editor extension for applying simple
VRChat avatar modifications from a drag-and-drop GUI.

## Display Name

The package and VPM display name is `Unitt!`.

Inside the Unity Editor window, the application name is localized from the
Unity Editor language setting:

- Japanese Editor: `ゆにっと！`
- Other Editor languages: `Unitt!`

The menu path is fixed as `Tools > Unitt! > Open` because Unity menu attributes
require compile-time constants.

## Installation

Add this VPM repository URL to your VCC or ALCOM package sources:

```text
https://unitt.github.io/Unitt-AvatarMod/vpm/index.json
```

Then add `com.unitt.avatar-mod` to a Unity 2022.3 avatar project.

## Usage

1. Open an avatar project in Unity.
2. Select `Tools > Unitt! > Open`.
3. Drag a scene avatar into `Avatar Root`, or select an avatar in the hierarchy
   before opening the window.
4. Drag an accessory prefab and/or replacement material into the GUI.
5. Set `Attach Bone Name` if the accessory should be parented under a specific
   transform. The default is `Head`.
6. Press `Validate` to review errors and warnings.
7. Press `Dry Run` to log the planned changes without modifying the scene.
8. Press `Apply` to instantiate the accessory and/or replace materials.

## Notes

- Accessory prefabs are parented under the named transform when it exists, or
  under the avatar root as a fallback.
- Material replacement targets all slots of every `SkinnedMeshRenderer` when
  material application is enabled.
- Scene changes are registered with Unity Undo.
- VRChat SDK detection is optional. The editor extension searches for
  `VRCAvatarDescriptor` by component type name so the package can compile even
  when the SDK is not installed.

## License

MIT. See `LICENSE` and `Packages/com.unitt.avatar-mod/LICENSE.md`.
