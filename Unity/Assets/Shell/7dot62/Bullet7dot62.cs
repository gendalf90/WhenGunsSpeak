using UnityEngine;

namespace Shell
{
    public class Bullet7dot62 : MonoBehaviour, IInitializable
    {
        private const string Name = "7dot62";
        private const float ThrowForce = 3.0f;

        private Thrower thrower;
        private GameObject bullet;

        private void Awake()
        {
            thrower = GetComponent<Thrower>();
            bullet = transform.Find("Bullet").gameObject;
        }

        public void InitializeIfNameIs(string shellName)
        {
            if(shellName != Name)
            {
                return;
            }

            thrower.SetForce(ThrowForce);
            bullet.SetActive(true);
        }
    }
}
