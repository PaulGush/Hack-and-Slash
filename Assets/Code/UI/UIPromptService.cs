using Code.Services;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class UIPromptService : MonoBehaviour, IUserInterfaceService
    {
        [SerializeField] private TextMeshProUGUI m_promptText;
        
        public void ShowText(string text)
        {
            m_promptText.text = text;
        }
        
        public void ClearText()
        {
            m_promptText.text = "";
        }
    }
}
