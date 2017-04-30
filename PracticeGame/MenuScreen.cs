using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CocosSharp;

namespace PracticeGame
{
    class MenuScreen : CCLayerColor
    {
        CCLabel label;

        CCSprite level1, demoLevel;

        public MenuScreen() : base(CCColor4B.AliceBlue)
        {
            // create and initialize a Label
            label = new CCLabel("Josh Schooley: Senior Project", "fonts/MarkerFelt", 22, CCLabelFormat.SpriteFont);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // Use the bounds to layout the positioning of our drawable assets
            var bounds = VisibleBoundsWorldspace;

            label.AnchorPoint = CCPoint.AnchorMiddle;
            label.Color = CCColor3B.DarkGray;
            label.PositionY = bounds.MaxY - 100;
            label.PositionX = bounds.MinX + 400;
            AddChild(label);

            level1 = new CCSprite("Level1Button.png");
            level1.PositionY = bounds.MaxY - 400;
            level1.PositionX = bounds.MinX + 400;
            AddChild(level1);

            demoLevel = new CCSprite("DemoButton.png");
            demoLevel.PositionY = bounds.MaxY - 600;
            demoLevel.PositionX = bounds.MinX + 400;
            AddChild(demoLevel);

            var level = new CCEventListenerTouchAllAtOnce();
            var demoTouch = new CCEventListenerTouchAllAtOnce();
            level.OnTouchesBegan = Level1Touch;
            demoTouch.OnTouchesBegan = DemoLevelTouch;
            AddEventListener(level, this);
            AddEventListener(demoTouch, this);
        }

        void Level1Touch(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                var pointTouched = touches[0].Location;
                var level1Box = level1.BoundingBoxTransformedToParent;
                if (level1Box.ContainsPoint(pointTouched))
                {
                    var scene = new CCScene(Window);
                    var firstLevel = new Level1();

                    scene.AddChild(firstLevel);
                    Window.RunWithScene(scene);
                }
            }
        }

        void DemoLevelTouch(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                var pointTouched = touches[0].Location;
                var demoBox = demoLevel.BoundingBoxTransformedToParent;
                if (demoBox.ContainsPoint(pointTouched))
                {
                    var scene = new CCScene(Window);
                    var demo = new DemoLevel();

                    scene.AddChild(demo);
                    Window.RunWithScene(scene);
                }
            }
        }
    }
}