using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;

public class SpriteRendererManager 
{

    private SpriteRendererManager()
    {

        textureContainer = new();
        spriteDataContainer = new();
        materialContainer = new();
        materialDataContainer = new();

        mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;

    }

    private static SpriteRendererManager instance;
    private Dictionary<int, Texture2D> textureContainer;
    private Dictionary<int, Material> materialContainer;
    private Dictionary<int, SpriteData> spriteDataContainer;
    private Dictionary<int, MaterialData> materialDataContainer;
    private Mesh mesh;

    public static SpriteRendererManager Instance 
    { 
        get 
        {

            Init();
            return instance;
        
        }
    }

    private static void Init()
    {

        if(instance == null)
        {

            instance = new SpriteRendererManager();

        }

    }

    public SpriteData CreateSpriteData(Sprite sprite)
    {

        int hash = sprite.GetHashCode();

        SpriteData data = new SpriteData();

        if (!textureContainer.ContainsKey(hash))
        {

            data.spriteHash = hash;
            data.scale = 
                new float2
                    (
                    sprite.rect.width / sprite.pixelsPerUnit,
                    sprite.rect.height / sprite.pixelsPerUnit
                    );

            data.pivot = sprite.pivot / sprite.pixelsPerUnit;

            textureContainer.Add(hash, sprite.texture);
            spriteDataContainer.Add(hash, data);

        }
        else
        {

            data = spriteDataContainer[hash];

        }

        return data;

    }

    public MaterialData CreateMaterialData(Material material)
    {

        int hash = material.GetHashCode();

        MaterialData data = new MaterialData();

        if (!materialContainer.ContainsKey(hash))
        {

            data.materialHash = hash;

            materialContainer.Add(hash, material);
            materialDataContainer.Add(hash, data);

        }
        else
        {

            data = materialDataContainer[hash];

        }

        return data;

    }

    public Texture2D GetTextureByHash(int hash)
    {

        if (textureContainer.TryGetValue(hash, out var tex))
        {

            return tex;

        }

        return null;

    }

    public Material GetMaterialByHash(int hash)
    {


        if (materialContainer.TryGetValue(hash, out var mat))
        {

            return mat;

        }


        return null;

    }

    public Mesh GetQurdMesh()
    {


        return mesh;

    }

}