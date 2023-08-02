using Articy.Languagegamearticy;
using Articy.Languagegamearticy.Features;
using Articy.Unity;
using Articy.Unity.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CurrentMinigame
{
    None,
    Inspectable,
    Dialogue
}

public class MinigameManager : MonoBehaviour
{

    public static MinigameManager instance = null;
    public CurrentMinigame currentMinigame;
    public GameObject minigameUI,prompt,emojiList, sentencesList, emojiPrefab, answerPrefab, scoreField;
    public List<ArticyObject> guessedEmojis, currentEmojis, allEmojis;
    
    public IObjectWithFeatureInspectableSentenceFeature currentInspectableSentence;
    public IObjectWithFeatureNPCFeature currentNPC;
    public IObjectWithFeatureDialogueLineFeature currentDialogueLine;

    public int dialogueIndex = 0;
    public int currentScore, maxScore;

    void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        currentMinigame = CurrentMinigame.None; // Reset the "currentMinigame"
        InitialiseAllEmojisList();              // Fetch all the Emojis in the Articy Database
    }

    private void Update()
    {
        scoreField.GetComponent<TMP_Text>().text = "Score: " + currentScore + "/" + maxScore; // Set the "SCoreField" object's text to currentScore/maxScore
    }

    void InitialiseAllEmojisList()  // Fetch all the Emojis in the Articy Database
    {
        foreach (ArticyObject entity in ArticyDatabase.GetAllOfType<ArticyObject>())    // Foreach Articy Object in the database
        {
            if (entity is IObjectWithFeatureEmojiFeature objectWithFeatureEmoji)        // If it has the "Emoji" feature
            {
                allEmojis.Add(objectWithFeatureEmoji as ArticyObject);                  // Add the ArticyObject to AllEmojis list
            }
        }
        
    }

    public void OpenMinigameUI(ArticyObject articyObject)   // Open the Minigame UI and start a minigame depending on the clicked object
    {
        minigameUI.SetActive(true);                                                                                                 // Open the main UI element
        if (articyObject is IObjectWithFeatureInspectableSentenceFeature objectWithFeatureInspectableSentence)                      // If the ArticyObject has the "Inspectable" feature
        {
            if(!SentenceDictionary.instance.discoveredSentences.Contains(objectWithFeatureInspectableSentence as ArticyObject))     // If the InspectableSentence is NOT already discovered
            {
                currentInspectableSentence = objectWithFeatureInspectableSentence;                                                  // Log the current InspectableSentence
                currentMinigame = CurrentMinigame.Inspectable;                                                                      // Set the currentMinigame to Inspectable
                StartEmojiMinigame();                                                                                               // Start the Emoji minigame
            }
                        
        }
        else if(articyObject is IObjectWithFeatureNPCFeature objectWithFeatureNPC)                                                  // If the ArticyObject has the "NPC" feature
        {
            currentNPC = objectWithFeatureNPC;                                                                                      // Log the current NPC
            currentMinigame = CurrentMinigame.Dialogue;                                                                             // Set the currentMinigame to Dialogue
            StartDialogueMinigame();                                                                                                // Start the Dialogue minigame
        }                                                                                                                           
    }                                                                                                                               

    public void CloseMinigameUI()   // Hide the Minigame UI
    {
        minigameUI.SetActive(false);
    }

    void StartEmojiMinigame()       // Setup the initial conditions for the Emoji Minigame
    {
        OpenEmojiList();                                                                                        // Display the EmojiList
        DisplayInspectableSentence();                                                                           // Set the "Prompt" text to the current InspectableSentence
        SetInitialScore(currentInspectableSentence.GetFeatureInspectableSentenceFeature().CorrectEmojis);       // Reset the current score, set the max score depending on content size
        NewEmojiTurn();                                                                                         // Start a new turn for the Emoji Minigame
    }

    void StartDialogueMinigame()    // Setup the initial conditions for the Dialogue Minigame
    {       
        dialogueIndex = 0;                                                                                      // Reset the dialogueIndex
        OpenSentencesList();                                                                                    // Display the Sentences list layout element
        DisplayAnswers();                                                                                       // Display all the sentences in the player's dicitonary as answers
        SetInitialScore(currentNPC.GetFeatureNPCFeature().dialogueLines);                                       // Reset the current score, set the max score depending on content size
        NewDialogueTurn();                                                                                      // Start a new turn for the Dialogue Minigame
    }

    void SetInitialScore(List<ArticyObject> contentList)    // Reset the current score, set the max score depending on content size
    {
        currentScore = 0;               // Reset the current score to 0
        maxScore = contentList.Count;   // Set the Maximum score to the number of Emojis or DialogueLines 
    }

    void OpenEmojiList()        // Display the Emoji list layout element
    {
        emojiList.SetActive(true);
        sentencesList.SetActive(false);
    }

    void OpenSentencesList()    // Display the Sentences list layout element
    {
        emojiList.SetActive(false);
        sentencesList.SetActive(true);
    }

    void DisplayInspectableSentence()   // Set the "Prompt" text to the current InspectableSentence
    {        
        if (currentInspectableSentence.GetFeatureInspectableSentenceFeature().Sentence is IObjectWithFeatureFinnishSentenceFeature finnishSentence) // Check and fetch the FinnishSentence of this InspectableSentence
        {
            prompt.GetComponent<TMP_Text>().text = finnishSentence.GetFeatureFinnishSentenceFeature().SentenceText;         // Display the FinnishSentence's text
            
        }
    }

    void DisplayNewDialogueLine()       // Set the "Prompt" text to the current DialogueLine
    {
        if (currentDialogueLine.GetFeatureDialogueLineFeature().Sentence is IObjectWithFeatureFinnishSentenceFeature finnishSentenceFeature)    // Check and fetch the FinnishSentence
        {
            prompt.GetComponent<TMP_Text>().text = finnishSentenceFeature.GetFeatureFinnishSentenceFeature().SentenceText;  // Display the FinnishSentence's text

        }
    }

    void NewEmojiTurn()     // Start a new turn for the Emoji Minigame
    {
        ClearCurrentEmojis();                                   // Delete all the emojis currently displayed

        if (GetNextCorrectEmoji())                              // If the "Next Correct Emoji" was succesfully fetched...
        {
            
            while (currentEmojis.Count < 6)                     // ...While there's less than 6 emojis...
            {
                
                GetWrongEmoji();                                // ...Get a new wrong emoji...
            }
            
            StartCoroutine(InstantiateCurrentEmojis());         // ...And then visualise all the Emojis
        }
        else                                                    // If there was NO "Next Correct Emoji" (content of the minigame is finished)...
        {
            EndEmojiMinigame();                                 // Finish the Emoji Minigame, add the sentence to the player's dictionary
        }        
    }

    void NewDialogueTurn()      // Start a new turn for the Dialogue Minigame
    {
        
        if (GetNextDialogueSentence())      // If the "Next Dialogue Line" was correctly fetched...
        {            
            DisplayNewDialogueLine();       // ...Display the new dialogue line
        }
        else                                // If there was NO "Next Dialogue Line" (content of the minigame is finished)...
        {
            EndDialogueMinigame();          // ...Finish the Dialogue Minigame, progress to the next level
        }
    }

    

    void ClearCurrentEmojis()   // Delete all the emojis currently displayed
    {
        currentEmojis.Clear();                              // Clear the list of Current Emojis
        foreach (Transform child in emojiList.transform)    // Foreach game object in the Emoji List object...
        {
            Destroy(child.gameObject);                      // ...Destroy the game object
        }
    }

    bool GetNextCorrectEmoji()  // Get the next emoji that will need to be guessed
    {
        bool result = false;
        foreach (ArticyObject emojiObject in currentInspectableSentence.GetFeatureInspectableSentenceFeature().CorrectEmojis)   // Foreach correct emoji in the sentence...
        {
            if (emojiObject is IObjectWithFeatureEmojiFeature)                                                                  // ...Safe check that it's an emoji...
            {
                if (!guessedEmojis.Contains(emojiObject))                                                                       // ...If it's not already guessed...
                {
                    currentEmojis.Add(emojiObject);                                                                             // ...Add to current list
                    result = true;
                    break;
                }
            }
        }
        return result;
    }

    void GetWrongEmoji()    // Get a random emoji that is NOT the correct choice
    {
        ArticyObject emojiObject = allEmojis[Random.Range(0, allEmojis.Count)];                                         // Get one random emoji from all the emojis...
        
        if (emojiObject is IObjectWithFeatureEmojiFeature)                                                              // ...Safe check that it's an emoji...
        {
            if (!currentEmojis.Contains(emojiObject) &&                                                                 // ...If not already in current emojis...
                !currentInspectableSentence.GetFeatureInspectableSentenceFeature().CorrectEmojis.Contains(emojiObject)) // ...And it's NOT correct...
            {
                currentEmojis.Add(emojiObject);                                                                         // ...Add to current list
                    
            }
        }
        
    }

    bool GetNextDialogueSentence()      // Get the next dialogue line of the NPC
    {
        bool result = false;
        if (dialogueIndex < currentNPC.GetFeatureNPCFeature().dialogueLines.Count)                              // If the DialogueIndex is smaller than the number of DialogueLines of the NPC...
        {
            ArticyObject dialogueLineObject = currentNPC.GetFeatureNPCFeature().dialogueLines[dialogueIndex];   // ...Get the ArticyObject at this DialogueIndex...
            {
                if (dialogueLineObject is IObjectWithFeatureDialogueLineFeature dialogueLineFeature)            // ...If it has the DialogueLineFeature...
                {
                    currentDialogueLine = dialogueLineFeature;                                                  // ...Log the current dialogue line
                    result = true;                   
                }
            }
        }
        return result;        
    }

    IEnumerator InstantiateCurrentEmojis()      // Display the current emojis of the turn
    {
        List<ArticyObject> shuffledList = currentEmojis.OrderBy(x => Random.value).ToList();    // Shuffle the list of current emojis
        foreach (ArticyObject articyObject in shuffledList)                                     // Foreach shuffled item in the list...
        {
            if (articyObject is IObjectWithFeatureEmojiFeature)                                 // ...If it has the EmojiFeature...
            {
                yield return new WaitForSeconds(0.2f);                                          // ... Leave some time for future animation...
                GameObject newEmoji = Instantiate(emojiPrefab, emojiList.transform);            // ...Instantiate a new Emoji prefab in the Emoji List...
                newEmoji.GetComponent<ArticyReference>().SetObject(articyObject);               // ... and Save the EmojiFeature in the ArticyReference component of the new object 


            }
        }
    }

    void DisplayAnswers()                   // Display all the sentences in the player's dicitonary as answers
    {
        foreach (ArticyObject articyObject in SentenceDictionary.instance.discoveredSentences)  // Foreach ArticyObject in the DiscoveredSentences in the SentenceDictionary...
        {
            if (articyObject is IObjectWithFeatureInspectableSentenceFeature)                   // ...If the ArticyObject is an InspectableSentence...
            {
                GameObject newAnswer = Instantiate(answerPrefab, sentencesList.transform);      // ...Instantiate a new "Answer Prefab"...
                newAnswer.GetComponent<ArticyReference>().SetObject(articyObject);              // ... and Save the InspectableSentence in the ArticyReference component of the new object
            }
        }
    }

    void EndEmojiMinigame()         // Finish the Emoji Minigame, add the sentence to the player's dictionary
    {
        Debug.Log("Finished inspectable minigame");
        CloseMinigameUI();                                                                          // Hide the Minigame UI
        guessedEmojis.Clear();                                                                      // Clear the guessed emojis (so they can be used in the next minigame)
        SentenceDictionary.instance.AddToDictionary(currentInspectableSentence as ArticyObject);    // Add the InspectableSentence to the player's dictionary
    }

    void EndDialogueMinigame()      // Finish the Dialogue Minigame, progress to the next level
    {
        Debug.Log("Finished dialogue minigame");
        CloseMinigameUI();                                                                          // Hide the Minigame UI
        // **LEVEL FINISHED!**
    }

    public void SelectEmoji(ArticyObject selectedEmoji) // Select an Emoji, check if it's the correct one, then start a new turn
    {
        if(selectedEmoji is IObjectWithFeatureEmojiFeature emojiObject)
        {
            Debug.Log(emojiObject.GetFeatureEmojiFeature().Meaning);
        }
        if (currentInspectableSentence.GetFeatureInspectableSentenceFeature().CorrectEmojis.Contains(selectedEmoji))    // If the Emoji is in the "Correct Emojis" list of the current Inspectable Sentence...
        {
            Debug.Log("Correct");
            guessedEmojis.Add(selectedEmoji);                                                                           // ...Log this emoji as already guessed...
            IncreaseScore();                                                                                            // ...Increase the player's current score...
            NewEmojiTurn();                                                                                             // ... and Start a new turn for the Emoji Minigame
        }
        else                                                                                                            // If the selected Emoji is wrong...
        {
            Debug.Log("Wrong");
            NewEmojiTurn();                                                                                             // ...Start a new turn for the Emoji Minigame
        }
    }

    public void SelectAnswer(ArticyObject selectedAnswer)   // Select an Answer, check if it's the correct one, then start a new turn
    {
        if(currentMinigame == CurrentMinigame.Dialogue)                                                                         // If the answer was clicked during the minigame (and not in the dictionary)...
        {
            if (selectedAnswer is IObjectWithFeatureInspectableSentenceFeature objectWithFeatureInspectable)                    // ...If it has the InspectableSentence feature...
            {
                if (currentDialogueLine.GetFeatureDialogueLineFeature().Answer == objectWithFeatureInspectable as ArticyObject) // ...If it is the correct answer of the current Dialogue Line...
                {
                    Debug.Log("Correct");
                    dialogueIndex++;                                                                                                // ...Advance the DialogueIndex...
                    IncreaseScore();                                                                                                // ...Increase the player's current score...
                    NewDialogueTurn();                                                                                              // ...and Start a new turn for the Dialogue Minigame
                }
                else                                                                                                            // ...If it is NOT the correct answer...
                {
                    Debug.Log("Wrong");
                    NewDialogueTurn();                                                                                          // ...Start a new turn for the Dialogue Minigame
                }
            }
        }
    }

    void IncreaseScore()    // Increase the player's current score
    {
        currentScore++;     // Add one point to the current score
    }

    

    

}
