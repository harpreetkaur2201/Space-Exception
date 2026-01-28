using System;
using System.IO;

class SpaceExpedition
{
    static void Main()
    {
        // Arrays to store artifact data (maximum 80)
        string[] encodedNames = new string[80];
        string[] planets = new string[80];
        string[] discoveryDates = new string[80];
        string[] storageLocations = new string[80];
        string[] descriptions = new string[80];

        int count = 0; // number of artifacts loaded

        try
        {
            string[] lines = File.ReadAllLines("galactic_vault.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                // skip empty lines
                if (lines[i].Trim() == "")
                {
                    continue;
                }

                string[] parts = lines[i].Split('|');

                // skip wrong lines
                if (parts.Length < 5)
                {
                    continue;
                }

                encodedNames[count] = parts[0].Trim();
                planets[count] = parts[1].Trim();
                discoveryDates[count] = parts[2].Trim();
                storageLocations[count] = parts[3].Trim();
                descriptions[count] = parts[4].Trim();

                count++;

                if (count == 80)
                {
                    break;
                }
            }

            Console.WriteLine("Artifacts loaded: " + count);
        }
        catch
        {
            Console.WriteLine("Error: Could not read galactic_vault.txt");
        }

        // simple check (optional)
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine(encodedNames[i] + " | " + planets[i]);
        }
    }
}
