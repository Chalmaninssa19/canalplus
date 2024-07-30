using System;
using System.Data.SqlClient;

namespace canalplus.Models;

class Chaines
{
    int id;
    string? nom;
    double prix;
    double codeReception;

    public Chaines() {}

    public Chaines(int id, string nom, double prix, double codeReception) {
        this.id = id;
        this.nom = nom;
        this.prix = prix;
        this.codeReception = codeReception;
    }

    public int Id {
        get { return this.id; }
        set { this.id = value; }
    }

    public string? Nom {
        get { return this.nom; }
        set { this.nom = value; }
    }

    public double Prix {
        get { return this.prix; }
        set { this.prix = value; }
    }

    public double CodeReception {
        get { return this.codeReception; }
        set { this.codeReception = value; }
    }

    public static List<Chaines> getAllChaines() {   //Recuperer toutes les chaines qui existent
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM chaines",connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        List<Chaines> chaineListes = new List<Chaines>();
        while (reader.Read())
        {
            Chaines temp = new Chaines((int) reader["id"], reader["nom"].ToString(), (double)reader["prix"],  (double)reader["codeReception"]);
            chaineListes.Add(temp);
        }

        reader.Close(); 

        return chaineListes;
    }

    public static Chaines getChaineById(int id) {   //Recuperer la chaine correspondant a l'id
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM chaines where id = " + id,connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        Chaines chaine = new Chaines();
        while (reader.Read())
        {
            chaine.Id = (int) reader["id"];
            chaine.Nom =  reader["nom"].ToString();
            chaine.Prix = (double)reader["prix"];
            chaine.CodeReception =  (double)reader["codeReception"];
        }

        reader.Close(); 

        return chaine;
    }

    public static List<Chaines> getChainesByRegions(Regions region) {   //Recuperer toutes les chaines disponible dans une region
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM chaines where codeReception <= " + region.Signal,connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        List<Chaines> chaineListes = new List<Chaines>();
        while (reader.Read())
        {
            Chaines temp = new Chaines((int) reader["id"], reader["nom"].ToString(), (double)reader["prix"],  (double)reader["codeReception"]);
            chaineListes.Add(temp);
        }

        reader.Close(); 

        return chaineListes;
    }

    public static Chaines getChainesById(int id) {   //Recuperer une chaine grace a son id
        if(id == null) {
            return null;
        }
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM chaines where id = " + id,connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        Chaines chaine = new Chaines();
        while (reader.Read())
        {
            chaine.Id = (int) reader["id"];
            chaine.Nom =  reader["nom"].ToString();
            chaine.Prix =  (double)reader["prix"];
            chaine.CodeReception =  (double)reader["codeReception"];
        }

        reader.Close(); 

        return chaine;
    }

    public static List<Chaines> getChainesIndispo(List<Chaines> chDispos, List<Chaines> allChaines) { //Recuperer les chaines indisponibles
        List<Chaines> chIndispo = new List<Chaines>();
        foreach( Chaines allChaine in allChaines) {
            if(!Chaines.contains(allChaine, chDispos)) {
                chIndispo.Add(allChaine);
            }
        }

        return chIndispo; 
    }

    public static Boolean contains(Chaines chaine, List<Chaines> listChaines) {  //Verifie si la chaine est contenu dans la liste ou pas
        foreach ( Chaines listChaine in listChaines) {
            if ( chaine.Id == listChaine.Id) {
                return true;
            }
        }
        return false;
    }

    public static double calculPrixTotal(List<Chaines> listChaines) {    //Calculer la somme des prix des chaines
        double prixTotal = 0.0;
        foreach(Chaines listChaine in listChaines) {
            prixTotal += listChaine.Prix;
        }
        return prixTotal;
    }
}