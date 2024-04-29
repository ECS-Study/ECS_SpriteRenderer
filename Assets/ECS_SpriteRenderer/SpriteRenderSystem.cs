using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class SpriteRenderSystem : SystemBase
{

    private readonly int HASH_MAIN_TEX = Shader.PropertyToID("_MainTex");
    private readonly int HASH_COLOR = Shader.PropertyToID("_Color");
    private readonly int HASH_MAIN_TEX_ST = Shader.PropertyToID("_MainTex_ST");

    protected override void OnUpdate()
    {

        Dictionary<SpriteRendererECS, List<LocalTransform>> container 
            = new();

        foreach(var (renderer, transform) 
            in SystemAPI.Query<RefRO<SpriteRendererECS>, RefRO<LocalTransform>>())
        {

            if (container.ContainsKey(renderer.ValueRO))
            {

                container[renderer.ValueRO].Add(transform.ValueRO);

            }
            else
            {

                container.Add(renderer.ValueRO, new() { transform.ValueRO });

            }

            //var texture = SpriteRendererManager.Instance.GetTextureByHash(renderer.ValueRO.sprite.spriteHash);
            //var material = SpriteRendererManager.Instance.GetMaterialByHash(renderer.ValueRO.material.materialHash);
            //
            //RenderParams renderParams = new(material);
            //renderParams.matProps = new MaterialPropertyBlock();
            //
            //renderParams.matProps.SetTexture(HASH_MAIN_TEX, texture);
            //renderParams.matProps.SetColor(HASH_COLOR, CastFloat4ToColor(renderer.ValueRO.color));
            //
            //var flipVector = new Vector3(renderer.ValueRO.flipX ? -1 : 1, renderer.ValueRO.flipY ? -1 : 1);
            //
            //Matrix4x4 matrix = Matrix4x4.TRS(
            //    transform.ValueRO.Position - new float3(renderer.ValueRO.sprite.pivot, 0) * transform.ValueRO.Scale,
            //    transform.ValueRO.Rotation,
            //    flipVector * transform.ValueRO.Scale * new float3(renderer.ValueRO.sprite.scale, 0) );
            //
            //Graphics.RenderMesh(renderParams, SpriteRendererManager.Instance.GetQurdMesh(), 0, matrix);

        }

        Debug.Log(container.Count);

        foreach(var item in container)
        {


            var texture = SpriteRendererManager.Instance.GetTextureByHash(item.Key.sprite.spriteHash);
            var material = SpriteRendererManager.Instance.GetMaterialByHash(item.Key.material.materialHash);

            List<Matrix4x4> mat = new();

            RenderParams renderParams = new(material);
            //renderParams.matProps = new MaterialPropertyBlock();
            //renderParams.matProps.SetTexture(HASH_MAIN_TEX, texture);
            //renderParams.matProps.SetColor(HASH_COLOR, CastFloat4ToColor(item.Key.color));

            foreach (var transform in item.Value)
            {

                var flipVector = new Vector3(item.Key.flipX ? -1 : 1, item.Key.flipY ? -1 : 1);

                Matrix4x4 matrix = Matrix4x4.TRS(
                    transform.Position - new float3(item.Key.sprite.pivot, 0) * transform.Scale,
                    transform.Rotation,
                    flipVector * transform.Scale * new float3(item.Key.sprite.scale, 0) );

                mat.Add(matrix);

                //Graphics.RenderMesh(renderParams, SpriteRendererManager.Instance.GetQurdMesh(), 0, matrix);

            }

            Graphics.RenderMeshInstanced(renderParams, SpriteRendererManager.Instance.GetQurdMesh(), 0, mat);

        }

    }

    private Color CastFloat4ToColor(in float4 value)
    {

        return new Color(value.x, value.y, value.z, value.w);

    }

}
