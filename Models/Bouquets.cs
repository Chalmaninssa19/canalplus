using System;
using System.Data.SqlClient;

namespace canalplus.Models;

class Bouquets
{
    int id;
    string? nom;
    double remise;
    Chaines chain;

    public Bouquets() {}

    public Bouquets(int id, string nom, double remise, Chaines chain) {
        this.id = id;
        this.nom = nom;
        this.remise = remise;
        this.chain = chain;
    }

    public int Id {
        get { return this.id; }
        set { this.id = value; }
    }

    public string? Nom {
        get { return this.nom; }
        set { this.nom = value; }
    }

    public double Remise {
        get { return this.remise; }
        set { this.remise = value; }
    }

    public Chaines Chain {
        get { return this.chain; }
        set { this.chain = value; }
    }

    public static Bouquets getBouquetById(int id) {   //Recuperer un bouquet grace a son id
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM bouquets where id = " + id,connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        Bouquets bouquet = new Bouquets();

        while (reader.Read())
        {
            Chaines chaines = Chaines.getChainesById((int)reader["idChaine"]);
            bouquet.Id = (int) reader["id"];
            bouquet.Nom =  reader["nom"].ToString();
            bouquet.Remise = (double)reader["reduction"];
            bouquet.Chain = chaines;
        }

        reader.Close(); 

        return bouquet;
    }
    
    public static List<Bouquets> getAllBouquets() { //Recuperer toutes les bouquets
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM bouquets",connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        List<Bouquets> bouquetListes = new List<Bouquets>();
        while (reader.Read())
        {
            Chaines chaine = Chaines.getChainesById((int)reader["idChaine"]);
            Bouquets temp = new Bouquets((int) reader["id"], reader["nom"].ToString(), (double)reader["reduction"], chaine);
            bouquetListes.Add(temp); 
        }

        reader.Close(); 

        return bouquetListes;
    }

    public static List<Bouquets> getBouquetsByRegions(Regions region) {   //Recuperer toutes les noms du bouquets disponible dans une region
        List<Chaines> chDispo = Chaines.getChainesByRegions(region); //Toutes les chaines disponibles d'une region
        List<Bouquets> allBouquets = Bouquets.getAllBouquets(); //Toutes les bouquets
        List<Chaines> allCh = Chaines.getAllChaines();  //Toutes les chaines
        List<Chaines> chIndispos = Chaines.getChainesIndispo(chDispo, allCh);    //Les chaines indisponibles du region
        List<Bouquets> bouqIndispos = Bouquets.getBouquetsIndispos(allBouquets, chIndispos); //Toutes les bouquets indisponibles
    
        return getBouquetsDispos(allBouquets, bouqIndispos);
    }

    public static List<Bouquets> getBouquetsIndispos( List<Bouquets> allBouquets, List<Chaines> chIndispos) {   //Avoir les bouquets indisponibles
        List<Bouquets> bouquetIndispo = new List<Bouquets>();
        foreach(Bouquets bouq in allBouquets) {
            List<Chaines> chaines = Bouquets.getChainesInBouq(bouq.Nom);
            foreach(Chaines chIndispo in chIndispos) {
                if(Chaines.contains(chIndispo, chaines)) {
                    if(!Bouquets.contains(bouq, bouquetIndispo)) {
                        bouquetIndispo.Add(bouq);
                    }
                }
            }
        }

        return bouquetIndispo;
    } 

    public static Boolean contains(Bouquets bouquet, List<Bouquets> listBouquet) { //Verifie si le bouquet est dans liste des bouquets ou pas
        foreach ( Bouquets bouq in listBouquet) {
            if ( bouquet.Nom == bouq.Nom) {
                return true;
            }
        }
        return false;
    }

    public static List<Bouquets> getBouquetsDispos( List<Bouquets> allBouquets, List<Bouquets> bouqIndispos) {  //Recuperer les bouquets disponibles
        List<Bouquets> bouquetDispos = new List<Bouquets>();
        foreach(Bouquets allBouquet in allBouquets) {
            if(!Bouquets.contains(allBouquet, bouqIndispos)) {
                if(!Bouquets.contains(allBouquet,bouquetDispos)) {
                    bouquetDispos.Add(allBouquet);
                }
            }
        }

        return bouquetDispos;
    }

    public static List<Chaines> getChainesInBouq(String nom) {   //Recuperer toutes les chaines d'un bouquet
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM bouquets where nom = '" + nom + "'",connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        List<Chaines> chaines = new List<Chaines>();
        Bouquets bouquet = new  Bouquets();
        while (reader.Read())
        {
            chaines.Add(Chaines.getChainesById((int)reader["idChaine"]));
        }

        reader.Close(); 

        return chaines;
    }

    public static Bouquets getBouquetByNom(string nom) {   //Recuperer un bouquet grace a son nom
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM bouquets where nom = '" + nom + "'",connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        Bouquets bouquet = new Bouquets();

        while (reader.Read())
        {
            Chaines chaines = Chaines.getChainesById((int)reader["idChaine"]);
            bouquet.Id = (int) reader["id"];
            bouquet.Nom =  reader["nom"].ToString();
            bouquet.Remise = (double)reader["reduction"];
            bouquet.Chain = chaines;
        }

        reader.Close(); 

        return bouquet;
    }
}