using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace SYAPSU.classes
{
    public class Serializer
    {
        public void SerializeData(List<Operator> data)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "XML files (.xml)|*.xml";
            bool? result = dialog.ShowDialog();

            if(result == true)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Operator>));
                using (FileStream fs = new FileStream(dialog.FileName, FileMode.OpenOrCreate))
                {
                    xmlSerializer.Serialize(fs, data);
                }
                Link.Logging(" Файл сериализован " + dialog.FileName, LogLvl.info);
            }
        }

        public List<Operator> DeserializeData() 
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "XML files (.xml)|*.xml";
            bool? result = dialog.ShowDialog();
            List<Operator>? data;
            if (result == true)
            {
                try
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Operator>));
                    using (FileStream fs = new FileStream(dialog.FileName, FileMode.OpenOrCreate))
                    {
                        data = xmlSerializer.Deserialize(fs) as List<Operator>;
                    }
                    Link.Logging(" Файл успешно загружен " + dialog.FileName, LogLvl.info);
                    return data;
                }
                catch 
                {
                    MessageBox.Show("Неверный формат");
                    Link.Logging(" Не удалось загрузить файл " + dialog.FileName, LogLvl.error);
                }
            }
            return null;
        }
    }
}
