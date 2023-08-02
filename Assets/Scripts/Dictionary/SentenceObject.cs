using Articy.Languagegamearticy;
using Articy.Unity.Interfaces;
using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SentenceObject : Interactable
{
    public TMP_Text sentenceField;
    public GameObject emojiField, emojiPreviewPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<ArticyReference>().GetObject<ArticyObject>() is IObjectWithFeatureInspectableSentenceFeature objectWithFeatureInspectable)         // If the object has the InspectableSentence feature...
        {
            if(objectWithFeatureInspectable.GetFeatureInspectableSentenceFeature().Sentence is IObjectWithFeatureFinnishSentenceFeature finnishSentence)    //...And if the sentence has the FinnishSentence feature...
            {
                sentenceField.text = finnishSentence.GetFeatureFinnishSentenceFeature().SentenceText;                                                           //... Set the text to the FinnishSentence text
            }
            foreach(ArticyObject articyObject in objectWithFeatureInspectable.GetFeatureInspectableSentenceFeature().CorrectEmojis)                         // Foreach Emoji of the InspectableSentence...
            {
                if(articyObject is IObjectWithFeatureEmojiFeature emojiFeature)                                                                             //...If the emoji has the Emoji feature
                {
                    GameObject newEmoji = Instantiate(emojiPreviewPrefab, emojiField.transform);                                                            //...Instantiate a new Emoji Prefab
                    IAsset m_sprite = emojiFeature.GetFeatureEmojiFeature().EmojiSprite as Asset;                                                           //...Fetch the sprite from Articy
                    if (m_sprite != null) newEmoji.GetComponent<SpriteRenderer>().sprite = m_sprite.LoadAssetAsSprite();                                    //...and display the sprite
                }
            }
        }
    }

    public override void Interact()
    {
        MinigameManager.instance.SelectAnswer(GetComponent<ArticyReference>().GetObject<ArticyObject>());
    }
}
