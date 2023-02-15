using Exercise.ClassLibrary;
using Newtonsoft.Json;
using System.Text;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System.Drawing;

class Program
{
    static async Task Main()
    {
        Console.ForegroundColor = ConsoleColor.White;
        LookupCodeService lookupCodeService = new LookupCodeService(new MySqlConnection("Server=10.34.184.125;Port=3306;Database=vas_admin;Uid=vasadmin;Pwd=3edc#EDC;"));
        using (var client = new HttpClient())
        {
            try
            {
                while (true)
                {
                    GetMultiplicationTableParameter Multiplicand = new GetMultiplicationTableParameter();
                    Multiplicand.Multiplicand = Convert.ToInt32(Console.ReadLine());
                    var queryParams = JsonConvert.SerializeObject(Multiplicand);
                    var url = "https://localhost:7084/api/MultiplicationTable";
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
                                    Console.ForegroundColor = lookupCodeService.GetConsoleColor(result.Data[j].Multiplications[i].Product);
                                    result.Data[j].Multiplications[i].PrintFormula();
                                }
                                Console.WriteLine();
                            }
                            Console.WriteLine();
                            for (int i = 0; i < 9; i++)
                            {
                                for (int j = 4; j < 8; j++)
                                {
                                    Console.ForegroundColor = lookupCodeService.GetConsoleColor(result.Data[j].Multiplications[i].Product);
                                    result.Data[j].Multiplications[i].PrintFormula();
                                }
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            foreach (var i in result.Data)
                            {
                                foreach (var j in i.Multiplications)
                                {
                                    Console.ForegroundColor = lookupCodeService.GetConsoleColor(j.Product);
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

