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

namespace ComputerModelMachine
{
    public partial class Form1 : Form
    {
        byte[] R = new byte[8];
        short PC, IR, MAR, MDR, IMAR, IMDR, SR, DR;
        byte[] DataMemory = new byte[32];
        //PSW 含义解释
        //H(半进位标志) S(符号标志位（NV异或值）)
        //V(有符号溢出) C(无符号溢出) 
        //N(运算结果为负) Z(运算结果为零)
        bool[] PSW = new bool[6];
        string PSW_str = "0 0 0 0 0 0 0 0  ";
        short InstructCount = 0x1000;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //绘制 Listview_DataMemory.Columns
            Listview_DataMemory.Columns.Add("DAD", 45, HorizontalAlignment.Left);
            Listview_DataMemory.Columns.Add("DValue", 72, HorizontalAlignment.Left);
            
            //TextBox[] textBox_R = new TextBox[] { Textbox_R0, Textbox_R1, Textbox_R2, Textbox_R3, Textbox_R4, Textbox_R5, Textbox_R6, Textbox_R7};
        }
        //初始化  (置零)
        private void btn_Reset_Click(object sender, EventArgs e)
        {
            TextBox[] textBox_R = new TextBox[] { Textbox_R0, Textbox_R1, Textbox_R2, Textbox_R3, Textbox_R4, Textbox_R5, Textbox_R6, Textbox_R7 };
            for (int i = 0; i < textBox_R.Length; i++)
                ToTextbox(textBox_R[i], R[i], 0);
            for (int i = 0; i < DataMemory.Length; i++)
                DataMemory[i] = 0;
            AddListview_DM();
            ToTextbox(Textbox_PC, PC, 0);
            ToTextbox(Textbox_IR, IR, 0);
            ToTextbox(Textbox_SR, SR, 0);
            ToTextbox(Textbox_DR, DR, 0);
            ToTextbox(Textbox_MAR, MAR, 0);
            ToTextbox(Textbox_MDR, MDR, 0);
            ToTextbox(Textbox_IMAR, IMAR, 0);
            ToTextbox(Textbox_IMDR, IMDR, 0);
            ToPSW('H', false);
            ToPSW('S', false);
            ToPSW('V', false);
            ToPSW('C', false);
            ToPSW('N', false);
            ToPSW('Z', false);
            Listbox_Code.Items.Clear();
            Listbox_MachineCode.Items.Clear();
            Listview_OrderMemory.Items.Clear();
        }
        //打开.data文件
        private void btn_InputFile_Click(object sender, EventArgs e)
        {
            btn_Reset_Click(sender, e);
            string filePath = @"C:\Users\longyuan\Documents\Tencent Files\1144916545\FileRecv\test.data";   //测试使用代码
            /*
             * 因测试被注释掉 
            string filePath = "";
            OpenFileDialog openfile = new OpenFileDialog(); //选择文件窗口
            openfile.Title = "请选择测试文件"; //弹窗标题
            openfile.Filter = "(*.data)|*.data|(*.*)|*.*";  //文件筛选器
            openfile.Multiselect = false;   //是否允许多选
            openfile.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//初始目录  桌面
            if(openfile.ShowDialog() == DialogResult.OK)
            {
                filePath = openfile.FileName;   //获取文件路径*/
                StreamReader fileReader = new StreamReader(filePath, Encoding.Default); //文件读取流
                string s = "";
                while(s != null)
                {
                    s = fileReader.ReadLine();
                    if(!string.IsNullOrEmpty(s))
                    {
                        Listbox_Code.Items.Add(s);  //导入listbox_code
                        AddListbox_MC(AssemblyToCode(s));   //同步翻译为机器码至listbox_machinecode
                        AddListview_IM(s);  //同步至指令存储单元
                    }
                }
                fileReader.Close();
            //}     //因测试被注释掉
        }

