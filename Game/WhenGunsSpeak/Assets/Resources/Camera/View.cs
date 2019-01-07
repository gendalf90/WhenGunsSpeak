using Messages;
using UnityEngine;
using UnityCamera = UnityEngine.Camera;

namespace Camera
{
    public class View : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<SetCameraPositionCommand>(SetCameraPositionHandle);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<SetCameraPositionCommand>(SetCameraPositionHandle);
        }

        private void SetCameraPositionHandle(SetCameraPositionCommand command)
        {
            var currentPosition = UnityCamera.main.transform.position;
            var newPosition = new Vector3(command.WorldPositon.x, command.WorldPositon.y, currentPosition.z);
            UnityCamera.main.transform.position = newPosition;
        }
    }
}