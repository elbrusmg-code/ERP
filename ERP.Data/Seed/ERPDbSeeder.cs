using ERP.Core.Entities.Catalog;
using ERP.Core.Entities.CRM;
using ERP.Core.Entities.Finance;
using ERP.Core.Entities.HR;
using ERP.Core.Entities.Inventory;
using ERP.Core.Entities.Organization;
using ERP.Core.Entities.POS;
using ERP.Core.Entities.Procurement;
using ERP.Core.Enums;
using ERP.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ERP.Data.Seed;

public static class ERPDbSeeder
{
    public static async Task SeedAsync(ERPDbContext context)
    {
        await SeedOrganizationAsync(context);
        await SeedHrAsync(context);
        await SeedCatalogAsync(context);
        await SeedWarehousesAndStockAsync(context);
        await SeedCashRegistersAsync(context);
        await SeedFinancialAccountsAsync(context);
        await SeedSuppliersAsync(context);
        await SeedCrmAsync(context);
    }

    private static async Task SeedOrganizationAsync(ERPDbContext context)
    {
        if (!await context.Companies.AnyAsync(x => x.TaxNumber == SeedDataConstants.CompanyTaxNumber))
        {
            context.Companies.Add(new Company
            {
                Name = SeedDataConstants.CompanyName,
                LegalName = "SmartMarket Məhdud Məsuliyyətli Cəmiyyəti",
                TaxNumber = SeedDataConstants.CompanyTaxNumber,
                Phone = "+994 12 000 00 00",
                Email = "info@smartmarket.local",
                Address = "Baku, Azerbaijan",
                IsActive = true
            });

            await context.SaveChangesAsync();
        }

        var company = await context.Companies.SingleAsync(
            x => x.TaxNumber == SeedDataConstants.CompanyTaxNumber);

        if (!await context.Branches.AnyAsync(x => x.CompanyId == company.Id))
        {
            context.Branches.AddRange(
                CreateBranch(company.Id, "28 May Branch", SeedDataConstants.Branch28MayCode, "28 May, Baku"),
                CreateBranch(company.Id, "Xetai Branch", SeedDataConstants.BranchXetaiCode, "Xetai, Baku"),
                CreateBranch(company.Id, "Narimanov Branch", SeedDataConstants.BranchNarimanovCode, "Narimanov, Baku"));

            await context.SaveChangesAsync();
        }
    }

    private static Branch CreateBranch(int companyId, string name, string code, string address)
    {
        return new Branch
        {
            CompanyId = companyId,
            Name = name,
            Code = code,
            Address = address,
            IsActive = true
        };
    }

    private static async Task SeedHrAsync(ERPDbContext context)
    {
        var branches = await GetSeedBranchesAsync(context);

        if (!await context.Departments.AnyAsync(x => branches.Select(b => b.Id).Contains(x.BranchId)))
        {
            foreach (var branch in branches)
            {
                context.Departments.AddRange(
                    CreateDepartment(branch.Id, "Administration", DepartmentType.Administration),
                    CreateDepartment(branch.Id, "Sales", DepartmentType.Sales),
                    CreateDepartment(branch.Id, "Warehouse", DepartmentType.Warehouse),
                    CreateDepartment(branch.Id, "Procurement", DepartmentType.Procurement),
                    CreateDepartment(branch.Id, "Finance", DepartmentType.Finance),
                    CreateDepartment(branch.Id, "HR", DepartmentType.HR),
                    CreateDepartment(branch.Id, "Security", DepartmentType.Security));
            }

            await context.SaveChangesAsync();
        }

        var branchIds = branches.Select(x => x.Id).ToArray();
        var departments = await context.Departments
            .Where(x => branchIds.Contains(x.BranchId))
            .ToListAsync();

        if (!await context.Positions.AnyAsync(x => departments.Select(d => d.Id).Contains(x.DepartmentId)))
        {
            foreach (var branch in branches)
            {
                var branchDepartments = departments.Where(x => x.BranchId == branch.Id).ToList();
                context.Positions.AddRange(
                    CreatePosition(branchDepartments, DepartmentType.Administration, "Branch Manager", 1500m),
                    CreatePosition(branchDepartments, DepartmentType.Sales, "Cashier", 600m),
                    CreatePosition(branchDepartments, DepartmentType.Sales, "Head Cashier", 800m),
                    CreatePosition(branchDepartments, DepartmentType.Warehouse, "Warehouse Manager", 900m),
                    CreatePosition(branchDepartments, DepartmentType.Procurement, "Procurement Manager", 1000m),
                    CreatePosition(branchDepartments, DepartmentType.Finance, "Accountant", 1200m),
                    CreatePosition(branchDepartments, DepartmentType.HR, "HR Manager", 1000m),
                    CreatePosition(branchDepartments, DepartmentType.Security, "Security Officer", 550m));
            }

            await context.SaveChangesAsync();
        }
    }

