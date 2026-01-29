using System;
using System.IO;

namespace Space_Expedition
{
    internal class Galactic_Vault
    {
        private Artifact[] artifacts = new Artifact[80];
        private int count = 0;

        // Load artifacts from galactic_vault.txt on program start
        public void LoadVault()
        {
            if (!File.Exists("galactic_vault.txt"))
            {
                Console.WriteLine("Vault file not found! Starting with empty inventory.");
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines("galactic_vault.txt");
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length < 5) continue; // skip invalid lines

                    AddArtifact(parts[0], line, false, false); // don't show message, don't save again
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading galactic_vault.txt: " + ex.Message);
            }
        }

        // Add an artifact to the vault
        public void AddArtifact(string encoded, string fullLine, bool showMessage = true, bool saveToFile = true)
        {
            if (count >= 80)
            {
                if (showMessage) Console.WriteLine("Cannot add more artifacts. Vault is full.");
                return;
            }

            Artifact art = new Artifact(encoded, fullLine);

            // Check for duplicates by DecodedName
            for (int i = 0; i < count; i++)
                if (artifacts[i].DecodedName == art.DecodedName)
                {
                    if (showMessage) Console.WriteLine("Artifact already exists in the vault.");
                    return;
                }

            // Insert in sorted order by DecodedName
            int pos = 0;
            while (pos < count && string.Compare(artifacts[pos].DecodedName, art.DecodedName) < 0)
                pos++;

            for (int i = count; i > pos; i--)
                artifacts[i] = artifacts[i - 1];

            artifacts[pos] = art;
            count++;

            if (showMessage) Console.WriteLine("Artifact added successfully.");

            // Save immediately to galactic_vault.txt
            if (saveToFile)
            {
                try
                {
                    File.AppendAllText("galactic_vault.txt", fullLine + Environment.NewLine);
                }
                catch
                {
                    Console.WriteLine("Error saving artifact to file.");
                }
            }
        }

        // Display all artifacts in memory
        public void DisplayArtifacts()
        {
            if (count == 0)
            {
                Console.WriteLine("Vault is empty.");
                return;
            }

            for (int i = 0; i < count; i++)
                Console.WriteLine($"{artifacts[i].EncodedName} -> {artifacts[i].DecodedName}");
        }

        // Optional: save all artifacts at once
        public void SaveVault()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("galactic_vault.txt"))
                {
                    for (int i = 0; i < count; i++)
                        writer.WriteLine(artifacts[i].FullData);
                }
                Console.WriteLine("Vault saved successfully.");
            }
            catch
            {
                Console.WriteLine("Error saving vault.");
            }
        }
    }
}
