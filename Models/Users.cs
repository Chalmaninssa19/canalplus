using System;
using System.Data.SqlClient;

namespace canalplus.Models;

class Users
{
    int id;
    string nom;
    string mdp;
    Regions region;
    int isAdmin;

    public Users() {}

    public Users(int id, string nom, string mdp, Regions region, int isAdmin) {
        this.id = id;
        this.nom = nom;
        this.mdp = mdp;
        this.region = region;
        this.isAdmin = isAdmin;
    }

    public Users(string nom, string mdp, Regions region, int isAdmin) {
        this.nom = nom;
        this.mdp = mdp;
        this.region = region;
        this.isAdmin = isAdmin;
    }

    public int Id {
        get { return this.id; }
        set { this.id = value; }
    }

    public string Nom {
        get { return this.nom; }
        set { this.nom = value; }
    }


    public string Mdp {
        get { return this.mdp; }
        set { this.mdp = value; }
    }

    public Regions Region {
        get { return this.region; }
        set { this.region = value; }
    }

    public int IsAdmin {
        get { return this.isAdmin; }
        set { this.isAdmin = value; }
    }

    public static List<Users> getAllClient() {   //Recuperer toutes les clients
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM users",connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        List<Users> clientLists = new List<Users>();
        while (reader.Read())
        {
            Regions region = Regions.getRegionById((int) reader["idRegion"]);
            Users temp = new Users((int) reader["idUser"], reader["nom"].ToString(), reader["mdp"].ToString(),region, (int)reader["isAdmin"]);
            clientLists.Add(temp);
        }

        reader.Close(); 

        return clientLists;
    }
    
    public static Users getClientById(int idUser) {   //Recuperer un user par son id
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM users where idUser = " + idUser,connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        Users client = new Users();
        while (reader.Read())
        {
            client.Id = (int) reader["idUser"];
            client.Nom = reader["nom"].ToString();
            client.Mdp =  reader["mdp"].ToString();
            Regions region = Regions.getRegionById( (int) reader["idRegion"]); 
            client.Region = region;
            client.IsAdmin = (int) reader["isAdmin"];
        }

        reader.Close(); 

        return client;
    }

    public static void createNewClient(Users newClient) {   //Creer un nouveau client
        SqlConnection connection = ConnexionModel.getConnection();
        string query = "INSERT INTO users VALUES ('" + newClient.Nom + "', '" + newClient.Mdp +"'," + newClient.Region.Id +"," + newClient.IsAdmin + " )";
        SqlCommand sql = new SqlCommand(query,connection);
        System.Console.WriteLine("io -> " + query);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();
        reader.Close(); 
    }

    public static Users getClientByCode(string codeClient) {   //Recuperer un user par son code client
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM users where mdp = '" + codeClient + "'",connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        Users client = new Users();
        while (reader.Read())
        {
            client.Id = (int) reader["idUser"];
            client.Nom = reader["nom"].ToString();
            client.Mdp =  reader["mdp"].ToString();
            Regions region = Regions.getRegionById( (int) reader["idRegion"]); 
            client.Region = region;
            client.IsAdmin = 0;
        }

        reader.Close(); 

        return client;
    }

}