// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentNew.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the DocumentNew type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace EDMs.Data.Entities
{
    /// <summary>
    /// The document new.
    /// </summary>
    public class ProcessReport
    {
        public DateTime WeekDate { get; set; }
        public string WeekName { get; set; }
        public double Planed { get; set; }
        public double Actual { get; set; }
        public double RecoveryPlan { get; set; }

    }
}
