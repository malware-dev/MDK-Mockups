using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VRage;
using VRage.Game.ModAPI.Ingame;

namespace IngameScript.Mockups.Base
{
    public class MockInventory : IMyInventory
    {
        protected static readonly MyFixedPoint Value0 = new MyFixedPoint { RawValue = 0 };
        protected static readonly MyFixedPoint Value1 = new MyFixedPoint { RawValue = 1 };

        protected virtual IEnumerable<MyItemType> AcceptedItems { get; } = new List<MyItemType>();

        protected List<MyInventoryItem> Inventory { get; } = new List<MyInventoryItem>();
        protected MyFixedPoint _volume = new MyFixedPoint { RawValue = 0 };

        public IMyEntity Owner { get; set; }

        public bool IsFull => MaxVolume == CurrentVolume;

        public MyFixedPoint CurrentMass { get; set; } = 0;

        public virtual MyFixedPoint MaxVolume { get; } = new MyFixedPoint() { RawValue = 0 };

        public MyFixedPoint CurrentVolume
        {
            get { return _volume; }
            set
            {
                if (value > MaxVolume)
                    throw new ArgumentOutOfRangeException();

                _volume = value;
            }
        }

        public int ItemCount { get; protected set; } = 0;

        public virtual bool CanItemsBeAdded(MyFixedPoint amount, MyItemType itemType)
        {
            throw new NotImplementedException("Data table for item volumes is not available.");
        }

        public bool CanTransferItemTo(IMyInventory otherInventory, MyItemType itemType)
        {
            if (!ContainItems(Value1, itemType))
                return false;

            if (!IsConnectedTo(otherInventory))
                return false;

            return otherInventory.CanItemsBeAdded(Value1, itemType);
        }

        public bool ContainItems(MyFixedPoint amount, MyItemType itemType)
            => Inventory.Any(i => i.Type == itemType && i.Amount.RawValue > 0);

        public MyInventoryItem? FindItem(MyItemType itemType) 
            => Inventory.FirstOrDefault(i => i.Type == itemType);

        public void GetAcceptedItems(List<MyItemType> itemsTypes, Func<MyItemType, bool> filter = null)
        {
            Debug.Assert(itemsTypes != null, $"{itemsTypes} must not be null.");

            itemsTypes.Clear();
            if (filter == null)
                filter = i => true;

            itemsTypes.AddRange(AcceptedItems.Where(filter));
        }

        public MyFixedPoint GetItemAmount(MyItemType itemType)
            => new MyFixedPoint { RawValue = Inventory.Where(i => i.Type == itemType)
                .DefaultIfEmpty(new MyInventoryItem(itemType, 0, Value0))
                .Sum(i => i.Amount.RawValue) };

        public MyInventoryItem? GetItemAt(int index) 
            => Inventory.ElementAtOrDefault(index);

        public MyInventoryItem? GetItemByID(uint id) 
            => Inventory.SingleOrDefault(i => i.ItemId == id);

        public void GetItems(List<MyInventoryItem> items, Func<MyInventoryItem, bool> filter = null)
        {
            Debug.Assert(items != null, $"{items} must not be null.");

            items.Clear();
            if (filter == null)
                filter = i => true;

            items.AddRange(Inventory.Where(filter));
        }

        public bool IsItemAt(int position)
            => Inventory.IsValidIndex(position);

        public bool IsConnectedTo(IMyInventory otherInventory)
        {
            throw new NotImplementedException();
        }

        public bool TransferItemFrom(IMyInventory sourceInventory, MyInventoryItem item, MyFixedPoint? amount = null)
        {
            throw new NotImplementedException();
        }

        public bool TransferItemFrom(IMyInventory sourceInventory, int sourceItemIndex, int? targetItemIndex = null, bool? stackIfPossible = null, MyFixedPoint? amount = null)
        {
            throw new NotImplementedException();
        }

        public bool TransferItemTo(IMyInventory dstInventory, MyInventoryItem item, MyFixedPoint? amount = null)
        {
            throw new NotImplementedException();
        }

        public bool TransferItemTo(IMyInventory dst, int sourceItemIndex, int? targetItemIndex = null, bool? stackIfPossible = null, MyFixedPoint? amount = null)
        {
            throw new NotImplementedException();
        }
    }
}
