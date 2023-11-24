using UnityEditor;
using UnityEngine;

namespace FoodlesUtilities.Editor
{
    public static class ScreenshotGrabber
    {
        [MenuItem("Screenshot/Grab")]
        public static void Grab()
        {
            ScreenCapture.CaptureScreenshot("Screenshot.png", 1);
        }
    }
}