using Jlabarca.OneBigMob.MonoBehaviours;
using Jlabarca.OneBigMob.Nav;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;

namespace Jlabarca.OneBigMob.Core.Unit.Systems
{
    /**
     * Find and add existing GO units in the scene
     */
    public class UnitsInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = default;

        public void Init()
        {
            var playerObjects = GameObject.FindGameObjectsWithTag("Locals");
            foreach (var actor in playerObjects) {
                var actorEntity = _world.NewEntity();
                actorEntity
                    .Replace(new Core.Unit.Components.Unit
                    {
                        Hp = 100,
                        UnitView = actor.GetComponent<UnitView>()
                    });

                if(actor.TryGetComponent<NavMeshAgent>(out var navMeshAgent))
                    actorEntity.Replace(new NavigationComponent {
                        navMeshAgent = navMeshAgent
                    });
            }
        }
    }
}
