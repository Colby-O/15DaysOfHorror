using DOH.Player;
using UnityEngine;

namespace DOH
{
    public interface IInteractable 
    {
        public bool Interact(Interactor interactor);

        public void EndInteraction();

        public bool IsInteractable();

        public void AddOutline();

        public void RemoveOutline();

        public string GetHint();

        public void Restart();
    }
}