using System;
using System.Data.SqlClient;

namespace canalplus.Models;

class AbonneList
{
    int? idUser;
    string? nomUser;
    string? codeClient;
    string? etat;

    public AbonneList() {}

    public AbonneList(int idUser, string nomUser, string codeClient, string etat) {
        this.idUser = idUser;
        this.nomUser = nomUser;
        this.codeClient = codeClient;
        this.etat = etat;
    }

    public int? IdUser {
        get { return this.idUser; }
        set { this.idUser = value; }
    }

    public string? NomUser {
        get { return this.nomUser; }
        set { this.nomUser = value; }
    }

    public string? CodeClient {
        get { return this.codeClient; }
        set { this.codeClient = value; }
    }

    public string? Etat {
        get { return this.etat; }
        set { this.etat = value; }
    }
}