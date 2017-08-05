using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lemmings.Editor {

    /// <summary>
    /// Replaces the unavailable .NET 4.6 version for .csproj files with an available one (4.5.1).
    /// </summary>
    class ChangeNetVersion : UnityEditor.AssetModificationProcessor {

        /// <summary>
        /// Called when assets are saved.
        /// </summary>
        /// <param name="paths">The paths of saved assets.</param>
        static string[] OnWillSaveAssets(string[] paths) {
            foreach (string fileName in new string[]{"Assembly-CSharp.csproj", "Assembly-CSharp-Editor.csproj"}) {
                ReplaceNetString(fileName);
            }
            return paths;
        }

        /// <summary>
        /// Replaces the unavailable .NET 4.6 version for a .csproj file with an available one (4.5.1).
        /// </summary>
        /// <param name="fileName">The file path of the .csproj file.</param>
        private static void ReplaceNetString(string fileName) {
            string text = File.ReadAllText(fileName);
            text = text.Replace("v4.6", "v4.5.1");
            File.WriteAllText(fileName, text);
        }
    }
}