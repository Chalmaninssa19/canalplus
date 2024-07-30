using System;
using System.Data.SqlClient;

namespace canalplus.Models;

class DetailsAboChaine
{
    Users user;
    Abonnement abonnement;
    Chaines? chaine;

    public DetailsAboChaine() {}

    public DetailsAboChaine(Users user, Abonnement abo, Chaines chaine) {
        this.user = user;
        this.abonnement = abo;
        this.chaine = chaine;
    }

    public Users User {
        get { return this.user; }
        set { this.user = value; }
    }

    public Abonnement Abo {
        get { return this.abonnement; }
        set { this.abonnement = value; }
    }

    public Chaines? Chaine {
        get { return this.chaine; }
        set { this.chaine = value; }
    }

    public static List<DetailsAboChaine> getDetailsAboByUser(Users user) {   //Recuperer les details abonnement par un client
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM detailsAboBouquet where idUser = " + user.Id,connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        List<DetailsAboChaine> details = new List<DetailsAboChaine>();
        while (reader.Read())
        {
            Users user1 = Users.getClientById((int) reader["idUser"]);
            Abonnement abo = Abonnement.getAbonnementByIdAbo((int) reader["idAbonnement"]);
            Chaines chaine = Chaines.getChainesById((int)reader["idAbonnement"]);
            DetailsAboChaine temp = new DetailsAboChaine(user1, abo, chaine);
            details.Add(temp);
        }

        reader.Close(); 

        return details;
    }

    public static void createDetailsAbo( Users user, Abonnement abo, Chaines chaine) {   //Creer le details abonnmenet
        SqlConnection connection = ConnexionModel.getConnection();
        string query = "INSERT INTO detailsAboChaine values (" + user.Id + ", " + abo.IdAbo + ", " + chaine.Id + ")";
        SqlCommand sql = new SqlCommand(query,connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();
    }
}