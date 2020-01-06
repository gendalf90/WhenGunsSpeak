using UnityEngine;

namespace Shell
{
    public class Bullet7dot62 : MonoBehaviour, IThrowable
    {
        private const string Key = "7dot62";
        private const float ThrowForce = 3.0f;

        private Thrower thrower;
        private GameObject bullet;

        private void Awake()
        {
            thrower = GetComponent<Thrower>();
            bullet = transform
                .Find("Bullet")
                .gameObject;
        }

        public void ThrowIfKeyIs(string shellKey)
        {
            if(shellKey != Key)
            {
                return;
            }

            thrower.SetForce(ThrowForce);
            bullet.SetActive(true);
        }
    }
}
