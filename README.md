# The Undra Map Prototype

This is a prototype of a "world map" of Undra's subterranean area, which contains 15,000 rooms (sold as NFTs). The Unity project offers two ways to create a map from that data:

1. The **plotter** which randomly maps 15,000 rooms on a suitably-sized grid, and
2. The **deed viewer** which sorts the rooms into abstracted "areas."

The intention of this mapping program was to sketch out the world of Undra in a physical sense (points on a 2D grid) and a relational sense (sorting room deeds into "locales"). It is intended for internal use, but could be brought into the public eye as pre-release marketing with a bit of spit and polish, or used as the basis for an in-game world map.

## Room Data Structure

The mapping program reads data from a JSON file and interprets them as room deeds. Room deeds have a data structure as follows:

- `id` _(string)_ - the unique ID of the room deed
- `owner` _(string)_ - the owner of the room deed (if there isn't an owner, returns a `null` value)
- `type` _(string)_ - the type of the room (Site, Store, or Command)
- `rarity` _(string)_ - the rarity of the room (Uncommon, Rare, Epic, or Legendary)

Currently, the data is mocked using Mockaroo ([the schema is linked here](https://mockaroo.com/62006fb0)); the intent was that the mapping program would read data from [Opensea's API](https://docs.opensea.io/reference/api-overview) in realtime.

The Room C# class outlined in `Utils/Room.cs` also contains a `coords` Vector2 value; the idea was that a room could be assigned a 2-dimensional coordinate value (x, y) which wouldn't change once the data was serialised, so that rooms would always appear at those coordinates on the world map.

## Plotter

The plotter can be found in the Plotter scene. Click "play" to run the scene in the Unity editor; the rooms will be plotted as 1x1 squares on a large grid, plotting one room every frame update. To plot 15,000 rooms, this takes a few minutes. Enough to make a cup of tea, if you're brewing it correctly.

Originally, all rooms were plotted at once when the scene was loaded, but this had a tendency to crash my hardware.

**JSON Data** - a list of the room deeds contained in a JSON file (see above)
**Grid Size** - the area of the grid (a room deed is a 1x1 square) - the area is `gridSize * gridSize`, so it's always a square
**Minimum Spacing** - the minimum amount of space between rooms

You can zoom in on your mouse position with the scroll wheel, and hovering over a plot will show the deed information in the right-hand panel.

You can also change the grid size in the right-hand control panel; the slider will automatically set the minimum range to be the minimum possible size for the number of rooms in the dataset and the desired minimum spacing between plots.

## Deed Viewer

The GlobalView scene will create an abstracted map of squares, each of which represent a locale of around 100 room deeds.

### Global View

Click on a tile to see the "local view" of ~100 rooms.

**This isn't actually complete at this time.** 
- you can't exit out to the global view
- it''s also pretty hard-coded right now to separate the deeds into locales of 100

### Local View

The local view is an abstracted map of a "locale" in the world of Undra. Hover over a tile to see the data associated with the tile.

This should be taken as a kind of map of where deeds are positioned _in relation to one another_. It was the first step towards making a three-dimensional relational map of Undra (similar to the map in [_Fez_](https://fezthegame.fandom.com/wiki/World_Map)); business decisions de-prioritised the development of this idea.

**Also not complete at this time.**
- deeds are represented in the order they appear in the JSON file.
- the user cannot "exit" to the global map