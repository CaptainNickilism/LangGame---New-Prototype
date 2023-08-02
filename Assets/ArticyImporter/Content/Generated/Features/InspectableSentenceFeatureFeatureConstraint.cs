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
    
    
    public class InspectableSentenceFeatureFeatureConstraint
    {
        
        private Boolean mLoadedConstraints;
        
        private ReferenceSlotConstraint mSentence;
        
        private ReferenceStripConstraint mCorrectEmojis;
        
        public ReferenceSlotConstraint Sentence
        {
            get
            {
                EnsureConstraints();
                return mSentence;
            }
        }
        
        public ReferenceStripConstraint CorrectEmojis
        {
            get
            {
                EnsureConstraints();
                return mCorrectEmojis;
            }
        }
        
        public virtual void EnsureConstraints()
        {
            if ((mLoadedConstraints == true))
            {
                return;
            }
            mLoadedConstraints = true;
            mSentence = new Articy.Unity.Constraints.ReferenceSlotConstraint("Entity;", "Drag here an FinnishSentence entity", "None;", "FinnishSentence;");
            mCorrectEmojis = new Articy.Unity.Constraints.ReferenceStripConstraint(10000, "Entity;", "Drag here the correct emojis for the sentence", "None;", "Emoji;");
        }
    }
}