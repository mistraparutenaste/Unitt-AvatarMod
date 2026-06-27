using UnityEditor;

namespace Unitt.AvatarMod.Editor
{
    internal static class UnittLocalization
    {
        public static string AppName
        {
            get
            {
                var language = EditorPrefs.GetString("EditorLanguage");
                if (language == "ja")
                {
                    return "ゆにっと！";
                }

                return "Unitt!";
            }
        }

        public static string MenuPath => "Tools/" + AppName;
    }
}
