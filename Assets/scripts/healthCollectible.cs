using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthCollectible : MonoBehaviour
{
    public AudioClip collectedClip;
    public ParticleSystem healthCollect;
    void OnTriggerEnter2D(Collider2D other)
  {
    rubyController controller = other.GetComponent<rubyController>();

    if (controller != null)
    {
        if(controller.health < controller.maxHealth)
          {
            Instantiate(healthCollect, controller.transform.position, Quaternion.identity);
	          controller.ChangeHealth(1);
	          Destroy(gameObject);
            controller.PlaySound(collectedClip);

            if (controller.oiled == 1)
            {
              controller.speed += 1;
              controller.oiled = 0;
              controller.oilUI.SetActive(false);
            }
          }
    }
}
}