using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ThinIce.TowerDefence.Input;
using ThinIce.TowerDefence.Targets;
using UnityEngine;

namespace ThinIce.TowerDefence
{
    public class TowerDefenceController : MonoBehaviour
    {
        [SerializeField] private GameState currentState = GameState.Pause;
        [SerializeField] private int targetCountOnCurrentStageDefault = 5;
        [SerializeField] private int currentWave;
        [SerializeField] private int allWaves = 3;
        [SerializeField] private int chillSecondsDefault = 15;
        [SerializeField] private List<Transform> wayPoints;

        private TargetSpawner _targetSpawner;
        private TargetSpawnPoint _targetSpawnPoint;
        private List<Target> _waveTargets = new List<Target>();
        private List<InputControllerBase> _controllers = new List<InputControllerBase>();
        private Coroutine _chillTimer;
        private int _chillSeconds;        
        private int _targetCountOnCurrentStage;


        private void Awake()
        {
            _targetSpawner = FindObjectOfType<TargetSpawner>();
            _targetSpawnPoint = FindObjectOfType<TargetSpawnPoint>();
            _controllers = FindObjectsOfType<InputControllerBase>().ToList();
        }

        private void Update()
        {
            _controllers.ForEach(x => x.enabled = true);
            switch (currentState)
            {
                case GameState.Pause:
                    _controllers.ForEach(x => x.enabled = false);
                    break;
                case GameState.Attack:
                    if (_targetCountOnCurrentStage <= 0 && _waveTargets.All(x => x.Health <= 0))
                    {
                        _waveTargets.Clear();
                        currentState = GameState.Chill;
                        if (currentWave >= allWaves)
                            currentState = GameState.Pause;
                    }

                    break;
                case GameState.Chill:
                    _chillTimer ??= StartCoroutine(StartChillTimer());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnEnable()
        {
            _targetSpawnPoint.OnTargetExit += SpawnEnemy;
        }

        private void SpawnEnemy()
        {
            if (_targetCountOnCurrentStage <= 0 || currentState != GameState.Attack) return;

            _targetCountOnCurrentStage--;
            var target = _targetSpawner.Spawn();
            target.Init(wayPoints);
            _waveTargets.Add(target);
        }
        
        private void SpawnEnemyImmediately()
        {
            _targetCountOnCurrentStage--;
            var target = _targetSpawner.Spawn();
            target.Init(wayPoints);
            _waveTargets.Add(target);
        }

        private IEnumerator StartChillTimer()
        {
            _chillSeconds = chillSecondsDefault;
            while (_chillSeconds >= 0)
            {
                Debug.Log(_chillSeconds);
                _chillSeconds--;
                yield return new WaitForSecondsRealtime(1);
            }

            _chillTimer = null;
            SetAttackState();
        }

        private void SetAttackState()
        {
            SpawnEnemyImmediately();
            _targetCountOnCurrentStage = targetCountOnCurrentStageDefault;
            currentState = GameState.Attack;
            currentWave++;
        }

        private void OnDisable()
        {
            _targetSpawnPoint.OnTargetExit -= SpawnEnemy;
        }
    }
}