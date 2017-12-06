using System.Collections.Generic;

using UnityEngine;

public class TutorialManager : MonoBehaviour {

    private TutorialView _view = null;
    private Dictionary<string, string> _tutoParams = null;


    private void Awake() {
        _view = FindObjectOfType<TutorialView>();
        _view.Hide();


        string str = PlayerPrefs.GetString("tutoData");
        if (str != "") {
            _tutoParams = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(str);
        } else {
            _tutoParams = new Dictionary<string, string>();
        }
    }

    public static void RaiseEvent(string eventName) {
        FindObjectOfType<TutorialManager>().PrivateRaiseEvent(eventName);
    }

    private void PrivateRaiseEvent(string eventName) {
        switch (eventName) {
            case "gameStart": Init(); return;
            case "CorpoNeedCreate": CorpoNeedCreate(); return;
            case "StationListShow": StationListShow(); return;
            case "ShipView": ShipView();return;
            case "ShipViewPlaning": ShipViewPlaning(); return;
            case "stationLineShowDetailNoHangar": stationLineShowDetailNoHangar(); return;
        }
    }

    public void ResetTuto() {
        PlayerPrefs.DeleteAll();
        _tutoParams = new Dictionary<string, string>();
        WriteToPref();
    }

    private void WriteToPref() {
        string s = Newtonsoft.Json.JsonConvert.SerializeObject(_tutoParams);
        PlayerPrefs.SetString("tutoData", s);
    }

    private void CorpoNeedCreate() {
        if (_tutoParams.ContainsKey("corpoView"))
            return;

        _view.SetTextButton1("Continuer");
        _view.SetButton1View(true);
        _view.SetButton2View(false);
        _view.SetText("Vous ne possédez pas encore de société, pour cette raison l'interface de corporation vous propose d'en créer une. Entrez les informations nécessaires et appuyez sur le bouton créer");
        _view.Show();

        _view.OnButton1Clic += CorpoViewClic1;
        WriteToPref();

    }

    private void CorpoViewClic1() {

        _view.SetTextButton1("Fermer");
        _view.SetButton1View(true);
        _view.SetButton2View(false);
        _view.SetText("Choisir une station pour votre quartier général vous permet d'y obtenir un hangar gratuit. De plus vous y recevrez un vaisseau. Ouvrez la vue Stations une fois votre corporation créée.");
        _view.Show();
        _tutoParams.Add("corpoView", "done");

        _view.OnButton1Clic -= CorpoViewClic1;
        _view.OnButton1Clic += CorpoViewClic2;
        WriteToPref();

    }

    private void CorpoViewClic2() {
        _view.OnButton1Clic -= CorpoViewClic2;
        _view.Hide();
    }

    private void Init() {
        if (_tutoParams.ContainsKey("init"))
            return;

        _view.Show();
        _view.SetButton1View(true);
        _view.SetTextButton1("Continue");
        _view.SetButton2View(true);
        _view.SetTextButton2("Abandonner");
        _view.SetText("Bonjour et bienvenue dans SpaceSim! Si vous n'avez pas besoin du tutorial cliquez sur abandonner, sinon appuyez sur continuer");
        _view.OnButton1Clic += InitButton1Clic;
        _view.OnButton2Clic += InitButton2Clic;
    }

    private void InitButton1Clic() {
        _view.OnButton1Clic -= InitButton1Clic;
        _view.OnButton2Clic -= InitButton2Clic;
        WriteToPref();
        _view.Hide();
        Presentation();
    }

    private void InitButton2Clic() {
        _view.OnButton1Clic -= InitButton1Clic;
        _view.OnButton2Clic -= InitButton2Clic;
        _tutoParams["init"] = "abort";
        WriteToPref();
        _view.Hide();
    }

    private void Presentation() {
        _view.Show();
        _view.SetButton1View(true);
        _view.SetButton2View(false);
        _view.SetTextButton1("Continue");
        _view.OnButton1Clic += PresentationButton1Clic;
        _view.SetText("SpaceSim est un jeu où vous jouez le rôle d'un investisseur dans un univers de science-fiction. Vous aurez plusieurs méthodes pour générer des revenus, elles vous seront expliquées dans la suite du tutoriel.");
    }

    private void PresentationButton1Clic() {
        _view.OnButton1Clic -= PresentationButton1Clic;
        WriteToPref();
        _view.Hide();
        CreateHQ();
    }

