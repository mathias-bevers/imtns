using KattenKasteel.FSM;
using UnityEditor;
using UnityEngine;

namespace KattenKasteel.FMS.Editor
{
    public static class EditorUtils
    {
        public static string ToBackingField(this string source) => string.Concat('<', source, '>', "k__BackingField");
    }
}
