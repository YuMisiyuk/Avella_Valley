// Procedural Terrain Painter by Staggart Creations http://staggart.xyz
// Copyright protected under Unity Asset Store EULA

using System.Collections.Generic;
using UnityEngine;

namespace sc.terrain.proceduralpainter
{
    public partial class Utilities
    {
        //Each splatmap has 4 channels, returns the number of splatmaps needed to fit all layers
        public static int GetSplatmapCount(int layerCount)
        {
            if (layerCount > 12) return 4;
            if (layerCount > 8) return 3;
            if (layerCount > 4) return 2;

            return 1;
        }
        
        public static int GetChannelIndex(int layerIndex)
        {
            return (layerIndex % 4);
        }
        
        //Create an RGBA component mask (eg. channelIndex=2 samples the Blue channel)
        public static Vector4 GetVectorMask(int channelIndex)
        {
            switch (channelIndex)
            {
                case 0: return new Vector4(1, 0, 0, 0);
                case 1: return new Vector4(0, 1, 0, 0);
                case 2: return new Vector4(0, 0, 1, 0);
                case 3: return new Vector4(0, 0, 0, 1);
                default: return Vector4.zero;
            }
        }
        
        public static int GetSplatmapIndex(int layerIndex)
        {
            if (layerIndex > 11) return 3;
            if (layerIndex > 7) return 2;
            if (layerIndex > 3) return 1;
            
            return 0;
        }
        
        public static Bounds RecalculateBounds(Terrain[] terrains)
        {
            Vector3 minSum = Vector3.one * Mathf.Infinity;
            Vector3 maxSum = Vector3.one * Mathf.NegativeInfinity;

            foreach (Terrain terrain in terrains)
            {
                if(terrain == null) continue;
                if(!terrain.gameObject.activeInHierarchy) continue;;

                //Min/max bounds corners in world-space
                Vector3 min = terrain.GetPosition(); //Safe to assume terrain starts at origin
                Vector3 max = terrain.GetPosition() + terrain.terrainData.size; //Note, size is slightly more correct in height than bounds

                if (min.x < minSum.x || min.y < minSum.y || min.z < minSum.z) minSum = min;
                if (max.x > maxSum.x || max.y > maxSum.y || max.z > maxSum.z) maxSum = max;
            }

            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

            bounds.SetMinMax(minSum, maxSum);

            //Increase bounds height for flat terrains
            if (bounds.size.y < 1f)
            {
                bounds.Encapsulate(new Vector3(bounds.center.x, bounds.center.y + 1f, bounds.center.z));
            }

            return bounds;
        }

        public static TerrainLayer[] SettingsToLayers(List<LayerSettings> layerSettings)
        {
            //Weirdness, using an array means the layers aren't actually assigned in reversed order
            List<TerrainLayer> layerList = new List<TerrainLayer>();
            
            //Convert LayerSettings to Layers
            for (int i = layerSettings.Count-1; i >= 0; i--)
            {
                layerList.Add(layerSettings[i].layer);
            }

            return layerList.ToArray();

        }

        public static bool HasMissingTerrain(Terrain[] terrains)
        {
            bool isMissing = false;

            for (int i = 0; i < terrains.Length; i++)
            {
                if (terrains[i] == null) isMissing = true;
            }

            return isMissing;
        }
    }
}