    private static Department CreateDepartment(int branchId, string name, DepartmentType type)
    {
        return new Department { BranchId = branchId, Name = name, Type = type, IsActive = true };
    }

    private static Position CreatePosition(
        IReadOnlyCollection<Department> departments,
        DepartmentType departmentType,
        string name,
        decimal baseSalary)
    {
        return new Position
        {
            DepartmentId = departments.Single(x => x.Type == departmentType).Id,
            Name = name,
            BaseSalary = baseSalary,
            IsActive = true
        };
    }

    private static async Task SeedCatalogAsync(ERPDbContext context)
    {
        if (!await context.UnitsOfMeasure.AnyAsync())
        {
            context.UnitsOfMeasure.AddRange(
                CreateUnit("Piece", "pcs", UnitType.Piece),
                CreateUnit("Kilogram", "kg", UnitType.Kilogram),
                CreateUnit("Liter", "L", UnitType.Liter),
                CreateUnit("Pack", "pack", UnitType.Pack),
                CreateUnit("Box", "box", UnitType.Box));
        }

        if (!await context.TaxRates.AnyAsync())
        {
            context.TaxRates.AddRange(
                new TaxRate { Name = SeedDataConstants.Vat18Name, Rate = 18m, IsActive = true },
                new TaxRate { Name = SeedDataConstants.Vat0Name, Rate = 0m, IsActive = true });
        }

        if (!await context.ProductCategories.AnyAsync())
        {
            context.ProductCategories.AddRange(
                CreateCategory("Food"),
                CreateCategory("Drinks"),
                CreateCategory("Dairy"),
                CreateCategory("Bakery"),
                CreateCategory("Fruits and Vegetables"),
                CreateCategory("Cleaning Products"),
                CreateCategory("Personal Care"));
        }

        if (!await context.ProductBrands.AnyAsync())
        {
            context.ProductBrands.AddRange(
                CreateBrand("Coca-Cola"),
                CreateBrand("Azersud"),
                CreateBrand("Bizim Tarla"),
                CreateBrand("Local Bakery"),
                CreateBrand("Fairy"),
                CreateBrand("Ariel"));
        }

        await context.SaveChangesAsync();

        if (!await context.Products.AnyAsync())
        {
            var piece = await context.UnitsOfMeasure.SingleAsync(x => x.ShortName == "pcs");
            var kilogram = await context.UnitsOfMeasure.SingleAsync(x => x.ShortName == "kg");
            var vat18 = await context.TaxRates.SingleAsync(x => x.Name == SeedDataConstants.Vat18Name);
            var categories = await context.ProductCategories.ToDictionaryAsync(x => x.Name);
            var brands = await context.ProductBrands.ToDictionaryAsync(x => x.Name);

            var products = new[]
            {
                CreateProduct("PRD-COLA-1L", "Coca-Cola 1L", categories["Drinks"], brands["Coca-Cola"], piece, vat18, 1.00m, 1.50m, ProductType.Standard, false, false, false),
                CreateProduct("PRD-MILK-1L", "Azersud Milk 1L", categories["Dairy"], brands["Azersud"], piece, vat18, 1.20m, 1.80m, ProductType.Standard, true, true, false),
                CreateProduct("PRD-BREAD-001", "Bread", categories["Bakery"], brands["Local Bakery"], piece, vat18, 0.35m, 0.65m, ProductType.Standard, true, true, false),
                CreateProduct("PRD-APPLE-KG", "Apple", categories["Fruits and Vegetables"], brands["Bizim Tarla"], kilogram, vat18, 1.00m, 2.20m, ProductType.Weighted, true, true, true),
                CreateProduct("PRD-FAIRY-500", "Fairy 500ml", categories["Cleaning Products"], brands["Fairy"], piece, vat18, 2.20m, 3.50m, ProductType.Standard, false, false, false)
            };

            context.Products.AddRange(products);
            context.ProductBarcodes.AddRange(
                CreateBarcode(products[0], "5449000000996"),
                CreateBarcode(products[1], "4760095000012"),
                CreateBarcode(products[2], "2000000000011"),
                CreateBarcode(products[3], "2900000000015"),
                CreateBarcode(products[4], "8001090000010"));

            await context.SaveChangesAsync();
        }
    }

    private static UnitOfMeasure CreateUnit(string name, string shortName, UnitType type)
    {
        return new UnitOfMeasure { Name = name, ShortName = shortName, Type = type, IsActive = true };
    }

    private static ProductCategory CreateCategory(string name)
    {
        return new ProductCategory { Name = name, IsActive = true };
    }

