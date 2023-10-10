using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using WongmaneeB_ConsumingAPIsII;

public class Program
{
    // ======================== INITIALIZE API AND ENDPOINTS AS BACKING FIELDS ======================== //
    public static SCPDataAPI scpAPI = new SCPDataAPI();
    private static string talesEndpoint = "/data/scp/tales/";
    private static string itemsEndpoint = "/data/scp/items/";
    private static string goiEndpoint = "/data/scp/goi/";
    private static string hubEndpoint = "/data/scp/hubs/";
    private static JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
    public static async Task Main(string[] args)
    {
        // ======================== DRIVER ======================== //

        TypeLine("Welcome to the SCP Gacha Interface, ");
        TypeLine("Anonymous Inquirer.\n");

        Thread.Sleep(300);

        TypeLine("Select your entry vector to initialize the data query.\n");

        Console.Write(Environment.NewLine);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("[1] ");
        Console.ResetColor();
        Console.WriteLine("Item");

        Thread.Sleep(500);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("[2] ");
        Console.ResetColor();
        Console.WriteLine("Tale");

        Thread.Sleep(500);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("[3] ");
        Console.ResetColor();
        Console.WriteLine("GOI");

        Thread.Sleep(500);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("[4] ");
        Console.ResetColor();
        Console.WriteLine("Exit");
        Console.ResetColor();

        Thread.Sleep(500);

        Console.Write(Environment.NewLine);

        string selectedOption;
        int optionIndex = 4; // Placeholder value.
        bool optionInputLoop = true;

        while (optionInputLoop)
        {
            TypeLine("Which option would you like to choose? ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            selectedOption = Console.ReadLine();

            if (Int32.TryParse(selectedOption, out optionIndex)
                && (optionIndex > 0 && optionIndex < 5))
            {
                optionInputLoop = false;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERROR] That was not a valid option.\n");
                Console.ResetColor();
            }
        }

        switch (optionIndex)
        {
            // ======================== SELECTED SCP ITEM ======================== //
            case 1:
                Console.ForegroundColor = ConsoleColor.DarkGray;
                TypeLine("You selected an ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                TypeLine("Item");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                TypeLine(" from the SCP database.\n\n");
                Console.ResetColor();
                TypeLine("Engaging retrieval protocol...");

                // ========== API CALL ========== //
                string itemJson = await scpAPI.SCPCall(itemsEndpoint);

                Console.Clear();

                // ========== DESERIALIZE ========== //
                if (!string.IsNullOrEmpty(itemJson))
                {
                    Dictionary<string, Item> scpItemsDict = JsonSerializer.Deserialize<Dictionary<string, Item>>(itemJson, options);

                    // ========== SERIES SELECT ========== //
                    TypeLine("Please select a Series.\n\n");
                    var seriesList = scpItemsDict.Values.Select(item => item.Series).Distinct();
                    int seriesIndex = 1;
                    foreach (var series in seriesList)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write($"[{seriesIndex}] ");
                        Console.ResetColor();
                        Console.WriteLine(series);
                        seriesIndex++;
                        Thread.Sleep(200);
                    }

                    Console.Write(Environment.NewLine);

                    bool seriesInputLoop = true;
                    string selectedSeries;
                    int selectedSeriesIndex = 1; // Placeholder value.

                    while (seriesInputLoop)
                    {
                        TypeLine("Which series would you like to choose? ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        selectedSeries = Console.ReadLine();
                        Console.ResetColor();

                        if (Int32.TryParse(selectedSeries, out selectedSeriesIndex)
                            && (selectedSeriesIndex > 0 && selectedSeriesIndex <= seriesList.Count()))
                        {
                            seriesInputLoop = false;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[ERROR] That was not a valid option.\n");
                            Console.ResetColor();
                        }
                    }

                    // ========== SERIES FILTER ========== //
                    var seriesFilter = seriesList.ElementAtOrDefault(selectedSeriesIndex - 1);
                    var filteredItems = scpItemsDict.Values.Where(item => item.Series == seriesFilter).ToList();

                    if (filteredItems.Count == 0 || seriesFilter == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[ERROR] No items found for the selected series.");
                        Console.ResetColor();
                        break;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        TypeLine($"Retrieving an SCP from ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        TypeLine(seriesFilter);
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        TypeLine("...\n");
                        Console.ResetColor();

                        Console.Write(Environment.NewLine);

                        Random itemRNG = new Random();
                        int randomItemIndex = itemRNG.Next(filteredItems.Count);
                        Item randomItem = filteredItems[randomItemIndex];

                        int bufferTime = 2000;
                        Thread.Sleep(bufferTime);

                        TypeLine("SCP successfully retrieved.\n");

                        bufferTime = 1000;
                        Thread.Sleep(bufferTime);

                        Console.Write(Environment.NewLine);

                        Console.ForegroundColor = ConsoleColor.Gray;
                        TypeLine("Mash <ANY KEY> to view your item.\n");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();

                        TypeLine(randomItem.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Failed to retrieve the data.");
                }

                break;

            // ======================== SELECTED SCP TALE ======================== //
            case 2:
                Console.ForegroundColor = ConsoleColor.DarkGray;
                TypeLine("You selected a ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                TypeLine("Tale");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                TypeLine(" from the SCP database.\n\n");
                Console.ResetColor();
                TypeLine("Engaging retrieval protocol...\n");

                Console.Write(Environment.NewLine);

                // ========== API CALL ========== //
                string taleJson = await scpAPI.SCPCall(talesEndpoint);

                // ========== DESERIALIZE ========== //
                if (!string.IsNullOrEmpty(taleJson))
                {
                    Dictionary<string, TaleGOI> scpTalesDict = JsonSerializer.Deserialize<Dictionary<string, TaleGOI>>(taleJson, options);

                    if (scpTalesDict.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[ERROR] Could not retrieve tales from the database..");
                        Console.ResetColor();
                        break;
                    }
                    else
                    {
                        Random taleRNG = new Random();
                        int randomIndex = taleRNG.Next(0, scpTalesDict.Count);
                        string randomKey = scpTalesDict.Keys.ElementAt(randomIndex);
                        TaleGOI randomTale = scpTalesDict[randomKey];

                        TypeLine("Tale successfully retrieved.\n");

                        int bufferTime = 1000;
                        Thread.Sleep(bufferTime);

                        Console.Write(Environment.NewLine);

                        Console.ForegroundColor = ConsoleColor.Gray;
                        TypeLine("Mash <ANY KEY> to view your tale.\n");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();

                        TypeLine(randomTale.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Failed to retrieve the data.");
                }

                break;

            // ======================== SELECTED SCP GOI ======================== //
            case 3:
                Console.ForegroundColor = ConsoleColor.DarkGray;
                TypeLine("You selected a ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                TypeLine("Group of Interest");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                TypeLine(" from the SCP database.\n\n");
                Console.ResetColor();
                TypeLine("Engaging retrieval protocol...\n");

                Console.Write(Environment.NewLine);

                // ========== API CALL ========== //
                string goiJson = await scpAPI.SCPCall(goiEndpoint);

                // ========== DESERIALIZE ========== //
                if (!string.IsNullOrEmpty(goiJson))
                {
                    Dictionary<string, TaleGOI> scpGOIDict = JsonSerializer.Deserialize<Dictionary<string, TaleGOI>>(goiJson, options);

                    if (scpGOIDict.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[ERROR] Could not retrieve GOI from the database..");
                        Console.ResetColor();
                        break;
                    }
                    else
                    {
                        Random goiRNG = new Random();
                        int randomIndex = goiRNG.Next(0, scpGOIDict.Count);
                        string randomKey = scpGOIDict.Keys.ElementAt(randomIndex);
                        TaleGOI randomGOI = scpGOIDict[randomKey];

                        TypeLine("Group of Interest successfully retrieved.\n");

                        int bufferTime = 1000;
                        Thread.Sleep(bufferTime);

                        Console.Write(Environment.NewLine);

                        Console.ForegroundColor = ConsoleColor.Gray;
                        TypeLine("Mash <ANY KEY> to view your GOI.");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();

                        TypeLine(randomGOI.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Failed to retrieve the data.");
                }
                break;

            // ======================== EXIT THE PROGRAM ======================== //
            case 4:
                Console.ForegroundColor = ConsoleColor.DarkGray;
                TypeLine("Shutting down interface...");
                Console.ResetColor();
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERROR] There was an error processing your request.");
                Console.ResetColor();
                Console.WriteLine("Press <ANY KEY> to shut down the database.");
                Console.ReadKey();
                Console.Clear();
                break;
        }
    }
    static void TypeLine(string line)
    {
        for (int i = 0; i < line.Length; i++)
        {
            Console.Write(line[i]);
            Thread.Sleep(30);
        }

        Thread.Sleep(200);
    }
}