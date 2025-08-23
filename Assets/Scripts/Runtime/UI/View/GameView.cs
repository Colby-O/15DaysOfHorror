using DOH.MonoSystem;
using PlazmaGames.Attribute;
using PlazmaGames.Core;
using PlazmaGames.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DOH
{
    public class GameView : View
    {
        [Header("Reference")]
        [SerializeField] private AudioSource _as;
        [SerializeField] private GameObject _holder;

        [Header("Settings")]
        [SerializeField] private float _typeSpeed = 0.1f;
        [SerializeField] private float _timeout = 4f;
        [SerializeField] private float _timeoutShow = 1f;

        [Header("Hints")]
        [SerializeField] private TMP_Text _hint;
        [SerializeField, ReadOnly] private bool _wasSetThisFrame = false;

        [Header("Dialogue")]
        [SerializeField] private float _waitDelayDialogue = 1f;
        [SerializeField] private TMP_Text _dialogueAvatarName;
        [SerializeField] private TMP_Text _dialogue;

        [Header("Text")]
        [SerializeField] private GameObject _readHolder;
        [SerializeField] private TMP_Text _readText;

        [Header("Picture")]
        [SerializeField] private Image _image;

        [SerializeField, ReadOnly] private bool _isWriting = false;
        [SerializeField, ReadOnly] private bool _isWritingDialogue = false;
        [SerializeField, ReadOnly] private bool _isWritingLocation = false;
        [SerializeField, ReadOnly] private float _timeSinceWriteStart = 0f;
        [SerializeField, ReadOnly] private string _currentMessage;
        [SerializeField, ReadOnly] private float _currentDelay = 0f;
        [SerializeField, ReadOnly] private bool _showedMessage = false;
        private bool _showingImage;

        IEnumerator TypeDialogue(string msg, float delay, float typeSpeed, TMP_Text target, bool isDialogue, UnityAction onFinished = null)
        {
            _isWriting = true;
            _currentMessage = msg;
            _showedMessage = false;
            _currentDelay = delay;

            if (isDialogue) _isWritingDialogue = true;
            else _isWritingLocation = true;

            _timeSinceWriteStart = 0f;

            if (_as) _as.Play();

            target.text = string.Empty;

            for (int i = 0; i < msg.Length; i++)
            {
                while (DOHGameManager.IsPaused)
                {
                    if (_as) _as.Stop();
                    yield return null;
                }

                if (_as && !_as.isPlaying) _as.Play();

                if (msg[i] == '<')
                {
                    int endIndex = msg.IndexOf('>', i);
                    if (endIndex != -1)
                    {
                        string fullTag = msg.Substring(i, endIndex - i + 1);
                        target.text += fullTag;
                        i = endIndex;
                        continue;
                    }
                }

                target.text += msg[i];
                yield return new WaitForSeconds(typeSpeed);
            }

            if (_as) _as.Stop();

            _showedMessage = true;
            yield return new WaitForSeconds(delay);

            if (isDialogue) _isWritingDialogue = false;
            else _isWritingLocation = false;

            _currentMessage = string.Empty;
            _currentDelay = 0f;
            _showedMessage = false;

            if (onFinished != null) onFinished.Invoke();
        }

        IEnumerator DialogueWait(float delay, UnityAction onFinished = null)
        {

            _timeSinceWriteStart = 0f;

            _showedMessage = true;
            yield return new WaitForSeconds(delay);

            _isWritingDialogue = false;

            _currentMessage = string.Empty;
            _showedMessage = false;

            if (onFinished != null) onFinished.Invoke();
        }

        private void Next()
        {
            _isWriting = false;
            GameManager.GetMonoSystem<IDialogueMonoSystem>().CloseDialogue();
        }

        public void SetHint(string hint)
        {
            _wasSetThisFrame = true;
            _hint.text = hint;
            _hint.transform.parent.gameObject.SetActive(true);
        }

        public void HideHint()
        {
            _hint.text = string.Empty;
            _hint.transform.parent.gameObject.SetActive(false);
        }

        public void ShowDialogue()
        {
            _dialogue.transform.parent.gameObject.SetActive(true);
        }

        public void DisplayDialogue(Dialogue dialogue)
        {
            _dialogueAvatarName.text = dialogue.avatarName;
            StartCoroutine(TypeDialogue(
                dialogue.msg[Language.EN], 
                (dialogue.waitDelayOverride <= 0) ? _waitDelayDialogue : dialogue.waitDelayOverride, 
                ((dialogue.typeSpeedOverride <= 0) ? _typeSpeed : dialogue.typeSpeedOverride),
                _dialogue, 
                true, 
                Next)
            );
        }

        public void HideDialogue()
        {
            _dialogueAvatarName.text = string.Empty;
            _dialogue.text = string.Empty;
            _dialogue.transform.parent.gameObject.SetActive(false);
        }

        public void ForceStopDialogue()
        {
            if (_isWritingDialogue)
            {
                StopAllCoroutines();
                _isWriting = false;
                _showedMessage = false;
                _isWritingDialogue = false;
                _isWritingLocation = false;
                _dialogue.text = string.Empty;
                _currentDelay = 0f;
                _as?.Stop();
                Next();
            }
        }

        public void SkipDialogue()
        {
            if (!_showedMessage && _isWritingDialogue)
            {
                StopAllCoroutines();
                _dialogue.text = _currentMessage;
                _as?.Stop();
                _showedMessage = true;
                StartCoroutine(DialogueWait(_currentDelay, Next));
            }
            else if(_isWritingDialogue)
            {
                StopAllCoroutines();
                _isWriting = false;
                _showedMessage = false;
                _isWritingDialogue = false;
                _isWritingLocation = false;
                _dialogue.text = string.Empty;
                _currentDelay = 0f;
                _as?.Stop();
                Next();
            }
        }

        public void ShowText(string text)
        {
            _readText.text = text;
            _readHolder.SetActive(true);
        }

        public void HideText()
        {
            _readText.text = string.Empty;
            _readHolder.SetActive(false);
        }

        public void ShowImage(Sprite sprite)
        {
            _image.gameObject.SetActive(true);
            _image.sprite = sprite;
            _showingImage = true;
            _wasSetThisFrame = true;
            SetHint("Click 'Space' To Close");
            DOHGameManager.IsPaused = true;
        }

        public void HideImage()
        {
            _image.gameObject.SetActive(false);
            _showingImage = false;
            HideHint();
            DOHGameManager.IsPaused= false;

            if (DOHGameManager.PickupManager.HasItem(Items.Pic6))
            {
                GameManager.GetMonoSystem<IUIMonoSystem>().Show<GameOverView>();
            }
        }

        public bool IsDialoguePlaying()
        {
            return _isWritingDialogue || _isWriting;
        }

        public override void Init()
        {
            HideHint();
            HideDialogue();
            HideText();
            HideImage();
        }

        public override void Show()
        {
            base.Show();
            _holder.SetActive(true);

            if (DOHGameManager.Inspector && DOHGameManager.Inspector.IsInspecting())
            {
                DOHGameManager.ShowCusor();
            }
            else
            {
                DOHGameManager.HideCusor();
            }
        }

        private void HandleTimeout()
        {
            if (DOHGameManager.IsPaused) return;

            if (_isWriting) _timeSinceWriteStart += Time.deltaTime;

            if (_timeSinceWriteStart > _timeout && !_showedMessage)
            {
                _showedMessage = true;
                if (_isWritingDialogue) _dialogue.text = _currentMessage;
            }
            else if (_timeSinceWriteStart > _timeout + _timeoutShow)
            {
                _isWriting = false;

                if (_isWritingDialogue)
                {
                    Next();
                }

                _showedMessage = false;
                _isWritingDialogue = false;
                _isWritingLocation = false;
                _as?.Stop();
            }
        }

        public override void Hide()
        {
            _holder.SetActive(false);
        }

        private void Start()
        {
            GameManager.GetMonoSystem<IInputMonoSystem>().JumpAction.AddListener(SkipDialogue);
        }

        private void Update()
        {
            if (!_wasSetThisFrame) HideHint();
            HandleTimeout();

            if (_showingImage && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                HideImage();
            }
        }

        private void LateUpdate()
        {
            if (!_showingImage) _wasSetThisFrame = false;
        }
    }
}
