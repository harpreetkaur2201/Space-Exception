using System;

namespace Space_Expedition
{
    class Program
    {
        static void Main()
        {
            GalacticVault vault = new GalacticVault();
            vault.LoadVault();

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n--- Galactic Vault Menu ---");
                Console.WriteLine("1. Add Artifact");
                Console.WriteLine("2. View Inventory ");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine()?.Trim() ?? "";

                switch (choice)
                {
                    case "1":
                        AddArtifactFromUser(vault);
                        break;
                    case "2":
                        vault.DisplayFullInventory();
                        break;
                    case "3":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static void AddArtifactFromUser(GalacticVault vault)
        {
            if (vault == null) return;

            Console.Write("Enter encoded artifact name (letter+digit): ");
            string encoded = Console.ReadLine()?.Trim() ?? "";

            if (encoded.Length < 2 || !char.IsLetter(encoded[0]) || !char.IsDigit(encoded[1]))
            {
                Console.WriteLine("Invalid encoded artifact format. Try again.");
                return;
            }

            Console.Write("Enter full artifact line (encoded|planet|date|location|desc): ");
            string fullLine = Console.ReadLine()?.Trim() ?? "";

            vault.AddArtifact(encoded, fullLine);
        }
    }
}
