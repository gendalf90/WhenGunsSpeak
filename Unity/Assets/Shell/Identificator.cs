using UnityEngine;

namespace Shell
{
    public class Identificator : MonoBehaviour
    {
        [SerializeField]
        private string soldierId;

        [SerializeField]
        private string shellId;

        public string SoldierId
        {
            get => soldierId;
            set => soldierId = value;
        }

        public string ShellId
        {
            get => shellId;
            set => shellId = value;
        }
    }
}
