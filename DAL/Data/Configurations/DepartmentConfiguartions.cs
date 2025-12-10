
namespace Demo.DAL.Data.Configurations
{
    internal class DepartmentConfiguartions : BaseEntityConfigurations<Department>
    {
        public override  void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(p => p.Id).
                            UseIdentityColumn(10, 10);
            builder.Property(p => p.Name).
                            HasColumnType("varchar(20)");
            builder.Property(p => p.Code).
                            HasColumnType("varchar(20)");
            builder.HasMany(p => p.Employees)
                    .WithOne(p => p.Department)
                    .HasForeignKey(p => p.DepartmentId)
                     .OnDelete(DeleteBehavior.SetNull);
            base.Configure(builder);


        }
    }
}
