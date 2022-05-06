using UnityEngine;

namespace sc.terrain.proceduralpainter
{
    [ExecuteInEditMode]
    public class TerrainChangeListener : MonoBehaviour
    {
        [HideInInspector]
        public Terrain terrain;

        void OnTerrainChanged(TerrainChangedFlags flags)
        {
            if (!terrain || !TerrainPainter.Current) return;

            if ((flags & TerrainChangedFlags.Heightmap) != 0)
            {
                if(TerrainPainter.Current.autoRepaint) TerrainPainter.Current.RepaintTerrain(terrain);
            }
        }
    }
}