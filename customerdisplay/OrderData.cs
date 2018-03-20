using System;
using System.Collections.Generic;

public class OrderData
{
    public class OrderItem
    {
        public enum CondementStyle
        {
            Excluded = 0,
            Added = 1,
            Extra = 2
        }

        public class Condement
        {
            public string description;
            public CondementStyle style;
            public int microsCheckItemID;
        }

        public string itemName;
        public int quantity;
        public float price;
        public int microsCheckItemID;
        public float discount = 0;
        public List<Condement> condements = new List<Condement>();

        

  
    }

    public List<OrderItem> orderItems = new List<OrderItem>();
    public float tax;
    public float discount;
    public float amountPaid;

    public void addItem(String itemName, int quantity, float price, int microsCheckItemID)
    {
        OrderItem item = new OrderItem();
        item.itemName = itemName;
        item.quantity = quantity;
        item.price = price;
        item.microsCheckItemID = microsCheckItemID;
        orderItems.Add(item);
    }

    //adds to last menu item ordered
    public void addCondement(String name, float price, int microsCheckItemID)
    {
        OrderItem.Condement condement = new OrderItem.Condement();
        condement.description = name;
        condement.microsCheckItemID = microsCheckItemID;
        orderItems[orderItems.Count - 1].condements.Add(condement);
    }

    public void removeCondement(int microsCheckItemID)
    {
        foreach (OrderItem.Condement c in orderItems[orderItems.Count - 1].condements)
        {
            if (c.microsCheckItemID == microsCheckItemID)
            {
                orderItems[orderItems.Count - 1].condements.Remove(c);
                return;
            }
                
        }
    }

    public bool hasItem(int microsCheckItemID)
    {
        foreach(OrderItem item in orderItems)
        {
            if (item.microsCheckItemID == microsCheckItemID)
                return true;
        }

        return false;
    }

    public float getSubtotal()
    {
        float subtotal = 0;
        foreach (OrderItem item in orderItems)
            subtotal += item.price;

        return subtotal;
    }
}
