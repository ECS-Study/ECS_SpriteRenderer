using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class SpriteRenderSystem : SystemBase
{

    private readonly int HASH_MAIN_TEX = Shader.PropertyToID("_MainTex");
    private readonly int HASH_COLOR = Shader.PropertyToID("_Color");
    private readonly int HASH_FLIP = Shader.PropertyToID("_Flip");

    protected override void OnUpdate()
    {

        foreach(var (renderer, transform) 
            in SystemAPI.Query<RefRO<SpriteRendererECS>, RefRO<LocalTransform>>())
        {

            var texture = SpriteRendererManager.Instance.GetTextureByHash(renderer.ValueRO.sprite.spriteHash);
            var material = SpriteRendererManager.Instance.GetMaterialByHash(renderer.ValueRO.material.materialHash);

            RenderParams renderParams = new(material);
            renderParams.matProps = new MaterialPropertyBlock();

            renderParams.matProps.SetTexture(HASH_MAIN_TEX, texture);
            renderParams.matProps.SetColor(HASH_COLOR, CastFloat4ToColor(renderer.ValueRO.color));
            renderParams.matProps.SetVector(HASH_FLIP, new float4
                (

                    renderer.ValueRO.flipX ? -1 : 1,
                    renderer.ValueRO.flipY ? -1 : 1,
                    1,
                    1

                ));

            Matrix4x4 matrix = Matrix4x4.TRS(
                transform.ValueRO.Position - new float3(renderer.ValueRO.sprite.pivot, 0) * transform.ValueRO.Scale,
                transform.ValueRO.Rotation,
                Vector3.one * transform.ValueRO.Scale * new float3(renderer.ValueRO.sprite.scale, 0));

            Graphics.RenderMesh(renderParams, SpriteRendererManager.Instance.GetQurdMesh(), 0, matrix);


        }

    }

    private Color CastFloat4ToColor(in float4 value)
    {

        return new Color(value.x, value.y, value.z, value.w);

    }

}
