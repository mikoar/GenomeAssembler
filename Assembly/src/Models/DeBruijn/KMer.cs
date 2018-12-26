namespace Assembly.Models
{
    internal class KMer
    {
        public KMer(string kmer)
        {
            _kmer = kmer;
        }

        private string _kmer;
        public Node LeftKMinus1Mer { get => new Node(_kmer.Substring(0, _kmer.Length - 1)); }
        public Node RigthKMinus1Mer { get => new Node(_kmer.Substring(1)); }
    }
}