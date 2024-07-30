using System;
using System.Data.SqlClient;

namespace canalplus.Models;

class V_ChainesInBouq
{
    int id;
    int idChaine;
    string nomBouquet;
    double remise;
    string nomChaine;
    double prixInitial;
    double codeReception;
    double prixReduit; 

    public V_ChainesInBouq() {}

    public V_ChainesInBouq(int id, int idChaine, string nomBouquet, double remise, string nomChaine, double prixInitial, double codeReception, double prixReduit) {
        this.id = id;
        this.idChaine = idChaine;
        this.nomBouquet = nomBouquet;
        this.remise = remise;
        this.nomChaine = nomChaine;
        this.prixInitial = prixInitial;
        this.codeReception = codeReception;
        this.prixReduit = prixReduit;
    }

    public int Id {
        get { return this.id; }
        set { this.id = value; }
    }

    public int IdChaine {
        get { return this.idChaine; }
        set { this.idChaine = value; }
    }
    public string NomBouquet {
        get { return this.nomBouquet; }
        set { this.nomBouquet = value; }
    }

    public double Remise {
        get { return this.remise; }
        set { this.remise = value; }
    }

    public string NomChaine {
        get { return this.nomChaine; }
        set { this.nomChaine = value; }
    }

    public double PrixInitial {
        get { return this.prixInitial; }
        set { this.prixInitial = value; }
    }

    public double CodeReception {
        get { return this.codeReception; }
        set { this.codeReception = value; }
    }

    public double PrixReduit {
        get { return this.prixReduit; }
        set { this.prixReduit = value; }
    }

    public static List<V_ChainesInBouq> getVBouquetByNom(String nomBouquet) { //Recuperer la relation entre bouquet et les chaines
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM v_ChainesInBouq where nomBouquet = '" + nomBouquet + "'",connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        List<V_ChainesInBouq> bouquetListes = new List<V_ChainesInBouq>();
        while (reader.Read())
        {
            V_ChainesInBouq temp = new V_ChainesInBouq((int) reader["id"], (int) reader["idChaine"],reader["nomBouquet"].ToString(), (double)reader["reduction"],reader["nomChaine"].ToString(),(double)reader["prix"],(double)reader["codeReception"], (double)reader["prixReduit"]);
            bouquetListes.Add(temp); 
        }

        reader.Close(); 

        return bouquetListes;
    }

    public static double getPrixBouquet(String nomBouquet) { //Avoir le prix du bouquet entrer
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("select sum(prixReduit) as prixBouq from v_ChainesInBouq where nomBouquet = '" + nomBouquet + "'",connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        double prixBouq = 0.0;
        while (reader.Read())
        {
            prixBouq = (double)reader["prixBouq"];
        }

        reader.Close(); 

        return prixBouq;
    }

    public static List<V_BouqDispo> getPrixParBouquets(List<Bouquets> listBouquets) { //Recuperer tous les prix par bouquets
        List<V_BouqDispo> bouqDispo = new List<V_BouqDispo>();
        int i = 1;
        foreach(Bouquets bouq in listBouquets ) {
            bouqDispo.Add(new V_BouqDispo(i, bouq.Id, bouq.Nom, getPrixBouquet(bouq.Nom)));
            i++;
        }
        return bouqDispo;
    }

}