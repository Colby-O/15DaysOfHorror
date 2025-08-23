using PlazmaGames.Core;
using PlazmaGames.UI;
using System.Collections.Generic;
using UnityEngine;

namespace DOH
{
    public class DirayPuzzleView : View
    {
        [SerializeField] private List<RectTransform> _pieaces;
        [SerializeField] private List<RectTransform> _solutions;
        [SerializeField] private float _tolerence = 0.1f;

        public bool IsSolved()
        {
            bool isSolved = true;

            for (int i = 0; i < _pieaces.Count; i++)
            {
                RectTransform pieace = _pieaces[i];
                RectTransform solution = _solutions[i];

                isSolved &= Vector3.Distance(pieace.position, solution.position) < _tolerence;
            }

            return isSolved;
        }

        public override void Init()
        {

        }

        public override void Show()
        {
            base.Show();
            DOHGameManager.ShowCusor();
        }

        private void Update()
        {
            if (IsSolved())
            {
                GameManager.GetMonoSystem<IUIMonoSystem>().ShowLast();
            }
        }
    }
}
