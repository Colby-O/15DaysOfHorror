using UnityEngine;

namespace DOH
{
    [CreateAssetMenu(fileName = "DefaultEvent", menuName = "Dialogue/Events/Event1")]
    public class Event1 : DialogueEvent
    {
        [SerializeField] private DoorInteractable _door;

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {
            _door = GameObject.FindWithTag("Door1").GetComponent<DoorInteractable>();
            _door.CanInteract = true;
        }

        public override void OnUpdate()
        {

        }
    }
}
