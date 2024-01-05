using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models;

namespace DataAccess.Data
{
    public class BookWebStoreDbContext : IdentityDbContext
    {
        public BookWebStoreDbContext(DbContextOptions<BookWebStoreDbContext> options) : base(options) { }
        public DbSet<Book> Book { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Publisher> Publisher { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Customer> Customer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);        
            modelBuilder.Entity<Book>()
                        .HasOne(i => i.Author)
                        .WithMany(x => x.Books)
                        .HasForeignKey(y => y.authorId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Book>()
                        .HasOne(i => i.Publisher)
                        .WithMany(x => x.Books)
                        .HasForeignKey(y => y.publisherID)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderDetail>()
                        .HasOne(i => i.Book)
                        .WithMany(x => x.OrderDetails)
                        .HasForeignKey(y => y.BookId)
                        .IsRequired();
            modelBuilder.Entity<OrderDetail>()
                        .HasOne(i => i.Order)
                        .WithMany(x => x.OrderDetails)
                        .HasForeignKey(y => y.OrderId)
                        .IsRequired();

            modelBuilder.Entity<CartItem>()
                        .HasOne(i => i.Book)
                        .WithMany(x => x.CartItems)
                        .HasForeignKey(y => y.BookId)
                        .IsRequired();
            modelBuilder.Entity<CartItem>()
                        .HasOne(i => i.Cart)
                        .WithMany(x => x.CartItems)
                        .HasForeignKey(y => y.CartId)
                        .IsRequired();

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasMany(d => d.Books).WithMany(p => p.Categories)
                    .UsingEntity<Dictionary<string, object>>(
                        "BookCategory",
                        r => r.HasOne<Book>().WithMany()
                            .HasForeignKey("BookId")
                            .HasConstraintName("FK_BOOKCATE_BOOKCATEG_BOOKS"),
                        l => l.HasOne<Category>().WithMany()
                            .HasForeignKey("CategoryId")
                            .HasConstraintName("FK_BOOKCATE_BOOKCATEG_CATEGORI"),
                        j =>
                        {
                            j.HasKey("CategoryId", "BookId").HasName("PK_BOOKCATEGORY");
                            j.ToTable("BookCategory");
                            j.HasIndex(new[] { "BookId" }, "BOOKCATEGORY2_FK");
                            j.HasIndex(new[] { "CategoryId" }, "BOOKCATEGORY_FK");
                        });
            });
        }


    }
}

