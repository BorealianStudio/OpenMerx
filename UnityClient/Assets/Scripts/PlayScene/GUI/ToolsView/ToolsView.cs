using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ToolsView : MonoBehaviour {

    [SerializeField] CanvasGroup menuView = null;
    [SerializeField] ToolButton buttonPrefab = null;
    [SerializeField] Transform buttonGrid = null;

    private WindowSystem sys = null;
    private PrefabManager pref = null;

    private ToolButton mailButton = null;

    private List<ToolButton> _buttonNeedingCorp = new List<ToolButton>();

    private void Start() {
        sys = FindObjectOfType<WindowSystem>();
        pref = FindObjectOfType<PrefabManager>();

        LocalDataManager.instance.OnCorporationChange += OnCorpChange;

        ToolButton corp = Instantiate(buttonPrefab, buttonGrid);
        corp.SetTitle("Corp");
        corp.GetComponent<Button>().onClick.AddListener(() => OnShowCorpClic());

        ToolButton ships = Instantiate(buttonPrefab, buttonGrid);
        ships.SetTitle("Ships");
        ships.GetComponent<Button>().onClick.AddListener(() => OnMyShipClic());
        _buttonNeedingCorp.Add(ships);

        ToolButton stations = Instantiate(buttonPrefab, buttonGrid);
        stations.SetTitle("Stations");
        stations.GetComponent<Button>().onClick.AddListener(() => OnStationsClic());
        _buttonNeedingCorp.Add(stations);

        ToolButton market = Instantiate(buttonPrefab, buttonGrid);
        market.SetTitle("Market");
        market.GetComponent<Button>().onClick.AddListener(() => OnMarketClic());
        _buttonNeedingCorp.Add(market);

        ToolButton bookmarks = Instantiate(buttonPrefab, buttonGrid);
        bookmarks.SetTitle("Places");
        bookmarks.GetComponent<Button>().onClick.AddListener(() => OnBookmarkClic());
        _buttonNeedingCorp.Add(bookmarks);

        ToolButton plans = Instantiate(buttonPrefab, buttonGrid);
        plans.SetTitle("Plans");
        plans.GetComponent<Button>().onClick.AddListener(() => OnPlanClick());
        _buttonNeedingCorp.Add(plans);

        mailButton = Instantiate(buttonPrefab, buttonGrid);
        mailButton.SetTitle("Mails");
        mailButton.GetComponent<Button>().onClick.AddListener(() => OnMailsClic());
        LocalDataManager.instance.OnMailboxChange += (m) => { mailButton.SetBlinking(true); };
        _buttonNeedingCorp.Add(mailButton);

        ToolButton options = Instantiate(buttonPrefab, buttonGrid);
        options.SetTitle("Menu");
        options.GetComponent<Button>().onClick.AddListener(() => OnMenuClic());        

        ToolButton console = Instantiate(buttonPrefab, buttonGrid);
        console.SetTitle("Console");
        console.GetComponent<Button>().onClick.AddListener(() => OnConsoleClic());

        OnCorpChange(null);
    }

    private void OnDestroy() {
        LocalDataManager.instance.OnCorporationChange -= OnCorpChange;
    }

    private void OnCorpChange(Corporation c) {
        bool enable = LocalDataManager.instance.LocalCorporation != null;

        foreach(ToolButton b in _buttonNeedingCorp) {
            b.Button.interactable = enable;
        }
    }

    private void OnShowCorpClic() {        

        CorporationView view = Instantiate<CorporationView>(pref.prefabCorporationView);        
        Window w = sys.NewWindow("corporationView", view.gameObject);        
        w.Title = "Corporation";
        Character c = LocalDataManager.instance.GetCharacterInfo(LocalDataManager.instance.LocalCharacterID);

        view.SetCorpID(c.Corp);
        w.Show();
    }

    private void OnMyShipClic() {
        ShipList view = Instantiate<ShipList>(pref.prefabShipListView);

        Window w = sys.NewWindow("myShipList", view.gameObject);
        w.Title = "Vaisseaux";
        w.Show();
    }

    private void OnStationsClic() {
        StationListView view = Instantiate(pref.prefabStationListView);

        Window w = sys.NewWindow("stations", view.gameObject);
        w.Title = "Stations";
        w.Show();
    }

    private void OnMarketClic() {
        MarketView view = Instantiate(pref.prefabMarketView);

        Window w = sys.NewWindow("Market", view.gameObject);
        w.Title = "Market";
        w.Show();
    }

    private void OnBookmarkClic() {
        BookmarksView view = Instantiate(pref.prefabBookmarksView);

        Window w = sys.NewWindow("Bookmarks", view.gameObject);
        w.Title = "Bookmarks";
        w.Show();
    }


    private void OnPlanClick() {
        MyFlightPlansView view = Instantiate(pref.prefabMyFlightPlanView);

        Window w = sys.NewWindow("MyFlighPlans", view.gameObject);
        w.Title = "Flight plans";

        view.SetCorporation(LocalDataManager.instance.LocalCorporation);
        w.Show();
    }

    private void OnMailsClic() {
        mailButton.SetBlinking(false);
        MailsView view = Instantiate(pref.prefabMailsView);

        Window w = sys.NewWindow("Mails", view.gameObject);
        w.Title = "Mails";

        view.SetMailbox(LocalDataManager.instance.Mailbox);
        w.Show();
    }

    private void OnMenuClic() {
        menuView.gameObject.SetActive(true);
        menuView.alpha = 1.0f;
        menuView.interactable = true;
        menuView.blocksRaycasts = true;
    }

    private void OnConsoleClic() {
        ConsoleView view = Instantiate(pref.prefabConsoleView);

        Window w = sys.NewWindow("Console", view.gameObject);
        w.Title = "Console";
        w.Show();
    }
}
