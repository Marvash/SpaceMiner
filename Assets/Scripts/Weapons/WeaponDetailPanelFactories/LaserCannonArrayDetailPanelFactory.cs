using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannonArrayDetailPanelFactory : IWeaponDetailPanelFactory
{
    public GameObject PanelPrefab { get; private set; }
    public LaserCannonArrayConfigSO LaserCannonArrayConfig { get; private set; }

    public LaserCannonArrayDetailPanelFactory(GameObject panelPrefab, LaserCannonArrayConfigSO config) {
        PanelPrefab = panelPrefab;
        LaserCannonArrayConfig = config;
    }

    public AWeaponDetailUI InstantiateWeaponDetailPanel()
    {
        GameObject detailPanel = GameObject.Instantiate(PanelPrefab);
        LaserCannonArrayDetailUI detailUI = detailPanel.GetComponent<LaserCannonArrayDetailUI>();
        detailUI.PopulateDetailPanel(LaserCannonArrayConfig, LaserCannonArrayConfig.CurrentUnlockedWeaponLevel);
        return detailUI;
    }

    public AWeaponDetailUI InstantiateWeaponDetailPanel(int weaponLevel)
    {
        GameObject detailPanel = GameObject.Instantiate(PanelPrefab);
        LaserCannonArrayDetailUI detailUI = detailPanel.GetComponent<LaserCannonArrayDetailUI>();
        detailUI.PopulateDetailPanel(LaserCannonArrayConfig, weaponLevel);
        return detailUI;
    }
}
