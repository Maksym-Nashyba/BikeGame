using System.Threading.Tasks;
using Menu;
using UnityEngine;
using UnityEngine.Rendering;

namespace SetUp
{
    public class GraphicsSetUp : MonoBehaviour
    {
        [SerializeField] private RenderPipelineAsset _lowSettings;
        [SerializeField] private RenderPipelineAsset _highSettings;
    
        private void Awake()
        {
            SetUpOperation setUpOperation = new SetUpOperation(SetUpQuality, "Graphics Loaded", false);
            FindObjectOfType<GameSetUp>().RegisterSetUpTask(setUpOperation);
        }

        private Task SetUpQuality()
        {
            RenderPipelineAsset selectedSettings = UserSettings.GetGraphicsTier() == UserSettings.GraphicsTier.High ? _highSettings : _lowSettings;
            QualitySettings.renderPipeline = selectedSettings;
            return Task.CompletedTask;
        }
    }
}