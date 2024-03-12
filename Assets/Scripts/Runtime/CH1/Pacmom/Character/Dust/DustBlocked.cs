using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class DustBlocked : MonoBehaviour
    {
        private Dust dust;
        private DustRoom room;
        [SerializeField]
        private InGameDialogue dialogue;
        [SerializeField]
        private float reachTime = 0f;
        private bool dustTalked = false;
        public bool isBlocked { get; private set; } = false;

        private void Awake()
        {
            dust = GetComponent<Dust>();
            room = GetComponent<DustRoom>();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            isBlocked = true;

            if (collision.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PlayerStr))
            {
                reachTime += Time.deltaTime;
            }

            if (!dustTalked && reachTime > 1.2f)
            {
                if (!room.isInRoom && dust.IsStronger())
                    dialogue.BlockedDialogue(dust.dustID);
                dustTalked = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            reachTime = 0f;
            dustTalked = false;
            isBlocked = false;
        }
    }
}