        /**
          * 修改一般Textbox寄存器
          * @param textbox 寄存器对应显示控件
          * @param Regiser 对应寄存器属性
          * @param num 目的修改值
          */
        private void ToTextbox(TextBox textBox, short Register, int num)
        {
            textBox.Text = Convert.ToString(num, 16).PadLeft(4, '0').ToUpper();
            Register = Convert.ToInt16(num);
        }
        /**
         * 修改PSW寄存器
         * @param which 键位  可选 H S V C N Z
         * @param Boolean 
         * @return
         */
        private void ToPSW(char which, bool boolean)
        {
            char ch = boolean ? '1' : '0';
            switch (which)
            {
                case 'H':
                    PSW_str = PSW_str.Substring(0, 4) + ch + PSW_str.Substring(5, PSW_str.Length - 5);
                    PSW[0] = boolean;
                    break;
                case 'S':
                    PSW_str = PSW_str.Substring(0, 6) + ch + PSW_str.Substring(7, PSW_str.Length - 7);
                    PSW[1] = boolean;
                    break;
                case 'V':
                    PSW_str = PSW_str.Substring(0, 8) + ch + PSW_str.Substring(9, PSW_str.Length - 9);
                    PSW[2] = boolean;
                    break;
                case 'C':
                    PSW_str = PSW_str.Substring(0, 10) + ch + PSW_str.Substring(11, PSW_str.Length - 11);
                    PSW[3] = boolean;
                    break;
                case 'N':
                    PSW_str = PSW_str.Substring(0, 12) + ch + PSW_str.Substring(13, PSW_str.Length - 13);
                    PSW[4] = boolean;
                    break;
                case 'Z':
                    PSW_str = PSW_str.Substring(0, 14) + ch + PSW_str.Substring(15, PSW_str.Length - 15);
                    PSW[5] = boolean;
                    break;
                default:break;
            }
            Textbox_PSW.Text = PSW_str;
        }
        private void MicroInstruct()
        {

        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            InstructCount = 0x1000;
            ToTextbox(Textbox_PC, PC, InstructCount);
            ChangeDM(5, 0x1A);
        }
        /**
          * 汇编指令转机器码
          */
        private string AssemblyToCode(string Assembly)
        {
            string[] array = Assembly.Split(new char[] { ' ', ',', '，'});
            string code = "";
            for(int i = 0;i < array.Length - 1;i++)
            {
                string temp = null;
                if(array[i] == "")
                {
                    temp = array[i];
                    array[i] = array[i + 1];
                    array[i + 1] = temp;
                }
            }
            switch(array[0].ToUpper())    //识别操作符
            {
                case "NOP": code += "0000"; break;
                case "ADD": code += "0001"; break;
                case "SUB": code += "0010"; break;
                case "AND": code += "0011"; break;
                case "INC": code += "0100"; break;
                case "DEC": code += "0101"; break;
                case "NEC": code += "0110"; break;
                case "JMP": code += "0111"; break;
                case "JC": code += "1000"; break;
                case "LD": code += "1001"; break;
                case "MOV": code += "1010"; break;
                case "LDI": code += "1110"; break;
                default:/* MessageBox.Show("Instruct OP Translate Error!");*/ break;
            }
            if (array[0].ToUpper() == "LD" || array[0].ToUpper() == "LDI")
            {
                code += Convert.ToString(Convert.ToInt16(array[2], 16), 2).PadLeft(8, '0');
                code += '0' + OPNum(array[1]);
            }
            else
            {
                if (array.Length > 2 && array[2] != "")
                    code += AddressingMode(array[2]);
                else
                    code += "000000";
                if (array.Length > 1 && array[1] != "")
                    code += AddressingMode(array[1]);
                else
                    code += "000000";
            }
            return code;
        }
        /**
         * 寻址方式---判断 + 操作数获取 
         */
        private string AddressingMode(string str)
        {
            string x = "";
            string temp = "";
            if (str[0] == '@')
            {
                x += "011";
                temp = str.Substring(2, 2);
            }
            else if (str[0] == 'X')
            {
                x += "100";
                temp = str.Substring(2, 2);
            }
            else if (str[0] == '（' || str[0] == '(')
            {
                if (str.Length >= 5 && str[4] == '+')
                    x += "010";
                else
                    x += "001";
                temp = str.Substring(1, 2);
            }
            else
            {
                x += "000";
                temp = str;
            }
            x += OPNum(temp);
            return x;
        }
        /**
         * 寄存器判断
         */
        private string OPNum(string temp)
        {
            string x = "";
            switch (temp)
            {
                case "R0": x += "000"; break;
                case "R1": x += "001"; break;
                case "R2": x += "010"; break;
                case "R3": x += "011"; break;
                case "R4": x += "100"; break;
                case "R5": x += "101"; break;
                case "R6": x += "110"; break;
                case "R7": x += "111"; break;
                default: break;
            }
            return x;
        }
        /**
         * 指令存储单元---添加项
         */
        private void AddListview_IM(string str)
        {
            ListViewItem lvi = new ListViewItem(Convert.ToString(InstructCount, 16).ToUpper());
            InstructCount++;
            lvi.SubItems.Add(AssemblyToCode(str));
            Listview_OrderMemory.Items.Add(lvi);
        }
        /**
         * 机器码---添加项
         */
        private void AddListbox_MC(string str)
        {
            string temp = "";
            for (int i = 0; i < str.Length; i+=4)
            {
                temp += str.Substring(i, 4);
                temp += " ";
            }
            Listbox_MachineCode.Items.Add(temp);
        }
        /**
         * 清除并赋值 Listview_DataMemory
         * Warning：DataMemory 中值不可为空
         */
        private void AddListview_DM()
        {
            if(Listview_DataMemory.Columns.Count > 0)
                Listview_DataMemory.Columns.Clear();
            if (Listview_DataMemory.Items.Count > 0)
                Listview_DataMemory.Clear();
            Listview_DataMemory.Columns.Add("DAD", 45, HorizontalAlignment.Left);
            Listview_DataMemory.Columns.Add("DValue", 72, HorizontalAlignment.Left);
            for (int Address = 0; Address < DataMemory.Length; Address++)
            {
                ListViewItem lv = new ListViewItem(Convert.ToString(Address, 16).PadLeft(4, '0').ToUpper());
                lv.SubItems.Add(Convert.ToString(DataMemory[Address], 16).PadLeft(4, '0').ToUpper());
                Listview_DataMemory.Items.Add(lv);
            }
        }
        private void OP(string MachineCode)
        {

        }
        /**
         * 修改Listview_DataMemory 中某项
         * @param Address 地址
         * @param num 目的值
         */
        private void ChangeDM(short Address, byte num)
        {
            if(Address < 32)
            {
                DataMemory[Address] = num;
                Listview_DataMemory.Items[Address].SubItems[1].Text = Convert.ToString(DataMemory[Address], 16).PadLeft(4, '0').ToUpper();
            }
        }
    }
}
