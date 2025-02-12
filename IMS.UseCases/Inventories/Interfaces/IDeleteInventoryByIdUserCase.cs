namespace IMS.UseCases.Inventories.Interfaces
{
    public interface IDeleteInventoryByIdUserCase
    {
        Task ExecuteAsync(int id);
    }
}