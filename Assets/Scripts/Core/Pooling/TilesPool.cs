using Jlabarca.OneBigMob.MonoBehaviours;
using Leopotam.Ecs;

namespace Jlabarca.OneBigMob.Core.Pooling
{
    public class TilesPool : GenericObjectPool<TileView>
    {
        public override void ReturnToPool(TileView objectToReturn)
        {
            var entity = objectToReturn.Entity;
            entity.Del<Core.Unit.Components.Unit>();
            base.ReturnToPool(objectToReturn);
        }
    }
}
