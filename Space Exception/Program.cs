using System;
using System.IO;

class SpaceExpedition
{
    static string[] encodedNames = new string[80];
    static string[] decodedNames = new string[80];
    static string[] fullData = new string[80];
    static int count = 0;

    static char[] original = {
        'A','B','C','D','E','F','G','H','I','J','K','L','M',
        'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
    };

    static char[] mapped = {
        'H','Z','A','U','Y','E','K','G','O','T','I','R','J',
        'V','W','N','M','F','Q','S','D','B','X','L','C','P'
    };

    static void Main()
    {
        LoadVault();
        DecodeAllNames();
        SortArtifacts();
        ShowMenu();
    }

    static void LoadVault()
    {
        if (!File.Exists("galactic_vault.txt"))
        {
            Console.WriteLine("Vault file not found! Starting with empty inventory.");
            return;
        }

        try
        {
            string[] lines = File.ReadAllLines("galactic_vault.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line == "")
                    continue;

                string[] parts = line.Split('|');

                if (parts.Length < 5)
                    continue;

                encodedNames[count] = parts[0].Trim();
                fullData[count] = line; // store full line
                count++;

                if (count >= 80)
                {
                    Console.WriteLine("Vault reached maximum capacity of 80 artifacts.");
                    break;
                }
            }
        }
        catch
        {
            Console.WriteLine("Error reading galactic_vault.txt");
        }
    }

    static void DecodeAllNames()
    {
        for (int i = 0; i < count; i++)
        {
            decodedNames[i] = DecodeName(encodedNames[i]);
        }
    }

    static char DecodeChar(char ch, int level)
    {
        if (level == 0)
            return (char)('Z' - (ch - 'A'));

        char next = ch;
        for (int i = 0; i < 26; i++)
        {
            if (original[i] == ch)
            {
                next = mapped[i];
                break;
            }
        }
        return DecodeChar(next, level - 1);
    }

    static string DecodeName(string encoded)
    {
        string result = "";
        for (int i = 0; i < encoded.Length; i++)
        {
            if (char.IsLetter(encoded[i]) && i + 1 < encoded.Length && char.IsDigit(encoded[i + 1]))
            {
                char letter = encoded[i];
                int level = encoded[i + 1] - '0';
                result += DecodeChar(letter, level);
                i++;
            }
        }
        return result;
    }

    static void DisplayDecodedNames()
    {
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine(encodedNames[i] + " -> " + decodedNames[i]);
        }
    }

    static void SortArtifacts()
    {
        for (int i = 1; i < count; i++)
        {
            string keyDecoded = decodedNames[i];
            string keyEncoded = encodedNames[i];
            string keyFull = fullData[i];

            int j = i - 1;
            while (j >= 0 && string.Compare(decodedNames[j], keyDecoded) > 0)
            {
                decodedNames[j + 1] = decodedNames[j];
                encodedNames[j + 1] = encodedNames[j];
                fullData[j + 1] = fullData[j];
                j--;
            }

            decodedNames[j + 1] = keyDecoded;
            encodedNames[j + 1] = keyEncoded;
            fullData[j + 1] = keyFull;
        }
    }

    static int BinarySearch(string target)
    {
        int left = 0;
        int right = count - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            if (decodedNames[mid] == target)
                return mid;
            else if (string.Compare(decodedNames[mid], target) > 0)
                right = mid - 1;
            else
                left = mid + 1;
        }
        return -1;
    }

    static void InsertArtifact(string newEncoded, string newDecoded, string newFullLine)
    {
        if (count >= 80)
        {
            Console.WriteLine("Cannot add more artifacts. Vault is full.");
            return;
        }

        int position = 0;
        while (position < count && string.Compare(decodedNames[position], newDecoded) < 0)
            position++;

        for (int i = count; i > position; i--)
        {
            encodedNames[i] = encodedNames[i - 1];
            decodedNames[i] = decodedNames[i - 1];
            fullData[i] = fullData[i - 1];
        }

        encodedNames[position] = newEncoded;
        decodedNames[position] = newDecoded;
        fullData[position] = newFullLine;
        count++;
    }

    static void ShowMenu()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- Galactic Vault Menu ---");
            Console.WriteLine("1. Add Artifact");
            Console.WriteLine("2. View Inventory");
            Console.WriteLine("3. Save and Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine()?.Trim() ?? "";

            switch (choice)
            {
                case "1":
                    AddArtifactFromUser();
                    break;
                case "2":
                    DisplayDecodedNames();
                    break;
                case "3":
                    SaveAndExit();
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    static void AddArtifactFromUser()
    {
        if (count >= 80)
        {
            Console.WriteLine("Cannot add more artifacts. Vault is full.");
            return;
        }

        Console.Write("Enter encoded artifact name (letter+digit): ");
        string encoded = Console.ReadLine()?.Trim() ?? "";

        if (encoded.Length < 2 || !char.IsLetter(encoded[0]) || !char.IsDigit(encoded[1]))
        {
            Console.WriteLine("Invalid encoded artifact format. Try again.");
            return;
        }

        Console.Write("Enter full artifact line (encoded|planet|date|location|desc): ");
        string fullLine = Console.ReadLine()?.Trim() ?? "";

        string decoded = DecodeName(encoded);

        if (BinarySearch(decoded) != -1)
        {
            Console.WriteLine("Artifact already exists in the vault.");
            return;
        }

        InsertArtifact(encoded, decoded, fullLine);
        Console.WriteLine("Artifact added successfully.");
    }

    static void SaveAndExit()
    {
        try
        {
            using (StreamWriter writer = new StreamWriter("expedition_summary.txt"))
            {
                for (int i = 0; i < count; i++)
                    writer.WriteLine(fullData[i]);
            }
            Console.WriteLine("Data saved successfully. Exiting program.");
        }
        catch
        {
            Console.WriteLine("Error saving file.");
        }
    }
}
