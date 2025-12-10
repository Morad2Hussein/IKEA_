
namespace Demo.DAL.Data.Configurations
{
    public class BaseEntityConfigurations<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity, new()
    {
        public virtual  void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(p => p.CreatedOn).
                           HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.ModifiedOn)
                            .ValueGeneratedOnAddOrUpdate()
                            .HasDefaultValueSql("GETDATE()");


        }

    }
}
