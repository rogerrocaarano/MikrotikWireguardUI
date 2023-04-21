using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MikrotikWireguardUI.Models;

namespace MikrotikWireguardUI.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<MikrotikWireguardUI.Models.Server> Server { get; set; } = default!;
    public DbSet<MikrotikWireguardUI.Models.Iface> Iface { get; set; } = default!;
    public DbSet<MikrotikWireguardUI.Models.Peer> Peer { get; set; } = default!;
}