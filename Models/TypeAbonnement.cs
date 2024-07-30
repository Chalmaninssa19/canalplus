using System;
using System.Data.SqlClient;

namespace canalplus.Models;

class TypeAbonnement
{
    int id;
    string nom;
    int duree;

    public TypeAbonnement() {}

    public TypeAbonnement(int id, string nom, int duree) {
        this.id = id;
        this.nom = nom;
        this.duree = duree;
    }

    public int Id {
        get { return this.id; }
        set { this.id = value; }
    }

    public string Nom {
        get { return this.nom; }
        set { this.nom = value; }
    }

    public int Duree {
        get { return this.duree; }
        set { this.duree = value; }
    }

    public static TypeAbonnement getTypeAbonnementById(int idTypeAbonnement) {   //Recuperer un typeAbonnement par son id
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM typeAbonnement where idTypeAbonnement = " + idTypeAbonnement,connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        TypeAbonnement typeAbo = new TypeAbonnement();
        while (reader.Read())
        {
            typeAbo.Id = (int) reader["idTypeAbonnement"];
            typeAbo.Nom = reader["nom"].ToString();
            typeAbo.Duree = (int) reader["duree"];
        }

        reader.Close(); 

        return typeAbo;
    }
}