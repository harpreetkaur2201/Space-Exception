using System;
using System.IO;

namespace Space_Expedition
{
    internal class Galactic_Vault
    {
        private Artifact[] artifacts = new Artifact[80];
        private int count = 0;

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
                    string trimmed = line.Trim();
                    if (string.IsNullOrEmpty(trimmed)) continue;

                    string[] parts = trimmed.Split('|');
                    if (parts.Length < 5) continue;

                    AddArtifact(parts[0].Trim(), line, false);
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
                if (showMessage) Console.WriteLine("Cannot add more artifacts. Vault is full.");
                return;
            }

            Artifact art = new Artifact(encoded, fullLine);

            // Check for duplicate
            for (int i = 0; i < count; i++)
                if (artifacts[i].DecodedName == art.DecodedName)
                {
                    if (showMessage) Console.WriteLine("Artifact already exists in the vault.");
                    return;
                }

            // Insert sorted by DecodedName
            int pos = 0;
            while (pos < count && string.Compare(artifacts[pos].DecodedName, art.DecodedName) < 0)
                pos++;

            for (int i = count; i > pos; i--)
                artifacts[i] = artifacts[i - 1];

            artifacts[pos] = art;
            count++;

            if (showMessage) Console.WriteLine("Artifact added successfully.");
        }

        public void DisplayArtifacts()
        {
            for (int i = 0; i < count; i++)
                Console.WriteLine($"{artifacts[i].EncodedName} -> {artifacts[i].DecodedName}");
        }

        public void SaveVault()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("expedition_summary.txt"))
                {
                    for (int i = 0; i < count; i++)
                        writer.WriteLine(artifacts[i].FullData);
                }
                Console.WriteLine("Data saved successfully.");
            }
            catch
            {
                Console.WriteLine("Error saving file.");
            }
        }
    }
}
