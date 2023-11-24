using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FoodlesUtilities
{
    [RequireComponent(typeof(TMP_Text))]
    public class TmpCopy : MonoBehaviour, IPointerDownHandler
    {
        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_text.text == string.Empty) return;
        
            _text.text.CopyToClipboard();
        }
    }
}
