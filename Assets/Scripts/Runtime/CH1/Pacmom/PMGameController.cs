using Runtime.ETC;
using Runtime.InGameSystem;
using System.Collections;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PMGameController : MonoBehaviour
    {
        #region 선언
        private PMSpriteController spriteController;
        private PMUIController uiController;
        public SoundSystem soundSystem;
        [SerializeField]
        private Timer timer;

        [Header("=Character=")]
        [SerializeField]
        private Rapley rapley;

        [SerializeField]
        private Pacmom pacmom;
        private AI pacmomAI;

        [SerializeField]
        private Dust[] dusts = new Dust[GlobalConst.DustCnt];
        private AI[] dustAIs = new AI[GlobalConst.DustCnt];
        private Room[] dustRooms = new Room[GlobalConst.DustCnt];

        [Header("=Else=")]
        [SerializeField]
        private Transform coins;
        [SerializeField]
        private Transform vacuums;
        [SerializeField]
        private GameObject Door;

        [Header("=Variable=")]
        [SerializeField]
        private int inRoom = 2;
        [SerializeField]
        private bool isGameOver = false; // 아웃트로 구현 전 연출을 위한 임시 변수
        private int rapleyScore;
        private int pacmomScore;
        private int pacmomLives;
        private readonly float vacuumDuration = 10f;
        private readonly float vacuumEndDuration = 3f;
        #endregion

        #region Awake
        private void Awake()
        {
            AssignComponent();
            AssignController();
        }

        private void AssignComponent()
        {
            spriteController = GetComponent<PMSpriteController>();
            uiController = GetComponent<PMUIController>();

            pacmomAI = pacmom.GetComponent<AI>();

            for (int i = 0; i < dusts.Length; i++)
            {
                dustAIs[i] = dusts[i].GetComponent<AI>();
                dustRooms[i] = dusts[i].GetComponent<Room>();
            }
        }

        private void AssignController()
        {
            pacmom.gameController = this;

            for (int i = 0; i < dusts.Length; i++)
            {
                dusts[i].gameController = this;
            }

            foreach (Transform coin in coins)
            {
                coin.GetComponent<Coin>().gameController = this;
            }

            foreach (Transform vacuum in vacuums)
            {
                vacuum.GetComponent<Vacuum>().gameController = this;
            }
        }
        #endregion

        #region Set
        private void SetRapleyScore(int score)
        {
            rapleyScore = score;
            uiController.ShowRapleyScore(score);
        }

        private void SetPacmomScore(int score)
        {
            pacmomScore = score;
            uiController.ShowPacmomScore(score);
        }

        private void SetPacmomLives(int lives)
        {
            if (lives < 0)
                return;

            pacmomLives = lives;
        }
        #endregion

        #region Start
        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            SetRapleyScore(0);
            SetPacmomScore(0);
            SetPacmomLives(3);

            ResetStates();
            
            timer.SetTimer(true);
        }
        #endregion

        #region End
        public void GameOver()
        {
            timer.SetTimer(false);
            isGameOver = true;

            SetCharacterStop();

            if (HasRemainingCoins())
            {
                StartCoroutine(GetRemaningCoins());
            }
            else
            {
                ChooseAWinner();
            }
        }

        private void ChooseAWinner()
        {
            soundSystem.PlayMusic("Outro");

            if (rapleyScore > pacmomScore)
            {
                Debug.Log("라플리 승리");
                uiController.ShowGameOverUI("Rapley");
            }
            else
            {
                Debug.Log("팩맘 승리");
                uiController.ShowGameOverUI("Pacmom");
            }
        }
        #endregion

        #region Vacuum Mode
        public void UseVacuum()
        {
            if (!pacmom.ai.isStronger)
            {
                soundSystem.PlayMusic("StartVacuum");
            }
            else
            {
                StopCoroutine("VacuumTime");
                soundSystem.StopMusic();

                soundSystem.PlayMusic("ContinueVacuum");
            }

            StartCoroutine("VacuumTime");
        }

        private IEnumerator VacuumTime()
        {
            VacuumModeOn();

            yield return new WaitForSeconds(vacuumDuration - vacuumEndDuration);
            if (isGameOver) yield break;
            
            spriteController.SetPacmomBlinkSprite();

            yield return new WaitForSeconds(vacuumEndDuration);
            if (isGameOver) yield break;

            VacuumModeOff();
        }

        private void VacuumModeOn()
        {
            spriteController.SetVaccumModeSprites();
            SetVacuumSpeed();
            SetVacuumMode(true);
        }

        private void VacuumModeOff()
        {
            spriteController.SetNormalSprites();
            SetNormalSpeed();
            SetVacuumMode(false);

            DustExitRoom();
        }
        #endregion

        #region Common
        private void ResetStates()
        {
            spriteController.SetNormalSprites();

            rapley.ResetState();
            pacmom.ResetState();

            for (int i = 0; i < dusts.Length; i++)
            {
                dusts[i].ResetState();
                dustRooms[i].SetInRoom(true);
            }
            inRoom = 2;

            DustExitRoom();
        }

        private void SetCharacterStop()
        {
            rapley.movement.SetCanMove(false);
            pacmom.movement.SetCanMove(false);
            for (int i = 0; i < dusts.Length; i++)
                dusts[i].movement.SetCanMove(false);
        }

        private void SetVacuumMode(bool isVacuumMode)
        {
            pacmom.VacuumMode(isVacuumMode);
            pacmomAI.SetStronger(isVacuumMode);
            for (int i = 0; i < dusts.Length; i++)
                dustAIs[i].SetStronger(!isVacuumMode);

            Door.SetActive(isVacuumMode);
        }

        private void SetVacuumSpeed()
        {
            pacmom.movement.SetSpeedMultiplier(1.2f);
            rapley.movement.SetSpeedMultiplier(0.7f);
            for (int i = 0; i < dusts.Length; i++)
                dusts[i].movement.SetSpeedMultiplier(0.7f);
        }

        private void SetNormalSpeed()
        {
            pacmom.movement.SetSpeedMultiplier(1f);
            rapley.movement.SetSpeedMultiplier(1f);
            for (int i = 0; i < dusts.Length; i++)
                dusts[i].movement.SetSpeedMultiplier(1f);
        }

        private void DustExitRoom()
        {
            for (int i = 0; i < dusts.Length; i++)
            {
                if (dustRooms[i].isInRoom)
                {
                    dustRooms[i].ExitRoom(GlobalConst.DustCnt - inRoom);
                    inRoom--;
                }
            }
        }
        #endregion

        #region Eaten
        public void RapleyEaten()
        {
            soundSystem.PlayEffect("PacmomEat");

            TakeHalfCoins(false);
            rapley.ResetState();
        }

        public void DustEaten(Dust dust)
        {
            soundSystem.PlayEffect("PacmomEat");

            dust.movement.SetCanMove(false);
            dust.ResetState();
            dust.GetComponent<Room>().SetInRoom(true);
            inRoom++;
        }

        public void PacmomEaten(string byWhom)
        {
            soundSystem.PlayEffect("PacmomStun");

            Debug.Log("팩맘 먹힘");

            if (byWhom == GlobalConst.PlayerStr)
                TakeHalfCoins(true);
            else if (byWhom == GlobalConst.DustStr)
                ReleaseHalfCoins();

            SetPacmomLives(pacmomLives - 1);
            uiController.LosePacmomLife(pacmomLives);

            if (pacmomLives > 0)
            {
                ResetStates();
            }
            else
            {
                pacmom.SetRotateToZero();
                spriteController.SetPacmomDieSprite();

                GameOver();
            }
        }

        public void CoinEaten(string byWhom)
        {
            if (byWhom == GlobalConst.PlayerStr)
            {
                soundSystem.PlayEffect("RapleyEatCoin");

                SetRapleyScore(rapleyScore + 1);
            }
            else if (byWhom == GlobalConst.PacmomStr)
            {
                soundSystem.PlayEffect("PacmomEatCoin");

                SetPacmomScore(pacmomScore + 1);
            }

            if (!HasRemainingCoins())
            {
                GameOver();
            }
        }
        #endregion

        #region About Coin
        private void TakeHalfCoins(bool isRapleyTake)
        {
            if (isRapleyTake)
            {
                int score = pacmomScore / 2;
                SetRapleyScore(rapleyScore + score);
                SetPacmomScore(pacmomScore - score);
            }
            else
            {
                int score = rapleyScore / 2;
                SetPacmomScore(pacmomScore + score);
                SetRapleyScore(rapleyScore - score);
            }
        }

        private void ReleaseHalfCoins()
        {
            soundSystem.PlayEffect("DropCoin");

            int score = pacmomScore / 2;
            SetPacmomScore(pacmomScore - score);
            Debug.Log("팩맘 코인 " + score + "개 떨굼");

            foreach (Transform coin in coins)
            {
                if (score <= 0)
                    break;

                if (!coin.gameObject.activeSelf)
                {
                    coin.gameObject.SetActive(true);
                    score--;
                }
            }
        }

        private IEnumerator GetRemaningCoins()
        {
            foreach (Transform coin in coins)
            {
                if (coin.gameObject.activeSelf)
                {
                    soundSystem.PlayEffect("RapleyEatCoin");

                    SetRapleyScore(rapleyScore + 1);
                    coin.gameObject.SetActive(false);
                    yield return new WaitForSeconds(0.03f);
                }
            }
            ChooseAWinner();
        }

        private bool HasRemainingCoins()
        {
            foreach (Transform coin in coins)
            {
                if (coin.gameObject.activeSelf)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}