using Rooms;
using UnityEngine;
using UnityEngine.UI;

namespace MenuOfRooms
{
    class RoomItem : MonoBehaviour
    {
        [SerializeField]
        private Color selectedColor;

        private Toggle toggle;
        private Text text;
        private Color baseColor;

        private void Awake()
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(Change);
            text = GetComponent<Text>();
            baseColor = text.color;
        }

        private void Change(bool value)
        {
            if (value)
            {
                text.color = selectedColor;
            }
            else
            {
                text.color = baseColor;
            }
        }

        public string Text
        {
            get
            {
                return text.text;
            }
            set
            {
                text.text = value;
            }
        }

        public bool IsChanged
        {
            get
            {
                return toggle.isOn;
            }
            set
            {
                toggle.isOn = value;
            }
        }

        public IRoomInfo Info { get; set; }
    }
}