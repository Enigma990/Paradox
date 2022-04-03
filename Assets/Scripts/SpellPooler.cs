using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPooler : MonoBehaviour
{
    private static SpellPooler instance;
    public static SpellPooler Instance { get { return instance; } }

    [SerializeField] GameObject spellPrefab = null;

    private List<GameObject> spellList;
    
    private void Awake()
    {
        instance = this;

        spellList = new List<GameObject>(10);

        for (int i = 0; i < 10; i++)
        {
            GameObject spellObject = Instantiate(spellPrefab, this.transform);
            spellObject.SetActive(false);
            spellList.Add(spellObject);
        }
    }

    public GameObject GetSpell()
    {
        foreach(GameObject spell in spellList)
        {
            if(!spell.activeInHierarchy)
            {
                return spell;
            }
        }

        GameObject spellObject = Instantiate(spellPrefab, this.transform);
        spellPrefab.SetActive(false);
        spellList.Add(spellObject);
        return spellObject;
    }

}
