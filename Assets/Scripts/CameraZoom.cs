using UnityEngine; 
using Cinemachine; 

public class CameraZoomController : MonoBehaviour // Declaratia clasei CameraZoomController care moștenește MonoBehaviour
{
    public float zoomSpeed = 10.0f; // Viteza de zoom
    public float minZoom = 5.0f; // Zoom-ul minim
    public float maxZoom = 20.0f; // Zoom-ul maxim

    private CinemachineVirtualCamera virtualCamera; // Referinta la camera virtuala Cinemachine

    void Start() // Metoda care se apeleaza o singura data la inceput
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>(); // Initializeaza camera virtuala
    }

    void Update() // Metoda care se apeleaza in fiecare cadru
    {
        // Apelul metodei Zoom in functie de butoanele apasate
        Zoom(Input.GetKey(KeyCode.Y) ? 1 : Input.GetKey(KeyCode.U) ? -1 : 0); 
    }

    void Zoom(int direction) // Metoda pentru zoom
    {
        // Verifica daca camera este de tip ortografic
        if (virtualCamera.m_Lens.Orthographic) 
        {
            // Zoom pentru camera ortografica
            virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(virtualCamera.m_Lens.OrthographicSize + direction * zoomSpeed * Time.deltaTime, minZoom, maxZoom);
        }
        else
        {
            // Zoom pentru camera perspectiva
            virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(virtualCamera.m_Lens.FieldOfView + direction * zoomSpeed * Time.deltaTime, minZoom, maxZoom);
        }
    }
}
