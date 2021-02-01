using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Vocabulary
{
	[Serializable]
	public class Word
	{
		private Rating _rating = new Rating();
		public Rating Rating { get { return _rating; } }
		private List<string> flist = new List<string>();
		private List<string> slist = new List<string>();
		public Word(string firstTranslate, string secondTranslate)
		{
			this.AddFirstTranslate(firstTranslate);
			this.AddSecondTranslate(secondTranslate);
			_rating = new Rating();
		}
		public Word() {  }
		public string FirstTranslate
		{
			get
			{
				return ToLine(flist);
			}
		}
		public string SecondTranslate
		{
			get
			{
				return ToLine(slist);
			}
		}
        public void AddFirstTranslate(string fw)
		{
			if (fw == null) throw new ArgumentNullException("fw"); 
			fw = fw.Trim().ToLower();
			if (flist.Contains(fw)) return;
			flist.Add(fw);
		}
		public void AddSecondTranslate(string sw)
		{
			if (sw == null) throw new ArgumentNullException("sw");
			sw = sw.Trim().ToLower();
			if (slist.Contains(sw)) return;
			slist.Add(sw);
		}
		public bool IsFirstTranslate(string word)
		{
			if (flist.Contains(word))
				return true;
			return false;
		}
		public bool IsSecondTranslate(string word)
		{
			if (slist.Contains(word))
				return true;
			return false;
		}
		public IEnumerable<string> GetFirstTranslates()
        {
			return from ft in flist
				   select ft;
        }
		public IEnumerable<string> GetSecondTranslates()
		{
			return from st in slist
				   select st;
		}
		public bool IsMatch(Word word)
        {
			foreach(var w in flist)
            {
				if (word.IsFirstTranslate(w)) return true;
            }
			foreach (var w in slist)
			{
				if (word.IsSecondTranslate(w)) return true;
			}
			return false;
		}
		public void Append(Word word)
        {
			foreach (var ft in word.GetFirstTranslates())
				if (!flist.Contains(ft)) AddFirstTranslate(ft);
			foreach (var st in word.GetSecondTranslates())
				if (!slist.Contains(st)) AddSecondTranslate(st);
        }

        public Word Clone()
        {
			Word res = new Word();
			res._rating = new Rating();
			flist.ForEach(tr=>res.flist.Add(tr));
			slist.ForEach(tr=>res.slist.Add(tr));
			return res;
        }

        public override string ToString()
		{
			return ToLine(flist) + " - " + ToLine(slist);
		}
		private string ToLine(List<string> c)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < c.Count; i++)
			{
				if (i != 0) sb.Append(", ");
				sb.Append(c[i]);
			}
			return sb.ToString();
		}
    }
}