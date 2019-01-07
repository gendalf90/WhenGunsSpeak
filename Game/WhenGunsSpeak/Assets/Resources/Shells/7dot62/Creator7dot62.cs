using Messages;
using System;
using UnityEngine;
using System.Linq;

namespace Shells
{
    class Creator7Dot62 : MonoBehaviour
    {
        [SerializeField]
        private string prefabPath;

        private GameObject prefab;
        private Observable observable;
        private ShellsSynchronizer synchronizer;

        private void Awake()
        {
            prefab = Resources.Load<GameObject>(prefabPath);
            observable = FindObjectOfType<Observable>();
            synchronizer = GetComponent<ShellsSynchronizer>();
        }

        private void CreateStatic(Guid id)
        {
            var newObject = Instantiate(prefab);
            var bullet = newObject.GetComponent<Bullet7dot62>();
            bullet.ShellGuid = id;
            var rigidbody = newObject.GetComponent<Rigidbody2D>();
            rigidbody.isKinematic = true;
            var hitDestroyer = newObject.GetComponent<HitDestroyer>();
            hitDestroyer.enabled = false;
            var hit = newObject.GetComponent<Hit>();
            hit.enabled = false;
            var thrower = newObject.GetComponent<Thrower>();
            thrower.enabled = false;
            var shellSynchronizer = newObject.GetComponent<ShellSynchronizer>();
            shellSynchronizer.ShellGuid = id;
        }

        private void Destroy(Guid id)
        {
            FindObjectsOfType<Bullet7dot62>().FirstOrDefault(obj => obj.ShellGuid == id).Do(obj => Destroy(obj.gameObject));
        }

        private void OnEnable()
        {
            observable.Subscribe<Throw7dot62Command>(CreateAndThrow);
            synchronizer.WhenCreatedOnRoomClient += CreateStatic;
            synchronizer.WhenDeletedOnRoomClient += Destroy;
        }

        private void OnDisable()
        {
            observable.Unsubscribe<Throw7dot62Command>(CreateAndThrow);
            synchronizer.WhenCreatedOnRoomClient -= CreateStatic;
            synchronizer.WhenDeletedOnRoomClient -= Destroy;
        }

        private void CreateAndThrow(Throw7dot62Command command)
        {
            var newObject = Instantiate(prefab);
            var bullet = newObject.GetComponent<Bullet7dot62>();
            bullet.ShellGuid = command.Id;
            var transform = newObject.GetComponent<Transform>();
            transform.position = command.Position;
            transform.rotation = Quaternion.Euler(0, 0, command.Rotation);
            var hitDestroyer = newObject.GetComponent<HitDestroyer>();
            hitDestroyer.ShellGuid = command.Id;
            var hitter = newObject.GetComponent<Hit>();
            hitter.ShellGuid = command.Id;
            var shellSynchronizer = newObject.GetComponent<ShellSynchronizer>();
            shellSynchronizer.ShellGuid = command.Id;
        }

        private void Update()
        {
            var syncIds = FindObjectsOfType<Bullet7dot62>().Select(obj => obj.ShellGuid);
            synchronizer.SetRoomOwnerIdsToSynchronization(syncIds);
        }
    }
}
