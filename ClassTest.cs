using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace laba9
{
    class ClassTest
    {
   
        // поля класса
        double[] mass; // массив с отложенной инициализацией
        int num, engin;
        double min,max;   // диапазон случ. числа
        int leg = 8;
        double[] x_gist;
        int[] y_gist;

        public ClassTest(string namefile) //конструктор для внешнего файла
        {
            StreamReader file_1 = new StreamReader(namefile);//открываем файл для чтения
            string s = file_1.ReadLine();//считываем данные
            string[] mass_s = s.Split();//распределяем числа в массив
            num = mass_s.Length;//определяем длину массива
            mass = new double[num];//инициализируем главный массив
            for (int i = 0; i < num; i++)//заполняем главный массив
            {
                mass[i] = Convert.ToDouble(mass_s[i]);
            }
            min = mass.Min();//находим мин. и макс.
            max = mass.Max();
            file_1.Close();//закрываем файл
            gist(leg);//запускаем обработку гистограммы
        }
        public ClassTest(int n, double mn, double mx, int proverka) // конструктор с 4-мя параметрами
        { 
            engin = proverka;//очень нужная проверка
            num = n++; // получить размер массива
            min = mn;
            max = mx;
            mass = new double[num]; // инициализация массива
            // вызвать закрытый метод заполнения массива случ. числами
            FillArray();//запуск генерации рандомных значений
            gist(leg);//запуск обработки гистограммы
        }
        // закрытый метод заполнения массива случ. числами

        public void gist(int dlina) //обработка гистограммы
        {
            dlina++;//длина массива = кол-во+1
            x_gist = new double[dlina];//иниициализируем массивы для X и Y осей
            y_gist = new int[dlina];
            double hag = (mass.Max() - mass.Min()) / dlina;//находим длину шага
            for (int i = 0; i < dlina; i++)//цикл заполнения массивов
            {
                x_gist[i] = (int)(mass.Min() + hag * i);//заполнение шагов
                for (int a = 0; a < num; a++)//цикл для распределения чисел
                {
                    if (mass[a] < x_gist[i] && mass[a] > x_gist[i] - hag)//проверка входа в диапазон
                    {
                        y_gist[i]++;//считаем числа в диапазоне
                    }

                }
            }
        }
        /* след. метод я начал делать перепутав варианты
         * поэтому тут есть как обработка целых чисел
         * и отрицательных, так и дробных
         * решил оставить, было жалко удалять
         * */
        void FillArray() //метод генерации рандомных значений
        {
            Random Rand = new Random(); // объявить объект класса (типа) Random
            if (engin == 1) //генерация рандомных значений с дробной частью
            {
                Random Rand2 = new Random();
                int min_need = 0, max_need = 99;//мин. и макс. дробной части
                int min_need1 = 0, max_need1 = 99;//мин. и макс. дробной части для первого и последнего числа
                int n1, n2;//n1 - целая часть     n2 - дробная часть
                int min_int, max_int;//мин. и макс. для целой части
                if (Convert.ToString(min).Contains(',')) //проверяем есть ли разделитель целой и дробной части
                {
                    min_int = (int)min;//берём целое для мин.
                    min_need1 = Convert.ToInt32((min - min_int) * 100);//берём дробное. для мин.
                }
                else
                {
                    min_int = Convert.ToInt32(min); //берём всё число если оно и так целое
                }

                if (Convert.ToString(max).Contains(','))//проверяем есть ли разделитель целой и дробной части
                {
                    max_int = (int)max;//берём целое для макс.
                    max_need1 = Convert.ToInt32((max - max_int) * 100);//берём дробное для макс.
                }
                else
                {
                    max_int = Convert.ToInt32(max);//берём всё число если оно и так целое
                }

                for (int i = 0; i < num; i++)//цикл для заполнения числами с запятой
                {
                    n1 = Rand.Next(min_int, max_int);//рандомная целая часть
                    n2 = Rand2.Next(min_need, max_need);//рандомная дробная часть
                    if (n1 == min_int && n2 < min_need) //проверяем не получилось ли число меньше мин. по дробной части
                    {
                        n2 = Rand2.Next(min_need1, max_need); //если число меньше генерируем снова на основе дробной части мин. числа
                    }
                    if (n1 == max_int && n2 > max_need) //проверяем не получилось ли число больше макс. по дробной части
                    {
                        n2 = Rand2.Next(min_need, max_need1); //если число больше генерируем снова на основе дробной части макс. числа
                    }
                    mass[i] = Convert.ToDouble(Convert.ToString(n1) + "," + Convert.ToString(n2)); //заполняем массив склеивавя целую и дробную часть
                }
            }
            else //если не требуется дробных чисел для генерации
            {
                for (int i = 0; i < num; i++)//заполняем массив рандомными целыми числами
                {
                    mass[i] = Rand.Next(Convert.ToInt32(min), Convert.ToInt32(max));
                }
            }
        }

        public string GetStringArray() //метод получения основного массива в виде строки
        {
            return String.Join(" ", mass);
        }

        public void sortmass()//метод сортировки массива
        {
            Array.Sort(mass);
        }

        public double[] mass_return()//метод возврата основного массива
        {
            return mass;
        }

        public double[] mass_return_x()//метод возврата массива оси Х для гистограммы
        {
            return x_gist;
        }
        public int[] mass_return_y()//метод возврата массива оси У для гистограммы
        {
            return y_gist;
        }
    }
}
