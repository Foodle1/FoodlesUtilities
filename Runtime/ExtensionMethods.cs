using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable once CheckNamespace
namespace FoodlesUtilities
{
    public static class ExtensionMethods
    {
        private static readonly System.Random _rng = new System.Random();

        /// <summary>
        /// Flips a bool
        /// </summary>
        /// <param name="boolToFlip"></param>
        public static void Flip(ref this bool boolToFlip)
        {
            boolToFlip = !boolToFlip;
        }

        #region String

        /// <summary>
        /// Appends the appropriate suffix to a number
        /// </summary>
        /// <param name="number">The number to return with suffix</param>
        /// <returns>Returns a number with the appropriate suffix</returns>
        public static string GetNumberWithSuffix(int number)
        {
            if (number is >= 11 and <= 13)
            {
                return number + "th";
            }

            return (number % 10) switch
            {
                1 => number + "st",
                2 => number + "nd",
                3 => number + "rd",
                _ => number + "th"
            };
        }
        
        public static string AddSpacesToString(this string text, bool preserveAcronyms = true)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            var newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (var i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) && 
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        #endregion

        #region CopyPaste

        /// <summary>
        /// Copies a string to clipboard
        /// </summary>
        /// <param name="str"></param>
        public static void CopyToClipboard(this string str)
        {
            GUIUtility.systemCopyBuffer = str;
            Debug.Log($"Copied: {str}");
        }

        /// <summary>
        /// Pastes from clipboard
        /// </summary>
        /// <returns>String from clipboard</returns>
        public static string PasteFromClipboard()
        {
            return GUIUtility.systemCopyBuffer;
        }

        #endregion

        #region Transform

        /// <summary>
        /// Recursively looks for transform with given name
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name">Name to look for</param>
        /// <param name="useContains">Whether name contains string or equals string</param>
        /// <returns>Found transform, otherwise null</returns>
        public static Transform FindRecursively(this Transform transform, string name, bool useContains = false)
        {
            Transform found = null;
            if (useContains)
            {
                transform.FindRecursivelyContainsHelper(name, ref found);
            }
            else
            {
                transform.FindRecursivelyHelper(name, ref found);
            }
            return found;
        }

        private static void FindRecursivelyHelper(this Transform transform, string name, ref Transform found)
        {
            foreach (Transform child in transform)
            {
                if (child.name == name) found = child;
                else
                {
                    child.transform.FindRecursivelyHelper(name, ref found);
                    if (found != null) break;
                }
            }
        }

        private static void FindRecursivelyContainsHelper(this Transform transform, string name, ref Transform found)
        {
            foreach (Transform child in transform)
            {
                if (child.name.Contains(name)) found = child;
                else
                {
                    child.transform.FindRecursivelyHelper(name, ref found);
                    if (found != null) break;
                }
            }
        }

        /// <summary>
        /// Returns all children in transform
        /// </summary>
        /// <param name="parent"></param>
        /// <returns>All children</returns>
        public static Transform[] GetChildren(this Transform parent)
        {
            var children = new Transform[parent.childCount];
            for (var i = 0; i < parent.childCount; i++)
            {
                children[i] = parent.GetChild(i);
            }

            return children;
        }

        //private static List<Transform> GetChildrenRecursive(this Transform parent)
        //{
        //    List<Transform> children = new List<Transform>();
        //    foreach (Transform child in parent)
        //    {
        //        children.Add(child);
        //        children.AddRange(GetChildrenRecursive(child));
        //    }

        //    return children;
        //}

        #endregion

        #region Vector

        /// <summary>
        /// Returns a random Vector3 between 2 points
        /// </summary>
        /// <param name="pointA">First point</param>
        /// <param name="pointB">Second point</param>
        /// <returns>Random Vector3 within provided points</returns>
        public static Vector3 RandomBetweenPoints(Vector3 pointA, Vector3 pointB)
        {
            return new Vector3(UnityEngine.Random.Range(pointA.x, pointB.x),
                UnityEngine.Random.Range(pointA.y, pointB.y), UnityEngine.Random.Range(pointA.z, pointB.z));
        }
        
