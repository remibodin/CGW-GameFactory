using UnityEngine;

namespace Cgw.Audio
{
    public static class SurfaceTypeExt
    {
        public static ESurfaceType GetSurfaceType(this GameObject p_object)
        {
            if (p_object.TryGetComponent<SurfaceType>(out var surfaceType))
            {
                return surfaceType.Type;
            }
            return ESurfaceType.Unknown;
        }

        public static ESurfaceType GetSurfaceType(this Transform p_transform)
        {
            return GetSurfaceType(p_transform.gameObject);
        }

        public static ESurfaceType GetSurfaceType(this Collider p_collider)
        {
            return GetSurfaceType(p_collider.gameObject);
        }

        public static ESurfaceType GetSurfaceType(this Collider2D p_collider)
        {
            return GetSurfaceType(p_collider.gameObject);
        }
    }
}