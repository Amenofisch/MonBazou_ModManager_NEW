namespace MonBazou_ModManager;

public partial class MainWindow : Form
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        Handler.Init();
        changelogTextBox.Text = await Handler.GetChangelog();
        var mods = (await Handler.GetModListData()).ToList();
        
        dataGridView1.DataSource = mods;
        dataGridView1.Columns["Reason"].Visible = false;
        dataGridView1.Columns["Disabled"].Visible = false;
        dataGridView1.Columns["Type"].Visible = false;
        dataGridView1.Columns["FileName"].Visible = false;
        dataGridView1.Columns["ModVersion"].Visible = false;
        dataGridView1.Columns["GameVersion"].Visible = false;
        dataGridView1.Columns["Description"].Visible = false;
        dataGridView1.Columns["downloadLink"].Visible = false;
    }

    private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }
}