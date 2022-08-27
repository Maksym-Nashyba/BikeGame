using IGUIDResources;
using UnityEngine;

namespace ProgressionStore.PaintShop
{
    public class PaintContainer : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _paintRenderer;

        public void ShowEmpty()
        {
            _paintRenderer.enabled = false;
        }

        public void ShowWithSkin(Skin skin)
        {
            _paintRenderer.enabled = true;
            _paintRenderer.material = skin.Material;
        }
    }
}