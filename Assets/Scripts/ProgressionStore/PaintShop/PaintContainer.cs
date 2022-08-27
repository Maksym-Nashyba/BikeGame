using IGUIDResources;
using UnityEngine;

namespace ProgressionStore.PaintShop
{
    public class PaintContainer : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _paintRenderer;
        public Skin CurrentSkin { get; private set; }

        public void ShowEmpty()
        {
            _paintRenderer.enabled = false;
            CurrentSkin = null;
        }

        public void ShowWithSkin(Skin skin)
        {
            _paintRenderer.enabled = true;
            _paintRenderer.material = skin.Material;
            CurrentSkin = skin;
        }
    }
}