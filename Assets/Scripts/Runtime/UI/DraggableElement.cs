using UnityEngine;
using UnityEngine.EventSystems;

namespace DOH
{
    public class DraggableElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform _rectTransform;
        [SerializeField] private Canvas _canvas;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _rectTransform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
    }
}
