using UnityEngine;

public class BikeDust : MonoBehaviour
{
    [SerializeField] private ParticleSystem DustRear;
    [SerializeField] private ParticleSystem DustFront;
    private bool _checker;
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
        
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            Debug.Log("Aboba");
            Renderer renderer = hit.transform.GetComponent<Renderer>();
            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider is null || renderer is null) return;
            
            _checker = true;

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
            EmitParticle();
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

    private void EmitParticle()
    {
        if (_checker != true) return;
        DustRear.Emit(1);
        DustFront.Emit(1);
    }

}
