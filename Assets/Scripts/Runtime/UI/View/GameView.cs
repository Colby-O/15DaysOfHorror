using PlazmaGames.UI;
using UnityEngine;

namespace DOH.UI
{
    public class GameView : View
    {
        public override void Init()
        {

        }

        public override void Show()
        {
            base.Show();
            DOHGameManager.ShowCusor();
        }
    }
}
