using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SpeechLib;

namespace misiu29_s_tts
{
    public partial class Form1 : Form
    {
        SpVoice voice = new SpVoice();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (ISpeechObjectToken Token in voice.GetVoices(string.Empty, string.Empty))
            {
                comboBox1.Items.Add(Token.GetDescription(49));
            }
            comboBox1.SelectedIndex = 0;
        }
        private void Tts()
        {
            //SpVoice voice = new SpVoice();
            voice.Rate = int.Parse(textBox1.Text);
            voice.Volume = int.Parse(textBox2.Text);
            voice.Voice = voice.GetVoices().Item(0);
            voice.Speak(richTextBox1.Text.ToString());
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            voice.Voice = voice.GetVoices(string.Empty, string.Empty).Item(comboBox1.SelectedIndex);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Tts();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TtsSave();
        }
        private void TtsSave()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "保存声音";
            saveFileDialog.Filter = "*.wav|*.wav|*.mp3|*.mp3";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult saveDialog = saveFileDialog.ShowDialog();
            try
            {

                if (saveDialog == System.Windows.Forms.DialogResult.OK)
                {
                    SpeechLib.SpeechStreamFileMode SSFM = SpeechLib.SpeechStreamFileMode.SSFMCreateForWrite;
                    SpeechLib.SpFileStream sfs = new SpeechLib.SpFileStream();
                    sfs.Open(saveFileDialog.FileName, SSFM, false);
                    voice.AudioOutputStream = sfs;
                    Tts();
                    voice.WaitUntilDone(System.Threading.Timeout.Infinite);
                    sfs.Close();
                    System.Diagnostics.Process.Start("Explorer.exe", string.Format(@"/select,{0}", saveFileDialog.FileName));//打开wav目录并选中文件
                }

            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && (e.KeyChar != 45))//这是允许输入退格键
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {
                    e.Handled = true;
                }
            }
            if (e.KeyChar == (char)('-') && ((TextBox)sender).Text.IndexOf('-') != -1)
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {
                    e.Handled = true;
                }
            }
        }

        /*
public void setDescription(string name)  //传语音名字 Microsoft Lili 
{
for (int i = 0; i < voice.GetVoices().Count; i++) //遍历语音库
{
string desc = voice.GetVoices().Item(i).GetDescription(); // 获取名字
if (desc.Equals(name)) //判断
{
voice.Voice = voice.GetVoices().Item(i); //赋值
}
}

}
*/

    }
}
