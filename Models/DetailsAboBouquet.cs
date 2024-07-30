using System;
using System.Data.SqlClient;

namespace canalplus.Models;

class DetailsAboBouquet
{
    Users user;
    Abonnement abonnement;
    Bouquets? bouquet;

    public DetailsAboBouquet() {}

    public DetailsAboBouquet(Users user, Abonnement abo, Bouquets bouquet) {
        this.user = user;
        this.abonnement = abo;
        this.bouquet = bouquet;
    }

    public Users User {
        get { return this.user; }
        set { this.user = value; }
    }

    public Abonnement Abo {
        get { return this.abonnement; }
        set { this.abonnement = value; }
    }

    public Bouquets? Bouquet {
        get { return this.bouquet; }
        set { this.bouquet = value; }
    }

    public static List<DetailsAboBouquet> getDetailsAboByUser(Users user) {   //Recuperer les details abonnement par un client
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM detailsAboBouquet where idUser = " + user.Id,connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        List<DetailsAboBouquet> details = new List<DetailsAboBouquet>();
        while (reader.Read())
        {
            Users user1 = Users.getClientById((int) reader["idUser"]);
            Abonnement abo = Abonnement.getAbonnementByIdAbo((int) reader["idAbonnement"]);
            Bouquets bouquet = Bouquets.getBouquetById((int)reader["idAbonnement"]);
            DetailsAboBouquet temp = new DetailsAboBouquet(user1, abo, bouquet);
            details.Add(temp);
        }

        reader.Close(); 

        return details;
    }

    public static void createDetailsAboBouq( Users user, Abonnement abo, Bouquets bouq) {   //Creer le details abonnmenet
        SqlConnection connection = ConnexionModel.getConnection();
        string query = "INSERT INTO detailsAboBouquet values (" + user.Id + ", " + abo.IdAbo + ", " + bouq.Id + ")";
        SqlCommand sql = new SqlCommand(query,connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();
    }
}