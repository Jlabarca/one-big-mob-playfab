using Jlabarca.OneBigMob.Core.PlayerInput;
using Jlabarca.OneBigMob.Core.Pooling;
using Jlabarca.OneBigMob.Core.Unit.Components;
using Jlabarca.OneBigMob.Core.Unit.Systems;
using Jlabarca.OneBigMob.Nav;
using Jlabarca.OneBigMob.ScriptableObjects;
using Jlabarca.OneBigMob.Tempo;
using Jlabarca.OneBigMob.Tempo.Components;
using Jlabarca.OneBigMob.Tempo.Systems;
using Leopotam.Ecs;
using UnityEngine;

namespace Jlabarca.OneBigMob
{
    public class GameStartup : MonoBehaviour
    {
        private EcsWorld _ecsWorld;
        private EcsSystems _systems;
        //private EcsSystems _fixedSystems;
        private GameState _gameState;

        private UnitsPool _unitsPool;
        private TilesPool _tilesPool;

        private AudioSource _audioSource;

        public Configuration configuration;

        private void OnEnable()
        {
            _ecsWorld = new EcsWorld();
            _systems = new EcsSystems(_ecsWorld);
           // _fixedSystems = new EcsSystems(_ecsWorld);
            _gameState = new GameState();

            _unitsPool = gameObject.GetComponentInChildren<UnitsPool>();
            _unitsPool.Prewarm(100, configuration.unitView);

            _tilesPool = gameObject.GetComponentInChildren<TilesPool>();
            _tilesPool.Prewarm(20, configuration.tileView);

            _audioSource = GetComponent<AudioSource>();

#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_ecsWorld);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
#endif

            _systems
                .Add(new UnitsInitSystem())
                .Add(new ClickMoveSystem())
                .Add(new DebugInputsSystem())
                .Add(new DynamicNavSystem())

                .Add(new TempoEmitSystem())
                .Add(new TempoTileSpawnerSystem())
                .Add(new TempoTilesMoveSystem())
                .Add(new TempoHitSystem())

                .Add(new UnitSpawnSystem())
                .Add(new UnitMoveSystem())

                .Inject(_gameState)
                .Inject(configuration)
                .Inject(_unitsPool)
                .Inject(_tilesPool)
                .Inject(_audioSource)

                .OneFrame<TempoEvent>()
                .OneFrame<TempoHitEvent>()
                .OneFrame<Spawn>()
                .OneFrame<Move>()

                .Init();
        }

        private void Update() {
            _systems.Run();
        }

        private void FixedUpdate()
        {
            //_fixedSystems.Run();
        }

        private void OnDisable() {
            _systems.Destroy();
            _systems = null;

            _ecsWorld.Destroy();
            _ecsWorld = null;
        }
    }
}
