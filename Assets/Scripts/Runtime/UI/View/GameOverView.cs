using PlazmaGames.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DOH
{
    public class GameOverView : View
    {
        public override void Init()
        {
            
        }

        private void Update()
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                Application.Quit();
            }
        }
    }
}
