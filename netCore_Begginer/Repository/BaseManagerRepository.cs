using AutoMapper;
using Microsoft.EntityFrameworkCore;
using netCore_Begginer.Interfaces;
using netCore_Begginer.Mappings;
using netCore_Begginer.Models;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace netCore_Begginer.Repository
{
    public class BaseManagerRepository<T,Type> : IBaseManager<T,Type> where T:class
    {
        private readonly ProductDbContext ProductDbContext;
        private DbSet<T> Set {  get; set; }
        private readonly IMapper Mapper;

        public BaseManagerRepository(ProductDbContext productDb, IMapper mapper)
        {
            ProductDbContext = productDb;
            Set = ProductDbContext.Set<T>();
            Mapper = mapper;
        }

        public async Task AddTheData(T data)
        {
            try
            {
                Set.Add(data);
                await ProductDbContext.SaveChangesAsync();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EditTheData(T data, Type id)
        {
            try
            {
                var existingData = await ProductDbContext.Set<T>().FindAsync(id);
                var list = data;
                if (existingData == null)
                {
                    throw new Exception($"No data found with ID {id}");
                }

               Mapper.Map(data,existingData);

                await ProductDbContext.SaveChangesAsync();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteTheData(Type id)
        {
            var deleteItem = await Set.FindAsync(id);
            if (deleteItem != null)
            {
                try
                {
                    Set.Remove(deleteItem);
                    await ProductDbContext.SaveChangesAsync();
                }catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<T> GetTheData(Type id)
        {
            if (id != null) {
                try
                {
                    return await Set.FindAsync(id);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return null;
        }

        public async Task<List<T>> GetAllTheData(string propertyName,Type id)
        {
            var list = await ProductDbContext.Set<T>().Where(x => EF.Property<Type>(x, propertyName).Equals(id)).ToListAsync();

            if(list != null)
            {
                return list;
            }

            return null;
        }
    }
}
