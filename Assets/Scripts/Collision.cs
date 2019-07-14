using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    // Script to control Player collision behavior

    [Header("Layers")]
    public LayerMask ground_layer;

    [Space]
    public bool is_grounded;
    public bool touching_pink;

    [Space]
    [Header("Collision")]
    public float collision_radius = 0.25f;
    public Vector2 bottom_offset;
    //private Color collision_color = Color.red;

    private void Update()
    {
        is_grounded = Physics2D.OverlapCircle((Vector2)transform.position + bottom_offset, collision_radius, ground_layer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // List with gizmo positions
        var positions = new Vector2[] { bottom_offset };

        Gizmos.DrawWireSphere((Vector2)transform.position + bottom_offset, collision_radius);
    }
}
