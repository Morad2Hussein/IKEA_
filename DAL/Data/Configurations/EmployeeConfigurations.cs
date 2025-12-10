
using Demo.BLL.DTOS.EmployeeDTOS.Common;
namespace Demo.DAL.Data.Configurations
{
    internal class EmployeeConfigurations : BaseEntityConfigurations<Employee>
    {
        public override void  Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(p => p.Name).HasColumnType("varchar(50)");
            builder.Property(p => p.Address).HasColumnType("varchar(50)");
            builder.Property(p => p.Salary).HasColumnType("decimal(18,2)");
            // Enum to string conversion
            builder.Property(p => p.Gender)
                            .HasConversion(
                             (EGender) => EGender.ToString(),
                             (gender) => (Gender)Enum.Parse(typeof(Gender), gender)
                            );

            builder.Property(p => p.EmployeeType).
                    HasConversion(
                                 (EType) => EType.ToString(),
                                 (etype) => (EmployeeType)Enum.Parse(typeof(EmployeeType), etype)
                                  );


            base.Configure(builder);


        }
    }
}
