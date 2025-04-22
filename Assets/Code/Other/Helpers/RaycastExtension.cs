using EscapeRooms.Components;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Providers;

namespace EscapeRooms.Helpers
{
    public static class RaycastExtension
    {
        public static bool GetHitEntity(ref OneHitRaycastComponent hitComponent, out Entity entity)
        {
            if (hitComponent.IsRayHit &&
                EntityProvider.map.TryGetValue(hitComponent.Hit.collider.gameObject.GetInstanceID(), out var item))
            {
                entity = item.entity;
                return true;
            }

            entity = default;
            return false;
        }
    }
}