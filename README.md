# PSFlockingLibrary

PSFlockingLibrary is a customizable Unity3D Flocking-Library used on Rigidbodies. It follows three general rules: 

* Alignment
* Cohesion
* Separation
* Optional: Follow a "goal"-GameObject

Each of these rules can be overriden to calculate a custom force that should be added to the boid, providing a lot of flexibility. 


## Features

* Override Alignment, Cohesion, Separation and Goal-Seeking rules
* Autocreate boids at startup
* Add and Remove Boids at any time


## Getting PSFlockingLibrary

You can download the complete Unity Project with two example scenes, demonstrating the standard flocking behaviour and a simple script subclassing the flocking class to create a customized flocking behaviour. 

Alternatively you can download the .dll file at the release page.


## Usage

In your .cs files, add a "using PSFlocking;" statement. 
In Unity, add the PSUnitManager to a gameobject, and set its variables in the inspector. Important: Set at least the "Unit Prefab" variable. If you want to use a goal that the units should follow, then also set the "Goal" variable. Change the other parameters as you like to get a feeling for them. 

You can add and remove units at any time by calling AddFlockingUnit and RemoveFlockingUnit on the PSUnitManager script.


## Custom Flocking

The four functions Alignment, Cohesion, Separation and SeekGoal can be subclassed. Each of them returns a Vector3 representing a force that will be applied on the unit's rigidbody. All four forces will be added together, normalized, and then applied to the rigidbody. By subclassing PSFlockingUnit, you can implement custom versions of these 4 functions. Look in the documentation for the exact function definitions.


## Contributors
[Florian Peinsold](https://github.com/Floppi003) (Maintainer)
[Philipp Sonnleitner](https://github.com/AlmostSonny)


## License

PSFlockingLibrary is released under the [MIT license](https://github.com/npruehs/game-math/blob/develop/LICENSE).