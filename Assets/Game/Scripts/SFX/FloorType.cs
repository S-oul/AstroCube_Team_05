using UnityEngine;

public enum FloorSurface
{
    Carpet,
    Tiles,
    Dirt,
    Concrete
}

public class FloorType : MonoBehaviour
{
    [Tooltip("Select the surface type for FMOD audio.")]
    [SerializeField] private FloorSurface _surfaceType = FloorSurface.Concrete;
    
    public FloorSurface SurfaceType => _surfaceType;
    
    public string FloorTypeTag => _surfaceType.ToString();

    private void Reset()
    {
        _surfaceType = FloorSurface.Concrete;
    }
}
