1.0.2

Added:
- Package description and assembly definition, allowing the tool to be used as a package
- Option to keep the original object's name

Fixed:
- When a prefab variant was assigned as the replacement object, the base prefab was being used

Changed:
- The "Prefab overrides" option now also keeps components on model prefab instances
- Undo/Redo support is now bypassed on Unity 2021.2+ while editing a prefab, as this causes an Editor crash

1.0.1

Changed:
- If only one object was selected, the new replacement is then selected

Fixed:
- Editor crash if a selected object was in the project window