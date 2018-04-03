Nathan Glick
202-02

Description of World:
	My environment is an open area with three distinct areas: a tropical section with a lake and a waterfall, a forest-like area on a cliff, and a town.


World Exploration:
	In order to explore the world, there are three different cameras: an overhead view, a view of the cows/path followers, and a first person controller. You can switch between cameras by pressing 'C'. To turn on debug lines, press 'F1'.


~Steering Behavior Description~

Flocking:
	The flockers are a group of birds that are flying around the map. The flockers are using the flow field in order to move through the world.

Path Following:
	The path followers are cows running around in a pen near the town. The cows are just running around in the pen in order to get exercise, and the path is a circular path inside the pen.

Flow Field Following:
	The flockers are following the flow field instead of wandering. In order to generate the flow field vectors, I use perlin noise to create a smooth flow field.

Area of Resistance:
	The area of resistance is a muddy section of the pen that the cows are in. As the cows walk through the mud, their speed is reduced.

Resouces:
	~Trees, water, and textures are from Standard Assets
	~Houses, waterfall, birds, fences, and cows are from the Unity Asset Store

	~All my algorithms are based on previous projects, and from lectures given in class / Nature of Code. No outside resources were used to develop my algorithms.

Other Notes:
	~The game lags considerably when debug lines are on (drops from about 60fps to 10fps on my computer)
	~Flockers will occationally overlap even though separation has a very high weight (I think this is because of the flow field because the code is taken directly from my flocking project where it worked correctly)
	~I created a custom shader that I wanted to add to the project, but I couldn't figure out how to apply it to the terrain, so it went unused