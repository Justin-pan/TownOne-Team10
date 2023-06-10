using UnityEngine;

public struct Hit
{
    public int Damage { get; set; }
    public Vector2 Knockback { get; set; }

    public Hit(int damage, Vector2 knockback)
    {
        Damage = damage;
        Knockback = knockback;
    }
}
