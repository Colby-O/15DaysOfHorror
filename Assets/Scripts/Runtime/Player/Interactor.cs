using DOH.MonoSystem;
using PlazmaGames.Attribute;
using PlazmaGames.Core;
using System.Collections.Generic;
using UnityEngine;

namespace DOH.Player
{
    public class Interactor : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _head;
        private IInputMonoSystem _input;

        [Header("Settings")]
        [SerializeField] private Transform _interactionPoint;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private float _interactionRadius = 0.1f;
        [SerializeField] private float _spehreCastRadius = 0.1f;

        [Header("Debugging")]
        [SerializeField, ReadOnly] private  List<IInteractable> _lastListOfPossibleInteractable;
        [SerializeField, ReadOnly] private bool _isInteracting = false;
        [SerializeField, ReadOnly] private IInteractable _currentInteractable;

        public void EndInteraction()
        {
            _currentInteractable?.EndInteraction();
        }

        private void StartInteraction(IInteractable interactable)
        {
            interactable.Interact(this);
        }

        private void CheckForInteractionInteract()
        {
            if (_isInteracting)
            {
                EndInteraction();
                return;
            }

            if
            (
                Physics.Raycast(_head.position, (_interactionPoint.position - _head.position).normalized, out RaycastHit hit, _interactionRadius, _interactionLayer) ||
                Physics.SphereCast(_head.position, _spehreCastRadius, (_interactionPoint.position - _head.position).normalized, out hit, _interactionRadius, _interactionLayer)
            )
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null && interactable.IsInteractable()) StartInteraction(interactable);
            }
        }

        private void CheckForPossibleInteractionInteract()
        {
            if (_isInteracting) return;

            List<IInteractable> possibleInteractable = new List<IInteractable>();

            if
            (
                Physics.Raycast(_head.position, (_interactionPoint.position - _head.position).normalized, out RaycastHit hit, _interactionRadius, _interactionLayer) ||
                Physics.SphereCast(_head.position, _spehreCastRadius, (_interactionPoint.position - _head.position).normalized, out hit, _interactionRadius, _interactionLayer)
            )
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    if (!interactable.IsInteractable()) return;
                    possibleInteractable.Add(interactable);
                    if (!_lastListOfPossibleInteractable.Contains(interactable)) interactable.AddOutline();
                    //if (interactable.GetHint() != string.Empty) GameManager.GetMonoSystem<IUIMonoSystem>().GetView<GameView>().SetHint(interactable.GetHint());
                }
            }

            foreach (IInteractable interactable in _lastListOfPossibleInteractable)
            {
                if (!possibleInteractable.Contains(interactable))
                {
                    if (interactable != null) interactable.RemoveOutline();
                }
            }

            _lastListOfPossibleInteractable = possibleInteractable;
        }

        private void Start()
        {
            _input = GameManager.GetMonoSystem<IInputMonoSystem>();
            _lastListOfPossibleInteractable = new List<IInteractable>();
            _input.InteractCallback.AddListener(CheckForInteractionInteract);
        }

        private void OnDisable()
        {
            foreach (IInteractable interactable in _lastListOfPossibleInteractable)
            {
                interactable.RemoveOutline();
            }
            _lastListOfPossibleInteractable.Clear();
        }

        private void LateUpdate()
        {
            CheckForPossibleInteractionInteract();
        }
    }
}