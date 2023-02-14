using Exercise.ClassLibrary;
using Newtonsoft.Json;
using System.Text;
using MySql.Data.MySqlClient;
class Program
{
    static async Task Main()
    {
        Console.ForegroundColor = ConsoleColor.White;
        using (var client = new HttpClient())
        {
            try
            {
                String connectionString = "Server=10.34.184.125;Port=3306;Database=vas_admin;Uid=vasadmin;Pwd=3edc#EDC;";
                LookupCodeService lookupCodeService=new LookupCodeService(new MySqlConnection(connectionString));
                lookupCodeService.CreateMultiplicationTable();
                while (true)
                {
                    GetMultiplicationTableParameter Multiplicand = new GetMultiplicationTableParameter();
                    Multiplicand.Multiplicand = Convert.ToInt32(Console.ReadLine());
                    var queryParams = JsonConvert.SerializeObject(Multiplicand);
                    var url = "https://localhost:7084/api/MultiplicatissonTable";
                    var response = await client.PostAsync(url, new StringContent(queryParams, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ReturnData<List<GetMultiplicationTableResult>>>(content);
                        if (Multiplicand.Multiplicand == 0)
                        {
                            for (int i = 0; i < 9; i++)
                            {
                                for (int j = 0; j < 4; j++)
                                {
                                    result.Data[j].Multiplications[i].PrintFormula();
                                }
                                Console.WriteLine();
                            }
                            Console.WriteLine();
                            for (int i = 0; i < 9; i++)
                            {
                                for (int j = 4; j < 8; j++)
                                {
                                    result.Data[j].Multiplications[i].PrintFormula();
                                }
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            foreach(var i in result.Data)
                            {
                                foreach(var j in i.Multiplications)
                                {
                                    j.PrintFormula();
                                    Console.WriteLine();
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("The request failed with status code: " + (int)response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The request failed with the following error: " + ex.Message);
            }
        }
    }
}