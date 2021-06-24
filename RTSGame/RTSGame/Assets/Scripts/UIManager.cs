using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;
using System;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Timer
    
    [SerializeField] public GameObject createVillager;
    [SerializeField] public GameObject spawnLoc;
    [SerializeField] public TextMeshProUGUI villagerQueue;
    [SerializeField] public Slider Timer;
    private float cooldownTime = 10f;
    private int vSpawnCount;
    private int villagerCount = 0;
    public int totalVillagers;

    private void Update()
    {
        if (vSpawnCount > 0)
        {
            cooldownTime -= 1 * Time.deltaTime;
            if (cooldownTime <= 0f && villagerCount < totalVillagers)
            {
                Instantiate(createVillager, spawnLoc.transform);
                cooldownTime = 3f;
                villagerCount = spawnLoc.transform.childCount - 1;
                vSpawnCount--;
            }
            villagerQueue.text = vSpawnCount.ToString("0");
            Timer.value = cooldownTime / 10f;
        }
    }
    public void SpawnObject()
    {
        if (villagerCount < totalVillagers && villagerCount + vSpawnCount < totalVillagers)
        {
            vSpawnCount++;
            if (vSpawnCount > 10)
                vSpawnCount = 10;
        }
    }
    #endregion

    //#region Tabs
    //public GameObject villagerBtn;
    //public GameObject jobsBtn;
    //public GameObject buildingsBtn;

    //public GameObject villagerPanel;
    //public GameObject jobsPanel;
    //public GameObject buildingsPanel;

    //[SerializeField]
    //public class TabPair
    //{
    //    public Button TabButton;
    //    public CanvasGroup TabContent;
    //    //public class tab strip : MonoBehaviour 
    //    //{
    //    //    public TabPair[] TabCollection;
    //    //    public Sprite TabIconPicked;
    //    //    public Sprite TabIconDefault;
    //    //    public Button DefaultTab;
    //    //    //Tutorial continues adding things here...

    //    //    protected int CurrentTabIndex { get; set; }
    //    //    protected void SetTabState(int index, bool picked)
    //    //    {
    //    //        TabPair affectedItem = TabCollection[index];
    //    //        affectedItem.TabContent.interactable = picked;
    //    //        affectedItem.TabContent.blocksRaycasts = picked;
    //    //        affectedItem.TabContent.alpha = picked ? 1 : 0;
    //    //        affectedItem.TabButton.image.sprite = picked ? TabIconPicked : TabIconDefault;
    //    //    }
    //    //    public void PickTab(int index)
    //    //    {
    //    //        SetTabState(CurrentTabIndex, false);
    //    //        CurrentTabIndex = index;
    //    //        SetTabState(CurrentTabIndex, true);
    //    //    }
    //    //    protected int? FindTabIndex(Button tabButton)
    //    //    {
    //    //        var currentTabPair = TabCollection.FirstOrDefault(x => x.TabButton == tabButton);
    //    //        if (currentTabPair == default)
    //    //        {
    //    //            Debug.LogWarning("The tab " + DefaultTab.gameObject.name + " does not belong to the tab strip " + name + ".");
    //    //            return null;
    //    //        }
    //    //        return Array.IndexOf(TabCollection, currentTabPair);
    //    //    }
    //    //    protected void OnEnable()
    //    //    {
    //    //        //Initialize all tabs to an unpicked state
    //    //        for (var i = 0; i < TabCollection.Length; i++)
    //    //        {
    //    //            SetTabState(i, false);
    //    //        }
    //    //        //Pick the default tab
    //    //        if (TabCollection.Length > 0)
    //    //        {
    //    //            var index = FindTabIndex(DefaultTab);
    //    //            //If tab is invalid, instead default to the first tab.
    //    //            if (index == null)
    //    //                index = 0;
    //    //            CurrentTabIndex = index.Value;
    //    //            SetTabState(CurrentTabIndex, true);
    //    //        }
    //    //    }
    //    //    protected void Start()
    //    //    {
    //    //        for (var i = 0; i < TabCollection.Length; i++)
    //    //        {
    //    //            //Storing the current value of i in a locally scoped variable.
    //    //            var index = i;
    //    //            TabCollection[index].TabButton.onClick.AddListener(new UnityAction(() => PickTab(index)));
    //    //        }
    //    //    }
    //    //}
    //}
    //#endregion
}
