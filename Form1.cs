using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace laba9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int x, prov = 0;
        double y, z;
        private void button2_Click(object sender, EventArgs e) //обработка нажатия на кнопку "Сортировка"
        {
            obj_1.sortmass(); //запуск метода сортировки
            richTextBox1.AppendText("\nСортированный \n");  //вывод отсортированного массива
            richTextBox1.AppendText(obj_1.GetStringArray());
        }

        private void button3_Click(object sender, EventArgs e) //обработка нажатия на кнопку "Данные из файла"
        {
            try //запуск обработки исключений
            {
                richTextBox1.Clear(); //очищение поля вывода

                openFileDialog1.ShowDialog(); //форма выбора файла
                StreamReader file = new StreamReader(openFileDialog1.FileName);   //первоначальное открытие файла
                string test = file.ReadToEnd();                                   //для проверки
                if (test.Length == 0) //проверка на пустой файл
                {
                    MessageBox.Show("Файл пустой!", "Ошибка");
                }
                else
                {
                    obj_1 = new ClassTest(openFileDialog1.FileName); //создание объекта с передачей параметра "название файла"

                    richTextBox1.AppendText("Из файла \n");          //вывод массива
                    richTextBox1.AppendText(obj_1.GetStringArray()); //из файла

                    button2.Enabled = true; //разблокировка других кнопок
                    button4.Enabled = true;
                    button5.Enabled = true;

                    label6.Visible = true;  //сообщение о пути и имени файла, а так же о кол-ве значений, мин. и макс.
                    label6.Text = "Файл: " + openFileDialog1.FileName + "\nКоличество: " + obj_1.mass_return().Length + "   Минимум: " + obj_1.mass_return().Min() + "   Максимум: " + obj_1.mass_return().Max();
                    file.Close(); //закрытие файла
                }
            }
            catch  //обработка ошибки при выборе файла не того формата
            {
                MessageBox.Show("Некорректный файл!", "Ошибка");
            }
            
        }

        private void button4_Click(object sender, EventArgs e) //обработка нажатия кнопки "График"
        {
            prov = 0;  
            chart1.Series["Series1"].Points.Clear();                         //создание графика
            chart1.Series["Series1"].ChartType = SeriesChartType.Line;       //на основе 
            chart1.Series["Series1"].Points.DataBindY(obj_1.mass_return());  //получечнных данных
            numericUpDown1.Enabled = false;   //запрет выбора кол-ва столбцов для гистограммы
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) //поле для ввода кол-ва значений
        {
            if (e.KeyChar != '\b')                      //ограничение на ввод
            {                                           //недопустимых значений
                e.Handled = !Char.IsDigit(e.KeyChar);
            }
            if (textBox1.Text.Length != 0)              //запрет ввода нескольких нулей в начале
            {
                if (textBox1.Text[0] == '0')
                {
                    textBox1.Text = textBox1.Text.Substring(1);
                }
            }
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e) //поле для ввода мин.
        {
            if (e.KeyChar != '\b')                      //ограничение на ввод
            {                                           //недопустимых значений
                e.Handled = !Char.IsDigit(e.KeyChar);
            }
            if (e.KeyChar == '-' && Convert.ToInt32(textBox2.Text) != 0)  //обработка нажатия клавиши "-"
            {
                textBox2.Text = Convert.ToString(Convert.ToInt32(textBox2.Text) * (-1)); //умножаем на -1

                if (Convert.ToDouble(textBox2.Text) < 0) //если число отрицательное то добавляем доп знак для ввода(чтобы получилось 3 знака + "-")
                {                                        //иначе убираем доп знак
                    textBox2.MaxLength++;
                }
                else
                {
                    textBox2.MaxLength--;
                }
            }
            if (textBox2.Text.Length != 0)               //запрет ввода нескольких нулей в начале
            {
                if (textBox2.Text[0] == '0')
                {
                    textBox2.Text = textBox2.Text.Substring(1);  //удаление лишнего незначащего нуля
                }
            }

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')                      //ограничение на ввод
            {                                           //недопустимых значений
                e.Handled = !Char.IsDigit(e.KeyChar);
            }
            if (e.KeyChar == '-' && Convert.ToInt32(textBox3.Text) != 0)//обработка нажатия клавиши "-"
            {
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) * (-1));//умножаем на -1

                if (Convert.ToDouble(textBox3.Text) < 0)//если число отрицательное то добавляем доп знак для ввода(чтобы получилось 3 знака + "-")
                {                                       //иначе убираем доп знак
                    textBox2.MaxLength++;
                }
                else
                {
                    textBox2.MaxLength--;
                }
            }
            if (textBox3.Text.Length != 0)              //запрет ввода нескольких нулей в начале
            {
                if (textBox3.Text[0] == '0')
                {
                    textBox3.Text = textBox3.Text.Substring(1);  //удаление лишнего незначащего нуля
                }
            }
        }



        private void button5_Click(object sender, EventArgs e) //обработка нажатия клавиши "Гистограмма"
        {
            prov = 1;
            obj_1.gist(Convert.ToInt32(numericUpDown1.Value)); //обновляем массивы с Х и У для гистограммы, используя значения из numericUpDown1
            chart1.Series["Series1"].Points.Clear();                    //создание самой гистограммы
            chart1.Series["Series1"].ChartType = SeriesChartType.Column;
            chart1.Series["Series1"].Points.DataBindXY(obj_1.mass_return_x(), obj_1.mass_return_y());//берём Х и У из объекта
            numericUpDown1.Enabled = true; //разрешаем ввод значений в numericUpDown1
        }

       

        private void numericUpDown1_Click(object sender, EventArgs e) //обработка выбора числа столбцов гистограммы
        {
            obj_1.gist(Convert.ToInt32(numericUpDown1.Value)); //обновляем массивы с Х и У для гистограммы, используя значения из numericUpDown1
            chart1.Series["Series1"].Points.Clear();
            chart1.Series["Series1"].ChartType = SeriesChartType.Column; //пересоздаём гистограмму
            chart1.Series["Series1"].Points.DataBindXY(obj_1.mass_return_x(), obj_1.mass_return_y());
        }


        ClassTest obj_1; //инициализация объекта
        private void button1_Click(object sender, EventArgs e) //обработка нажатия кнопки "Создать"
        {
            label6.Visible = false; //выключаем текст с данными о файле(если был включён)
            try  //блок обработки исключений
            {
                x = Convert.ToInt32(textBox1.Text); //считывание значений из полей для ввода
                y = Convert.ToInt32(textBox2.Text);
                z = Convert.ToInt32(textBox3.Text);

                if (checkBox1.Checked)//проверка выбора галочки для определения значений с запятой или без
                {
                    obj_1 = new ClassTest(x, y, z, 1); //основной конструктор класса
                }
                else
                {
                    obj_1 = new ClassTest(x, y, z, 0); //основной конструктор класса
                }
                if (Convert.ToInt32(textBox1.Text) > 0) //проверка на нулевое значение поля "Количество"
                {
                    richTextBox1.Clear(); //очищение поляя для вывода данных
                    richTextBox1.AppendText("Обычный \n");
                    richTextBox1.AppendText(obj_1.GetStringArray()); //вывод массива с числами

                    button2.Enabled = true;//разблокировка кнопок
                    button4.Enabled = true;
                    button5.Enabled = true;
                }
                else if (Convert.ToInt32(textBox1.Text) == 0) //вывод сообщения если кол-во равно 0
                {
                    MessageBox.Show("Введено нулевое значение кол-ва чисел!", "Ошибка");
                }

            }
            catch
            {
                try
                {
                    if (Convert.ToDouble(textBox2.Text) > Convert.ToDouble(textBox3.Text)) //проверка на мин < макс
                    {
                        MessageBox.Show("Начало диапазона превышает конец!", "Ошибка");
                    }
                    else
                    {
                        MessageBox.Show("Некорректный диапазон!", "Ошибка");
                    }
                }
                catch             //обработка прочих ошибок ввода
                {
                    MessageBox.Show("Некорректный данные ввода!", "Ошибка");
                }
            }

            if (prov == 0) //проверка для автоматического обновления Графика или гистограммы
            {              //зависит от того, что сейчас выбрано
                button4.PerformClick();
            }
            else
            {
                button5.PerformClick();
            }
        }
    }
}
