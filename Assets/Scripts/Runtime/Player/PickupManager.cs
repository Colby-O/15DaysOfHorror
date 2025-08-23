using PlazmaGames.Core;
using PlazmaGames.Runtime.DataStructures;
using PlazmaGames.UI;
using UnityEngine;

namespace DOH
{
    public enum Items
    {
        Camera,
        BasementKey,
        Pic1,
        Pic2,
        Pic3,
        Pic4,
        Pic5,
        Pic6,
        Diray,
    }

    public class PickupManager : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<Items, bool> _items;

        public bool HasItem(Items item)
        {
            return _items.ContainsKey(item) ? _items[item] : false;
        }

        public void GiveItem(Items item)
        {
            if (_items.ContainsKey(item)) _items[item] = true;
            else _items.Add(item, true);

            if (item == Items.Pic1)
            {
                GameObject.FindWithTag("Door2").GetComponent<DoorInteractable>().CanInteract = true;
            }
            else if (item == Items.BasementKey)
            {
                GameObject.FindWithTag("Door3").GetComponent<DoorInteractable>().CanInteract = true;
            }
            else if (item == Items.Diray)
            {
                GameObject.FindWithTag("MusicBox").GetComponent<AudioSource>().Stop();
                GameManager.GetMonoSystem<IUIMonoSystem>().Show<DirayPuzzleView>();
            }
        }

        private void Awake()
        {
            _items = new SerializableDictionary<Items, bool>();
        }

    }
}
