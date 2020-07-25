using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToDoApp
{
    public partial class Form1 : Form
    {
        string clip {
            get
            {
                return richTextBox1.Text;
            }
            set
            {
                richTextBox1.Text = value;
            }
        }
        List<Info> infos;
        public Form1()
        {
            InitializeComponent();
            if (File.Exists("data.json"))
            {
                try
                {
                    infos = JsonConvert.DeserializeObject<List<Info>>(File.ReadAllText("data.json"));

                }
                catch (Exception)
                {

                    infos = new List<Info>();
                }
            }
            else
            {
                infos = new List<Info>();


            }

            if (File.Exists("clip.json"))
            {
                clip= File.ReadAllText("clip.json");
            }
            refresh();
        }

        bool change = false;
        void refresh()
        {
            infos = infos.OrderByDescending(o => o.done).ToList();
            listView1.Items.Clear();
            change = true;
            foreach (var info in infos)
            {
                var lv = new ListViewItem(info.tasks);
                listView1.Items.Add(lv);
                lv.Checked = info.done;
            }
            change = false;
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            if (textBox_task.Text.Trim() != "")
            {
                var info = new Info()
                {
                    tasks = textBox_task.Text.Trim(),
                    done = false
                };
                infos.Add(info);
                listView1.Items.Add(new ListViewItem(info.tasks)
                {
                    Checked = info.done
                });
                textBox_task.Text = "";
                write();
            }
        }

        public void write()
        {
            File.WriteAllText("data.json", JsonConvert.SerializeObject(infos));
            
        }

        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!change)
            {
                infos[e.Index].done = e.NewValue == CheckState.Checked;
                write();
            }
        }

        private void delToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var itm = listView1.SelectedItems[0];
                infos.RemoveAt(itm.Index);
                listView1.Items.RemoveAt(itm.Index);
                write();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText("clip.json", clip);
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            var k = new SaveFileDialog();
            if (k.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(k.FileName, clip);

            }
        }
    }
}
