using System;
using System.Data.SqlClient;

namespace canalplus.Models;

class Regions
{
    int id;
    string? nom;
    double signal;

    public Regions() {}

    public Regions(int id, string nom, double signal) {
        this.id = id;
        this.nom = nom;
        this.signal = signal;
    }

    public int Id {
        get { return this.id; }
        set { this.id = value; }
    }

    public string? Nom {
        get { return this.nom; }
        set { this.nom = value; }
    }

    public double Signal {
        get { return this.signal; }
        set { this.signal = value; }
    }

    public static List<Regions> getAllRegions() {   //Recuperer tous les regions
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM regions",connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        List<Regions> regionListes = new List<Regions>();
        while (reader.Read())
        {
            Regions temp = new Regions((int) reader["id"], reader["nom"].ToString(), (double)reader["signal"]);
            regionListes.Add(temp);
        }

        reader.Close(); 

        return regionListes;
    }

     public static Regions getRegionById(int id) {   //Recuperer le signal de reception du region par id rentre
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM regions where id = " + id,connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        Regions region = new Regions();
        while (reader.Read())
        {
            region.Id = (int) reader["id"];
            region.Nom =  reader["nom"].ToString();
            region.Signal =  (double)reader["signal"];
        }

        reader.Close(); 

        return region;
    }
}