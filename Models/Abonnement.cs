using System;
using System.Data.SqlClient;

namespace canalplus.Models;

class Abonnement
{
    int idAbo;
    Users user;
    TypeAbonnement typeAbo;
    double prixPaye;
    DateTime dateDebut;
    DateTime dateFin;

    public Abonnement() {}

    public Abonnement(int idAbo, Users user, TypeAbonnement typeAbo, double prixPaye, DateTime dateDebut, DateTime dateFin) {
        this.idAbo = idAbo;
        this.user = user;
        this.typeAbo = typeAbo;
        this.prixPaye = prixPaye;
        this.dateDebut = dateDebut;
        this.dateFin = dateFin;
    }
    public Abonnement(Users user, TypeAbonnement typeAbo, double prixPaye, DateTime dateDebut, DateTime dateFin) {
        this.user = user;
        this.typeAbo = typeAbo;
        this.prixPaye = prixPaye;
        this.dateDebut = dateDebut;
        this.dateFin = dateFin;
    }

    public int IdAbo {
        get { return this.idAbo; }
        set { this.idAbo = value; }
    }

    public Users User {
        get { return this.user; }
        set { this.user = value; }
    }

    public TypeAbonnement TypeAbo {
        get { return this.typeAbo; }
        set { this.typeAbo = value; }
    }

    public double PrixPaye {
        get { return this.prixPaye; }
        set { this.prixPaye = value; }
    }

    public DateTime DateDebut {
        get { return this.dateDebut; }
        set { this.dateDebut = value; }
    }

    public DateTime DateFin {
        get { return this.dateFin; }
        set { this.dateFin = value; }
    }

    public static void abonne( Abonnement abo, int idref) {   //S'abooner sur le site
        if(idref == 0 ) {
            SqlConnection connection = ConnexionModel.getConnection();
            string dateFin = "(SELECT DATEADD(day, "+abo.TypeAbo.Duree+", GETDATE()))";
            string query = "INSERT INTO abonnement values (" + abo.User.Id + ", " + abo.TypeAbo.Id + ", " + abo.PrixPaye + ", GETDATE(), " + dateFin + ")";
            System.Console.WriteLine("sql-> " + query);
            SqlCommand sql = new SqlCommand(query,connection);
            connection.Open();
            SqlDataReader reader = sql.ExecuteReader();
        }
        else {
             SqlConnection connection = ConnexionModel.getConnection();
            string dateFin = "(SELECT DATEADD(day, "+abo.TypeAbo.Duree+", '" + abo.DateFin.ToString("yyyy-MM-dd HH:mm:ss") + "'))";
            string query = "INSERT INTO abonnement values (" + abo.User.Id + ", " + abo.TypeAbo.Id + ", " + abo.PrixPaye + ", GETDATE(), " + dateFin + ")";
            System.Console.WriteLine("sql-> " + query);
            SqlCommand sql = new SqlCommand(query,connection);
            connection.Open();
            SqlDataReader reader = sql.ExecuteReader();
        }
    }

    public static Abonnement getAbonnementByClient(Users client) {  //recuperer l'abonnement d'un client
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM abonnement where idUser = " + client.Id,connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        Abonnement aboClient = new Abonnement();
        while (reader.Read())
        {
            aboClient.IdAbo = (int) reader["idAbonnement"];
            Users user = Users.getClientById((int) reader["idUser"]);
            aboClient.User = user;
            TypeAbonnement typeAbo = TypeAbonnement.getTypeAbonnementById((int) reader["idTypeAbonnement"]);
            aboClient.TypeAbo = typeAbo;
            aboClient.PrixPaye = (double) reader["prixPaye"];
            aboClient.DateDebut = (DateTime) reader["dateDebut"];
            aboClient.DateFin = (DateTime) reader["dateFin"];
        }

        reader.Close(); 

        return aboClient;
    }

    public static Abonnement getAbonnementByDate(Users user) {  //recuperer l'abonnement d'un client par la date de son abonnement 
        SqlConnection connection = ConnexionModel.getConnection();
        string query = "SELECT top(1)[idAbonnement],[idUser],[idTypeAbonnement],[prixPaye],[dateDebut],[dateFin] FROM abonnement where idUser = " + user.Id;
        SqlCommand sql = new SqlCommand(query,connection);
        System.Console.WriteLine("zay -> " + query);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        Abonnement aboClient = new Abonnement();
        while (reader.Read())
        {
            aboClient.IdAbo = (int) reader["idAbonnement"];
            Users user1 = Users.getClientById((int) reader["idUser"]);
            aboClient.User = user1;
            TypeAbonnement typeAbo = TypeAbonnement.getTypeAbonnementById((int) reader["idTypeAbonnement"]);
            aboClient.TypeAbo = typeAbo;
            aboClient.PrixPaye = (double) reader["prixPaye"];
            aboClient.DateDebut = (DateTime) reader["dateDebut"];
            aboClient.DateFin = (DateTime) reader["dateFin"];
        }

        reader.Close(); 

        return aboClient;
    }

    public static Abonnement getAbonnementByIdAbo(int idAbo) {  //recuperer l'abonnement d'un client par le id 
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM abonnement where idAbonnement = " + idAbo,connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        Abonnement aboClient = new Abonnement();
        while (reader.Read())
        {
            aboClient.IdAbo = (int) reader["idAbonnement"];
            Users user = Users.getClientById((int) reader["idUser"]);
            aboClient.User = user;
            TypeAbonnement typeAbo = TypeAbonnement.getTypeAbonnementById((int) reader["idTypeAbonnement"]);
            aboClient.TypeAbo = typeAbo;
            aboClient.PrixPaye = (double) reader["prixPaye"];
            aboClient.DateDebut = (DateTime) reader["dateDebut"];
            aboClient.DateFin = (DateTime) reader["dateFin"];
        }

        reader.Close(); 

        return aboClient;
    }

    public static Abonnement lastAbonnement(Users client, DateTime timeNow) {  //recuperer le dernier abonnement d'un client
        SqlConnection connection = ConnexionModel.getConnection();
        string query = "SELECT top(1)[idAbonnement],[idUser],[idTypeAbonnement],[prixPaye],[dateDebut],[dateFin] FROM abonnement where idUser = " + client.Id + " and dateFin >= GETDATE() order by dateFin desc";
        SqlCommand sql = new SqlCommand(query,connection);
        System.Console.WriteLine("query -> " + query);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        Abonnement aboClient = new Abonnement();
        while (reader.Read())
        {
            aboClient.IdAbo = (int) reader["idAbonnement"];
            Users user = Users.getClientById((int) reader["idUser"]);
            aboClient.User = user;
            TypeAbonnement typeAbo = TypeAbonnement.getTypeAbonnementById((int) reader["idTypeAbonnement"]);
            aboClient.TypeAbo = typeAbo;
            aboClient.PrixPaye = (double) reader["prixPaye"];
            aboClient.DateDebut = (DateTime) reader["dateDebut"];
            aboClient.DateFin = (DateTime) reader["dateFin"];
        }

        reader.Close(); 

        return aboClient;
    }
}