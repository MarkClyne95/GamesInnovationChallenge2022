using System;
using System.Collections.Generic;
using UnityEngine;

public class TrashCollection : MonoBehaviour
{
    List<GameObject> trash = new List<GameObject>();
    [SerializeField]Transform playerTransform;
    [SerializeField]Animator anim;
    [SerializeField]AudioSource audioSrc;
    [SerializeField]TMPro.TMP_Text factField;
    public AudioClip collectSound;
    private int trashCollected;
    Score ScoreSet;

    public TMPro.TMP_Text countText;
    int trashCount;

    string[] randomFacts = new string[] {"78% of marine mammals are at risk of choking on plastic", "On average, one supermarket goes through 60 million paper bags each year.", 
    "There is a giant floating patch of trash off the west coast of the USA", "Humans only use around 1% of the available water on Earth", "Toilet paper alone accounts for up to 10 million trees being cut down per year.",
    "The Earth as we know it is around 1 million years old, since then over a million species have become extinct.", "Recycling a glass bottle saves enough energy to light a single lightbulb for up to 4 hours!", "Water, air and housing quality are thought to be responcible for upwards of 13% of disease in Europe", "Air pollution accounts for eight months on average, of life lost.", "Throwing plastic bags and other plastic in the ocean, kills as many as 1 million sea creatures annually.", "Rainforests are cut down at a rate of 100 acres per minute.", 
    "By 2100, the global urban population will produce three times more waste than today.", "The world population is ~215,000 people larger today than it was yesterday.", "The world's rainforests will be gone by 2100 if the current rate of destruction continues."};

    public string GetRandomFact(){
        int randomNum = UnityEngine.Random.Range(0, randomFacts.Length);

        return randomFacts[randomNum];
    }

    //getter for trash collected to use in another class
    public int GetTrashCollected(){
        return trashCollected;
    }

    private void Awake()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Trash"))
        {
            trash.Add(go);
        }
    }

    private void Start()
    {
        trashCount = trash.Count;
        countText.text = trashCount.ToString();
        ScoreSet = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
        ScoreSet.SetScoreInUI();
    }

    private void FixedUpdate()
    {
        CleanUpTrash();
    }

    private void CleanUpTrash()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        try
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began && Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Trash"))
                {
                    GameObject clickedObject = hit.collider.gameObject;
                    Vector3 distanceBetweenCharAndTrash = playerTransform.position - clickedObject.transform.position;
                    
                    if (clickedObject != null && distanceBetweenCharAndTrash.magnitude <= 10.0f)
                    {
                        audioSrc.PlayOneShot(collectSound);
                        anim.SetTrigger("PickingUp");
                        ScoreSet.OverallScore += clickedObject.GetComponent<Trash_Base>().trash.Score;
                        ScoreSet.SetScoreInUI();
                        trash.Remove(clickedObject);
                        countText.text = trash.Count.ToString();
                        Destroy(clickedObject);
                        Debug.Log($"{clickedObject.name} hit");
                        factField.text = GetRandomFact();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }
}
