using TMPro;
using UnityEngine;

namespace Menu.Garage.Computer
{
    public class DialogueWindow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _messageText;
        
        public void SetMessageText(string message)
        {
            _messageText.SetText(message);
        }
        
        public void OnCloseButton()
        {
            Close();
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}