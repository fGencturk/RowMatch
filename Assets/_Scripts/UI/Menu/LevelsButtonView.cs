using Common.Event;
using Common.UI.Window.Event;
using UI.Menu.Windows;
using UnityEngine;

namespace UI.Menu
{
    public class LevelsButtonView : MonoBehaviour
    {

        #region Handlers

        public void _OnLevelsButtonClicked()
        {
            EventManager.Send(OpenWindowEvent.Create<LevelsWindow>());
        }

        #endregion

    }
}