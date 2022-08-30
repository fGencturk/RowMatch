using UnityEditor;
using UnityEngine;

namespace _Scripts.Editor
{
    public static class MenuUtilityButtons
    {
        [MenuItem("Utilities/Open Persistent Path")]
        public static void OpenPersistantPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }
}