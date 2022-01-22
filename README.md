# Unity Color Palette Generator
Simple little tool that extracts colors from a texture and generates materials with given shader.
It extracts colors with distance threshold that compares colors using Euclidean distance on color space.

## Usage
 ### Editor
 1. Open Tools/Color Palette Generator.
 2. Select a texture.
 3. Specify threshold.
 4. Select a folder to save materials.

 ### Runtime
 1. Call ExtractColorsFromTexture static method from ColorPaletteGenerator class.
 2. Create material(s) with extracted color(s) using CreateMaterial(s) method.