        /// <summary>
        /// Returns a random Vector2 between 2 points
        /// </summary>
        /// <param name="pointA">First point</param>
        /// <param name="pointB">Second point</param>
        /// <returns>Random Vector2 within provided points</returns>
        public static Vector2 RandomBetweenPoints(Vector2 pointA, Vector2 pointB)
        {
            return new Vector2(UnityEngine.Random.Range(pointA.x, pointB.x),
                UnityEngine.Random.Range(pointA.y, pointB.y));
        }

        #endregion

        #region List

        /// <summary>
        /// Creates a new copy of a list and returns it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="useRandomOrder">Whether oder is randomised</param>
        /// <returns>A new copy of the list</returns>
        public static List<T> ListCopy<T>(this IEnumerable<T> list, bool useRandomOrder = false)
        {
            var newList = list.ToList();

            if (useRandomOrder)
            {
                newList = Shuffle(newList).ToList();
            }

            return newList;
        }

        /// <summary>
        /// Shuffles a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        public static IEnumerable<T> Shuffle<T>(IEnumerable<T> enumerable)
        {
            return enumerable.OrderBy(_ => _rng.Next()); // user453230 and Heinzi https://stackoverflow.com/questions/273313/randomize-a-listt
        }

        public static T Next<T>(this List<T> list, T currentItem)
        {
            if (list == null || list.Count == 0)
            {
                throw new ArgumentException("The list cannot be null or empty.");
            }

            var currentIndex = list.IndexOf(currentItem);

            if (currentIndex == -1)
            {
                throw new ArgumentException("The current item is not in the list.");
            }

            var nextIndex = (currentIndex + 1) % list.Count;
            return list[nextIndex];
        }

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            var array = enumerable as T[] ?? enumerable.ToArray();
            return array.ElementAt(_rng.Next(0, array.Length));
        }

        #endregion

        #region Remap

        /// <summary>
        /// Remaps a value in a range to another range
        /// </summary>
        /// <param name="source">Value to change</param>
        /// <param name="sourceMin">Original min value</param>
        /// <param name="sourceMax">Original max value</param>
        /// <param name="targetMin">New min value</param>
        /// <param name="targetMax">new max value</param>
        /// <returns>Remapped value</returns>
        public static float Remap(float source, float sourceMin, float sourceMax, float targetMin, float targetMax)
        {
            return targetMin + (source - sourceMin) * (targetMax - targetMin) / (sourceMax - sourceMin);
        }

        /// <summary>
        /// Remaps a value in a range to a 0 to 1 range
        /// </summary>
        /// <param name="source">Value to change</param>
        /// <param name="sourceMin">Original min value</param>
        /// <param name="sourceMax">Original max value</param>
        /// <returns>Remapped value</returns>
        public static float Remap01(float source, float sourceMin, float sourceMax)
        {
            return Remap(source, sourceMin, sourceMax, 0, 1);
        }

        /// <summary>
        /// Remaps a value in a range to a -1 to 1 range
        /// </summary>
        /// <param name="source">Value to change</param>
        /// <param name="sourceMin">Original min value</param>
        /// <param name="sourceMax">Original max value</param>
        /// <returns>Remapped value</returns>
        public static float Remap11(float source, float sourceMin, float sourceMax)
        {
            return Remap(source, sourceMin, sourceMax, -1, 1);
        }

        #endregion

        #region Rotation

