//using UnityEngine;
//using UnityEngine.UI;
//using System.Linq;
//using System;
//using System.Collections;
//using System.Collections.Generic;

//namespace MenuOfRooms
//{
//    class RoomsList : MonoBehaviour
//    {
//        private GameObject itemPrefab;
//        private ToggleGroup toggleGroup;

//        private void Awake()
//        {
//            itemPrefab = Resources.Load<GameObject>("Menu/Rooms/RoomItem");
//            toggleGroup = GetComponent<ToggleGroup>();
//        }

//        public RoomItem Add()
//        {
//            var newItem = Instantiate(itemPrefab);
//            var toggle = newItem.GetComponent<Toggle>();
//            toggle.group = toggleGroup;
//            newItem.transform.SetParent(transform);
//            return newItem.GetComponent<RoomItem>();
//        }

//        public IEnumerable<RoomItem> Items
//        {
//            get
//            {
//                return GetComponentsInChildren<RoomItem>();
//            }
//        }
//    }
//}