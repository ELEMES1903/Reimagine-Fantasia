using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public HealthBar healthBar;

    void Update()
    {
        // Check for spacebar press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Call TakeDamage method of HealthBar script
            healthBar.TakeDamage(10f, false); 
        }

        // Check for spacebar press
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Call TakeDamage method of HealthBar script
            healthBar.TakeDamage(10f, true); 
        }
    }
}
