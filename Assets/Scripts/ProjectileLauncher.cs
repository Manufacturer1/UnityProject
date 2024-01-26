using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Acest script ProjectileLauncher se ocupă de lansarea proiectilelor.
 */

public class ProjectileLauncher : MonoBehaviour
{
    // Referință la proiectilul ce urmează a fi lansat
    public GameObject projectilePrefab;

    // Punctul de lansare al proiectilului
    public Transform launchPoint;

    // Funcția pentru a lansa un proiectil
    public void FireProjectile()
    {
        // Instantierea proiectilului la poziția punctului de lansare și cu aceeași rotație
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);

        // Salvarea scalei originale a proiectilului
        Vector3 origScale = projectile.transform.localScale;

        // Redimensionarea proiectilului în funcție de scala obiectului care lansează proiectilul
        projectile.transform.localScale = new Vector3(
            origScale.x * transform.localScale.x > 0 ? 1 : -1,
            origScale.y,
            origScale.z
        );
    }
}