    private void CreateHQ() {
        _view.Show();
        _view.SetButton1View(true);
        _view.SetTextButton1("Fermer");
        _view.SetButton2View(false);
        _view.SetText("Afin de démarrer votre aventure, vous devez choisir dans quelle station vous allez installer votre quartier général. Pour se faire, ouvrez la fenêtre de corporation en appuyant sur le bouton corporation.");
        _view.OnButton1Clic += CreateHQButton1;
    }

    private void CreateHQButton1() {
        _view.OnButton1Clic += CreateHQButton1;
        if (!_tutoParams.ContainsKey("init"))
            _tutoParams.Add("init", "done");
        else
            _tutoParams["init"] = "done";

        WriteToPref();
        _view.Hide();
    }

    private void StationListShow() {
        if (_tutoParams.ContainsKey("StationListShow"))
            return;

        _view.Show();
        _view.SetButton1View(true);
        _view.SetTextButton1("Suivant");
        _view.SetButton2View(false);
        _view.SetText("Vous venez d'afficher la liste des stations. Vous pouvez y voir toutes les stations connues. Les filtres en haut de la fenêtre permettent de réduire le nombre de station affichée.");
        _view.OnButton1Clic += StationListShow2;

    }

    private void StationListShow2() {
        _view.Show();
        _view.SetButton1View(true);
        _view.SetTextButton1("Fermer");
        _view.SetButton2View(false);
        _view.SetText("Chaque ligne vous donne des informations sur la station telle que son nom. Si vous possédez un hangar dans une station, la ligne sera agrémentée d'une icône de hangar. Cliquer sur ce dernier vous amènera à la vue détaillée du hangar.");
        _view.OnButton1Clic -= StationListShow2;
        _view.OnButton1Clic += StationListShow3;
        if (!_tutoParams.ContainsKey("StationListShow"))
            _tutoParams.Add("StationListShow", "done");
        WriteToPref();
    }

    private void StationListShow3() {
        _view.Hide();
        _view.OnButton1Clic -= StationListShow3;
    }

    private void ShipView() {
        if (_tutoParams.ContainsKey("ShipView"))
            return;

        _view.Show();
        _view.SetButton1View(true);
        _view.SetTextButton1("Fermer");
        _view.SetButton2View(false);
        _view.SetText("Bienvenue sur la vue vaisseau. Ici vous trouverez toutes les informations liées à un vaisseau en particulier. Les informations sont regroupées sous différent onglets. Le 1er onglet contient les informations générales du vaisseau. Cliquez sur un autre onglet pour voir la suite des informations.");
        _view.OnButton1Clic += ShipView2;
    }

    private void ShipView2() {
        if (!_tutoParams.ContainsKey("ShipView"))
            _tutoParams.Add("ShipView", "done");
        WriteToPref();


        _view.OnButton1Clic -= ShipView2;
        _view.Hide();
    }

    private void ShipViewPlaning() {                                     
        if (_tutoParams.ContainsKey("ShipViewPlaning"))
            return;

        _view.Show();
        _view.SetButton1View(true);
        _view.SetTextButton1("Fermer");
        _view.SetButton2View(false);
        _view.SetText("La vue de plan de vol permet de consulter le plan de vol en cours. Un vaisseau à l’arrêt ne possèdera aucune information à ce niveau, ajoutez le à une flotte pour lui assigner un plan de vol.");
        _view.OnButton1Clic += ShipViewPlaning2;
    }

    private void ShipViewPlaning2() {
        if (!_tutoParams.ContainsKey("ShipViewPlaning"))
            _tutoParams.Add("ShipViewPlaning", "done");
        WriteToPref();

        _view.OnButton1Clic -= ShipViewPlaning2;
        _view.Hide();
    }
        
    private void stationLineShowDetailNoHangar() {
        if (_tutoParams.ContainsKey("stationLineShowDetailNoHangar"))
            return;

        _view.Show();
        _view.SetButton1View(true);
        _view.SetTextButton1("Fermer");
        _view.SetButton2View(false);
        _view.SetText("Vous venez d'afficher les détails d'une station ou vous n'avez pas de hangar, vous pouvez donc y acheter un hangar avec le bouton. Obtenir un hangar est gratuit mail il vous en coutera un montant périodique en frais de location.");
        _view.OnButton1Clic += stationLineShowDetailNoHanga2;
    }

    private void stationLineShowDetailNoHanga2() {
        if (!_tutoParams.ContainsKey("stationLineShowDetailNoHangar"))
            _tutoParams.Add("stationLineShowDetailNoHangar", "done");
        WriteToPref();

        _view.OnButton1Clic -= stationLineShowDetailNoHanga2;
        _view.Hide();
    }

}
