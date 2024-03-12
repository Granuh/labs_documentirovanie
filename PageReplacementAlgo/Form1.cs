using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PageReplacementAlgo
{
    public partial class Form1 : Form
    {
        // переменная хранит систему
        SystemProg system;
        // количество страниц памяти
        const int AllMem = 50;
        // количество страниц в оперативной памяти
        const int OpMem = 20;
        // максимальное количество страниц на процесс
        const int pagesForOne = 5;
        // генератор случайных чисел
        private Random random = new Random();

        // конструктор формы
        public Form1()
        {
            InitializeComponent();
            // указываем логеру куда писать
            Logging.getInstance().SetListBox(listBox1);
            label4.Text = "";
        }
        // заполнение системы начальными данными
        private void Initialize()
        {
            // создаем страницы памяти
            List<Page> mem = new List<Page>();
            for (int i = 0; i < AllMem; i++)
            {
                mem.Add(new Page(i + 1, PageType.VP));
            }
            Logging.getInstance().LogWrite(new List<string> { "Начальная инициализация", $"Создаем страниц памяти - {AllMem}", "" });
            system = new SystemProg(mem, OpMem);

            Logging.getInstance().LogWrite(new List<string> { "Создаем процессы", "" });
            // создаем процессы пока хватает страниц памяти
            int limit = 0;
            int x = 1;
            while (limit < AllMem)
            {
                // количество страниц для данного процесса
                int pages = random.Next(1, pagesForOne);
                // создаем процесс
                system.AddProcess(x, pages, random, true);
                x++;
                limit += pages;
            }
            FillDataGrid();
        }

        // функция создает случайную строку указанной длинны
        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // заполняет таблицы на форме
        private void FillDataGrid()
        {
            // очищаем таблицы
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            // обходи список страниц
            foreach (var item in system.Memory)
            {
                // получаем процесс которому принадлежит данная страница памяти
                var proc = system.WhoseMem(item.Id);
                // в зависимости от того где находится страница, выводим в нужную таблицу
                if (item.Type == PageType.OP)
                {
                    dataGridView1.Rows.Add(item.Id, item.Value, item.Counter, proc);
                }
                else
                {
                    dataGridView2.Rows.Add(item.Id, item.Value, item.Counter, proc);
                }
            }
        }
        // пункт меню выход
        private void выходToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        // пункт меню начать
        private void начатьToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            // очищаем форму
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            listBox1.Items.Clear();
            // формируем начальное состояние системы
            Initialize();
            label4.Text = "";
            // если таймер выключен, то включаем
            if (timer1.Enabled == false)
                timer1.Enabled = true;
        }
        // пункт меню завершить
        private void завершитьToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            // есл таймер включен, то останавливаем
            if (timer1.Enabled == true)
                timer1.Enabled = false;
            // убираем надпись
            label4.Text = "";
        }

        // функция вызывается на каждый тик таймера
        private void timer1_Tick(object sender, EventArgs e)
        {
            // моделируем процесс  работы системы

            // случайное число от 0 до 9
            int val = random.Next(10);
            // если число 0 или 1
            if (val <= 1)
            {
                // создаем новый процесс
                // количество страниц памяти
                int pages = random.Next(1, pagesForOne);
                // создаем процесс
                system.AddProcess(system.NextProcessId, pages, random);
                // выводим данные
                FillDataGrid();
            }
            else if (val > 1 && val < 8) // случайное число от 2 до 7
            {
                // записываем на страницу памяти
                // случайное число
                int index = random.Next(system.Processes.Count);
                // если нет процессов выходим
                if (system.Processes.Count == 0)
                {
                    Logging.getInstance().LogWrite("Ошибка! Нет запущеных процессов");
                    return;
                }
                // получаем процесс из списка в системе
                Process process = system.Processes[index];
                int size = random.Next(5, 20);
                // случайная строка
                string valPage = RandomString(size);
                // изменяем страницу
                process.WritePage(valPage, random);
                // пересчитываем счетчики у страниц
                system.PagesRecount();
                // выводим данные
                FillDataGrid();
            }
            else if (val > 7) // если число 8 или 9
            {
                // удаляем процесс
                // случайное число
                int index = random.Next(system.Processes.Count);
                // если нет процессов выходим
                if (system.Processes.Count == 0)
                {
                    Logging.getInstance().LogWrite("Ошибка! Нет запущеных процессов");
                    return;
                }
                // получаем процесс из списка в системе
                Process process = system.Processes[index];
                // удаляем процесс из списка
                system.RemoveProcess(process);
                // выводим данные
                FillDataGrid();
            }
        }
    }
}
