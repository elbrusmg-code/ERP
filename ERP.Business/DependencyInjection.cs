using FluentValidation;
using ERP.Business.Features.Catalog.Interfaces;
using ERP.Business.Features.Catalog.Services;
using ERP.Business.Features.CRM.Interfaces;
using ERP.Business.Features.CRM.Services;
using ERP.Business.Features.Finance.Interfaces;
using ERP.Business.Features.Finance.Services;
using ERP.Business.Features.Inventory.Interfaces;
using ERP.Business.Features.Inventory.Services;
using ERP.Business.Features.Organization.Interfaces;
using ERP.Business.Features.Organization.Services;
using ERP.Business.Features.POS.Interfaces;
using ERP.Business.Features.POS.Services;
using ERP.Business.Features.Procurement.Interfaces;
using ERP.Business.Features.Procurement.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddAutoMapper(
            configuration => { },
            typeof(DependencyInjection).Assembly);
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<ICrmService, CrmService>();
        services.AddScoped<IFinanceService, FinanceService>();
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<IPosService, PosService>();
        services.AddScoped<IProcurementService, ProcurementService>();
        return services;
    }
}
