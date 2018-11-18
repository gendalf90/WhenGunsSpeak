using System;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Multiplayer
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
            toggle.onValueChanged.AddListener(Select);
            text = GetComponent<Text>();
            baseColor = text.color;
        }

        private void Select(bool value)
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

        public Guid OwnerId { get; set; }

        public string Header
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

        public bool IsSelected
        {
            get
            {
                return toggle.isOn;
            }
        }
    }
}