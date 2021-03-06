﻿using Messages;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Menu.Multiplayer
{
    class RoomsList : MonoBehaviour
    {
        private ToggleGroup toggleGroup;
        private GameObject itemPrefab;

        private Observable observable;
        private EventTimer refreshAllRoomsTimer;

        private RoomItem selectedItem;
        private RoomItem currentSelectedItem;

        public RoomsList()
        {
            refreshAllRoomsTimer = new EventTimer();
        }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            itemPrefab = Resources.Load<GameObject>("Scenes/MultiplayerMenu/RoomItem");
            toggleGroup = GetComponent<ToggleGroup>();
        }

        private void OnEnable()
        {
            observable.Subscribe<AllRoomsAreUpdatedEvent>(RefreshAllHandler);
            refreshAllRoomsTimer.Action += SendRefreshAllRoomsCommand;
        }

        private void Start()
        {
            refreshAllRoomsTimer.Start(TimeSpan.FromSeconds(3));
        }

        private void SendRefreshAllRoomsCommand()
        {
            observable.Publish(new RefreshAllRoomsCommand());
        }

        private void RefreshAllHandler(AllRoomsAreUpdatedEvent e)
        {
            CreateNewItems(e.AllRooms);
            RemoveOldItems(e.AllRooms);
        }

        private void CreateNewItems(IEnumerable<RoomShortData> rooms)
        {
            var existOwnerIds = GetComponentsInChildren<RoomItem>().Select(item => item.OwnerId).ToArray();
            var newRooms = rooms.Where(room => !existOwnerIds.Contains(room.OwnerId));

            foreach(var newRoom in newRooms)
            {
                CreateItem(newRoom);
            }
        }

        private void CreateItem(RoomShortData room)
        {
            var newItem = Instantiate(itemPrefab);
            var toggle = newItem.GetComponent<Toggle>();
            toggle.group = toggleGroup;
            newItem.transform.SetParent(transform);
            var item = newItem.GetComponent<RoomItem>();
            item.OwnerId = room.OwnerId;
            item.Header = room.Header;
        }

        private void RemoveOldItems(IEnumerable<RoomShortData> rooms)
        {
            var newRoomOwners = rooms.Select(room => room.OwnerId).ToArray();
            var oldOwners = GetComponentsInChildren<RoomItem>().Select(item => item.OwnerId)
                                                               .Where(ownerId => !newRoomOwners.Contains(ownerId))
                                                               .ToArray();

            foreach (var owner in oldOwners)
            {
                RemoveItem(owner);
            }
        }

        private void RemoveItem(Guid ownerId)
        {
            var toRemoveItem = GetComponentsInChildren<RoomItem>().FirstOrDefault(item => item.OwnerId == ownerId);

            if(toRemoveItem != null)
            {
                Destroy(toRemoveItem.gameObject);
            }
        }

        private void Update()
        {
            UpdateTimer();
            SendSelectedItemEventIfAny();
            SendAllItemsAreUnselectedIfSo();
        }

        private void UpdateTimer()
        {
            refreshAllRoomsTimer.Update();
        }

        private void SendSelectedItemEventIfAny()
        {
            SetSelectedItem();

            if(!IsThereSelectedItem)
            {
                return;
            }

            if(IsItemAlreadySelected)
            {
                return;
            }

            SendSelectedItemEvent();
            UpdateSelectedItem();
        }

        private bool IsThereSelectedItem => currentSelectedItem != null;

        private bool IsItemAlreadySelected => selectedItem != null && currentSelectedItem.OwnerId == selectedItem.OwnerId;

        private void SetSelectedItem()
        {
            currentSelectedItem = GetComponentsInChildren<RoomItem>().FirstOrDefault(item => item.IsSelected);
        }

        private void SendSelectedItemEvent()
        {
            observable.Publish(new RoomIsSelectedEvent(currentSelectedItem.OwnerId));
        }

        private void UpdateSelectedItem()
        {
            selectedItem = currentSelectedItem;
        }

        private void SendAllItemsAreUnselectedIfSo()
        {
            SetSelectedItem();

            if(IsThereSelectedItem)
            {
                return;
            }

            if(ThereWasNoSelectedItems)
            {
                return;
            }

            SendAllItemsAreUnselectedEvent();
            UpdateSelectedItem();
        }

        private bool ThereWasNoSelectedItems => selectedItem == null;

        private void SendAllItemsAreUnselectedEvent()
        {
            observable.Publish(new AllRoomsAreUnselectedEvent());
        }
    }
}