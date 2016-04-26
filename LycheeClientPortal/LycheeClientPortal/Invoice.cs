using System;
using System.Collections.Generic;
using System.Text;

namespace LycheeClientPortal
{
    public enum InvoiceStateValue
    {
        paid,
        late,
        open,
        closed
    };
    public class Invoice
    {
        public string InvoiceNumber { get; set; }
        public decimal InvoiceAmount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public InvoiceStateValue InvoiceStateValue { get; set; }
        public Invoice(string invoiceNumber, decimal invoiceAmount, InvoiceStateValue invoiceStateValue, DateTime invoiceDate, DateTime? paidDate = null)
        {
            this.InvoiceNumber = invoiceNumber;
            this.InvoiceAmount = invoiceAmount;
            this.InvoiceStateValue = invoiceStateValue;
            this.InvoiceDate = invoiceDate;
            this.PaidDate = paidDate;

        }
        public InvoiceStateValue refreshInvoiceState()
        {
            if (InvoiceStateValue == InvoiceStateValue.open)
            {
                if (DateTime.Now.ToLocalTime() > InvoiceDate)
                {
                    InvoiceStateValue = InvoiceStateValue.late;
                }
            }
            else if (InvoiceStateValue == InvoiceStateValue.late)
            {
                if (DateTime.Now.ToLocalTime() <= InvoiceDate)
                {
                    InvoiceStateValue = InvoiceStateValue.open;
                }
            }
            else if (InvoiceStateValue == InvoiceStateValue.paid)
            {
                if (DateTime.Now.ToLocalTime() > InvoiceDate)
                {
                    InvoiceStateValue = InvoiceStateValue.closed;
                }
            }
            else if (InvoiceStateValue == InvoiceStateValue.closed)
            {
                if (DateTime.Now.ToLocalTime() <= InvoiceDate)
                {
                    InvoiceStateValue = InvoiceStateValue.paid;
                }
            }
            return InvoiceStateValue;
        }
    }
}