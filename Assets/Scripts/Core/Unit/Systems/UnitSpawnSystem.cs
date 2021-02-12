using Jlabarca.OneBigMob.Core.Pooling;
using Jlabarca.OneBigMob.Core.Unit.Components;
using Jlabarca.OneBigMob.MonoBehaviours;
using Jlabarca.OneBigMob.Nav;
using Leopotam.Ecs;
using UnityEngine.AI;

namespace Jlabarca.OneBigMob.Core.Unit.Systems
{
    public class UnitSpawnSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = default;
        private readonly UnitsPool _unitsPool = default;
        private readonly EcsFilter<Spawn> _filter = default;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var spawn = ref _filter.Get1(index);
                var actorView = _unitsPool.Get();
                actorView.transform.position = spawn.Position;

                var actorEntity = _world.NewEntity();
                actorEntity
                    .Replace(new Core.Unit.Components.Unit
                    {
                        Hp = 100,
                        UnitView = actorView.GetComponent<UnitView>(),
                    });

                if(actorView.TryGetComponent<NavMeshAgent>(out var navMeshAgent))
                    actorEntity.Replace(new NavigationComponent {
                        navMeshAgent = navMeshAgent
                    });

                actorView.Entity = actorEntity;
                actorView.gameObject.SetActive(true);
            }
        }
    }
}
