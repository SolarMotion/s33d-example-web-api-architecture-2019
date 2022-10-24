using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    #region List

    public class CommissionListRequest : ApiBaseRequest
    {
    }

    public class CommissionListResponse
    {
        [JsonProperty(PropertyName = "formulas")]
        public List<CommissionFormula> Formulas { get; set; } = new List<CommissionFormula>();
    }

    public class CommissionFormula
    {
        [JsonProperty(PropertyName = "commission_structure_name")]
        public string CommissionStructureName { get; set; }

        [JsonProperty(PropertyName = "formula_name")]
        public string FormulaName { get; set; }

        [JsonProperty(PropertyName = "formula_code")]
        public string FormulaCode { get; set; }

        [JsonProperty(PropertyName = "rates")]
        public List<CommissionRateType> Rates { get; set; } = new List<CommissionRateType>();
    }

    public class CommissionRateType
    {
        [JsonProperty(PropertyName = "rate")]
        public string Rate { get; set; }

        [JsonProperty(PropertyName = "value_from")]
        public string RequiredValueFrom { get; set; }

        [JsonProperty(PropertyName = "value_to")]
        public string RequiredValueTo { get; set; }

        [JsonProperty(PropertyName = "payout")]
        public string Payout { get; set; }
    }

    #endregion

    #region Fixed Rate Commission By Amount

    public class FixedRateCommissionByAmountRequest : ApiBaseRequest
    {
        public decimal TransactionFigure { get; set; }
    }

    public class FixedRateCommissionByAmountResponse
    {
        public Guid CommissionRefNo { get; set; }
        public DateTime TransactionDT { get; set; }
        public decimal TransactionFigure { get; set; }
        public decimal CommisionAmount { get; set; }
    }

    #endregion

    #region Fixed Rate Commission By Percentage

    public class FixedRateCommissionByPercentageRequest : FixedRateCommissionByAmountRequest
    {
    }

    public class FixedRateCommissionByPercentageResponse : FixedRateCommissionByAmountResponse
    {
    }

    #endregion

    #region Tiered Rate Commission By Amount

    public class TieredRateCommissionByAmountRequest : FixedRateCommissionByAmountRequest
    {
    }

    public class TieredRateCommissionByAmountResponse : FixedRateCommissionByAmountResponse
    {
    }

    #endregion

    #region Tiered Rate Commission By Percentage

    public class TieredRateCommissionByPercentageRequest : FixedRateCommissionByAmountRequest
    {
    }

    public class TieredRateCommissionByPercentageResponse : FixedRateCommissionByAmountResponse
    {
    }

    #endregion

    #region Tiered Rate Commission By Distance Amount

    public class TieredRateCommissionByDistanceAmountRequest : FixedRateCommissionByAmountRequest
    {
    }

    public class TieredRateCommissionByDistanceAmountResponse : FixedRateCommissionByAmountResponse
    {
    }

    #endregion

    #region Tiered Rate Commission By Distance Percentage

    public class TieredRateCommissionByDistancePercentageRequest : FixedRateCommissionByAmountRequest
    {
    }

    public class TieredRateCommissionByDistancePercentageResponse : FixedRateCommissionByAmountResponse
    {
    }

    #endregion

    #region Payout

    public class CommissionPayoutRequest : ApiBaseRequest
    {
    }

    public class CommissionPayoutResponse
    {
    }

    #endregion
}