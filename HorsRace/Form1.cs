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

namespace HorsRace
{
    public partial class Form1 : Form
    {
        //Делегат для функции безопасного обращения к ProgressBar из других потоков.
        private delegate void Safemethod(object obj);
        //Список баров.
        readonly List<ProgressBar> horses; 
        //Переменная делегата.
        readonly Safemethod safemethod;
        //Список результатов забега.
        List<string> winnerList;
        //Таймер.
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        
        public Form1()
        {
            InitializeComponent();
            //Инициализация делегата безопасного обращения к прогрессбарам.
            safemethod = new Safemethod(HorseMove);
            //Заполнение списков прогрессбаров.
            horses = new List<ProgressBar> { Horse1, Horse2, Horse3, Horse4, Horse5 };            
            //Инициализация списка результатов забега.
            winnerList = new List<string>();       
            //Стартовая позиция основного окна.
            StartPosition = FormStartPosition.CenterScreen;           
        }
        //Метод обновления прогрессбаров.
       public void HorseMove(object obj)
        {
            ProgressBar progressBar = (ProgressBar)obj;
            //Изменение текущей позиции индикатора прогрессбара.
            progressBar.Value++;
            //Если текущая позиция достигла максимума.
            if(progressBar.Value==progressBar.Maximum)
            {
                //Добавляем имя лошади в список результатов забега.
                winnerList.Add((string)progressBar.Tag);               
            }
            //Если список результатов забега заполнен.
            if(winnerList.Count==horses.Count)
            {
                //Включаем задержку на 1 секунду, чтобы прогрессбары на форме
                //успели отрисоваться до конца и вызываем окно результатов.
                timer.Tick += new EventHandler(MyResault);
                timer.Interval=1000;
                timer.Start();               
            }
        }
        //Метод вызова формы результатов.
        public void MyResault(object sender,EventArgs e)
        {
            //Останавливаем таймер задержки результатов.
            timer.Stop();
            //Создаем форму результатов.
            ResaultTable resault = new ResaultTable(winnerList);
            //Если форма результатов закрыта, то закрываем основную форму.
            if (resault.ShowDialog() == DialogResult.Cancel)
            {
                this.Close();
            }
        }
        //Метод, который вызывается в потоке из пула потоков.
        private void LetsMove(object state)
        {            
            for (int i = 0; i <100 ; i++)
            { 
                //Безопасное обращение к прогрессбару основного потока.
            ((ProgressBar)state).Invoke(safemethod, state);
                //Рандомная задержка для достижения разной скорости прогрессбаров.
            Thread.Sleep(new Random().Next(5,50));  
            }            
        }
        //Обработчик кнопки старт.
        private void ButtonStart_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = false;
            buttonStart.Hide(); 
            //Ставим потоки в очередь пула потоков.
            for(int i=0;i<horses.Count;i++)
            {
                ThreadPool.QueueUserWorkItem(LetsMove,horses[i]);
            }
        }
    }
}
