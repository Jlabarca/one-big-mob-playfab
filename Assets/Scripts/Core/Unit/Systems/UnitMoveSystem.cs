using Jlabarca.OneBigMob.Core.Unit.Components;
using Jlabarca.OneBigMob.Nav;
using Leopotam.Ecs;
using UnityEngine;

namespace Jlabarca.OneBigMob.Core.Unit.Systems
{
    internal sealed class UnitMoveSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Move> _filter = default;
        private readonly EcsFilter<Core.Unit.Components.Unit, NavigationComponent> _units = default;

        public void Run()
        {
            foreach (var commandIndex in _filter)
            {
                var moveCommand = _filter.Get1(commandIndex);
                var positions = GetFormationPositions(moveCommand.Position, _units.GetEntitiesCount());

                foreach (var unitIndex in _units)
                {
                    ref var unitNavigation = ref _units.Get2(unitIndex);
                    var navMeshAgent = unitNavigation.navMeshAgent;
                    navMeshAgent.isStopped = false;
                    navMeshAgent.radius = 0.1f;
                    navMeshAgent.stoppingDistance = 0.1f;
                    navMeshAgent.angularSpeed = 0;
                    navMeshAgent.SetDestination(positions[unitIndex]);
                }
            }
        }

        private static Vector3[] GetFormationPositions(Vector3 targetPosition, int size)
        {
            var tempPositions = new Vector3[size];
            GetFormationPositions(ref tempPositions, targetPosition, 0, size,  0.5f );

            return tempPositions;
        }

        private static void GetFormationPositions(ref Vector3[] tempPositions, Vector3 targetPosition, int offset, int size, float formationOffset)
        {
            float increment = 360f / size;
            for(int k = offset; k < offset+size; k++)
            {
                float angle = increment * k;
                var offsetVector3 = new Vector3(formationOffset * Mathf.Cos(angle * Mathf.Deg2Rad), 0f, formationOffset * Mathf.Sin(angle * Mathf.Deg2Rad));
                tempPositions[k] = targetPosition + offsetVector3;
            }
        }
    }
}
