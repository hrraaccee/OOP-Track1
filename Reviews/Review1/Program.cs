using System;
using System.Collections.Generic;
using System.Linq;

var product = new Product("Клавиатура", 2500m, 10, "Электроника", "Проводная");
var seller = new Seller("Анна", "S-01", 50000m, "Внутренний номер 101");
var store = new Store("Техника", "Тюмень", "09:00–20:00");
seller.AddProduct(product); store.AddSeller(seller);
Console.WriteLine(product.GetProductInfo());
Console.WriteLine(seller.GetSellerInfo());
Console.WriteLine(store.GetStoreInfo());
store.ListProducts();
seller.SellProduct(product, 3);
Verify.That(product.Quantity == 7 && product.GetTotalPrice() == 17500m, "Продажа");
Verify.Throws<InvalidOperationException>(() => seller.SellProduct(product, 8));
Verify.Throws<ArgumentOutOfRangeException>(() => seller.SellProduct(product, -1));
Verify.Throws<ArgumentOutOfRangeException>(() => product.Price = -1m);
Verify.That(product.Quantity == 7, "Неуспешная продажа не меняет склад");
product.UpdateQuantity(2);
Verify.That(product.Quantity == 9, "Пополнение");
Console.WriteLine("Проверки выполнены успешно.");

public class Product
{
    private string name;
    private decimal price;
    private int quantity;
    protected string category;
    internal string description;
    public string Name
    {
        get => name;
        set => name = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Пустое название") : value;
    }
    public decimal Price
    {
        get => price;
        set { if (value < 0) throw new ArgumentOutOfRangeException(nameof(value)); price = value; }
    }
    public int Quantity
    {
        get => quantity;
        private set { if (value < 0) throw new InvalidOperationException("Недостаточно товара"); quantity = value; }
    }
    public string Category => category;
    public string Description { get => description; set => description = value; }
    public Product(string name, decimal price, int quantity, string category, string description)
    {
        this.name = ""; this.category = category; this.description = description;
        Name = name; Price = price; Quantity = quantity;
    }
    public decimal GetTotalPrice() => Price * Quantity;
    public void UpdateQuantity(int amount) => Quantity = checked(Quantity + amount);
    public string GetProductInfo() => $"{Name}, {Category}, {Description}: {Price} руб., {Quantity} шт.";
}
public class Seller
{
    private string name;
    private string employeeId;
    protected decimal salary;
    internal string contactInfo;
    private readonly List<Product> products = new();
    public string Name { get => name; set => name = Required(value); }
    public string EmployeeId { get => employeeId; set => employeeId = Required(value); }
    public decimal Salary
    {
        get => salary;
        set { if (value < 0) throw new ArgumentOutOfRangeException(nameof(value)); salary = value; }
    }
    public string ContactInfo { get => contactInfo; set => contactInfo = value; }
    public IReadOnlyList<Product> Products => products.AsReadOnly();
    public Seller(string name, string employeeId, decimal salary, string contactInfo)
    {
        this.name = Required(name); this.employeeId = Required(employeeId); this.contactInfo = contactInfo; Salary = salary;
    }
    private static string Required(string value) => string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Пустое значение") : value;
    public void AddProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (!products.Contains(product)) products.Add(product);
    }
    public void SellProduct(Product product, int quantity)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        if (!products.Contains(product)) throw new InvalidOperationException("Товар не закреплён за продавцом");
        product.UpdateQuantity(-quantity);
    }
    public string GetSellerInfo() => $"{Name}, {EmployeeId}, зарплата {Salary}, контакт {ContactInfo}";
}
public class Store
{
    private string storeName;
    private string location;
    public string storeHours;
    private readonly List<Seller> sellers = new();
    public string StoreName { get => storeName; set => storeName = Required(value); }
    public string Location { get => location; set => location = Required(value); }
    public string StoreHours { get => storeHours; set => storeHours = Required(value); }
    public Store(string name, string location, string hours)
    {
        storeName = Required(name); this.location = Required(location); storeHours = Required(hours);
    }
    private static string Required(string value) => string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Пустое значение") : value;
    public void AddSeller(Seller seller)
    {
        ArgumentNullException.ThrowIfNull(seller);
        if (!sellers.Contains(seller)) sellers.Add(seller);
    }
    public void ListProducts()
    {
        foreach (var product in sellers.SelectMany(s => s.Products).Distinct()) Console.WriteLine(product.GetProductInfo());
    }
    public string GetStoreInfo() => $"{StoreName}, {Location}, часы {StoreHours}, продавцов {sellers.Count}";
}

public static class Verify
{
    public static void That(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    public static void Throws<T>(Action action) where T : Exception
    {
        try { action(); }
        catch (T) { return; }
        throw new InvalidOperationException($"Ожидалось исключение {typeof(T).Name}");
    }
}
