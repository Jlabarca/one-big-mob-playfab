using Jlabarca.OneBigMob.Core.Unit.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Jlabarca.OneBigMob.Nav
{
    public class DynamicNavSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Unit, NavigationComponent> _actors = default;

        public void Run()
        {
            //if(_actors.GetEntitiesCount() < 10) return;
            var sizeFactor = 0.7f - _actors.GetEntitiesCount()/1000f;
            foreach (var index in _actors)
            {
                var navigationComponent = _actors.Get2(index);
                var navMeshAgent = navigationComponent.navMeshAgent;
                if (!navMeshAgent.isStopped)
                {
                    var actorComponent = _actors.Get1(index);
                    if (Vector3.Distance(actorComponent.UnitView.transform.position, navMeshAgent.destination) < 1
                    && navMeshAgent.radius < sizeFactor
                    && navMeshAgent.stoppingDistance < 3)
                    {
                        navMeshAgent.radius +=  0.001f;
                        navMeshAgent.stoppingDistance += 0.07f;
                    }
                }
            }
        }
    }
}
