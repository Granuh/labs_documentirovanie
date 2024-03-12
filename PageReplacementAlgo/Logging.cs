using System.Collections.Generic;
using System.Windows.Forms;

namespace PageReplacementAlgo
{
    // класс-синглтон для логгирования действий
    // это реализация шаблона одиночка, при обращении к 
    // классу всегда будет возвращатся тот же объект
    class Logging
    {
        // тут будет хранится сам объект класса
        private static Logging instance;
        // запрещаем создане через new
        private Logging()
        { }
        // если первый вызов, то создаем новый объект, 
        // иначе возвращает тот который уже существует
        public static Logging getInstance()
        {
            if (instance == null)
                instance = new Logging();
            return instance;
        }

        private  ListBox listBox1;

        public  void SetListBox(ListBox lb)
        {
            listBox1 = lb;
        }

        public  void LogWrite(string s)
        {
            listBox1.Items.Add(s);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        public  void LogWrite(List<string> ses)
        {
            foreach (var s in ses)
            {
                listBox1.Items.Add(s);
            }
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }
    }
}
