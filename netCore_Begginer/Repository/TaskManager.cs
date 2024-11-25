using Microsoft.EntityFrameworkCore;
using netCore_Begginer.Interfaces;
using netCore_Begginer.Models;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace netCore_Begginer.Repository
{
    public class TaskManager<T,Type> : ITaskManager<T,Type> where T:class
    {
        private readonly ProductDbContext _productDb;
        private DbSet<T> set {  get; set; }

        public TaskManager(ProductDbContext productDb)
        {
            _productDb = productDb;
            set = _productDb.Set<T>();
        }

        public async Task AddTheData(T data)
        {
            try
            {
                set.Add(data);
                await _productDb.SaveChangesAsync();
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task EditTheData(T data, Type id)
        {
            try
            {
                var existingData = await _productDb.Set<T>().FindAsync(id);
                if (existingData == null)
                {
                    throw new Exception($"No data found with ID {id}");
                }
                //reflection 
                foreach(var property in typeof(T).GetProperties())
                {
                    var value = property.GetValue(data);
                    if (value!=null)
                    {
                        property.SetValue(existingData, value);
                    }
                }

                _productDb.Entry(existingData).State = EntityState.Modified;

                await _productDb.SaveChangesAsync();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteTheData(Type id)
        {
            var deleteItem = await set.FindAsync(id);
            if (deleteItem != null)
            {
                try
                {
                    set.Remove(deleteItem);
                    await _productDb.SaveChangesAsync();
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
                    return await set.FindAsync(id);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return null;
        }

        public async Task<List<T>> GetAllTheData(Type email)
        {
            var list = await _productDb.Set<T>().Where(x => EF.Property<Type>(x, "Email").Equals(email)).ToListAsync();

            if(list != null)
            {
                return list;
            }

            return null;
        }
    }
}
