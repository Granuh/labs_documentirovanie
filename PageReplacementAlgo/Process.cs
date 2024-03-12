using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PageReplacementAlgo
{
    class Process
    {
        // номер
        int id;
        // список номеров страниц
        List<int> table;
        // система
        SystemProg system;

        // конструктор, аргументы номер, список номеров страниц, система
        public Process(int id, List<int> table, SystemProg system)
        {
            this.id = id;
            this.table = table;
            this.system = system;
        }

        // свойства для доступа к полям класса
        public int Id => id;
        public List<int> Table => table;

        // изменение страницы памяти
        public void WritePage(string val, Random random)
        {
            if(table.Count > 0)
            {
                // случайный номер страницы
                int s = random.Next(table.Count);
                // вызываем метод класса Система для изменения страницы
                system.WritePage(Table[s], val, id);
            }
        }
    }
}
