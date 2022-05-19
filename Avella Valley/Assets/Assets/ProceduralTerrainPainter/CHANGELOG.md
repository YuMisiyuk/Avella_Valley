1.0.3

Changed:
- When closing the Terrain Painter inspector, the project will now be saved. This ensures the terrain data is saved to disk. Avoids any errors from the PaintContext.ApplyDelayedActions function
- Asset now uses an assembly definition.
- Minimum supported version of MicroSplat is now 3.8.1

Fixed:
- Errors in 2021.2.0a15 due to the TerrainAPI leaving the experimental namespace

1.0.2

Added:
- Callback event for when a terrain is repainted (TerrainPainter.OnTerrainRepaint)
- A warning is now displayed if any terrain references have gone missing

Changed:
- Adding a new modifier now automatically selects it

Fixed:
- Texture modifier not aligning correctly if a terrain had a large negative position
- Modifier settings not drawing when Odin is installed

1.0.1

Added:
- Auto repaint option in Settings tab. Repaints a terrain when its height is modified.
- Option to refresh Vegetation Studio Pro when terrain is repainted

Changed:
- Renamed namespace to "sc.terrain.proceduralpainter" for consistency between other terrain tools

Fixed:
- Heatmap preview highlighting wrong terrain layer in some cases

1.0.0
Initial release