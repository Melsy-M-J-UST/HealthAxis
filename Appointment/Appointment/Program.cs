 using System;

namespace Appointment;


public enum TimeSlot
{
    NineToEleven,
    ElevenToOne,
    TwoToFour,
    FourToSix
}

public enum Status
{
    Pending,
    Confirmed,
    Cancelled,
    Completed
}

public class Appointment
{
    public string appointment_id { get; set; }
    public Patient patient{ get; set; } = null!;
    public Doctor doctor { get; set; } = null!;
    public DateTime ScheduledDate{get; set; }
    public TimeSlot slot;
    public Status status;
    public string CancellationReason;

    public Appointment(string appointment_id, DateTime scheduledDate, TimeSlot slot)
    {
        this.appointment_id = appointment_id;
        ScheduledDate = scheduledDate;
        this.slot = slot;
        this.status = Status.Pending;
        CancellationReason = "NIL";
    }

    public void Confirm()
    {
        if(status == Status.Cancelled)
        {
            throw new InvalidOperationException("Cancelled appointments cannot be confirmed.")
        }
        else if(status == Status.Completed)
        {
            throw new InvalidOperationException("Appointment already Completed. Cannot be confirmed.");
        }
        status = Status.Confirmed;
        
    }

    public void Cancel(string reason)
    {
        if(status == Status.Completed)
        {
            throw new InvalidOperationException("Completed appointments cannot be cancelled");
        }
        status = Status.Cancelled;
        CancellationReason = reason;
    }

    public void Complete()
    {
        if(status == Status.Cancelled)
        {
            throw new InvalidOperationException("Cancelled appointments cannot be completed");
        }
        status = Status.Completed;
    }

    public string GetDetails()
    {
        return $"Appointment ID: {appointment_id}  Scheduled Date: {ScheduledDate}  Time Slot: {slot}  Status: {status}   Cancellation reason(if any): {CancellationReason}";
    }

}
