using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;

namespace SP_HW4._2
{
    public partial class Form1 : Form
    {
        Mutex mutex1 = new Mutex();
        Mutex mutex2 = new Mutex();
        AutoResetEvent autoResetEvent1 = new(false);
        AutoResetEvent autoResetEvent2 = new(false);
        BindingList<int> list1 = new BindingList<int>();
        BindingList<int> list2 = new BindingList<int>();
        BindingList<int> list3 = new BindingList<int>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.DataSource = list1;
            listBox2.DataSource = list2;
            listBox3.DataSource = list3;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            autoResetEvent1.Reset();
            list1.Clear();
            list2.Clear();
            Thread thread1 = new(Print20Num);
            Thread thread2 = new(Print10Num);
            thread1.Start();
            thread2.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            autoResetEvent2.Reset();
            list3.Clear();
            for (int i = 1; i < 11; i++)
            {
                list3.Add(i);
                Thread.Sleep(10);
            }
            Task task1 = Task.Run(() => AddRandomValuesToArray(list3));
            Task<(int min, int max)> task2 = Task.Run(() => ReturnMinMaxFromArray(list3));
            textBox1.Text = task2.Result.min.ToString();
            textBox2.Text = task2.Result.max.ToString();
        }

        private void Print10Num()
        {
            autoResetEvent1.WaitOne();
            mutex1.WaitOne();
            try
            {
                for (int i = 11 - 1; i >= 0; i--)
                {
                    list2.Add(i);
                    Thread.Sleep(10);
                }
            }
            finally
            {
                mutex1.ReleaseMutex();
                autoResetEvent1.Set();
            }
        }

        private void Print20Num()
        {
            mutex1.WaitOne();
            try
            {
                for (int i = 0; i < 21; i++)
                {
                    list1.Add(i);
                    Thread.Sleep(10);
                }
            }
            finally
            {
                mutex1.ReleaseMutex();
                autoResetEvent1.Set();
            }
        }

        private void AddRandomValuesToArray(object obj)
        {           
            mutex2.WaitOne();
            try
            {
                BindingList<int> list = (BindingList<int>)obj;
                Random rand = new();
                for (int i = 0; i < list.Count; i++)
                {
                    list[i] += rand.Next(0, 101);
                    Thread.Sleep(100);
                }
            }
            finally
            {
                mutex2.ReleaseMutex();
                autoResetEvent2.Set();
            }
        }

        private (int, int) ReturnMinMaxFromArray(object obj)
        {
            autoResetEvent2.WaitOne();
            mutex2.WaitOne();
            try
            {
                return ((obj as BindingList<int>).Min(), (obj as BindingList<int>).Max());
            }
            finally
            {
                mutex2.ReleaseMutex();
                autoResetEvent2.Set();
            }
        }



    }
}