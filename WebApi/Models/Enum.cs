using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public enum ValidationMessage
    {
        [Description("Request is empty.")]
        RequestIsNull,

        [Description("OrgRefNo not valid.")]
        InvalidOrgGuid,

        [Description("FormulaCode not valid.")]
        InvalidFormulaCode,

        [Description("TransactionFigure not valid.")]
        InvalidTransactionFigure,

        [Description("Formula/Rate not configured.")]
        NoRateConfigure
    }

    public enum ApiVersion
    {
        [Description("1.0.0")]
        One,

        [Description("2.0.0")]
        Two
    }

    public enum ApiResponseMessage
    {
        [Description("Yay! Api call is success.")]
        Success,

        [Description("Oops! Value pass in is incorrect.")]
        BadRequest,

        [Description("API under construction. Please come back later.")]
        NotImplemented,

        [Description("API under maintenance. Please come back later.")]
        ServiceUnavailable,

        [Description("Oops! Something went wrong. Please contact us.")]
        InternalServerError
    }

    public enum FormulaCode
    {
        [Description("Fixed Rate Commission By Amount")]
        F4,

        [Description("Fixed Rate Commission By Percentage")]
        F3,

        [Description("Tiered Rate Commission By Amount")]
        F1,

        [Description("Tiered Rate Commission By Percentage")]
        F5,

        [Description("Tiered Rate Commission By Distance Amount")]
        F6,

        [Description("Tiered Rate Commission By Distance Percentage")]
        F2,
    }
}