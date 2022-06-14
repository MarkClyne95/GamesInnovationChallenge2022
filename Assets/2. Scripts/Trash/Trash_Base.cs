using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash_Base : MonoBehaviour
{
    public ScriptableInteractableObject_Base trash;
    
    // Start is called before the first frame update


    private void Start()
    {
        
        switch(trash.Rarity)
        {
            case ItemRarity.Common:
                GetComponent<MeshRenderer>().material.color = Color.gray;
                break;

            case ItemRarity.Uncommon:
                GetComponent<MeshRenderer>().material.color = Color.white;
                break;

            case ItemRarity.Rare:
                GetComponent<MeshRenderer>().material.color = Color.blue;
                break;

            case ItemRarity.Epic:
                GetComponent<MeshRenderer>().material.color = new Color(255, 215, 0, 1);
                break;

            case ItemRarity.Legendary:
                GetComponent<MeshRenderer>().material.color = new Color(255, 215, 0, 1);
                break;
        }
    }
}
