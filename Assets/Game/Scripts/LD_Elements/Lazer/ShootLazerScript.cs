using UnityEngine;

[ExecuteAlways]
public class ShootLazerScript : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private float lazerMaxDistance = 30f;

    private LazerBeam _beam;
    private bool _isLazerActive = false;

    private void OnEnable()
    {
        EventManager.OnButtonPressed += ActivateLazer;
        EventManager.OnButtonReleased += DeactivateLazer;
        _isLazerActive = Application.isPlaying ? false : true;
    }

    private void Start()
    {
        ActivateLazer();
    }

    private void OnDisable()
    {
        EventManager.OnButtonPressed -= ActivateLazer;
        EventManager.OnButtonReleased -= DeactivateLazer;
    }

    private void Update()
    {
        if (_isLazerActive)
        {
            DestroyImmediate(GameObject.Find("Lazer Beam")); 
            _beam = new LazerBeam(transform.position, transform.right, _material, lazerMaxDistance);
        }
    }

    private void ActivateLazer()
    {
        _isLazerActive = true;
    }

    private void DeactivateLazer()
    {
        _isLazerActive = false;
        DestroyImmediate(GameObject.Find("Lazer Beam"));
    }
}
