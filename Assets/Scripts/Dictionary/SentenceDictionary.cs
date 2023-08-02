using Articy.Languagegamearticy;
using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceDictionary : MonoBehaviour
{
    public static SentenceDictionary instance;

    public List<ArticyObject> discoveredSentences;

    public GameObject sentencePrefab, sentencesList, dictionaryObject;

    bool open = false;
    void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void AddToDictionary(ArticyObject sentenceToAdd)
    {
        if(sentenceToAdd is IObjectWithFeatureInspectableSentenceFeature)   // If the object has the InspectableSentence feature...
        {
            discoveredSentences.Add(sentenceToAdd);                         //...Add the object to the lsit of discovered sentences
        }
    }

    public void ToggleDictionary()
    {
        if (!open)
        {
            dictionaryObject.SetActive(true);
            open = true;
            DisplayDictionary();
        }
        else
        {
            dictionaryObject.SetActive(false);
            foreach (Transform child in sentencesList.transform)    // Destroy all objects in the sentencesList object
            {
                Destroy(child.gameObject);
            }
            open = false;
        }
        
    }

    public void DisplayDictionary()
    {
        foreach (ArticyObject articyObject in discoveredSentences)
        {
            if (articyObject is IObjectWithFeatureInspectableSentenceFeature)
            {
                Instantiate(sentencePrefab, sentencesList.transform);
                sentencePrefab.GetComponent<ArticyReference>().SetObject(articyObject);
            }
        }
    }
}
