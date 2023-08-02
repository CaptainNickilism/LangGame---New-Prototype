//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Articy.Languagegamearticy;
using Articy.Unity;
using Articy.Unity.Constraints;
using Articy.Unity.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Articy.Languagegamearticy.Features
{
    
    
    public class EmojiFeatureFeatureConstraint
    {
        
        private Boolean mLoadedConstraints;
        
        private TextConstraint mMeaning;
        
        private ReferenceSlotConstraint mEmojiSprite;
        
        public TextConstraint Meaning
        {
            get
            {
                EnsureConstraints();
                return mMeaning;
            }
        }
        
        public ReferenceSlotConstraint EmojiSprite
        {
            get
            {
                EnsureConstraints();
                return mEmojiSprite;
            }
        }
        
        public virtual void EnsureConstraints()
        {
            if ((mLoadedConstraints == true))
            {
                return;
            }
            mLoadedConstraints = true;
            mMeaning = new Articy.Unity.Constraints.TextConstraint(2048, "Write here the meaning of the emoji", null, true, false);
            mEmojiSprite = new Articy.Unity.Constraints.ReferenceSlotConstraint("Asset;", "Drag emoji sprite here", "None;Image;", "");
        }
    }
}