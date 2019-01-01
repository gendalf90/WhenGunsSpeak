using Messages;
using UnityEngine;
using UnityCamera = UnityEngine.Camera;

namespace Camera
{
    public class View : MonoBehaviour
    {
        private Observable observable;
        private Vector3 currentPosition;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<SetCameraPositionCommand>(SetCameraPositionHandle);
        }

        private void SetCameraPositionHandle(SetCameraPositionCommand command)
        {
            currentPosition = new Vector3(command.WorldPositon.x, command.WorldPositon.y, currentPosition.z);
        }

        private void Start()
        {
            InitializeCurrentPostion();
        }

        private void InitializeCurrentPostion()
        {
            currentPosition = UnityCamera.main.transform.position;
        }

        private void LateUpdate()
        {
            UnityCamera.main.transform.position = currentPosition;
        }
    }
}