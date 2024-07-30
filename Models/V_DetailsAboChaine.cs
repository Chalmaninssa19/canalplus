using System;
using System.Data.SqlClient;

namespace canalplus.Models;

class V_DetailsAboChaine
{
    int? idUser;
    double? prixPaye;
    DateTime? dateDebut;
    DateTime? dateFin;
    string? nomUser;
    string? codeClient;
    string? nomChaine;
    string? nomRegion;
    string? nomTypeAbo;

    public V_DetailsAboChaine() {}

    public V_DetailsAboChaine(int idUser, double prixPaye, DateTime dateDebut, DateTime dateFin, string nomUser, string codeClient, string nomChaine, string nomRegion, string momTypeAbo) {
        this.idUser = idUser;
        this.prixPaye = prixPaye;
        this.dateDebut = dateDebut;
        this.dateFin = dateFin;
        this.nomUser = nomUser;
        this.codeClient = codeClient;
        this.nomChaine = nomChaine;
        this.nomRegion = nomRegion;
        this.nomTypeAbo = nomTypeAbo;

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

    public string NomUser {
        get { return this.nomUser; }
        set { this.nomUser = value; }
    }

    public string CodeClient {
        get { return this.codeClient; }
        set { this.codeClient = value; }
    }

    public string NomChaine {
        get { return this.nomChaine; }
        set { this.nomChaine = value; }
    }

    public string NomRegion {
        get { return this.nomRegion; }
        set { this.nomRegion = value; }
    }

    public string NomTypeAbo {
        get { return this.nomTypeAbo; }
        set { this.nomTypeAbo = value; }
    }

    public static List<V_DetailsAboChaine> getDetailsAboChaineByClient(Users user) {   //Recuperer un bouquet grace a son nom
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM v_detailsAboChaine where idUser = " + user.Id,connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        List<V_DetailsAboChaine> details = new List<V_DetailsAboChaine>();

        while (reader.Read())
        {
            V_DetailsAboChaine temp = new V_DetailsAboChaine((int)reader["idUser"],(double)reader["prixPaye"],(DateTime)reader["dateDebut"],(DateTime)reader["dateFin"],reader["nomUser"].ToString(),reader["mdp"].ToString(),reader["nomChaine"].ToString(),reader["nomRegion"].ToString(),reader["nomTypeAbo"].ToString());
            details.Add(temp);
        }

        reader.Close(); 

        return details;
    }

    public static string isClientAboChaine(Users user, DateTime dateFin) {   //Est ce qu'un client est abonne ajourd'hui
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT top(1)[nomUser] FROM v_detailsAboChaine where idUser = " + user.Id + " and dateFin >= '" + dateFin.ToString("yyyy-MM-dd HH:mm:ss") + "' order by dateFin desc",connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();
        if( reader.Read()) {
            reader.Close();
            return "abonne chaine";
        }
        else {
            reader.Close();
            return "desabonne chaine";
        }
    }

   /* public static boolean isClientAboChaine(Users user, DateTime dateFin) {   //Est ce qu'un client est abonne ajourd'hui
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT top(1)[nomUser] FROM v_detailsAboChaine where idUser = " + user.Id + " and dateFin >= '" + dateFin + "' order by dateFin desc",connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();
        if( reader.Read()) {
            reader.Close();
            return "abonne chaine";
        }
        else {
            reader.Close();
            return "desabonne chaine";
        }
    }*/
}