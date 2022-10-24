using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    #region Formula

    public class LibraryCommissionFormulaDetails
    {
        public string CommissionStructureName { get; set; }
        public string CommissionStructureCode { get; set; }
        public int FormulaID { get; set; }
        public string FormulaName { get; set; }
        public string FormulaCode { get; set; }
        public List<LibraryCommissionRateType> Rates { get; set; } = new List<LibraryCommissionRateType>();
    }

    public class LibraryCommissionRateType
    {
        public string RateName { get; set; }
        public string RateCode { get; set; }
        public decimal RequiredValueFrom { get; set; }
        public decimal? RequiredValueTo { get; set; }
        public decimal? PayoutAmount { get; set; }
        public decimal? PayoutPercentage { get; set; }
    }

    #endregion

    #region Commission Balance & Journal 

    public class CommissionBalanceAndJournalDetails
    {
        public int OrgID { get; set; }
        public bool IsPlus { get; set; }
        public decimal Amount { get; set; }
        public DateTime CurrentDT { get; set; }
        public int AccessID { get; set; }

        // Below is not compulsory, just put in necessary ID
        public int? CommissionID { get; set; }
        public int? PayoutID { get; set; }
    }

    #endregion
}
