﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour {
    public Texture2D mapTexture;
    public List<ColorToPrefab> colorToPrefabsList;

    [System.Serializable]
    public struct ColorToPrefab
    {
        public Color32 color;
        public GameObject prefab;
    }

    private void Start()
    {
        LoadMap();
    }

    public void EmptyMap()
    {
        while (transform.childCount > 0)
        {
            Transform c = transform.GetChild(0);
            c.SetParent(null);
            Destroy(c.gameObject);
        }
    }

    public void LoadMap()
    {
        EmptyMap();
        Color32[] allPixels = mapTexture.GetPixels32();

        int width = mapTexture.width;
        int height = mapTexture.height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SpawnTileAt(allPixels[(y * width) + x], x, y);
            }
        }
    }

    void SpawnTileAt(Color32 color, int x, int y)
    {
        if (color.a == 0)
            return;

        foreach (ColorToPrefab ctp in colorToPrefabsList)
        {
            if(ctp.color.Equals(color))
            {
                GameObject go = Instantiate<GameObject>(ctp.prefab, new Vector3(x, y, 0), Quaternion.identity);
                return;
            }
        }
        Debug.LogError("No colo to prefab found for:" + color.ToString());
    }
}