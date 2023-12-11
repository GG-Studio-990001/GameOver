using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class DustSpriteControl : MonoBehaviour
    {
        [SerializeField]
        private SpriteAnimation spriteAnim;

        [SerializeField]
        private Sprite[] normalSprites;
        [SerializeField]
        private Sprite[] frightenedSprites;

        [SerializeField]
        private GameObject eyes;

        public void Awake()
        {
            spriteAnim = new SpriteAnimation(GetComponent<SpriteRenderer>());
        }

        private void Start()
        {
            InvokeRepeating("SpriteAnimation", spriteAnim.animTime, spriteAnim.animTime);
        }

        private void SpriteAnimation()
        {
            spriteAnim.NextSprite();
        }

        public void GetNormalSprite()
        {
            spriteAnim.sprites = new Sprite[normalSprites.Length];

            for (int i = 0; i < spriteAnim.sprites.Length; i++)
            {
                spriteAnim.sprites[i] = normalSprites[i];
            }

            eyes.SetActive(true);
        }

        public void GetFrightendSprite()
        {
            spriteAnim.sprites = new Sprite[frightenedSprites.Length];

            for (int i = 0; i < spriteAnim.sprites.Length; i++)
            {
                spriteAnim.sprites[i] = frightenedSprites[i];
            }

            eyes.SetActive(false);
        }
    }
}
