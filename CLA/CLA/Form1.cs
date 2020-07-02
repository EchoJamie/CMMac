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

namespace CLA
{
    public partial class Form1 : Form
    {
        //被加数
        int[] a = new int[8];
        //加数
        int[] b = new int[8];
        //本地进位
        int[] d = new int[8];
        //传送条件
        int[] t = new int[8];
        //进位
        int[] c = new int[9];
        //结果
        int[] re = new int[8];
        int step = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Num1_Leave(object sender, EventArgs e)
        {
            String Number1 = Convert.ToString(Convert.ToSByte(Num1dec.Text), 2).PadLeft(8, '0');
            Num1bin.Text = Number1;
            int temp = Number1.Length - 1;
            for (int i = temp; i >= temp-7; i--)
            {
                a[temp - i] = Number1[i] - '0';
            }
            a0text.Text = Number1[temp--].ToString();
            a1text.Text = Number1[temp--].ToString();
            a2text.Text = Number1[temp--].ToString();
            a3text.Text = Number1[temp--].ToString();
            a4text.Text = Number1[temp--].ToString();
            a5text.Text = Number1[temp--].ToString();
            a6text.Text = Number1[temp--].ToString();
            a7text.Text = Number1[temp--].ToString();
        }

        private void Num2_Leave(object sender, EventArgs e)
        {
            String Number2 = Convert.ToString(Convert.ToSByte(Num2dec.Text), 2).PadLeft(8, '0');
            Num2bin.Text = Number2;
            int temp = Number2.Length - 1;
            for (int i = temp; i >= temp-7; i--)
            {
                b[temp - i] = Number2[i] - '0';
            }
            b0text.Text = Number2[temp--].ToString();
            b1text.Text = Number2[temp--].ToString();
            b2text.Text = Number2[temp--].ToString();
            b3text.Text = Number2[temp--].ToString();
            b4text.Text = Number2[temp--].ToString();
            b5text.Text = Number2[temp--].ToString();
            b6text.Text = Number2[temp--].ToString();
            b7text.Text = Number2[temp--].ToString();
        }

        private void First()
        {
            d[0] = a[0] & b[0];
            t[0] = a[0] | b[0];
            c[1] = d[0] | t[0] & c[0];
            d[1] = a[1] & b[1];
            t[1] = a[1] | b[1];
            c[2] = d[1] | t[1] & (d[0] | t[0] & c[0]);
            d[2] = a[2] & b[2];
            t[2] = a[2] | b[2];
            c[3] = d[2] | t[2] & (d[1] | t[1] & (d[0] | t[0] & c[0]));
            d[3] = a[3] & b[3];
            t[3] = a[3] | b[3];
            c[4] = d[3] | t[3] & (d[2] | t[2] & (d[1] | t[1] & (d[0] | t[0] & c[0])));
            //ui值修改
            //本地进位
            d0text.Text = d[0].ToString();
            d1text.Text = d[1].ToString();
            d2text.Text = d[2].ToString();
            d3text.Text = d[3].ToString();
            //传送条件
            t3text.Text = t[3].ToString();
            t0text.Text = t[0].ToString();
            t1text.Text = t[1].ToString();
            t2text.Text = t[2].ToString();
            //进位
            c1text.Text = c[1].ToString();
            c2text.Text = c[2].ToString();
            c3text.Text = c[3].ToString();
            c4text.Text = c[4].ToString();
        }
        private void Second()
        {
            d[4] = a[4] & b[4];
            t[4] = a[4] | b[4];
            c[5] = d[4] | t[4] & c[4];
            d[5] = a[5] & b[5];
            t[5] = a[5] | b[5];
            c[6] = d[5] | t[5] & (d[4] | t[4] & c[4]);
            d[6] = a[6] & b[6];
            t[6] = a[6] | b[6];
            c[7] = d[6] | t[6] & (d[5] | t[5] & (d[4] | t[4] & c[4]));
            d[7] = a[7] & b[7];
            t[7] = a[7] | b[7];
            c[8] = d[7] | t[7] & (d[6] | t[6] & (d[5] | t[5] & (d[4] | t[4] & c[4])));
            //ui值修改
            //本地进位
            d4text.Text = d[4].ToString();
            d5text.Text = d[5].ToString();
            d6text.Text = d[6].ToString();
            d7text.Text = d[7].ToString();
            //传送条件
            t4text.Text = t[4].ToString();
            t5text.Text = t[5].ToString();
            t6text.Text = t[6].ToString();
            t7text.Text = t[7].ToString();
            //进位
            c5text.Text = c[5].ToString();
            c6text.Text = c[6].ToString();
            c7text.Text = c[7].ToString();
            c8text.Text = c[8].ToString();
        }
        private void Third()
        {
            string ans = "";
            for(int i = 0;i < 8;i++)
            {
                re[i] = (a[i] + b[i] + c[i]) % 2;
                //MessageBox.Show((c[i] == 1 && d[i] == t[i] || c[i] == 0 && d[i] != t[i])? "1" : "0");
                //re[i] = ((a[i] == 1 & b[i] == 0) | (a[i] == 0 & b[i] == 1)) ? 1 : 0;
                //re[i] = ((c[i] == 1 & re[i] == 0) | (c[i] == 0 & re[i] == 1)) ? 1 : 0;
                //MessageBox.Show(re[i].ToString());
                ans = re[i].ToString() + ans;
            }
            //MessageBox.Show(ans);
            AnswerDec.Text = Convert.ToSByte(ans, 2).ToString();
            AnswerBin.Text = ans;
        }

        private void test_Click(object sender, EventArgs e)
        {
            First();
            Thread.Sleep(3000);
            Second();
            Thread.Sleep(1000);
            Third();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch(step)
            {
                case 0: First();step++; break;
                case 1: Second();step++; break;
                case 2: Third(); step = 0; break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            First();
            Second();
            Third();
        }
    }
}
