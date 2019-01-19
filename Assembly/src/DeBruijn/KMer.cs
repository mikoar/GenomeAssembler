namespace Assembly.DeBruijn
{
    public class KMer
    {
        public KMer(string kmer)
        {
            _kmer = kmer;
        }

        private string _kmer;
        public Node LeftKMinus1Mer { get => new Node(_kmer.Substring(0, _kmer.Length - 1)); }
        public Node RigthKMinus1Mer { get => new Node(_kmer.Substring(1)); }

        public override string ToString()
        {
            return _kmer;
        }

        public override int GetHashCode()
        {
            return _kmer.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var node = obj as KMer;
            return node != null && node._kmer == _kmer;
        }
    }
}