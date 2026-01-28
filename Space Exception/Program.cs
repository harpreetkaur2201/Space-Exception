using System;
using System.IO;
using System.Security.Cryptography;

class SpaceExpedition
{
    
    static string[] encodedNames = new string[80];
    static string[] decodedNames = new string[80];
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
        DisplayDecodedNames();
        SortArtifacts(); 

        Console.WriteLine("\nAfter Sorting:");
        DisplayDecodedNames();
    }
    static void LoadVault()
    {
        try
        {
            string[] lines = File.ReadAllLines("galactic_vault.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim() == "")
                    continue;

                string[] parts = lines[i].Split('|');

                if (parts.Length < 5)
                    continue;

                encodedNames[count] = parts[0].Trim();
                count++;

                if (count == 80)
                    break;
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
        {
            return (char)('Z' - (ch - 'A'));
        }

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
            if (char.IsLetter(encoded[i]))
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
            
            int j = i - 1;

            while (j >= 0 && string.Compare(decodedNames[j], key) > 0)
            {
                decodedNames[j + 1] = decodedNames[j];
                j--;
            }

            decodedNames[j + 1] = keyDecoded;
            encodedNames[j + 1] = keyEncoded;
        }
    }

    static int BinarySearch (string target)
    {
        int left = 0;
        int right = count + 1;

        while (left<= right)
        {
            int mid = (left + right) / 2;

            if (decodedNames[mid] == target)
            {
                return mid;
            }
            else if (string.Compare(decodedNames[mid], target)  > 0)
            {
                right = mid - 1;
            }
            else
            {
                left = mid + 1;
            }
        }
        return -1;
    }
}
