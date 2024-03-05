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
            SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT * FROM Coffee " +
                "JOIN Country ON Coffee.id_country = Country.id_country " +
                "JOIN CoffeeType ON Coffee.id_type = CoffeeType.id_type",
                connection
            );
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
           
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Coffee");

            // Task 4
            DataTable coffeeTable = dataSet.Tables["Coffee"];

            var top3CountriesByCoffeeVariety = coffeeTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("country_name"))
                .Select(g => new { Country = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(3);

            Console.WriteLine("Топ-3 країн за кількістю сортів кави:");
            foreach (var item in top3CountriesByCoffeeVariety)
                Console.WriteLine(
                    $"Країна: {item.Country,-10}" +
                    $"Кількість сортів кави: {item.Count,-10}"
                );

            var top3CountriesByCoffeeWeight = coffeeTable.AsEnumerable()
                .GroupBy(r => r.Field<string>("country_name"))
                .Select(g => new { Country = g.Key, TotalWeight = g.Sum(r => r.Field<double>("weight")) })
                .OrderByDescending(g => g.TotalWeight)
                .Take(3);

            Console.WriteLine("\nТоп-3 країн за кількістю грамів кави:");
            foreach (var item in top3CountriesByCoffeeWeight)
                Console.WriteLine(
                    $"Країна: {item.Country,-10}" +
                    $"Загальна кількість грамів: {item.TotalWeight,-10}"
                );

            Console.WriteLine();

            string[] coffeeTypes = { "Арабика", "Робуста", "Купаж/бленд" };
            foreach (var coffeeType in coffeeTypes)
            {
                var top3CoffeeByWeight = coffeeTable.AsEnumerable()
                    .Where(r => r.Field<string>("type_name") == coffeeType)
                    .OrderByDescending(r => r.Field<double>("weight"))
                    .Take(3);

                Console.WriteLine($"\nТоп-3 види кави \"{coffeeType}\" за кількістю грамів:");
                foreach (var row in top3CoffeeByWeight)
                    Console.WriteLine(
                        $"Назва: {row.Field<string>("name"),-28}" +
                        $"Вага: {row.Field<double>("weight"),-10}"
                    );
            }
            Console.WriteLine();

            var allCoffeeTypes = coffeeTable.AsEnumerable()
                .Select(r => r.Field<string>("type_name"))
                .Distinct();

            foreach (var coffeeType in allCoffeeTypes)
            {
                var top3CoffeeByWeight = coffeeTable.AsEnumerable()
                    .Where(r => r.Field<string>("type_name") == coffeeType)
                    .OrderByDescending(r => r.Field<double>("weight"))
                    .Take(3);

                Console.WriteLine($"\nТоп-3 види кави \"{coffeeType}\" за кількістю грамів:");
                foreach (var row in top3CoffeeByWeight)
                    Console.WriteLine(
                        $"Назва: {row.Field<string>("name"),-28}" + 
                        $"Вага: {row.Field<double>("weight"),-10}"
                    );
            }
        }
    }
}
