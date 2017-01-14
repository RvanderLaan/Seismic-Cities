# Seismic Cities

[Wiki page](https://github.com/donand/Earthquake-Game/wiki)

Team members:
 - Andrea Donati
 - Remi Van Der Laan
 - Sanders Kats
 - Fabian Van Gent
 - Joris Bouwens
 
# Latest weekly report (# 7, 14-1-2017)
In the holidays, some time was spent on improving and refactoring the game up until that point. We were aware that the core concept was not good enough, but it wasn't until we received the feedback on the beta version this week that we had a brainstorm about how we can make the game (more) (1) informative, (2) challenging, (3) understandable and (4) fun.

Since then we have thought of and started implementing a solution for these four goals:

1. Informative: Until now, we heaviliy relied on the Unity physics engine simulate the earthquake. We tried different ways of creating buildings out of separate parts connected with joints, but we concluded that this is too unpredictable and costs too much time to tweak. This week we have implemented a much simpler system. For each of the building zones in a level, we pre-define what building needs to be placed there. This gives us much more control in designing the levels with a specific goal to let the player learn something, and apply that knowledge in the next levels.

2. Challenging: We have designed 9 levels with specific learning objectives, in which we introduce new features one by a time. In addition to placing buildings, we have implemented the basics of an upgrading system. Foundations can be placed on buildings that will improve its stability. This can cause some buildings to be placed on soils that they were previously unsuited for. This in combination with the level design adds a new level of complexity to the game. 

3. Understandable: In order to introduce the player to the controls and game mechanics, we have implemented a simple tutorial system for the first level. Furthermore, we thought of a dialog system which ties in with the next point. One character gives you advice at the start of a level and when you fail.

4. Fun: First of all, the game would not be fun if it was not understandable and had some challenge to it. In order to enhance the fun however, we have thought of a story that makes use of the dialog system. One character is the cause of the earthquakes, while the other introduces you to the game and gives you advice. Other than that, sound effects and animations are still on the to-do list, but have a lower priority.


In this new concept, we got rid of the 'being the earthquake' mode. We could think of no benefits, while it would cost much time to further design and implement. 

Such a drastic change in concept is obviously not supposed to occur in the second to last week, but we think it is the only way to make this project successful. This weekend we will implement the levels that were designed, so they can be tested on monday. The rest of the week will be spent on polishing and applying feedback. 

The tasks for the programmers are the most obvious, but the non-programmers have the following tasks to work on:
* Create a fun and informative story, related to what will be learned in each level
* Design the levels, this includes the layout soil layers, building zones, which buildings can be placed and most importantly: What will the player learn from it
* Write short but informative tooltips for buildings and soil layers
* Test the game with other people and collect useful feedback about the goals that were described previously
