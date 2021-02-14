using Jlabarca.OneBigMob.Core.Unit.Components;
using Jlabarca.OneBigMob.Tempo.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Jlabarca.OneBigMob.Tempo.Systems
{
    public class TempoHitSystem :IEcsRunSystem
    {
        private readonly EcsFilter<TempoHitEvent> _hitEventsFilter = default;
        private readonly EcsFilter<Unit> _unitsFilter = default;
        private readonly EcsWorld _world = default;

        public void Run()
        {
            foreach (var hitEventIndex in _hitEventsFilter)
            {
                var hitEvent = _hitEventsFilter.Get1(hitEventIndex);

                int hitCounter = 0;
                foreach (var unitIndex in _unitsFilter)
                {
                    var unit = _unitsFilter.Get1(unitIndex);

                    if (InsideSquare(hitEvent.Position, hitEvent.Width, hitEvent.Height,
                        unit.UnitView.transform.position))
                    {
                        Debug.Log("yea");

                        hitCounter++;
                        unit.Hp--;
                    }
                }

                if (hitCounter > 0)
                {
                    var actorEntity = _world.NewEntity();
                    actorEntity.Get<Spawn>().Position = hitEvent.Position;
                }
            }
        }

        private bool InsideSquare(Vector3 center, float width, float height, Vector3 innerPosition)
        {
            Debug.Log("InsideSquare");
            var halfWidth = width / 2;
            var halfHeight = height / 2;
            return innerPosition.x <= center.x + halfWidth
                   && innerPosition.x >= center.x - halfWidth
                   && innerPosition.z <= center.z + halfHeight
                   && innerPosition.z >= center.z - halfHeight;
        }
    }
}
