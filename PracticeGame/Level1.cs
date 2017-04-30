using System;
using System.Collections.Generic;
using CocosSharp;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Diagnostics;

namespace PracticeGame
{
    public class Level1 : CCLayerColor
    {

        // Label objects
        CCLabel label, deathCounter;
        // Sprite objects, basically anything that has an image
        CCSprite jumpButton, leftButton, rightButton, player1, platform1, platform2, wall1, wall2, goal;
        // Arrays to keep track 
        CCSprite[] collisionPlatforms = new CCSprite[2];
        CCSprite[] collisionWalls = new CCSprite[2];
        CCSprite[] normalPlatforms = new CCSprite[2];
        CCSprite[] normalWalls = new CCSprite[2];
        CCSprite[] bluePlatforms = new CCSprite[0];
        CCSprite[] blueWalls = new CCSprite[0];
        CCSprite[] greenPlatforms = new CCSprite[0];
        CCSprite[] greenWalls = new CCSprite[0];

        // Keeps track of the player's original position
        int playerXStart = 100;
        int playerYStart = 200;

        public Level1() : base(CCColor4B.White)
        {

            // create and initialize a Label
            label = new CCLabel("", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
            deathCounter = new CCLabel("Death Count: ", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);

            // add the labels as a child to this Layer
            AddChild(label);
            AddChild(deathCounter);

            // Creates a medium platform at given coordinates
            platform1 = new CCSprite("LargePlatform.png");
            platform1.PositionX = 100;
            platform1.PositionY = 150;
            AddChild(platform1);

            // Creates a large platform at given coordinates
            platform2 = new CCSprite("LargePlatform.png");
            platform2.PositionX = 700;
            platform2.PositionY = 150;
            AddChild(platform2);
            
            // Creates a vertical wall at given coordinates
            wall1 = new CCSprite("VerticalWall.png");
            wall1.PositionX = 6;
            wall1.PositionY = 280;
            //wall2.AnchorPoint = CCPoint.AnchorMiddleLeft;
            AddChild(wall1);

            // Creates a vertical wall at given coordinates
            wall2 = new CCSprite("VerticalWall.png");
            wall2.PositionX = 762;
            wall2.PositionY = 280;
            //wall2.AnchorPoint = CCPoint.AnchorMiddleRight;
            AddChild(wall2);

            // Adds the platforms to my platform array so I can check for collision
            collisionPlatforms[0] = platform1;
            collisionPlatforms[1] = platform2;

            // Adds the walls to my wall array so I can check for collision
            collisionWalls[0] = wall1;
            collisionWalls[1] = wall2;

            // Adds the platforms to my platform array so I can check for collision
            normalPlatforms[0] = platform1;
            normalPlatforms[1] = platform2;

            // Adds the walls to my wall array so I can check for collision
            normalWalls[0] = wall1;
            normalWalls[1] = wall2;

            // Creates the player at given coordinates
            player1 = new CCSprite("Player.png");
            player1.PositionX = playerXStart;
            player1.PositionY = playerYStart;
            AddChild(player1);

            // Creates the goal square at given area
            goal = new CCSprite("Goal.png");
            goal.PositionX = platform2.PositionX + 30;
            goal.PositionY = platform2.PositionY + 20;
            AddChild(goal);

            // Add the jump button
            jumpButton = new CCSprite("JumpButton");
            AddChild(jumpButton);

            // Creates the left button
            leftButton = new CCSprite("Left");
            AddChild(leftButton);

            // Creates the right button
            rightButton = new CCSprite("Right");
            AddChild(rightButton);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;

            // Places the "Jump" button
            jumpButton.PositionX = 768;
            jumpButton.PositionY = 0;
            jumpButton.AnchorPoint = CCPoint.AnchorLowerRight;

            // Places the "Left" button
            leftButton.PositionX = 0;
            leftButton.PositionY = 0;
            leftButton.AnchorPoint = CCPoint.AnchorLowerLeft;

            // Places the "Right" button
            rightButton.PositionX = 384;
            rightButton.PositionY = 0;
            rightButton.AnchorPoint = CCPoint.AnchorMiddleBottom;

            // Places and colors the death counter label
            deathCounter.PositionX = bounds.MinX + 5;
            deathCounter.PositionY = bounds.MaxY - 5;
            deathCounter.AnchorPoint = CCPoint.AnchorUpperLeft;
            deathCounter.Color = CCColor3B.Black;

            // Places and colors the end game label
            label.PositionY = bounds.MaxY - 500;
            label.PositionX = bounds.MinX + 400;
            label.AnchorPoint = CCPoint.AnchorMiddle;
            label.Color = CCColor3B.Red;




            // Register for touch events
            var touchJump = new CCEventListenerTouchAllAtOnce();
            var touchLeft = new CCEventListenerTouchAllAtOnce();
            var touchRight = new CCEventListenerTouchAllAtOnce();
            touchJump.OnTouchesBegan = OnJumpButtonTouch;
            touchRight.OnTouchesBegan = OnRightButtonTouch;
            touchRight.OnTouchesEnded = OnRightButtonEnd;
            touchLeft.OnTouchesBegan = OnLeftButtonTouch;
            touchLeft.OnTouchesEnded = OnLeftButtonEnd;
            AddEventListener(touchJump, this);
            AddEventListener(touchRight, this);
            AddEventListener(touchLeft, this);
            Schedule(RunGameLogic);
        }

        #region Screen Touch Events

        /// <summary>
        /// Logic for when the user touch the "Right" button. While touched,
        /// the player moves to the right at the specified velocity
        /// </summary>
        /// <param name="touches"></param>
        /// <param name="touchEvent"></param>
        void OnRightButtonTouch(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                var pointTouched = touches[0].Location;
                var rightButtonBox = rightButton.BoundingBoxTransformedToParent;
                if (rightButtonBox.ContainsPoint(pointTouched))
                {

                    player1XVelocity = 200;

                }
            }
        }
        /// <summary>
        /// Logic for when the user stops touching the "Right" button.
        /// The player stops moving right
        /// </summary>
        /// <param name="touches"></param>
        /// <param name="touchEvent"></param>
        void OnRightButtonEnd(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                player1XVelocity = 0;
            }
        }
        /// <summary>
        /// Logic for when the user touch the "Left" button. While touched,
        /// the player moves to the left at the specified velocity
        /// </summary>
        /// <param name="touches"></param>
        /// <param name="touchEvent"></param>
        void OnLeftButtonTouch(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                var pointTouched = touches[0].Location;
                var leftButtonBox = leftButton.BoundingBoxTransformedToParent;
                if (leftButtonBox.ContainsPoint(pointTouched))
                {
                    player1XVelocity = -200;
                }
            }
        }
        /// <summary>
        /// Logic for when the user stops touching the "Left" button.
        /// The player stops moving left
        /// </summary>
        /// <param name="touches"></param>
        /// <param name="touchEvent"></param>
        void OnLeftButtonEnd(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                player1XVelocity = 0;
            }
        }
        /// <summary>
        /// Logic for when the user touches the "Jump" button.
        /// The player moves upwards at a specified velocity only
        /// if the player is currently on top of a platform
        /// </summary>
        /// <param name="touches"></param>
        /// <param name="touchEvent"></param>
        void OnJumpButtonTouch(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                if (doesPlayerOverlapAPlatform)
                {
                    var pointTouched = touches[0].Location;
                    var jumpButtonBox = jumpButton.BoundingBoxTransformedToParent;
                    if (jumpButtonBox.ContainsPoint(pointTouched))
                    {
                        player1.PositionY = collisionPlatforms[platformIndex].PositionY + 20;
                        player1YVelocity += 200;
                    }
                }
            }
        }
        #endregion

        #region Collision Logic

        /// <summary>
        /// Takes walls and checks if the player hits the right
        /// side of any, keeping track of which wall the player hits
        /// </summary>
        /// <param name="walls"></param>
        /// <returns></returns>
        public bool PlayerWallRightSideCollision(CCSprite[] walls)
        {
            bool atRightSideOfWall = false;
            for (int i = 0; i < walls.Length; i++)
            {
                if (player1.BoundingBoxTransformedToParent.IntersectsRect(walls[i].BoundingBoxTransformedToParent))
                {
                    if (player1.BoundingBoxTransformedToParent.MinX < walls[i].BoundingBoxTransformedToParent.MaxX &&
                        player1.BoundingBoxTransformedToParent.MinX > walls[i].BoundingBoxTransformedToParent.MinX)
                    {
                        atRightSideOfWall = true;
                        wallIndex = i;
                    }
                }
            }
            return atRightSideOfWall;
        }

        /// <summary>
        /// Takes walls and checks if the player hits the left
        /// side of any, keeping track of which wall the player hits
        /// </summary>
        /// <param name="walls"></param>
        /// <returns></returns>
        public bool PlayerWallLeftSideCollision(CCSprite[] walls)
        {
            bool atLeftSideOfWall = false;
            for (int i = 0; i < walls.Length; i++)
            {
                if (player1.BoundingBoxTransformedToParent.IntersectsRect(walls[i].BoundingBoxTransformedToParent))
                {
                    if (player1.BoundingBoxTransformedToParent.MaxX > walls[i].BoundingBoxTransformedToParent.MinX &&
                        player1.BoundingBoxTransformedToParent.MaxX < walls[i].BoundingBoxTransformedToParent.MaxX)
                    {
                        atLeftSideOfWall = true;
                        wallIndex = i;
                    }

                }
            }
            return atLeftSideOfWall;
        }
        /// <summary>
        /// Takes all platforms and checks if the player hits
        /// the bottom of any, keeping track of which platform
        /// the player may have hit
        /// </summary>
        /// <param name="platforms"></param>
        /// <returns></returns>
        public bool PlayerHitsBottomOfPlatform(CCSprite[] platforms)
        {
            bool BottomOfPlatformHit = false;
            for (int i = 0; i < platforms.Length; i++)
            {
                if (player1.BoundingBoxTransformedToParent.IntersectsRect(platforms[i].BoundingBoxTransformedToParent))
                {
                    if (player1.BoundingBoxTransformedToParent.MaxY >= platforms[i].BoundingBoxTransformedToParent.MinY &&
                        player1.BoundingBoxTransformedToParent.MaxY < platforms[i].BoundingBoxTransformedToParent.MaxY)
                    {
                        BottomOfPlatformHit = true;
                        bottomPlatformIndex = i;
                    }
                }
            }
            return BottomOfPlatformHit;
        }
        /// <summary>
        /// Adds collision to all platforms for the player
        /// and keeps track of which platform the player is on
        /// </summary>
        /// <param name="platforms"></param>
        /// <returns></returns>
        public bool PlayerPlatformCollision(CCSprite[] platforms)
        {
            bool isTouchingAPlatform = false;
            for (int i = 0; i < platforms.Length; i++)
            {
                if (player1.BoundingBoxTransformedToParent.IntersectsRect(platforms[i].BoundingBoxTransformedToParent))
                {
                    if (player1.BoundingBoxTransformedToParent.MaxY > platforms[i].BoundingBoxTransformedToParent.MaxY)
                    {
                        isTouchingAPlatform = true;
                        platformIndex = i;
                    }

                }
            }
            return isTouchingAPlatform;
        }
        #endregion

        // Booleans that check for collision
        bool doesPlayerHitBottomOfPlatform = false;
        bool doesPlayerOverlapAPlatform;
        bool doesPlayerOverlapLeftSideOfWall = false;
        bool doesPlayerOverlapRightSideOfWall = false;
        bool playerFallsDownPit = false;
        // Boolean for collision with end game square
        bool GameEnd;
        // Boolean for if the player is blue
        bool playerIsBlue = false;
        // Boolean for if the player is green
        bool playerIsGreen = false;
        // Floats for keeping track of player velocity
        float player1XVelocity;
        float player1YVelocity;
        // Float for gravity
        const float gravity = 140;
        // Ints to keep track of which wall the player is colliding with
        int platformIndex = 0;
        int bottomPlatformIndex = 0;
        int wallIndex = 0;
        // Int to keep track of deaths
        int deathCount = 0;

        void RunGameLogic(float frameTimeInSeconds)
        {
            // Sets the bool if the player falls down the pit, aka goes past the bottom of the screen
            playerFallsDownPit = (player1.BoundingBoxTransformedToParent.MaxY < 0.0);
            // Sets the bool for player platform collision
            doesPlayerOverlapAPlatform = PlayerPlatformCollision(collisionPlatforms);
            // Sets the bool for if the player hits the bottom of a platform
            doesPlayerHitBottomOfPlatform = PlayerHitsBottomOfPlatform(collisionPlatforms);
            // Sets the bool for if the player hits the left side of a wall
            doesPlayerOverlapLeftSideOfWall = PlayerWallLeftSideCollision(collisionWalls);
            // Sets the bool for if the player hits the right side of a wall
            doesPlayerOverlapRightSideOfWall = PlayerWallRightSideCollision(collisionWalls);
            // Sets the bool for if the player reaches the ending square
            GameEnd = player1.BoundingBoxTransformedToParent.IntersectsRect(goal.BoundingBoxTransformedToParent);
            // Death counter label for death count
            deathCounter.Text = "Death Count: " + deathCount;
            // Logic for if the player falls down the pit
            // If true, add one to death counter and reset players position
            if (playerFallsDownPit)
            {
                deathCount++;
                RemoveChild(player1);
                player1 = new CCSprite("Player.png");
                player1.PositionX = playerXStart;
                player1.PositionY = playerYStart;
                AddChild(player1);
                playerIsBlue = false;
                playerIsGreen = false;
                ReplaceAllCollision();
            }
            /*
            // Logic to pick up the blue powerup and turn the player blue
            // Turns the player blue and removes the blue platforms/walls from collision
            if (player1.BoundingBoxTransformedToParent.IntersectsRect(bluePowerup.BoundingBoxTransformedToParent) && !playerIsBlue)
            {
                bluePlayer.PositionX = player1.PositionX;
                bluePlayer.PositionY = player1.PositionY;
                AddChild(bluePlayer);
                RemoveChild(player1);
                player1 = bluePlayer;
                playerIsBlue = true;
                playerIsGreen = false;
                BlueLoseCollision();
            }
            // Logic to pick up the green powerup and turn the player green
            // Turns the player green and removes the green platforms/walls from collision
            if (player1.BoundingBoxTransformedToParent.IntersectsRect(greenPowerup.BoundingBoxTransformedToParent) && !playerIsGreen)
            {
                greenPlayer.PositionX = player1.PositionX;
                greenPlayer.PositionY = player1.PositionY;
                AddChild(greenPlayer);
                RemoveChild(player1);
                player1 = greenPlayer;
                playerIsGreen = true;
                playerIsBlue = false;
                GreenLoseCollision();
            }
            // Logic for picking up the powerup that clears all colors
            // Turns the player back to the original color and adds all 
            // platforms and walls into the collision platforms/walls
            if (player1.BoundingBoxTransformedToParent.IntersectsRect(clearPowerups.BoundingBoxTransformedToParent)
                    && (playerIsBlue || playerIsGreen))
            {
                player1 = new CCSprite("Player.png");
                if (playerIsBlue)
                {
                    player1.PositionX = bluePlayer.PositionX;
                    player1.PositionY = bluePlayer.PositionY;
                }
                else if (playerIsGreen)
                {
                    player1.PositionX = greenPlayer.PositionX;
                    player1.PositionY = greenPlayer.PositionY;
                }
                AddChild(player1);
                RemoveChild(bluePlayer);
                RemoveChild(greenPlayer);
                playerIsBlue = false;
                playerIsGreen = false;
                ReplaceAllCollision();
            }
            */
            // Logic for stopping a player from going through walls with collision
            if (doesPlayerOverlapRightSideOfWall)
            {
                player1.PositionX += 4;
            }
            if (doesPlayerOverlapLeftSideOfWall)
            {
                player1.PositionX -= 4;
            }
            // Logic for if the player hits the bottom of a platform
            if (doesPlayerHitBottomOfPlatform)
            {
                player1YVelocity = 0;
                player1.PositionY = collisionPlatforms[bottomPlatformIndex].BoundingBoxTransformedToParent.MinY - 12;
            }
            // Logic for if the player isn't touching a platform
            if (!doesPlayerOverlapAPlatform)
            {
                player1YVelocity += frameTimeInSeconds * -gravity;
            }
            // Simple math for player movement
            player1.PositionX += player1XVelocity * frameTimeInSeconds;
            player1.PositionY += player1YVelocity * frameTimeInSeconds;
            // Logic for if the player is on top of a platform
            if (doesPlayerOverlapAPlatform)
            {
                player1YVelocity = 0;
                player1.PositionY = collisionPlatforms[platformIndex].BoundingBoxTransformedToParent.MaxY + 10;
            }
            // Float variables with the right and left edges of the screen
            float screenRight = VisibleBoundsWorldspace.MaxX;
            float screenLeft = VisibleBoundsWorldspace.MinX;

            // logic if the player goes off the sides of the screen
            if (player1.PositionX > screenRight)
            {
                player1.PositionX = 0;
            }
            if (player1.PositionX < screenLeft)
            {
                player1.PositionX = screenRight;
            }
            // Logic of the player reaches the goal square
            if (GameEnd)
            {
                RemoveAllChildren();
                AddChild(label);
                label.Text = "Congratulations!";
            }

        }
        #region Helper methods for picking up certain powerups
        /// <summary>
        /// Replaces all of the collision for all platforms
        /// Used when the clear all powerups circle is touched
        /// </summary>
        private void ReplaceAllCollision()
        {
            collisionPlatforms = new CCSprite[normalPlatforms.Length + greenPlatforms.Length + bluePlatforms.Length];
            for (int i = 0; i < normalPlatforms.Length; i++)
            {
                collisionPlatforms[i] = normalPlatforms[i];
            }
            for (int i = 0; i < greenPlatforms.Length; i++)
            {
                collisionPlatforms[i + normalPlatforms.Length] = greenPlatforms[i];
            }
            for (int i = 0; i < bluePlatforms.Length; i++)
            {
                collisionPlatforms[i + normalPlatforms.Length + greenPlatforms.Length] = bluePlatforms[i];
            }
            collisionWalls = new CCSprite[normalWalls.Length + greenWalls.Length + blueWalls.Length];
            for (int i = 0; i < normalWalls.Length; i++)
            {
                collisionWalls[i] = normalWalls[i];
            }
            for (int i = 0; i < greenWalls.Length; i++)
            {
                collisionWalls[i + normalWalls.Length] = greenWalls[i];
            }
            for (int i = 0; i < blueWalls.Length; i++)
            {
                collisionWalls[i + normalWalls.Length + greenWalls.Length] = blueWalls[i];
            }
        }
        /// <summary>
        /// Blue platforms and walls lose collision
        /// Used when blue powerup is touched
        /// </summary>
        private void BlueLoseCollision()
        {
            collisionPlatforms = new CCSprite[normalPlatforms.Length + greenPlatforms.Length];
            for (int i = 0; i < normalPlatforms.Length; i++)
            {
                collisionPlatforms[i] = normalPlatforms[i];
            }
            for (int i = 0; i < greenPlatforms.Length; i++)
            {
                collisionPlatforms[i + normalPlatforms.Length] = greenPlatforms[i];
            }
            collisionWalls = new CCSprite[normalWalls.Length + greenWalls.Length];
            for (int i = 0; i < normalWalls.Length; i++)
            {
                collisionWalls[i] = normalWalls[i];
            }
            for (int i = 0; i < greenWalls.Length; i++)
            {
                collisionWalls[i + normalWalls.Length] = greenWalls[i];
            }
        }
        /// <summary>
        /// Green platforms and walls lose collision
        /// Used when green powerup is touched
        /// </summary>
        private void GreenLoseCollision()
        {
            collisionPlatforms = new CCSprite[normalPlatforms.Length + bluePlatforms.Length];
            for (int i = 0; i < normalPlatforms.Length; i++)
            {
                collisionPlatforms[i] = normalPlatforms[i];
            }
            for (int i = 0; i < bluePlatforms.Length; i++)
            {
                collisionPlatforms[i + normalPlatforms.Length] = bluePlatforms[i];
            }
            collisionWalls = new CCSprite[normalWalls.Length + blueWalls.Length];
            for (int i = 0; i < normalWalls.Length; i++)
            {
                collisionWalls[i] = normalWalls[i];
            }
            for (int i = 0; i < blueWalls.Length; i++)
            {
                collisionWalls[i + normalWalls.Length] = blueWalls[i];
            }
        }
        #endregion
    }
}

