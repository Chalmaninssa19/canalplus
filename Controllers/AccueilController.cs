using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using canalplus.Models;

namespace canalplus.Controllers;

public class AccueilController : Controller
{
    private readonly ILogger<AccueilController> _logger;

    public AccueilController(ILogger<AccueilController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult welcome()
    {
        List<Regions> datas = Regions.getAllRegions();
        ViewBag.Datas = datas;
        return View("pages/welcome");
    }

    public IActionResult Action()
    {       
        int id = (int)TempData["donnees"];
        Regions region = Regions.getRegionById(id);
        List<Chaines> chDispos = Chaines.getChainesByRegions(region);
        List<Bouquets> bouqDispos = Bouquets.getBouquetsByRegions(region);
 
        List<V_BouqDispo> v_bouqDispos = V_ChainesInBouq.getPrixParBouquets(bouqDispos); 
        ViewBag.Region = region;
        ViewBag.Datas = chDispos;
        ViewBag.Bouquets = v_bouqDispos;

        // Traiter les données
        //return RedirectToAction("Details", new { name = name, age = age });
        //return RedirectToAction("accueil", new { nom = nom});
        return View("pages/accueil");
    }

    [HttpGet]
    public IActionResult Details(string nom)
    {
        // Traiter les données
        List<V_ChainesInBouq> details = V_ChainesInBouq.getVBouquetByNom(nom);
        ViewBag.Details = details;
        ViewBag.Nom = nom;
        ViewBag.PrixTotal = V_ChainesInBouq.getPrixBouquet(nom);
        ViewBag.Remise = details[0].Remise;

        return View("pages/details");
    }
 
    public IActionResult accueil(string nom) {
        ViewBag.Nom = nom;
        return View("pages/accueil");
    }

    [HttpPost]
    public IActionResult choice() {
        ViewBag.TypeAbo = Request.Form["typeAbo"];
        int type = Int32.Parse(Request.Form["typeAbo"]);
        if (type == 1) {
            string chaine = Request.Form["chaine"];
            string [] lists = chaine.Split(",");
            List<Chaines> chainesChoice = new List<Chaines>();
            foreach(string list in lists) {
                chainesChoice.Add(Chaines.getChaineById(Int32.Parse(list)));
            }
            double prixTotal = Chaines.calculPrixTotal(chainesChoice);
            if(lists.Length >= 5) {
                ViewBag.AuLieu = prixTotal;
                double remise = lists.Length - 5;
                ViewBag.Remise = remise;
                prixTotal = prixTotal -(prixTotal*(remise/100));
            }
            ViewBag.PrixAPayer = prixTotal;
            ViewBag.Choix = chaine;
        }
        if(type == 2) {
            string bouquet = Request.Form["bouquet"];
            string prixPaye = Request.Form["prixPaye"];
            string [] prixTab = prixPaye.Split(",");
            double sum = Util.sum(prixTab);
            ViewBag.PrixAPayer = sum;
            ViewBag.Choix = bouquet;
        }

        return View("pages/abonnement");
    }

    public IActionResult identify() {   //List des clients dans la vue identification
        List<Users> datas = Users.getAllClient();
        ViewBag.Datas = datas;
        return View("pages/identification");
    }

    public IActionResult login() {  //S'identifier
        int idU = Int32.Parse(Request.Form["user"]);
        Users user = Users.getClientById(idU);

        // Récupérer l'objet ISession à partir du contexte HTTP
        ISession session = HttpContext.Session;

        // Ajouter une chaîne à la session
        session.SetInt32("idClient", user.Id);

        if(user.IsAdmin == 0) {
            Regions region = Regions.getRegionById(user.Region.Id);
            TempData["donnees"] = region.Id;

            return RedirectToAction("Action");
        }
        return RedirectToAction("listAbonne");
    }

    public IActionResult listAbonne() {
        List<Users> users = Users.getAllClient();
        List<AbonneList> aboneeListChaine = new List<AbonneList>();
        List<AbonneList> aboneeListBouquet = new List<AbonneList>();
        foreach(Users user in users) {
            AbonneList abonne = new AbonneList(user.Id, user.Nom, user.Mdp, V_DetailsAboChaine.isClientAboChaine(user, DateTime.Now));
            aboneeListChaine.Add(abonne);
        }
        foreach(Users user in users) {
            AbonneList abonne = new AbonneList(user.Id, user.Nom, user.Mdp, V_DetailsAboBouquet.isClientAboBouquet(user, DateTime.Now));
            aboneeListBouquet.Add(abonne);
        }
        ViewBag.UsersChaine = aboneeListChaine;
        ViewBag.UsersBouq = aboneeListBouquet;

        return View("pages/listAbonne");
    }

    public IActionResult detailsAbo(string idUser, string type) {
        int id = Int32.Parse(idUser);
        int typer = Int32.Parse(type);
        Users user =  Users.getClientById(id);
        ViewBag.Client = user;
        if(typer == 0) { //Abonnement par chaine
            List<V_DetailsAboChaine> detailsAboChaine = V_DetailsAboChaine.getDetailsAboChaineByClient(user);
            ViewBag.DetailsAboChaines = detailsAboChaine;

            return View("pages/detailsAbonneChaine");
        }
         
        //Abonnement par bouquet
            List<V_DetailsAboBouquet> detailsAboBouquet = V_DetailsAboBouquet.getDetailsAboBouquetByClient(user);
            ViewBag.DetailsAboBouq = detailsAboBouquet;

        return View("pages/detailsAbonneBouquet");
    }

    public IActionResult inscrire() {
        List<Regions> datas = Regions.getAllRegions();
        ViewBag.Datas = datas;
        return View("pages/inscription");
    }

    [HttpPost]
    public IActionResult inscription() {    //inscription d'un nouveau client
        string nom = Request.Form["nom"];
        string mdp = Request.Form["mdp"];
        string idRegion = Request.Form["region"];
        Regions region = Regions.getRegionById(Int32.Parse(idRegion));
        Users user1 = new Users(nom, mdp, region, 0);
        Users.createNewClient(user1);
        Users user = Users.getClientByCode(mdp);
        // Récupérer l'objet ISession à partir du contexte HTTP
        ISession session = HttpContext.Session;

        // Ajouter une chaîne à la session
        session.SetInt32("idClient", user.Id);
        TempData["donnees"] = region.Id;

        return RedirectToAction("Action");
    }

    public IActionResult abonne(string prixPaye, string typeAbo, string choix) { //Confirmation de l'abonnement d'un client
        int idClient = (int)HttpContext.Session.GetInt32("idClient");
        Users user = Users.getClientById(idClient);
        double prixPayer = double.Parse(prixPaye);
        int typeAb = Int32.Parse(typeAbo);
        DateTime dateDebut = DateTime.Now;
        string choisi = choix;
        if(typeAb == 1) {
            if(V_DetailsAboChaine.isClientAboChaine(user,dateDebut) == "abonne chaine") {
                System.Console.WriteLine("tafiditra");
                Abonnement lastAbonnement = Abonnement.lastAbonnement(user,dateDebut);
                TypeAbonnement typeAbonne = TypeAbonnement.getTypeAbonnementById(0);
                DateTime dateFin = lastAbonnement.DateFin;
                Abonnement abonnement = new Abonnement(user,typeAbonne,prixPayer,dateDebut,dateFin);
                Abonnement.abonne(abonnement, 1);
                Abonnement abon = Abonnement.getAbonnementByDate(user);
                string [] lists = choisi.Split(",");
                List<Chaines> chainesChoice = new List<Chaines>();
                foreach(string list in lists) {
                    chainesChoice.Add(Chaines.getChaineById(Int32.Parse(list)));
                }
                foreach(Chaines chaineChoice in chainesChoice) {
                    DetailsAboChaine.createDetailsAbo(user,abon,chaineChoice);
                } 
            }
            else {
                TypeAbonnement typeAbonne = TypeAbonnement.getTypeAbonnementById(0);
                DateTime dateFin = dateDebut;
                Abonnement abonnement = new Abonnement(user,typeAbonne,prixPayer,dateDebut,dateFin);
                Abonnement.abonne(abonnement, 0);
                Abonnement abon = Abonnement.getAbonnementByDate(user);
                string [] lists = choisi.Split(",");
                List<Chaines> chainesChoice = new List<Chaines>();
                foreach(string list in lists) {
                    chainesChoice.Add(Chaines.getChaineById(Int32.Parse(list)));
                }
                foreach(Chaines chaineChoice in chainesChoice) {
                    DetailsAboChaine.createDetailsAbo(user,abon,chaineChoice);
                }  
            }
        }
        if(typeAb == 2) {
            if(V_DetailsAboBouquet.isClientAboBouquet(user,dateDebut) == "abonne chaine") {
                Abonnement lastAbonnement = Abonnement.lastAbonnement(user,dateDebut);
                TypeAbonnement typeAbonne = TypeAbonnement.getTypeAbonnementById(1);
                DateTime dateFin = lastAbonnement.DateFin;
                Abonnement abonnement = new Abonnement(user,typeAbonne,prixPayer,dateDebut,dateFin);
                Abonnement.abonne(abonnement, 1);
                Abonnement abon = Abonnement.getAbonnementByDate(user);
                string [] lists = choisi.Split(",");
                List<Bouquets> bouqChoices = new List<Bouquets>();
                foreach(string list in lists) {
                    bouqChoices.Add(Bouquets.getBouquetById(Int32.Parse(list)));
                }
                foreach(Bouquets bouqChoice in bouqChoices) {
                    DetailsAboBouquet.createDetailsAboBouq(user,abon,bouqChoice);
                }  
            }
            else {
                TypeAbonnement typeAbonne = TypeAbonnement.getTypeAbonnementById(1);
                DateTime dateFin = dateDebut;
                Abonnement abonnement = new Abonnement(user,typeAbonne,prixPayer,dateDebut,dateFin);
                Abonnement.abonne(abonnement, 0);
                Abonnement abon = Abonnement.getAbonnementByDate(user);
                string [] lists = choisi.Split(",");
                List<Bouquets> bouqChoices = new List<Bouquets>();
                foreach(string list in lists) {
                    bouqChoices.Add(Bouquets.getBouquetById(Int32.Parse(list)));
                }
                foreach(Bouquets bouqChoice in bouqChoices) {
                    DetailsAboBouquet.createDetailsAboBouq(user,abon,bouqChoice);
                }  
            } 
        }

        return View("./pages/test");
   } 
}