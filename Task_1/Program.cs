using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        string connectionString = ConfigurationManager.ConnectionStrings["CoffeeDB"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Coffee", connection);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Coffee");

            // Task 1: insert, update, delete
            DataTable coffeeTable = dataSet.Tables["Coffee"];
            DataRow newRow = coffeeTable.NewRow();
            newRow["name"] = "Бразильская Либерика";
            newRow["description"] = "Экзотический вкус с нотками тропических фруктов (вишня, клубника, манго).";
            newRow["id_country"] = 1;
            newRow["cost"] = 14.99;
            newRow["weight"] = 600;
            newRow["id_type"] = 3;
            coffeeTable.Rows.Add(newRow);

            DataRow editRow = coffeeTable.Rows[0];
            editRow["name"] = "Бразильская Арабика Резерв";

            DataRow deleteRow = coffeeTable.Rows[7];
            coffeeTable.Rows.Remove(deleteRow);

            adapter.Update(dataSet, "Coffee");
        }
    }
}
