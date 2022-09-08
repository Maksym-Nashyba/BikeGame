using UnityEngine;

public class BikeDust : MonoBehaviour
{
    [SerializeField] private ParticleSystem DustRear;
    [SerializeField] private ParticleSystem DustFront;
    private IBicycle _bicycle;
    private int _fixUpdateNumber;
    private float _currentSpeed;

    private void Awake()
    {
        _bicycle = GetComponent<IBicycle>();
    }

    private void FixedUpdate()
    {
        _currentSpeed = _bicycle.GetCurrentSpeed();

        GetTextureColor();
        CreateDust(_currentSpeed);
    }

    private void GetTextureColor()
    {
        Vector3 rayTransform = transform.position + Vector3.up * 1.2f;
        
        if (Physics.Raycast(rayTransform, Vector3.down, out RaycastHit hit))
        {
            Debug.Log("Aboba");
            Renderer renderer = hit.transform.GetComponent<Renderer>();
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider is null) return;
            
            Texture2D texture = renderer.material.mainTexture as Texture2D;
            Vector2 pixelOnTexture = hit.textureCoord;
            pixelOnTexture.x *= texture.width;
            pixelOnTexture.y *= texture.height;

            Color color = texture.GetPixel((int)pixelOnTexture.x, (int)pixelOnTexture.y);
            SetDustColor(color);
        }
    }

    private void CreateDust(float _currentSpeed)
    {
        if (_currentSpeed < 5) return;
        int _emitDelay = (int)Mathf.Abs(_currentSpeed - 13);
        _fixUpdateNumber++;

        if (_fixUpdateNumber == _emitDelay)
        {
            DustRear.Emit(1);
            DustFront.Emit(1);
        }
        if (_fixUpdateNumber > _emitDelay)
        {
            _fixUpdateNumber = 0;
        }
    }

    private void SetDustColor(Color color)
    {
        ParticleSystemRenderer rendererRear = DustRear.GetComponent<ParticleSystemRenderer>();
        ParticleSystemRenderer rendererFront = DustFront.GetComponent<ParticleSystemRenderer>();

        rendererRear.material.color = color;
        rendererFront.material.color = color;
    }
}
