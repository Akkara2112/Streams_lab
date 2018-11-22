using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
// сортировки слияния и Flashsort
namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        int[] mas1 = new int[50000];
        int[] mas2 = new int[50000];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //проверка есть ли массив


            // создание рандомного массива для сортировки
            Random r = new Random();
            System.IO.StreamWriter udal = new System.IO.StreamWriter("ishod_mas.txt", false);
            { udal.Write(""); udal.Close(); }
            System.IO.StreamWriter zapis = new System.IO.StreamWriter("ishod_mas.txt", true, Encoding.Default);
            for (int i = 0; i < mas1.Length; i++)
            {
                mas1[i] = r.Next();
                if (i != mas1.Length)
                    zapis.Write(mas1[i] + " ");
                else
                    zapis.Write(mas1[i]);
            }
            zapis.Close();
            for (int i = 0; i < mas1.Length; i++)
                mas2[i] = mas1[i];

            for (int i = 0; i < 10; i++)
                textBox1.Text = mas1[i] + ", ";
            listBox1.Items.Add("Массив создан!");
            listBox1.Items.Add("Изначальный массив записан в файл!");

        }
        Thread myThread1;
        Thread myThread2;
        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

            myThread1 = new Thread(new ThreadStart(Count1));
            myThread2 = new Thread(new ThreadStart(Count2));

            myThread1.Start();
            myThread2.Start();
            listBox1.Items.Add("Потоки запущены!");
        }

        public void Count1()// был статик
        {            
            var time = new Stopwatch();
            time.Start();
            sort(mas2);
            time.Stop();
            double l1 = time.Elapsed.TotalMilliseconds;
            textBox2.Text = l1.ToString();
            listBox1.Items.Add("Поток 1 завершен!");
        }
        public void Count2()// был статик
        {
            var time1 = new Stopwatch();
            time1.Start();
            System.IO.StreamWriter udal = new System.IO.StreamWriter("sli_otsort_mas.txt", false);
            { udal.Write(""); udal.Close(); }
            System.IO.StreamWriter zapis = new System.IO.StreamWriter("sli_otsort_mas.txt", true, Encoding.Default);
            zapis.Write(string.Join(" ", sort1(mas1)));
            zapis.Close();
            time1.Stop();
            double l1 = time1.Elapsed.TotalMilliseconds;
            textBox3.Text = l1.ToString();
            listBox1.Items.Add("Поток 2 завершен!");
        }

        //************************************** НАЧАЛО.методы для сортировки слияния**********************************
        public int[] sort1(int[] massive)
        {
            if (massive.Length == 1)
                return massive;
            int mid_point = massive.Length / 2;
            return merged(sort1(massive.Take(mid_point).ToArray()), sort1(massive.Skip(mid_point).ToArray()));
        } //намек на бесконечный цикл ^

        public int[] merged(int[] mass1, int[] mass2)//был статик!!!!
        {
            int a = 0, b = 0;
            int[] merged = new int[mass1.Length + mass2.Length];
            for (int i = 0; i < mass1.Length + mass2.Length; i++)
            {
                if (b < mass2.Length && a < mass1.Length)
                    if (mass1[a] > mass2[b] && b < mass2.Length)
                        merged[i] = mass2[b++];
                    else
                        merged[i] = mass1[a++];
                else
                    if (b < mass2.Length)
                        merged[i] = mass2[b++];
                    else
                        merged[i] = mass1[a++];
            }
            return merged;
        }
/*FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF*/
        void sort(int[] a)
        {
            FlashSort(a); //Сначала FlashSort
            InsertionSort(a); //Напоследок сортировка вставками


            System.IO.StreamWriter udal1 = new System.IO.StreamWriter("FLASHsort_mas.txt", false);
            { udal1.Write(""); udal1.Close(); }
            System.IO.StreamWriter zapis = new System.IO.StreamWriter("FLASHsort_mas.txt", true, Encoding.Default);
            for (int i = 0; i < a.Length; i++)
                zapis.Write(a[i] + " ");
            zapis.Close();
        }
        //FlashSort, применив которую получим
        //почти упорядоченный массив
        private void FlashSort(int[] a)
        {
            int n = a.Length; //Размерность массива
            double m = n * 0.42; //Количество классов
            int[] l = new int[(int)m]; //Вспомогательный массив

            int i = 0, j = 0, k = 0; //Счётчики в циклах
            int anmin = a[0]; //Минимальный элемент
            int nmax = 0; //Индекс максимального элемента

            for (i = 1; i < n; i++)
            {
                if (a[i] < anmin)
                    anmin = a[i];
                if (a[i] > a[nmax])
                    nmax = i;
            }
            if (anmin == a[nmax])
                return;
            double c1 = ((double)m - 1) / (a[nmax] - anmin);
            for (i = 0; i < n; i++)
            {
                k = (int)(c1 * (a[i] - anmin));
                l[k]++;
            }
            for (k = 1; k < m; k++)
            {
                l[k] += l[k - 1];
            }
            int hold = a[nmax];
            a[nmax] = a[0];
            a[0] = hold;
            int nmove = 0;
            int flash;
            j = 0;

            k = (int)m - 1;

            while (nmove < n - 1)
            {
                while (j > (l[k] - 1))
                {
                    j++;
                    k = (int)(c1 * (a[j] - anmin));
                }
                flash = a[j];
                while (!(j == l[k]))
                {
                    k = (int)(c1 * (flash - anmin));

                    hold = a[l[k] - 1];
                    a[l[k] - 1] = flash;
                    flash = hold;

                    l[k]--;
                    nmove++;
                }            }        }
        private void InsertionSort(int[] a)
        {
            int i, j, hold;
            for (i = a.Length - 3; i >= 0; i--)
            {
                if (a[i + 1] < a[i])
                {
                    hold = a[i];
                    j = i;
                    while (a[j + 1] < hold)
                    {
                        a[j] = a[j + 1];
                        j++;
                    }
                    a[j] = hold;
                }            }        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((myThread1.IsAlive) && (myThread2.IsAlive))
            {
                myThread1.Suspend();
                myThread2.Suspend();
                listBox1.Items.Add("Потоки были приостановлены!");
            }
            button4.Enabled = true;        }
        private void button4_Click(object sender, EventArgs e)
        {
            if ((myThread1.IsAlive) && (myThread2.IsAlive))
            {
                myThread1.Resume();
                myThread2.Resume();
                listBox1.Items.Add("Работа потоков возобнавлена!");button4.Enabled = false;
            }          
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if ((myThread1.IsAlive) && (myThread2.IsAlive))
            {
                myThread1.Abort();
                myThread2.Abort();
                listBox1.Items.Add("Потоки были принудительно остановлены!");
                button1.Enabled = false;
            }        }
/*FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF*/
    }
}
