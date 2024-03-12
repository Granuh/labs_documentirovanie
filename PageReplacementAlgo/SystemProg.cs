using System;
using System.Collections.Generic;
using System.Linq;

namespace PageReplacementAlgo
{
    // класс системы
    class SystemProg
    {
        // список процессов
        List<Process> processes;
        // список страниц
        List<Page> memory;
        // номера занятых страниц
        List<int> used;
        // номера свободных страниц
        List<int> free;

        // конструктор, аргументы список страниц памяти и количество в опративной памяти
        public SystemProg(List<Page> memory, int inOP)
        {
            this.memory = memory;
            // создаем списки
            processes = new List<Process>();
            used = new List<int>();
            free = new List<int>();
            // добаляем все страницу в свободные 
            foreach (var item in memory)
            {
                free.Add(item.Id);
            }
            // часть отправляем в оперативную память
            for (int i = 0; i < inOP; i++)
            {
                memory[i].Type = PageType.OP;
            }
        }

        // свойства для доступа к полям
        public int FreePage => free.Count;
        public List<Page> Memory => memory;
        public List<Process> Processes => processes;

        // проверяем какому процессу принадлежит память
        // с данным номером, если память свободна, то возвращаем  -1
        public int WhoseMem(int n)
        {
            if (processes.Count == 0)
                return -1;
            foreach (var item in processes)
            {
                if (item.Table.Contains(n))
                    return item.Id;
            }
            return -1;
        }

        // свойство возвращает следующий номер процесса, максимальный в списке + 1
        public int NextProcessId => processes.Max(x => x.Id) + 1;

        // создание нового процесса, аргумент start нужен,
        // чтобы логи не выводились при начальном заполнении
        // принимает номер процесса и количество страниц памяти
        // cnt - кол-во страниц памяти для процесса
        public int AddProcess(int n, int cnt, Random random, bool start = false)
        {
            // если страниц не хватает выходим
            if (FreePage < cnt)
            {
                if(! start)
                    Logging.getInstance().LogWrite(new List<string> { "Не удалось добавить процесс", "нехватает памяти", "" });
                return -1;
            }
            // выделяем страницы
            var proces = GetMemSpace(cnt);
            // создаем новый процесс и добавляем его в список
            processes.Add(new Process(n, proces, this));
            if (!start)
            {
                Logging.getInstance().LogWrite("Запрос на создание процесса");
                Logging.getInstance().LogWrite($"Добавлен процесс id = {n}");
                var result = string.Join(";", proces);
                Logging.getInstance().LogWrite("Процесс использует страницы:");
                Logging.getInstance().LogWrite(result);
                Logging.getInstance().LogWrite("");
            }
            // возвращаем номер процесса
            return n;
        }
        //  возвращает список страниц памяти, аргумент количество  страниц
        List<int> GetMemSpace(int cnt)
        {
            List<int> proces = new List<int>();
            int i = 0;
            // убираем страницу из списка свободных и добавляем в занятые
            while(i < cnt)
            {
                int gh = free[0];
                //free.Remove(gh);
                //used.Add(gh);
                //proces.Add(gh);
                i++;
            }
            return proces;
        }

        // удаление процесса
        public void RemoveProcess(Process proc)
        {
            var result = string.Join(";", proc.Table);
            Logging.getInstance().LogWrite(new List<string> {"Запрос на удаление процесса",
            $"Удален процесс id = {proc.Id}", "Свободные страницы:", result, ""});
            // занятые процессом страницы памяти возвращаем в список свободных
            foreach (var item in proc.Table)
            {
                //used.Remove(item);
                //free.Add(item);
            }
            // удаляем процесс из списка
            //processes.Remove(proc);
        }
        // все страницы памяти пересчитывают счетчик
        public void PagesRecount()
        {
            foreach (var item in memory)
            {
                item.CalculateCounter();
            }
        }
        // изменяем страницу памяти, аргументы номер страницы, текст, номер процесса
        public void WritePage(int n, string val, int proc_id)
        {
            Logging.getInstance().LogWrite("Запрос на запись в страницу памяти");
            Logging.getInstance().LogWrite($"Процесс {proc_id}");
            // получаем страницу памяти по id
            var page = memory.Where(x => x.Id == n).ToList()[0];
            Logging.getInstance().LogWrite($"Страница {page.Id}");
            // тут происходит обработка страниц памяти по алгоритму старение
            // если страница на внешнем носителе
            if (page.Type == PageType.VP)
            {
                Logging.getInstance().LogWrite("Страница во внешней памяти");
                // получаем список страниц в оперативной памяти
                var pagesOP = memory.Where(x => x.Type == PageType.OP).ToList();
                // находим минимальное значение счетчика
                int minVal = pagesOP.Min(x => x.Counter);
                // берем первую попавшуюся страницу с минимальным счетчиком
                var minPage = pagesOP.Where(x => x.Counter == minVal).FirstOrDefault();
                // меняем страницы местами
                page.Type = PageType.OP;
                minPage.Type = PageType.VP;
                Logging.getInstance().LogWrite("Меняем местами страницы");
                Logging.getInstance().LogWrite($"{page.Id} <-> {minPage.Id}");
            }
            // изменяем страницу
            page.SetValue(val);
            Logging.getInstance().LogWrite(new List<string> { "Записываем информацию", "" });
        }
    }
}
