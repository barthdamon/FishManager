using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// Detach and destroy a child from a parent by index.
    /// </summary>
    /// <param name="transform">A parent transform.</param>
    /// <param name="index">The child index for the child to be detached and destroyed.</param>
    /// <param name="destroy_inactive_children">If true this method will destroy an inactive child.</param>
    /// <returns>True if the child at the given index was found and destroyed, false otherwise.</returns>
    public static bool DestroyChild(this Transform transform, int index, bool destroy_inactive_children = false)
    {
        if (index >= transform.childCount)
        {
            return false;
        }

        var child_object = transform.GetChild(index)?.gameObject;
        if (child_object != null && (destroy_inactive_children || child_object.activeInHierarchy))
        {
            GameObject.Destroy(child_object);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Detach and destroy all children from a parent.
    /// </summary>
    /// <param name="transform">A parent transform.</param>
    /// <param name="destroy_inactive_children">If true this method will destroy an inactive child.</param>
    /// <returns>The number of children destroyed by this method.</returns>
    public static int DestroyChildren(this Transform transform, bool destroy_inactive_children = false)
    {
        int destroyed_children = 0;
        for (int child_index = transform.childCount - 1; child_index >= 0; --child_index)
        {
            if (transform.DestroyChild(child_index, destroy_inactive_children))
            {
                ++destroyed_children;
            }
        }

        return destroyed_children;
    }
}
