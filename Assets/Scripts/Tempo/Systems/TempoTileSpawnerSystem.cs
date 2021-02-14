using Jlabarca.OneBigMob.Core.Pooling;
using Jlabarca.OneBigMob.MonoBehaviours;
using Jlabarca.OneBigMob.Tempo.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Jlabarca.OneBigMob.Tempo.Systems
{
    public class TempoTileSpawnerSystem : IEcsRunSystem
    {
        private readonly EcsFilter<TempoEvent> _filter = default;
        private readonly EcsWorld _world = default;
        private readonly TilesPool _tilesPool = default;

        private static readonly int[] SpawnPositionsX = { -5, 0, 5 };
        private static readonly int SpawnPositionZ = 100;
        private static readonly int[] TargetPositionsZ = { -5, 0, 5 };
        private static readonly float SpawnPositionY = .1f;
        private static readonly Vector3 InitialPosition = new Vector3(0, SpawnPositionY, 0);

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var tempoEvent = ref _filter.Get1(index);
                //Debug.Log($"{Time.time} - {tempoEvent.soundEvent}");

                var randomX = Random.Range(0, SpawnPositionsX.Length);
                var randomZ = Random.Range(0, TargetPositionsZ.Length);
                var tileView = _tilesPool.Get();
                tileView.transform.position = InitialPosition;

                var spawnPosition = new Vector3(SpawnPositionsX[randomX], SpawnPositionY, SpawnPositionZ);
                var targetPosition = new Vector3(SpawnPositionsX[randomX], SpawnPositionY, TargetPositionsZ[randomZ]);

                tileView.movingTile.localPosition = spawnPosition;
                tileView.targetTile.localPosition = targetPosition;

                var tileEntity = _world.NewEntity();
                tileEntity.Replace(new Tile
                    {
                        SpawnPosition = spawnPosition,
                        TargetPosition = targetPosition,
                        ReleaseTime = tempoEvent.ReleaseTime,
                        FinalTime = tempoEvent.SoundEvent.time,
                        TileView = tileView.GetComponent<TileView>(),
                    });

                tileView.Entity = tileEntity;
                tileView.gameObject.SetActive(true);
            }
        }
    }
}
