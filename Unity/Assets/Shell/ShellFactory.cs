using UnityEngine;

namespace Shell
{
    public class ShellData
    {
        public string SoldierId { get; set; }

        public string ShellId { get; set; }

        public string ShellKey { get; set; }

        public Vector2 Position { get; set; }

        public float Rotation { get; set; }
    }

    public class ShellFactory : MonoBehaviour
    {
        private GameObject prefab;

        private GameObject currentShell;
        private ShellData currentData;

        private void Awake()
        {
            prefab = Resources.Load<GameObject>("Shell");
        }

        public void CreateAndThrowShell(ShellData data)
        {
            currentShell = Instantiate(prefab);
            currentData = data;

            SetIdentificator();
            SetPosition();
            Throw();
        }

        private void SetIdentificator()
        {
            var identificator = currentShell.GetComponent<Identificator>();

            identificator.SoldierId = currentData.SoldierId;
            identificator.ShellId = currentData.ShellId;
        }

        private void SetPosition()
        {
            var position = currentShell.GetComponent<Position>();

            position.SetPosition(currentData.Position);
            position.SetRotation(currentData.Rotation);
        }

        private void Throw()
        {
            var throwers = currentShell.GetComponents<IThrowable>();

            foreach (var thrower in throwers)
            {
                thrower.ThrowIfKeyIs(currentData.ShellKey);
            }
        }
    }
}
