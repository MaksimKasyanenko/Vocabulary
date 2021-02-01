using System;
namespace Vocabulary
{
    [Serializable]
    public class Rating
    {
        public Rating()
        {
        }
        private int _rate=0;
        public int MaxRate { get; set; } = 100;
        public int Rate { get {
                return _rate;
            } set {
                _rate = value;
                if (_rate < 0) _rate = 0;
                if (_rate > MaxRate) _rate = MaxRate;
            } 
        }
    }
}
