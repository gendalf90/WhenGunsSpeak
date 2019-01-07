using Messages;
using UnityEngine;
using UnityInput = UnityEngine.Input;
using UnityCamera = UnityEngine.Camera;

namespace Input
{
    public class Mouse : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void Update()
        {
            UpdateCursor();
            UpdateFire();
        }

        private void UpdateCursor()
        {
            observable.Publish(new CursorEvent(WorldPosition, ScreenPosition));
        }

        private Vector2 ScreenPosition
        {
            get
            {
                return UnityInput.mousePosition;
            }
        }

        private Vector2 WorldPosition
        {
            get
            {
                return UnityCamera.main.ScreenToWorldPoint(ScreenPosition);
            }
        }

        private void UpdateFire()
        {
            if (UnityInput.GetMouseButtonDown(0))
            {
                observable.Publish(new StartFireEvent());
            }

            if (UnityInput.GetMouseButtonUp(0))
            {
                observable.Publish(new EndFireEvent());
            }
        }
    }
}