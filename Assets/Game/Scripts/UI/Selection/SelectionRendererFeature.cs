using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SelectionRendererFeature : ScriptableRendererFeature
{
    class SelectionRenderPass : ScriptableRenderPass
    {
        private Material _selectionMaterial;
        private FilteringSettings _filteringSettings;


        public SelectionRenderPass(Material material, uint renderingLayerMask)
        {
            _selectionMaterial = material;
            _filteringSettings = new FilteringSettings(RenderQueueRange.opaque)
            {
                renderingLayerMask = renderingLayerMask
            };
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Outline Pass");

            // Drawing settings
            DrawingSettings drawingSettings = CreateDrawingSettings(new ShaderTagId("UniversalForward"), ref renderingData, SortingCriteria.CommonOpaque);
            drawingSettings.overrideMaterial = _selectionMaterial;

            // Draw only objects matching the rendering layer mask
            context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref _filteringSettings);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    public SelectionRendererFeatureSettings Settings = new SelectionRendererFeatureSettings();
    private SelectionRenderPass _selectionRenderPass;

    public override void Create()
    {
        _selectionRenderPass = new SelectionRenderPass(Settings.SelectionMaterial, Settings.SelectionRenderingLayerMask);
        _selectionRenderPass.renderPassEvent = Settings.passEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_selectionRenderPass);
    }
}

[System.Serializable]
public class SelectionRendererFeatureSettings
{
    public Material SelectionMaterial;

    [Tooltip("Select the rendering layers that should have an outline.")]
    public uint SelectionRenderingLayerMask;
    public RenderPassEvent passEvent;

}