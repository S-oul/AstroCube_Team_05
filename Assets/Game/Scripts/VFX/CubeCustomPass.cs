using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering.RendererUtils;

public class CubeCustomPass : CustomPass
{
    [SerializeField] private LayerMask _handsLayer;
    
    protected override void Execute(CustomPassContext ctx)
    {
        CoreUtils.ClearRenderTarget(ctx.cmd, ClearFlag.Depth, Color.clear);

        var shaderTagId = new ShaderTagId("Forward");
        var rendererListDesc = new RendererListDesc(shaderTagId, ctx.cullingResults, ctx.hdCamera.camera)
        {
            layerMask = _handsLayer,
            renderQueueRange = RenderQueueRange.all,
            sortingCriteria = SortingCriteria.CommonOpaque,
            stateBlock = new RenderStateBlock(RenderStateMask.Depth)
            {
                depthState = new DepthState(true, CompareFunction.LessEqual),
                rasterState = new RasterState(CullMode.Back),
            }
        };
        var context = ctx.renderContext;
        var rendererList = context.CreateRendererList(rendererListDesc);
        ctx.cmd.DrawRendererList(rendererList);
    }
}
