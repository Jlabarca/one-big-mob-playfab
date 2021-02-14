using Jlabarca.OneBigMob.Core.Pooling;
using Jlabarca.OneBigMob.Tempo.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Jlabarca.OneBigMob.Tempo.Systems
{
    public class TempoTilesMoveSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Tile> _filter = default;
        private readonly TilesPool _tilesPool = default;
        private readonly GameState _gameState = default;
        private readonly EcsWorld _world = default;

        public void Run()
        {
            foreach (var index in _filter)
            {
                var tile = _filter.Get1(index);

                var t = (_gameState.time - tile.ReleaseTime) / (tile.FinalTime - tile.ReleaseTime);
                if (t > 1) t = 1;

                tile.TileView.movingTile.position = Vector3.Lerp(tile.SpawnPosition, tile.TargetPosition, t);

                if (tile.TileView.movingTile.position == tile.TargetPosition)
                {
                    var entity = _filter.GetEntity(index);
                    _tilesPool.ReturnToPool(tile.TileView);
                    entity.Destroy();

                    var newEntity = _world.NewEntity();
                    newEntity.Replace(new TempoHitEvent
                    {
                        Position = tile.TargetPosition,
                        Width = 5,
                        Height = 5
                    });
                }
            }
        }
    }
}
