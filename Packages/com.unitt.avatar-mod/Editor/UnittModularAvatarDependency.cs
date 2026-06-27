using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Unitt.AvatarMod.Editor
{
    internal static class UnittModularAvatarDependency
    {
        private const string ModularAvatarPackageName = "nadena.dev.modular-avatar";
        private const string ModularAvatarVpmUrl = "https://vpm.nadena.dev/";
        private const string PromptShownSessionKey = "Unitt.AvatarMod.ModularAvatarInstallPromptShown";

        public static bool IsInstalled()
        {
            return PackageInfo.FindForPackageName(ModularAvatarPackageName) != null;
        }

        public static void PromptInstallIfMissing()
        {
            if (IsInstalled() || SessionState.GetBool(PromptShownSessionKey, false))
            {
                return;
            }

            SessionState.SetBool(PromptShownSessionKey, true);

            var shouldOpenUrl = EditorUtility.DisplayDialog(
                UnittLocalization.AppName,
                "Modular Avatar is not installed in this project.\n\nInstall Modular Avatar with VCC or ALCOM, then reopen this Unity project before using Modular Avatar-based workflows.",
                "Open Modular Avatar VPM",
                "Later");

            if (shouldOpenUrl)
            {
                Application.OpenURL(ModularAvatarVpmUrl);
            }
        }
    }
}
