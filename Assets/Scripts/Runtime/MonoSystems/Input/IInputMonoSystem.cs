using PlazmaGames.Core.MonoSystem;
using UnityEngine;
using UnityEngine.Events;

namespace DOH.MonoSystem
{
    public interface IInputMonoSystem : IMonoSystem
    {
        public UnityEvent JumpAction { get; }
        public UnityEvent InteractCallback { get; }
        public UnityEvent RCallback { get; }
        public Vector2 RawMovement { get; }
        public Vector2 RawLook { get; }
    }
}
