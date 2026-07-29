namespace TechStore.App.Views;
partial class CrudForm
{
    TextBox search = null!;
    DataGridView grid = null!;

    void InitializeComponent()
    {
        Width = 950; Height = 600; Font = new Font("Segoe UI", 9F);
        search = new TextBox { Left = 20, Top = 25, Width = 300 };
        var find = new Button { Left = 330, Top = 23, Text = "Buscar / Actualizar", Width = 140 }; find.Click += Refresh_Click;
        var add = new Button { Left = 480, Top = 23, Text = "Nuevo", Width = 90 }; add.Click += Action_Click;
        var edit = new Button { Left = 580, Top = 23, Text = "Editar", Width = 90 }; edit.Click += Action_Click;
        var disable = new Button { Left = 680, Top = 23, Text = "Desactivar", Width = 100 }; disable.Click += Action_Click;
        grid = new DataGridView { Left = 20, Top = 70, Width = 890, Height = 450, ReadOnly = true, AutoGenerateColumns = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, AllowUserToAddRows = false, RowHeadersVisible = false, BackgroundColor = Color.White };
        Controls.AddRange([new Label { Left = 20, Top = 7, Text = "Buscar por nombre, código o documento:" }, search, find, add, edit, disable, grid]);
    }
}
