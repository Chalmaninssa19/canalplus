using System;
using System.Data.SqlClient;

namespace canalplus.Models;

class V_BouqDispo
{
    int? id;
    int? idBouquet;
    string? nomBouquet;
    double? prixBouquet;

    public V_BouqDispo() {}

    public V_BouqDispo(int id, int idBouquet, string nomBouquet, double prixBouquet) {
        this.id = id;
        this.idBouquet = idBouquet;
        this.nomBouquet = nomBouquet;
        this.prixBouquet = prixBouquet;
    }

    public int? Id {
        get { return this.id; }
        set { this.id = value; }
    }

    public int? IdBouquet {
        get { return this.id; }
        set { this.id = value; }
    }

    public string? NomBouquet {
        get { return this.nomBouquet; }
        set { this.nomBouquet = value; }
    }

    public double? PrixBouquet {
        get { return this.prixBouquet; }
        set { this.prixBouquet = value; }
    }
}