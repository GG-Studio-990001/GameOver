using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Vacuum : MonoBehaviour
    {
        public PMGameController gameController;

        private void Eaten()
        {
            gameController?.UseVacuum();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PacmomStr))
            {
                gameObject.SetActive(false);
                Eaten();
            }

        }
    }
}