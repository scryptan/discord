using System;
using UnityEngine;
using UnityEngine.UI;

namespace ThinIce
{
    public class GamePlaying : MonoBehaviour
    {
        private float _stamina = 100f;
        private float _workload = 0f;
        private uint _round = 0;

        private float _timer = 0f;
        private float _roundTimer = 0f;
        private float _timerRecovery = 0f;

        private float _roundMaxTimer = 2f;
        private bool _girlSearchingAndThrow = false;

        [Header("Game Objects")]
        public PlayerController guy;
        public Girl girl;
        public HeapController heap;
        public GameObject[] items;
        public Image staminaBar;
        public Text staminaText;
        public Image weightBar;
        public Text weightText;

        [Header("Start Game Config")]
        public float startStamina = 100f;
        public Vector3 startGuyPosition = new Vector3(0, -4, 0);
        public uint startRound = 0;
        public float timerNextRound = 30f;
        public float forceToPush = 3f;
        public float horizontalCoef = 1.5f;
        public float maxWeightGame = 100f;
        public float startWorkLoad = 0f;
        public float maxTimerRecovery = 1f;
        public float weightGrabCoef = 2f;

        [Header("Rounds Config")] 
        public float[] rounds;

        public float caughtCoefItemScore = 2f;

        // Start is called before the first frame update
        void Awake()
        {
        }

        public void Initialize()
        {
            _stamina = startStamina;
            _round = startRound;
            _timer = 0f;
            _roundTimer = 0f;
            _girlSearchingAndThrow = false;
            _workload = startWorkLoad;

            if (rounds.Length > 0)
                _roundMaxTimer = rounds[0];
        
            guy.PlayerSpawn(startGuyPosition);
            girl.Initialize();
            heap.Initialize();
        }

        // Update is called once per frame
        private void Update()
        {
            _roundTimer += Time.deltaTime;
            _timerRecovery += Time.deltaTime;

            if (!_girlSearchingAndThrow)
            {
                _timer += Time.deltaTime;
            
                if (_timer >= _roundMaxTimer)
                {
                    _timer = 0f;
                    girl.PlaySearch();
                    _girlSearchingAndThrow = true;
                }
            }

            if (_timerRecovery >= maxTimerRecovery)
            {
                _timerRecovery = 0f;
                RecoveryStamina();
            }

            if (_roundTimer >= timerNextRound)
            {
                _roundTimer = 0f;
                NextRound();
            }
        
            KeyboardUpdate();
        }

        private void KeyboardUpdate()
        {
            // Horizontal Movement
            var moveLeft = Input.GetKey(KeyCode.A);
            var moveRight = Input.GetKey(KeyCode.D);

            if (moveLeft || moveRight)
            {
                var xPosition = -Convert.ToInt16(moveLeft) + Convert.ToInt16(moveRight);
                const float yPosition = 0f;
            
                var directional = new Vector3(xPosition, yPosition, 0f);
            
                guy.PlayerMove(directional);
            }
            else
            {
                guy.PlayerIdle();
            }
        }

        public void SpawnItem()
        {
            var itemId = UnityEngine.Random.Range(0, items.Length);
            var item = Instantiate<GameObject>(items[itemId], transform);
            item.transform.position = girl.transform.position;
        
            var x = UnityEngine.Random.Range(-forceToPush, forceToPush);
            var y = UnityEngine.Random.Range(-forceToPush / horizontalCoef, forceToPush / horizontalCoef);
            item.GetComponent<Item>().AddForce(new Vector2(x, y));
        
            item.GetComponent<Item>().SetGamePlaying(this);

            _girlSearchingAndThrow = false;
        }
    
        private void NextRound()
        {
            if (_round >= rounds.Length - 1)
                return;

            _roundMaxTimer = rounds[++_round];
            Debug.Log($"Next round [${_round}]!");
        }

        public void AddWeight(float weight)
        {
            _workload += weight;
        
            weightBar.fillAmount = _workload;
            weightText.text = _workload.ToString();
        
            if (_workload >= maxWeightGame)
                GameController.Instance.GameLose();
        }

        public void SubtractWeight(float weight)
        {
            _workload -= weight;
        
            weightBar.fillAmount = _workload;
            weightText.text = _workload.ToString();

            if (_workload < 0)
            {
                _workload = 0f;
            }
        }
    
        public void AddStamina(float stamina)
        {
            _stamina += stamina;
        
            staminaBar.fillAmount = _stamina;
            staminaText.text = _stamina.ToString();

            if (_stamina >= startStamina)
                _stamina = startStamina;
        }
    
        public void SubtractStamina(float stamina)
        {
            _stamina -= stamina;
        
            staminaBar.fillAmount = _stamina;
            staminaText.text = _stamina.ToString();

            if (_stamina < 0)
                GameController.Instance.GameLose();
        }

        private void RecoveryStamina()
        {
            AddStamina(1f);
        }

        public void AddHeap(float score)
        {
            heap.AddHeap(score);
        }

    }
}
