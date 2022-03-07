# BazaarBananas
Midterm project for Intermediate Game Programming.  
This game is a procedurally generated merchant game, where the player plays as a travelling merchant, represented by a wagon. They must traverse a map of nodes to reach the end buy as many bananas as possible in the end.
Each node can either be a market or an event. In a market, the player may freely buy and sell their resources in preparation for the next segment of the run.
With an event, the player has a series of choices on how to spend resources to get past them and somethings gain.
Markets are identified by cyan nodes and events by pink nodes. You must select a choice to move on.
You can close the menu to look at the map, but you must open it again and either hit 'confirm' in the market or select a choice in the event.

#Sources  
[DelaunatorSharp](https://github.com/nol1fe/delaunator-sharp) Package by nol1fe is licensed under the MIT license.   
Documentation of [Delaunay Triangulation](https://github.com/mapbox/delaunator) and original project the above Package was adapted from Delaunator by mapbox which is licensed under the ISC license. 
Inspiration to use A* star with delaunay was inspired from a [project](https://github.com/yurkth/stsmapgen) by yurkth though no direct code was used.

A* Pathfinding Implementation was done largely by myself, but the method was adopted from [A* Pathfinding in Unity](https://www.youtube.com/watch?v=alU04hvz6L4) by Code Monkey.

#Art and Audio Assets  
Icons, Background, and Sound: [Free 2D Mega Pack](https://assetstore.unity.com/packages/2d/free-2d-mega-pack-177430) from Brackeys, Unity Asset Store  
Trees and Foliage: [Fantasy Forest Environment - Free Demo](https://assetstore.unity.com/packages/3d/environments/fantasy/fantasy-forest-environment-free-demo-35361) from Triforge Assets, Unity Asset Store  
GUI: [Fantasy Wooden GUI : Free](https://assetstore.unity.com/packages/2d/gui/fantasy-wooden-gui-free-103811) from Black Hammer, Unity Asset Store  
Player Wagon, Mountain, Rocks, and Wooden Structures: [RPG Poly Pack - Lite](https://assetstore.unity.com/packages/3d/environments/landscapes/rpg-poly-pack-lite-148410) from Gigel, Unity Asset Store
Banana: Copyright: [stockunlimited](https://stock.pixlr.com/creator/stockunlimited)