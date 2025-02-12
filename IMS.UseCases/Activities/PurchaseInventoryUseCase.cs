﻿using IMS.CoreBusiness;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Activities
{
    public class PurchaseInventoryUseCase : IPurchaseInventoryUseCase
    {
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public PurchaseInventoryUseCase(
            IInventoryTransactionRepository inventoryTransactionRepository,
            IInventoryRepository inventoryRepository
        )
        {
            _inventoryTransactionRepository = inventoryTransactionRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task ExecuteAsync(string orderNumber, Inventory inventory, int quantity, string doneBy)
        {
            // insert a record in the transaction table
            _inventoryTransactionRepository.PurchaseAsync(orderNumber, inventory, quantity, doneBy, inventory.Price);

            // increase the quantity
            inventory.Quantity += quantity;
            await _inventoryRepository.UpdateAsync(inventory);
        }
    }
}
