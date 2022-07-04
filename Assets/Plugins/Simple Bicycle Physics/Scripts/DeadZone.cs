
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviour
{
    [SerializeField] private float _deadZone;
    private GameObject _playerTransform;

    private void Awake()
    {
        _playerTransform = FindObjectOfType<GameObject>();
    }
    
    private void Update()
    {
        if (_playerTransform.transform.position.y < _deadZone)
        {
            SceneManager.LoadScene(0);
        }
    }
}

