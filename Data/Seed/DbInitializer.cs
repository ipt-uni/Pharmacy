using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pharmacy.Data.Models;

namespace pharmacy.Data;

public static class DbInitializer
{
    public static async Task Seed(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        if (await context.Companies.AnyAsync())
            return;

        foreach (var role in new[] { "Customer", "Staff" })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var companies = new[]
        {
            new Company { Name = "Sanofi India Ltd" },
            new Company { Name = "Alembic Pharmaceuticals Ltd" },
            new Company { Name = "Abbott" },
            new Company { Name = "AstraZeneca" },
            new Company { Name = "Zydus Cadila" },
            new Company { Name = "Micro Labs Ltd" },
            new Company { Name = "Medley Pharmaceuticals" },
            new Company { Name = "Glenmark Pharmaceuticals Ltd" },
            new Company { Name = "Lupin Ltd" },
            new Company { Name = "Leeford Healthcare Ltd" },
            new Company { Name = "Pfizer Ltd" },
            new Company { Name = "Novartis India Ltd" },
            new Company { Name = "USV Ltd" },
            new Company { Name = "Intas Pharmaceuticals Ltd" },
            new Company { Name = "Franco-Indian Pharmaceuticals Pvt Ltd" },
            new Company { Name = "Win-Medicare Pvt Ltd" },
            new Company { Name = "Opticarma India SMC Pvt Ltd" },
            new Company { Name = "Sun Pharmaceutical Industries Ltd" },
            new Company { Name = "Jagsonpal Pharmaceuticals Ltd" },
            new Company { Name = "Ipca Laboratories Ltd" },
            new Company { Name = "MSD Pharmaceuticals Pvt Ltd" },
            new Company { Name = "Boehringer Ingelheim" },
            new Company { Name = "Mankind Pharma Ltd" },
            new Company { Name = "Med Manor Organics Pvt Ltd" },
            new Company { Name = "Cipla Ltd" },
            new Company { Name = "J B Chemicals and Pharmaceuticals Ltd" },
            new Company { Name = "Torrent Pharmaceuticals Ltd" },
            new Company { Name = "Dr Reddy's Laboratories Ltd" },
        };
        context.Companies.AddRange(companies);
        await context.SaveChangesAsync();

        var suppliers = new[]
        {
            new Supplier { Name = "MedSupply Co" },
            new Supplier { Name = "PharmaDist" },
            new Supplier { Name = "HealthLogistics" },
            new Supplier { Name = "MedWorld" },
            new Supplier { Name = "DrugChain" },
            new Supplier { Name = "PharmaLink" },
            new Supplier { Name = "BioDist" },
            new Supplier { Name = "MedExpress" },
            new Supplier { Name = "GlobalPharma" },
            new Supplier { Name = "CareSupply" },
        };
        context.Suppliers.AddRange(suppliers);
        await context.SaveChangesAsync();

        var c = companies;
        var s = suppliers;

        var rawMeds = new[]
        {
            (
                name: "Avil 25 Tablet",
                price: 40m,
                company: c[0],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/mmsye6bf97tkcocat24j.jpg"
            ),
            (
                name: "Azithral 200 Liquid",
                price: 32m,
                company: c[1],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/mbdxk2kboh3llijyaao2.jpg"
            ),
            (
                name: "Avil Injection",
                price: 42m,
                company: c[0],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/n2al1qoxuxtzshwgwbt0.jpg"
            ),
            (
                name: "Brufen 400 Tablet",
                price: 33m,
                company: c[2],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/yssbye7myd6dgld7wn2n.jpg"
            ),
            (
                name: "Brilinta 90mg Tablet",
                price: 49m,
                company: c[3],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/17e4a7ea8d2841e58d718ec9b7cd85fe.jpg"
            ),
            (
                name: "Bilypsa Tablet",
                price: 50m,
                company: c[4],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/poriofjctoxokvugfyhu.jpg"
            ),
            (
                name: "Diapride M2 Tablet PR",
                price: 43m,
                company: c[5],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/1f0accbfd0624978b96f3e5608915e41.jpg"
            ),
            (
                name: "Dompan SR Tablet",
                price: 38m,
                company: c[6],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/rhtgxirzanuvei2ghntd.jpg"
            ),
            (
                name: "Diziron Tablet",
                price: 21m,
                company: c[7],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/zjyvrginvhgknv96fmlm.jpg"
            ),
            (
                name: "Danclear Shampoo",
                price: 38m,
                company: c[4],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/xr9mupzqfrwy3unekyhw.jpg"
            ),
            (
                name: "Doxiflo-M Tablet SR",
                price: 48m,
                company: c[8],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/fa068b4c7f0c445a87a60a4b4b314f6a.jpg"
            ),
            (
                name: "Debistal-GM 2 Tablet ER",
                price: 38m,
                company: c[9],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/26e63da5094049b3b597905f1cb4806b.jpg"
            ),
            (
                name: "Eliquis 5mg Tablet",
                price: 42m,
                company: c[10],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/qouhjkuftzyxgcxzeens.jpg"
            ),
            (
                name: "Exelon Patch 15",
                price: 22m,
                company: c[11],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/2d4a1903fe0d4d48bb733abe47fe009b.jpg"
            ),
            (
                name: "Ecosprin 75 Tablet",
                price: 32m,
                company: c[12],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/reowbvajejs6awykdplk.jpg"
            ),
            (
                name: "Gabapin NT 100 Tablet",
                price: 15m,
                company: c[13],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/krpmtanzzd90igxknddd.png"
            ),
            (
                name: "Glycomet-GP 2 Tablet PR",
                price: 33m,
                company: c[12],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/raiknno8yegabnob3rve.jpg"
            ),
            (
                name: "Grilinctus-BM Syrup",
                price: 25m,
                company: c[14],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/72bc254105c64b68919797b5c4d41f45.jpg"
            ),
            (
                name: "Gabapin 100 Tablet",
                price: 31m,
                company: c[13],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/ncv25wqijuof2b5dcul5.jpg"
            ),
            (
                name: "Glyciphage SR 500mg Tablet",
                price: 26m,
                company: c[14],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/em6mrhuqaiof22rrhogy.jpg"
            ),
            (
                name: "Hepamerz Tablet",
                price: 31m,
                company: c[15],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/xy8atqccjo8zu6ty5zl4.jpg"
            ),
            (
                name: "Hifenac-MR Tablet",
                price: 34m,
                company: c[13],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/ubsehzogi90hcjivru5z.jpg"
            ),
            (
                name: "Hepamerz Infusion",
                price: 36m,
                company: c[15],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/mbmv87vff4ifhhcl8ltz.jpg"
            ),
            (
                name: "Hyocimax-S Tablet",
                price: 29m,
                company: c[4],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/dnz8dvgbofx9i2bd01dg.jpg"
            ),
            (
                name: "Heptral 400mg Tablet",
                price: 40m,
                company: c[2],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/02c513f74e6e4485a593d827d4a63f28.jpg"
            ),
            (
                name: "Inderal 40 Tablet",
                price: 30m,
                company: c[2],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/sha74npjyhaxwlifqaer.jpg"
            ),
            (
                name: "ITR Plus Eye Drop",
                price: 33m,
                company: c[16],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/b7d0354ed34740b3a4a98503dbe6eb56.jpg"
            ),
            (
                name: "Idrofos 150 Tablet",
                price: 26m,
                company: c[17],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/zjvio3b44jp8ukjaqncg.jpg"
            ),
            (
                name: "Indocap Capsule",
                price: 26m,
                company: c[18],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/hyigy4oteyt4lebzbsl2.jpg"
            ),
            (
                name: "Istamet 50mg/1000mg Tablet",
                price: 38m,
                company: c[17],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/sodfg3up8pxdxj5f2n9i.jpg"
            ),
            (
                name: "Januvia 100mg Tablet",
                price: 45m,
                company: c[20],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/tylw7gc7xhvlqetdlvji.jpg"
            ),
            (
                name: "Janumet 50mg/1000mg Tablet",
                price: 50m,
                company: c[20],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/5de83662cc664ee381fc9b5dc8b02aa3.jpg"
            ),
            (
                name: "Jardiance 10mg Tablet",
                price: 47m,
                company: c[21],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/4f108e3cd95f4a94bc7a1ae8fb6c6f63.jpg"
            ),
            (
                name: "Ketoscalp Shampoo",
                price: 38m,
                company: c[9],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/rd7ixqu9ed5qtggnflt9.jpg"
            ),
            (
                name: "Ketostar Cream",
                price: 26m,
                company: c[22],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/f7e0dooonwlonymdlhhg.jpg"
            ),
            (
                name: "Kidpred Syrup",
                price: 47m,
                company: c[2],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/jdfdhhbtdyytc6dr3j6y.jpg"
            ),
            (
                name: "Ketoflam-P Tablet",
                price: 36m,
                company: c[8],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/tkigms1yllalumrie5tb.jpg"
            ),
            (
                name: "Keto Cream",
                price: 25m,
                company: c[23],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/lek6dou1miynfmcqx1ep.jpg"
            ),
            (
                name: "Lubrex Eye Drop",
                price: 41m,
                company: c[5],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/cropped/c436uvbbglvz7v740yy0.jpg"
            ),
            (
                name: "Levolin 1mg Syrup",
                price: 24m,
                company: c[24],
                img: "https://onemg.gumlet.io/l_watermark_346,w_480,h_480/a_ignore,w_480,h_480,c_fit,q_auto,f_auto/bewn13bcxkyunlpifst7.jpg"
            ),
        };

        var medicines = rawMeds
            .Select(m => new Medicine
            {
                Name = m.name,
                RetailPrice = m.price,
                CompanyId = m.company.Id,
                imageSrc = m.img,
                Suppliers = new List<Supplier>
                {
                    s[Random.Shared.Next(s.Length)],
                    s[Random.Shared.Next(s.Length)],
                },
            })
            .ToList();

        context.Medicines.AddRange(medicines);
        await context.SaveChangesAsync();

        var customerData = new (
            string email,
            string firstName,
            string lastName,
            int age,
            string address,
            string phone
        )[]
        {
            ("customer1@test.com", "Alice", "Johnson", 30, "123 Main St, New York", "555-0101"),
            ("customer2@test.com", "Bob", "Smith", 25, "456 Oak Ave, Los Angeles", "555-0102"),
            ("customer3@test.com", "Carol", "Williams", 28, "789 Pine Rd, Chicago", "555-0103"),
            ("customer4@test.com", "Dave", "Brown", 35, "321 Elm St, Houston", "555-0104"),
            ("customer5@test.com", "Eve", "Davis", 22, "654 Maple Dr, Phoenix", "555-0105"),
            ("customer6@test.com", "Frank", "Miller", 40, "987 Cedar Ln, Philadelphia", "555-0106"),
            ("customer7@test.com", "Grace", "Wilson", 33, "147 Birch St, San Antonio", "555-0107"),
            ("customer8@test.com", "Henry", "Moore", 45, "258 Walnut Ave, San Diego", "555-0108"),
            ("customer9@test.com", "Iris", "Taylor", 27, "369 Spruce Ct, Dallas", "555-0109"),
            ("customer10@test.com", "Jack", "Anderson", 38, "741 Ash Blvd, San Jose", "555-0110"),
        };

        var staffData = new (string email, string firstName, string lastName, int age)[]
        {
            ("staff1@test.com", "Charlie", "Brown", 35),
            ("staff2@test.com", "Diana", "Prince", 28),
            ("staff3@test.com", "Ethan", "Clark", 42),
            ("staff4@test.com", "Fiona", "Lewis", 31),
            ("staff5@test.com", "George", "Hall", 26),
            ("staff6@test.com", "Hannah", "Allen", 39),
            ("staff7@test.com", "Ian", "Young", 44),
            ("staff8@test.com", "Julia", "King", 29),
            ("staff9@test.com", "Kevin", "Wright", 36),
            ("staff10@test.com", "Laura", "Scott", 33),
        };

        foreach (var c2 in customerData)
        {
            var user = new IdentityUser
            {
                UserName = c2.email,
                Email = c2.email,
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(user, "Password1");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Customer");
                context.Customers.Add(
                    new Customer
                    {
                        UserId = user.Id,
                        FirstName = c2.firstName,
                        LastName = c2.lastName,
                        Age = c2.age,
                        Address = c2.address,
                        PhoneNumber = c2.phone,
                    }
                );
            }
        }

        foreach (var s2 in staffData)
        {
            var user = new IdentityUser
            {
                UserName = s2.email,
                Email = s2.email,
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(user, "Password1");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Staff");
                context.NonCustomers.Add(
                    new NonCustomer
                    {
                        UserId = user.Id,
                        FirstName = s2.firstName,
                        LastName = s2.lastName,
                        Age = s2.age,
                    }
                );
            }
        }

        await context.SaveChangesAsync();
    }
}
