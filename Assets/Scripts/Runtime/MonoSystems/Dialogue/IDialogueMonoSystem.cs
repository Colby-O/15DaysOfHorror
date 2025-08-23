using PlazmaGames.Core.MonoSystem;
using UnityEngine;

namespace DOH
{
    public interface IDialogueMonoSystem : IMonoSystem
    {
        public void CloseDialogue();
        public void Load(DialogueSO dialogueEvent);
        public bool IsLoaded(DialogueSO dialogueEven);
        public void ResetDialogue();
        public void ResetDialogueAll();
    }
}
