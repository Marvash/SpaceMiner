using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherArrayDetailPanelFactory : IWeaponDetailPanelFactory
{
    public GameObject PanelPrefab { get; private set; }
    public RocketLauncherArrayConfigSO RocketLauncherArrayConfig { get; private set; }

    public RocketLauncherArrayDetailPanelFactory(GameObject panelPrefab, RocketLauncherArrayConfigSO config) {
        PanelPrefab = panelPrefab;
        RocketLauncherArrayConfig = config;
    }

    public AWeaponDetailUI InstantiateWeaponDetailPanel()
    {
        GameObject detailPanel = GameObject.Instantiate(PanelPrefab);
        RocketLauncherArrayDetailUI detailUI = detailPanel.GetComponent<RocketLauncherArrayDetailUI>();
        detailUI.PopulateDetailPanel(RocketLauncherArrayConfig, RocketLauncherArrayConfig.CurrentUnlockedWeaponLevel);
        return detailUI;
    }

    public AWeaponDetailUI InstantiateWeaponDetailPanel(int weaponLevel)
    {
        GameObject detailPanel = GameObject.Instantiate(PanelPrefab);
        RocketLauncherArrayDetailUI detailUI = detailPanel.GetComponent<RocketLauncherArrayDetailUI>();
        detailUI.PopulateDetailPanel(RocketLauncherArrayConfig, weaponLevel);
        return detailUI;
    }
}
