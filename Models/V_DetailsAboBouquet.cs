using System;
using System.Data.SqlClient;

namespace canalplus.Models;

class V_DetailsAboBouquet
{
    int? idUser;
    double? prixPaye;
    DateTime? dateDebut;
    DateTime? dateFin;
    string? nomBouquet;
    string? nomTypeAbo;
    string? nomUser;
    string? nomRegion;
    string? nomCodeClient;

    public V_DetailsAboBouquet() {}

    public V_DetailsAboBouquet(int idUser, double prixPaye, DateTime dateDebut, DateTime dateFin, string nomBouquet, string nomTypeAbo, string nomUser, string nomRegion, string codeClient) {
        this.idUser = idUser; 
        this.prixPaye = prixPaye;
        this.dateDebut = dateDebut;
        this.dateFin = dateFin;
        this.nomBouquet = nomBouquet;
        this.nomTypeAbo = nomTypeAbo;
        this.nomUser = nomUser;
        this.nomRegion = nomRegion;
        this.nomCodeClient = codeClient;

    }

    public int? IdUser {
        get { return this.idUser; }
        set { this.idUser = value; }
    }

    public double? PrixPaye {
        get { return this.prixPaye; }
        set { this.prixPaye = value; }
    }

    public DateTime? DateDebut {
        get { return this.dateDebut; }
        set { this.dateDebut = value; }
    }

    public DateTime? DateFin {
        get { return this.dateFin; }
        set { this.dateFin = value; }
    }

    public string NomBouquet {
        get { return this.nomBouquet; }
        set { this.nomBouquet = value; }
    }

    public string NomTypeAbo {
        get { return this.nomTypeAbo; }
        set { this.nomTypeAbo = value; }
    }

    public string NomUser {
        get { return this.nomUser; }
        set { this.nomUser = value; }
    }

    public string NomRegion {
        get { return this.nomRegion; }
        set { this.nomRegion = value; }
    }

    public string CodeClient {
        get { return this.nomCodeClient; }
        set { this.nomCodeClient = value; }
    } 

    public static List<V_DetailsAboBouquet> getDetailsAboBouquetByClient(Users user) {   //Details abonnement d'un client
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM v_detailsAboBouquet where idUser = " + user.Id,connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        List<V_DetailsAboBouquet> details = new List<V_DetailsAboBouquet>();

        while (reader.Read())
        {
            V_DetailsAboBouquet temp = new V_DetailsAboBouquet((int)reader["idUser"],(double)reader["prixPaye"],(DateTime)reader["dateDebut"],(DateTime)reader["dateFin"],reader["nomBouquet"].ToString(),reader["nomTypeAbo"].ToString(),reader["nomUser"].ToString(),reader["nomRegion"].ToString(),reader["mdp"].ToString());
            details.Add(temp);
        }

        reader.Close(); 

        return details;
    }

    public static string isClientAboBouquet(Users user, DateTime dateFin) {   //Est ce qu'un client est abonne ajourd'hui
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT top(1)[nomUser] FROM v_detailsAboBouquet where idUser = " + user.Id + " and dateFin >= '" + dateFin + "' order by dateFin desc",connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();
        if(reader.Read()) {
            reader.Close();
            return "abonne bouquet";
        }
        else {
            reader.Close();
            return "desabonne bouquet";
        }

    }
}