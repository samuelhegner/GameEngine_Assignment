# Submission Markdown

## What it does and how it works

### Procedural Mesh
This is a procedural landmass that is made using octave of perlin noise. This landmass is highly customisable in Unity Editor with some custom unity editor scripts and exposing important variables to the user in the Map Generator gameObject. The first option in the Map Generator is switching between 3 modes of generating. The first two generate 2d texture onto a separate plane under the mesh and the last creates a the mesh with those first two textures applied to it. The second variable is the Noise scale which divides essentially zooms in and out of the noise map created. The third variable is the Octaves of Noise. This decides how many octaves of noise will be stacked over each other. These octaves of noise are controlled by the next two variables. Lacunarity chooses the increase in frequency/ level of detail increase per octave and persistence controls the amplitude/ the level of impact of later octaves. The mesh height multiplier works in tandem with the Mesh height curve. All mesh vertices y numbers are multiplied by the height multiplier. The height curve can then limit the multiplication of those y values based on their noise value. Essentially it allows user to stop things like water being bumpy. The seed and the Offset are tools to randomise the location of the noise that is sampled. The Auto Update bool allows unity to live update the mesh with every change you make and the Random on Start creates a unique landscape based on the variables chosen on Star of the program. The Regions are Structs that hold information about the colour map of the world. You can create as many Regions as you want, assign them a colour, whether the region should have trees or not.

### Clouds
These are cloud that are spawned of screen and float over the mesh. The cloud avoid the ground and try to keep a constant hover distance. The clouds grow in size slowly when over land and grow quickly when over water. When they hit maximum size, the start shrinking until they hit minimum size and repeat the process. While they shrink, they rain or snow based on their height.

### Procedurally placed Forest
This forest spawn at runtime. The trees are spread out based on which regions have the bool trees ticked. The max number of trees is then split evenly between the regions picked.

### Day and Night Cycle
Two directional lights rotate around the mesh. One being the moon and one being the sun. These look at the mesh to keep it light day and night. The sun is also tied in with the procedural skybox to make for some dramatic sun rises and sets.

## Tutorial vs Me
I got help with the terrain generation from the lectures and youtube tutorials. Primarily the editor scripts and getting the mesh and the 2d texture to display in editor without using playmode. The rest I managed to do myself with help of the api and forums.

## What am I the most proud of
I’m most proud of the clouds and the forest mainly because I was able to builds of tutorials and adjust/modify them to suit me. I was able to do things like sample colours using raycast for the clouds, i was able to procedurally place object on the mesh vertices based on regions, work with the post processing stack, basic obstacle avoidance with raycasts, particle systems with particle collisions. Also I feel like from now on I can expand the procedural world, add more things to spawn and start adding ai and other behaviours to spawnable things. I also leaned to Object pool and to use more oop concepts in unity

## Video
[![YouTube](http://img.youtube.com/vi/9A_KPoW-GiQ/0.jpg)](https://www.youtube.com/watch?v=9A_KPoW-GiQ)
