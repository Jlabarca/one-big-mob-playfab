using Jlabarca.OneBigMob.MonoBehaviours;
using Jlabarca.OneBigMob.Nav;
using Leopotam.Ecs;

namespace Jlabarca.OneBigMob.Core.Pooling
{
    public class UnitsPool : GenericObjectPool<UnitView>
    {
        public override void ReturnToPool(UnitView objectToReturn)
        {
            var entity = objectToReturn.Entity;
            entity.Del<Core.Unit.Components.Unit>();
            entity.Del<NavigationComponent>();
            base.ReturnToPool(objectToReturn);
        }
    }
}
