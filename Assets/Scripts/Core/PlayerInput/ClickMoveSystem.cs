using Jlabarca.OneBigMob.Core.Unit.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Jlabarca.OneBigMob.Core.PlayerInput
{
    public sealed class ClickMoveSystem : IEcsRunSystem {
        private readonly EcsWorld ecsWorld = null;
        private GameState gameState;

        public void Run() {
            var mousePosition = Input.mousePosition;

            if (
                !Input.GetMouseButtonDown(0) ||
                !Physics.Raycast(Camera.main.ScreenPointToRay(new Vector2(mousePosition.x, mousePosition.y)), out var hit)
            ) return;

            var entity = ecsWorld.NewEntity();
            entity.Replace(new Move
            {
                Position = hit.point
            });
        }
    }
}
