using UnityEngine;

namespace Soldier
{
    public class Identificator : MonoBehaviour
    {
        [SerializeField]
        private string soldierId;

        public string SoldierId
        {
            get => soldierId;
            set => soldierId = value;
        }
    }
}
