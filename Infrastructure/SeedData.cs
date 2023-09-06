using server.Data;
using server.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;


namespace server.Infrastructure
{
	public class SeedData
	{
		private static string HashPassword(string password)
		{
			byte[] salt;
			new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

			var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
			byte[] hash = pbkdf2.GetBytes(20);

			byte[] hashBytes = new byte[36];
			Array.Copy(salt, 0, hashBytes, 0, 16);
			Array.Copy(hash, 0, hashBytes, 16, 20);

			return Convert.ToBase64String(hashBytes);
		}
		public static void SeedDatabase(serverContext context)
		{
			context.Database.Migrate();
			if (!context.User.Any())
			{
				context.User.AddRange(
					new User
					{
						Username = "customer",
						Password = HashPassword("Qwer.123"),
						Email = "customer@gmail.com",
						FirstName = "Cus",
						LastName = "A",
						Phone = "01515151",
						Role = 1,
						Status = true,
						Verified = true
					},
					new User
					{
						Username = "staff",
						Password = HashPassword("Qwer.123"),
						Email = "staff@gmail.com",
						FirstName = "Sta",
						LastName = "B",
						Phone = "01515151",
						Role = 2,
						Status = true,
						Verified = true
					},
					new User
					{
						Username = "manager",
						Password = HashPassword("Qwer.123"),
						Email = "manager@gmail.com",
						FirstName = "Mana",
						LastName = "C",
						Phone = "01515151",
						Role = 3,
						Status = true,
						Verified = true
					}
				);
				context.SaveChanges();
			}

			if (!context.Product.Any())
			{
				Brand A = new Brand { Title = "Asus" };
				Brand B = new Brand { Title = "Acer" };
				Brand C = new Brand { Title = "MSI" };
				Brand D = new Brand { Title = "Lenovo" };
				Brand E = new Brand { Title = "HP" };
				Brand F = new Brand { Title = "Dell" };
				Brand G = new Brand { Title = "Apple" };

				context.Product.AddRange(
						new Product
						{
							Brand = A,
							Name = "Asus vivobook 14X",
							Price = 1000,
							Stock = 4,
							Image = "/images/Product/asus1.png",
							Color = "Red",
							Gift = "Mouse",
							Model = "Flag ship",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},

						new Product
						{
							Brand = A,
							Name = "Asus vivobook 15X",
							Price = 900,
							Stock = 10,
							Image = "/images/Product/asus2.png",
							Color = "White",
							Gift = "Keyboard",
							Model = "Flag ship",
							Warranty = "1 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = A,
							Name = "Asus zenbook S13",
							Price = 1100,
							Stock = 4,
							Image = "/images/Product/asus3.png",
							Color = "Black",
							Gift = "Mouse",
							Model = "Flag ship",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = A,
							Name = "Asus ROG Strix G16",
							Price = 1500,
							Stock = 4,
							Image = "/images/Product/asus4.png",
							Color = "Black",
							Gift = "Mouse",
							Model = "Gaming",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = A,
							Name = "Asus ROG zephyrus duo",
							Price = 2000,
							Stock = 3,
							Image = "/images/Product/asus5.png",
							Color = "White",
							Gift = "Headphones",
							Model = "Gaming",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = B,
							Name = "Acer Nitro 17",
							Price = 1000,
							Stock = 5,
							Image = "/images/Product/acer1.png",
							Color = "Black",
							Gift = "Headphones",
							Model = "Gaming",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = B,
							Name = "Acer Helios Neo",
							Price = 1300,
							Stock = 5,
							Image = "/images/Product/acer2.png",
							Color = "White",
							Gift = "Mouse",
							Model = "Gaming",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = B,
							Name = "Acer Nitro 5 Eagle",
							Price = 1100,
							Stock = 2,
							Image = "/images/Product/acer3.png",
							Color = "Black",
							Gift = "Headphones",
							Model = "Gaming",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = B,
							Name = "Acer Nitro 5",
							Price = 1000,
							Stock = 1,
							Image = "/images/Product/acer4.png",
							Color = "White",
							Gift = "Mouse",
							Model = "Gaming",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = B,
							Name = "Acer Aspire 7",
							Price = 800,
							Stock = 6,
							Image = "/images/Product/acer5.png",
							Color = "Black",
							Gift = "Keyboard",
							Model = "Flag ship",
							Warranty = "1 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = C,
							Name = "MSI Modern 15",
							Price = 1000,
							Stock = 2,
							Image = "/images/Product/msi1.png",
							Color = "White",
							Gift = "Mouse",
							Model = "Flag ship",
							Warranty = "1 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = C,
							Name = "MSI Creator Z16",
							Price = 1300,
							Stock = 2,
							Image = "/images/Product/msi2.png",
							Color = "White",
							Gift = "Headphones",
							Model = "Flag ship",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = C,
							Name = "MSI Creator Z17",
							Price = 1400,
							Stock = 4,
							Image = "/images/Product/msi3.png",
							Color = "White",
							Gift = "Mouse",
							Model = "Flag ship",
							Warranty = "1 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = C,
							Name = "MSI Katana 15",
							Price = 1000,
							Stock = 11,
							Image = "/images/Product/msi4.png",
							Color = "Black",
							Gift = "Headphones",
							Model = "Gaming",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = C,
							Name = "MSI Katana GF66",
							Price = 1100,
							Stock = 5,
							Image = "/images/Product/msi5.png",
							Color = "White",
							Gift = "Mouse",
							Model = "Gaming",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = D,
							Name = "Lenovo Slim 5",
							Price = 1000,
							Stock = 5,
							Image = "/images/Product/lenovo1.png",
							Color = "White",
							Gift = "Headphones",
							Model = "Flag ship",
							Warranty = "1 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = D,
							Name = "Lenovo Thinkpad T14",
							Price = 900,
							Stock = 10,
							Image = "/images/Product/lenovo2.png",
							Color = "Black",
							Gift = "Mouse",
							Model = "Flag ship",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = D,
							Name = "Lenovo Ideapad 1",
							Price = 800,
							Stock = 9,
							Image = "/images/Product/lenovo3.png",
							Color = "Silver",
							Gift = "Headphones",
							Model = "Flag ship",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = D,
							Name = "Lenovo Legion 5",
							Price = 1400,
							Stock = 7,
							Image = "/images/Product/lenovo4.png",
							Color = "White",
							Gift = "Keyboard",
							Model = "Gaming",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = D,
							Name = "Lenovo Legion 5 Pro",
							Price = 1600,
							Stock = 10,
							Image = "/images/Product/lenovo5.png",
							Color = "Silver",
							Gift = "Headphones",
							Model = "Gaming",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = E,
							Name = "HP Envy 13",
							Price = 800,
							Stock = 3,
							Image = "/images/Product/hp1.png",
							Color = "Silver",
							Gift = "Mouse",
							Model = "Flag ship",
							Warranty = "1 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = E,
							Name = "HP ProBook",
							Price = 900,
							Stock = 7,
							Image = "/images/Product/hp2.png",
							Color = "Black",
							Gift = "Headphones",
							Model = "Flag ship",
							Warranty = "3 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = E,
							Name = "HP Spectre",
							Price = 1000,
							Stock = 15,
							Image = "/images/Product/hp3.png",
							Color = "White",
							Gift = "Headphones",
							Model = "Flag ship",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = E,
							Name = "HP Victus 15",
							Price = 1200,
							Stock = 7,
							Image = "/images/Product/hp4.png",
							Color = "Black",
							Gift = "Headphones",
							Model = "Gaming",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = E,
							Name = "HP Omen 16",
							Price = 1100,
							Stock = 9,
							Image = "/images/Product/hp5.png",
							Color = "Black",
							Gift = "Keyboard",
							Model = "Gaming",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = F,
							Name = "Dell XPS 13",
							Price = 750,
							Stock = 16,
							Image = "/images/Product/dell1.png",
							Color = "White",
							Gift = "Mouse",
							Model = "Flag ship",
							Warranty = "1 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = F,
							Name = "Dell Inspiron 14",
							Price = 800,
							Stock = 6,
							Image = "/images/Product/dell2.png",
							Color = "Gold",
							Gift = "Headphones",
							Model = "Flag ship",
							Warranty = "1 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = F,
							Name = "Dell G15",
							Price = 1200,
							Stock = 10,
							Image = "/images/Product/dell3.png",
							Color = "Black",
							Gift = "Headphones",
							Model = "Gaming",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = F,
							Name = "Dell Alienware M15",
							Price = 2000,
							Stock = 20,
							Image = "/images/Product/dell4.png",
							Color = "Black",
							Gift = "Headphones, Mouse",
							Model = "Gaming",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = F,
							Name = "Dell Alienware M17",
							Price = 2100,
							Stock = 15,
							Image = "/images/Product/dell5.png",
							Color = "Black",
							Gift = "Headphones, Mouse",
							Model = "Gaming",
							Warranty = "2 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = G,
							Name = "Apple MacBook air M2 Midnight",
							Price = 1200,
							Stock = 9,
							Image = "/images/Product/mac1.png",
							Color = "Silver",
							Gift = "Bag",
							Model = "Flag ship",
							Warranty = "1 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = G,
							Name = "Apple MacBook air 2020",
							Price = 1100,
							Stock = 7,
							Image = "/images/Product/mac2.png",
							Color = "Silver",
							Gift = "Bag",
							Model = "Flag ship",
							Warranty = "1 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = G,
							Name = "Apple MacBook air M2 Starlight",
							Price = 1200,
							Stock = 7,
							Image = "/images/Product/mac3.png",
							Color = "Silver",
							Gift = "Bag",
							Model = "Flag ship",
							Warranty = "1 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = G,
							Name = "Apple MacBook air M2 SpaceGray",
							Price = 1200,
							Stock = 7,
							Image = "/images/Product/mac4.png",
							Color = "Silver",
							Gift = "Bag",
							Model = "Flag ship",
							Warranty = "1 years",
							Description = "Intel cpu, Nvidia graphic card"
						},
						new Product
						{
							Brand = G,
							Name = "Apple MacBook M2 Pro",
							Price = 2000,
							Stock = 10,
							Image = "/images/Product/mac5.png",
							Color = "Gold",
							Gift = "Bag",
							Model = "Flag ship",
							Warranty = "1 years",
							Description = "Intel cpu, Nvidia graphic card"
						}
				);
				context.SaveChanges();
			}
		}
	}
}