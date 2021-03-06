﻿using System;
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
            public float price;
        }

        public string itemName;
        public int quantity;
        public float price;
        public int microsCheckItemID;
        public float discount = 0;
        public float amountPaid = 0;
        public List<Condement> condements = new List<Condement>();

        

  
    }

    public List<OrderItem> orderItems = new List<OrderItem>();
    public float tax;
    public float discount;
    public float totalDue;
    public float amountPaid;
    public float change;

    public void SetTotalAndPaid(float total, float paid)
    {
        this.totalDue = total;
        this.amountPaid = paid;

        if (this.amountPaid > this.totalDue)
            this.change = this.amountPaid - this.totalDue;
    }


    public void addItem(String itemName, int quantity, float price, int microsCheckItemID)
    {
        if (this.hasItem(microsCheckItemID))
            return;

        OrderItem item = new OrderItem();
        item.itemName = itemName;
        item.quantity = quantity;
        item.price = price;
        item.microsCheckItemID = microsCheckItemID;
        orderItems.Add(item);
    }

    public void voidItem(int itemid)
    {
        orderItems.RemoveAt(orderItems.Count - 1);
    }

    public bool hasCondement(int microsCheckItemID)
    {
        foreach (OrderItem item in orderItems)
        {
            foreach(OrderItem.Condement c in item.condements)
            {
                if (c.microsCheckItemID == microsCheckItemID)
                    return true;
            }
        }

        return false;
    }

    //adds to last menu item ordered
    public void addCondement(String name, float price, int microsCheckItemID)
    {
        if (this.hasCondement(microsCheckItemID))
            return;

        OrderItem.Condement condement = new OrderItem.Condement();
        condement.description = name;
        condement.price = price;
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
        {
            subtotal += item.price;

            foreach (OrderItem.Condement c in item.condements)
                subtotal += c.price;
        }
        return subtotal;
    }
}
