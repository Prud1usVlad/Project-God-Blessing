using Assets.Scripts.Helpers;
using Assets.Scripts.Models;
using Assets.Scripts.Stats;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Curse", fileName = "CurseCard")]
public class CurseCard : SerializableScriptableObject
{
    [SerializeField]
    private List<string> prophesies;
    [SerializeField]
    private List<GameObject> imagePrefabs;

    public string curseName;
    public int level;

    [System.NonSerialized]
    public int prophesyIdx = 0;
    [System.NonSerialized]
    public int imageIdx = 0;

    public List<StatMod> statMods;
    public List<ResMod> resMods;

    public string prophesy => prophesies[prophesyIdx];
    public GameObject image => imagePrefabs[imageIdx];

    public void Randomize()
    {
        prophesyIdx = Random.Range(0, prophesies.Count);
        imageIdx = Random.Range(0, imagePrefabs.Count);
    }

    public void Init(int propIdx, int imgIdx)
    {
        prophesyIdx = propIdx;
        imageIdx = imgIdx;
    }
}
