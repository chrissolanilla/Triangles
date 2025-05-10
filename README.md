# puzzlegame

To create a new level, there are several pieces to take care of. First, start by duplicating the PROTOTYPE_Game_Level scene. This will start you off with much of the game UI already hooked up for you.

Next, you'll need to set up the puzzle for this level. PEYTON WRITE HOW TO DO THIS HERE PLZ! :)

Once the puzzle itself is set up, you'll want to bring in all the pieces needed to solve the puzzle and set up what hints they should provide. All the available triangles are located in Prefabs/DefaultTriangles; go ahead and just drag and drop from there into the scene.

![Screenshot of the default triangle prefabs.](https://cdn.discordapp.com/attachments/619263932636463137/1100179173936463962/image.png)

Then, for each of these triangles, reposition its "hint piece" to one of the possible locations it could exist for a solution. Do this by selecting the hintPiece object for this triangle in the hierarchy of the level, and then use the move and rotate tools Unity provides to position it correctly. Here, we've positioned the hint piece for the large equilateral triangle. 

![Screenshot of the game hierarchy with the hintPiece object selected, next to a screenshot of the hint piece in the level positioned correctly.](https://cdn.discordapp.com/attachments/619263932636463137/1100179996326232154/image.png)

Don't worry about hiding these hint pieces for the game - our scripts will manage that.

Once the level has been set up, you need to connect it to the main menu and the player statistics page. Navigate to the MainMenu scene and create a button that will lead to this newly created puzzle. This button should exist under this hierarchy shown below, and you should be able to copy paste one of the existing PuzzleSlot buttons and simply modify its values.

![Screenshot of the game hierarchy with one of the Puzzle Slot objects selected.](https://cdn.discordapp.com/attachments/619263932636463137/1100181005958123582/image.png)

Once you've duplicated one of the existing puzzle slot buttons and repositioned it on the page to where you'd like it to exist, go ahead and click on it again to open the Inspector for this object. Scroll down to the On Click () panel. You will need to adjust two of the fields here: the text box under SceneNavigation.EnterLevel is where you put the name of the level as you would like it to appear on the player statistics report page. The text box under SceneNavigation.LoadScene is where you put the name of the level as it appears in Unity. For example, if you named your level Game_Level_1, you would write that exactly as it appears. 

![Screenshot of the On Click () functionality of a button leading to Level 1.](https://cdn.discordapp.com/attachments/810903500124192778/1100181666581991515/image.png)

That's it! From there, your level should be working perfectly, though you'll also probably want to drag in an image on top of the button to show what the button leads to. If you run into any issues, please feel free to reach out to one of us, and we will help as we can!
