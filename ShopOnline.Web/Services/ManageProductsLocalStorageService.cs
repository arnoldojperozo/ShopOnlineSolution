using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Services;

public class ManageProductsLocalStorageService : IManageProductsLocalStorageService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IProductService _productService;
    private readonly string key = "ProductCollection";

    public ManageProductsLocalStorageService(ILocalStorageService localStorageService, IProductService productService)
    {
        _productService = productService;
        _localStorageService = localStorageService;
    }
    
    public async Task<IEnumerable<ProductDto>> GetCollection()
    {
        return await _localStorageService.GetItemAsync<IEnumerable<ProductDto>>(key) ?? await AddCollection();
    }

    public async Task RemoveCollection()
    {
        await _localStorageService.RemoveItemAsync(key);
    }

    private async Task<IEnumerable<ProductDto>> AddCollection()
    {
        var productCollection = await this._productService.GetItems();

        if (productCollection != null)
        {
            await _localStorageService.SetItemAsync(key, productCollection);
        }

        return productCollection;
    }
}