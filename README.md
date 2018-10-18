# Game Engine Assignment
An assignment that sets out to create a customisable world generation tool. 

## Introduction
In this project I aim to create a procedurally generated 3d terrain. This means procedurally generating the mesh and everything to do with it.
![](Pictures/Example%20Terrain.jpg)

## The Goal
The Goal of the project is to create a procedurally generated world that can be easily edited and changed from the unity editor. I plan on being able to control how mountainous the level is, whether its an island or not, the colours of the different Biomes ect. The procedural terrain will be created using Perlin noise at different derivatives to achieve a natural looking landscape.
![](Pictures/Procedural%20Island.jpg)


## The Difficulty
In terms of what is going to cause problem in this project, I can foresee a few challenges. The first difficulty will be getting a grasp of being able to shape the custom mesh with the derrivatives/octives of Perlin noise. This however is very well documented and described through many websites and tutorials which will aid when I inevitably get stuck. The second difficulty will be figuring out how to shade the scene to represent a realistic looking landscape. I've never touched writing custom shaders, so hopefully this wont block progress. Finally, I also think that adding custom Unity Editor slider, windows ect. will provide a challenge. Again this will be the first time i have attempted to implement that in any of my projects.

![](Pictures/Terrain%20Example.jpg)

## References
- https://www.scratchapixel.com/lessons/procedural-generation-virtual-worlds/perlin-noise-part-2/perlin-noise-computing-derivatives //Handy resource to learn about noise derivatives
- http://flafla2.github.io/2014/08/09/perlinnoise.html //Explination of Perlin Noise and its aplications
- https://catlikecoding.com/unity/tutorials/noise-derivatives/ //Tutorial (Great starting point)
- https://www.youtube.com/watch?v=IKB1hWWedMk // Tutorial (Related Java example of using noise terrain generation)
- https://gamedevacademy.org/complete-guide-to-procedural-level-generation-in-unity-part-1/ //Tutorial (Unity Related Example)
- https://starscenesoftware.com/fractscape.html //Goal (Be able to create procedural terrain and being able to control what you are creating)
- https://www.youtube.com/watch?v=8ZEMLCnn8v0 //Explination on Perlin Noise
- https://docs.unity3d.com/ScriptReference/Mathf.PerlinNoise.html //Unity static Mathf function that will be used
