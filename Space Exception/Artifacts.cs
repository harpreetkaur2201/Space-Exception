namespace Space_Expedition
{
    internal class Artifact
    {
        public string EncodedName { get; private set; }
        public string DecodedName { get; private set; }
        public string FullData { get; private set; }

        public Artifact(string encoded, string fullLine)
        {
            EncodedName = encoded;
            FullData = fullLine;
            DecodedName = Encode.DecodeName(encoded);
        }
    }
}
