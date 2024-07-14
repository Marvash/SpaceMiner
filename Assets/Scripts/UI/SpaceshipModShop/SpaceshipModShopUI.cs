using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpaceshipModShopUI : MonoBehaviour, IGameUI
{
    [SerializeField]
    private Canvas spaceshipModShopCanvas;
    public GameInputControls InputControls { get; private set;}

    public UnityEvent<IGameUI> OnActivateUI { get; private set;}

    public UnityEvent<IGameUI> OnDeactivateUI { get; private set;}

    public bool IsActive { get; private set;}
    [field:SerializeField]
    public int Priority { get; set; }
    [SerializeField]
    private GameObject modListItemPrefab;

    [SerializeField]
    private Transform modShopListContainer;
    [SerializeField]
    private TextMeshProUGUI moneyTextField;

    private List<SpaceshipModListItemUI> modShopListItems = new List<SpaceshipModListItemUI>();

    void Awake() {
        OnActivateUI = new UnityEvent<IGameUI>();
        OnDeactivateUI = new UnityEvent<IGameUI>();
        InputControls = GameInputControls.ShopMenu;
        IsActive = false;
    }

    public void InitSpaceshipModShopData(SpaceshipModsPlayerDataSO spaceshipModsPlayerData, int playerCash) {
        UpdateModList(spaceshipModsPlayerData.CompleteModsConfigList);
        UpdateBalance(playerCash);
    }

    private void UpdateModList(List<SpaceshipModConfigBaseSO> modList) {
        int modListItemCount = modShopListItems.Count;
        for(int i = modListItemCount - 1; i >= 0; i--) {
            modShopListItems[i].SetToggleGroup(null);
            modShopListItems[i].gameObject.SetActive(false);
            Destroy(modShopListItems[i].gameObject);
        }
        modShopListItems.Clear();
        ToggleGroup group = modShopListContainer.GetComponent<ToggleGroup>();
        foreach(SpaceshipModConfigBaseSO mod in modList) {
            if(mod.CurrentModLevel > 0) {
                GameObject modShopListItem = Instantiate(modListItemPrefab);
                SpaceshipModListItemUI listItem = modShopListItem.GetComponent<SpaceshipModListItemUI>();
                listItem.UpdateModListItemUI(mod);
                listItem.SetToggleGroup(group);
                modShopListItem.transform.SetParent(modShopListContainer, false);
                modShopListItems.Add(listItem);
                listItem.OnItemSelect.AddListener(HandleModListItemSelected);
            }
        }
        foreach(SpaceshipModConfigBaseSO mod in modList) {
            if(mod.CurrentModLevel == 0) {
                GameObject modShopListItem = Instantiate(modListItemPrefab);
                SpaceshipModListItemUI listItem = modShopListItem.GetComponent<SpaceshipModListItemUI>();
                listItem.UpdateModListItemUI(mod);
                listItem.SetToggleGroup(group);
                modShopListItem.transform.SetParent(modShopListContainer, false);
                modShopListItems.Add(listItem);
                listItem.OnItemSelect.AddListener(HandleModListItemSelected);
            }
        }
    }

    public void UpdateBalance(int playerCash) {
        moneyTextField.text = playerCash + " $";
    }

    private void HandleModListItemSelected(SpaceshipModConfigBaseSO config) {
    }

    public void ActivateUI()
    {
        spaceshipModShopCanvas.enabled = true;
        IsActive = true;
        OnActivateUI.Invoke(this);
    }

    public void DeactivateUI()
    {
        OnDeactivateUI.Invoke(this);
        spaceshipModShopCanvas.enabled = false;
        IsActive = false;
    }
}
