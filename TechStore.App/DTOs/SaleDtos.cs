using TechStore.App.Enums;
namespace TechStore.App.DTOs;
public record SaleLineRequest(int ProductId,int Quantity); public record CreateSaleRequest(int CustomerId,int BranchId,int SellerId,DateTime Date,PaymentMethod PaymentMethod,PaymentStatus PaymentStatus,string? Notes,IReadOnlyCollection<SaleLineRequest> Items);
