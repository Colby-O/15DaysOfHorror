using UnityEngine;

namespace DOH
{
    public class MusicBox : MonoBehaviour, IInteractable
    {
        private bool _hasInteracted = false;

        public void AddOutline()
        {

        }

        public void EndInteraction()
        {

        }

        public string GetHint()
        {
            return "Click 'E' To Use Music Box";
        }

        public bool Interact(Interactor interactor)
        {
            GameObject.FindWithTag("MusicBox").GetComponent<AudioSource>().Play();
            GameObject.FindWithTag("Door4").GetComponent<DoorInteractable>().CanInteract = true;
            _hasInteracted = true;
            return true;
        }

        public bool IsInteractable()
        {
            return !_hasInteracted && DOHGameManager.PickupManager.HasItem(Items.Pic2) && DOHGameManager.PickupManager.HasItem(Items.Pic3);
        }

        public void RemoveOutline()
        {

        }

        public void Restart()
        {

        }
    }
}
