namespace PageReplacementAlgo
{
    // класс страница памяти
    class Page
    {
        // номер
        readonly int id;
        // конструктор, аргументы номер и где находится
        public Page(int id, PageType type)
        {
            this.id = id;
            Type = type;
            R = false;   // R - изменялась ли страница или нет
            Counter = 0;
            Value = "";
        }
        // свойства для доступа
        public bool R { get; set; }
        public PageType Type { get; set; }
        public byte Counter { get; private set; }
        public int Id => id;
        public string Value { get; private set; }

        // вычисляется значение счетчика (Если изменяли, то самый левый бит ставим 1, не изменяли оставляем 0, и R делаем false)
        public void CalculateCounter()
        {
            Counter >>= 1;
            if(R)
                Counter |= 1 << 7;
            R = false;
        }
        // задается значение страницы
        public void SetValue(string val)
        {
            Value = val;
            R = true;
        }
    }
}
