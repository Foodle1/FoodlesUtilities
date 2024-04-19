using System;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FoodlesUtilities.Editor
{
    public static class ScreenshotGrabber
    {
        [MenuItem("Screenshot/Grab")]
        [Shortcut("Screenshot", KeyCode.F6, ShortcutModifiers.Shift)]
        public static void Grab()
        {
            var localTime = DateTime.Now.ToLocalTime();
            var formattedString = $"{SceneManager.GetActiveScene().name}-{localTime:yyyy-MM-dd}-{localTime:HH-mm-ss}.png";
            ScreenCapture.CaptureScreenshot(formattedString, 1);
        }
    }
}