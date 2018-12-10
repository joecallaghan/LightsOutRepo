The LightsOut solution contains 3 projects

1)  LightsOut is a class library project which contains a single class 'Grid' which implements the behaviour of the game.
    The public interface of the class contains;
	a) A constructor to instantiate a game grid of the specified dimensions with a specified number of randomly 'lit'
	   initial positions
	b) A boolean indexer property whch gets the status (true=lit, false=unlit) of a single grid position (supplied as a row/column pair)
	c) An int property CountLit which gets the number of currently 'lit' positions in the grid
	d) A boolean property Complete which gets the status of the game (ie solved or in-play)
	e) A method ActivatePosition which toggles the status of a specified position and adjacent positions

2) LightsOutTest is a class library project which contains a series of NUNIT tests for the Grid class. 

3) LightsOutGame is a windows forms application which provides a visual interface to the Grid class. It allows the user
   to start and play an instance of the game until either the game is complete, the user quits the application or
   the user chooses to start a new game. If the user completes the game (fat chance) the application will detect this
   and terminate the game, disabling the game grid until the user either starts a new game or quits the application.

   By default, the game will be played within a 5 by 5 row/column matrix and there will be 5 randomly selected positions
   which are initially lit. These default settings may be configured in the application config file by editing the
   following keys in the appSettings section;
   rows - the number of rows in the grid (may be set to a value between 3 and 20)
   columns - the number columns in the grid (may be set to a value between 3 and 20)
   initialCount - the number of randomly selected, initially lit positions in each new game (may be set to a
   value between 1 and the size of the grid)
