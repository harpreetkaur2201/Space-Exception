using System;
using System.IO;

namespace Space_Expedition
{
    internal class GalacticVault
    {
        private Artifact[] artifacts = new Artifact[80];
        private int count = 0;

        public void LoadVault()
        {
            if (!File.Exists("galactic_vault.txt"))
            {
                Console.WriteLine("Vault file not found! Starting empty.");
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines("galactic_vault.txt");
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length < 5) continue;

                    AddArtifact(parts[0], line, false);
                }
            }
            catch
            {
                Console.WriteLine("Error reading galactic_vault.txt");
            }
        }

        public void AddArtifact(string encoded, string fullLine, bool showMessage = true)
        {
            if (count >= 80)
            {
                if (showMessage) Console.WriteLine("Vault full. Cannot add more artifacts.");
                return;
            }

            Artifact art = new Artifact(encoded, fullLine);

            for (int i = 0; i < count; i++)
                if (artifacts[i].DecodedName == art.DecodedName)
                {
                    if (showMessage) Console.WriteLine("Artifact already exists.");
                    return;
                }

            int pos = 0;
            while (pos < count && string.Compare(artifacts[pos].DecodedName, art.DecodedName) < 0)
                pos++;

            for (int i = count; i > pos; i--)
                artifacts[i] = artifacts[i - 1];

            artifacts[pos] = art;
            count++;

            if (showMessage)
            {
                Console.WriteLine("Artifact added successfully.");
                Console.WriteLine($"{art.EncodedName} -> {art.DecodedName}");

                Console.WriteLine("\n--- Current Vault ---");
                for (int i = 0; i < count; i++)
                    Console.WriteLine(artifacts[i].FullData);
            }

            try
            {
                File.AppendAllText("galactic_vault.txt", fullLine + "\n");
            }
            catch
            {
                Console.WriteLine("Error saving artifact to file.");
            }
        }
        public void DisplayFullInventory()
        {
            if (count == 0)
            {
                Console.WriteLine("Vault is empty.");
                return;
            }

            for (int i = 0; i < count; i++)
                Console.WriteLine(artifacts[i].FullData);
        }
    }
}