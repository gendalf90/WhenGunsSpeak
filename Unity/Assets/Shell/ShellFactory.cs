using UnityEngine;

namespace Shell
{
    public class CreateShellData
    {
        public string SoldierId { get; set; }

        public string ShellId { get; set; }

        public string ShellName { get; set; }

        public Vector2 Position { get; set; }

        public float Rotation { get; set; }
    }

    public class ShellFactory : MonoBehaviour
    {
        private GameObject prefab;

        private GameObject currentWeapon;
        private CreateShellData currentData;

        private void Awake()
        {
            prefab = Resources.Load<GameObject>("Shell");
        }

        public void CreateOfflineShell(CreateShellData data)
        {
            currentWeapon = Instantiate(prefab);
            currentData = data;

            SetIdentificator();
            SetPosition();
            Initialize();
        }

        private void SetIdentificator()
        {
            var identificator = currentWeapon.GetComponent<Identificator>();

            identificator.SoldierId = currentData.SoldierId;
            identificator.ShellId = currentData.ShellId;
        }

        private void SetPosition()
        {
            var position = currentWeapon.GetComponent<Position>();

            position.SetPosition(currentData.Position);
            position.SetRotation(currentData.Rotation);
        }

        private void Initialize()
        {
            var initializers = currentWeapon.GetComponents<IInitializable>();

            foreach (var initializer in initializers)
            {
                initializer.InitializeIfNameIs(currentData.ShellName);
            }
        }
    }
}
