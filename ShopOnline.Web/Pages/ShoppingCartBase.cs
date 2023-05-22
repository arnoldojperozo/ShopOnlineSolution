using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages;

public class ShoppingCartBase : ComponentBase
{
    [Inject]
    public IJSRuntime Js { get; set; }
    
    [Inject]
    public IShoppingCartService ShoppingCartService { get; set; }
    
    public List<CartItemDto> ShoppingCartItems { get; set; }
    
    [Inject]
    public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }
    
    public string ErrorMessage { get; set; }

    public string TotalPrice { get; set; }

    public int TotalQuantity { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
            CartChanged();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    protected async Task DeleteCartItem_Click(int id)
    {
        var cartItemDto = await ShoppingCartService.DeleteItem(id);
        
        await RemoveCartItem(cartItemDto.Id);

        CartChanged();
    }

    protected async Task UpdateQtyCartItem_Click(int id, int qty)
    {
        try
        {
            if (qty > 0)
            {
                var updateItemDto = new CartItemQtyUpdateDto()
                {
                    CartItemId = id,
                    Qty = qty
                };

                var returnedUpdateItemDto = await ShoppingCartService.UpdateQty(updateItemDto);

                await UpdateItemTotalPrice(returnedUpdateItemDto);
                CartChanged();
                await MakeUpdateQtyButtonVisible(id, false);
            }
            else
            {
                var item = ShoppingCartItems.FirstOrDefault(i => i.Id == id);

                if (item !=null)
                {
                    item.Qty = 1;
                    item.TotalPrice=item.Price;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    private async Task MakeUpdateQtyButtonVisible(int id, bool visible)
    {
        await Js.InvokeVoidAsync("MakeQtyButtonVisible", id, visible);
    }

    private CartItemDto GetCartItem(int id)
    {
        return ShoppingCartItems.FirstOrDefault(i => i.Id == id);
    }

    protected async Task UpdateQty_Input(int id)
    {
        await MakeUpdateQtyButtonVisible(id, true);
    }

    private async Task RemoveCartItem(int id)
    {
        var cartItemDto = GetCartItem(id);

        ShoppingCartItems.Remove(cartItemDto);

        await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
    }

    private async Task UpdateItemTotalPrice(CartItemDto cartItemDto)
    {
        var item = GetCartItem(cartItemDto.Id);

        if (item != null)
        {
            item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
        }

        await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
    }

    private void CalculateCartSummaryTotals()
    {
        SetTotalQty();
        SetTotalPrice();
    }

    private void SetTotalPrice()
    {
        TotalPrice = ShoppingCartItems.Sum(p => p.TotalPrice).ToString("C");
    }

    private void SetTotalQty()
    {
        TotalQuantity = ShoppingCartItems.Sum(p => p.Qty);
    }

    private void CartChanged()
    {
        CalculateCartSummaryTotals();
        ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
    }
}