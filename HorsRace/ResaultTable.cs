using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HorsRace
{
    public partial class ResaultTable : Form
    {
        public ResaultTable(List<string> winnerList)
        {
            InitializeComponent();
            ShowResault(winnerList);            

        }
        //Метод заполнения таблицы результатов.
        private void ShowResault(List<string> winnerList)
        {
            int i = 0;
            //Обходим коллекцию контролов и когда
            //обнаруживаем TexBox вносим в него данные.
          foreach(Control item in groupBox1.Controls)
          {
                if (typeof(TextBox) == item.GetType())
                {
                    item.Text = winnerList[(winnerList.Count-1)-i];
                    i++;
                }
          }
        }
       
    }
}
