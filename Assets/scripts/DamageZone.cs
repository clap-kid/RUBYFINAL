using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damage;
    void OnTriggerStay2D(Collider2D other)
    {
        rubyController controller = other.GetComponent<rubyController >();

        if (controller != null)
        {
            controller.ChangeHealth(-damage);
        }
    }

}
