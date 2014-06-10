using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using AutoServiceManager.Common.Model;

namespace AutoServiceManager.Common.Invoice
{
    public class InvoiceFactory
    {
        public static Invoice GetInvoiceForFaultId(long id)
        {
            return new Invoice();
        }
    }
}