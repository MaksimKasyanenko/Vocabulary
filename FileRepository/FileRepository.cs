using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Vocabulary;
using Vocabulary.Abstract;

namespace VocabularyRepository
{
    public class FileRepository : IWordRepository
    {
        private readonly string path;
        List<Word> list = new List<Word>();
        public FileRepository(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException("file path");
            path = filePath;
            Load();
        }
        public List<Word> GetWords()
        {
            return list;
        }

        public void SaveChanges()
        {
            using(FileStream fs = File.OpenWrite(path))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, list);
                fs.Close();
            }
        }
        private void Load()
        {
            if(!File.Exists(path)){
                File.Create(path);
            }
            using (FileStream fs = File.OpenRead(path))
            {
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    list = (List<Word>)bf.Deserialize(fs);
                }
                catch (SerializationException) { }
                finally
                {
                    fs.Close();
                }
            }
        }
    }
}
