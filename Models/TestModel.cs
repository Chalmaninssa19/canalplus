using System;
using System.Data.SqlClient;

namespace canalplus.Models;

class TestModel
{
    int age;
    string? nom;

    public TestModel() {}

    public TestModel(int age, string nom) {
        this.age = age;
        this.nom = nom;
    }

    public int Age {
        get { return this.age; }
        set { this.age = value; }
    }

    public string? Nom {
        get { return this.nom; }
        set { this.nom = value; }
    }

    public static List<TestModel> getTest() {
        SqlConnection connection = ConnexionModel.getConnection();
        SqlCommand sql = new SqlCommand("SELECT * FROM test",connection);
        connection.Open();
        SqlDataReader reader = sql.ExecuteReader();

        List<TestModel> testListes = new List<TestModel>();
        while (reader.Read())
        {
            TestModel temp = new TestModel((int) reader["age"], reader["nom"].ToString());
            testListes.Add(temp);
        }

        reader.Close();
        //TestModel[] results = chaineListes.ToArray();
        return testListes;
    }
}