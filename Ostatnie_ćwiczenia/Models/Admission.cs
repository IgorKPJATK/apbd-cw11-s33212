using System;
using System.Collections.Generic;

namespace Ostatnie_ćwiczenia.Models;

public partial class Admission
{
    public int Id { get; set; }

    public DateTime AdmissionDate { get; set; }

    public DateTime? DischargeDate { get; set; }

    public string PatientPesel { get; set; } = null!;

    public int WardId { get; set; }

    public virtual Patient PatientPeselNavigation { get; set; } = null!;

    public virtual Ward Ward { get; set; } = null!;
}
