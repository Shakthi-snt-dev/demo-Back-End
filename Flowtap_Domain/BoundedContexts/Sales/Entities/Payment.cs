using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flowtap_Domain.SharedKernel.Enums;

namespace Flowtap_Domain.BoundedContexts.Sales.Entities;

public class Payment
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid InvoiceId { get; set; }

    public Invoice? Invoice { get; set; }

    public PaymentMethod Method { get; set; } = PaymentMethod.Cash;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [MaxLength(500)]
    public string? TransactionReference { get; set; } // payment gateway txn id, etc.

    public DateTime PaidAt { get; set; } = DateTime.UtcNow;

    // ===========================
    // DOMAIN METHODS - Business Logic
    // ===========================

    /// <summary>
    /// Updates the payment method
    /// </summary>
    public void UpdatePaymentMethod(PaymentMethod method)
    {
        Method = method;
    }

    /// <summary>
    /// Updates the payment amount
    /// </summary>
    public void UpdateAmount(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Payment amount must be greater than zero", nameof(amount));

        Amount = amount;
    }

    /// <summary>
    /// Updates the transaction reference
    /// </summary>
    public void UpdateTransactionReference(string? transactionReference)
    {
        TransactionReference = transactionReference;
    }

    /// <summary>
    /// Updates the payment date
    /// </summary>
    public void UpdatePaidAt(DateTime paidAt)
    {
        if (paidAt > DateTime.UtcNow)
            throw new ArgumentException("Payment date cannot be in the future", nameof(paidAt));

        PaidAt = paidAt;
    }

    /// <summary>
    /// Updates all payment details
    /// </summary>
    public void UpdatePayment(PaymentMethod method, decimal amount, string? transactionReference, DateTime paidAt)
    {
        UpdatePaymentMethod(method);
        UpdateAmount(amount);
        UpdateTransactionReference(transactionReference);
        UpdatePaidAt(paidAt);
    }
}

