using Articy.Languagegamearticy;
using Articy.Unity;
using Articy.Unity.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiObject : Interactable
{
    private void Start()
    {
        if(GetComponent<ArticyReference>().GetObject<ArticyObject>() is IObjectWithFeatureEmojiFeature emojiFeature)    // If the object has the Emoji feature...
        {
            IAsset m_sprite = emojiFeature.GetFeatureEmojiFeature().EmojiSprite as Asset;                               //...Fetch the sprite from Articy
            if (m_sprite != null) GetComponent<SpriteRenderer>().sprite = m_sprite.LoadAssetAsSprite();                 //...and Display the EMoji's sprite
        }
        
        
    }
    public override void Interact()
    {
        
        MinigameManager.instance.SelectEmoji(GetComponent<ArticyReference>().GetObject<ArticyObject>());
    }
}
