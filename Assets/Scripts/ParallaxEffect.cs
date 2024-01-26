using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Scriptul ParallaxEffect controlează un efect de parallax între obiectul la care este atașat și camera de joc.
 * Parallaxul este un efect vizual în care obiectele mai îndepărtate par a se mișca mai încet decât cele mai apropiate,
 * oferind astfel percepția de adâncime și mișcare în fundal.
 * 
 * Acest script necesită o referință la camera de joc (variabila 'cam') și un obiect de urmărire (variabila 'followTarget').
 * 
 * Proprietăți și variabile cheie:
 * - startingPosition: Poziția inițială a obiectului la începutul efectului de parallax.
 * - startingZ: Poziția z inițială a obiectului la începutul efectului de parallax.
 * - camMoveSinceStart: Calculul mișcării camerei de la începutul efectului de parallax.
 * - zDistanceFromTarget: Calculul distanței de adâncime dintre obiect și țintă.
 * - clippingPlane: Calculul distanței până la planul de tăiere, luând în considerare dacă obiectul este în fața sau în spatele țintei.
 * - parallaxFactor: Calculul factorului de parallax în funcție de distanța de adâncime dintre obiect și țintă.
 * 
 * Funcții:
 * - Start(): Inițializează poziția și adâncimea inițială ale obiectului.
 * - Update(): Actualizează poziția obiectului în funcție de mișcarea camerei și factorul de parallax.
 */

public class ParallaxEffect : MonoBehaviour
{
    // Referință la camera în care are loc efectul de parallax
    public Camera cam;

    // Referință la obiectul care urmărește pentru a calcula efectul de parallax
    public Transform followTarget;

    // Poziția inițială a obiectului
    Vector2 startingPosition;

    // Poziția z inițială a obiectului
    float startingZ;

    // Calculul mișcării camerei de la începutul efectului de parallax
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

    // Calculul distanței de adâncime dintre obiect și țintă
    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    // Calculul distanței până la planul de tăiere în funcție de faptul
    // dacă obiectul se află în fața sau în spatele țintei.
    // Ține cont de adâncimea camerei (cam.transform.position.z),
    // iar dacă obiectul se află în spatele țintei, folosește planul de tăiere îndepărtat;
    // dacă este în față, folosește planul de tăiere apropiat.
    // Planurile de tăiere departe și aproape definesc intervalul de distanțe față de camera unde sunt redate obiectele.
    // Acest calcul este crucial pentru a determina cât de mult din obiect este vizibil în trunchiul camerei.
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    // Calculul factorului de parallax în funcție de distanța de adâncime dintre obiect și țintă
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;

    // Start is called before the first frame update
    void Start()
    {
        // Salvarea poziției și adâncimii inițiale ale obiectului
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculul noii poziții pe baza mișcării camerei și a factorului de parallax
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;

        // Actualizarea poziției obiectului
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
