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

            // Task 2
            DataTable coffeeTable = dataSet.Tables["Coffee"];

            var cherryCoffee = coffeeTable.AsEnumerable()
                .Where(r => r.Field<string>("description")
                .Contains("вишня"));

            Console.WriteLine("Кава, в описі якої зустрічається вишня:");
            foreach (var row in cherryCoffee)
                Console.WriteLine(
                    $"Назва: {row.Field<string>("name")}\n" +
                    $"Опис: {row.Field<string>("description")}"
                );

            var costRangeCoffee = coffeeTable.AsEnumerable()
                .Where(r => r.Field<decimal>("cost") >= 10
                && r.Field<decimal>("cost") <= 20);

            Console.WriteLine("\nКава з собівартістю у вказаному діапазоні:");
            foreach (var row in costRangeCoffee)
                Console.WriteLine(
                    $"Назва: {row.Field<string>("name"),-28}" +
                    $"Ціна: {row.Field<decimal>("cost"),-10}"
                );

            var weightRangeCoffee = coffeeTable.AsEnumerable()
                .Where(r => r.Field<double>("weight") >= 200 
                && r.Field<double>("weight") <= 500);

            Console.WriteLine("\nКава з кількістю грамів у вказаному діапазоні:");
            foreach (var row in weightRangeCoffee)
                Console.WriteLine(
                    $"Назва: {row.Field<string>("name"),-28}" +
                    $"Вага: {row.Field<double>("weight"),-10}"
                );

            var specificCountryCoffee = coffeeTable.AsEnumerable()
                .Where(r => r.Field<int>("id_country") == 1 
                || r.Field<int>("id_country") == 2);

            Console.WriteLine("\nКава із зазначених країн:");
            foreach (var row in specificCountryCoffee)
                Console.WriteLine(
                    $"Назва: {row.Field<string>("name"),-28}" +
                    $"ID країни: {row.Field<int>("id_country"),-10}"
                );
        }
    }
}