    private static ProductBrand CreateBrand(string name)
    {
        return new ProductBrand { Name = name, IsActive = true };
    }

    private static Product CreateProduct(
        string sku,
        string name,
        ProductCategory category,
        ProductBrand brand,
        UnitOfMeasure unit,
        TaxRate taxRate,
        decimal costPrice,
        decimal salePrice,
        ProductType type,
        bool isPerishable,
        bool trackBatch,
        bool allowDecimalQuantity)
    {
        return new Product
        {
            SKU = sku,
            Name = name,
            ProductCategoryId = category.Id,
            ProductBrandId = brand.Id,
            UnitOfMeasureId = unit.Id,
            TaxRateId = taxRate.Id,
            CostPrice = costPrice,
            SalePrice = salePrice,
            Type = type,
            Status = ProductStatus.Active,
            IsActive = true,
            IsPerishable = isPerishable,
            TrackBatch = trackBatch,
            AllowDecimalQuantity = allowDecimalQuantity
        };
    }

    private static ProductBarcode CreateBarcode(Product product, string barcode)
    {
        return new ProductBarcode
        {
            Product = product,
            Barcode = barcode,
            Type = BarcodeType.EAN13,
            IsPrimary = true,
            IsActive = true
        };
    }

    private static async Task SeedWarehousesAndStockAsync(ERPDbContext context)
    {
        var branches = await GetSeedBranchesAsync(context);
        var branchIds = branches.Select(x => x.Id).ToArray();

        if (!await context.Warehouses.AnyAsync(x => branchIds.Contains(x.BranchId)))
        {
            foreach (var branch in branches)
            {
                context.Warehouses.AddRange(
                    new Warehouse
                    {
                        BranchId = branch.Id,
                        Name = "Main Warehouse",
                        Code = $"WH-{branch.Code}-MAIN",
                        Type = WarehouseType.BranchStore,
                        IsActive = true
                    },
                    new Warehouse
                    {
                        BranchId = branch.Id,
                        Name = "Damaged Goods Warehouse",
                        Code = $"WH-{branch.Code}-DMG",
                        Type = WarehouseType.Damaged,
                        IsActive = true
                    });
            }

            await context.SaveChangesAsync();
        }

        if (!await context.StockItems.AnyAsync(x => branchIds.Contains(x.BranchId)))
        {
            var products = await context.Products.ToDictionaryAsync(x => x.SKU);
            var stockDefinitions = new[]
            {
                new StockDefinition("PRD-COLA-1L", 100m, 20m, 300m, 40m),
                new StockDefinition("PRD-MILK-1L", 80m, 15m, 200m, 30m),
                new StockDefinition("PRD-BREAD-001", 60m, 20m, 150m, 30m),
                new StockDefinition("PRD-APPLE-KG", 120m, 25m, 300m, 50m),
                new StockDefinition("PRD-FAIRY-500", 50m, 10m, 150m, 20m)
            };

            foreach (var branch in branches)
            {
                var warehouse = await context.Warehouses.SingleAsync(
                    x => x.BranchId == branch.Id && x.Type == WarehouseType.BranchStore);

                foreach (var definition in stockDefinitions)
                {
                    context.StockItems.Add(new StockItem
                    {
                        ProductId = products[definition.Sku].Id,
                        BranchId = branch.Id,
                        WarehouseId = warehouse.Id,
                        Quantity = definition.Quantity,
                        ReservedQuantity = 0m,
                        MinimumStockLevel = definition.Minimum,
                        MaximumStockLevel = definition.Maximum,
                        ReorderLevel = definition.Reorder,
                        LastStockUpdateAt = DateTime.UtcNow
                    });
                }
            }

            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedCashRegistersAsync(ERPDbContext context)
    {
        var branches = await GetSeedBranchesAsync(context);
        var branchIds = branches.Select(x => x.Id).ToArray();

        if (!await context.CashRegisters.AnyAsync(x => branchIds.Contains(x.BranchId)))
        {
            foreach (var branch in branches)
            {
                context.CashRegisters.AddRange(
                    CreateCashRegister(branch, 1),
                    CreateCashRegister(branch, 2));
            }

            await context.SaveChangesAsync();
        }
    }

    private static CashRegister CreateCashRegister(Branch branch, int number)
    {
        return new CashRegister
        {
            BranchId = branch.Id,
            Name = $"Cash Register {number}",
            Code = $"POS-{branch.Code}-{number:00}",
            Status = CashRegisterStatus.Active,
            IsActive = true
        };
    }

    private static async Task SeedFinancialAccountsAsync(ERPDbContext context)
    {
        if (!await context.FinancialAccounts.AnyAsync())
        {
            context.FinancialAccounts.AddRange(
                CreateAccount("1000", "Cash", FinancialAccountType.Cash),
                CreateAccount("1010", "Bank", FinancialAccountType.Bank),
                CreateAccount("2000", "Supplier Payables", FinancialAccountType.SupplierPayable),
                CreateAccount("4000", "POS Sales Revenue", FinancialAccountType.Revenue),
                CreateAccount("5000", "General Expenses", FinancialAccountType.Expense),
                CreateAccount("5010", "Salary Expenses", FinancialAccountType.Salary),
                CreateAccount("5020", "Rent Expenses", FinancialAccountType.Expense),
                CreateAccount("5030", "Utilities Expenses", FinancialAccountType.Expense));

            await context.SaveChangesAsync();
        }
    }

    private static FinancialAccount CreateAccount(string code, string name, FinancialAccountType type)
    {
        return new FinancialAccount
        {
            AccountCode = code,
            Name = name,
            Type = type,
            Status = FinancialAccountStatus.Active,
            IsSystemAccount = true,
            IsActive = true
        };
    }

    private static async Task SeedSuppliersAsync(ERPDbContext context)
    {
        if (!await context.Suppliers.AnyAsync())
        {
            var suppliers = new[]
            {
                CreateSupplier("Coca-Cola Distributor Azerbaijan", "coca-cola@suppliers.local"),
                CreateSupplier("Azersud Supplier", "azersud@suppliers.local"),
                CreateSupplier("Local Bakery Supplier", "bakery@suppliers.local"),
                CreateSupplier("Cleaning Products Supplier", "cleaning@suppliers.local")
            };

            context.Suppliers.AddRange(suppliers);

            foreach (var supplier in suppliers)
            {
                context.SupplierContacts.Add(new SupplierContact
                {
                    Supplier = supplier,
                    FullName = "Sales Representative",
                    Position = "Account Manager",
                    Email = supplier.Email,
                    IsPrimary = true,
                    IsActive = true
                });
            }

            await context.SaveChangesAsync();
        }
    }

    private static Supplier CreateSupplier(string companyName, string email)
    {
        return new Supplier
        {
            CompanyName = companyName,
            Email = email,
            Status = SupplierStatus.Active,
            IsActive = true
        };
    }

    private static async Task SeedCrmAsync(ERPDbContext context)
    {
        if (!await context.CustomerGroups.AnyAsync())
        {
            context.CustomerGroups.AddRange(
                new CustomerGroup { Name = "Regular Customers", IsActive = true },
                new CustomerGroup { Name = "VIP Customers", DefaultDiscountPercent = 5m, IsActive = true },
                new CustomerGroup { Name = "Corporate Customers", IsActive = true });

            await context.SaveChangesAsync();
        }

        if (!await context.Customers.AnyAsync())
        {
            var groups = await context.CustomerGroups.ToDictionaryAsync(x => x.Name);
            var regularCustomer = new Customer
            {
                CustomerCode = "CUS-DEMO-REG",
                FullName = "Regular Customer Demo",
                Type = CustomerType.Retail,
                Status = CustomerStatus.Active,
                CustomerGroupId = groups["Regular Customers"].Id,
                IsLoyaltyEnabled = true
            };
            var vipCustomer = new Customer
            {
                CustomerCode = "CUS-DEMO-VIP",
                FullName = "VIP Customer Demo",
                Type = CustomerType.VIP,
                Status = CustomerStatus.Active,
                CustomerGroupId = groups["VIP Customers"].Id,
                IsLoyaltyEnabled = true
            };

            context.Customers.AddRange(regularCustomer, vipCustomer);
            context.LoyaltyCards.AddRange(
                CreateLoyaltyCard(regularCustomer, "LOY-DEMO-REG"),
                CreateLoyaltyCard(vipCustomer, "LOY-DEMO-VIP"));

            await context.SaveChangesAsync();
        }
    }

    private static LoyaltyCard CreateLoyaltyCard(Customer customer, string cardNumber)
    {
        return new LoyaltyCard
        {
            Customer = customer,
            CardNumber = cardNumber,
            Status = LoyaltyCardStatus.Active,
            IssuedAt = DateTime.UtcNow,
            IsPrimary = true
        };
    }

    private static Task<List<Branch>> GetSeedBranchesAsync(ERPDbContext context)
    {
        var codes = new[]
        {
            SeedDataConstants.Branch28MayCode,
            SeedDataConstants.BranchXetaiCode,
            SeedDataConstants.BranchNarimanovCode
        };

        return context.Branches.Where(x => codes.Contains(x.Code)).ToListAsync();
    }

    private sealed record StockDefinition(
        string Sku,
        decimal Quantity,
        decimal Minimum,
        decimal Maximum,
        decimal Reorder);
}
