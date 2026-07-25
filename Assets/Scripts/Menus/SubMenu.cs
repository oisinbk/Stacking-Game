using UnityEngine;

namespace Menus
{
    public class SubMenu : MonoBehaviour
    {
        [SerializeField] private GameObject thisMenu;
        [SerializeField] private GameObject mainMenu;

        public void BackToMainMenu()
        {
            mainMenu.SetActive(true);
            thisMenu.SetActive(false);
        }
    }
}