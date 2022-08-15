using UnityEngine;

namespace Tahsin
{
    [DisallowMultipleComponent]
    public class Player : MonoBehaviour
    {
        public static Player player;
        private void Start()
        {
            player = this;
        }
    }
}
