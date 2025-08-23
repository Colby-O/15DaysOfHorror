using PlazmaGames.Animation;
using PlazmaGames.Attribute;
using PlazmaGames.Core;
using UnityEngine;
using UnityEngine.Events;

namespace DOH
{
    public class DoorInteractable : MonoBehaviour, IInteractable
    {
        public bool CanInteract = false;

        [Header("References")]
        [SerializeField] private Transform _pivot;
        [SerializeField] private Transform _center;

        [Header("Settings")]
        [SerializeField] private float _openSpeed = 1.5f;
        [SerializeField] private int _directionOverride = 0;

        [Header("Audio")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _openSound;
        [SerializeField] private AudioClip _closeSound;
        [SerializeField] private AudioClip _lockedSound;

        [SerializeField, ReadOnly] private bool _isOpen = false;
        [SerializeField, ReadOnly] private bool _inProgress = false;
        [SerializeField, ReadOnly] private Quaternion _startRot;

        public UnityEvent OnOpen = new UnityEvent();

        public void SetDirectionOverride(int dir)
        {
            _directionOverride = dir;
        }

        private float CurrentAngle()
        {
            float angle = _pivot.localRotation.eulerAngles.y;
            angle %= 360;
            angle = angle > 180 ? angle - 360 : angle;
            return angle;
        }

        public bool Interact(Interactor interactor)
        {
            if (_isOpen) Close();
            else Open(interactor.transform);
            return true;
        }

        public string GetHint()
        {
            return $"Click 'E' To {(_isOpen ? "Close" : "Open")}";
        }

        public void Open(Transform from, bool overrideAudio = false)
        {
            if (_isOpen) return;
            if (_inProgress) return;

            if (!overrideAudio && _audioSource && _openSound) _audioSource.PlayOneShot(_openSound);
            OnOpen.Invoke();

            _isOpen = true;
            _inProgress = true;
            float start = CurrentAngle();
            float target = ((_directionOverride == 0 && Vector3.Dot(-_center.forward, (_center.position - from.position).normalized) < 0) || _directionOverride < 0) ? -90 : 90;
            GameManager.GetMonoSystem<IAnimationMonoSystem>().RequestAnimation(
                this,
                _openSpeed,
                (float progress) =>
                {
                    _pivot.localRotation = Quaternion.Euler(0, start + (target - start) * progress, 0);
                },
                () =>
                {
                    _inProgress = false;
                }
            );
        }

        public void Restart()
        {
            Close(true, true);
        }

        public void Close(bool overrideAudio = false, bool force = false)
        {
            if (force)
            {
                GameManager.GetMonoSystem<IAnimationMonoSystem>().StopAllAnimations(this);
                _isOpen = false;
                _inProgress = false;
                _pivot.localRotation = _startRot;
                if (!overrideAudio && _audioSource && _closeSound) _audioSource.PlayOneShot(_closeSound);
                return;
            }

            if (!_isOpen) return;
            if (_inProgress) return;

            if (!overrideAudio && _audioSource && _closeSound) _audioSource.PlayOneShot(_closeSound);

            _inProgress = true;
            _isOpen = false;
            float start = CurrentAngle();
            GameManager.GetMonoSystem<IAnimationMonoSystem>().RequestAnimation(
                this,
                _openSpeed,
                (float progress) =>
                {
                    _pivot.localRotation = Quaternion.Euler(0, start - progress * start, 0);
                },
                () =>
                {
                    _inProgress = false;
                }
            );
        }

        public void AddOutline()
        {

        }

        public void EndInteraction()
        {

        }

        public bool IsInteractable()
        {
            return CanInteract;
        }

        public void RemoveOutline()
        {

        }
    }
}
