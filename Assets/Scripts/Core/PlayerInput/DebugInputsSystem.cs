using Jlabarca.OneBigMob.Core.Unit.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Jlabarca.OneBigMob.Core.PlayerInput
{
    public sealed class DebugInputsSystem : IEcsRunSystem {
        private EcsWorld _world;

        public void Run() {

            if (
                Input.GetKey(KeyCode.LeftShift) &&
                Input.GetMouseButtonDown(1) &&
                Physics.Raycast(Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y)),
                    out var hit)
            )
            {
                for (int i = 0; i < 1; i++)
                {
                    var actorEntity = _world.NewEntity();
                    actorEntity.Get<Spawn>().Position = hit.point;
                }
            }
        }
    }
}
