using PlazmaGames.Core;
using PlazmaGames.UI;
using UnityEngine;

namespace DOH
{
    public class PictureInteract : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _hint;
        [SerializeField] private Items _picture;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Sprite _extra;
        [SerializeField] private Items _requiredItem;
        [SerializeField] private Items _requiredItem2;
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
            DOHGameManager.PickupManager.GiveItem(_picture);
            GameManager.GetMonoSystem<IUIMonoSystem>().GetView<GameView>().ShowImage(_sprite, _extra);
            return true;
        }

        public bool IsInteractable()
        {
            return DOHGameManager.PickupManager.HasItem(Items.Camera) && DOHGameManager.PickupManager.HasItem(_requiredItem) && DOHGameManager.PickupManager.HasItem(_requiredItem2) && !DOHGameManager.PickupManager.HasItem(_picture);
        }

        public void RemoveOutline()
        {

        }

        public void Restart()
        {

        }
    }
}
