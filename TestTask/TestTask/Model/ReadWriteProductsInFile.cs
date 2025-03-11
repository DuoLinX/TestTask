using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;

namespace TestTask.Model
{
    public class ReadWriteProductsInFile
    {
        private string _fullPathToFile;
        private ObservableCollection<Product> _productsCollection;
        public ReadWriteProductsInFile(string fullPathToFile, ObservableCollection<Product> collection)
        {
            _fullPathToFile = fullPathToFile;
            _productsCollection = collection;
        }
        public void WriteToFile()
        {
            using (StreamWriter sw = new StreamWriter(_fullPathToFile))
            {
                sw.Write(JsonConvert.SerializeObject(_productsCollection, Formatting.Indented));
            }
        }

        public ObservableCollection<Product> ReadFromFile() 
        {
            using (StreamReader sr = new StreamReader(_fullPathToFile))
            {
                string jsonData = sr.ReadToEnd();
                _productsCollection = JsonConvert.DeserializeObject<ObservableCollection<Product>>(jsonData);
            }
            return _productsCollection;
        }

        public void ClearList()
        {
            _productsCollection.Clear();
        }
    }
}
