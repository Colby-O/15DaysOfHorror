using UnityEngine;

namespace DOH
{
    public class ItemPickup : MonoBehaviour, IInteractable
    {
        [SerializeField] Items _item;
        [SerializeField] private string _hint;

        public void AddOutline()
        {

        }

        public void EndInteraction()
        {

        }

        public string GetHint()
        {
            return _hint;
        }

        public bool Interact(Interactor interactor)
        {
            DOHGameManager.PickupManager.GiveItem(_item);
            Destroy(gameObject);
            return true;
        }

        public bool IsInteractable()
        {
            return true;
        }

        public void RemoveOutline()
        {

        }

        public void Restart()
        {

        }
    }
}
