using PlazmaGames.Core;
using UnityEngine;

namespace DOH
{
    public class EnityInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _hint;
        [SerializeField] private DialogueSO _dialogue;
        [SerializeField] private bool _talkedTo;

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
            _talkedTo = true;
            GameManager.GetMonoSystem<IDialogueMonoSystem>().Load(_dialogue);
            return true;
        }

        public bool IsInteractable()
        {
            return !_talkedTo;
        }

        public void RemoveOutline()
        {

        }

        public void Restart()
        {
            
        }
    }
}
