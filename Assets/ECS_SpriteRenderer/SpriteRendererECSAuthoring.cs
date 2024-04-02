using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SpriteRendererECSAuthoring : MonoBehaviour
{

    public Sprite sprite;
    public Material material;
    public Color color = Color.white;
    public bool flipX;
    public bool flipY;

}

public struct SpriteData
{

    public int spriteHash;
    
    public float2 scale;
    public float2 pivot;

}

public struct MaterialData
{

    public int materialHash;

}

public struct SpriteRendererECS : IComponentData
{

    public SpriteData sprite;
    public MaterialData material;
    public float4 color;
    public bool flipX;
    public bool flipY;

}

public class SpriteRendererECSBaker : Baker<SpriteRendererECSAuthoring>
{
    public override void Bake(SpriteRendererECSAuthoring authoring)
    {

        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new SpriteRendererECS
        {

            sprite = SpriteRendererManager.Instance.CreateSpriteData(authoring.sprite),
            material = SpriteRendererManager.Instance.CreateMaterialData(authoring.material),
            color = new float4(authoring.color.r, authoring.color.g, authoring.color.b, authoring.color.a),
            flipX = authoring.flipX,
            flipY = authoring.flipY

        });

    }


}