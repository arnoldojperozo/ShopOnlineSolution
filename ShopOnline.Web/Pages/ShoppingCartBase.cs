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
    
    public string ErrorMessage { get; set; }

    public string TotalPrice { get; set; }

    public int TotalQty { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
            CalculateCartSummaryTotals();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    protected async Task DeleteCartItem_Click(int id)
    {
        var cartItemDto = await ShoppingCartService.DeleteItem(id);
        
        RemoveCartItem(cartItemDto.Id);

        CalculateCartSummaryTotals();
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

                UpdateItemTotalPrice(returnedUpdateItemDto);
                CalculateCartSummaryTotals();
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

    private void RemoveCartItem(int id)
    {
        var cartItemDto = GetCartItem(id);

        ShoppingCartItems.Remove(cartItemDto);
    }

    private void UpdateItemTotalPrice(CartItemDto cartItemDto)
    {
        var item = GetCartItem(cartItemDto.Id);

        if (item != null)
        {
            item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
        }
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
        TotalQty = ShoppingCartItems.Sum(p => p.Qty);
    }
}