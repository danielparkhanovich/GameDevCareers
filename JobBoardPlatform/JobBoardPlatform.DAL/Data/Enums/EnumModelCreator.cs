using JobBoardPlatform.DAL.Models.Contracts;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data.Enums
{
    public class EnumModelCreator : IEnumModelCreator
    {
        private readonly ModelBuilder modelBuilder;


        public EnumModelCreator(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public void SetDataForEntity<TEntity, TEnum>()
            where TEntity : class, IEnumEntity, new()
            where TEnum : struct
        {
            var enumValues = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .ToArray();

            var enumData = new TEntity[enumValues.Length];

            for (int i = 0; i < enumValues.Length; i++)
            {
                string enumName = enumValues[i].ToString();

                enumData[i] = new TEntity()
                {
                    Id = i + 1,
                    Type = enumName
                };
            };

            modelBuilder.Entity<TEntity>().HasData(enumData);
        }
    }
}
