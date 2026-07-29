using Microsoft.EntityFrameworkCore;
using TechStore.App.Controllers;
using TechStore.App.Data;
using TechStore.App.Enums;
using TechStore.App.Models;

namespace TechStore.App.Views;

public partial class CrudForm : Form
{
    readonly string module;
    readonly AppController controller = new();

    public CrudForm(string module)
    {
        this.module = module;
        InitializeComponent();
        Text = module;
        Shown += async (_, _) => await LoadData();
        search.KeyDown += async (_, e) => { if (e.KeyCode == Keys.Enter) await LoadData(); };
    }

    async Task LoadData()
    {
        grid.DataSource = module == "Productos"
            ? await controller.ProductsAsync(search.Text.Trim())
            : await controller.EntitiesAsync(module, search.Text.Trim());
    }

    int? SelectedId() => grid.CurrentRow?.Cells["Id"].Value is int id ? id : null;
    async void Refresh_Click(object? sender, EventArgs e) => await LoadData();

    async void Action_Click(object? sender, EventArgs e)
    {
        try
        {
            var action = ((Button)sender!).Text;
            if (action == "Desactivar")
            {
                var id = SelectedId() ?? throw new InvalidOperationException("Seleccione un registro.");
                if (MessageBox.Show("¿Confirma cambiar el estado del registro?", module,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                await ToggleActive(id);
            }
            else
            {
                var id = action == "Editar"
                    ? SelectedId() ?? throw new InvalidOperationException("Seleccione un registro.")
                    : (int?)null;
                await Edit(id);
            }
            await LoadData();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.InnerException?.Message ?? ex.Message, "Validación",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    async Task ToggleActive(int id)
    {
        await using var db = new TechStoreDbContext();
        object entity = module switch
        {
            "Productos" => await db.Products.FindAsync(id) ?? throw NotFound(),
            "Categorías" => await db.Categories.FindAsync(id) ?? throw NotFound(),
            "Clientes" => await db.Customers.FindAsync(id) ?? throw NotFound(),
            "Sucursales" => await db.Branches.FindAsync(id) ?? throw NotFound(),
            _ => await db.Sellers.FindAsync(id) ?? throw NotFound()
        };
        switch (entity)
        {
            case Product x: x.IsActive = !x.IsActive; x.UpdatedAt = DateTime.Now; break;
            case Category x: x.IsActive = !x.IsActive; break;
            case Customer x: x.IsActive = !x.IsActive; break;
            case Branch x: x.IsActive = !x.IsActive; break;
            case Seller x: x.IsActive = !x.IsActive; break;
        }
        await db.SaveChangesAsync();
    }

    async Task Edit(int? id)
    {
        await using var db = new TechStoreDbContext();
        switch (module)
        {
            case "Productos": await EditProduct(db, id); break;
            case "Categorías": await EditCategory(db, id); break;
            case "Clientes": await EditCustomer(db, id); break;
            case "Sucursales": await EditBranch(db, id); break;
            default: await EditSeller(db, id); break;
        }
    }

    static async Task EditProduct(TechStoreDbContext db, int? id)
    {
        var x = id.HasValue ? await db.Products.FindAsync(id.Value) ?? throw NotFound() : new Product();
        var categories = await db.Categories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync();
        if (categories.Count == 0) throw new InvalidOperationException("Primero debe crear una categoría activa.");
        var branches = await db.Branches.Where(b => b.IsActive).OrderBy(b => b.Name).ToListAsync();
        var inventories = id.HasValue
            ? await db.Inventories.Where(i => i.ProductId == id.Value).ToDictionaryAsync(i => i.BranchId)
            : new Dictionary<int, Inventory>();
        using var d = new RecordDialog(id.HasValue ? "Editar producto" : "Nuevo producto");
        var code = d.TextField("Código", x.Code); var name = d.TextField("Nombre", x.Name);
        var description = d.TextField("Descripción", x.Description ?? "");
        var category = d.ComboField("Categoría", categories, "Name", "Id", x.CategoryId);
        var price = d.DecimalField("Precio", x.Price, 0.01m, 999999999m);
        var stockFields = branches.Select(branch =>
        {
            inventories.TryGetValue(branch.Id, out var inventory);
            return new ProductStockFields(
                branch,
                inventory,
                d.IntegerField($"Stock - {branch.Name}", inventory?.Stock ?? 0, 0, 1000000),
                d.IntegerField($"Mínimo - {branch.Name}", inventory?.MinimumStock ?? 0, 0, 1000000));
        }).ToList();
        if (d.ShowDialog() != DialogResult.OK) return;
        Require(code.Text, "código"); Require(name.Text, "nombre");
        x.Code = code.Text.Trim(); x.Name = name.Text.Trim(); x.Description = NullIfEmpty(description.Text);
        x.CategoryId = (int)category.SelectedValue; x.Price = price.Value; x.UpdatedAt = DateTime.Now;
        if (!id.HasValue) db.Products.Add(x);
        await db.SaveChangesAsync();
        foreach (var fields in stockFields)
        {
            var inventory = fields.Inventory ?? new Inventory { ProductId = x.Id, BranchId = fields.Branch.Id };
            inventory.Stock = (int)fields.Stock.Value;
            inventory.MinimumStock = (int)fields.MinimumStock.Value;
            if (fields.Inventory is null) db.Inventories.Add(inventory);
        }
        await db.SaveChangesAsync();
    }

    static async Task EditCategory(TechStoreDbContext db, int? id)
    {
        var x = id.HasValue ? await db.Categories.FindAsync(id.Value) ?? throw NotFound() : new Category();
        using var d = new RecordDialog(id.HasValue ? "Editar categoría" : "Nueva categoría");
        var name = d.TextField("Nombre", x.Name); var description = d.TextField("Descripción", x.Description ?? "");
        if (d.ShowDialog() != DialogResult.OK) return;
        Require(name.Text, "nombre"); x.Name = name.Text.Trim(); x.Description = NullIfEmpty(description.Text);
        if (!id.HasValue) db.Categories.Add(x); await db.SaveChangesAsync();
    }

    static async Task EditCustomer(TechStoreDbContext db, int? id)
    {
        var x = id.HasValue ? await db.Customers.FindAsync(id.Value) ?? throw NotFound() : new Customer();
        using var d = new RecordDialog(id.HasValue ? "Editar cliente" : "Nuevo cliente");
        var document = d.TextField("CUIT / documento", x.DocumentNumber ?? ""); var name = d.TextField("Razón social", x.BusinessName);
        var email = d.TextField("Correo", x.Email ?? ""); var phone = d.TextField("Teléfono", x.Phone ?? ""); var address = d.TextField("Dirección", x.Address ?? "");
        var type = d.EnumField("Tipo", x.CustomerType); var discount = d.DecimalField("Descuento %", x.DiscountPercentage, 0, 100);
        if (d.ShowDialog() != DialogResult.OK) return;
        Require(name.Text, "razón social"); x.DocumentNumber = NullIfEmpty(document.Text); x.BusinessName = name.Text.Trim();
        x.Email = NullIfEmpty(email.Text); x.Phone = NullIfEmpty(phone.Text); x.Address = NullIfEmpty(address.Text);
        x.CustomerType = (CustomerType)type.SelectedItem!; x.DiscountPercentage = discount.Value;
        if (!id.HasValue) db.Customers.Add(x); await db.SaveChangesAsync();
    }

    static async Task EditBranch(TechStoreDbContext db, int? id)
    {
        var x = id.HasValue ? await db.Branches.FindAsync(id.Value) ?? throw NotFound() : new Branch();
        using var d = new RecordDialog(id.HasValue ? "Editar sucursal" : "Nueva sucursal");
        var name = d.TextField("Nombre", x.Name); var address = d.TextField("Dirección", x.Address); var phone = d.TextField("Teléfono", x.Phone);
        if (d.ShowDialog() != DialogResult.OK) return;
        Require(name.Text, "nombre"); Require(address.Text, "dirección");
        x.Name = name.Text.Trim(); x.Address = address.Text.Trim(); x.Phone = phone.Text.Trim();
        if (!id.HasValue) db.Branches.Add(x); await db.SaveChangesAsync();
        if (!id.HasValue)
        {
            var products = await db.Products.Where(p => p.IsActive).ToListAsync();
            db.Inventories.AddRange(products.Select(p => new Inventory { ProductId = p.Id, BranchId = x.Id, Stock = 0, MinimumStock = 0 }));
            await db.SaveChangesAsync();
        }
    }

    static async Task EditSeller(TechStoreDbContext db, int? id)
    {
        var x = id.HasValue ? await db.Sellers.FindAsync(id.Value) ?? throw NotFound() : new Seller();
        using var d = new RecordDialog(id.HasValue ? "Editar vendedor" : "Nuevo vendedor");
        var name = d.TextField("Nombre", x.Name); var document = d.TextField("Legajo / documento", x.DocumentNumber);
        var email = d.TextField("Correo", x.Email ?? ""); var phone = d.TextField("Teléfono", x.Phone ?? "");
        if (d.ShowDialog() != DialogResult.OK) return;
        Require(name.Text, "nombre"); Require(document.Text, "legajo / documento");
        x.Name = name.Text.Trim(); x.DocumentNumber = document.Text.Trim(); x.Email = NullIfEmpty(email.Text); x.Phone = NullIfEmpty(phone.Text);
        if (!id.HasValue) db.Sellers.Add(x); await db.SaveChangesAsync();
    }

    static Exception NotFound() => new InvalidOperationException("El registro ya no existe.");
    static void Require(string value, string field) { if (string.IsNullOrWhiteSpace(value)) throw new InvalidOperationException($"El campo {field} es obligatorio."); }
    static string? NullIfEmpty(string value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

sealed class RecordDialog : Form
{
    int row;
    public RecordDialog(string title)
    {
        Text = title; Width = 510; StartPosition = FormStartPosition.CenterParent; FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false; MinimizeBox = false; AutoScroll = true;
    }
    T Add<T>(string label, T control) where T : Control
    {
        Controls.Add(new Label { Text = label + ":", Left = 20, Top = 24 + row * 42, Width = 145 });
        control.Left = 170; control.Top = 20 + row * 42; control.Width = 290; Controls.Add(control); row++; return control;
    }
    public TextBox TextField(string label, string value) => Add(label, new TextBox { Text = value, MaxLength = 150 });
    public NumericUpDown DecimalField(string label, decimal value, decimal min, decimal max)
    {
        var input = new NumericUpDown { Minimum = min, Maximum = max, DecimalPlaces = 2, ThousandsSeparator = true };
        input.Value = Math.Clamp(value, min, max);
        return Add(label, input);
    }
    public NumericUpDown IntegerField(string label, int value, int min, int max)
    {
        var input = new NumericUpDown { Minimum = min, Maximum = max, DecimalPlaces = 0, ThousandsSeparator = true };
        input.Value = Math.Clamp(value, min, max);
        return Add(label, input);
    }
    public ComboBox EnumField<T>(string label, T value) where T : struct, Enum => Add(label, new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, DataSource = Enum.GetValues<T>(), SelectedItem = value });
    public ComboBox ComboField(string label, object data, string display, string value, int selected)
    {
        var combo = Add(label, new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, DataSource = data, DisplayMember = display, ValueMember = value });
        if (selected > 0) combo.SelectedValue = selected; return combo;
    }
    protected override void OnShown(EventArgs e)
    {
        Height = Math.Min(700, 105 + row * 42); var ok = new Button { Text = "Guardar", Left = 270, Top = 25 + row * 42, Width = 90, DialogResult = DialogResult.OK };
        var cancel = new Button { Text = "Cancelar", Left = 370, Top = ok.Top, Width = 90, DialogResult = DialogResult.Cancel };
        Controls.AddRange([ok, cancel]); AcceptButton = ok; CancelButton = cancel; base.OnShown(e);
    }
}

sealed record ProductStockFields(Branch Branch, Inventory? Inventory, NumericUpDown Stock, NumericUpDown MinimumStock);
