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
    public class Platforms : CCNode
    {
        CCSprite platform;
        public Platforms(PlatformSize size) : base()
        {
            switch (size)
            {
                case PlatformSize.Large:
                    platform = new CCSprite("LargePlatform.png");
                    break;
                case PlatformSize.Medium:
                    platform = new CCSprite("MediumPlatform.png");
                    break;
                case PlatformSize.Small:
                    platform = new CCSprite("SmallPlatform.png");
                    break;
                case PlatformSize.Tiny:
                    platform = new CCSprite("TinyPlatform.png");
                    break;
            }
            platform.AnchorPoint = CCPoint.AnchorMiddle;
            AddChild(platform);
        }
    }


    public enum PlatformSize
    {
        Large,
        Medium,
        Small,
        Tiny
    }
}