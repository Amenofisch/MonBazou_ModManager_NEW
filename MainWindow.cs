using System.ComponentModel;

namespace MonBazou_ModManager
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            Handler.Init();
            List<Mod> mods = new List<Mod>();
            changelogTextBox.Text = await Handler.GetChangelog();
            foreach(Mod Mod in Handler.GetModListData())
            {
                mods.Add(Mod);
            }
            dataGridView1.DataSource = mods;
            dataGridView1.Columns["Reason"].Visible = false;
            dataGridView1.Columns["Disabled"].Visible = false;
            dataGridView1.Columns["Type"].Visible = false;
            dataGridView1.Columns["dllName"].Visible = false;
            dataGridView1.Columns["zipName"].Visible = false;
            dataGridView1.Columns["ModVersion"].Visible = false;
            dataGridView1.Columns["GameVersion"].Visible = false;
            dataGridView1.Columns["Description"].Visible = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}