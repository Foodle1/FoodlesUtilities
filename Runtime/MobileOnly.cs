using UnityEngine;

namespace FoodlesUtilities
{
    public class MobileOnly : MonoBehaviour
    {
        private void Start()
        {
#if !(UNITY_ANDROID || UNITY_IOS)

            Destroy(gameObject);

#endif
        }
    }
}