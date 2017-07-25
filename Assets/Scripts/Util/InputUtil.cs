﻿using UnityEngine;
using System;
using System.Collections.Generic;

namespace Lemmings.Util {
    /// <summary>
    /// Utility methods for querying input.
    /// </summary>
    static class InputUtil {

        /// <summary>
        /// Checks if a button was just pressed.
        /// </summary>
        /// <returns>Whether the button was just pressed.</returns>
        /// <param name="code">The ID of the button to check.</param>
        public static bool GetButtonDown(InputCode code) {
            return Input.GetButtonDown(code.ToString());
        }

        /// <summary>
        /// Gets whether a button is pressed. Accommodates for macOS trackpad right-clicking.
        /// </summary>
        /// <returns>Whether the button was pressed</returns>
        /// <param name="button">The ID of the button to check for.</param>
        public static bool GetButtonDownOrRightMouse(InputCode code) {
            return GetButtonDown(code) || Input.GetMouseButtonDown(0) && GetKey(KeyCode.LeftControl, KeyCode.RightControl);
        }

        /// <summary>
        /// Checks if a button is being pressed
        /// </summary>
        /// <returns>Whether the button is being pressed.</returns>
        /// <param name="code">The ID of the button to check.</param>
        public static bool GetButton(InputCode code) {
            return Input.GetButton(code.ToString());
        }

        /// <summary>
        /// Checks if all buttons in an array are pressed.
        /// </summary>
        /// <returns>Whether all given buttons are pressed.</returns>
        /// <param name="buttons">The buttons to check for being pressed.</param>
        public static bool GetButtons(params InputCode[] buttons) {
            foreach (InputCode button in buttons) {
                if (!GetButton(button)) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if any of the given keys are pressed.
        /// </summary>
        /// <returns>Whether any of the given keys are pressed.</returns>
        /// <param name="keys">The keys to check for being pressed.</param>
        public static bool GetKey(params KeyCode[] keys) {
            foreach (KeyCode key in keys) {
                if (Input.GetKey(key)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if any of the given keys have just been pressed.
        /// </summary>
        /// <returns>Whether any of the given keys have just been pressed.</returns>
        /// <param name="keys">The keys to check for having just been pressed.</param>
        public static bool GetKeyDown(params KeyCode[] keys) {
            foreach (KeyCode key in keys) {
                if (Input.GetKeyDown(key)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets a numerical value for the specified axis.
        /// </summary>
        /// <returns>A numerical value for the specified axis.</returns>
        /// <param name="code">The axis to get a value for.</param>
        public static float GetAxis(InputCode code) {
            return Input.GetAxis(code.ToString());
        }

        /// <summary>
        /// Gets the movement of the mouse in the current frame.
        /// </summary>
        /// <returns>The movement of the mouse in the current frame.</returns>
        public static Vector2 GetMouseMovement() {
            return new Vector2(GetAxis(InputCode.MouseX), GetAxis(InputCode.MouseY));
        }

        /// <summary>
        /// Gets the scroll wheel delta in this frame.
        /// </summary>
        /// <returns>The scroll wheel delta in this frame.</returns>
        public static float GetScrollWheel() {
            return GetAxis(InputCode.MouseScrollWheel);
        }

        /// <summary>
        /// Gets the index after a certain search text.
        /// </summary>
        /// <returns>The index after the search text.</returns>
        /// <param name="text">The text to search through.</param>
        /// <param name="find">The text to find the index after.</param>
        public static int GetIndexAfter(string text, string find) {
            int index = text.IndexOf(find);
            if (index >= 0) {
                index += find.Length;
            }
            return index;
        }

        /// <summary>
        /// Parses a list of vectors from text.
        /// </summary>
        /// <returns>The vector list parsed from the text.</returns>
        /// <param name="text">The text to parse.</param>
        /// <param name="startIndex">The text index to start parsing from.</param>
        public static List<Vector3> ParseVectorList(string text, ref int currentIndex) {
            List<Vector3> vectorList = new List<Vector3>();
            while (currentIndex < text.Length && text[currentIndex] == '(') {
                currentIndex++;
                float x = GetVectorNumber(text, ref currentIndex, ',');
                float y = GetVectorNumber(text, ref currentIndex, ',');
                float z = GetVectorNumber(text, ref currentIndex, ')');
                Vector3 newVector = new Vector3(x, y, z);
                vectorList.Add(newVector);
                currentIndex++;
            }
            return vectorList;
        }

        /// <summary>
        /// Parses a number for a vector.
        /// </summary>
        /// <returns>The vector number.</returns>
        /// <param name="text">The text to parse.</param>
        /// <param name="currentIndex">The current parsing index of the text.</param>
        /// <param name="endChar">The character after the number.</param>
        private static float GetVectorNumber(string text, ref int currentIndex, char endChar) {
            int endCharIndex = text.IndexOf(endChar, currentIndex);
            string numberText = text.Substring(currentIndex, endCharIndex - currentIndex);
            currentIndex = endCharIndex + 1;
            return (float)Convert.ToDouble(numberText);
        }

        /// <summary>
        /// Converts a vector to an array to send in a POST request.
        /// </summary>
        /// <returns>The string to send in the POST request.</returns>
        /// <param name="vector">The vector to convert.</param>
        public static string ConvertVectorToPOST(Vector3 vector) {
            return "[" + vector.x + "," + vector.y + "," + vector.z + "]";
        }
    }
}