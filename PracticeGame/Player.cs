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
    public class Player : CCNode
    {
        CCSprite sprite;

        public Player () : base()
        {
            sprite = new CCSprite("Player.png");
            sprite.AnchorPoint = CCPoint.AnchorUpperLeft;
            AddChild(sprite);
        }
    }
}