        /// <summary>
        /// Convert angle from 0 - 360, to -180 - 180
        /// </summary>
        /// <param name="angle">Angle to convert</param>
        /// <returns>Wrapped Angle</returns>
        public static float WrapAngle(float angle)
        {
            angle %= 360;
            return angle > 180 ? angle - 360 : angle < -180 ? angle + 360 : angle;

            /*Human Readable version
            // Make sure that we get value between (-360, 360], we cannot use here module of 180 and call it a day, because we would get wrong values
            angle %= 360;
            if (angle > 180)
            {
                // If we get number above 180 we need to move the value around to get negative between (-180, 0]
                return angle - 360;
            }
            else if (angle < -180)
            {
                // If we get a number below -180 we need to move the value around to get positive between (0, 180]
                return angle + 360;
            }
            else
            {
                // We are between (-180, 180) so we just return the value
                return angle;
            }

             Credit Tomasz Juszczak
             https://stackoverflow.com/questions/47680017/how-to-limit-angles-in-180-180-range-just-like-unity3d-inspector
            */
        }

        /// <summary>
        /// Converts a Euler from 0 - 360, to -180 - 180
        /// </summary>
        /// <param name="euler">Euler to convert</param>
        /// <returns>Wrapped Euler</returns>
        public static Vector3 WrapEuler(Vector3 euler)
        {
            return new Vector3(WrapAngle(euler.x),
                WrapAngle(euler.y),
                WrapAngle(euler.z));
        }

        #endregion
        
        #region Enum

        /// <summary>
        /// Get random enum value of provided type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Returns a random enum</returns>
        public static T RandomEnumValue<T>() where T : Enum
        {
            var v = Enum.GetValues(typeof(T));
            return (T) v.GetValue(_rng.Next(v.Length));
        }

        public static T RandomEnumValue<T>(T[] excludedValues) where T : Enum
        {
            var v = Enum.GetValues(typeof(T));
            var values = v.Cast<T>().Where(value => !excludedValues.Contains(value)).ToArray();
            return values[_rng.Next(values.Length)];
        }

        /// <summary>
        /// Returns next enum in order
        /// </summary>
        /// <param name="current"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Next<T>(this T current) where T : Enum
        {
            var enums = (T[])Enum.GetValues(typeof(T));
            var indexOf = Array.IndexOf(enums, current) + 1;
            return enums.Length == indexOf ? enums[0] : enums[indexOf];
        }

        #endregion

        #region Animator

        public static bool IsPlaying(this Animator animator, string stateName, int animLayer)
        {
            return animator.GetCurrentAnimatorStateInfo(animLayer).IsName(stateName) &&
                   animator.GetCurrentAnimatorStateInfo(animLayer).normalizedTime < 1.0f;
        }

        #endregion

        #region Rigidbody

        /// <summary>
        /// Resets a rigidbodies velocity and angular velocity to 0
        /// </summary>
        /// <param name="rb"></param>
        public static void ResetVelocity(this Rigidbody rb)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        /// <summary>
        /// Sets a rigidbodies position and rotation
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="position">Target Position</param>
        /// <param name="rotation">Target Rotation</param>
        public static void SetPositionAndRotation(this Rigidbody rb, Vector3 position, Quaternion rotation)
        {
            rb.position = position;
            rb.rotation = rotation;
        }
        
        /// <summary>
        /// Sets a rigidbodies position and rotation
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="position">Target position</param>
        /// <param name="euler">Target rotation as euler</param>
        public static void SetPositionAndRotation(this Rigidbody rb, Vector3 position, Vector3 euler)
        {
            rb.SetPositionAndRotation(position, Quaternion.Euler(euler));
        }
        
        /// <summary>
        /// Sets a rigidbodies position and rotation
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="targetTransform">Transform to set position and rotation to</param>
        public static void SetPositionAndRotation(this Rigidbody rb, Transform targetTransform)
        {
            rb.SetPositionAndRotation(targetTransform.position, targetTransform.rotation);
        }

        #endregion

        #region Scene

        /// <summary>
        /// Reloads the current scene
        /// </summary>
        public static void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        #endregion
    }
}